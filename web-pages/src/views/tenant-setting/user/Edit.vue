<template>
  <kqi-dialog
    title="テナントユーザ編集"
    :type="'EDIT'"
    :delete-button-params="deleteButtonParams"
    @submit="submit"
    @delete="deleteUser"
    @close="emitCancel"
  >
    <el-form ref="form" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <kqi-display-text-form label="ユーザ名" :value="detail.name" />
      <kqi-display-text-form label="認証タイプ" :value="displayServiceType" />
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
      form: {
        tenantRoleIds: [],
      },
      displayServiceType: '',
      deleteButtonParams: {},
      dialogVisible: true,
      error: null,
      rules: {
        tenantRoleIds: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters({
      detail: ['user/tenantUserDetail'],
      roles: ['role/roles'],
    }),
  },

  async created() {
    try {
      await this['user/fetchTenantUserDetail']()
      await this['role/fetchRoles']()

      // serviceTypeのIDを変換
      switch (this.detail.serviceType) {
        case 1:
          this.displayServiceType = 'ローカル'
          break
        case 2:
          this.displayServiceType = 'LDAP'
          break
        default:
          this.displayServiceType = this.detail.serviceType
          break
      }
      // ロール一覧からIDを抽出
      this.detail.roles.forEach(s => {
        this.form.tenantRoleIds.push(s.id)
      })
      // dangerButtonのパラメータを設定
      this.deleteButtonParams = {
        isDanger: true,
        warningText:
          'ユーザを除外すると、対象ユーザは現在のテナントに入れなくなります。処理を続けるにはユーザ名を入力してください。',
        confirmText: this.detail.name,
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
