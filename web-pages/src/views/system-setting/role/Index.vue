<template>
  <div>
    <h2>ロール管理2</h2>
    <el-row>
      <el-col :span="4" :offset="20" class="create-new">
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
        :data="roles"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="ロール名" width="auto" />
        <el-table-column prop="displayName" label="表示名" width="auto" />
        <el-table-column prop="isSystemRole" label="種別" width="auto">
          <template slot-scope="scope">
            <span v-if="scope.row.isSystemRole">システム</span>
            <span v-else-if="scope.row.tenantId">テナント(カスタム)</span>
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
  title: 'ロール管理',
  computed: {
    ...mapGetters(['roles']),
  },
  async created() {
    await this.fetchRoles()
  },
  methods: {
    ...mapActions(['fetchRoles']),
    openCreateDialog() {
      this.$router.push('/role/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/role/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/role')
    },
    async done() {
      await this.fetchRoles()
      this.closeDialog()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
