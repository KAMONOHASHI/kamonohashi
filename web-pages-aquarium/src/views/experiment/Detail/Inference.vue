<template>
  <div>
    <h2>推論一覧</h2>
    作成したAIの評価の履歴一覧です
    <el-row>
      <el-col :span="18">
        <el-table :data="inferenceList" style="width: 100%">
          <el-table-column prop="id" label="ID" width="180"> </el-table-column>
          <el-table-column prop="data" label="データ" width="180">
          </el-table-column>
          <el-table-column prop="result" label="結果"> </el-table-column>
        </el-table>
      </el-col>
    </el-row>
    <el-row>
      <el-col> </el-col>
      <el-button type="primary" style="margin-top:20px" @click="submit">
        別のデータで推論を実行
      </el-button>
    </el-row>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: '推論',
  components: {},
  data() {
    return {
      importfile: null,
      inferenceList: [
        { id: '1', data: '製造所A部品データ1', result: '成功' },
        { id: '2', data: '製造所A部品B', result: '実行中' },
        { id: '3', data: '製造所A部品データ2', result: 'エラー' },
        { id: '4', data: '製造所A部品データ3', result: 'エラー' },
      ],
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

    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done() {
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
