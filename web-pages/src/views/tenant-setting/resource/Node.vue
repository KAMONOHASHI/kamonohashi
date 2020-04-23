<template>
  <div>
    <el-table
      ref="nodeTable"
      height="calc(100vh - 250px)"
      :data="tenantNodes"
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
            <el-table-column width="auto" />
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
      <el-table-column prop="name" label="ノード" width="auto" />
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

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  computed: {
    ...mapGetters(['tenantNodes']),
    columnWidth: function() {
      return '150px'
    },
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchTenantNodes']),
    async retrieveData() {
      await this.fetchTenantNodes()
    },
    handleEditOpen(row) {
      if (row) {
        this.$router.push('/manage/resource/' + row.nodeName + '/' + row.name)
      }
    },
    rowClassName({ row }) {
      if (row.containerResourceList && row.containerResourceList.length === 0) {
        return 'row-disabled'
      }
    },
    expandRow(refName, row) {
      this.$refs[refName].toggleRowExpansion(row)
    },
    closeDialog() {
      this.$router.push('/manage/resource/')
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
