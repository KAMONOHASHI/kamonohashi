<template>
  <el-dialog class="dialog"
             title="ユーザ作成"
             :visible="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form :model="this"
             :rules="rules"
             ref="form">
      <pl-display-error :error="error"/>
      <el-form-item label="ユーザ名" prop="name">
        <el-input v-model="name"/>
      </el-form-item>
      <el-form-item label="パスワード" prop="password">
        <el-input v-model="password[0]" type="password"/>
        &nbsp;&nbsp;パスワード（再入力）<br/>
        <el-input v-model="password[1]" type="password"/>
      </el-form-item>
      <el-form-item label="システムロール" prop="roleIds">
        <pl-role-selector v-model="roleIds" :multiple="true" :system="true"/>
      </el-form-item>
      <el-form-item label="テナント" prop="tenants">
        <pl-tenant-role-selector v-model="tenants" :multiple="true"/>
      </el-form-item>
      <el-row>
        <el-col class="button-group">
          <el-button @click="handleCancel">キャンセル</el-button>
          <el-button @click="handleCreate" type="primary">作成</el-button>
        </el-col>
      </el-row>
    </el-form>

  </el-dialog>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'
  import RoleSelector from '@/components/common/RoleSelector'
  import TenantRoleSelector from '@/components/system-setting/user/TenantRoleSelector'

  export default {
    name: 'UserCreate',
    components: {
      'pl-role-selector': RoleSelector,
      'pl-tenant-role-selector': TenantRoleSelector,
      'pl-display-error': DisplayError
    },
    data () {
      return {
        dialogVisible: true,
        error: null,

        name: '',
        password: ['', ''],
        roleIds: [],
        tenants: [],

        rules: {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          tenants: [{required: true, trigger: 'blur', validator: this.validateTenants}],
          password: [{required: true, trigger: 'blur', validator: this.validatePassword}]
        }
      }
    },
    methods: {
      async handleCreate () {
        let form = this.$refs.form
        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.postUser()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },

      async handleCancel () {
        this.emitCancel()
      },

      async postUser () {
        let postTenants = []
        this.tenants.forEach(t => {
          postTenants.push({id: t.id, default: t.default, roles: []})
          t.roles.forEach(r => {
            postTenants[postTenants.length - 1].roles.push(r.id)
          })
        })
        let param = {
          model: {
            name: this.name,
            password: this.password[0],
            systemRoles: this.roleIds,
            tenants: postTenants
          }
        }
        await api.user.admin.post(param)
      },

      validatePassword (rule, value, callback) {
        if (!(value[0] && value[1])) {
          callback(new Error('必須項目です'))
        } else if (!(value[0] === value[1])) {
          callback(new Error('同一のパスワードを入力してください'))
        } else {
          callback()
        }
      },

      validateTenants (rule, value, callback) {
        console.log(value)
        if (value.length === 0) {
          callback(new Error('必須項目です'))
        } else {
          if (value.some(x => x.roles.length === 0)) {
            callback(new Error('ロールが選択されていないテナントがあります'))
          } else {
            callback()
          }
        }
      },

      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      }
    }
  }
</script>

<style lang="scss" scoped>
  .button-group {
    text-align: right;
    padding-top: 10px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }
</style>
