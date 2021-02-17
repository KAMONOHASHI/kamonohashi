<template>
  <div>
    <h2>実験一覧</h2>

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
          新しく実験を開始する
        </el-button>
      </el-col>
    </el-row>

    <el-row class="test">
      <el-table
        class="data-table pl-index-table"
        :data="viewHistoryes"
        border
        @row-click="openEditExperiment"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="名前" width="auto" />

        <el-table-column
          prop="dataSet.aquariumDataSetName"
          label="データセット"
          width="auto"
        />

        <el-table-column
          prop="template.name"
          label="テンプレート"
          width="auto"
        />

        <el-table-column prop="createdAt" label="開始日時" width="auto" />
        <el-table-column label="ステータス" width="auto">
          <div slot-scope="scope">
            <div
              v-if="
                (scope.row.status === 'None') | (scope.row.status === 'Pending')
              "
            >
              <i class="el-icon-time" style="color: #889683;" />
              学習実行前
            </div>
            <div v-else-if="scope.row.status === 'Running'">
              <i class="el-icon-success" style="color: #67C23A;" />
              実行中
            </div>
            <div v-else-if="scope.row.status === 'Completed'">
              <i class="el-icon-success" style="color: #67C23A;" />
              完了
            </div>
            <div v-else>
              <i class="el-icon-warning" style="color: #E6A23C;" />
              {{ scope.row.status }}
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
      @done="done"
      @return="back"
      @copy="handleCopy"
    />
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
//import { createNamespacedHelpers } from 'vuex'
//const { mapGetters, mapActions } = createNamespacedHelpers('experiment')
import { mapActions, mapGetters } from 'vuex'
export default {
  title: '実験',
  components: {
    KqiPagination,
  },
  data() {
    return {
      viewHistoryes: [],
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'name', name: '名前', type: 'text' },
        {
          prop: 'dataSet.aquariumDataSetId',
          name: 'データセット',
          type: 'text',
        },
        { prop: 'template.name', name: 'テンプレート', type: 'text' },
        { prop: 'outputValue', name: '平均適合率', type: 'text' },

        { prop: 'createdAt', name: '開始日時', type: 'date' },
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
      tableData: [],
    }
  },
  computed: {
    ...mapGetters({
      histories: ['experiment/histories'],
      total: ['experiment/total'],
      dataSets: ['aquariumDataSet/dataSets'],
    }),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'experiment/fetchHistories',
      'experiment/delete',
      'aquariumDataSet/fetchDataSets',
    ]),

    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this['experiment/fetchHistories'](params)
      //実験履歴一覧それぞれについてデータセット名を取得する
      this.viewHistoryes = []
      for (let i in this.histories) {
        await this['aquariumDataSet/fetchDataSets']({
          id: this.histories[i].dataSet.aquariumDataSetId,
        })
        let obj = JSON.parse(JSON.stringify(this.histories[i]))
        obj['dataSet']['aquariumDataSetName'] = this.dataSets[0].name

        this.viewHistoryes.push(obj)
      }
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
      this.$router.push('/aquarium/experiment')
    },
    back() {
      this.$router.go(-1)
    },
    openCreateDialog() {
      this.$router.push('/aquarium/experiment/create')
    },
    openEditExperiment(selectedRow) {
      this.$router.push('/aquarium/experiment/detail/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/aquarium/experiment/create/' + id)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
    files(id) {
      this.$router.push('/aquarium/experiment/' + id + '/files')
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
