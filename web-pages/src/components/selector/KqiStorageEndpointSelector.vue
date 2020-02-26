<template>
  <el-form-item label="ストレージ" prop="storageId">
    <el-select
      class="selectStorage"
      :value="storageId"
      placeholder="Select"
      :clearable="true"
      @change="handleChange"
    >
      <el-option
        v-for="item in storages"
        :key="item.id"
        :label="item.name"
        :value="item.id"
      >
        <span style="float: left">{{ item.name }}</span>
        <span style="float: right; color: #8492a6; font-size: 13px">{{
          item.serverUrl
        }}</span>
      </el-option>
    </el-select>
  </el-form-item>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('storage')

export default {
  props: {
    storageId: {
      type: Number,
      default: null,
    },
  },
  computed: {
    ...mapGetters(['storages']),
  },
  methods: {
    async handleChange(storageId) {
      if (storageId === '') {
        this.$emit('changeStorageId', { value: null })
      } else {
        this.$emit('changeStorageId', { value: storageId })
      }
    },
  },
}
</script>

<style scoped>
.selectStorage {
  width: 100%;
}
</style>
