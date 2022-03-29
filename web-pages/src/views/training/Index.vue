<template>
  <div>
    <h2>学習管理</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
      <el-col class="right-top-button" :span="8">
        <el-button
          v-if="selections.length !== 0"
          @click="updateTagDialogVisible = true"
        >
          タグ変更
        </el-button>
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
      <el-dialog :visible.sync="updateTagDialogVisible" title="タグ変更">
        <el-form>
          <el-form-item>
            <el-col :span="24">
              <multi-input v-model="tags" />
            </el-col>
          </el-form-item>
        </el-form>
        <div class="right-top-button">
          <el-button type="primary" @click="updateTags('post')"
            >一括追加</el-button
          >
          <el-button type="primary" @click="updateTags('delete')"
            >一括削除</el-button
          >
        </div>
        <el-table :data="selections">
          <el-table-column prop="id" label="ID" width="120px" />
          <el-table-column prop="name" label="学習名" width="120px" />
          <el-table-column prop="createdAt" label="開始日時" width="200px" />
          <el-table-column
            prop="memo"
            label="メモ"
            width="auto"
            class-name="memo-column"
          />
          <el-table-column prop="tag" label="タグ" width="120px">
            <template slot-scope="scope">
              <span
                v-for="(tag, index) in scope.row.tags"
                :key="index"
                style="padding-left: 10px;"
              >
                <el-tag size="mini">
                  {{ tag }}
                </el-tag>
              </span>
            </template>
          </el-table-column>
        </el-table>
      </el-dialog>
    </el-row>
    <el-row :gutter="20">
      <el-col class="search">
        <el-button type="button" @click="searchDialogVisible = true"
          >詳細検索</el-button
        >
        <el-select
          v-model="searchConditionId"
          clearable
          placeholder="Select"
          @change="selectSearchCondition"
          @clear="clear"
        >
          <el-option
            v-if="searchingFlg"
            key="search"
            label="(詳細検索中)"
            value="search"
          ></el-option>
          <el-option
            v-for="item in searchHistories"
            :key="item.id"
            :label="item.name"
            :value="item.id"
            ><span style="float: left">{{ item.id }}:{{ item.name }}</span>
            <el-button
              style="float: right; color: #8492a6; font-size: 13px"
              size="mini"
              @click.stop="clickDeleteSearchHistory(item)"
              >x</el-button
            >
          </el-option>
        </el-select>
      </el-col>
      <search
        :search-dialog-visible="searchDialogVisible"
        :search-form="searchForm"
        @save="saveSearchCondition"
        @search="search"
        @close="searchDialogVisible = false"
      />
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
        <el-table-column prop="name" label="学習名" width="120px" />
        <el-table-column prop="createdAt" label="開始日時" width="200px" />
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
        <el-table-column prop="tag" label="タグ" width="120px">
          <template slot-scope="scope">
            <span
              v-for="(tag, index) in scope.row.tags"
              :key="index"
              style="padding-left: 10px;"
            >
              <el-tag size="mini">
                {{ tag }}
              </el-tag>
            </span>
          </template>
        </el-table-column>
        <el-table-column width="25px">
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
    <el-select v-model="test">
      <el-option
        v-for="item in testop"
        :key="item.val"
        :label="item.key"
        :value="item"
        ><span style="float: left">{{ item.key }}</span>
        <el-button
          style="float: right; color: #8492a6; font-size: 13px"
          @click="clickDeleteSearchHistory(item)"
          >x</el-button
        >
      </el-option></el-select
    >
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

<script>
import KqiPagination from '@/components/KqiPagination'
import Search from './Search'
import MultiInput from './MultiInput'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')

