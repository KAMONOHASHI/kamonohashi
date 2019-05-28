<template>
  <el-dialog class="dialog"
             title="ユーザ編集"
             :visible="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form :model="this"
             :rules="rules"
             ref="form">
      <pl-display-error :error="error"/>

      <span v-if="serviceType===1">
        <pl-display-text label="ユーザアカウント" :value="name"/>
        <pl-display-text label="認証タイプ" :value="displayServiceType"/>
        <el-form-item label="パスワード（変更する場合のみ入力）" prop="password">
          <el-input v-model="password[0]" type="password"/>
          パスワード（再入力）<br/>
          <el-input v-model="password[1]" type="password"/>
        </el-form-item>
      </span>
      <span v-else-if="serviceType===2">
        <pl-display-text label="ユーザアカウント" :value="name"/>
        <pl-display-text label="認証タイプ" :value="displayServiceType"/>
      </span>
      <span v-else>
        認証タイプ：不明
      </span>

      <el-form-item label="システムロール" prop="roleIds">
        <pl-role-selector v-model="roleIds" :multiple="true" :system="true"/>
      </el-form-item>
      <el-form-item label="テナント" prop="tenants">
        <pl-tenant-role-selector v-model="tenants" :multiple="true" ref="tenantsForm"/>
      </el-form-item>
      <el-row>
        <el-col class="button-group">
          <el-button @click="handleUpdate" class="pull-right btn-update" type="primary" icon="el-icon-edit-outline">保存
          </el-button>
          <el-button @click="handleCancel" class="pull-right btn-cancel" icon="el-icon-close">キャンセル</el-button>
          <pl-delete-button class="pull-left btn-update" @delete="handleRemove" v-bind:user-name="this.name"/>
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
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DeleteUserButton from '@/components/system-setting/user/DeleteUserButton.vue'

  export default {
    name: 'UserEdit',
    components: {
      'pl-role-selector': RoleSelector,
      'pl-tenant-role-selector': TenantRoleSelector,
      'pl-display-error': DisplayError,
      'pl-display-text': DisplayTextForm,
      'pl-delete-button': DeleteUserButton
    },
    props: {
      'id': 0
    },
    data () {
      return {
        dialogVisible: true,
        error: null,

        name: '',
        serviceType: 1,
        displayServiceType: '',
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

    async created () {
      await this.changeId()
    },

    watch: {
      async id () {
        await this.changeId()
      }
    },

    methods: {
      async changeId () {
        try {
          let params = {
            id: this.id
          }
          let data = (await api.user.admin.getById(params)).data
          this.name = data.name
          this.serviceType = data.serviceType
          this.displayServiceType = this.serviceType
          if (this.serviceType === 1) this.displayServiceType = 'ローカル'
          if (this.serviceType === 2) this.displayServiceType = 'LDAP'
          this.roleIds = this.selectedArrayByIds(data.systemRoles)
          this.tenants = data.tenants
          this.error = null
          this.$refs.tenantsForm.updateTenants(data.tenants)
        } catch (e) {
          this.error = e
        }
      },

      selectedArrayByIds (selected) {
        let ret = []
        selected.forEach(s => {
          ret.push(s.id)
        })
        return ret
      },

      async handleClose (done) {
        this.close(false)
      },

      async handleUpdate () {
        let form = this.$refs.form
        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.putPassword()
              await this.putUser()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async putPassword () {
        if (this.serviceType === 1) { // ローカルユーザーのみ更新
          if (this.password[0]) {
            let param = {
              id: this.id,
              password: this.password[0]
            }
            await api.user.admin.putPassword(param)
          }
        }
      },
      async putUser () {
        let postTenants = []
        this.tenants.forEach(t => {
          postTenants.push({id: t.id, default: t.default, roles: []})
          t.roles.forEach(r => {
            postTenants[postTenants.length - 1].roles.push(r.id)
          })
        })
        let param = {
          id: this.id,
          model: {
            systemRoles: this.roleIds,
            tenants: postTenants
          }
        }
        await api.user.admin.put(param)
      },

      async handleRemove () {
        try {
          let params = {
            id: this.id
          }
          await api.user.admin.delete(params)
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
      },

      validatePassword (rule, value, callback) {
        if (!value[0] && !value[1]) {
          callback()
        } else if (!(value[0] === value[1])) {
          callback(new Error('同一のパスワードを入力してください'))
        } else {
          callback()
        }
      },
      validateTenants (rule, value, callback) {
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

      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      }
    }
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
    font-weight: bold !important
  }

  .pull-right {
    float: right !important;
  }

  .pull-left {
    float: left !important;
  }
</style>
