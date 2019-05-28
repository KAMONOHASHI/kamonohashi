title
<template>
  <div class="header">
    <el-row justify="center">
      <el-col :span="12" class="title">
        <el-button type="text" @click="handleMenu" class="menu label-color">≡</el-button>
        <router-link to="/">
          <img class="logo" src="../../static/images/KAMONOHASHI_logo_white.png" alt="">
        </router-link>
      </el-col>
      <el-col :span="12" class="user">
        <el-dropdown trigger="click" @command="handleSwitchTenant" v-if="user">
          <span class="el-dropdown-link user-label">
            <icon name="user" scale="1.4" class="user-label" style="position: relative; top: 7px; left: -8px;"></icon>
            {{user.userName}} / {{user.selectedTenant.displayName}}
            <i class="el-icon-caret-bottom"></i>
          </span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item
              v-for="(tenant, index) in user.tenants"
              :command="tenant.id"
              :key="index"
              :class="{ activeTenant: user.selectedTenant.id === tenant.id }">
              {{tenant.displayName}}
            </el-dropdown-item>
            <hr/>
            <el-dropdown-item key="@setting" command="@setting">
              {{$t('userSetting')}}
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
        <el-button style="padding-left:15px;" type="text" @click="handleLogin" class="user-label">{{$t('logout')}}
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'Header',
    props: ['login'],
    data: function () {
      return {
        user: null
      }
    },
    async created () {
      this.$store.watch(this.$store.getters.getLoginTenant, this.watchLogin)
    },
    async beforeMount () {
    },
    methods: {
      async watchLogin (tenant) {
        if (tenant) {
          let res = await api.account.get()
          this.user = res.data
        } else {
          this.user = null
        }
      },
      async handleSwitchTenant (tenant) {
        if (tenant === '@setting') {
          this.$router.push('/setting')
        } else {
          let params = {
            tenantId: tenant
          }
          let res = await api.account.postTokenTenants(params)
          let data = res.data
          this.$emit('login', data.userName, data.tenantId, data.token, '/')
        }
      },
      async handleLogin () {
        this.$emit('logout')
      },
      async handleMenu () {
        this.$emit('menu')
      }
    },
    i18n: {
      messages: {
        en: {
          manual: 'Manual',
          logout: 'Sign out',
          userSetting: 'Account Setting'
        },
        ja: {
          manual: 'マニュアル',
          logout: 'ログアウト',
          userSetting: 'ユーザ情報設定'
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .header {
    height: inherit;
  }

  .el-row {
    height: inherit;
    .el-col {
      height: inherit;
    }
  }

  .title {
    vertical-align: middle;
    text-align: left;
  }

  .user {
    position: absolute;
    top: 4px;
    right: 10px;
    text-align: right;
  }

  .menu {
    margin: 0px;
    padding: 0px;
    position: absolute;
    top: 4px;
    left: -10px;
    font-size: 38px;
    color: white;

    &:hover {
      color: white;
    }
    &:focus {
      color: white;
    }
  }

  .logo {
    position: absolute;
    left: 30px;
    top: 10px;
    height: 30px;
    padding-left: 0px;
  }

  .manual {
    position: absolute;
    left: 180px;
    top: 13px;
    font-size: 14px;
    color: white;
    text-decoration: none;
  }

  .activeTenant {
    font-weight: bold;
  }

  .user-label {
    color: white;
    cursor: pointer;

    &:hover {
      color: white;
    }
    &:focus {
      color: white;
    }
  }
</style>
