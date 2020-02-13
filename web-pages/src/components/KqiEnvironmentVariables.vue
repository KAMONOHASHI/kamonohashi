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
          @input="updateVariables"
        />
      </el-col>
      <el-col :span="10">
        <el-input
          v-model="d.value"
          size="small"
          placeholder="Value"
          @input="updateVariables"
        />
      </el-col>
      <el-col :span="2">
        <el-button
          v-if="index > 0"
          size="small"
          type="danger"
          width="100%"
          @click="clickDelete(index)"
        >
          -
        </el-button>
      </el-col>
    </el-row>
    <el-row type="flex" justify="end">
      <el-col :span="2">
        <el-button size="small" type="primary" @click="clickAdd">
          +
        </el-button>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations } = createNamespacedHelpers(
  'environmentVariables',
)

export default {
  data() {
    return { variables: null }
  },
  computed: {
    ...mapGetters({ vuexVariables: ['variables'] }),
  },
  created() {
    // vuexに登録されている環境変数を読み込み
    this.variables = this.vuexVariables
  },
  methods: {
    ...mapMutations(['addVariables', 'removeVariables', 'setVariables']),
    clickAdd() {
      this.addVariables({ key: '', value: '' })
    },

    clickDelete(index) {
      this.removeVariables(index)
    },

    updateVariables() {
      // 環境変数の変更をvuexに反映
      this.setVariables(this.variables)
    },
  },
}
</script>

<style lang="scss" scoped></style>
