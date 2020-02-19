<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    :disabled="isNotEditable"
    submit-text="作成"
    @submit="submit"
    @delete="deleteRole"
    @close="emitCancel"
  >
    <el-form ref="updateForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="ロール名" prop="name">
        <el-input v-model="form.name" :disabled="isNotEditable" />
      </el-form-item>
      <el-form-item label="表示名" prop="displayName">
        <el-input v-model="form.displayName" :disabled="isNotEditable" />
      </el-form-item>
      <el-form-item label="種別" prop="isSystemRole">
        <el-input
          v-if="form.tenantName"
          v-model="form.tenantName"
          :disabled="isNotEditable"
        />
        <el-select
          v-else
          v-model="form.isSystemRole"
          placeholder="Select"
          style="width:100%;"
          :clearable="true"
          :disabled="id !== null"
        >
          <el-option
            v-for="item in form.roleTypes"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          >
          </el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="ソート順" prop="sortOrder">
        <br />
        <el-input-number
          v-model="form.sortOrder"
          controls-position="right"
          style="vertical-align: middle;"
        />
        並び順。小さいほど前に表示される。一意性は不要。
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('role')
const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
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
        displayName: null,
        isSystemRole: false,
        sortOrder: 0,
        tenantName: null,
        roleTypes: [
          { label: 'テナント(共通)', value: false },
          { label: 'システム', value: true },
        ],
      },
      title: '',
      error: null,
      isNotEditable: false,
      rules: {
        name: [formRule],
        displayName: [formRule],
        isSystemRole: [formRule],
        sortOrder: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters(['detail']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'ロール作成'
    } else {
      this.title = 'ロール編集'
      try {
        await this.fetchDetail()
        this.form.name = this.detail.name
        this.form.displayName = this.detail.displayName
        this.form.isSystemRole = this.detail.isSystemRole
        this.form.sortOrder = this.detail.sortOrder
        this.error = null
        this.isNotEditable = this.detail.isNotEditable
        if (this.detail.tenantName) {
          this.tenantName = `テナント(カスタム) / ${this.detail.tenantName}`
        }
      } catch (e) {
        this.error = e
      }
    }
  },

  methods: {
    ...mapActions(['fetchDetail', 'post', 'put', 'delete']),
    async submit() {
      let form = this.$refs.updateForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              model: {
                name: this.form.name,
                displayName: this.form.displayName,
                isSystemRole: this.form.isSystemRole,
                sortOrder: this.form.sortOrder,
              },
            }
            if (this.id === null) {
              await this.post(params)
            } else {
              await this.put(params)
            }
            this.error = null
            this.emitDone()
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteRole() {
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

<style lang="scss" scoped></style>
