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
        <result />
      </el-tab-pane>
      <el-tab-pane label="推論" name="inference"><inference /></el-tab-pane>
    </el-tabs>
    <router-view @done="done" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import Info from './Info'
import Result from './Result'

import Inference from './Inference'
const { mapGetters, mapActions } = createNamespacedHelpers('experiment')

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
        createdAt: '',
        createdBy: '',
        dataSetId: null,
        dataSetVersion: null,
        templateId: null,
        templateName: '',
        dataSetURL: '',
        templateURL: '',
      },
      activeName: 'info',
    }
  },
  computed: {
    ...mapGetters(['detail', 'events']),
  },

  async created() {
    await this.initialize()
  },
  methods: {
    ...mapActions([
      'fetchDetail',
      'fetchEvents',
      'postUserCancel',
      'postFiles',
      'put',
      'delete',
      'deleteFile',
    ]),
    async initialize() {
      this.title = '実験履歴'
      await this.retrieveData()
      this.name = this.detail.name
      this.infoForm.createdAt = this.detail.createdAt
      this.infoForm.createdBy = this.detail.createdBy
      this.infoForm.id = this.detail.id
      this.infoForm.dataSetId = this.detail.dataSet.aquariumDataSetId
      this.infoForm.dataSetVersion = this.detail.dataSet.version
      this.infoForm.templateId = this.detail.template.id
      this.infoForm.templateName = this.detail.template.name
      this.infoForm.dataSetURL =
        '/aquarium/dataset/detail/' + this.detail.dataSet.aquariumDataSetId
      this.infoForm.templateURL =
        '/aquarium/model-template/' + this.detail.template.id
    },
    async retrieveData() {
      await this.fetchDetail(this.id)
      if (
        this.detail.statusType === 'Running' ||
        this.detail.statusType === 'Error'
      ) {
        await this.fetchEvents(this.detail.id)
      }
    },
    async deleteJob() {
      try {
        await this.delete(this.detail.id)
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
