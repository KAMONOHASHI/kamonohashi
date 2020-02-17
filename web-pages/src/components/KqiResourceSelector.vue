<template>
  <div>
    <el-form-item label="CPU" required>
      <el-slider
        v-model="resource.cpu"
        class="el-input"
        :min="1"
        :max="200"
        show-input
        @change="$emit('input', resource)"
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="メモリ(GB)" required>
      <el-slider
        v-model="resource.memory"
        class="el-input"
        :min="1"
        :max="200"
        show-input
        @change="$emit('input', resource)"
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="GPU" required>
      <el-slider
        v-model="resource.gpu"
        class="el-input"
        :min="0"
        :max="16"
        show-input
        @change="$emit('input', resource)"
      >
      </el-slider>
    </el-form-item>
  </div>
</template>

<script>
export default {
  data() {
    return {
      resource: {
        cpu: 1,
        memory: 1,
        gpu: 0,
      },
    }
  },
}
</script>

<style></style>
