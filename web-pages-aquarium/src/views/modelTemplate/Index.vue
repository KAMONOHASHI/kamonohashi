<template>
  <div>
    <h2>モデルテンプレート</h2>
    <h3>Deep Learningのソースコードを閲覧できます</h3>
    <div class="model-template">
      <!-- TODO 検索ボタンデザイン -->
      <el-row :gutter="20">
        <el-col class="search">
          <kqi-smart-search-input
            v-model="searchCondition"
            :configs="searchConfigs"
            @search="search"
          />
        </el-col>
      </el-row>
      <!-- TODO新規登録ボタンデザイン -->
      <el-row type="flex" justify="space-between" :gutter="20">
        <el-col class="right-top-button" :span="2">
          <el-button
            icon="el-icon-edit-outline"
            type="primary"
            plain
            @click="openCreateDialog()"
          >
            モデルテンプレート登録
          </el-button>
        </el-col>
      </el-row>

      <!-- TODO
        登録したテンプレートをカード形式で表示 -->
      <div class="dashboard">
        <div
          v-for="(menu, index) in menuList"
          :key="index"
          class="card-container"
        >
          <!-- <router-link :to="menu.url"> -->
          <el-card
            class="template"
            style="border: solid 1px #ebeef5; width: 360px; height: 200px;"
            @click="openEditDialog"
          >
            <div class="template-name">
              {{ menu.name }}
            </div>
            <div
              class="template-description"
              style="padding: 10px; font-size: 14px;"
            >
              {{ menu.description }}
            </div>
          </el-card>
          <!-- </router-link> -->
          <router-view
            @done="done"
            @cancel="closeDialog"
            @close="closeDialog"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
// TODO template API に変更
const { mapGetters, mapActions } = createNamespacedHelpers('account')

export default {
  title: 'モデルテンプレート',
  components: {
    KqiSmartSearchInput,
  },
  data() {
    return {
      unwatchLogin: undefined,
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'テンプレート名', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
      ],
    }
  },
  computed: {
    // TODO template API に変更
    ...mapGetters(['menuList']),
  },
  async created() {
    // TODO template API に変更
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
    // TODO template API に変更
    ...mapActions(['fetchMenuList']),
    async watchLogin(tenant) {
      if (tenant) {
        await this.fetchMenuList()
      }
    },
    async retrieveData() {
      let params = this.searchCondition
      await this.fetchMenuList(params)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
    async done(type) {
      if (type === 'delete') {
        // 削除時、表示していたページにデータが無くなっている可能性がある。
        // 総数 % ページサイズ === 1の時、残り1の状態で削除したため、currentPageが1で無ければ1つ前のページに戻す
        if (this.total % this.pageStatus.currentPageSize === 1) {
          if (this.pageStatus.currentPage !== 1) {
            this.pageStatus.currentPage -= 1
          }
        }
      }
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    openEditDialog(selectedTemplate) {
      this.$router.push('/aquarium/model-template/edit/' + selectedTemplate.id)
    },
    openCreateDialog() {
      this.$router.push('/aquarium/model-template/create')
    },
    closeDialog() {
      this.$router.push('/aquarium/model-template')
    },
  },
}
</script>

<style lang="scss" scoped>
a {
  text-decoration: none;
}

.template {
  &:hover {
    transform: scale(1.05);
  }
}

.template-name {
  font-weight: bold;
  padding: 10px;
  font-size: 20px;
}

.template-description {
  font-weight: lighter;
}

.model-template {
  padding-top: 80px;
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
</style>
