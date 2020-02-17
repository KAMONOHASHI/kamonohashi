<template>
  <div>
    <h2>ノード管理2</h2>
    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      ></kqi-pagination>
      <el-col class="right-top-button" :span="8">
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
        class="data-table pl-index-table"
        :data="nodes"
        border
        @row-click="openEditDialog"
      >
        <el-table-column prop="id" label="ID" width="100px" />
        <el-table-column prop="name" label="ノード名" width="200px" />
        <el-table-column
          prop="partition"
          label="パーティション"
          width="320px"
        />
        <el-table-column
          prop="accessLevelStr"
          label="アクセスレベル"
          width="200px"
        />
        <el-table-column
          prop="tensorBoardEnabled"
          label="TensorBoard"
          width="200px"
        >
          <template slot-scope="prop">
            {{ getTensorBoardFlag(prop.row.tensorBoardEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="notebookEnabled" label="Notebook" width="200px">
          <template slot-scope="prop">
            {{ getNotebookFlag(prop.row.notebookEnabled) }}
          </template>
        </el-table-column>
        <el-table-column prop="memo" label="メモ" width="auto" />
      </el-table>
    </el-row>
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      ></kqi-pagination>
    </el-row>
    <router-view @cancel="closeDialog()" @done="done()"></router-view>
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('node')

export default {
  components: {
    KqiPagination,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
    }
  },
  computed: {
    ...mapGetters(['nodes', 'total']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchNodes']),
    async retrieveData() {
      let params = {
        page: this.pageStatus.currentPage,
        perPage: this.pageStatus.currentPageSize,
        withTotal: true,
      }
      await this.fetchNodes(params)
    },
    getTensorBoardFlag(enabled) {
      return enabled ? 'OK' : 'NG'
    },
    getNotebookFlag(enabled) {
      return enabled ? 'OK' : 'NG'
    },
    openCreateDialog() {
      this.$router.push('node/edit')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/node/edit/' + selectedRow.id)
    },
    closeDialog() {
      this.$router.push('/node')
    },
    async done() {
      this.closeDialog()
      this.retrieveData()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
