<template>
  <div>
    <h3>コンテナリソース</h3>
    <el-row class="row">
      <el-col :span="3">保存されている履歴</el-col>
      <el-col :span="1"> 開始日 </el-col>
      <el-col :span="5" class="grid-content">
        {{ allContainersStartDate }}
      </el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="5" class="grid-content">
        {{ allContainersEndDate }}</el-col
      >
      <el-col :span="2">件数</el-col>
      <el-col :span="3">{{ containersCount }}件</el-col>
      <el-col :span="4">
        <el-button
          type="primary"
          plain
          icon="el-icon-refresh"
          @click="handleUpdateContainersData"
        >
          更新
        </el-button>
      </el-col>
    </el-row>
    <el-row class="row">
      <el-col :span="3">履歴のダウンロード</el-col>
      <el-col :span="1">開始日</el-col>
      <el-col :span="5">
        <el-date-picker
          v-model="containersHistoryStartDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="5">
        <el-date-picker
          v-model="containersHistoryEndDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="2">ヘッダー</el-col>
      <el-col :span="3">
        <el-switch
          v-model="containersHeader"
          active-text="あり"
          inactive-text="なし"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="primary"
          plain
          icon="el-icon-download"
          @click="handleContainersDownload"
        />
      </el-col>
    </el-row>
    <el-row class="row">
      <el-col :span="9">履歴の削除</el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="10">
        <el-date-picker
          v-model="containersDeleteEndDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="danger"
          icon="el-icon-delete"
          @click="handleContainersDelete"
        />
      </el-col>
    </el-row>

    <h3>ジョブ実行履歴</h3>
    <el-row class="row">
      <el-col :span="3">保存されている履歴</el-col>
      <el-col :span="1">開始日</el-col>
      <el-col :span="5" class="grid-content">
        {{ allJobsStartDate }}
      </el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="5" class="grid-content">
        {{ allJobsEndDate }}
      </el-col>

      <el-col :span="2">件数</el-col>
      <el-col :span="3">{{ jobsCount }}件</el-col>
      <el-col :span="4">
        <el-button
          type="primary"
          plain
          icon="el-icon-refresh"
          @click="handleUpdateJobsData"
        >
          更新
        </el-button>
      </el-col>
    </el-row>
    <el-row class="row">
      <el-col :span="3">履歴のダウンロード</el-col>
      <el-col :span="1">開始日</el-col>
      <el-col :span="5">
        <el-date-picker
          v-model="jobsHistoryStartDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="5">
        <el-date-picker
          v-model="jobsHistoryEndDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="2">ヘッダー</el-col>
      <el-col :span="3">
        <el-switch
          v-model="jobsHeader"
          active-text="あり"
          inactive-text="なし"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="primary"
          plain
          icon="el-icon-download"
          @click="handleJobsDownload"
        />
      </el-col>
    </el-row>
    <el-row class="row">
      <el-col :span="9">履歴の削除</el-col>
      <el-col :span="1">終了日</el-col>
      <el-col :span="10">
        <el-date-picker
          v-model="jobsDeleteEndDate"
          type="date"
          placeholder="Pick a day"
        />
      </el-col>
      <el-col :span="4">
        <el-button
          type="danger"
          icon="el-icon-delete"
          @click="handleJobsDelete"
        />
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

interface DataType {
  // コンテナリソース
  allContainersStartDate: string
  allContainersEndDate: string
  containersCount: string
  containersHistoryStartDate: null | string
  containersHistoryEndDate: null | string
  containersHeader: boolean
  containersDeleteEndDate: null | string

  // ジョブ実行履歴
  allJobsStartDate: string
  allJobsEndDate: string
  jobsCount: string
  jobsHistoryStartDate: null | string
  jobsHistoryEndDate: null | string
  jobsHeader: boolean
  jobsDeleteEndDate: null | string
}

