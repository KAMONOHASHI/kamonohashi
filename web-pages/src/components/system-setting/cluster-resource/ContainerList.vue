<template>
  <div>
    <el-col class="pagination" :span="10">
      <el-pagination layout="total" :total="total" />
    </el-col>
    <el-table
      :data="containerList"
      class="table pl-index-table"
      border
      @row-click="handleEditOpen"
    >
      <el-table-column prop="nodeName" label="ノード" width="auto" />
      <el-table-column prop="tenantName" label="テナント" width="auto" />
      <el-table-column prop="createdBy" label="ユーザ" width="auto" />
      <el-table-column prop="name" label="コンテナ" width="auto" />
      <el-table-column align="right" prop="cpu" label="CPU" :width="width1" />
      <el-table-column
        align="right"
        prop="memory"
        label="メモリ"
        :width="width1"
      />
      <el-table-column align="right" prop="gpu" label="GPU" :width="width1" />
      <el-table-column
        align="center"
        prop="status"
        label="ステータス"
        :width="width1"
      />
    </el-table>
    <router-view @cancel="closeDialog" @done="done"></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'List',
  data: function() {
    return {
      width1: '150px',
      containerList: [],
      total: 0,
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      this.containerList = (await api.resource.admin.getContainers()).data
      this.total = this.containerList.length
    },
    handleEditOpen(row) {
      if (row) {
        this.$router.push(
          '/cluster-resource/container-list/' + row.tenantId + '/' + row.name,
        )
      }
    },
    rowClassName(row) {
      if (row.containerResourceList && row.containerResourceList.length === 0) {
        return 'row-disabled'
      }
    },
    expandRow(refName, row) {
      this.$refs[refName].toggleRowExpansion(row)
    },
    closeDialog() {
      this.$router.push('/cluster-resource/container-list')
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
  },
}
</script>

<style scoped>
.table /deep/ .el-table__expanded-cell {
  padding-top: 0px !important;
  padding-right: 0px !important;
}

.table /deep/ .row-disabled {
  background-color: #f0f0f0;
}
</style>
