<template>
  <div>
    <h2>テナントロール管理</h2>
    <el-row>
      <el-col class="create-new">
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
        :data="tenantRoles"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="ロール名" width="auto" />
        <el-table-column prop="displayName" label="表示名" width="auto" />
        <el-table-column prop="isSystemRole" label="種別" width="auto">
          <template slot-scope="scope">
            <span v-if="scope.row.tenantId">テナント(カスタム)</span>
            <span v-else>テナント(共通)</span>
          </template>
        </el-table-column>
        <el-table-column prop="sortOrder" label="表示順" width="auto" />
      </el-table>
    </el-row>

    <router-view @cancel="closeDialog()" @done="done()" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('role')

export default {
  title: 'テナントロール管理',
  computed: {
    ...mapGetters(['tenantRoles']),
  },
  async created() {
    await this.fetchTenantRoles()
  },
  methods: {
    ...mapActions(['fetchTenantRoles']),
    openCreateDialog() {
      this.$router.push('/manage/role/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/manage/role/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/manage/role')
    },
    async done() {
      await this.fetchTenantRoles()
      this.closeDialog()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped>
.create-new {
  text-align: right;
  padding-top: 10px;
}
</style>
