<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    :disabled="form.tenantId"
    :delete-disapproval="deleteDisapproval"
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
        <el-input v-if="form.tenantName" v-model="form.tenantName" disabled />
        <el-select
          v-else
          v-model="form.isCostomRole"
          placeholder="Select"
          style="width: 100%;"
          disabled
        >
          <el-option
            v-for="item in form.roleTypes"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="ソート順" prop="sortOrder">
        <br />
        <el-input-number
          v-model="form.sortOrder"
          controls-position="right"
          style="vertical-align: middle;"
          :min="0"
          :disabled="isNotEditable"
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
        isCostomRole: false,
        sortOrder: 0,
        tenantName: null,
        roleTypes: [
          { label: 'テナント(共通)', value: false },
          { label: 'テナント(カスタム)', value: true },
        ],
      },
      title: '',
      error: null,
      isNotEditable: false,
      rules: {
        name: [formRule],
        displayName: [formRule],
        sortOrder: [formRule],
      },
      deleteDisapproval: false,
    }
  },
  computed: {
    ...mapGetters(['detail']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'ロール作成'
      this.form.isCostomRole = true
    } else {
      this.title = 'ロール編集'
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        this.form.displayName = this.detail.displayName
        this.form.sortOrder = this.detail.sortOrder
        this.error = null
        this.isNotEditable = this.detail.isNotEditable
        if (this.detail.tenantName) {
          this.form.tenantName = `テナント(カスタム)`
          this.form.isCostomRole = true
        } else {
          this.title = 'ロール詳細'
          this.deleteDisapproval = true
          this.isNotEditable = true
        }
      } catch (e) {
        this.error = e
      }
    }
  },

  methods: {
    ...mapActions(['fetchDetail', 'tenantPost', 'tenantPut', 'tenantDelete']),
    async submit() {
      let form = this.$refs.updateForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              displayName: this.form.displayName,
              sortOrder: this.form.sortOrder,
            }
            if (this.id === null) {
              await this.tenantPost(params)
            } else {
              await this.tenantPut({ id: this.id, params: params })
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
        await this.tenantDelete(this.id)
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
