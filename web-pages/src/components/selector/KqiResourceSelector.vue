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
    value: {
      type: Object,
      default: () => {
        return { cpu: 1, memory: 1, gpu: 0 }
      },
    },
  },
  methods: {
    changeCpu(cpu) {
      // 変更前情報を取得(created後、非同期でvuexの情報が更新された際に対処するため)
      this.resource = this.vuexResource
      this.resource.cpu = cpu
      this.$emit('input', this.resource)
    },
    changeMemory(memory) {
      this.resource = this.vuexResource
      this.resource.memory = memory
      this.$emit('input', this.resource)
    },
    changeGpu(gpu) {
      this.resource = this.vuexResource
      this.resource.gpu = gpu
      this.$emit('input', this.resource)
    },
  },
}
</script>

<style></style>
