<template>
  <div>
    <h2>ユーザグループ管理</h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
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
        :data="userGroups"
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="ユーザグループ名" width="300px" />
        <el-table-column prop="isGroup" label="種別" width="150px">
          <template slot-scope="scope">
            <span v-if="scope.row.isGroup">グループ</span>
            <span v-else>OU</span>
          </template>
        </el-table-column>
        <el-table-column prop="dn" label="DN" width="auto" />
        <el-table-column prop="memo" label="メモ" width="auto" />
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog()" @done="done()" />
  </div>
</template>
<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('userGroup')

export default {
  title: 'ユーザグループ管理',
  computed: {
    ...mapGetters(['userGroups']),
  },
  async created() {
    await this.fetchUserGroups()
  },
  methods: {
    ...mapActions(['fetchUserGroups']),
    openCreateDialog() {
      this.$router.push('/usergroup/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/usergroup/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/usergroup')
    },
    async done() {
      await this.fetchUserGroups()
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
</style>
