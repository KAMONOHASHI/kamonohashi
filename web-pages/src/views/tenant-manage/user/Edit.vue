<template>
  <kqi-dialog
    title="テナントユーザ編集2"
    :type="'EDIT'"
    :delete-button-params="deleteButtonParams"
    @submit="submit"
    @delete="deleteUser"
    @close="emitCancel"
  >
    <el-form ref="form" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <kqi-display-text-form label="ユーザ名" :value="form.name" />
      <kqi-display-text-form
        label="認証タイプ"
        :value="form.displayServiceType"
      />
      <el-form-item label="テナントロール" prop="tenantRoleIds">
        <kqi-role-selector
          v-model="form.tenantRoleIds"
          :roles="roles"
          :show-system-role="false"
        />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiRoleSelector from '@/components/selector/KqiRoleSelector'
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
      deleteButtonParams: {},

      form: {
        name: '',
        serviceType: 1,
        displayServiceType: '',
        tenantRoleIds: [],
      },
      rules: {
        tenantRoleIds: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters({ detail: ['user/tenantUserDetail'], roles: ['role/roles'] }),
  },

  async created() {
    await this['user/fetchTenantUserDetail']()
    await this['role/fetchRoles']()
    try {
      this.form.name = this.detail.name
      this.form.serviceType = this.detail.serviceType
      this.form.displayServiceType = this.form.serviceType
      if (this.form.serviceType === 1) this.form.displayServiceType = 'ローカル'
      if (this.form.serviceType === 2) this.form.displayServiceType = 'LDAP'
      this.detail.roles.forEach(s => {
        this.form.tenantRoleIds.push(s.id)
      })
      this.deleteButtonParams = {
        isDanger: true,
        warningText:
          'ユーザを除外すると、対象ユーザは現在のテナントに入れなくなります。処理を続けるにはユーザ名を入力してください。',
        confirmText: this.form.name,
      }
      this.error = null
    } catch (e) {
      this.error = e
    }
  },

  methods: {
    ...mapActions([
      'user/fetchTenantUserDetail',
      'user/tenantRolesPut',
      'user/tenantUserDelete',
      'role/fetchRoles',
    ]),
    /* eslint-enable */
    async submit() {
      let form = this.$refs.form
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              roleIds: this.form.tenantRoleIds,
            }
            await this['user/tenantRolesPut'](params)
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
        await this['user/tenantUserDelete'](params)
        this.emitDone()
        this.error = null
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
