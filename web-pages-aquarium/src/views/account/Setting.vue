<template>
  <div>
    <h2>ユーザ情報設定</h2>
    <div class="parent-container">
      <!-- 選択中テナント情報 -->
      <tenant-info
        :user-name="account.userName"
        :tenant="account.selectedTenant"
      />

      <!-- ユーザ情報設定 -->
      <el-card class="container detail">
        <!-- キャプション -->
        <div class="cp_tab">
          <input
            id="tab1_1"
            type="radio"
            name="cp_tab"
            aria-controls="first_tab01"
            checked
          />
          <label for="tab1_1">Tenant</label>
          <input
            id="tab1_2"
            type="radio"
            name="cp_tab"
            aria-controls="second_tab01"
          />
          <label for="tab1_2">Access Token</label>
          <input
            id="tab1_3"
            type="radio"
            name="cp_tab"
            aria-controls="third_tab01"
          />
          <label for="tab1_3">Git Token</label>
          <input
            id="tab1_4"
            type="radio"
            name="cp_tab"
            aria-controls="force_tab01"
          />
          <label for="tab1_4">Registry Token</label>
          <input
            id="tab1_5"
            type="radio"
            name="cp_tab"
            aria-controls="fifth_tab01"
          />
          <label for="tab1_5">Webhook</label>
          <input
            v-if="passwordChangeEnabled"
            id="tab1_6"
            type="radio"
            name="cp_tab"
            aria-controls="sixth_tab01"
          />
          <label v-if="passwordChangeEnabled" for="tab1_6">
            Password
          </label>

          <div class="cp_tabpanels">
            <!-- デフォルトテナント設定 -->
            <div id="first_tab01" class="cp_tabpanel">
              <kqi-display-error :error="tenantError" />
              <default-tenant-setting
                v-model="defaultTenantName"
                :tenants="account.tenants"
                @defaultTenantUpdate="defaultTenantUpdate"
              />
            </div>

            <!-- アクセストークン取得 -->
            <div id="second_tab01" class="cp_tabpanel">
              <kqi-display-error :error="accessTokenError" />
              <access-token-setting
                v-model="tokenForm.day"
                :token="tokenForm.token"
                @getAccessToken="getAccessToken"
              />
            </div>

            <!-- Gitトークン設定 -->
            <div id="third_tab01" class="cp_tabpanel">
              <kqi-display-error :error="gitTokenError" />
              <git-token-setting
                v-model="gitForm"
                :gits="gits"
                @updateGitToken="updateGitToken"
              />
            </div>

            <!-- Registryトークン設定 -->
            <div id="force_tab01" class="cp_tabpanel">
              <kqi-display-error :error="registryTokenError" />
              <registry-token-setting
                v-model="registryForm"
                :registries="registries"
                @updateRegistryToken="updateRegistryToken"
              />
            </div>

            <!-- Webhook設定 -->
            <div id="fifth_tab01" class="cp_tabpanel">
              <kqi-display-error :error="webhookError" />
              <webhook-setting
                v-model="webhookForm"
                @updateWebhook="updateWebhook"
                @sendNotification="sendNotification"
              />
            </div>

            <!-- パスワード変更 -->
            <div
              v-if="passwordChangeEnabled"
              id="sixth_tab01"
              class="cp_tabpanel"
            >
              <kqi-display-error :error="passwordError" />
              <Password-Setting
                v-model="passForm"
                @updatePassword="updatePassword"
              />
            </div>
          </div>
        </div>
      </el-card>
    </div>
  </div>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import TenantInfo from './TenantInfo'
import DefaultTenantSetting from './DefaultTenantSetting'
import AccessTokenSetting from './AccessTokenSetting'
import GitTokenSetting from '@/views/account/GitTokenSetting'
import RegistryTokenSetting from '@/views/account/RegistryTokenSetting'
import PasswordSetting from './PasswordSetting'
import WebhookSetting from './WebhookSetting'
import { mapGetters, mapActions } from 'vuex'

