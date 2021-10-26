<template>
  <div>
    <el-table
      class="data-table pl-index-table"
      :data="allocatableNodes"
      border
      size="mini"
      :empty-text="emptyMessage"
    >
      <el-table-column prop="name" label="ノード名" width="150px" />
      <el-table-column prop="partition" label="パーティション" width="180px" />
      <el-table-column prop="allocatableCpu" label="CPU" width="80px" />
      <el-table-column
        prop="allocatableMemory"
        label="メモリ(GB)"
        width="100px"
      />
      <el-table-column prop="allocatableGpu" label="GPU" width="80px" />
    </el-table>
  </div>
</template>

<script>
export default {
  props: {
    allocatableNodes: {
      type: Array,
      default: () => [],
    },
    emptyMessage: {
      type: String,
      default: '利用可能なノードがありません。',
    },
  },
}
</script>

<style lang="scss" scoped></style>
