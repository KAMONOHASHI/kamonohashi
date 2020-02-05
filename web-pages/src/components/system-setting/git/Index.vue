<template>
  <div>
    <h2>Git管理</h2>
    <el-row :gutter="20">
      <el-col class="create-new">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog"
          >新規登録</el-button
        >
      </el-col>
    </el-row>
    <el-row>
      <el-table
        class="git-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="リポジトリ名" width="auto" />
        <el-table-column
          prop="repositoryUrl"
          label="リポジトリURL"
          width="auto"
        />
        <el-table-column prop="serviceType" label="種別" width="auto">
          <template slot-scope="scope">
            {{ displayNameOfServiceType(scope.row.serviceType) }}
          </template>
        </el-table-column>
        <el-table-column prop="apiUrl" label="API URL" width="auto">
        </el-table-column>
      </el-table>
    </el-row>

    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'GitIndex',
  title: 'Git 管理',
  data() {
    return {
      tableData: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      // serviceTypeIdと表示名の一覧取得
      this.serviceTypes = (await api.git.admin.getTypes()).data
      let response = await api.git.admin.getEndpoints()
      this.tableData = response.data
    },
    // ServiceTypeの数値から表示名に変換
    displayNameOfServiceType(serviceTypeId) {
      let serviceType = this.serviceTypes.find(s => s.id === serviceTypeId)
      return serviceType.name
    },
    openCreateDialog() {
      this.$router.push('git/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/git/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/git')
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
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
