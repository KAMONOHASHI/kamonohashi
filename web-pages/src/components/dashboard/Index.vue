<template>
  <div>
    <h2>ダッシュボード</h2>
    <div class="dashboard">
      <div
        v-for="(menu, index) in menuList"
        :key="index"
        class="dashboard-menu-list card-container"
      >
        <router-link :to="menu.url">
          <el-card
            class="menu"
            style="border:solid 1px #ebeef5; width:360px;height: 200px; border-left: 5px solid rgba(26, 191, 213, 0.7);"
          >
            <div class="menu-name">
              <icon
                v-if="menu.category"
                :name="menu.category"
                scale="1.5"
                class="menu-icon"
              ></icon>
              {{ menu.name }}
            </div>
            <div class="menu-description" style="padding:10px;font-size:14px;">
              {{ menu.description }}
            </div>
          </el-card>
        </router-link>
      </div>
    </div>
    <div class="footer">
      <span class="footer-content"
        >© 2016-2019 NS Solutions Corporation, All Rights Reserved.</span
      >
    </div>
  </div>
</template>
<script>
import api from '@/api/v1/api'

export default {
  name: 'Dashboard',
  title: 'Dashboard',

  props: {
    selectedMenu: String,
  },
  data() {
    return {
      menuList: [],
      unwatchLogin: undefined,
    }
  },

  async created() {
    this.unwatchLogin = this.$store.watch(
      this.$store.getters.getLoginTenant,
      this.watchLogin,
    )
    await this.init()
  },

  async beforeDestroy() {
    this.unwatchLogin()
  },

  methods: {
    async init() {
      let response = await api.menuList.getMenuList()
      this.menuList = response.data
    },
    async watchLogin(tenant) {
      if (tenant) {
        let response = await api.menuList.getMenuList()
        this.menuList = response.data
      }
    },
  },
}
</script>
<style lang="scss" scoped>
a {
  text-decoration: none;
}

.menu {
  &:hover {
    transform: scale(1.05);
  }
}

.menu-icon {
  position: relative;
  top: 5px;
  left: -5px;
}

.menu-name {
  font-weight: bold;
  padding: 10px;
  font-size: 20px;
}

.menu-description {
  font-weight: lighter;
}

.dashboard {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: space-evenly;
  align-content: flex-start;
}

.card-container {
  float: left;
  margin: 20px 20px 10px 0;
}

.footer {
  display: flex;
  justify-content: center;
}

.footer-content {
  position: fixed;
  bottom: 0;
  padding: 10px;
}
</style>
