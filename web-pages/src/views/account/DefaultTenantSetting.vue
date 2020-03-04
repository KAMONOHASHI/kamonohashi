<template>
  <!-- デフォルトテナント設定 -->
  <div id="first_tab01" class="cp_tabpanel">
    <el-row class="row-element">
      <el-col :span="6">
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
  margin: 40px;
}
</style>
