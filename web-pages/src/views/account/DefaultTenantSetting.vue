<template>
  <!-- デフォルトテナント設定 -->
  <div>
    <el-row class="row-element" style="padding-top: 50px;">
      <el-col :span="6" class="content-color">
        既定のテナント
      </el-col>
      <el-col :span="12">
        <el-select
          v-if="tenants.length !== 0"
          :value="value"
          placeholder="Select"
          @change="defaultTenantChange"
        >
          <el-option
            v-for="(t, index) in tenants"
            :key="index"
            :label="t.displayName"
            :value="t.name"
          />
        </el-select>
      </el-col>
    </el-row>
    <el-row>
      <el-col class="button-group">
        <el-button type="primary" @click="$emit('defaultTenantUpdate')">
          更新
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: String,
      default: null,
    },
    tenants: {
      type: Array,
      default: () => {
        return []
      },
    },
  },
  methods: {
    defaultTenantChange(tenantName) {
      this.$emit('input', tenantName)
    },
  },
}
</script>

<style scoped>
.row-element {
  font-size: 14px;
  line-height: 40px;
  margin-top: 30px;
  font-weight: bold !important;
}

.button-group {
  text-align: right;
  padding-top: 100px;
  padding-right: 30px;
}

.content-color {
  color: #606266;
}
</style>
