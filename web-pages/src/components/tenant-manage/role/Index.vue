<template>
  <div>
    <h2> テナントロール管理 </h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button @click="openCreateDialog" icon="el-icon-edit-outline" type="primary" plain>
          新規作成
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
        <el-table-column prop="id" label="ID" width="120px"/>
        <el-table-column prop="name" label="ロール名" width="auto"/>
        <el-table-column prop="displayName" label="表示名" width="auto"/>
        <el-table-column prop="tenantId" label="種別" width="auto">
          <template slot-scope="scope">
            <span v-if="scope.row.tenantId">カスタム</span>
            <span v-else>共通</span>
          </template>
        </el-table-column>
        <el-table-column prop="sortOrder" label="表示順" width="auto"/>
      </el-table>
    </el-row>

    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'ManageRoleIndex',
    title: 'テナントロール管理',
    data () {
      return {
        tableData: []
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        let params = {}
        let response = await api.role.tenant.get(params)
        this.tableData = response.data
      },
      openCreateDialog () {
        this.$router.push('/manage/role/create')
      },
      openEditDialog (selectedRow) {
        this.$router.push('/manage/role/' + selectedRow.id)
      },
      closeDialog () {
        this.$router.push('/manage/role')
      },
      async done () {
        this.closeDialog()
        await this.retrieveData()
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
</style>
