<template>
  <div>
    <h2>データセット</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
      <el-col class="right-top-button" :span="8">
        <el-button
          icon="el-icon-plus"
          type="primary"
          plain
          @click="openCreateDialog()"
        >
          新しいデータセット
        </el-button>
      </el-col>
    </el-row>

    <el-row class="test">
      <el-table
        class="data-table pl-index-table"
        :data="dataSets"
        border
        @row-click="openEditDataset"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="データセット名" width="auto" />
        <el-table-column
          prop="latestVersion"
          label="最新バージョン"
          width="auto"
        />
        <el-table-column prop="modifiedAt" label="最終更新日時" width="auto" />
        <el-table-column prop="status" label="ステータス" width="auto" />
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
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('aquariumDataSet')

export default {
  title: 'データセット',
  components: {
    KqiPagination,
  },
  data() {
    return {
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'latestVersion', name: '最新バージョン', type: 'text' },

        { prop: 'modifiedAt', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
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
      this.$router.push('/aquarium/dataset')
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
      this.$router.push('/aquarium/dataset/create')
    },
    openEditDataset(selectedRow) {
      this.$router.push('/aquarium/dataset/detail/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/aquarium/dataset/create/' + id)
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
