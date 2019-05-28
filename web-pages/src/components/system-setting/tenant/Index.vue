<template>
  <div>
    <h2>テナント管理</h2>
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
        <el-table-column prop="name" label="テナント名" width="auto"/>
        <el-table-column prop="displayName" label="表示名" width="auto"/>
        <el-table-column prop="storagePath" label="ストレージパス" width="auto"/>
      </el-table>
    </el-row>

    <router-view @done="done()" @cancel="closeDialog"></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'TenantIndex',
    title: 'テナント管理',
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
        let response = await api.tenant.admin.get(params)
        this.tableData = response.data
      },

      openCreateDialog () {
        this.$router.push('/tenant/create')
      },
      closeDialog () {
        this.$router.push('/tenant')
      },
      done () {
        this.retrieveData()
        this.closeDialog()
        this.showSuccessMessage()
      },

      async openEditDialog (row) {
        if (row) {
          this.$router.push('/tenant/' + row.id)
        }
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
