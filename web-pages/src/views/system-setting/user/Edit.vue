<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    submit-text="作成"
    :delete-button-params="deleteButtonParams"
    @submit="submit"
    @delete="deleteUser"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />

      <span v-if="form.serviceType === 1">
        <el-form-item v-if="isCreateDialog" label="ユーザ名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
        <kqi-display-text-form v-else label="ユーザ名" :value="form.name" />
        <el-form-item label="ユーザ表示名" prop="displayName">
          <el-input v-model="form.displayName" />
        </el-form-item>
        <kqi-display-text-form
          v-if="id !== null"
          label="認証タイプ"
          :value="form.displayServiceType"
        />
        <el-form-item :label="passwordLabel" prop="password">
          <el-input v-model="form.password[0]" type="password" />
          パスワード（再入力）
          <br />
          <el-input v-model="form.password[1]" type="password" />
        </el-form-item>
      </span>
      <span v-else-if="form.serviceType === 2">
        <kqi-display-text-form label="ユーザ名" :value="form.name" />
        <kqi-display-text-form label="ユーザ表示名" :value="form.displayName" />
        <kqi-display-text-form
          label="認証タイプ"
          :value="form.displayServiceType"
        />
      </span>
      <span v-else>
        認証タイプ：不明
      </span>

      <el-form-item label="システムロール" prop="roleIds">
        <kqi-role-selector
          v-model="form.selectedSystemRoleIds"
          :roles="roles"
          show-system-role
        />
      </el-form-item>

      <tenant-role-selector
        ref="tenantsForm"
        v-model="form.tenants"
        :tenants="tenants"
        :roles="roles"
        :not-origin-tenants="notOriginTenants"
        @default="setNotOriginTenants"
      />
    </el-form>
  </kqi-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDialog from '@/components/KqiDialog.vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiRoleSelector from '@/components/selector/KqiRoleSelector.vue'
import TenantRoleSelector from '@/views/system-setting/user/TenantRoleSelector.vue'
import { mapGetters, mapActions } from 'vuex'
import * as gen from '@/api/api.generate'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

interface DataType {
  form: {
    name: string
    displayName: string
    serviceType: number
    displayServiceType: string | number
    password: [string, string]
    selectedSystemRoleIds: Array<number>
    tenants: {
      selectedTenantIds: Array<number>
      selectedTenants: Array<{
        tenantId: number
        tenantName: string
        selectedRoleIds: Array<number>
        default: boolean
      }>
      selectedNotOriginTenants?: Array<number>
    }
  }
  rules: {
    name: Array<typeof formRule>
    password: [{ required: boolean; trigger: string; validator: Function }]
    tenants: [{ required: boolean; trigger: string; validator: Function }]
  }
  error: null | Error
  title: null | string
  deleteButtonParams:
    | {}
    | {
        isDanger: boolean
        warningText: string
        confirmText: string
      }
  passwordLabel: string
  isCreateDialog: boolean
  notOriginTenants: {
    selectedTenantIds: Array<number>
    selectedTenants: Array<{
      tenantId: number
      tenantName: string
      selectedRoleIds: Array<number>
      default: boolean
    }>
  }
}

