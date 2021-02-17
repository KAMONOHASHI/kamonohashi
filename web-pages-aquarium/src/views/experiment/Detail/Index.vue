<template>
  <div>
    <h2>{{ title }}</h2>
    <el-row>
      <el-col :span="8" class="back">
        <span @click="openExperiment">
          実験一覧
        </span>
        <i class="el-icon-arrow-right" />
        <span>{{ id }}:{{ name }}</span>
      </el-col>
    </el-row>
    <el-tabs v-model="activeName">
      <el-tab-pane label="実行情報" name="info">
        <info v-model="infoForm" />
      </el-tab-pane>
      <el-tab-pane label="実行結果" name="result">
        <result :id="infoForm.id" />
      </el-tab-pane>
      <el-tab-pane label="推論" name="inference"><inference /></el-tab-pane>
    </el-tabs>
    <router-view @done="done" />
  </div>
</template>

<script>
import Info from './Info'
import Result from './Result'
import Inference from './Inference'
import { mapActions, mapGetters } from 'vuex'
export default {
  title: '実験詳細',
  components: { Info, Result, Inference },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      iconname: 'pl-plus',
      name: null,
      infoForm: {
        id: null,
        name: '',
        status: '',
        preprocessStatus: '',
        experimentPreprocessHistoryId: null,
        createdAt: '',
        createdBy: '',
        completedAt: '',
        dataSetId: null,
        dataSetName: '',
        dataSetVersion: null,
        templateId: null,
        templateName: '',
        templateVersion: null,
        dataSetURL: '',
        templateURL: '',
      },
      activeName: 'info',
    }
  },
  computed: {
    ...mapGetters({
      detail: ['experiment/detail'],
      events: ['experiment/events'],
      dataSets: ['aquariumDataSet/dataSets'],
      preprocessHistory: ['experiment/preprocessHistories'],
    }),
  },

  async created() {
    await this.initialize()
  },
  methods: {
    ...mapActions([
      'experiment/fetchDetail',
      'experiment/fetchPreprocessHistories',
      'experiment/fetchEvents',
      'experiment/postUserCancel',
      'experiment/postFiles',
      'experiment/put',
      'experiment/delete',
      'experiment/deleteFile',
      'aquariumDataSet/fetchDataSets',
    ]),
    async initialize() {
      this.title = '実験履歴'
      await this.retrieveData()
      this.name = this.detail.name
      this.infoForm.createdAt = this.detail.createdAt
      this.infoForm.createdBy = this.detail.createdBy
      this.infoForm.completedAt = this.detail.completedAt
      this.infoForm.id = this.detail.id
      this.infoForm.name = this.detail.name
      this.infoForm.status = this.detail.status
      this.infoForm.dataSetId = this.detail.dataSet.aquariumDataSetId
      this.infoForm.dataSetName = this.dataSets[0].name
      this.infoForm.dataSetVersion = this.detail.dataSet.version
      this.infoForm.templateId = this.detail.template.id
      this.infoForm.templateName = this.detail.template.name
      this.infoForm.templateVersion = this.detail.template.version
      this.infoForm.dataSetURL =
        '/aquarium/dataset/detail/' + this.detail.dataSet.aquariumDataSetId
      this.infoForm.templateURL =
        '/aquarium/model-template/' + this.detail.template.id
      this.infoForm.experimentPreprocessHistoryId = this.detail.experimentPreprocessHistoryId
      this.infoForm.preprocessStatus = this.preprocessHistory.status
    },
    async retrieveData() {
      await this['experiment/fetchDetail'](this.id)
      await this['aquariumDataSet/fetchDataSets']({
        id: this.detail.dataSet.aquariumDataSetId,
      })
      if (this.detail.experimentPreprocessHistoryId !== null) {
        await this['experiment/fetchPreprocessHistories']({
          id: this.detail.id,
        })
      }
    },
    async deleteJob() {
      try {
        await this['experiment/delete'](this.detail.id)
        this.$emit('done', 'delete')
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    async openExperiment() {
      this.$router.push('/aquarium/experiment')
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}
.back {
  padding-bottom: 20px;
  cursor: pointer;
  :hover {
    color: #409eff;
  }
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
