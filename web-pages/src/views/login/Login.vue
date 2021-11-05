<template>
  <div class="parent">
    <p>
      <el-row>
        <el-col class="image">
          <img class="logo" src="@/assets/logo_A.png" alt="" />
        </el-col>
      </el-row>

      <el-row>
        <el-col class="error-message">
          <kqi-display-error :error="error" />
        </el-col>
      </el-row>

      <el-row>
        <el-form
          ref="loginForm"
          :label-position="labelPosition"
          :rules="rules"
          :model="form"
          @submit.native.prevent="handleLogin"
        >
          <el-form-item prop="user" :label-width="labelwidth">
            <el-input v-model="form.user" placeholder="ユーザ名" />
          </el-form-item>
          <el-form-item prop="password" :label-width="labelwidth">
            <el-input
              v-model="form.password"
              type="password"
              placeholder="パスワード"
            />
          </el-form-item>
          <el-form-item class="button-group">
            <el-button style="width: 100%;" type="primary" native-type="submit">
              {{ 'ログイン' }}
            </el-button>
          </el-form-item>
        </el-form>
      </el-row>
    </p>
  </div>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import Util from '@/util/util'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('account')
const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  title: 'ログイン',
  components: {
    KqiDisplayError,
  },
  data() {
    let err = null
    if (this.$route.query.timeout) {
      err = Error('認証エラー：ログインしてください。')
    }
    return {
      labelwidth: '100px',
      error: err,
      form: {
        user: '',
        password: '',
      },
      rules: {
        user: [formRule],
        password: [formRule],
      },
      returnUrl: this.$route.query.return_url,
      labelPosition: 'top',
    }
  },
  computed: {
    ...mapGetters(['loginData']),
  },
  methods: {
    ...mapActions(['postLogin']),
    async handleLogin() {
      this.$refs['loginForm'].validate(async valid => {
        if (valid) {
          try {
            let params = {
              $config: { apiDisabledError: true },
              body: {
                userName: this.form.user,
                password: this.form.password,
              },
            }
            await this.postLogin(params)
            this.error = null
            this.$emit(
              'login',
              this.loginData.userName,
              this.loginData.tenantId,
              this.loginData.token,
              this.returnUrl || '/',
            )
          } catch (error) {
            this.handleLogout()
            this.error = error
          }
        }
      })
    },
    async handleLogout() {
      this.deleteToken()
      this.$store.commit('setLogin', { name: '', tenant: '' })
      this.$router.push('/login')
    },
    deleteToken() {
      Util.deleteCookie('.Platypus.Auth')
    },
  },
}
</script>

<style lang="scss" scoped>
.parent {
  position: relative;
  height: 600px;
}

.parent p {
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  -webkit-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
  width: 500px;
  text-align: left;
}

.image {
  text-align: center;
  padding-bottom: 30px;
}

.error-message {
  text-align: center;
  padding-bottom: 30px;
}

.button-group {
  text-align: center;
  padding-top: 10px;
}

.el-button--primary {
  background-color: #1abfd5;
  border-color: #1abfd5;
  box-shadow: 0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12),
    0 3px 1px -2px rgba(0, 0, 0, 0.2); /*影*/
  -webkit-tap-highlight-color: transparent;
  transition: 0.3s ease-out; /*変化を緩やかに*/
}

.el-button--primary:hover {
  box-shadow: 0 3px 3px 0 rgba(0, 0, 0, 0.14), 0 1px 7px 0 rgba(0, 0, 0, 0.12),
    0 3px 1px -1px rgba(0, 0, 0, 0.2); /*浮き上がるように*/
}
</style>
