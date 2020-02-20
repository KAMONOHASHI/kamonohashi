<template>
  <div>
    <el-form-item label="パーティション" prop="partition">
      <el-select
        :value="partition"
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
        >
        </el-option>
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('cluster')

export default {
  props: {
    partition: {
      type: String,
      default: null,
    },
  },
  computed: {
    ...mapGetters(['partitions']),
  },
  methods: {
    changePartition(partition) {
      this.$emit('input', partition)
    },
  },
}
</script>

<style lang="scss" scoped></style>
