<template>
  <div>
    <h2>推論管理</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
      <el-col class="right-top-button" :span="8">
        <el-button v-if="selections.length !== 0" @click="showDeleteConfirm">
          一括削除
        </el-button>
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
        <kqi-smart-search-input
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
        <el-table-column type="selection" width="55px" />
        <el-table-column width="25px">
          <div slot-scope="scope">
            <i v-if="scope.row.favorite" class="el-icon-star-on favorite" />
          </div>
        </el-table-column>
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="推論名" width="150px" />
        <el-table-column prop="createdAt" label="開始日時" width="100px" />
        <el-table-column label="マウントした学習" width="200px">
          <template slot-scope="scope">
            <span
              v-for="(ParentName, index) in scope.row.parentFullNameList"
              :key="index"
            >
              <span class="parent">
                {{ ParentName }}
              </span>
            </span>
          </template>
        </el-table-column>
        <el-table-column label="マウントした推論" width="200px">
          <template slot-scope="scope">
            <span
              v-for="(ParentInferenceName, index) in scope.row
                .parentInferenceFullNameList"
              :key="index"
            >
              <span class="parent">
                {{ ParentInferenceName }}
              </span>
            </span>
          </template>
        </el-table-column>
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
        <el-table-column
          prop="memo"
          label="メモ"
          width="auto"
          class-name="memo-column"
        />
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
              <i class="el-icon-success" style="color: #67C23A;" />
            </div>
            <div v-else>
              <i class="el-icon-warning" style="color: #E6A23C;" />
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
      @copyCreate="copyCreate"
    />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiPagination from '@/components/KqiPagination.vue'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('inference')
//import { DeepWriteable } from '@/@types/type'
import * as gen from '@/api/api.generate'

interface DataType {
  pageStatus: {
    currentPage: number
    currentPageSize: number
  }
  selections: Array<
    gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
  >
  searchCondition: { page?: number; perPage?: number; withTotal?: boolean }
  searchConfigs: Array<{
    prop: string
    name: string
    type: string
    option?: { items: Array<string> }
  }>
}
export default Vue.extend({
  components: {
    KqiPagination,
    KqiSmartSearchInput,
  },
  data(): DataType {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      selections: [],
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: '推論名', type: 'text' },
        { prop: 'startedAt', name: '開始日時', type: 'date' },
        { prop: 'startedBy', name: '実行者', type: 'text' },
        { prop: 'parentId', name: 'マウントした学習ID', type: 'number' },
        {
          prop: 'parentInferenceId',
          name: 'マウントした推論ID',
          type: 'number',
        },
        { prop: 'parentName', name: 'マウントした学習名', type: 'text' },
        {
          prop: 'parentInferenceName',
          name: 'マウントした推論名',
          type: 'text',
        },
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
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },

    handleSelectionChange(
      val: Array<
        gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
      >,
    ) {
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
              //@ts-ignore
              this.error = null
            } catch (e) {
              //@ts-ignore
              this.error = e
            }
          }
          //@ts-ignore
          await this.$notify.info({
            type: 'info',
            message: `推論履歴を削除しました。(成功：${successCount}件、 失敗：${this
              .selections.length - successCount}件）`,
          })
          this.pageStatus.currentPage = 1
          await this.retrieveData()
        })
        .catch(() => {
          // キャンセル時はなにもしないので例外を無視
        })
    },
    async done(type: string) {
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
    closeDialog() {
      this.$router.push('/inference')
    },
    back() {
      this.$router.go(-1)
    },
    openEditDialog(
      selectedRow: gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel,
    ) {
      this.$router.push('/inference/' + selectedRow.id)
    },
    openCreateDialog() {
      this.$router.push('/inference/create')
    },
    copyCreate(originId: number) {
      this.$router.push('/inference/create/' + originId)
    },
    files(id: number) {
      this.$router.push('/inference/' + id + '/files')
    },
    shell(id: number) {
      this.$router.push('/inference/' + id + '/shell')
    },
    log(id: number) {
      this.$router.push('/inference/' + id + '/log')
    },
  },
})
</script>

<style scoped>
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

.favorite {
  color: rgb(230, 162, 60);
}

.parent {
  margin-right: 5px;
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
