<template>
  <div>
    <el-form-item label="CPU" required>
      <el-slider
        v-model="value.cpu"
        class="el-input"
        :min="1"
        :max="quota.cpu === 0 ? defaultMax.cpu : quota.cpu"
        show-input
      />
    </el-form-item>
    <el-form-item label="メモリ(GB)" required>
      <el-slider
        v-model="value.memory"
        class="el-input"
        :min="1"
        :max="quota.memory === 0 ? defaultMax.memory : quota.memory"
        show-input
      />
    </el-form-item>
    <el-form-item label="GPU" required>
      <el-slider
        v-model="value.gpu"
        class="el-input"
        :min="0"
        :max="quota.gpu === 0 ? defaultMax.gpu : quota.gpu"
        show-input
      />
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
    // 接続中テナントのクォータ情報
    quota: {
      type: Object,
      default: () => {
        return { cpu: 0, memory: 0, gpu: 0 }
      },
    },
  },
  data() {
    return {
      // デフォルトの最大値
      defaultMax: {
        cpu: 250,
        memory: 500,
        gpu: 16,
      },
    }
  },
}
</script>

<style></style>
