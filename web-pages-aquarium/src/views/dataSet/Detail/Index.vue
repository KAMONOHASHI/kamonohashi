<template>
  <div>
    <h2>{{ name }}</h2>
    <el-tabs v-model="activeName">
      <el-tab-pane label="アップロード" name="upload">
        <upload :id="id" :datasetname="name" @latestVersionId="setVersionId" />
      </el-tab-pane>
      <el-tab-pane label="イメージ" name="image">
        <images :id="id" :latest-version-id="latestVersionId" />
      </el-tab-pane>
    </el-tabs>
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
      activeName: 'upload',
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
    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      await this.fetchDataSets({ id: this.id })
      this.name = this.dataSets[0].name
    },

    openCreateDialog() {
      this.$router.push('/dataset/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/dataset/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      await this.retrieveData()
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
