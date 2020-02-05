<template>
  <div>
    <el-table
      ref="nodeTable"
      :data="nodeResource"
      align="right"
      class="table pl-index-table"
      :row-class-name="rowClassName"
      @row-click="expandRow('nodeTable', $event)"
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
            <el-table-column width="auto"></el-table-column>
            <el-table-column prop="tenantName" width="auto" />
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
      <el-table-column prop="name" label="ノード" width="auto" />
      <el-table-column label="テナント" width="auto" />
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
  name: 'NodeResource',
  data: function() {
    return {
      width1: '150px',
      nodeResource: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      this.nodeResource = (await api.resource.admin.getNodes()).data
    },
    handleEditOpen(row) {
      if (row) {
        this.$router.push('/cluster-resource/' + row.tenantId + '/' + row.name)
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
      this.$router.push('/cluster-resource/')
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
