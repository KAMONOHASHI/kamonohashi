<template>
  <el-form-item label="ストレージ情報" prop="storageId">
    <div class="left-margin">
      <el-select
        class="selectStorage"
        :value="value"
        placeholder="Select"
        @change="handleChange"
      >
        <el-option
          v-for="item in storages"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left;">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px;">
            {{ item.serverUrl }}
          </span>
        </el-option>
      </el-select>
    </div>
  </el-form-item>
</template>

<script>
export default {
  props: {
    // 表示するstorageの一覧
    storages: {
      type: Array,
      default: () => {
        return []
      },
    },
    // storagesの中から選択したid
    value: {
      type: Number,
      default: null,
    },
  },
  methods: {
    async handleChange(storageId) {
      if (storageId === '') {
        this.$emit('input', null)
      } else {
        this.$emit('input', storageId)
      }
    },
  },
}
</script>

<style scoped>
.selectStorage {
  width: 100%;
}
.left-margin {
  padding-left: 30px;
}
</style>