export default Vue.extend({
  data(): DataType {
    return {
      // コンテナリソース
      allContainersStartDate: '　',
      allContainersEndDate: '　',
      containersCount: '－',
      containersHistoryStartDate: null,
      containersHistoryEndDate: null,
      containersHeader: true,
      containersDeleteEndDate: null,

      // ジョブ実行履歴
      allJobsStartDate: '　',
      allJobsEndDate: '　',
      jobsCount: '－',
      jobsHistoryStartDate: null,
      jobsHistoryEndDate: null,
      jobsHeader: true,
      jobsDeleteEndDate: null,
    }
  },
  computed: {
    ...mapGetters([
      'nodes',
      'historiesContainersMetadata',
      'historiesContainersData',
      'historiesJobsMetadata',
      'historiesJobsData',
    ]),
  },
  methods: {
    ...mapActions([
      'fetchHistoriesContainersMetadata',
      'fetchHistoriesContainersData',
      'deleteHistoriesContainers',
      'fetchHistoriesJobsMetadata',
      'fetchHistoriesJobsData',
      'deleteHistoriesJobs',
    ]),
    async handleUpdateContainersData() {
      await this.fetchHistoriesContainersMetadata()
      this.allContainersStartDate = this.historiesContainersMetadata.startDate
      this.allContainersEndDate = this.historiesContainersMetadata.endDate
      this.containersCount = this.historiesContainersMetadata.count
    },
    async handleContainersDownload() {
      let param = {
        startDate: this.containersHistoryStartDate,
        endDate: this.containersHistoryEndDate,
        withHeader: this.containersHeader,
      }
      await this.fetchHistoriesContainersData(param)
      let blob = new Blob([this.historiesContainersData], {
        type: 'text/csv',
      })
      let link = document.createElement('a')
      link.href = window.URL.createObjectURL(blob)
      let date: Date | string = new Date()
      date =
        date.getFullYear() +
        ('0' + (date.getMonth() + 1)).slice(-2) +
        ('0' + date.getDate()).slice(-2) +
        ('0' + date.getHours()).slice(-2) +
        ('0' + date.getMinutes()).slice(-2) +
        ('0' + date.getSeconds()).slice(-2)
      // ファイル名の指定
      link.download = date + '_container.csv'
      link.click()
    },
    async handleContainersDelete() {
      let confirmMessage = ''
      if (this.containersDeleteEndDate == null) {
        confirmMessage = 'すべてのコンテナリソース履歴を削除しますか'
      } else {
        confirmMessage = `${this.containersDeleteEndDate.getFullYear() +
          '/' +
          ('0' + (this.containersDeleteEndDate.getMonth() + 1)).slice(-2) +
          '/' +
          ('0' + this.containersDeleteEndDate.getDate()).slice(
            -2,
          )}までのコンテナリソース履歴を削除しますか`
      }
      // 確認ダイアログ
      await this.$confirm(confirmMessage, 'Warning', {
        distinguishCancelAndClose: true,
        confirmButtonText: 'はい',
        cancelButtonText: 'キャンセル',
        type: 'warning',
      })
        .then(async () => {
          let param = {
            endDate: this.containersDeleteEndDate,
          }
          await this.deleteHistoriesContainers(param).then(() =>
            this.showSuccessMessage(),
          )
        })
        .catch(() => {})
    },

    async handleUpdateJobsData() {
      await this.fetchHistoriesJobsMetadata()
      this.allJobsStartDate = this.historiesJobsMetadata.startDate
      this.allJobsEndDate = this.historiesJobsMetadata.endDate
      this.jobsCount = this.historiesJobsMetadata.count
    },
    async handleJobsDownload() {
      let param = {
        startDate: this.jobsHistoryStartDate,
        endDate: this.jobsHistoryEndDate,
        withHeader: this.jobsHeader,
      }

      await this.fetchHistoriesJobsData(param)
      let blob = new Blob([this.historiesJobsData], {
        type: 'text/csv',
      })
      let link = document.createElement('a')
      link.href = window.URL.createObjectURL(blob)
      let date: Date | string = new Date()
      date =
        date.getFullYear() +
        ('0' + (date.getMonth() + 1)).slice(-2) +
        ('0' + date.getDate()).slice(-2) +
        ('0' + date.getHours()).slice(-2) +
        ('0' + date.getMinutes()).slice(-2) +
        ('0' + date.getSeconds()).slice(-2)
      // ファイル名の指定
      link.download = date + '_job.csv'
      link.click()
    },

    async handleJobsDelete() {
      let confirmMessage = ''
      if (this.jobsDeleteEndDate == null) {
        confirmMessage = 'すべてのジョブ実行履歴を削除しますか'
      } else {
        confirmMessage = `${this.jobsDeleteEndDate.getFullYear() +
          '/' +
          ('0' + (this.jobsDeleteEndDate.getMonth() + 1)).slice(-2) +
          '/' +
          ('0' + this.jobsDeleteEndDate.getDate()).slice(
            -2,
          )}までのジョブ実行履歴を削除しますか`
      }
      // 確認ダイアログ
      await this.$confirm(confirmMessage, 'Warning', {
        distinguishCancelAndClose: true,
        confirmButtonText: 'はい',
        cancelButtonText: 'キャンセル',
        type: 'warning',
      })
        .then(async () => {
          let param = {
            endDate: this.jobsDeleteEndDate,
          }
          await this.deleteHistoriesJobs(param).then(() =>
            this.showSuccessMessage(),
          )
        })
        .catch(() => {})
    },
  },
})
</script>

<style scoped>
.row {
  font-size: 14px;
  line-height: 43px;
  padding: 5px;
  margin-left: 20px;
  border-bottom: 1px solid #ddd;
}
h3 {
  padding-top: 30px;
  padding-bottom: 10px;
  border-bottom: 1px solid #ddd;
}
.grid-content {
  min-height: 36px;
}
.el-date-editor.el-input {
  max-width: 220px !important;
  width: 100%;
  min-width: 160px !important;
}
</style>
