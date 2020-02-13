<template>
  <div>
    <h2>Git管理2</h2>
    <el-row>
      <el-col :span="4" :offset="20" class="create-new">
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
        :data="endpoints"
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
        </el-table-column>
        <el-table-column prop="apiUrl" label="API URL" width="auto">
        </el-table-column>
      </el-table>
    </el-row>

    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('git')

export default {
  computed: {
    ...mapGetters(['endpoints']),
  },
  async created() {
    await this.fetchEndpoints()
  },
  methods: {
    ...mapActions(['fetchEndpoints']),
    openCreateDialog() {
      this.$router.push('git/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/git/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/git')
    },
    async done() {
      this.closeDialog()
      this.fetchEndpoints()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
