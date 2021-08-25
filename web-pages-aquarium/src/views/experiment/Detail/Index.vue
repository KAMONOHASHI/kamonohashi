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
        <info v-if="childrenShow" :id="id" v-model="infoForm" />
      </el-tab-pane>
      <el-tab-pane label="推論" name="inference"
        ><inference v-if="childrenShow" :id="id" v-model="infoForm" />
      </el-tab-pane>

      <el-tab-pane label="デバッグ" name="debug"
        ><debug v-if="childrenShow" :id="id" v-model="infoForm" />
      </el-tab-pane>
    </el-tabs>
    <router-view @done="done" />
  </div>
</template>

<script>
import Info from './Info'
import Inference from './Inference'
import Debug from './Debug'
import { mapActions, mapGetters } from 'vuex'

export default {
  title: '実験詳細',
  // components: { Info, Inference, Debug },
  components: { Info, Inference, Debug },
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
      infoForm: null,
      activeName: 'info',
      childrenShow: false,
    }
  },
  computed: {
    ...mapGetters({
      detail: ['experiment/detail'],
      account: ['account/account'],
    }),
  },

  async created() {
    let tenantName = this.$route.query.tenantName
    await this['account/fetchAccount']()
    //テナント名からテナントIDを取得し、セットする
    for (let i in this.account.tenants) {
      if (this.account.tenants[i].name == tenantName) {
        await sessionStorage.setItem(
          '.Platypus.Tenant',
          this.account.tenants[i].id,
        )
        await sessionStorage.setItem('.Platypus.TenantName', tenantName)
        this.$store.commit('setLogin', {
          name: this.account.userName,
          tenant: this.account.tenants[i].id,
        })
        break
      }
    }

    let tab = this.$route.query.tab
    if (tab != null) {
      this.activeName = tab
    }
    this.childrenShow = true

    await this.initialize()
  },
  methods: {
    ...mapActions(['experiment/fetchDetail', 'account/fetchAccount']),
    tabChange() {
      this.$router.replace({
        query: { tab: this.activeName },
      })
    },
    async initialize() {
      let tenantName = await sessionStorage.getItem('.Platypus.TenantName')
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
        this.detail.dataSetVersion.version +
        '&tenantName=' +
        tenantName
      this.infoForm.templateURL =
        '/aquarium/model-template/' +
        this.detail.template.id +
        '?version=' +
        this.detail.templateVersion.version +
        '&tenantName=' +
        tenantName

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
