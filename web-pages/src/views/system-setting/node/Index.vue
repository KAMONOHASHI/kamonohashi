<template>
  <div>
    <h2>ノード管理2</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <el-col class="pagination" :span="16">
        <el-pagination
          layout="total, sizes, prev, pager, next"
          :total="total"
          :current-page="currentPage"
          :page-size="currentPageSize"
          :page-sizes="[10, 30, 50, 100, 200]"
          @size-change="handleSizeChange"
          @current-change="currentChange"
        />
      </el-col>
      <el-col class="right-top-button" :span="8">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog"
        >
          新規登録
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table
        class="data-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="ノード名" width="200px" />
        <el-table-column
          prop="partition"
          label="パーティション"
          width="320px"
        />
        <el-table-column
          prop="accessLevelStr"
          label="アクセスレベル"
          width="200px"
        />
        <el-table-column
          prop="tensorBoardEnabled"
          label="TensorBoard"
          width="200px"
        >
          <template slot-scope="prop">
            {{ getTensorBoardFlag(prop.row.tensorBoardEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="notebookEnabled" label="Notebook" width="200px">
          <template slot-scope="prop">
            {{ getNotebookFlag(prop.row.notebookEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="memo" label="メモ" width="auto" />
      </el-table>
    </el-row>
    <el-row>
      <el-col class="pagination" :span="10">
        <el-pagination
          layout="total, sizes, prev, pager, next"
          :total="total"
          :current-page="currentPage"
          :page-size="currentPageSize"
          :page-sizes="[10, 30, 50, 100, 200]"
          @size-change="handleSizeChange"
          @current-change="currentChange"
        />
      </el-col>
    </el-row>
    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('node')

export default {
  data() {
    return {
      total: 0,
      tableData: [],
      currentPage: 1,
      currentPageSize: 10,
    }
  },
  computed: {
    ...mapGetters(['nodeData']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchNodeData']),
    async retrieveData() {
      await this.fetchNodeData()
      this.tableData = this.nodeData.data
      this.total = parseInt(this.nodeData.headers['x-total-count'])
    },
    async currentChange(page) {
      this.currentPage = page
      await this.retrieveData()
    },
    // ページのサイズ(表示件数)変更
    async handleSizeChange(size) {
      this.currentPageSize = size
      this.currentPage = 1
      // データを再ロードする
      await this.retrieveData()
    },
    getTensorBoardFlag(enabled) {
      return enabled ? 'OK' : 'NG'
    },
    getNotebookFlag(enabled) {
      return enabled ? 'OK' : 'NG'
    },
    openCreateDialog() {
      this.$router.push('node/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/node/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/node')
    },
    async done() {
      this.closeDialog()
      this.retrieveData()
      this.showSuccessMessage()
    },
  },
}
</script>
