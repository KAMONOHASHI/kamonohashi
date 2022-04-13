<template>
  <div>
    <el-col class="pagination" :span="10">
      <el-pagination layout="total" :total="tenantContainerLists.length" />
    </el-col>
    <el-table
      height="calc(100vh - 270px)"
      :data="tenantContainerLists"
      class="table pl-index-table"
      border
      @row-click="handleEditOpen"
    >
      <el-table-column prop="name" label="コンテナ" width="auto" />
      <el-table-column prop="createdBy" label="ユーザ" width="auto">
        <template slot-scope="scope">
          <span>
            {{ scope.row.createdBy }}【{{ scope.row.displayNameCreatedBy }}】
          </span>
        </template>
      </el-table-column>
      <el-table-column prop="nodeName" label="ノード" width="auto" />
      <el-table-column prop="cpu" label="CPU" width="auto" />
      <el-table-column prop="memory" label="メモリ" width="auto" />
      <el-table-column prop="gpu" label="GPU" width="auto" />
      <el-table-column prop="status" label="ステータス" width="auto" />
    </el-table>
    <router-view @cancel="closeDialog()" @done="done()" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  computed: {
    ...mapGetters(['tenantContainerLists']),
  },
  async created() {
    await this.fetchTenantContainerLists()
  },
  methods: {
    ...mapActions(['fetchTenantContainerLists']),
    handleEditOpen(row) {
      if (row) {
        this.$router.push(
          '/manage/resource/container-list/' + row.nodeName + '/' + row.name,
        )
      }
    },
    closeDialog() {
      this.$router.push('/manage/resource/container-list')
    },
    async done() {
      this.closeDialog()
      await this.fetchTenantContainerLists()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
