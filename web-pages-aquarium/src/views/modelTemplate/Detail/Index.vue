<template>
  <div>
    <h2>
      テンプレート詳細＞（テンプレート名）
      <el-tag type="info" style="border-radius:15px">Version 1</el-tag>
    </h2>

    <el-tabs v-model="activeName" @tab-click="handleClick">
      <el-tab-pane label="基本設定" name="baseSetting">
        <base-setting :datail="detail" />
      </el-tab-pane>
      <el-tab-pane label="前処理" name="preprocessing">
        <preprocessing v-if="preprocForm" v-model="preprocForm" />
      </el-tab-pane>
      <el-tab-pane label="学習と推論" name="train">
        <training :datail="detail" />
      </el-tab-pane>
    </el-tabs>
    <router-view @cancel="closeDialog" @done="done" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import BaseSetting from './BaseSetting'
import Preprocessing from './Preprocessing'
import Training from './Training'
const { mapGetters, mapActions } = createNamespacedHelpers('template')

export default {
  title: 'モデルテンプレート',
  components: { BaseSetting, Preprocessing, Training },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'type', name: '種類', type: 'text' },
        { prop: 'totalImageNumber', name: 'イメージの総数', type: 'text' },
        {
          prop: 'labeledImageNumber',
          name: 'ラベル付きのイメージ数',
          type: 'text',
        },
        { prop: 'lastModified', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
      ],
      tableData: [],
      activeName: 'baseSetting',
      preprocForm: null,
    }
  },
  computed: {
    ...mapGetters(['detail', 'total']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDetail', 'delete']),
    handleClick() {},
    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      await this.fetchDetail(this.id)
      console.log('index', this.detail)
      this.preprocForm = {
        containerImage: { ...this.detail.preprocessContainerImage },
        gitModel: { ...this.detail.preprocessGitModel },
        entryPoint: this.detail.preprocessEntryPoint,
      }
    },
    closeDialog() {
      this.$router.push('/aquarium/model-template')
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

    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
