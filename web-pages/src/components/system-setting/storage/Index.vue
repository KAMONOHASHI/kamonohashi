<template>
  <div>
    <h2>ストレージ管理</h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog"
        >
          新規登録
        </el-button>
      </el-col>
    </el-row>

    <el-row>
      <el-table
        class="storage-table pl-index-table"
        :data="tableData"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="ストレージ名" width="auto" />
        <el-table-column prop="serverUrl" label="サーバURL" width="auto" />
        <el-table-column prop="nfsServer" label="NFSサーバ" width="auto" />
        <el-table-column
          prop="nfsRoot"
          label="NFS共有ディレクトリ"
          width="auto"
        />
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog" @done="done()"></router-view>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'StorageIndex',
  title: 'ストレージ管理',
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
      let response = await api.storage.admin.get()
      this.tableData = response.data
    },
    openCreateDialog() {
      this.$router.push('/storage/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/storage/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/storage')
    },
    async done() {
      await this.retrieveData()
      this.closeDialog()
      this.showSuccessMessage()
    },
  },
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
