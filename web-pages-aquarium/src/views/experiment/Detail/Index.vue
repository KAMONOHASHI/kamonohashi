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
    <el-tabs v-model="activeName" @tab-click="tabChange">
      <el-tab-pane label="実行情報" name="info">
        <info :id="id" v-model="infoForm" />
      </el-tab-pane>
      <el-tab-pane label="推論" name="inference"
        ><inference :id="id" v-model="infoForm" />
      </el-tab-pane>

      <el-tab-pane label="デバッグ" name="debug"
        ><debug :id="id" v-model="infoForm" />
      </el-tab-pane>
    </el-tabs>
    <router-view @done="done" />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import Info from './Info.vue'
//@ts-ignore
import Inference from './Inference.vue'
import Debug from './Debug.vue'
import * as gen from '@/api/api.generate'

import { mapActions, mapGetters } from 'vuex'
interface DataType {
  iconname: string
  name: null
  infoForm: Form | null
  activeName: string | Array<string | null>
}
interface Form {
  createdAt?: null | string
  createdBy?: null | string
  completedAt?: null | string
  id?: number
  name?: null | string
  status?: null | string
  dataSetId?: number
  dataSetName?: null | string
  dataSetVersion?: gen.NssolPlatypusApiModelsAquariumDataSetApiModelsVersionIndexOutputModel
  templateId?: number
  templateName?: null | string
  templateVersion?: gen.NssolPlatypusApiModelsTemplateApiModelsVersionIndexOutputModel
  dataSetURL?: null | string
  templateURL?: null | string
  preprocessId?: null | number
  preprocessStatus?: null | string
  trainingId?: null | number
  trainingStatus?: null | string
}
export default Vue.extend({
  // components: { Info, Inference, Debug },
  components: { Info, Inference, Debug },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    return {
      iconname: 'pl-plus',
      name: null,
      infoForm: null,
      activeName: 'info',
    }
  },
  computed: {
    //@ts-ignore
    ...mapGetters({
      detail: ['experiment/detail'],
    }),
  },

  async created() {
    let tab = this.$route.query.tab
    if (tab != null) {
      this.activeName = tab
    }
    await this.initialize()
  },
  methods: {
    ...mapActions(['experiment/fetchDetail']),
    tabChange() {
      this.$router.replace({
        query: { tab: this.activeName },
      })
    },
    async initialize() {
      this.title = '実験履歴'
      await this.retrieveData()
      this.name = this.detail.name
      this.infoForm = {}
      this.infoForm.createdAt = this.detail.createdAt
      this.infoForm.createdBy = this.detail.createdBy
      this.infoForm.completedAt = this.detail.completedAt
      this.infoForm.id = this.detail.id
      this.infoForm.name = this.detail.name
      this.infoForm.status = this.detail.status

      this.infoForm.dataSetId = this.detail.dataSet.id
      this.infoForm.dataSetName = this.detail.dataSet.name
      this.infoForm.dataSetVersion = this.detail.dataSetVersion.version
      this.infoForm.templateId = this.detail.template.id
      this.infoForm.templateName = this.detail.template.name
      this.infoForm.templateVersion = this.detail.templateVersion.version
      this.infoForm.dataSetURL =
        '/aquarium/dataset/detail/' +
        this.detail.dataSet.id +
        '?version=' +
        this.detail.dataSetVersion.version
      this.infoForm.templateURL =
        '/aquarium/model-template/' +
        this.detail.template.id +
        '?version=' +
        this.detail.templateVersion.version

      if (this.detail.preprocess != null) {
        this.infoForm.preprocessId = this.detail.preprocess.id
        this.infoForm.preprocessStatus = this.detail.preprocess.status
      } else {
        this.infoForm.preprocessId = null
        this.infoForm.preprocessStatus = null
      }
      if (this.detail.training != null) {
        this.infoForm.trainingId = this.detail.training.id
        this.infoForm.trainingStatus = this.detail.training.status
      } else {
        this.infoForm.trainingId = null
        this.infoForm.trainingStatus = null
      }
    },
    async retrieveData() {
      await this['experiment/fetchDetail'](this.id)
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
})
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
