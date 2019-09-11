<template>
  <div>
    <h2> ユーザ情報設定 </h2>
    <div class="parent-container">

      <pl-display-error :error="error"/>
      <el-card class="container base">
        <img class="logo" src="/static/images/logo_A.png" alt="">
        <el-form ref="userForm" :rules="userRules" :model="userForm">
          <el-form-item style="text-align: center;">
            {{ userForm.userName }}
          </el-form-item>
          <el-form-item :label="$t('selectedTenant')" :label-width="labelwidth">
            {{ userForm.selectedTenant.displayName }}
            (ID: {{ userForm.selectedTenant.id }})
          </el-form-item>
          <el-form-item :label="$t('role')" :label-width="labelwidth">
            <div v-for="(r, index) in userForm.selectedTenant.roles" :key="index">
              {{ r.displayName }}
            </div>
          </el-form-item>
        </el-form>
      </el-card>
      <el-card class="container detail">
        <div class="cp_tab">
          <input type="radio" name="cp_tab" id="tab1_1" aria-controls="first_tab01" checked>
          <label for="tab1_1">tenant</label>
          <input type="radio" name="cp_tab" id="tab1_2" aria-controls="second_tab01">
          <label for="tab1_2">access Token</label>
          <input type="radio" name="cp_tab" id="tab1_3" aria-controls="third_tab01">
          <label for="tab1_3">Git Token</label>
          <input type="radio" name="cp_tab" id="tab1_4" aria-controls="force_tab01">
          <label for="tab1_4">Registry Token</label>
          <input type="radio" name="cp_tab" id="tab1_5" aria-controls="five_tab01">
          <label for="tab1_5" v-if="passForm.passwordChangeEnabled">password</label>
          <div class="cp_tabpanels">
            <div id="first_tab01" class="cp_tabpanel">
              <el-form ref="userForm" :rules="userRules" :model="userForm">
                <el-form-item :label="$t('defaultTenant')" prop="tenants" :label-width="labelwidth">
                  <el-select v-model="userForm.defaultTenant.name" placeholder="Select" v-if="userForm.tenants"
                             :clearable="true">
                    <el-option
                      v-for="(t, index) in userForm.tenants"
                      :key="index"
                      :label="t.displayName"
                      :value="t.name">
                    </el-option>
                  </el-select>
                </el-form-item>
                <el-form-item class="button-group">
                  <el-button type="primary" @click="handleSave">{{$t('update')}}</el-button>
                </el-form-item>
              </el-form>
            </div>
            <div id="second_tab01" class="cp_tabpanel">
              <el-form ref="tokenForm" :rules="tokenRules" :model="tokenForm">
                <el-form-item :label="$t('limitDays')" prop="day" :label-width="labelwidth">
                  <el-slider
                    class="el-input"
                    v-model="tokenForm.day"
                    :min="1"
                    :max="3650"
                    show-input>
                  </el-slider>
                  <br/>
                  {{$t('description')}}
                </el-form-item>
                <el-form-item :label="$t('token')" prop="token" :label-width="labelwidth" v-if="tokenForm.token">
                  <el-input v-model="tokenForm.token" type="textarea" autosize readonly/>
                </el-form-item>
              </el-form>
              <el-form>
                <el-form-item class="button-group" v-if="!tokenForm.token">
                  <el-button type="primary" @click="handleAccessToken">{{$t('tokenPayout')}}</el-button>
                </el-form-item>
              </el-form>
            </div>
            <div id="third_tab01" class="cp_tabpanel">
              <div v-if="gitForm.gits.length <= 0 ">
                Gitリポジトリが選択されていません。
                システム管理者にお問い合わせください。
              </div>
              <div v-else>
                <el-form ref="gitForm" :rules="gitRules" :model="gitForm">
                  <el-form-item :label="$t('selectedGit')" :label-width="labelwidth">
                    <pl-git-selecter v-model="gitForm"></pl-git-selecter>
                  </el-form-item>
                  <el-form-item class="button-group">
                    <el-button type="primary" @click="handleGitToken">{{$t('update')}}</el-button>
                  </el-form-item>
                </el-form>
              </div>
            </div>
            <div id="force_tab01" class="cp_tabpanel">
              <div v-if="registryForm.registries.length <= 0 ">
                Registryが選択されていません。
                システム管理者にお問い合わせください。
              </div>
              <div v-else>
                <el-form ref="registryForm" :rules="registryRules" :model="registryForm">
                  <el-form-item :label="$t('selectedRegistry')" :label-width="labelwidth">
                    <pl-registry-selecter v-model="registryForm"></pl-registry-selecter>
                  </el-form-item>
                  <el-form-item class="button-group">
                    <el-button type="primary" @click="handleRegistryToken">{{$t('update')}}</el-button>
                  </el-form-item>
                </el-form>
              </div>
            </div>
            <div id="five_tab01" class="cp_tabpanel" v-if="passForm.passwordChangeEnabled">
              <el-form ref="passForm" :rules="passRules" :model="passForm">
                <el-form-item :label="$t('nowPassword')" prop="currentPassword" :label-width="labelwidth">
                  <el-input v-model="passForm.currentPassword" type="password"/>
                </el-form-item>
                <el-form-item :label="$t('newPassword')" prop="password" :label-width="labelwidth">
                  <el-input v-model="passForm.password[0]" type="password"/>
                  <span style="position: relative; left: -200px; top:24px;">{{$t('rePassword')}}</span>
                  <el-input v-model="passForm.password[1]" type="password"
                            style="position: relative; top:-20px;"/>
                </el-form-item>
                <el-form-item class="button-group">
                  <el-button type="primary" @click="handlePassword">{{$t('update')}}</el-button>
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
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'
  import RegistrySelector from '@/components/account/RegistrySelector.vue'
  import GitSelectorctor from '@/components/account/GitSelector.vue'

  export default {
    name: 'AccountSetting',
    title () {
      return this.$t('title')
    },
    components: {
      'pl-display-error': DisplayError,
      'pl-registry-selecter': RegistrySelector,
      'pl-git-selecter': GitSelectorctor
    },
    data () {
      return {
        error: null,
        error1: null,
        error2: null,
        error3: null,
        error4: null,
        labelwidth: '220px',

        userForm: {
          userId: 0,
          userName: '',
          selectedTenant: '',
          selectedTenantName: '',
          selectedTenantRoles: [],
          defaultTenant: '',
          defaultTenantName: '',
          tenants: {}
        },
        userRules: {
          tenants: [{required: true, trigger: 'blur', message: this.$t('required')}]
        },
        registryForm: {
          registryId: 0,
          userName: '',
          password: '',
          name: '',
          registries: {}
        },
        registryRules: {
          registries: [{required: true, trigger: 'blur', message: this.$t('required')}]
        },
        gitForm: {
          gitId: 0,
          name: '',
          token: '',
          gits: {}
        },
        gitRules: {
          gits: [{required: true, trigger: 'blur', message: this.$t('required')}]
        },
        passForm: {
          passwordChangeEnabled: true,
          currentPassword: '',
          password: ['', '']
        },
        passRules: {
          currentPassword: [{required: true, trigger: 'blur', message: this.$t('required')}],
          password: [{required: true, trigger: 'blur', validator: this.validatorPassword}]
        },

        tokenForm: {
          token: '',
          day: 30
        },
        tokenRules: {
          day: [{required: true, trigger: 'blur', message: this.$t('required')}]
        }
      }
    },
    async created () {
      await this.init()
    },
    methods: {
      validatorPassword (rule, value, callback) {
        if (!(value[0] && value[1])) {
          callback(new Error(this.$t('required')))
        } else if (!(value[0] === value[1])) {
          callback(new Error(this.$t('passError1')))
        } else {
          callback()
        }
      },

      async init () {
        try {
          let data = (await api.account.get()).data
          this.userForm.userId = data.userId
          this.userForm.userName = data.userName
          this.userForm.selectedTenant = data.selectedTenant
          this.userForm.selectedTenantName = data.selectedTenantName
          this.userForm.selectedTenantRoles = data.selectedTenantRoles
          this.userForm.defaultTenant = data.defaultTenant
          this.userForm.defaultTenantName = data.defaultTenantName
          this.userForm.tenants = data.tenants
          this.passForm.passwordChangeEnabled = data.passwordChangeEnabled
          let registries = (await api.account.getRegistries()).data
          this.registryForm.registries = registries.registries
          this.registryForm.registryId = registries.defaultRegistryId
          for (let i = 0; i < this.registryForm.registries.length; i++) {
            if (this.registryForm.registryId === this.registryForm.registries[i].id) {
              this.registryForm.name = this.registryForm.registries[i].name
              this.registryForm.userName = this.registryForm.registries[i].userName
              this.registryForm.password = this.registryForm.registries[i].password
            }
          }
          let gits = (await api.account.getGits()).data
          console.log(gits)
          this.gitForm.gits = gits.gits
          this.gitForm.gitId = gits.defaultGitId
          for (let i = 0; i < this.gitForm.gits.length; i++) {
            if (this.gitForm.gitId === this.gitForm.gits[i].id) {
              this.gitForm.name = gits.gits[i].name
              this.gitForm.serviceType = gits.gits[i].serviceType
              this.gitForm.token = gits.gits[i].token
            }
          }
        } catch (e) {
          this.error = e
        }
      },

      async handleSave () {
        this.$refs['userForm'].validate(async (valid) => {
          if (valid) {
            try {
              await this.putUserSetting()
              this.showSuccessMessage()
              this.error1 = null
            } catch (error) {
              this.error1 = error
            }
          }
        })
      },

      async handlePassword () {
        this.$refs['passForm'].validate(async (valid) => {
          if (valid) {
            try {
              await this.putPassword()
              this.showSuccessMessage()
              this.error2 = null
            } catch (error) {
              this.error2 = error
            }
          }
        })
      },

      async handleRegistryToken () {
          try {
            await this.putRegistryToken()
            this.showSuccessMessage()
            this.error4 = null
          } catch (error) {
            this.error4 = error
          }
      },

      async handleGitToken () {
          try {
            await this.putGitToken()
            this.showSuccessMessage()
            this.error4 = null
          } catch (error) {
            this.error4 = error
          }
      },

      async handleAccessToken () {
        this.$refs['tokenForm'].validate(async (valid) => {
          if (valid) {
            try {
              let res = await this.postTenantsToken()
              this.tokenForm.token = res.data.token
              this.showSuccessMessage()
              this.error3 = null
            } catch (error) {
              this.error3 = error
            }
          }
        })
      },

      async putRegistryToken () {
        let params = {
          model: {
            id: this.registryForm.registryId,
            userName: this.registryForm.userName,
            password: this.registryForm.password
          }
        }
        await api.account.putRegistries(params)
      },

      async putGitToken () {
        let params = {
          model: {
            id: this.gitForm.gitId,
            token: this.gitForm.token
          }
        }
        await api.account.putGits(params)
      },

      async putUserSetting () {
        let params = {
          defaultTenant: this.userForm.defaultTenant.name
        }
        await api.account.put(params)
      },

      async putPassword () {
        let params = {
          model: {
            currentPassword: this.passForm.currentPassword,
            newPassword: this.passForm.password[0]
          }
        }
        await api.account.putPassword(params)
      },

      async postTenantsToken () {
        let params = {
          tenantId: this.userForm.selectedTenant.id,
          expiresIn: this.tokenForm.day * 60 * 60 * 24
        }
        let res = await api.account.postTokenTenants(params)
        return res
      }
    },
    i18n: {
      messages: {
        en: {
          title: 'Account Setting',
          acccesToken: 'Access Token',
          user: 'User',
          selectedTenant: 'Selected Tenant',
          selectedGit: 'selectedGit',
          selectedRegistry: 'selectedRegistry',
          userName: 'userName',
          password: 'password',
          role: 'Role',
          defaultTenant: 'Default Tenant',
          defaultGit: 'Default Git Repository',
          defaultRegistry: 'Default Registry',
          update: 'Update',
          passwordChange: 'Change Password',
          nowPassword: 'Now password',
          newPassword: 'New password',
          rePassword: 'Retry input',
          limitDays: 'Days expired',
          description: 'Please enter a number from 1 to 3650.',
          token: 'Token',
          tokenPayout: 'Get Token',
          required: 'required',
          passError1: 'Please enter the same password'
        },
        ja: {
          title: 'ユーザ情報設定',
          acccesToken: 'アクセストークン',
          user: 'ユーザ名',
          selectedTenant: '選択中のテナント',
          selectedGit: '選択中のGitリポジトリ',
          selectedRegistry: '選択中のRegistry',
          userName: 'ユーザ名',
          password: 'パスワード',
          role: 'ロール',
          defaultTenant: '既定のテナント',
          defaultGit: '既定のGitリポジトリ',
          defaultRegistry: '既定のRegistry',
          update: '更新',
          passwordChange: 'パスワード変更',
          nowPassword: '現在のパスワード',
          newPassword: '新しいパスワード',
          rePassword: '（再入力）',
          limitDays: '期限切れまでの日数',
          description: '値は 1 ～ 3650 の数字を入力して下さい。',
          token: 'トークン',
          tokenPayout: 'トークン発行',
          required: '必須項目です',
          passError1: '同一のパスワードを入力してください'
        }
      }
    }
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

  .base {
    grid-row: 1 / 3;
    grid-column: 1 / 2;
    margin-right: 20px;
  }

  .detail {
    grid-row: 1 / 3;
    grid-column: 2 / 3;
  }

  .logo {
    text-align: center;
    margin-left: 106px;
  }

  .container-title {
    font-weight: bold;
    font-size: 18px;
    color: #1abfd5;
  }

  .error-message {
    padding-bottom: 30px;
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

  .cp_tab *, .cp_tab *:before, .cp_tab *:after {
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
  .cp_tab > input:nth-child(3):checked ~ .cp_tabpanels > .cp_tabpanel:nth-child(2),
  .cp_tab > input:nth-child(5):checked ~ .cp_tabpanels > .cp_tabpanel:nth-child(3),
  .cp_tab > input:nth-child(7):checked ~ .cp_tabpanels > .cp_tabpanel:nth-child(4),
  .cp_tab > input:nth-child(9):checked ~ .cp_tabpanels > .cp_tabpanel:nth-child(5),
  .cp_tab > input:nth-child(11):checked ~ .cp_tabpanels > .cp_tabpanel:nth-child(6) {
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
