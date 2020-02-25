<template>
  <div>
    <h2>テナント管理2</h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog"
        >
          新規作成
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table
        class="data-table pl-index-table"
        :data="tenants"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="テナント名" width="auto" />
        <el-table-column prop="displayName" label="表示名" width="auto" />
        <el-table-column
          prop="storagePath"
          label="ストレージパス"
          width="auto"
        />
      </el-table>
    </el-row>

    <router-view @done="done()" @cancel="closeDialog()"></router-view>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('tenant')

export default {
  title: 'テナント管理',
  computed: {
    ...mapGetters(['tenants']),
  },
  async created() {
    await this.fetchTenants()
  },
  methods: {
    ...mapActions(['fetchTenants']),
    openCreateDialog() {
      this.$router.push('/tenant/edit')
    },
    closeDialog() {
      this.$router.push('/tenant')
    },
    done() {
      this.fetchTenants()
      this.closeDialog()
      this.showSuccessMessage()
    },

    async openEditDialog(row) {
      if (row) {
        this.$router.push('/tenant/edit/' + row.id)
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
  padding-top: 10px;
}
</style>
