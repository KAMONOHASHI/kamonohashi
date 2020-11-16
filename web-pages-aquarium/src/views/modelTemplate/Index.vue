<template>
  <div>
    <h2>モデルテンプレート</h2>
    <h3>Deep Learningのソースコードを閲覧できます</h3>
    <div class="model-template">
      <div>
        <!-- TODO
        登録したテンプレートをカード形式で表示 -->
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
      unwatchLogin: undefined,
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

.model-template {
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
