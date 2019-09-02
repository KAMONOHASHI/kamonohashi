<template>
  <div>

    <h2> ノード管理 </h2>
    <el-row type="flex" justify="space-between" :gutter="20">

      <el-col class="pagination" :span="16">
        <el-pagination layout="total, sizes, prev, pager, next"
                       :total="total"
                       @size-change="handleSizeChange"
                       @current-change="currentChange"
                       :current-page="currentPage"
                       :page-size="currentPageSize"
                       :page-sizes="[10, 30, 50, 100, 200]"
        />
      </el-col>
      <el-col class="right-top-button" :span="8">
        <el-button @click="openCreateDialog" icon="el-icon-edit-outline" type="primary" plain>
          新規登録
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
        <el-table-column prop="id" label="ID" width="100px"/>
        <el-table-column prop="name" label="ノード名" width="200px"/>
        <el-table-column prop="partition" label="パーティション" width="320px"/>
        <el-table-column prop="accessLevelStr" label="アクセスレベル" width="200px"/>
        <el-table-column prop="tensorBoardEnabled" label="TensorBoard" width="200px">
          <template slot-scope="prop">
            {{ getTensorBoardFlag(prop.row.tensorBoardEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="notebookEnabled" label="Notebook" width="200px">
          <template slot-scope="prop">
            {{ getNotebookFlag(prop.row.notebookEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="memo" label="メモ" width="auto"/>
      </el-table>
    </el-row>
    <el-row>
      <el-col class="pagination" :span="10">
        <el-pagination layout="total, sizes, prev, pager, next"
                       :total="total"
                       :current-page="currentPage"
                       @size-change="handleSizeChange"
                       @current-change="currentChange"
                       :page-size="currentPageSize"
                       :page-sizes="[10, 30, 50, 100, 200]"
        />
      </el-col>
    </el-row>
    <router-view @cancel="closeDialog" @done="done"></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'NodeIndex',
    title: 'ノード管理',
    data () {
      return {
        total: 0,
        tableData: [],
        currentPage: 1,
        currentPageSize: 10
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async currentChange (page) {
        this.currentPage = page
        await this.retrieveData()
      },
      async retrieveData () {
        let params = {}
        params.withTotal = true
        let response = await api.nodes.admin.get(params)
        this.tableData = response.data
        this.total = parseInt(response.headers['x-total-count'])
      },
      // ページのサイズ(表示件数)変更
      async handleSizeChange (size) {
        this.currentPageSize = size
        this.currentPage = 1
        // データを再ロードする
        await this.retrieveData()
      },
      getTensorBoardFlag (enabled) {
        return enabled ? 'OK' : 'NG'
      },
      getNotebookFlag (enabled) {
        return enabled ? 'OK' : 'NG'
      },
      openCreateDialog () {
        this.$router.push('/node/create')
      },
      openEditDialog (selectedRow) {
        this.$router.push('/node/' + selectedRow.id)
      },
      closeDialog () {
        this.$router.push('/node')
      },
      async done () {
        await this.retrieveData()
        this.closeDialog()
        this.showSuccessMessage()
      }
    }
  }
</script>

<style lang="scss" scoped>
  .left-top-button {
    text-align: left;
  }

  .right-top-button {
    text-align: right;
  }

  .pagination /deep/ .el-input {
    text-align: left;
    width: 120px;
  }
</style>
