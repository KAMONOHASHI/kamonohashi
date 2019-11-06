<template>
  <div>
    <h2> レジストリ管理 </h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button @click="openCreateDialog" icon="el-icon-edit-outline" type="primary" plain>
          新規登録
        </el-button>
      </el-col>
    </el-row>

    <el-row>
      <el-table class="registry-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
        <el-table-column prop="id" label="ID" width="100px"/>
        <el-table-column prop="name" label="レジストリ名" width="auto"/>
        <el-table-column prop="registryPath" label="レジストリパス" width="auto"/>
        <el-table-column prop="projectName" label="GitLabプロジェクト名" width="auto">
          <template slot-scope="prop">
            {{ displayNameOfProjectName(prop.row.serviceType, prop.row.projectName) }}
          </template>
        </el-table-column>
        <el-table-column prop="serviceType" label="レジストリ種別" width="auto">
          <template slot-scope="prop">
            {{ displayNameOfServiceType(prop.row.serviceType) }}
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog" @done="done()"></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'RegistryIndex',
    title: 'レジストリ管理',
    data () {
      return {
        tableData: [],
        selectedRowId: undefined
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        // serviceTypeIdと表示名の一覧取得
        this.serviceTypes = (await api.registry.admin.getType()).data
        let response = await api.registry.admin.get()
        this.tableData = response.data
      },
      // ServiceTypeの数値から表示名に変換
      displayNameOfServiceType (serviceTypeId) {
        let serviceType = this.serviceTypes.find(s => s.id === serviceTypeId)
        return serviceType.name
      },
      displayNameOfProjectName (serviceTypeId, projectName) {
        return this.displayNameOfServiceType(serviceTypeId) === 'GitLab' ? projectName : ''
      },
      openCreateDialog () {
        this.$router.push('/registry/create')
      },
      openEditDialog (selectedRow) {
        this.$router.push('/registry/' + selectedRow.id)
      },
      closeDialog () {
        this.$router.push('/registry')
      },
      async done () {
        await this.retrieveData()
        this.closeDialog()
        this.showSuccessMessage()
      }
    }
  }
</script>

<style lang="scss" scoped>
  .right-top-button {
    text-align: right;
    padding-top: 10px;
  }

  .pagination /deep/ .el-input {
    text-align: left;
    width: 120px;
  }
</style>
