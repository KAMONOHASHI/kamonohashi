<script src="../../api/v1/api.js"></script>
<script src="../../api/v1/api.generator.js"></script>
<template>
  <div>
    <h2>{{$t('title')}}</h2>
    <el-row>
      <el-col :span="8" class="back">
        <span @click="openPreprocessing">
            前処理管理
        </span>
        <el-button type="text" @click="openPreprocessing"></el-button>
        <i class="el-icon-arrow-right"></i>
        <span>{{id}}</span>
      </el-col>
    </el-row>
    <br/>
    <el-row>
      <el-col type="flex" justify="space-between" :gutter="20" class="pagination" :span="16">
        <el-pagination layout="total, sizes, prev, pager, next"
                       :total="total"
                       @size-change="handleSizeChange"
                       @current-change="currentChange"
                       :page-size="currentPageSize"
                       :current-page="currentPage"
                       :page-sizes="[10, 30, 50, 100, 200]"
        />
      </el-col>
    </el-row>

    <pl-display-error :error="error"/>
    <br/>
    <div v-if="loading">
      <i class="el-icon-loading"></i>
    </div>
    <div v-else>
      <div v-if="tableData.length===0">
        履歴が見つかりませんでした。
      </div>
      <div v-else>
        <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
          <el-table-column prop="dataId" label="入力データID" width="120px"/>
          <el-table-column prop="dataName" label="入力データ名" width="auto"/>
          <el-table-column prop="createdAt" label="実行日時" width="170px"/>
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
      </div>
    </div>

    <router-view
      @done="done"
      @cancel="closeEditDialog"
      @return="back"
      @shell="shell"
      @log="log"
    ></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'PreprocessingHistory',
    components: {
      'pl-display-error': DisplayError
    },
    props: ['id'],
    data () {
      return {
        error: null,
        loading: true,
        selectedEdit: {},
        tableData: [],
        historyData: [],
        total: 0,
        currentPage: 1,
        currentPageSize: 10
      }
    },
    async created () {
      await this.changeId()
    },
    watch: {
      async id () {
        await this.changeId()
      }
    },
    methods: {
      async refreshTable () {
        // api.preprocessings.getHistory()で、全履歴が取得できているため、手動でページ分割して表示
        this.tableData = this.historyData.slice(
          (this.currentPage - 1) * this.currentPageSize,
          (this.currentPage - 1) * this.currentPageSize + this.currentPageSize)
      },
      async changeId () {
        this.tableData = []

        if (this.id) {
          try {
            this.loading = true
            let params = {
              $config: {apiDisabledLoading: true},
              id: this.id
            }
            this.historyData = (await api.preprocessings.getHistory(params)).data
            this.total = this.historyData.length
            this.error = null
          } catch (e) {
            this.error = e
          } finally {
            this.loading = false
          }
          this.refreshTable()
        }
      },
      async openEditDialog (row) {
        if (row) {
          this.$router.push('/preprocessingHistory/' + this.id + '/' + row.dataId)
        } else {
          this.selectedEdit = null
        }
      },
      async closeEditDialog () {
        this.$router.push('/preprocessingHistory/' + this.id)
      },
      async done () {
        await this.changeId()
        this.closeEditDialog()
        this.showSuccessMessage()
      },
      async currentChange (page) {
        this.currentPage = page
        await this.refreshTable()
      },
      async handleSizeChange (size) {
        this.currentPageSize = size
        this.currentPage = 1
        await this.refreshTable()
      },
      async openPreprocessing () {
        this.$router.push('/preprocessing')
        this.$store.commit('setLoading', false)
      },
      shell (data) {
        this.$router.push('/preprocessingHistory/' + data.id + '/' + data.dataId + '/shell')
      },
      log (data) {
        this.$router.push('/preprocessingHistory/' + data.id + '/' + data.dataId + '/log')
      },
      back () {
        this.$router.go(-1)
      }
    },
    i18n: {
      messages: {
        en: {
          title: 'PreprocessingHistory'
        },
        ja: {
          title: '前処理履歴'
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .back {
    padding-top: 20px;
    cursor: pointer;
    :hover {
      color: #409eff;
    }
  }

  .pagination /deep/ .el-input {
    text-align: left;
    width: 120px;
  }

</style>
