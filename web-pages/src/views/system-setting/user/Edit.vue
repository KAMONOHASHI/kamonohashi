<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    submit-text="作成"
    :delete-button-params="deleteButtonParams"
    @submit="submit"
    @delete="deleteUser"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />

      <span v-if="form.serviceType === 1">
        <el-form-item v-if="id === null" label="ユーザ名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
        <kqi-display-text-form v-else label="ユーザ名" :value="form.name" />
        <kqi-display-text-form
          v-if="id !== null"
          label="認証タイプ"
          :value="form.displayServiceType"
        />
        <el-form-item :label="passwordLabel" prop="password">
          <el-input v-model="form.password[0]" type="password" />
          パスワード（再入力）<br />
          <el-input v-model="form.password[1]" type="password" />
        </el-form-item>
      </span>
      <span v-else-if="form.serviceType === 2">
        <kqi-display-text-form label="ユーザ名" :value="form.name" />
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
          :roles="systemRoles"
          show-system-role
        />
      </el-form-item>
      <el-form-item label="テナント" prop="tenants">
        <tenant-role-selector ref="tenantsForm" v-model="form.tenants" />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiRoleSelector from '@/components/selector/KqiRoleSelector'
import TenantRoleSelector from '@/views/system-setting/user/TenantRoleSelector'
import { mapGetters, mapActions } from 'vuex'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
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
  data() {
    return {
      dialogVisible: true,
      error: null,
      title: null,
      deleteButtonParams: {},
      passwordLabel: '',

      form: {
        name: '',
        serviceType: 1,
        displayServiceType: '',
        password: ['', ''],
        selectedSystemRoleIds: [],
        tenants: {
          selectedTenantsId: [],
          selectedTenants: [],
        },
      },
      rules: {
        name: [formRule],
        password: [
          { required: true, trigger: 'blur', validator: this.validatePassword },
        ],
        tenants: [
          { required: true, trigger: 'blur', validator: this.validateTenants },
        ],
      },
    }
  },
  computed: {
    ...mapGetters({
      detail: ['user/detail'],
      tenants: ['tenant/tenants'],
      systemRoles: ['role/roles'],
    }),
  },
  async created() {
    await this['role/fetchRoles']()
    await this['tenant/fetchTenants']()
    if (this.id === null) {
      this.title = 'ユーザ作成'
      this.passwordLabel = 'パスワード'
    } else {
      this.title = 'ユーザ編集'
      this.passwordLabel = 'パスワード（変更する場合のみ入力）'
      await this['user/fetchDetail']()
      try {
        this.form.name = this.detail.name
        this.form.serviceType = this.detail.serviceType
        this.form.displayServiceType = this.form.serviceType
        if (this.form.serviceType === 1)
          this.form.displayServiceType = 'ローカル'
        if (this.form.serviceType === 2) this.form.displayServiceType = 'LDAP'
        this.detail.systemRoles.forEach(s => {
          this.form.selectedSystemRoleIds.push(s.id)
        })
        this.form.tenants.selectedTenants = this.detail.tenants
        this.detail.tenants.forEach(s => {
          this.form.tenants.selectedTenantsId.push(s.id)
        })
        this.form.error = null
        this.deleteButtonParams = {
          isDanger: true,
          warningText:
            'ユーザを削除すると、紐づいているテナントからユーザ情報が失われます。処理を続けるにはユーザ名を入力してください。',
          confirmText: this.form.name,
        }
      } catch (e) {
        this.error = e
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
    /* eslint-enable */
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let postTenants = []
            for (
              let i = 0;
              i < this.form.tenants.selectedTenantsId.length;
              i++
            ) {
              this.form.tenants.selectedTenants[i].forEach(t => {
                postTenants.push({ id: t.id, default: t.default, roles: [] })
                t.roles.forEach(r => {
                  postTenants[postTenants.length - 1].roles.push(r.id)
                })
              })
            }
            let param = {
              model: {
                name: this.form.name,
                password: this.form.password[0],
                systemRoles: this.form.selectedSystemRoleIds,
                tenants: postTenants,
                serviceType: this.form.serviceType,
              },
            }
            if (this.id === null) {
              await this['user/post'](param)
            } else {
              await this['user/put'](param)
            }
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteUser() {
      try {
        let params = {
          id: this.id,
        }
        await this['user/delete'](params)
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    validatePassword(rule, value, callback) {
      if (!value[0] && !value[1]) {
        callback()
      } else if (!(value[0] === value[1])) {
        callback(new Error('同一のパスワードを入力してください'))
      } else {
        callback()
      }
    },
    validateTenants(rule, value, callback) {
      if (value.selectedTenantsId.length === 0) {
        callback(new Error('必須項目です'))
      } else {
        for (let i = 0; i < value.selectedTenantsId.length; i++) {
          if (value.selectedTenants[i].some(x => x.roles.length === 0)) {
            callback(new Error('ロールが選択されていないテナントがあります'))
          } else {
            callback()
          }
        }
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
