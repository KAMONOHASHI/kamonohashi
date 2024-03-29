<template>
  <div>
    <el-menu
      class="el-menu-vertical-demo"
      :collapse="isCollapse"
      :unique-opened="true"
      :default-active="activeIndex"
    >
      <div>
        <el-menu-item index="/" @click="handleClick('/')">
          <icon class="icon" name="aq-dashboard" scale="1" />
          <span>ダッシュボード</span>
        </el-menu-item>
      </div>
      <div v-for="(menu, index) in menuTree" :key="index">
        <el-submenu v-if="menu.children" :index="String(index)">
          <template slot="title">
            <icon
              v-if="menu.category"
              class="icon"
              :name="menu.category"
              scale="1.5"
            />
            <span slot="title">{{ menu.label }}</span>
          </template>
          <el-menu-item
            v-for="item in menu.children"
            :key="item.label"
            :index="item.url"
            @click="handleClick(item.url)"
          >
            <icon
              v-if="item.category"
              class="icon"
              :name="item.category"
              scale="1.5"
            />
            <span slot="title">{{ item.label }}</span>
          </el-menu-item>
        </el-submenu>
        <el-menu-item v-else :index="menu.url" @click="handleClick(menu.url)">
          <icon
            v-if="menu.category"
            class="icon"
            :name="menu.category"
            scale="1.5"
          />
          <span slot="title">{{ menu.label }}</span>
        </el-menu-item>
      </div>
      <el-menu-item index="/version" @click="handleClick('/version')">
        <i class="el-icon-info" />
        <span>バージョン情報</span>
      </el-menu-item>
    </el-menu>

    <!--スクロール調整-->
    <div style="background-color: transparent; height: 200px; width: 1px;" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('account')

export default {
  data() {
    return {
      isCollapse: false,
      activeIndex: null,
    }
  },
  computed: {
    ...mapGetters(['menuTree']),
  },
  watch: {
    $route() {
      this.setActiveIndex()
    },
  },

  created() {
    this.activeIndex = this.$route.path
    this.setActiveIndex()
  },
  mounted() {
    window.addEventListener('load', this.handleResize)
    window.addEventListener('resize', this.handleResize)
  },
  beforeDestroy: function() {
    window.removeEventListener('resize', this.handleResize)
    window.removeEventListener('load', this.handleResize)
  },

  methods: {
    ...mapActions(['fetchMenuTree']),
    setActiveIndex() {
      // ["", "cluster-resource", "tenant"]等のメニュー項目を取得し設定。例：/training, /cluster-resource
      this.activeIndex = `/${this.$route.path.split('/')[1]}`
      if (this.activeIndex === '/preprocessingHistory') {
        this.activeIndex = '/preprocessing'
      }

      // テナント管理系の場合、/manage/tenant等の指定が必要であるため追記
      if (this.activeIndex === '/manage') {
        this.activeIndex += `/${this.$route.path.split('/')[2]}`
      }
    },
    async handleClick(url) {
      if (url) {
        this.$router.push(url)
      }
    },

    handleResize(event) {
      if (event.currentTarget.innerWidth < 1000) {
        this.isCollapse = true
      } else {
        this.isCollapse = false
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 50px;
}

@media screen and (max-width: 1000px) {
  .el-menu-vertical-demo /deep/ span {
    display: none;
  }
}
.icon {
  color: #666666;
  width: 20px;
  height: 20px;
  margin-right: 10px;
  margin-bottom: -5px;
}

/deep/ .el-dropdown-menu__item--divided:before,
/deep/ .el-menu,
/deep/ .el-menu-item:focus,
/deep/ .el-menu-item:hover,
/deep/ .el-menu--horizontal > .el-menu-item:not(.is-disabled):focus,
/deep/ .el-menu--horizontal > .el-menu-item:not(.is-disabled):hover,
/deep/ .el-menu--horizontal > .el-submenu .el-submenu__title:hover,
/deep/ .el-submenu__title:hover {
  background-color: inherit;
  box-shadow: inherit;
}

.el-menu-item.is-active {
  background-color: transparent;
  color: #409eff;
  border-left: 5px solid #409eff;
}
</style>
