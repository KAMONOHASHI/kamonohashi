<template>
  <div>
    <el-col class="pagination" :span="10">
      <el-pagination layout="total" :total="containerLists.length" />
    </el-col>
    <el-table
      :data="containerLists"
      class="table pl-index-table"
      border
      @row-click="handleEditOpen"
    >
      <el-table-column prop="nodeName" label="ノード" width="auto" />
      <el-table-column prop="tenantName" label="テナント" width="auto" />
      <el-table-column prop="createdBy" label="ユーザ" width="auto" />
      <el-table-column prop="name" label="コンテナ" width="auto" />
      <el-table-column
        align="right"
        prop="cpu"
        label="CPU"
        :width="columnWidth"
      />
      <el-table-column
        align="right"
        prop="memory"
        label="メモリ"
        :width="columnWidth"
      />
      <el-table-column
        align="right"
        prop="gpu"
        label="GPU"
        :width="columnWidth"
      />
      <el-table-column
        align="center"
        prop="status"
        label="ステータス"
        :width="columnWidth"
      />
    </el-table>
    <router-view @cancel="closeDialog" @done="done" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  computed: {
    ...mapGetters(['containerLists']),
    columnWidth: function() {
      return '150px'
    },
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchContainerLists']),
    async retrieveData() {
      await this.fetchContainerLists()
    },
    handleEditOpen(row) {
      if (row) {
        this.$router.push(
          '/cluster-resource/container-list/' + row.tenantId + '/' + row.name,
        )
      }
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

<style scoped></style>
