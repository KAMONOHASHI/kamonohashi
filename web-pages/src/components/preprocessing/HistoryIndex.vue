<template>
  <div>
    <pl-display-error :error="error" />

    <div v-if="loading">
      <i class="el-icon-loading"></i>
    </div>
    <div v-else>
      <div v-if="tableData.length === 0">
        履歴が見つかりませんでした。
      </div>
      <div v-else>
        <el-table
          class="data-table"
          :data="tableData"
          border
          @row-click="openEditDialog"
        >
          <el-table-column prop="dataId" label="データID" width="120px" />
          <el-table-column prop="dataName" label="データ名" width="auto" />
          <el-table-column prop="createdAt" label="実行日時" width="170px" />
          <el-table-column prop="status" label="ステータス" width="120px" />
        </el-table>
      </div>
    </div>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import DisplayError from '@/components/common/DisplayError'

export default {
  name: 'PreprocessingHistoryIndex',
  components: {
    'pl-display-error': DisplayError,
  },
  props: ['id'],
  data() {
    return {
      error: null,
      loading: true,
      tableData: [],
    }
  },
  watch: {
    async id() {
      await this.changeId()
    },
  },
  async created() {
    await this.changeId()
  },
  methods: {
    async changeId() {
      this.tableData = []

      if (this.id) {
        try {
          this.loading = true
          let params = {
            $config: { apiDisabledLoading: true },
            id: this.id,
          }
          this.tableData = (await api.preprocessings.getHistory(params)).data
          this.error = null
        } catch (e) {
          this.error = e
        } finally {
          this.loading = false
        }
      }
    },
    openEditDialog(row) {
      if (row) {
        this.$emit('openPreprocessingHistoryEdit', {
          id: this.id,
          dataId: row.dataId,
        })
      }
    },
    closeEditDialog() {
      this.$router.push('/preprocessing/')
    },
    async done() {
      this.closeEditDialog()
      await this.changeId()
      this.showSuccessMessage()
    },
  },
}
</script>

<style lang="scss" scoped></style>