export default Vue.extend({
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiRoleSelector,
    TenantRoleSelector,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    //@ts-ignore
    let passwordValidator = (rule, value, callback) => {
      // 作成時はパスワード入力必須
      //@ts-ignore
      if (this.isCreateDialog && !value[0] && !value[1]) {
        callback(new Error('必須項目です'))
      }
      // 編集時に両方空の場合は、パスワードは未編集とみなして続行
      //@ts-ignore
      if (this.isEditDialog && !value[0] && !value[1]) {
        callback()
      }
      if (!(value[0] === value[1])) {
        callback(new Error('同一のパスワードを入力してください'))
      }
      callback()
    }
    //@ts-ignore
    let tenantsValidator = (rule, value, callback) => {
      //@ts-ignore
      if (this.form.tenants.selectedTenantIds.length === 0) {
        callback(new Error('必須項目です'))
      } else {
        //@ts-ignore
        this.form.tenants.selectedTenants.forEach(
          (tenant: { selectedTenantIds: []; selectedTenants: [] }) => {
            //@ts-ignore
            if (tenant.selectedRoleIds.length === 0) {
              callback(new Error('ロールが選択されていないテナントがあります'))
            }
          },
        )
      }
      callback()
    }
    return {
      form: {
        name: '',
        displayName: '',
        serviceType: 1,
        displayServiceType: '',
        password: ['', ''],
        selectedSystemRoleIds: [],
        tenants: {
          selectedTenantIds: [],
          selectedTenants: [],
        },
      },
      rules: {
        name: [formRule],
        password: [
          { required: true, trigger: 'blur', validator: passwordValidator },
        ],
        tenants: [
          { required: true, trigger: 'blur', validator: tenantsValidator },
        ],
      },
      error: null,
      title: null,
      deleteButtonParams: {},
      passwordLabel: '',
      isCreateDialog: false,
      notOriginTenants: {
        selectedTenantIds: [],
        selectedTenants: [],
      },
    }
  },
  computed: {
    ...mapGetters({
      //@ts-ignore
      detail: ['user/detail'],
      tenants: ['tenant/tenants'],
      roles: ['role/roles'],
    }),
    isEditDialog(): boolean {
      return !this.isCreateDialog
    },
  },
  async created() {
    if (this.id === null) {
      this.isCreateDialog = true
    }
    await this['role/fetchRoles']()
    await this['tenant/fetchTenants']()
    if (this.isCreateDialog) {
      this.title = 'ユーザ作成'
      this.passwordLabel = 'パスワード'
    } else {
      this.title = 'ユーザ編集'
      this.passwordLabel = 'パスワード（変更する場合のみ入力）'
      await this['user/fetchDetail'](this.id)
      try {
        //@ts-ignore
        this.form.name = this.detail.name
        //@ts-ignore
        this.form.serviceType = this.detail.serviceType
        this.form.displayServiceType = this.form.serviceType
        if (this.form.serviceType === 1)
          this.form.displayServiceType = 'ローカル'
        if (this.form.serviceType === 2) this.form.displayServiceType = 'LDAP'
        //@ts-ignore
        this.detail.systemRoles.forEach(
          (s: gen.NssolPlatypusInfrastructureInfosRoleInfo) => {
            this.form.selectedSystemRoleIds.push(s.id!)
          },
        )
        this.form.tenants.selectedTenants = []
        this.form.tenants.selectedNotOriginTenants = []
        //@ts-ignore
        this.detail.tenants.forEach(
          (tenant: gen.NssolPlatypusInfrastructureInfosTenantInfo) => {
            let selectedRoleIds: Array<number> = []
            let selectedNotOriginTenants: Array<number> = []
            tenant.roles!.forEach(
              (role: gen.NssolPlatypusInfrastructureInfosRoleInfo) => {
                // KQI上で付与されたロールかどうか
                if (role.isOrigin) {
                  selectedRoleIds.push(role.id!)
                }
                // LDAP経由で付与されたロールかどうか
                if (role.userGroupTanantMapIdLists!.length > 0) {
                  selectedNotOriginTenants.push(role.id!)
                }
              },
            )
            // KQI上で参加したテナントかどうか
            if (tenant.isOrigin) {
              this.form.tenants.selectedTenants.push({
                tenantId: tenant.id!,
                tenantName: tenant.name!,
                selectedRoleIds: selectedRoleIds,
                default: tenant.default!,
              })
              this.form.tenants.selectedTenantIds.push(tenant.id!)
            }
            // LDAP経由で参加したテナントかどうか
            if (selectedNotOriginTenants.length > 0) {
              this.notOriginTenants.selectedTenants.push({
                tenantId: tenant.id!,
                tenantName: tenant.name!,
                selectedRoleIds: selectedNotOriginTenants!,
                default: tenant.default!,
              })
              this.notOriginTenants.selectedTenantIds.push(tenant.id!)
            }
          },
        )

        this.error = null
        this.deleteButtonParams = {
          isDanger: true,
          warningText:
            'ユーザを削除すると、紐づいているテナントからユーザ情報が失われます。処理を続けるにはユーザ名を入力してください。',
          confirmText: this.form.name,
        }
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    }
  },

  methods: {
    ...mapActions([
      'role/fetchRoles',
      'tenant/fetchTenants',
      'user/fetchDetail',
      'user/post',
      'user/put',
      'user/delete',
    ]),
    async submit() {
      let form = this.$refs.createForm
      //@ts-ignore
      await form.validate(async valid => {
        if (valid) {
          try {
            let postTenants: Array<{
              id: number
              default: boolean
              roles: Array<number>
            }> = []
            this.form.tenants.selectedTenants.forEach(tenant => {
              let postTenant = {
                id: tenant.tenantId,
                default: tenant.default,
                roles: tenant.selectedRoleIds,
              }
              postTenants.push(postTenant)
            })
            // Ldap経由のテナントはロールを空にして追加
            this.notOriginTenants.selectedTenants.forEach(tenant => {
              let postTenant = {
                id: tenant.tenantId,
                default: tenant.default,
                roles: [],
              }
              postTenants.push(postTenant)
            })
            let params = {
              name: this.form.name,
              displayName: this.form.displayName,
              password: this.form.password[0],
              systemRoles: this.form.selectedSystemRoleIds,
              tenants: postTenants,
              serviceType: this.form.serviceType,
            }
            if (this.isCreateDialog) {
              await this['user/post'](params)
            } else {
              await this['user/put']({ id: this.id, params: params })
            }
            this.emitDone()
            this.error = null
          } catch (e) {
            if (e instanceof Error) this.error = e
          }
        }
      })
    },
    async deleteUser() {
      try {
        await this['user/delete'](this.id)
        this.emitDone()
        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
    setNotOriginTenants(tenants: {
      selectedTenantIds: Array<number>
      selectedTenants: Array<{
        tenantId: number
        tenantName: string
        selectedRoleIds: Array<number>
        default: boolean
      }>
    }) {
      this.notOriginTenants = tenants
    },
    emitDone() {
      this.$emit('done')
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
})
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.btn-update {
  margin-left: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.pull-right {
  float: right !important;
}

.pull-left {
  float: left !important;
}
</style>
