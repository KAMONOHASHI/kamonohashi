<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    :disabled-params="disabledParams"
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
      <el-form-item label="種別">
        <el-select
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
    }
  },
  computed: {
    ...mapGetters(['tenantRoleDetail']),
    disabledParams() {
      return {
        deleteButton: this.isNotEditable,
        submitButton: this.isNotEditable,
      }
    },
  },
  async created() {
    if (this.id === null) {
      this.title = 'テナントロール作成'
      this.form.isCostomRole = true
    } else {
      this.title = 'テナントロール編集'
      try {
        await this.fetchTenantRoleDetail(this.id)
        this.form.name = this.tenantRoleDetail.name
        this.form.displayName = this.tenantRoleDetail.displayName
        this.form.sortOrder = this.tenantRoleDetail.sortOrder
        this.isNotEditable = this.tenantRoleDetail.isNotEditable
        if (this.tenantRoleDetail.tenantId) {
          // テナント(カスタム)
          this.form.isCostomRole = true
        } else {
          this.title = 'テナントロール詳細'
          // テナント(共通)
          this.form.isCostomRole = false
          // テナント(共通)は編集、削除ともにできない。
          this.isNotEditable = true
        }
        this.error = null
      } catch (e) {
        this.error = e
      }
    }
  },

  methods: {
    ...mapActions([
      'fetchTenantRoleDetail',
      'postTenantRole',
      'putTenantRole',
      'deleteTenantRole',
    ]),
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
              await this.postTenantRole(params)
            } else {
              await this.putTenantRole({ id: this.id, params: params })
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
        await this.deleteTenantRole(this.id)
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
