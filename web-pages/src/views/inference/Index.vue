<template>
  <div>
    <h2>推論管理2</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      ></kqi-pagination>
      <el-col class="right-top-button" :span="8">
        <el-button v-if="selections.length !== 0" @click="showDeleteConfirm"
          >一括削除</el-button
        >
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog()"
        >
          新規実行
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
        class="data-table pl-index-table"
        :data="histories"
        border
        @row-click="openEditDialog"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55px"></el-table-column>
        <el-table-column width="25px">
          <div slot-scope="scope">
            <i v-if="scope.row.favorite" class="el-icon-star-on favorite"></i>
          </div>
        </el-table-column>
        <el-table-column
          prop="id"
          label="推論ID"
          width="110px"
          align="center"
        />
        <el-table-column prop="name" label="推論名" width="150px" />
        <el-table-column prop="createdAt" label="開始日時" width="100px" />
        <el-table-column
          prop="parentName"
          label="マウントした学習"
          width="200px"
        />
        <el-table-column
          prop="dataSet.name"
          label="データセット"
          width="120px"
        />
        <el-table-column
          prop="entryPoint"
          label="実行コマンド"
          width="auto"
          class-name="entry-point-column"
        />
        <el-table-column prop="memo" label="メモ" width="auto" />
        <el-table-column
          prop="outputValue"
          label="出力値"
          width="200px"
          sortable
        />
        <el-table-column width="30px">
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
      </el-table>
    </el-row>

    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      ></kqi-pagination>
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
import KqiPagination from '@/components/KqiPagination'
import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('inference')

export default {
  title: '推論管理',
  components: {
    'kqi-pagination': KqiPagination,
    'pl-smart-search-input': SmartSearchInput,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      selections: [],
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: '推論ID', type: 'number' },
        { prop: 'name', name: '推論名', type: 'text' },
        { prop: 'startedAt', name: '開始日時', type: 'date' },
        { prop: 'parentName', name: 'マウントした学習', type: 'text' },
        { prop: 'dataSet', name: 'データセット', type: 'text' },
        { prop: 'entryPoint', name: '実行コマンド', type: 'text' },
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
    }
  },
  computed: {
    ...mapGetters(['histories', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchHistories', 'delete']),

    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchHistories(params)
    },
    async search() {
      this.currentPage = 1
      await this.retrieveData()
    },

    handleSelectionChange(val) {
      // checkboxの要素変更
      this.selections = val
    },

    async showDeleteConfirm() {
      let confirmMessage = `推論履歴を${this.selections.length}件削除しますか（出力データ数が多い場合、処理に時間がかかります）`
      await this.$confirm(confirmMessage, 'Warning', {
        distinguishCancelAndClose: true,
        confirmButtonText: 'はい',
        cancelButtonText: 'キャンセル',
        type: 'warning',
      })
        .then(async () => {
          let successCount = 0
          for (let selection of this.selections) {
            try {
              await this.delete(selection.id)
              successCount++
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
          await this.$notify.info({
            type: 'info',
            message: `推論履歴を削除しました。(成功：${successCount}件、 失敗：${this
              .selections.length - successCount}件）`,
          })
          this.currentPage = 1
          await this.retrieveData()
        })
        .catch(() => {
          // キャンセル時はなにもしないので例外を無視
        })
    },
    async done() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
      this.closeDialog()
      this.showSuccessMessage()
    },
    closeDialog() {
      this.$router.push('/inference')
    },
    back() {
      this.$router.go(-1)
    },
    openEditDialog(selectedRow) {
      this.$router.push('/inference/' + selectedRow.id)
    },
    openCreateDialog() {
      this.$router.push('/inference/create')
    },
    copyCreate(originId) {
      this.$router.push('/inference/create/' + originId)
    },
    files(id) {
      this.$router.push('/inference/' + id + '/files')
    },
    shell(id) {
      this.$router.push('/inference/' + id + '/shell')
    },
    log(id) {
      this.$router.push('/inference/' + id + '/log')
    },
  },
}
</script>

<style scoped>
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

<style lang="scss">
.entry-point-column {
  display: table-cell;
}

.entry-point-column div.cell {
  /*! autoprefixer: off */
  overflow: hidden;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  display: -webkit-box;
}
</style>