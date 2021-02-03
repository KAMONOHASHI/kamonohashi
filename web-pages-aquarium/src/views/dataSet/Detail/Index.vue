<template>
  <div>
    <h2>{{ name }}</h2>
    <el-tabs v-model="activeName">
      <el-tab-pane label="アップロード" name="upload">
        <upload
          :id="id"
          :datasetname="name"
          @latestVersionId="setVersionId"
          @handleTabChange="changeActiveTab"
        />
      </el-tab-pane>
      <el-tab-pane label="イメージ" name="image">
        <images :id="id" :latest-version-id="latestVersionId" />
      </el-tab-pane>
    </el-tabs>
    <router-view @done="done" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import Upload from './Upload'
import Images from './Images'
const { mapGetters, mapActions } = createNamespacedHelpers('aquariumDataSet')

export default {
  title: 'データセット',
  components: { Upload, Images },
  props: {
    id: {
      type: String,
      default: null,
    },
  },

  data() {
    return {
      iconname: 'pl-plus',
      searchCondition: {},
      latestVersionId: null,
      activeName: 'image',
      name: null,
    }
  },
  computed: {
    ...mapGetters(['versions', 'dataSets']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchVersions', 'fetchDataSets']),
    setVersionId(x) {
      this.latestVersionId = x
    },
    changeActiveTab(tabName) {
      this.activeName = tabName
    },
    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      await this.fetchDataSets({ id: this.id })
      this.name = this.dataSets[0].name
    },

    async search() {
      await this.retrieveData()
    },

    async done() {
      await this.retrieveData()
      this.closeDialog()
      this.showSuccessMessage()
    },
    closeDialog() {
      this.$router.push('/aquarium/dataset')
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
