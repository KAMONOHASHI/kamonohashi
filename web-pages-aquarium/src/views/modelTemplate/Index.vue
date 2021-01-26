<template>
  <div>
    <h2>モデルテンプレート</h2>
    <h3>AI作成のためのテンプレートを登録できます</h3>
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
          v-for="(template, index) in templates"
          :key="index"
          class="card-container"
        >
          <!-- TODO /aquarium/model-template/<templateid>に飛ぶようにする -->
          <router-link to="/aquarium/model-template/1">
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
      <router-view @cancel="closeDialog" />
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
// TODO template API に変更
const { mapGetters, mapActions } = createNamespacedHelpers('template')

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
      preprocessings: [
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
    // TODO template API に変更
    ...mapGetters(['templates']),
  },
  async created() {
    // TODO template API に変更
    await this.retrieveData()
  },

  methods: {
    // TODO template API に変更
    ...mapActions(['fetchModelTemplates']),

    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchModelTemplates(params)
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
    // openEditDialog(selectedTemplate) {
    //   this.$router.push('/aquarium/model-template/edit/' + selectedTemplate.id)
    // },
    // TODO テンプレートIDを引数にとってページ遷移
    openEditDialog() {
      this.$router.push('/aquarium/model-template/edit/')
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
