<template>
  <div>
    <h2>{{ $t('title') }}</h2>
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
          新規作成
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
    <el-row class="test">
      <el-table
        class="data-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="データセット名" width="auto" />
        <el-table-column prop="memo" label="メモ" width="auto" />
        <el-table-column prop="createdAt" label="登録日時" width="170px" />
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
      @copy="handleCopy"
    ></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'

export default {
  name: 'DataSetIndex',
  title() {
    return this.$t('title')
  },
  components: {
    'pl-smart-search-input': SmartSearchInput,
  },
  data() {
    return {
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        { prop: 'createdAt', name: '登録日時', type: 'date' },
      ],
      total: 0,
      tableData: [],
      currentPage: 1,
      currentPageSize: 10,
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async currentChange(page) {
      this.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.currentPage
      params.perPage = this.currentPageSize
      params.withTotal = true

      let response = await api.datasets.get(params)
      this.tableData = response.data
      this.total = parseInt(response.headers['x-total-count'])
    },
    // ページのサイズ(表示件数)変更
    async handleSizeChange(size) {
      this.currentPageSize = size
      this.currentPage = 1
      // データを再ロードする
      await this.retrieveData()
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
      this.$router.push('/dataset/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      this.currentPage = 1
      await this.retrieveData()
    },
  },
  i18n: {
    messages: {
      en: {
        title: 'Data Set',
      },
      ja: {
        title: 'データセット管理',
      },
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
</style>
