<template>
  <div>
    <h2> クォータ管理 </h2>
    <el-row :gutter="20">
      <el-col class="create-new">
        <pl-display-error :error="error"/>
        <el-button @click="update" icon="el-icon-edit-outline" type="primary">更新</el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="quota-table pl-index-table" :data="tableData" border>
        <!-- テーブルの各列の表示項目を注入 -->
        <el-table-column prop="tenantName" label="テナント" width="auto"/>
        <el-table-column prop="cpu" label="CPU" width="auto">
          <template slot-scope="scope">
            <el-input-number v-model="scope.row.cpu" size="small" :min="0" controls-position="right"/>
          </template>
        </el-table-column>
        <el-table-column prop="memory" label="Memory" width="auto">
          <template slot-scope="scope">
            <el-input-number v-model="scope.row.memory" :min="0" size="small" controls-position="right"/>
            GB
          </template>
        </el-table-column>
        <el-table-column prop="gpu" label="GPU" width="auto">
          <template slot-scope="scope">
            <el-input-number v-model="scope.row.gpu" :min="0" size="small" controls-position="right"/>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'QuotaIndex',
    title: 'クォータ管理',
    components: {
      'pl-display-error': DisplayError
    },
    data () {
      return {
        error: null,
        tableData: []
      }
    },
    async created () {
      let response = await api.quotas.get()
      this.tableData = response.data
    },
    methods: {
      async update () {
        try {
          let model = this.tableData.map(function (quota) {
            return {
              tenantId: quota.tenantId,
              cpu: quota.cpu,
              memory: quota.memory,
              gpu: quota.gpu
            }
          })
          await api.quotas.post({models: model})
          this.showSuccessMessage()
          this.error = null
        } catch (e) {
          this.error = e
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .create-new {
    text-align: right;
    padding-top: 10px;
  }
</style>
