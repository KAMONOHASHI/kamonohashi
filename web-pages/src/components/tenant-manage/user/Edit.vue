<template>
  <el-dialog class="dialog"
             title="テナントユーザ編集"
             :visible="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form :model="this"
             :rules="rules"
             ref="form">
      <pl-display-error :error="error"/>
      <pl-display-text label="ユーザアカウント" :value="name"/>
      <pl-display-text label="認証タイプ" :value="displayServiceType"/>
      <el-form-item label="テナントロール" prop="roleIds">
        <pl-role-selector v-model="roleIds" :multiple="true" :tenant="true"/>
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
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import RemoveUserButton from '@/components/tenant-manage/user/RemoveUserButton.vue'

  export default {
    name: 'UserEdit',
    components: {
      'pl-role-selector': RoleSelector,
      'pl-display-error': DisplayError,
      'pl-display-text': DisplayTextForm,
      'pl-delete-button': RemoveUserButton
    },
    props: {
      'id': {
        type: String,
        defalut: 0
      }
    },
    data () {
      return {
        dialogVisible: true,
        error: null,

        name: '',
        serviceType: 1,
        displayServiceType: '',
        roleIds: [],

        rules: {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          roleIds: [{required: true, trigger: 'blur', message: '必須項目です'}]
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
          let data = (await api.user.tenant.getById(params)).data
          this.name = data.name
          this.serviceType = data.serviceType
          this.displayServiceType = this.serviceType
          if (this.serviceType === 1) this.displayServiceType = 'ローカル'
          if (this.serviceType === 2) this.displayServiceType = 'LDAP'
          this.roleIds = this.selectedArrayByIds(data.roles)
          this.error = null
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
              await this.putTenantRoles()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },

      async putTenantRoles () {
        let param = {
          id: this.id,
          roleIds: this.roleIds
        }
        await api.user.tenant.putRoles(param)
      },

      async handleRemove () {
        try {
          let params = {
            id: this.id
          }
          await api.user.tenant.delete(params)
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
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
