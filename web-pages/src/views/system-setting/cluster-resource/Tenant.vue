<template>
  <div>
    <el-table
      ref="tenantTable"
      height="calc(100vh - 250px)"
      :data="tenants"
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
            <el-table-column align="right" prop="cpu" :width="columnWidth" />
            <el-table-column align="right" prop="memory" :width="columnWidth" />
            <el-table-column align="right" prop="gpu" :width="columnWidth" />
            <el-table-column
              align="center"
              prop="status"
              label="ステータス"
              :width="columnWidth"
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
        :width="columnWidth"
      />
      <el-table-column
        align="right"
        prop="memoryInfo"
        label="メモリ"
        :width="columnWidth"
      />
      <el-table-column
        align="right"
        prop="gpuInfo"
        label="GPU"
        :width="columnWidth"
      />
      <el-table-column align="center" label="ステータス" :width="columnWidth" />
    </el-table>
    <router-view @cancel="closeDialog" @done="done" />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

import * as gen from '@/api/api.generate'

export default Vue.extend({
  computed: {
    ...mapGetters(['tenants']),
    columnWidth: function() {
      return '150px'
    },
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchTenants']),
    async retrieveData() {
      await this.fetchTenants()
    },
    handleEditOpen(
      row: gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsOutputModel,
    ) {
      // tenantIdが-1のものはDBに登録されていないテナントのものであり詳細情報を取得できないため選択不可とする
      if (row && row.tenantId !== -1) {
        this.$router.push(
          '/cluster-resource/tenant/' + row.tenantId + '/' + row.name,
        )
      }
    },
    rowClassName({
      row,
    }: {
      row: gen.NssolPlatypusApiModelsResourceApiModelsTenantResourceOutputModel
    }) {
      if (row.containerResourceList && row.containerResourceList.length === 0) {
        return 'row-disabled'
      }
    },
    expandRow(
      refName: string,
      row: gen.NssolPlatypusApiModelsResourceApiModelsTenantResourceOutputModel,
    ) {
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
})
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
