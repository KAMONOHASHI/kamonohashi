<template>
  <div>
    <el-table
      ref="nodeTable"
      height="calc(100vh - 250px)"
      :data="nodes"
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
            <el-table-column prop="tenantName" width="auto" />
            <el-table-column prop="createdBy" width="auto">
              <template slot-scope="scope">
                <span>
                  {{ scope.row.createdBy
                  }}<span v-if="scope.row.displayNameCreatedBy"
                    >【{{ scope.row.displayNameCreatedBy }}】</span
                  >
                </span>
              </template>
            </el-table-column>
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
    ...mapGetters(['nodes']),
    columnWidth: function() {
      return '150px'
    },
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchNodes']),
    async retrieveData() {
      await this.fetchNodes()
    },
    handleEditOpen(row) {
      // tenantIdが-1のものはDBに登録されていないテナントのものであり詳細情報を取得できないため選択不可とする
      if (row && row.tenantId !== -1) {
        this.$router.push('/cluster-resource/' + row.tenantId + '/' + row.name)
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
