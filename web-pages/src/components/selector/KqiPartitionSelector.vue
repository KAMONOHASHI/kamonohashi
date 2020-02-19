<template>
  <el-form-item label="パーティション" prop="partition">
    <el-select
      v-model="partition"
      class="el-input"
      filterable
      :clearable="true"
      @change="$emit('input', partition)"
    >
      <el-option
        v-for="item in partitions"
        :key="item"
        :label="item"
        :value="item"
      >
      </el-option>
    </el-select>
  </el-form-item>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('cluster')

export default {
  data() {
    return {
      partition: null,
    }
  },
  computed: {
    ...mapGetters(['partitions']),
  },
}
</script>

<style lang="scss" scoped></style>
