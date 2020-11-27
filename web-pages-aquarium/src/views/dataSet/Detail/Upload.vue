<template>
  <div>
    <h2>インポートするファイルの選択</h2>
    <div style="width:600px;padding-top:20px;padding-bottom:40px">
      カスタムモデルを作成するには、最初に一連の画像をインポートしてトレーニングする必要があります。各画像はラベルで分類する必要があります（ラベルは画像を特定する方法をモデルに伝えるために不可欠です）
    </div>
    <el-radio v-model="importfile" label="1" style="display:block;padding:10px">
      パソコンから画像をアップロード
    </el-radio>
    <el-radio v-model="importfile" label="2" style="display:block;padding:10px">
      KAMONOHASHIからデータセットを選択
    </el-radio>
    <div v-if="importfile == 1" class="importfile-detail">
      <h3>パソコンから画像をアップロード</h3>
      任意のファイル形式がアップロードできます。画像の表示はJPG,PNG,ZIPがサポートされています。<br />
      1回のアップロードでさいだいXXXファイル送信できます。アップロードしたファイルはKAMONOHASHIのデータ、データセットに保存されます。
      <el-button
        type="plain"
        plain
        @click="openDialog()"
        style="display:block;margin-top:10px"
      >
        ファイルを選択
      </el-button>
    </div>
    <div v-else-if="importfile == 2" class="importfile-detail">
      <h3>KAMONOHASHIからデータセットを選択</h3>
      <el-button
        type="plain"
        plain
        @click="openDialog()"
        style="display:block;margin-top:10px"
      >
        データを選択
      </el-button>
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: 'データセット',
  components: {},
  data() {
    return {
      importfile: null,
    }
  },
  computed: {
    ...mapGetters(['dataSets', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDataSets']),

    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
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
    openCreateDialog() {
      this.$router.push('/dataset/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/dataset/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.importfile-detail {
  padding-top: 50px;
}
.importfile-detail > h3 {
  padding-bottom: 10px;
}
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
