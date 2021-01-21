<template>
  <div>
    <h2>新しくモデルをトレーニングする</h2>
    <h3>テンプレートを選択しAIを作成します</h3>
    <div class="model-template">
      <!-- TODO 検索ボタンデザイン -->
      <el-row :gutter="20">
        <el-col class="search">
          <kqi-smart-search-input
            v-model="searchCondition"
            style="width:300px"
            :configs="searchConfigs"
            @search="search"
          />
        </el-col>
      </el-row>

      <!-- TODO
        登録したテンプレートをカード形式で表示 -->
      <div class="dashboard">
        <div
          v-for="(template, index) in templates"
          :key="index"
          class="card-container"
        >
          <router-link to="/aquarium/experiment/createStep">
            <el-card
              class="template"
              style="border: solid 1px #ebeef5; width: 360px; height: 300px;"
            >
              <div class="template-name">
                {{ template.name }}
              </div>

              <div
                class="template-description"
                style="padding: 10px; font-size: 14px;"
              >
                {{ template.memo }}
              </div>
              <!-- タグを想定 -->
              <div
                class="template-description"
                style="padding: 20px; font-size: 18px;text-align:center;"
              >
                <div style="padding:20px">
                  <el-button type="primary">Pytorch</el-button>
                </div>
                <div>
                  <el-tag class="tag"> {{ template.tag }}</el-tag>
                </div>
              </div>
            </el-card>
          </router-link>
        </div>
      </div>
      <router-view />
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
// TODO template API に変更
const { mapGetters, mapActions } = createNamespacedHelpers('preprocessing')

export default {
  title: 'モデルテンプレート',
  components: {
    KqiSmartSearchInput,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      unwatchLogin: undefined,
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'テンプレート名', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
      ],
      templates: [
        //TODO 後で消す
        { name: 'A工場●●分類', memo: '説明文説明文', tag: 'Classification' },
        {
          name: 'A部署異常検知前処理',
          memo: '事前学習済みXXXモデルを使用',
          tag: 'Classification',
        },
        {
          name: 'D工場X画像前処理',
          memo: '説明文説明文',
          tag: 'Classification',
        },
        {
          name: '●●部署●●セグメント',
          memo: '説明文説明文',
          tag: 'Classification',
        },
      ],
    }
  },
  computed: {
    ...mapGetters({
      dataSets: ['dataSet/dataSets'],
      registries: ['registrySelector/registries'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
      images: ['registrySelector/images'],
      tags: ['registrySelector/tags'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      repositories: ['gitSelector/repositories'],
      branches: ['gitSelector/branches'],
      commits: ['gitSelector/commits'],
      commitDetail: ['gitSelector/commitDetail'],
      loadingRepositories: ['gitSelector/loadingRepositories'],
      detail: ['experiment/detail'],
    }),
  },
  async created() {
    await this.initialize()
  },

  methods: {
    ...mapActions(
      'fetchDetail',
      'postHalt',
      'postUserCancel',
      'postFiles',
      'put',
      'delete',
    ),

    async retrieveData() {
      await this.fetchDetail(this.id)
      await this.fetchUploadedFiles(this.detail.id)

      if (
        this.detail.statusType === 'Running' ||
        this.detail.statusType === 'Error'
      ) {
        await this.fetchEvents(this.detail.id)
      }
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
    openEditDialog() {
      this.$router.push('/aquarium/experiment/edit/')
    },
    openCreateDialog() {
      this.$router.push('/aquarium/experiment/create')
    },
    closeDialog() {
      this.$router.push('/aquarium/experiment')
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
  padding: 30px 10px;
  font-size: 24px;
  border-bottom: 1px solid #ebeef5;
  text-align: center;
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
.tag {
  border-radius: 15px;
}
</style>
