<template>
  <div>
    <h2>ユーザ情報設定</h2>
    <div class="parent-container">
      <kqi-display-error :error="error" />
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
          <label for="tab1_1">tenant</label>
          <input
            id="tab1_2"
            type="radio"
            name="cp_tab"
            aria-controls="second_tab01"
          />
          <label for="tab1_2">access Token</label>
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
            aria-controls="five_tab01"
          />
          <label v-if="passForm.passwordChangeEnabled" for="tab1_5">
            password
          </label>

          <div class="cp_tabpanels">
            <!-- デフォルトテナント設定 -->
            <default-tenant-setting
              id="first_tab01"
              v-model="defaultTenantName"
              class="cp_tabpanel"
              :tenants="account.tenants"
              @defaultTenantUpdate="defaultTenantUpdate"
            />

            <!-- アクセストークン取得 -->
            <access-token-setting
              v-model="tokenForm.day"
              :token="tokenForm.token"
              @getAccessToken="getAccessToken"
            />

            <!-- Gitトークン設定 -->
            <git-token-setting
              v-model="gitForm"
              :gits="gits"
              @updateGitToken="updateGitToken"
            />

            <!-- Registryトークン設定 -->
            <registry-token-setting
              v-model="registryForm"
              :registries="registries"
              @updateRegistryToken="updateRegistryToken"
            />

            <!-- パスワード変更 -->
            <div
              v-if="passForm.passwordChangeEnabled"
              id="five_tab01"
              class="cp_tabpanel"
            >
              <el-form ref="passForm" :rules="passRules" :model="passForm">
                <el-form-item
                  label="現在のパスワード"
                  prop="currentPassword"
                  :label-width="labelwidth"
                >
                  <el-input
                    v-model="passForm.currentPassword"
                    type="password"
                  />
                </el-form-item>
                <el-form-item
                  label="新しいパスワード"
                  prop="password"
                  :label-width="labelwidth"
                >
                  <el-input v-model="passForm.password[0]" type="password" />
                  <span style="position: relative; left: -200px; top:24px;">
                    （再入力）
                  </span>
                  <el-input
                    v-model="passForm.password[1]"
                    type="password"
                    style="position: relative; top:-20px;"
                  />
                </el-form-item>
                <el-form-item class="button-group">
                  <el-button type="primary" @click="handlePassword">
                    更新
                  </el-button>
                </el-form-item>
              </el-form>
            </div>
          </div>
        </div>
      </el-card>
    </div>
  </div>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import GitTokenSetting from '@/views/account/GitTokenSetting'
import RegistryTokenSetting from '@/views/account/RegistryTokenSetting'
import TenantInfo from './TenantInfo'
import DefaultTenantSetting from './DefaultTenantSetting'
import AccessTokenSetting from './AccessTokenSetting'
import { mapGetters, mapActions } from 'vuex'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  title: 'ユーザ情報設定',
  components: {
    TenantInfo,
    DefaultTenantSetting,
    AccessTokenSetting,
    KqiDisplayError,
    GitTokenSetting,
    RegistryTokenSetting,
  },
  data() {
    return {
      error: null,
      labelwidth: '220px',

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
      },

      passForm: {
        passwordChangeEnabled: true,
        currentPassword: '',
        password: ['', ''],
      },
      passRules: {
        currentPassword: [formRule],
        password: [
          {
            required: true,
            trigger: 'blur',
            validator: this.passwordValidator,
          },
        ],
      },
    }
  },
  computed: {
    ...mapGetters({
      account: ['account/account'],
      token: ['account/token'],
      gits: ['gitSelector/gits'],
      git: ['gitSelector/git'],
      registries: ['registrySelector/registries'],
      registry: ['registrySelector/registry'],
    }),
  },
  async created() {
    try {
      // ログインユーザのアカウント情報を取得する
      await this['account/fetchAccount']()
      this.defaultTenantName = this.account.defaultTenant.name
      this.passForm.passwordChangeEnabled = this.account.passwordChangeEnabled

      // 選択中のテナントにおけるGit情報を取得する
      await this['gitSelector/fetchGits']()
      // gitFormにデフォルトGit情報を設定
      this.gitForm.id = this.git.id
      this.gitForm.name = this.git.name
      this.gitForm.token = this.git.token

      // 選択中のテナントにおけるレジストリ情報を取得する
      await this['registrySelector/fetchRegistries']()
      this.registryForm.id = this.registry.id
      this.registryForm.name = this.registry.name
      this.registryForm.userName = this.registry.userName
      this.registryForm.password = this.registry.password
    } catch (e) {
      this.error = e
    }
  },

  methods: {
    ...mapActions([
      'account/fetchAccount',
      'account/put',
      'account/putPassword',
      'account/postTokenTenants',
      'account/putGitToken',
      'account/putRegistryToken',
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
        this.error = null
      } catch (error) {
        this.error = error
      }
    },
    async getAccessToken() {
      try {
        let params = {
          tenantId: this.account.selectedTenant.id,
          expiresIn: this.tokenForm.day * 60 * 60 * 24,
        }
        // 新規アクセストークンを取得する
        await this['account/postTokenTenants'](params)
        this.tokenForm.token = this.token
        this.showSuccessMessage()
        this.error = null
      } catch (error) {
        this.error = error
      }
    },

    async updateGitToken() {
      try {
        let params = {
          model: {
            id: this.gitForm.id,
            token: this.gitForm.token,
          },
        }
        await this['account/putGitToken'](params)
        this.showSuccessMessage()
        this.error = null
      } catch (error) {
        this.error = error
      }
    },

    async updateRegistryToken() {
      try {
        let params = {
          model: {
            id: this.registryForm.id,
            userName: this.registryForm.userName,
            password: this.registryForm.password,
          },
        }
        await this['account/putRegistryToken'](params)
        this.showSuccessMessage()
        this.error = null
      } catch (error) {
        this.error = error
      }
    },

    passwordValidator(rule, value, callback) {
      if (!(value[0] && value[1])) {
        callback(new Error('必須項目です'))
      } else if (!(value[0] === value[1])) {
        callback(new Error('同一のパスワードを入力してください'))
      } else {
        callback()
      }
    },

    async handlePassword() {
      this.$refs['passForm'].validate(async valid => {
        if (valid) {
          try {
            let params = {
              model: {
                currentPassword: this.passForm.currentPassword,
                newPassword: this.passForm.password[0],
              },
            }
            await this['account/putPassword'](params)
            this.showSuccessMessage()
            this.error = null
          } catch (error) {
            this.error = error
          }
        }
      })
    },
  },
}
</script>

<style lang="scss" scoped>
.parent-container {
  display: grid;
  grid-template-rows: 100px 700px;
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

.button-group {
  text-align: right;
  padding-top: 150px;
}

.el-form-item {
  margin-top: 30px;
}

.el-form-item /deep/ .el-form-item__label {
  font-weight: bold !important;
  padding-right: 30px;
  text-align: left;
}

.el-form-item /deep/ .el-form-item__inner {
  text-align: left;
}

.el-form-item /deep/ .el-form-item__content {
  font-weight: bold !important;
  padding-right: 30px;
}

.el-form-item.is-required {
  padding-top: 100px;
  text-align: left;
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
