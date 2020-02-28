<template>
  <div>
    <el-form-item label="CPU" required>
      <el-slider
        v-model="value.cpu"
        class="el-input"
        :min="1"
        :max="200"
        show-input
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="メモリ(GB)" required>
      <el-slider
        v-model="value.memory"
        class="el-input"
        :min="1"
        :max="200"
        show-input
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="GPU" required>
      <el-slider
        v-model="value.gpu"
        class="el-input"
        :min="0"
        :max="16"
        show-input
      >
      </el-slider>
    </el-form-item>
  </div>
</template>

<script>
export default {
  props: {
    // cpu, memory, gpuのリソース量
    value: {
      type: Object,
      default: () => {
        return { cpu: 1, memory: 1, gpu: 0 }
      },
    },
  },
}
</script>

<style></style>
