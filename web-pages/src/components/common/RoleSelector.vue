<template>
  <span>
    <span v-if="multiple">
      <el-select class="selectTenant" :value="value" @change="handleChange" multiple placeholder="Select">
        <el-option
          v-for="item in list"
          :key="item.id"
          :label="item.displayName"
          :value="item.id"
          v-if="item.isSystemRole === showSystem"/>
      </el-select>
    </span>
    <span v-else>
      <el-select class="selectTenant" :value="value" @change="handleChange" placeholder="Select" :clearable="true">
        <el-option
          v-for="item in list"
          :key="item.id"
          :label="item.displayName"
          :value="item.id"
          v-if="item.isSystemRole === showSystem">
        </el-option>
      </el-select>
    </span>
  </span>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'RoleSelector',
  props: ['value', 'multiple', 'system', 'tenant'],
  data () {
    return {
      list: [],
      showSystem: false
    }
  },
  async created () {
    await this.init()
  },
  methods: {
    async init () {
      if (this.tenant) {
        this.list = (await api.role.tenant.get()).data
        this.showSystem = false
      } else {
        this.list = (await api.role.admin.get()).data
        this.showSystem = (this.system === true)
      }
    },
    async handleChange (v) {
      this.$emit('input', v)
    }
  }
}
</script>

<style scoped>
  .selectTenant {
    width: 100%;
  }
</style>
