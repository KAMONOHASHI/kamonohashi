<template>
  <div>
    <el-form-item label="パーティション" prop="partition">
      <el-select
        :value="value"
        class="el-input"
        filterable
        :clearable="true"
        @change="changePartition"
      >
        <el-option
          v-for="item in partitions"
          :key="item"
          :label="item"
          :value="item"
        />
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
export default {
  props: {
    // パーティション名の一覧
    partitions: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択されたパーティション名
    value: {
      type: String,
      default: null,
    },
  },
  methods: {
    changePartition(partition) {
      if (partition === '') {
        this.$emit('input', null)
      } else {
        this.$emit('input', partition)
      }
    },
  },
}
</script>

<style lang="scss" scoped></style>
