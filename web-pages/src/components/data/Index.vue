<template>
  <div>
    <h2>{{$t('title')}}</h2>
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
        <div>
          <el-button @click="openPreprocessingDialog">前処理実行</el-button>
          <el-button @click="openCreateDialog" icon="el-icon-edit-outline" type="primary" plain>
            新規登録
          </el-button>
        </div>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col class="search">
        <pl-smart-search-input v-model="searchCondition" :configs="searchConfigs" @search="search"/>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog"
                @selection-change="handleSelectionChange" border>
        <el-table-column type="selection" width="55px"></el-table-column>
        <el-table-column prop="id" label="ID" width="120px"/>
        <el-table-column prop="name" label="データ名" width="auto"/>
        <el-table-column label="元データ名" width="120px">
          <div slot-scope="scope">
            <div v-if="scope.row.isRaw">
            </div>
            <div v-else>
              {{ scope.row.parentDataName }}
            </div>
          </div>
        </el-table-column>
        <el-table-column prop="createdAt" label="登録日時" width="200px"/>
        <el-table-column prop="createdBy" label="登録者" width="120px"/>
        <el-table-column prop="memo" label="メモ" width="auto"/>
        <el-table-column prop="tag" label="タグ" width="120px">
          <template slot-scope="scope">
          <span v-for="(tag, index) in scope.row.tags" :key="index" style="padding-left:10px">
            <el-tag size="mini">
              {{tag}}
            </el-tag>
          </span>
          </template>
        </el-table-column>
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
    <router-view
      @cancel="closeDialog()"
      @done="done()"
      @preprocessing="openPreprocessingDialog()"
      @close="closeDialog()"
      @runPreprocessing="redirectPreprocessingPage()"
    ></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'

  export default {
    name: 'DataIndex',
    title () {
      return this.$t('title')
    },
    components: {
      'pl-smart-search-input': SmartSearchInput
    },
    data () {
      return {
        searchCondition: {},
        searchConfigs: [
          {prop: 'id', name: 'ID', type: 'number'},
          {prop: 'name', name: 'データ名', type: 'text'},
          {prop: 'createdAt', name: '登録日時', type: 'date'},
          {prop: 'createdBy', name: '登録者', type: 'text'},
          {prop: 'memo', name: 'メモ', type: 'text'},
          {prop: 'tag', name: 'タグ', type: 'text', multiple: true}
        ],
        total: 0,
        tableData: [],
        currentPage: 1,
        selectedRowId: undefined,
        currentPageSize: 10,
        multipleSelection: [],
        multiSelectedId: ''
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

        let response = await api.data.get(params)
        this.tableData = response.data
        this.total = parseInt(response.headers['x-total-count'])
      },
      // checkboxの要素変更
      handleSelectionChange (val) {
        this.multipleSelection = val
      },
      // ページのサイズ(表示件数)変更
      async handleSizeChange (size) {
        this.currentPageSize = size
        this.currentPage = 1
        // データを再ロードする
        await this.retrieveData()
      },
      openEditDialog (selectedRow) {
        this.selectedRowId = selectedRow.id
        this.$router.push('/data/' + selectedRow.id)
      },
      async search () {
        this.currentPage = 1
        await this.retrieveData()
      },
      openCreateDialog () {
        this.$router.push('/data/create')
      },
      closeDialog () {
        this.$router.push('/data')
      },
      async done () {
        this.closeDialog()
        await this.retrieveData()
        this.showSuccessMessage()
      },
      openPreprocessingDialog () {
        if (this.multipleSelection.length === 0) {
          this.$router.push('/preprocessing/run')
        } else {
          this.multiSelectedId = ''
          this.multipleSelection.forEach(value => {
            this.multiSelectedId += (value.id).toString() + ' '
          })
          this.$router.push('/data/' + this.multiSelectedId + '/preprocessing')
        }
      },
      redirectPreprocessingPage () {
        this.$router.push('/preprocessing')
      }
    },
    i18n: {
      messages: {
        en: {
          title: 'Data'
        },
        ja: {
          title: 'データ管理'
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

  .el-tag--mini {
    max-width: 100%;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
  }
</style>
