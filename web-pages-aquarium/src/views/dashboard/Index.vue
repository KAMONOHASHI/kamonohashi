<template>
  <div>
    <h2>アクアリウムダッシュボード</h2>
    <div v-for="(subMenuList, subIndex) in allMenues" :key="'sub' + subIndex">
      <div v-if="subMenuList.menus.length > 0">
        <div>
          {{ subMenuList.description }}
        </div>
        <div class="aqarium-dashboard">
          <div
            v-for="(menu, index) in subMenuList.menus"
            :key="'m' + index"
            class="card-container"
          >
            <router-link :to="menu.url">
              <el-card
                class="menu"
                style="border: solid 1px #ebeef5; width: 400px; height: 200px;"
              >
                <div class="menu-name">
                  {{ menu.name }}
                </div>
                <div
                  class="menu-description"
                  style="padding: 10px; font-size: 14px;"
                >
                  {{ menu.description }}
                </div>
                <div class="menu-start" style="padding: 20px; font-size: 14px;">
                  <icon :name="iconname" scale="1.5" class="menu-icon" />
                  開始
                </div>
              </el-card>
            </router-link>
          </div>
        </div>
      </div>
    </div>
    <div class="footer">
      <span class="footer-content">
        © 2016-2020 NS Solutions Corporation, All Rights Reserved.
      </span>
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('account')

export default {
  title: 'ダッシュボード',
  data() {
    return {
      iconname: 'pl-arrow-right',
      unwatchLogin: undefined,
      allMenues: [
        { description: 'データの準備を行う', menus: [] },
        { description: '実験を行う', menus: [] },
        { description: 'テンプレートを管理する', menus: [] },
      ],
    }
  },
  computed: {
    ...mapGetters(['menuList']),
  },
  async created() {
    this.unwatchLogin = this.$store.watch(
      this.$store.getters.getLoginTenant,
      this.watchLogin,
    )
    await this.fetchMenuList()
    await this.setSubMenues()
  },

  async beforeDestroy() {
    this.unwatchLogin()
  },

  methods: {
    ...mapActions(['fetchMenuList']),
    async watchLogin(tenant) {
      if (tenant) {
        await this.fetchMenuList()
      }
    },
    async setSubMenues() {
      this.allMenues[0].menus = this.menuList.filter(menu => {
        return menu.category === 'aq-dataset'
      })

      this.allMenues[1].menus = this.menuList.filter(menu => {
        return menu.category.startsWith('aq-experiment')
      })

      this.allMenues[2].menus = this.menuList.filter(menu => {
        return menu.category === 'aq-template'
      })
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
.menu-start {
  font-weight: lighter;

  position: absolute;
  bottom: 0;

  align-content: flex-end;
}
.aqarium-dashboard {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-content: flex-start;
  margin-bottom: 40px;
}

.card-container {
  float: left;
  margin: 20px 20px 10px 0;
  position: relative;
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
