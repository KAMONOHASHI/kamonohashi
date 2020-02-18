<template>
  <div>
    <h2>レジストリ管理2</h2>
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
        class="registry-table pl-index-table"
        :data="registries"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="レジストリ名" width="auto" />
        <el-table-column
          prop="registryPath"
          label="レジストリパス"
          width="auto"
        />
        <el-table-column
          prop="projectName"
          label="GitLabプロジェクト名"
          width="auto"
        >
        </el-table-column>
        <el-table-column prop="serviceType" label="レジストリ種別" width="auto">
        </el-table-column>
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('registry')

export default {
  title: 'レジストリ管理', //<title>設定
  computed: {
    ...mapGetters(['registries']),
  },
  async created() {
    await this.fetchRegistries()
  },
  methods: {
    ...mapActions(['fetchRegistries']),
    openCreateDialog() {
      this.$router.push('/registry/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/registry/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/registry')
    },
    async done() {
      await this.fetchRegistries()
      this.closeDialog()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
