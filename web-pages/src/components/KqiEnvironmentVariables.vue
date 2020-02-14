<template>
  <el-form-item label="環境変数">
    <el-row></el-row>
    <el-row
      v-for="(d, index) in variables"
      :key="index"
      type="flex"
      justify="space-between"
    >
      <el-col :span="10" :offset="1">
        <el-input
          v-model="d.key"
          size="small"
          placeholder="Key"
          @input="$emit('updateVariables', variables)"
        />
      </el-col>
      <el-col :span="10">
        <el-input
          v-model="d.value"
          size="small"
          placeholder="Value"
          @input="$emit('updateVariables', variables)"
        />
      </el-col>
      <el-col :span="2">
        <el-button
          v-if="index > 0"
          size="small"
          type="danger"
          width="100%"
          @click="$emit('removeVariables', index)"
        >
          -
        </el-button>
      </el-col>
    </el-row>
    <el-row type="flex" justify="end">
      <el-col :span="2">
        <el-button size="small" type="primary" @click="$emit('addVariables')">
          +
        </el-button>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('environmentVariables')

export default {
  data() {
    return { variables: null }
  },
  computed: {
    ...mapGetters({ vuexVariables: ['variables'] }),
  },
  created() {
    // storeに登録されている環境変数を読み込み
    this.variables = this.vuexVariables
  },
}
</script>

<style lang="scss" scoped></style>
