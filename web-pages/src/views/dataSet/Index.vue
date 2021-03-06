<template>
  <div>
    <h2>データセット管理</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
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
        <kqi-smart-search-input
          v-model="searchCondition"
          :configs="searchConfigs"
          @search="search"
        />
      </el-col>
    </el-row>
    <el-row class="test">
      <el-table
        class="data-table pl-index-table"
        :data="dataSets"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="データセット名" width="auto" />
        <el-table-column
          prop="memo"
          label="メモ"
          width="auto"
          class-name="memo-column"
        />
        <el-table-column prop="createdAt" label="登録日時" width="170px" />
      </el-table>
    </el-row>
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
    </el-row>
    <router-view @cancel="closeDialog" @done="done" @copy="handleCopy" />
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: 'データセット管理',
  components: {
    KqiPagination,
    KqiSmartSearchInput,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        { prop: 'createdAt', name: '登録日時', type: 'date' },
      ],
      tableData: [],
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

    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done(type) {
      if (type === 'delete') {
        // 削除時、表示していたページにデータが無くなっている可能性がある。
        // 総数 % ページサイズ === 1の時、残り1の状態で削除したため、currentPageが1で無ければ1つ前のページに戻す
        if (this.total % this.pageStatus.currentPageSize === 1) {
          if (this.pageStatus.currentPage !== 1) {
            this.pageStatus.currentPage -= 1
          }
        }
      }
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
      this.pageStatus.currentPage = 1
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
