<template>
  <div>
    <h2>テナントリソース管理2</h2>

    <br />
    <br />
    <el-table :data="tenantContainerLists" class="table pl-index-table" border>
      <el-table-column prop="name" label="コンテナ" width="auto" />
      <el-table-column prop="createdBy" label="ユーザ" width="auto" />
      <el-table-column prop="nodeName" label="ノード" width="auto" />
      <el-table-column prop="cpu" label="CPU" width="auto" />
      <el-table-column prop="memory" label="メモリ" width="auto" />
      <el-table-column prop="gpu" label="GPU" width="auto" />
      <el-table-column prop="status" label="ステータス" width="auto" />
    </el-table>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  title: 'テナントリソース管理',
  data: function() {
    return {
      containerList: [],
      editDialogVisible: false,
      selectedRowId: 0,
    }
  },
  computed: {
    ...mapGetters(['tenantContainerLists']),
  },
  async created() {
    await this.fetchTenantContainerLists()
  },
  methods: {
    ...mapActions(['fetchTenantContainerLists']),
  },
}
</script>

<style lang="scss" scoped></style>
