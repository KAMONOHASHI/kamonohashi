<template>
  <div>
    <h2>テナントリソース管理</h2>

    <br />
    <br />
    <el-table
      :data="containerList"
      class="table pl-index-table"
      border
      @row-click="handleEditOpen"
    >
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
import api from '@/api/v1/api'

export default {
  name: 'ManageResourceIndex',
  title: 'テナントリソース管理',
  data: function() {
    return {
      containerList: [],
      editDialogVisible: false,
      selectedRowId: 0,
    }
  },
  async created() {
    await this.getTenantsResource()
  },
  methods: {
    async getTenantsResource() {
      this.containerList = (await api.resource.tenant.getContainers()).data
    },
    async handleEditOpen(row) {
      if (row) {
        this.editDialogVisible = true
        this.selectedRowId = {
          name: row.name,
        }
      } else {
        this.selectedRowId = null
      }
    },
    async handleEditDone() {
      await this.retrieveData()
      this.editDialogVisible = false
    },
    async handleEditCancel() {
      this.editDialogVisible = false
    },
  },
}
</script>

<style lang="scss" scoped></style>
