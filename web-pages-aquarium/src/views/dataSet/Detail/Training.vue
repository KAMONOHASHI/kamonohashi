<template>
  <div>
    <h2>
      モデル
    </h2>
    <el-button type="primary" plain @click="openCreateDialog()">
      新しいモデルをトレーニング
    </el-button>
  </div>
</template>

<script>
export default {
  title: 'データセット',
  components: {},
  data() {
    return {}
  },
  computed: {},

  async created() {
    await this.retrieveData()
  },
  methods: {},
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
