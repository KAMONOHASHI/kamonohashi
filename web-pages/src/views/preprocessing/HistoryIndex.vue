<template>
  <div>
    <h2>前処理履歴</h2>
    <el-row>
      <el-col :span="8" class="back">
        <span @click="openPreprocessing">
          前処理管理
        </span>
        <el-button type="text" @click="openPreprocessing" />
        <i class="el-icon-arrow-right" />
        <span>{{ id }}</span>
      </el-col>
    </el-row>
    <br />
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="histories.length"
        @change="retrieveData"
      />
    </el-row>

    <kqi-display-error :error="error" />
    <br />
    <div v-if="histories.length === 0">
      履歴が見つかりませんでした。
    </div>
    <div v-else>
      <el-table
        class="data-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="dataId" label="入力データID" width="120px" />
        <el-table-column prop="dataName" label="入力データ名" width="auto" />
        <el-table-column prop="createdAt" label="実行日時" width="170px" />
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
      </el-table>
    </div>

    <router-view
      @done="done"
      @cancel="closeEditDialog"
      @return="back"
      @shell="shell"
      @log="log"
      @cancelShell="closeShell"
    />
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('preprocessing')

export default {
  title: '前処理履歴',
  components: {
    KqiPagination,
    KqiDisplayError,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      tableData: [],
      error: null,
    }
  },
  computed: {
    ...mapGetters(['histories']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchHistories']),

    async retrieveData() {
      if (this.id) {
        try {
          await this.fetchHistories(this.id)
          this.error = null
        } catch (e) {
          this.error = e
        }
        // fetchHistories()で、全履歴が取得できているため、手動でページ分割して表示
        this.tableData = this.histories.slice(
          (this.pageStatus.currentPage - 1) * this.pageStatus.currentPageSize,
          (this.pageStatus.currentPage - 1) * this.pageStatus.currentPageSize +
            this.pageStatus.currentPageSize,
        )
      }
    },

    async openEditDialog(row) {
      if (row) {
        this.$router.push('/preprocessingHistory/' + this.id + '/' + row.dataId)
      }
    },
    async closeEditDialog() {
      this.$router.push('/preprocessingHistory/' + this.id)
    },
    async done(type) {
      if (type === 'delete') {
        // 削除時、表示していたページにデータが無くなっている可能性がある。
        // 総数 % ページサイズ === 1の時、残り1の状態で削除したため、currentPageが1で無ければ1つ前のページに戻す
        if (this.histories.length % this.pageStatus.currentPageSize === 1) {
          if (this.pageStatus.currentPage !== 1) {
            this.pageStatus.currentPage -= 1
          }
        }
      }
      await this.retrieveData()
      this.closeEditDialog()
      this.showSuccessMessage()
    },
    async openPreprocessing() {
      this.$router.push('/preprocessing')
    },
    shell(data) {
      this.$router.push('/preprocessingShell/' + data.id)
    },
    log(data) {
      this.$router.push(
        '/preprocessingHistory/' + data.id + '/' + data.dataId + '/log',
      )
    },
    back() {
      this.$router.go(-1)
    },
    closeShell() {
      this.$router.go(-2)
    },
  },
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
</style>
