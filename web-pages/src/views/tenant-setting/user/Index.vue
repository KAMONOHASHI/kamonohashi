<template>
  <div>
    <h2>テナントユーザ管理</h2>
    <el-row>
      <el-table
        class="data-table pl-index-table"
        :data="tenantUsers"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="name" label="ユーザ名" width="300px" />
        <el-table-column prop="serviceType" label="認証タイプ" width="150px">
          <template slot-scope="scope">
            <span v-if="scope.row.serviceType === 1">ローカル</span>
            <span v-else-if="scope.row.serviceType === 2">LDAP</span>
            <span v-else>{{ serviceType }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="roles" label="ロール" width="auto">
          <template slot-scope="scope">
            <span v-for="role in scope.row.roles" :key="role.id">
              <el-tag class="role-tag">{{ role.displayName }}</el-tag>
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-row>

    <router-view @done="done()" @cancel="closeDialog()" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('user')

export default {
  title: 'テナントユーザ管理',
  data() {
    return {
      tenantEditDialogVisible: false,
    }
  },
  computed: {
    ...mapGetters(['tenantUsers']),
  },
  async created() {
    await this.fetchTenantUsers()
  },

  methods: {
    ...mapActions(['fetchTenantUsers']),

    async openEditDialog(row) {
      this.$router.push('/manage/user/' + row.id)
    },
    closeDialog() {
      this.$router.push('/manage/user')
    },
    async done() {
      this.closeDialog()
      await this.fetchTenantUsers()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped>
.role-tag {
  margin-right: 8px;
}
</style>