export default {
  title: '学習管理',
  components: {
    KqiPagination,
    Search,
    MultiInput,
  },
  data() {
    return {
      test: null,
      testop: [
        { key: 'aa', val: 1 },
        { key: 'bb', val: 2 },
      ],
      searchingFlg: false,
      updateTagDialogVisible: false,
      searchDialogVisible: false,
      saveSearchFormDialogVisible: false,
      selectBoxVisible: false, // 新規タグの入力エリアの表示有無
      tagValue: '', // 新規タグの入力値
      searchForm: {
        idLower: '',
        idUpper: '',
        name: [],
        nameOr: true,
        parentName: [],
        parentNameOr: true,
        startedAtLower: '',
        startedAtUpper: '',
        startedBy: [],
        startedByOr: true,
        dataSet: [],
        dataSetOr: true,
        memo: [],
        memoOr: true,
        status: [],
        statusOr: true,
        entryPoint: [],
        entryPointOr: true,
        tags: [],
        tagsOr: true,
      },
      tags: [],
      options: [],
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      selections: [],
      searchConditionId: null,
      searchCondition: null,
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: '学習名', type: 'text' },
        { prop: 'parentId', name: 'マウントした学習ID', type: 'number' },
        { prop: 'parentName', name: 'マウントした学習名', type: 'text' },
        { prop: 'startedAt', name: '開始日時', type: 'date' },
        { prop: 'startedBy', name: '実行者', type: 'text' },
        { prop: 'dataSet', name: 'データセット', type: 'text' },
        { prop: 'entryPoint', name: '実行コマンド', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
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
    ...mapGetters(['histories', 'total', 'searchHistories']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'fetchHistories',
      'delete',
      'fetchTrainHistories',
      'fetchSearchHistories',
      'postSearchHistory',
      'deleteSearchHistory',
      'postTags',
      'deleteTags',
    ]),
    async retrieveData() {
      let params
      if (this.searchCondition == null) {
        params = {}
      } else {
        params = this.searchCondition.searchDetail
      }
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchTrainHistories(params)
      await this.fetchSearchHistories()
    },

    clear() {
      this.searchConditionId = null
      this.searchForm = {
        idLower: '',
        idUpper: '',
        name: [],
        nameOr: true,
        parentName: [],
        parentNameOr: true,
        startedAtLower: '',
        startedAtUpper: '',
        startedBy: [],
        startedByOr: true,
        dataSet: [],
        dataSetOr: true,
        memo: [],
        memoOr: true,
        status: [],
        statusOr: true,
        entryPoint: [],
        entryPointOr: true,
        tags: [],
        tagsOr: true,
      }
      this.searchCondition = {
        searchDetail: this.changeSearchFormListToString(),
      }
      this.searchingFlg = false
    },

    async search() {
      this.pageStatus.currentPage = 1
      this.searchDialogVisible = false
      this.searchCondition = {
        searchDetail: this.changeSearchFormListToString(),
      }
      await this.retrieveData()
      this.searchConditionId = 'search'

      this.searchingFlg = true
    },

    changeSearchFormStringToList(item) {
      let form = {
        idLower: item.idLower,
        idUpper: item.idUpper,
        name: this.changeStringToList(item.name),
        nameOr: item.nameOr,
        parentName: this.changeStringToList(item.parentName),
        parentNameOr: item.parentNameOr,
        startedAtLower: item.startedAtLower,
        startedAtUpper: item.startedAtUpper,
        startedBy: this.changeStringToList(item.startedBy),
        startedByOr: item.startedByOr,
        dataSet: this.changeStringToList(item.dataSet),
        dataSetOr: item.dataSetOr,
        memo: this.changeStringToList(item.memo),
        memoOr: item.memoOr,
        status: this.changeStringToList(item.status),
        statusOr: item.statusOr,
        entryPoint: this.changeStringToList(item.entryPoint),
        entryPointOr: item.entryPointOr,
        tags: this.changeStringToList(item.tags),
        tagsOr: item.tagsOr,
      }
      return form
    },
    changeStringToList(str) {
      if (str == null || str.length == 0) {
        return []
      }
      let strs = str.split(',')
      let list = []
      for (let i in strs) {
        list.push(strs[i])
      }
      return list
    },

    changeSearchFormListToString() {
      let form = {
        idLower: this.searchForm.idLower,
        idUpper: this.searchForm.idUpper,
        name: this.changeListToString(this.searchForm.name),
        nameOr: this.searchForm.nameOr,
        parentName: this.changeListToString(this.searchForm.parentName),
        parentNameOr: this.searchForm.parentNameOr,
        startedAtLower: this.searchForm.startedAtLower,
        startedAtUpper: this.searchForm.startedAtUpper,
        startedBy: this.changeListToString(this.searchForm.startedBy),
        startedByOr: this.searchForm.startedByOr,
        dataSet: this.changeListToString(this.searchForm.dataSet),
        dataSetOr: this.searchForm.dataSetOr,
        memo: this.changeListToString(this.searchForm.memo),
        memoOr: this.searchForm.memoOr,
        status: this.changeListToString(this.searchForm.status),
        statusOr: this.searchForm.statusOr,
        entryPoint: this.changeListToString(this.searchForm.entryPoint),
        entryPointOr: this.searchForm.entryPointOr,
        tags: this.changeListToString(this.searchForm.tags),
        tagsOr: this.searchForm.tagsOr,
      }
      return form
    },

    changeListToString(list) {
      if (list == null || list.length == 0) {
        return null
      }
      let str = ''
      for (let i in list) {
        if (i == 0) {
          str = str + list[i]
        } else {
          str = str + ',' + list[i]
        }
      }
      return str
    },

    async saveSearchCondition() {
      await this.fetchSearchHistories()
    },
    async selectSearchCondition() {
      let params = {}
      this.searchCondition = null
      for (let i in this.searchHistories) {
        if (this.searchConditionId == this.searchHistories[i].id) {
          this.searchCondition = this.searchHistories[i]
        }
      }

      if (
        this.searchCondition != null &&
        this.searchCondition.searchDetail != null
      ) {
        params = this.searchCondition.searchDetail
        this.searchForm = this.changeSearchFormStringToList(
          this.searchCondition.searchDetail,
        )
      }
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchTrainHistories(params)
      this.searchForm = this.changeSearchFormStringToList(
        this.searchCondition.searchDetail,
      )
      this.searchingFlg = false
    },

    async clickDeleteSearchHistory(item) {
      await this.deleteSearchHistory(item.id)
      await this.fetchSearchHistories()
    },

    async updateTags(type) {
      let ids = []
      for (let i in this.selections) {
        ids.push(this.selections[i].id)
      }
      let params = { id: ids, tags: this.tags }

      if (type == 'post') {
        await this.postTags(params)
      } else if (type == 'delete') {
        await this.deleteTags({ data: params })
      }

      this.tags = []
      this.retrieveData()
      this.updateTagDialogVisible = false
    },

    handleSelectionChange(val) {
      // checkboxの要素変更
      this.selections = val
    },
    async showDeleteConfirm() {
      let confirmMessage = `学習履歴を${this.selections.length}件削除しますか（出力データ数が多い場合、処理に時間がかかります）`
      await this.$confirm(confirmMessage, 'Warning', {
        distinguishCancelAndClose: true,
        confirmButtonText: 'はい',
        cancelButtonText: 'キャンセル',
        type: 'warning',
      })
        .then(async () => {
          let successCount = 0
          for (let selection of this.selections) {
            await this.delete(selection.id)
              .then(() => successCount++)
              .catch(() => {})
          }
          await this.$notify.info({
            type: 'info',
            message: `学習履歴を削除しました。(成功：${successCount}件、 失敗：${this
              .selections.length - successCount}件`,
          })
          this.pageStatus.currentPage = 1
          await this.retrieveData()
        })
        .catch(() => {})
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

    closeDialog() {
      this.$router.push('/training')
    },
    back() {
      this.$router.go(-1)
    },
    openEditDialog(selectedRow) {
      this.$router.push('/training/' + selectedRow.id)
    },
    openCreateDialog() {
      this.$router.push('/training/run/')
    },
    copyCreate(parentId) {
      this.$router.push('/training/run/' + parentId)
    },
    files(id) {
      this.$router.push('/training/' + id + '/files')
    },
    shell(id) {
      this.$router.push('/training/' + id + '/shell')
    },
    log(id) {
      this.$router.push('/training/' + id + '/log')
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

.favorite {
  color: rgb(230, 162, 60);
}

.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

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

.el-tag--mini {
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.parent {
  margin-right: 5px;
}
</style>