export default {
  title: 'ユーザ情報設定',
  components: {
    KqiDisplayError,
    TenantInfo,
    DefaultTenantSetting,
    AccessTokenSetting,
    GitTokenSetting,
    RegistryTokenSetting,
    PasswordSetting,
    WebhookSetting,
  },
  data() {
    return {
      tenantError: null,
      accessTokenError: null,
      gitTokenError: null,
      registryTokenError: null,
      passwordError: null,
      webhookError: null,
      defaultTenantName: '',
      tokenForm: {
        token: '',
        day: 30,
      },
      gitForm: {
        id: 0,
        name: '',
        token: '',
      },
      registryForm: {
        id: 0,
        name: '',
        userName: '',
        password: '',
        serviceType: 0,
        projectName: '',
      },
      passwordChangeEnabled: true,
      passForm: {
        currentPassword: '',
        password: ['', ''],
      },
      webhookForm: {
        slackUrl: '',
        mention: '',
      },
    }
  },
  computed: {
    ...mapGetters({
      account: ['account/account'],
      token: ['account/token'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      registries: ['registrySelector/registries'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
      webhook: ['account/webhook'],
    }),
  },
  async created() {
    // ログインユーザのアカウント情報を取得する
    await this['account/fetchAccount']()
    this.defaultTenantName = this.account.defaultTenant.name
    this.passwordChangeEnabled = this.account.passwordChangeEnabled

    // 選択中のテナントにおけるGit情報を取得する
    await this['gitSelector/fetchGits']()
    // gitFormにデフォルトGit情報を設定
    this.gitForm = this.gits.find(git => {
      return git.id === this.defaultGitId
    })

    // 選択中のテナントにおけるレジストリ情報を取得する
    await this['registrySelector/fetchRegistries']()
    this.registryForm = this.registries.find(registry => {
      return registry.id === this.defaultRegistryId
    })

    // Webhook情報を取得する
    await this['account/fetchWebhook']()
    this.webhookForm = this.webhook
  },

  methods: {
    ...mapActions([
      'account/fetchAccount',
      'account/fetchWebhook',
      'account/put',
      'account/putPassword',
      'account/postTokenTenants',
      'account/putGitToken',
      'account/putRegistryToken',
      'account/putWebhook',
      'account/sendNotification',
      'gitSelector/fetchGits',
      'registrySelector/fetchRegistries',
    ]),

    async defaultTenantUpdate() {
      try {
        let params = {
          defaultTenant: this.defaultTenantName,
        }
        await this['account/put'](params)
        this.showSuccessMessage()
        this.tenantError = null
      } catch (error) {
        this.tenantError = error
      }
    },

    async getAccessToken() {
      try {
        let params = {
          tenantId: this.account.selectedTenant.id,
          body: { expiresIn: this.tokenForm.day * 60 * 60 * 24 },
        }
        // 新規アクセストークンを取得する
        await this['account/postTokenTenants'](params)
        this.tokenForm.token = this.token
        this.showSuccessMessage()
        this.accessTokenError = null
      } catch (error) {
        this.accessTokenError = error
      }
    },

    async updateGitToken() {
      try {
        let params = {
          body: {
            id: this.gitForm.id,
            token: this.gitForm.token,
          },
        }
        await this['account/putGitToken'](params)
        // storeで保持するgit tokenの情報を更新する
        await this['gitSelector/fetchGits']()
        this.showSuccessMessage()
        this.gitTokenError = null
      } catch (error) {
        this.gitTokenError = error
      }
    },

    async updateRegistryToken() {
      try {
        let params = {
          body: {
            id: this.registryForm.id,
            userName: this.registryForm.userName,
            password: this.registryForm.password,
          },
        }
        await this['account/putRegistryToken'](params)
        // storeで保持するregistry tokenの情報を更新する
        await this['registrySelector/fetchRegistries']()
        this.showSuccessMessage()
        this.registryTokenError = null
      } catch (error) {
        this.registryTokenError = error
      }
    },

    async updatePassword() {
      try {
        let params = {
          body: {
            currentPassword: this.passForm.currentPassword,
            newPassword: this.passForm.password[0],
          },
        }
        await this['account/putPassword'](params)
        this.showSuccessMessage()
        this.passwordError = null
      } catch (error) {
        this.passwordError = error
      }
    },

    async updateWebhook() {
      try {
        let params = {
          body: {
            slackUrl: this.webhookForm.slackUrl,
            mention: this.webhookForm.mention,
          },
        }
        await this['account/putWebhook'](params)
        this.showSuccessMessage()
        this.webhookError = null
      } catch (error) {
        this.webhookError = error
      }
    },

    async sendNotification() {
      try {
        let params = {
          body: {
            slackUrl: this.webhookForm.slackUrl,
            mention: this.webhookForm.mention,
          },
        }
        await this['account/sendNotification'](params)
        this.showSuccessMessage()
        this.webhookError = null
      } catch (error) {
        this.webhookError = error
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.parent-container {
  display: grid;
  grid-template-rows: 100px 600px;
  grid-template-columns: 500px 1fr;
}

.container {
  margin-top: 10px;
}

.detail {
  grid-row: 1 / 3;
  grid-column: 2 / 3;
}

.container-title {
  font-weight: bold;
  font-size: 18px;
  color: #1abfd5;
}

.cp_tab *,
.cp_tab *:before,
.cp_tab *:after {
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
}

.cp_tab {
  margin: 1em auto;
}

.cp_tab > input[type='radio'] {
  margin: 0;
  padding: 0;
  border: none;
  border-radius: 0;
  outline: none;
  background: none;
  -webkit-appearance: none;
  appearance: none;
  display: none;
}

.cp_tab .cp_tabpanel {
  display: none;
}

.cp_tab > input:first-child:checked ~ .cp_tabpanels > .cp_tabpanel:first-child,
.cp_tab
  > input:nth-child(3):checked
  ~ .cp_tabpanels
  > .cp_tabpanel:nth-child(2),
.cp_tab
  > input:nth-child(5):checked
  ~ .cp_tabpanels
  > .cp_tabpanel:nth-child(3),
.cp_tab
  > input:nth-child(7):checked
  ~ .cp_tabpanels
  > .cp_tabpanel:nth-child(4),
.cp_tab
  > input:nth-child(9):checked
  ~ .cp_tabpanels
  > .cp_tabpanel:nth-child(5),
.cp_tab
  > input:nth-child(11):checked
  ~ .cp_tabpanels
  > .cp_tabpanel:nth-child(6) {
  display: block;
}

.cp_tab > label {
  position: relative;
  display: inline-block;
  padding: 15px;
  cursor: pointer;
  border-bottom: 0;
}

.cp_tab > label:hover,
.cp_tab > input:focus + label {
  color: #1abfd5;
}

.cp_tab > input:checked + label {
  margin-bottom: -1px;
  border-bottom: 5px solid #1abfd5;
  color: #1abfd5;
  font-weight: bold;
}

.cp_tab .cp_tabpanel {
  padding: 0.5em 1em;
  border-top: 1px solid #cccccc;
}

@media (max-width: 480px) {
  .cp_tab {
    width: 100%;
    font-size: 0.8em;
  }
  .cp_tab label {
    padding: 0.5em;
  }
}
</style>
