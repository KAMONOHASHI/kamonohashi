<template>
  <div>
    <div v-show="trees">
      <el-menu class="el-menu-vertical-demo" :collapse="isCollapse" :unique-opened="true" :default-active="activeIndex">
        <div v-for="(menu, index) in trees" :key="index">
          <el-submenu v-if="menu.children" :index="String(index)">
            <template slot="title">
              <icon v-if="menu.category" class="icon" :name="menu.category" scale="1.5"></icon>
              <span slot="title">{{ menu.label }}</span>
            </template>
            <el-menu-item v-for="(sub, index) in menu.children" :key="index" :index="sub.url"
                          @click="handleClick(sub.url)" :style="disableActive">
              <icon v-if="sub.category" class="icon" :name="sub.category" scale="1.5"></icon>
              <span slot="title">{{ sub.label }}</span>
            </el-menu-item>
          </el-submenu>
          <el-menu-item v-else :index="menu.url" @click="handleClick(menu.url)" :style="disableActive">
            <icon v-if="menu.category" class="icon" :name="menu.category" scale="1.5"></icon>
            <span slot="title">{{ menu.label }}</span>
          </el-menu-item>
        </div>
        <el-menu-item index="/version" @click="handleClick('/version')" :style="disableActive">
          <i class="el-icon-info"></i>
          <span>バージョン情報</span>
        </el-menu-item>
      </el-menu>
    </div>
    <div v-show="!trees">
      <el-menu class="el-menu-vertical-demo" :collapse="isCollapse">
        <el-menu-item index="/version" @click="handleClick('/version')" :style="disableActive">
          <i class="el-icon-info"></i>
          <span>バージョン情報</span>
        </el-menu-item>
      </el-menu>
    </div>

    <div style="background-color: transparent; height: 200px; width: 1px;">
      <!--スクロール調整-->
    </div>

  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'Menu',

    data () {
      return {
        isCollapse: false,
        menus: null,
        trees: null,
        activeIndex: null,
        disableActive: null
      }
    },

    watch: {
      '$route' (to, from) {
        this.activeIndex = this.$route.path

        // ダッシュボード画面にアクセスした際には、cssを上書きすることでハイライトを削除
        if (to.path === '/') {
          this.disableActive = {
            color: '#303133 !important',
            border: '0px !important'
          }
        } else {
          this.disableActive = null
        }
      }
    },

    async beforeMount () {
    },
    async created () {
      this.$store.watch(this.$store.getters.getLoginTenant, this.watchLogin)
      this.activeIndex = this.$route.path
    },
    async mounted () {
      window.addEventListener('load', this.handleResize)
      window.addEventListener('resize', this.handleResize)
    },
    beforeDestroy: function () {
      window.removeEventListener('resize', this.handleResize)
      window.removeEventListener('load', this.handleResize)
    },

    methods: {
      async handleClick (url) {
        if (url) {
          this.$router.push(url)
        }
      },

      async watchLogin (tenant) {
        if (tenant) {
          let res = await api.account.getTreeMenus()
          this.trees = res.data
        } else {
          this.trees = null
        }
      },

      handleResize (event) {
        if (event.currentTarget.innerWidth < 1000) {
          this.isCollapse = true
        } else {
          this.isCollapse = false
        }
      }

    }
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
    color: #409EFF;
    border-left: 5px solid #409EFF;
  }

  .el-icon-info {
    margin-right: 10px;
  }

  .icon {
    color: #666666;
    width: 24px;
    height: 24px;
    margin-right: 10px;
  }
</style>
