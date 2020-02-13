<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteNode"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="名前" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="メモ">
        <el-input v-model="form.memo" type="textarea" />
      </el-form-item>
      <el-form-item label="パーティション">
        <el-input v-model="form.partition" />
      </el-form-item>
      <el-form-item label="アクセスレベル">
        <el-radio-group v-model="form.accessLevel" style="width:100%;">
          <el-radio-button :label="0">Disabled</el-radio-button>
          <el-radio-button :label="1">Private</el-radio-button>
          <el-radio-button :label="2">Public</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <transition name="el-fade-in-linear">
        <el-transfer
          v-if="form.accessLevel === 1"
          v-model="form.selectedTenants"
          :data="form.tenants"
          :transfer-titles="form.transferTitles"
        ></el-transfer>
      </transition>
      <el-form-item label="TensorBoard">
        <el-switch
          v-model="form.tensorBoardEnabled"
          style="width: 100%;"
          inactive-text="実行しない"
          active-text="実行する"
        />
      </el-form-item>
      <el-form-item label="Notebook">
        <el-switch
          v-model="form.notebookEnabled"
          style="width: 100%;"
          inactive-text="実行しない"
          active-text="実行する"
        />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('node')
const formRules = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  name: 'NodeEdit',
  components: {
    KqiDialog,
    KqiDisplayError,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      form: {
        name: null,
        memo: null,
        partition: null,
        accessLevel: 2,
        selectedTenants: [], // Selected tenants which can access this node.
        tenants: [], // Tenants to display on a transfer component.
        transferTitles: ['アクセス拒否', 'アクセス許可'], // The title of the transfer component.
        tensorBoardEnabled: null,
        notebookEnabled: null,
      },
      title: '',
      error: null,
      rules: {
        name: [formRules],
      },
    }
  },
  computed: {
    ...mapGetters(['detail', 'tenant']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'ノード登録'
    } else {
      this.title = 'ノード編集'
      try {
        await this.fetchDetail()
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        this.form.partition = this.detail.partition
        this.form.accessLevel = this.detail.accessLevel
        this.form.selectedTenants = this.detail.assignedTenants
          ? this.detail.assignedTenants.map(t => {
              return t.id
            })
          : []
        this.form.tensorBoardEnabled = this.detail.tensorBoardEnabled
        this.form.notebookEnabled = this.detail.notebookEnabled

        // retrieve tenant to set up a transfer list.
        await this.fetchTenant()
        let allTenants = this.tenant.data
        allTenants.forEach(t => {
          if (this.form.selectedTenants.every(s => s.id !== t.id)) {
            this.form.tenants.push({
              key: t.id,
              label: t.displayName,
            })
          }
        })
      } catch (e) {
        this.error = e
      }
    }
  },
  methods: {
    ...mapActions(['fetchDetail', 'fetchTenant', 'post', 'put', 'delete']),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              model: {
                name: this.form.name,
                memo: this.form.memo,
                partition: this.form.partition,
                accessLevel: this.form.accessLevel,
                assignedTenantIds:
                  this.form.accessLevel === 1 ? this.form.selectedTenants : [],
                tensorBoardEnabled: this.form.tensorBoardEnabled,
                notebookEnabled: this.form.notebookEnabled,
              },
            }
            if (this.id === null) {
              await this.post(params)
            } else {
              await this.put(params)
            }
            this.emitDone()
            this.error = undefined
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    async deleteNode() {
      try {
        await this.delete()
        this.error = null
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },
    emitDone() {
      this.$emit('done')
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.footer {
  padding-top: 40px;
}
</style>
