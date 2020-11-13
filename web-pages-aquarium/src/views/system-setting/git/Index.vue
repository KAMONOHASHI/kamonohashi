<template>
  <div>
    <h2>Git管理</h2>
    <el-row>
      <el-col class="create-new">
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
        <el-table-column prop="serviceTypeName" label="種別" width="auto" />
        <el-table-column prop="apiUrl" label="API URL" width="auto" />
      </el-table>
    </el-row>

    <router-view @cancel="closeDialog()" @done="done()" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('git')

export default {
  title: 'Git管理', //<title>設定
  computed: {
    ...mapGetters(['endpoints']),
  },
  async created() {
    await this.fetchEndpoints()
  },
  methods: {
    ...mapActions(['fetchEndpoints']),
    openCreateDialog() {
      this.$router.push('/git/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/git/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/git')
    },
    async done() {
      this.closeDialog()
      await this.fetchEndpoints()
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
