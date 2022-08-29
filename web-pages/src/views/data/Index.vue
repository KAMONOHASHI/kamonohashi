<template>
  <div>
    <h2>データ管理</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
      <el-col class="right-top-button" :span="8">
        <div>
          <el-button
            v-if="$store.getters['account/isAvailablePreprocessing']"
            @click="openPreprocessingDialog"
          >
            前処理実行
          </el-button>
          <el-button
            icon="el-icon-edit-outline"
            type="primary"
            plain
            @click="openCreateDialog"
          >
            新規登録
          </el-button>
        </div>
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
        :data="data"
        border
        @row-click="openEditDialog"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55px" />
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="データ名" width="auto" />
        <el-table-column label="元データ名" width="120px">
          <div slot-scope="scope">
            <div v-if="scope.row.isRaw"></div>
            <div v-else>
              {{ scope.row.parentDataName }}
            </div>
          </div>
        </el-table-column>
        <el-table-column prop="createdAt" label="登録日時" width="200px" />
        <el-table-column prop="createdBy" label="登録者" width="120px" />
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
    </el-row>
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
    </el-row>
    <router-view
      @cancel="closeDialog()"
      @done="done"
      @preprocessing="openPreprocessingDialog()"
      @close="closeDialog()"
      @runPreprocessing="redirectPreprocessingPage()"
    />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiPagination from '@/components/KqiPagination.vue'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('data')
import * as gen from '@/api/api.generate'

import { DeepWriteable } from '@/@types/type'
type DataApiApiV2DataGetRequest = DeepWriteable<gen.DataApiApiV2DataGetRequest>

interface DataType {
  pageStatus: {
    currentPage: number
    currentPageSize: number
  }
  searchCondition: DataApiApiV2DataGetRequest
  searchConfigs: Array<{
    prop: string
    name: string
    type: string
    multiple?: boolean
  }>
  selections: Array<gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel>
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
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データ名', type: 'text' },
        { prop: 'createdAt', name: '登録日時', type: 'date' },
        { prop: 'createdBy', name: '登録者', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
      ],
      selections: [],
    }
  },
  computed: {
    ...mapGetters(['data', 'total']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchData']),
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchData(params)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
    // checkboxの要素変更
    handleSelectionChange(
      val: Array<gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel>,
    ) {
      this.selections = val
    },
    openEditDialog(
      selectedRow: gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel,
    ) {
      this.$router.push('/data/edit/' + selectedRow.id)
    },
    openCreateDialog() {
      this.$router.push('/data/edit')
    },
    closeDialog() {
      this.$router.push('/data')
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
    openPreprocessingDialog() {
      if (this.selections.length === 0) {
        this.$router.push('/preprocessing/run')
      } else {
        let selectionString = ''
        this.selections.forEach(value => {
          if (value.id != undefined) {
            selectionString += value.id.toString() + ' '
          }
        })
        this.$router.push('/data/' + selectionString + '/preprocessing')
      }
    },
    redirectPreprocessingPage() {
      this.$router.push('/preprocessing')
    },
  },
})
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

.el-tag--mini {
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
