<template>
  <div>
    <el-form-item label="CPU" required>
      <el-slider
        :value="vuexResource.cpu"
        class="el-input"
        :min="1"
        :max="200"
        show-input
        @input="changeCpu"
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="メモリ(GB)" required>
      <el-slider
        :value="vuexResource.memory"
        class="el-input"
        :min="1"
        :max="200"
        show-input
        @input="changeMemory"
      >
      </el-slider>
    </el-form-item>
    <el-form-item label="GPU" required>
      <el-slider
        :value="vuexResource.gpu"
        class="el-input"
        :min="0"
        :max="16"
        show-input
        @input="changeGpu"
      >
      </el-slider>
    </el-form-item>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('resource')

export default {
  data() {
    return {
      resource: {
        cpu: 10,
        memory: 1,
        gpu: 0,
      },
    }
  },
  computed: {
    ...mapGetters({ vuexResource: ['resource'] }),
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
