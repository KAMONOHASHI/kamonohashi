<template>
  <div>
    <h2>アクアリウムダッシュボード</h2>
    <div>データの準備を行う</div>
    <div class="aqarium-dashboard">
      <div
        v-for="(menu, index) in dataset"
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
    <div>実験を行う</div>
    <div class="aqarium-dashboard">
      <div
        v-for="(menu, index) in training"
        :key="'i' + index"
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
      <div
        v-for="(menu, index) in validation"
        :key="'i' + index"
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

    <div>テンプレートを管理する</div>
    <div class="aqarium-dashboard">
      <div
        v-for="(menu, index) in template"
        :key="'t' + index"
        class="card-container"
      >
        <router-link :to="menu.url">
          <el-card
            class="menu"
            style="border: solid 1px #ebeef5; width: 400px; height: 200px; "
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
      dataset: [
        {
          category: '',
          name: 'データセット',
          url: '/aquarium/dataset',
          description: 'データアップロード、データセット作成を行います。',
        },
      ],
      training: [
        {
          category: '',
          name: '学習',
          url: '/aquarium/experiment/template',
          description: 'テンプレートを選択し目的に応じたAIを作成します。',
        },
      ],
      validation: [
        {
          category: '',
          name: '評価',
          url: '/aquarium/experiment',
          description: '作成したAIの精度を確認し評価します。',
        },
      ],
      template: [
        {
          category: '',
          name: 'テンプレート管理',
          url: '/aquarium/model-template',
          description: 'テンプレートの閲覧、編集、新規登録をします。',
        },
      ],
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
