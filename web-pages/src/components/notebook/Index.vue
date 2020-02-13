<template>
  <div>
    <h2>ノートブック管理</h2>
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
          @click="openCreateDialog()"
        >
          ノートブック作成
        </el-button>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col class="search">
        <pl-smart-search-input
          v-model="searchCondition"
          :configs="searchConfigs"
          @search="search"
        />
      </el-col>
    </el-row>
    <el-row>
      <el-table
        ref="table"
        class="data-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column width="25px">
          <div slot-scope="scope">
            <i v-if="scope.row.favorite" class="el-icon-star-on favorite"></i>
          </div>
        </el-table-column>
        <el-table-column prop="id" label="ノートブックID" width="120px" />
        <el-table-column prop="name" label="ノートブック名" width="240px" />
        <el-table-column prop="createdAt" label="作成日時" width="200px" />
        <el-table-column prop="memo" label="メモ" width="auto" />
        <el-table-column width="25px">
          <div slot-scope="scope">
            <div
              v-if="
                (scope.row.status === 'Running') |
                  (scope.row.status === 'Completed')
              "
            >
              <i class="el-icon-success" style="color: #67C23A"></i>
            </div>
            <div v-else>
              <i class="el-icon-warning" style="color: #E6A23C"></i>
            </div>
          </div>
        </el-table-column>
        <el-table-column prop="status" label="ステータス" width="120px" />
        <el-table-column prop="status" label="Action" width="300px">
          <div slot-scope="scope">
            <div v-if="scope.row.status === 'Running'">
              <el-button
                type="plain"
                icon="el-icon-document"
                @click="openNotebook(scope.row)"
                >ノートブックを開く</el-button
              >
            </div>
            <div v-if="scope.row.status === 'Killed'">
              <el-button
                type="plain"
                icon="el-icon-refresh"
                @click="openRerunDialog(scope.row)"
                >再実行</el-button
              >
            </div>
            <div v-else></div>
          </div>
        </el-table-column>
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
    <router-view
      @cancel="closeDialog"
      @done="done"
      @return="back"
      @files="files"
      @shell="shell"
      @log="log"
      @copyCreate="copyCreateDialog"
    ></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'

export default {
  name: 'NotebookIndex',
  title: 'ノートブック管理',
  components: {
    'pl-smart-search-input': SmartSearchInput,
  },
  data() {
    return {
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ノートブックID', type: 'number' },
        { prop: 'name', name: 'ノートブック名', type: 'text' },
        { prop: 'createdAt', name: '作成日時', type: 'date' },
        { prop: 'createdBy', name: '作成者', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        {
          prop: 'status',
          name: 'ステータス',
          type: 'select',
          option: {
            items: [
              'None',
              'Pending',
              'Succeeded',
              'Running',
              'Completed',
              'UserCanceled',
              'Failed',
              'Killed',
              'Invalid',
              'Forbidden',
              'Multiple',
              'Empty',
              'Error',
            ],
          },
        },
      ],
      total: 0,
      tableData: [],
      statuses: [],
      currentPage: 1,
      currentPageSize: 10,
      notebookUrl: undefined,
      notebookUrlFlg: false,
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.currentPage
      params.perPage = this.currentPageSize
      params.withTotal = true

      let response = await api.notebook.get(params)
      this.tableData = response.data
      this.total = parseInt(response.headers['x-total-count'])
    },
    // ページのサイズ(表示件数)変更
    async handleSizeChange(size) {
      this.currentPageSize = size
      this.currentPage = 1
      await this.retrieveData()
    },
    async currentChange(page) {
      this.currentPage = page
      await this.retrieveData()
      this.$router.push('/notebook')
    },
    methods: {
      async retrieveData () {
        let params = this.searchCondition
        params.page = this.currentPage
        params.perPage = this.currentPageSize
        params.withTotal = true

        let response = await api.notebook.get(params)
        this.tableData = response.data
        this.total = parseInt(response.headers['x-total-count'])
      },
      // ページのサイズ(表示件数)変更
      async handleSizeChange (size) {
        this.currentPageSize = size
        this.currentPage = 1
        await this.retrieveData()
      },
      async currentChange (page) {
        this.currentPage = page
        await this.retrieveData()
        this.$router.push('/notebook')
      },
      done () {
        this.currentChange(1)
        this.closeDialog()
        this.showSuccessMessage()
      },
      closeDialog () {
        this.$router.push('/notebook')
      },
      back () {
        this.$router.go(-1)
      },
      openEditDialog (selectedRow) {
        if (this.$route.path === '/notebook' && !this.notebookUrlFlg) {
          this.$router.push('/notebook/' + selectedRow.id)
        }
      },
      openCreateDialog () {
        this.$router.push('/notebook/run/')
      },
      async openNotebook (selectedRow) {
        this.notebookUrlFlg = true
        let endpoint = await api.notebook.getEndpointById({id: selectedRow.id})
        this.notebookUrl = endpoint.data.url
        window.open(this.notebookUrl)
        this.notebookUrlFlg = false
      },
      async search () {
        this.currentPage = 1
        await this.retrieveData()
      },
      files (id) {
        this.$router.push('/notebook/' + id + '/files')
      },
      shell (id) {
        this.$router.push('/notebook/' + id + '/shell')
      },
      log (id) {
        this.$router.push('/notebook/' + id + '/log')
      },
      openRerunDialog (selectedRow) {
        this.$router.push('/notebook/run/' + selectedRow.id)
      },
      copyCreateDialog (originId) {
        this.$router.push('/notebook/run/' + originId + '?run=copy')
      }
    },
    openCreateDialog() {
      this.$router.push('/notebook/run/')
    },
    async openNotebook(selectedRow) {
      this.notebookUrlFlg = true
      let endpoint = await api.notebook.getEndpointById({ id: selectedRow.id })
      this.notebookUrl = endpoint.data.url
      window.open(this.notebookUrl)
      this.notebookUrlFlg = false
    },
    async search() {
      this.currentPage = 1
      await this.retrieveData()
    },
    files(id) {
      this.$router.push('/notebook/' + id + '/files')
    },
    shell(id) {
      this.$router.push('/notebook/' + id + '/shell')
    },
    log(id) {
      this.$router.push('/notebook/' + id + '/log')
    },
    openRerunDialog(selectedRow) {
      this.$router.push('/notebook/run/' + selectedRow.id)
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

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}

.favorite {
  color: rgb(230, 162, 60);
}
.el-dropdown {
  vertical-align: top;
}
.el-dropdown-link {
  cursor: pointer;
}
</style>
