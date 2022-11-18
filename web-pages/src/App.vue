<template>
  <div id="app">
    <el-container>
      <el-header>
        <kqi-header @menu="handleMenu" />
      </el-header>
      <el-container class="main-container">
        <kqi-menu v-show="menu" class="sidenav" />
        <el-main>
          <router-view class="content" :class="{ 'content-hidden': !menu }" />
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiHeader from '@/components/KqiHeader.vue'
import KqiMenu from '@/components/KqiMenu.vue'
import Util from '@/util/util'

export default Vue.extend({
  components: {
    KqiHeader,
    KqiMenu,
  },
  data() {
    return {
      menu: this.getMenu(),
    }
  },
  watch: {
    menu() {
      this.setMenu(this.menu)
    },
  },
  created() {
    window.addEventListener('beforeunload', this.setCookieToken)
  },
  destroyed() {
    window.removeEventListener('beforeunload', this.setCookieToken)
  },
  methods: {
    async handleMenu() {
      this.menu = !this.menu
    },
    setMenu(showFlg) {
      Util.setCookie('.Platypus.ShowMenu', showFlg)
    },
    getMenu() {
      let v = Util.getCookie('.Platypus.ShowMenu')
      return v === 'true'
    },
    setCookieToken() {
      let token = this.$store.getters['account/token']
      let cookieTokenKey = '.Platypus.Auth'
      let cookieToken = Util.getCookie(cookieTokenKey, token)

      if (!cookieToken) {
        // Cookieにトークン情報がないときはログアウト
        this.$store.dispatch['account/logout']
      } else {
        // Cookieにトークン情報があるときはCookie情報を更新
        Util.setCookie(cookieTokenKey, token)
      }
    },
  },
})
</script>

<style lang="scss" scoped>
#app {
  color: #2c3e50;

  $header-height: 50px;
  $menu-back-color: #fafafa;

  .el-header {
    height: $header-height !important;
    position: fixed;
    width: 100%;
    z-index: 2;
    margin-top: 0%;
    top: 0px;
    left: 0px;
    background-color: #1abfd5;
    border-bottom: 1px solid #cccccc;

    background: linear-gradient(
      to bottom,
      rgba(26, 191, 213, 1) 0%,
      rgba(26, 191, 213, 1) 30%,
      rgba(26, 191, 213, 0.7) 100%
    );
    filter: alpha(opacity=90);
    -moz-opacity: 0.9;
    opacity: 0.9;
  }

  .sidenav {
    margin-top: $header-height;
    height: 100%;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
    overflow-x: hidden;
    background-color: $menu-back-color;
    box-shadow: inset -2px 0 0 #e5e5e5;
  }
  @media screen and (max-width: 1000px) {
    .sidenav {
      width: 65px;
    }
  }
  @media screen and (min-width: 1000px) {
    .sidenav {
      width: 200px;
    }
  }

  .el-main {
    padding-top: $header-height;
    height: 100vh;
  }

  // Components Common style
  .el-transfer-panel {
    width: 40% !important;
  }

  @media screen and (max-width: 1000px) {
    .content {
      margin-left: 65px;
    }
  }
  @media screen and (min-width: 1000px) {
    .content {
      margin-left: 200px;
    }
  }
}

.content-hidden {
  margin-left: 0px !important;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */
 {
  opacity: 0;
}
</style>

<style src="./reset.css" />
<style src="./style.css" />
