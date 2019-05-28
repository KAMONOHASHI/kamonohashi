<template>
  <div>
    <h2> ストレージ管理 </h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button @click="createDialogVisible = true" icon="el-icon-edit-outline" type="primary" plain>
          新規登録
        </el-button>
      </el-col>
    </el-row>

    <el-row>
      <el-table class="storage-table pl-index-table" :data="tableData" @row-click="openEditDialog" border>
        <el-table-column prop="id" label="ID" width="100px"/>
        <el-table-column prop="name" label="ストレージ名" width="auto"/>
        <el-table-column prop="serverUrl" label="サーバURL" width="auto"/>
        <el-table-column prop="nfsServer" label="NFSサーバ" width="auto"/>
        <el-table-column prop="nfsRoot" label="NFS共有ディレクトリ" width="auto"/>
      </el-table>
    </el-row>
    <create-storage
      v-if="createDialogVisible"
      @cancel="createDialogVisible = false"
      @done="retrieveData()"
    />
    <edit-storage
      v-if="editDialogVisible"
      :storageId="selectedRowId"
      @cancel="editDialogVisible = false"
      @done="retrieveData()"
    />
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import CreateStorage from '@/components/system-setting/storage/Create.vue'
  import EditStorage from '@/components/system-setting/storage/Edit.vue'

  export default {
    name: 'StorageIndex',
    title: 'ストレージ管理',
    components: {
      'create-storage': CreateStorage,
      'edit-storage': EditStorage
    },
    data () {
      return {
        tableData: [],
        createDialogVisible: false,
        editDialogVisible: false,
        selectedRowId: undefined
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        let response = await api.storage.admin.get()
        this.tableData = response.data
        this.editDialogVisible = false
        this.createDialogVisible = false
      },
      openEditDialog (selectedRow) {
        this.selectedRowId = selectedRow.id
        this.editDialogVisible = true
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
