<template>
  <el-form-item label="環境変数">
    <el-row></el-row>
    <el-row
      v-for="(d, index) in value"
      :key="index"
      type="flex"
      justify="space-between"
    >
      <el-col :span="10" :offset="1">
        <el-input v-model="d.key" size="small" placeholder="Key" />
      </el-col>
      <el-col :span="10">
        <el-input v-model="d.value" size="small" placeholder="Value" />
      </el-col>
      <el-col :span="2">
        <el-button
          size="small"
          type="danger"
          width="100%"
          @click="removeVariables(index)"
        >
          -
        </el-button>
      </el-col>
    </el-row>
    <el-row type="flex" justify="end">
      <el-col :span="2">
        <el-button size="small" type="primary" @click="addVariables">
          +
        </el-button>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
export default {
  props: {
    value: {
      type: Array,
      default: () => {
        return [{ key: '', value: '' }]
      },
    },
  },
  methods: {
    removeVariables(index) {
      this.value.splice(index, 1)
    },
    addVariables() {
      this.value.push({ key: '', value: '' })
    },
  },
}
</script>

<style lang="scss" scoped></style>
