<template>
  <div>
    <h2>{{$t('title')}}</h2>

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
      <el-col class="right-top-button" :span="8">
        <el-button @click="openPreprocessingDialog">前処理実行</el-button>
        <el-button @click="openCreateDialog()" icon="el-icon-edit-outline" type="primary" plain>
          新規作成
        </el-button>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col class="search">
        <pl-smart-search-input v-model="searchCondition" :configs="searchConfigs" @search="search"/>
      </el-col>
    </el-row>
    <div>
      <el-row>
        <el-table ref="table" class="data-table pl-index-table" :data="tableData"
                  @row-click="openEditDialog"
                  border>
          <el-table-column prop="id" label="ID" width="120px"/>
          <el-table-column prop="name" label="前処理名" width="auto"/>
          <el-table-column prop="memo" label="メモ" width="auto"/>
          <el-table-column prop="createdAt" label="登録日時" width="170px"/>
          <el-table-column width="auto">
            <template slot-scope="props">
              <el-button @click="openHistoryIndex(props.row)" icon="el-icon-time" type="primary"
                         plain>履歴
              </el-button>
            </template>
          </el-table-column>
        </el-table>
      </el-row>
    </div>
    <router-view
      @done="done"
      @cancel="closeDialog"
      @copy="handleCopy"
      @return="back"
      @shell="shell"
    ></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import SmartSearchInput from '@/components/common/SmartSearchInput/Index'

  export default {
    name: 'PreprocessingIndex',
    title () {
      return this.$t('title')
    },
    components: {
      'pl-smart-search-input': SmartSearchInput
    },
    data () {
      return {
        mode: '1',
        searchCondition: {},
        searchConfigs: [
          {prop: 'id', name: 'ID', type: 'number'},
          {prop: 'name', name: '前処理名', type: 'text'},
          {prop: 'memo', name: 'メモ', type: 'text'},
          {prop: 'createdAt', name: '登録日時', type: 'date'}
        ],
        total: 0,
        tableData: [],
        currentPage: 1,
        selectedRowId: 0,
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
        let params = this.searchCondition
        params.page = this.currentPage
        params.perPage = this.currentPageSize
        params.withTotal = true

        let response = await api.preprocessings.get(params)
        this.tableData = response.data
        this.total = parseInt(response.headers['x-total-count'])
      },

      async search () {
        this.currentPage = 1
        await this.retrieveData()
      },

      async handleSizeChange (size) {
        this.currentPageSize = size
        this.currentPage = 1
        await this.retrieveData()
      },
      async handleModeChange () {
        await this.retrieveData()
      },
      closeDialog () {
        this.$router.push('/preprocessing')
        this.$store.commit('setLoading', true)
      },
      openCreateDialog () {
        this.$router.push('/preprocessing/create')
        this.$store.commit('setLoading', false)
      },
      handleCopy (id) {
        this.$router.push('/preprocessing/create/' + id)
        this.$store.commit('setLoading', false)
      },
      async done () {
        this.closeDialog()
        await this.retrieveData()
        this.showSuccessMessage()
      },
      async openHistoryIndex (row) {
        this.$router.push('/preprocessingHistory/' + row.id)
        this.$store.commit('setLoading', false)
      },

      async openEditDialog (selectedRow) {
        this.selectedRowId = selectedRow.id
        this.$router.push('/preprocessing/' + selectedRow.id + '/edit')
        this.$store.commit('setLoading', false)
      },
      openPreprocessingDialog () {
        this.$router.push('preprocessing/run')
        this.$store.commit('setLoading', false)
      },
      openPreprocessingHistoryEditDialog (data) {
        this.$router.push('/preprocessing/' + data.id + '/' + data.dataId)
      },
      expand (row) {
        // 編集ボタンクリック時は、expandしないように処理
        if (this.$route.path === '/preprocessing') {
          this.$refs.table.toggleRowExpansion(row)
        }
      },
      shell (data) {
        this.$router.push('/preprocessing/' + data.id + '/' + data.dataId + '/shell')
      },
      back () {
        this.$router.go(-1)
      }
    },
    i18n: {
      messages: {
        en: {
          title: 'Preprocessing'
        },
        ja: {
          title: '前処理管理'
        }
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

  .text {
    font-size: 16px;
  }

  .item {
    padding: 18px 0;
  }

  .card-container {
    float: left;
    margin: 20px 20px 10px 0;
    .button {
      padding-top: 30px;
    }
  }

</style>
