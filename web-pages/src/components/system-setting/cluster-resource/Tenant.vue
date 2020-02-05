<template>
  <div>
    <el-table
      ref="tenantTable"
      :data="tenantResource"
      align="right"
      class="table pl-index-table"
      :row-class-name="rowClassName"
      @row-click="expandRow('tenantTable', $event)"
    >
      <el-table-column type="expand">
        <template slot-scope="props">
          <el-table
            :data="props.row.containerResourceList"
            align="right"
            class="table"
            :show-header="false"
            @row-click="handleEditOpen"
          >
            <el-table-column width="auto" />
            <el-table-column prop="nodeName" width="auto" />
            <el-table-column prop="createdBy" width="auto" />
            <el-table-column prop="name" width="auto" />
            <el-table-column align="right" prop="cpu" :width="width1" />
            <el-table-column align="right" prop="memory" :width="width1" />
            <el-table-column align="right" prop="gpu" :width="width1" />
            <el-table-column
              align="center"
              prop="status"
              label="ステータス"
              :width="width1"
            />
          </el-table>
        </template>
      </el-table-column>
      <el-table-column prop="name" label="テナント" width="auto" />
      <el-table-column label="ノード" width="auto" />
      <el-table-column label="ユーザ" width="auto" />
      <el-table-column
        prop="containerResourceList.length"
        label="コンテナ"
        width="auto"
      />
      <el-table-column
        align="right"
        prop="cpuInfo"
        label="CPU"
        :width="width1"
      />
      <el-table-column
        align="right"
        prop="memoryInfo"
        label="メモリ"
        :width="width1"
      />
      <el-table-column
        align="right"
        prop="gpuInfo"
        label="GPU"
        :width="width1"
      />
      <el-table-column align="center" label="ステータス" :width="width1" />
    </el-table>
    <router-view @cancel="closeDialog" @done="done"></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'Tenant',
  data: function() {
    return {
      width1: '150px',
      tenantResource: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      this.tenantResource = (await api.resource.admin.getTenants()).data
    },
    handleEditOpen(row) {
      if (row) {
        this.$router.push(
          '/cluster-resource/tenant/' + row.tenantId + '/' + row.name,
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
      this.$router.push('/cluster-resource/tenant')
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
