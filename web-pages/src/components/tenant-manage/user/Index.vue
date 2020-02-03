<template>
  <div>
    <h2>テナントユーザ管理</h2>
    <el-row>
      <el-table
        class="data-table pl-index-table"
        :data="tableData"
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
              <el-tag>{{ role.displayName }}</el-tag
              >&nbsp;
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-row>

    <router-view @done="done()" @cancel="closeDialog()"> </router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'ManageUserIndex',
  title: 'テナントユーザ管理',
  data() {
    return {
      tenantEditDialogVisible: false,
      tableData: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      let response = await api.user.tenant.get()
      this.tableData = response.data
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    async openEditDialog(row) {
      this.$router.push('/manage/user/' + row.id)
    },
    closeDialog() {
      this.$router.push('/manage/user')
    },
  },
}
</script>

<style lang="scss" scoped></style>
