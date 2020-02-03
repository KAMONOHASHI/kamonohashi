<template>
  <span>
    <span v-if="multiple">
      <el-select
        class="selectTenant"
        :value="value"
        multiple
        placeholder="Select"
        @change="handleChange"
      >
        <template v-for="item in list">
          <el-option
            v-if="item.isSystemRole === showSystem"
            :key="item.id"
            :label="item.displayName"
            :value="item.id"
          >
          </el-option>
        </template>
      </el-select>
    </span>
    <span v-else>
      <el-select
        class="selectTenant"
        :value="value"
        placeholder="Select"
        :clearable="true"
        @change="handleChange"
      >
        <template v-for="item in list">
          <el-option
            v-if="item.isSystemRole === showSystem"
            :key="item.id"
            :label="item.displayName"
            :value="item.id"
          >
          </el-option>
        </template>
      </el-select>
    </span>
  </span>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'RoleSelector',
  props: ['value', 'multiple', 'system', 'tenant'],
  data() {
    return {
      list: [],
      showSystem: false,
    }
  },
  async created() {
    await this.init()
  },
  methods: {
    async init() {
      if (this.tenant) {
        this.list = (await api.role.tenant.get()).data
        this.showSystem = false
      } else {
        this.list = (await api.role.admin.get()).data
        this.showSystem = this.system === true
      }
    },
    async handleChange(v) {
      this.$emit('input', v)
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}
</style>
