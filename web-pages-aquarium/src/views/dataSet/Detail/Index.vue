<template>
  <div>
    <h2>{{ dataSets[0].name }}</h2>
    <el-tabs v-model="activeName">
      <el-tab-pane label="アップロード" name="upload">
        <upload :id="id" :datasetname="dataSets[0].name" />
      </el-tab-pane>
      <el-tab-pane label="イメージ" name="image">
        <images :id="id" />
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
  data() {
    return {
      iconname: 'pl-plus',
      searchCondition: {},

      tableData: [],
      activeName: 'upload',
    }
  },
  computed: {
    ...mapGetters(['versions', 'dataSets']),
  },

  props: {
    id: {
      type: String,
      default: null,
    },
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchVersions', 'fetchDataSets']),
    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      await this.fetchDataSets(this.id)
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
