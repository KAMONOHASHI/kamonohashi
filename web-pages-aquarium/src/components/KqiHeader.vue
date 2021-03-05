title
<template>
  <div class="header">
    <el-row justify="center">
      <el-col :span="12" class="title">
        <el-button type="text" class="menu label-color" @click="handleMenu">
          ≡
        </el-button>
        <router-link to="/">
          <img class="logo" src="@/assets/logo_aquarium.png" alt="" />
        </router-link>
      </el-col>
      <el-col :span="12" class="user">
        <el-dropdown v-if="user" trigger="click" @command="handleSwitchTenant">
          <span class="el-dropdown-link user-label">
            <icon
              name="user"
              scale="1.4"
              class="user-label"
              style="position: relative; top: 7px; left: -8px;"
            />
            {{ user.userName }} / {{ user.selectedTenant.displayName }}
            <i class="el-icon-caret-bottom" />
          </span>
          <el-dropdown-menu
            slot="dropdown"
            :class="{ scroll: user.tenants.length > 10 }"
          >
            <el-dropdown-item
              v-for="(tenant, index) in user.tenants"
              :key="index"
              :command="tenant.id"
              :class="{
                activeTenant: user.selectedTenant.id === tenant.id,
              }"
            >
              {{ tenant.displayName }}
            </el-dropdown-item>
            <hr />
            <el-dropdown-item key="@setting" command="@setting">
              ユーザ情報設定
            </el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
        <el-button
          style="padding-left: 15px;"
          type="text"
          class="user-label"
          @click="handleLogin"
        >
          ログアウト
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('account')

export default {
  name: 'Header',
  props: {
    login: {
      type: Boolean,
      default: true,
    },
  },
  data() {
    return {
      user: null,
    }
  },
  computed: {
    ...mapGetters(['account', 'loginData']),
  },
  async created() {
    this.$store.watch(this.$store.getters.getLoginTenant, this.watchLogin)
  },
  methods: {
    ...mapActions(['fetchAccount', 'postTokenTenants']),
    async watchLogin(tenant) {
      if (tenant) {
        await this.fetchAccount()
        this.user = this.account
        if (this.user.userName.length > 25) {
          this.user.userName = this.user.userName.substr(0, 25) + '...'
        }
        if (this.user.selectedTenant.displayName.length > 25) {
          this.user.selectedTenant.displayName =
            this.user.selectedTenant.displayName.substr(0, 25) + '...'
        }
      } else {
        this.user = null
      }
    },
    async handleSwitchTenant(tenant) {
      if (tenant === '@setting') {
        this.$router.push('/setting')
      } else {
        let params = {
          tenantId: tenant,
        }
        await this.postTokenTenants(params)
        this.$emit(
          'login',
          this.loginData.userName,
          this.loginData.tenantId,
          this.loginData.token,
          '/',
        )
      }
    },
    async handleLogin() {
      this.$emit('logout')
    },
    async handleMenu() {
      this.$emit('menu')
    },
  },
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

.scroll {
  height: 425px;
  overflow-y: scroll;
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
