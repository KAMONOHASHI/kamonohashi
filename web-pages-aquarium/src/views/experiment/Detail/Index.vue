<template>
  <div>
    <h2>（実験名）</h2>
    <el-tabs v-model="activeName" @tab-click="handleClick">
      <el-tab-pane label="実行情報" name="info"> <info /> </el-tab-pane>
      <el-tab-pane label="実行結果" name="result">
        <result />
      </el-tab-pane>
      <el-tab-pane label="推論" name="inference"><inference /></el-tab-pane>
    </el-tabs>
    <router-view @done="done" @copy="handleCopy" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import Info from './Info'
import Result from './Result'

import Inference from './Inference'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: '実験',
  components: { Info, Result, Inference },
  data() {
    return {
      iconname: 'pl-plus',

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
      activeName: 'info',
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

      params.withTotal = true
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
      this.$router.push('/aqarium/experiment/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/aqarium/experiment/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/aqarium/experiment/create/' + id)
    },
    async search() {
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
