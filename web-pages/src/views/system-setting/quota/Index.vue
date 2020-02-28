<template>
  <div>
    <h2>クォータ管理</h2>
    <el-row>
      <kqi-display-error :error="error" />
      <el-col class="create-new">
        <el-button icon="el-icon-edit-outline" type="primary" @click="update">
          更新
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="quota-table pl-index-table" :data="quotasData" border>
        <!-- テーブルの各列の表示項目を注入 -->
        <el-table-column prop="tenantName" label="テナント" width="auto" />
        <el-table-column prop="cpu" label="CPU" width="auto">
          <template slot-scope="scope">
            <el-input-number
              v-model="scope.row.cpu"
              size="small"
              :min="0"
              :max="200"
              controls-position="right"
            />
          </template>
        </el-table-column>
        <el-table-column prop="memory" label="Memory" width="auto">
          <template slot-scope="scope">
            <el-input-number
              v-model="scope.row.memory"
              :min="0"
              :max="200"
              size="small"
              controls-position="right"
            />
            GB
          </template>
        </el-table-column>
        <el-table-column prop="gpu" label="GPU" width="auto">
          <template slot-scope="scope">
            <el-input-number
              v-model="scope.row.gpu"
              :min="0"
              :max="16"
              size="small"
              controls-position="right"
            />
          </template>
        </el-table-column>
      </el-table>
    </el-row>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiDisplayError from '@/components/KqiDisplayError'

const { mapGetters, mapActions } = createNamespacedHelpers('quota')

export default {
  title: 'クォータ管理',
  components: {
    KqiDisplayError,
  },
  data() {
    return {
      error: null,
      quotasData: [],
    }
  },
  computed: {
    ...mapGetters(['quotas']),
  },
  async created() {
    await this.fetchQuotas()
    this.quotasData = this.quotas
  },
  methods: {
    ...mapActions(['fetchQuotas', 'post']),
    async update() {
      try {
        let model = this.quotasData.map(function(quota) {
          return {
            tenantId: quota.tenantId,
            cpu: quota.cpu,
            memory: quota.memory,
            gpu: quota.gpu,
          }
        })
        await this.post({ models: model })
        this.showSuccessMessage()
        this.error = null
      } catch (e) {
        this.error = e
      }
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
