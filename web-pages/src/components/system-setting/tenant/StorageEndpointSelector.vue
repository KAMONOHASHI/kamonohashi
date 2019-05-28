<template>
  <el-select class="selectStorage" :value="value" @change="handleChange" placeholder="Select" :clearable="true">
    <el-option
      v-for="item in list"
      :key="item.id"
      :label="item.name"
      :value="item.id">
      <span style="float: left">{{ item.name }}</span>
      <span style="float: right; color: #8492a6; font-size: 13px">{{ item.repositoryUrl }}</span>
    </el-option>
  </el-select>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'StorageEndpointSelector',
    props: ['value'],
    data () {
      return {
        list: []
      }
    },
    async created () {
      await this.init()
    },
    methods: {
      async init () {
        this.list = (await api.storage.admin.get()).data
      },
      async handleChange (v) {
        this.$emit('input', v)
      }
    }
  }
</script>

<style scoped>
  .selectStorage {
    width: 100%;
  }
</style>
