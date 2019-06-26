<template>
  <div>
    <h2> 学習管理 </h2>
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
        <el-button @click="openCreateDialog()" icon="el-icon-edit-outline" type="primary" plain>
          新規実行
        </el-button>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col class="search">
        <pl-smart-search-input v-model="searchCondition" :configs="searchConfigs" @search="search"/>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
        <el-table-column width="25px">
          <div slot-scope="scope">
            <i class="el-icon-star-on favorite" v-if="scope.row.favorite"></i>
          </div>
        </el-table-column>
        <el-table-column prop="id" label="学習ID" width="120px"/>
        <el-table-column prop="name" label="学習名" width="120px"/>
        <el-table-column prop="createdAt" label="開始日時" width="200px"/>
        <el-table-column prop="dataSet.name" label="データセット" width="120px"/>
        <el-table-column prop="entryPoint" label="実行コマンド" width="auto"/>
        <el-table-column prop="memo" label="メモ" width="auto"/>
        <el-table-column width="25px">
          <div slot-scope="scope">
            <div v-if="scope.row.status === 'Running' | scope.row.status === 'Completed'">
              <i class="el-icon-success" style="color: #67C23A"></i>
            </div>
            <div v-else>
              <i class="el-icon-warning" style="color: #E6A23C"></i>
            </div>
          </div>
        </el-table-column>
        <el-table-column prop="status" label="ステータス" width="120px"/>
      </el-table>
    </el-row>
    <el-row>
      <el-col class=" pagination
        " :span="10">
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
    <router-view
      @cancel="closeDialog"
      @done="done"
      @return="back"
      @files="files"
      @shell="shell"
      @log="log"
      @copyCreate="copyCreate"
    ></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'

  export default {
    name: 'TrainIndex',
    title: '学習管理',
    components: {
      'pl-smart-search-input': SmartSearchInput
    },
    data () {
      return {
        searchCondition: {},
        searchConfigs: [
          {prop: 'id', name: '学習ID', type: 'number'},
          {prop: 'name', name: '学習名', type: 'text'},
          {prop: 'startedAt', name: '開始日時', type: 'date'},
          {prop: 'dataSet', name: 'データセット', type: 'text'},
          {prop: 'entryPoint', name: '実行コマンド', type: 'text'},
          {prop: 'memo', name: 'メモ', type: 'text'},
          {
            prop: 'status',
            name: 'ステータス',
            type: 'select',
            option: {items: ['None', 'Pending', 'Succeeded', 'Running', 'Completed', 'Failed', 'Killed', 'Invalid', 'Forbidden', 'Multiple', 'Empty', 'Error']}
          }
        ],
        total: 0,
        tableData: [],
        statuses: [],
        currentPage: 1,
        currentPageSize: 10,
        entryPoint: undefined
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        let params = this.searchCondition
        params.page = this.currentPage
        params.perPage = this.currentPageSize
        params.withTotal = true

        let response = await api.training.get(params)
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
        this.$router.push('/training')
      },
      done () {
        this.currentChange(1)
        this.closeDialog()
        this.showSuccessMessage()
      },
      closeDialog () {
        this.$router.push('/training')
      },
      back () {
        this.$router.go(-1)
      },
      openEditDialog (selectedRow) {
        this.$router.push('/training/' + selectedRow.id)
      },
      openCreateDialog () {
        this.$router.push('/training/run/')
      },
      async search () {
        this.currentPage = 1
        await this.retrieveData()
      },
      copyCreate (parentId) {
        this.$router.push('/training/run/' + parentId)
      },
      files (id) {
        this.$router.push('/training/' + id + '/files')
      },
      shell (id) {
        this.$router.push('/training/' + id + '/shell')
      },
      log (id) {
        this.$router.push('/training/' + id + '/log')
      }
    }
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

</style>
