<template>
  <div>
    <h2>ノートブック管理</h2>
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
          ノートブック作成
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
    <el-row>
      <el-table
        ref="table"
        class="data-table pl-index-table"
        :data="histories"
        border
        @row-click="openEditDialog"
      >
        <el-table-column width="25px">
          <div slot-scope="scope">
            <i v-if="scope.row.favorite" class="el-icon-star-on favorite" />
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
              <i class="el-icon-success" style="color: #67C23A" />
            </div>
            <div v-else>
              <i class="el-icon-warning" style="color: #E6A23C" />
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
                @click.stop="openNotebook(scope.row)"
              >
                ノートブックを開く
              </el-button>
            </div>
            <div v-if="scope.row.status === 'Killed'">
              <el-button
                type="plain"
                icon="el-icon-refresh"
                @click.stop="openRerunDialog(scope.row)"
              >
                再実行
              </el-button>
            </div>
          </div>
        </el-table-column>
      </el-table>
    </el-row>
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
    </el-row>
    <router-view
      @cancel="closeDialog"
      @cancelShell="closeDialog"
      @done="done"
      @return="back"
      @files="files"
      @shell="shell"
      @log="log"
      @copyCreate="copyCreateDialog"
    />
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('notebook')

export default {
  title: 'ノートブック管理',
  components: {
    KqiSmartSearchInput,
    KqiPagination,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
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
      statuses: [],
    }
  },
  computed: {
    ...mapGetters(['histories', 'total', 'endpoint']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchHistories', 'fetchEndpoint']),
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchHistories(params)
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
      await this.retrieveData()
      this.closeDialog()
      this.showSuccessMessage()
    },
    closeDialog() {
      this.$router.push('/notebook')
    },
    back() {
      this.$router.go(-1)
    },
    openEditDialog(selectedRow) {
      if (this.$route.path === '/notebook') {
        this.$router.push('/notebook/' + selectedRow.id)
      }
    },
    openCreateDialog() {
      this.$router.push('/notebook/run/')
    },
    async openNotebook(selectedRow) {
      await this.fetchEndpoint(selectedRow.id)
      window.open(this.endpoint)
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
    copyCreateDialog(originId) {
      this.$router.push('/notebook/run/' + originId + '?run=copy')
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
