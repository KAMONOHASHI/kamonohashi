<template>
  <div>
    <h2>ストレージ管理2</h2>
    <el-row>
      <el-col :span="4" :offset="20" class="create-new">
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
        :data="storages"
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
    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('storage')

export default {
  title: 'ストレージ管理',
  computed: {
    ...mapGetters(['storages']),
  },
  async created() {
    await this.fetchStorages()
  },
  methods: {
    ...mapActions(['fetchStorages']),
    openCreateDialog() {
      this.$router.push('/storage/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/storage/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/storage')
    },
    async done() {
      await this.fetchStorages()
      this.closeDialog()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
