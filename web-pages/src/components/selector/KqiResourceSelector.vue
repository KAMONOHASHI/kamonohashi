<template>
  <div>
    <el-row type="flex">
      <el-col>
        <label>実行要求リソース</label>
      </el-col>
      <el-col align="right">
        <el-popover
          ref="allocatableNodeInfo"
          title="ノード情報"
          trigger="hover"
        >
          <kqi-allocatable-node-info :allocatable-nodes="nodes" />
        </el-popover>
        <el-button
          v-popover:allocatableNodeInfo
          icon="el-icon-info"
          type="primary"
          plain
          size="mini"
        >
          ノード情報
        </el-button>
      </el-col>
    </el-row>

    <el-row style="padding-top:5px">
      <div v-if="errors" class="error-message">
        <div v-for="(error, index) in errors" :key="index">
          {{ error }}
        </div>
      </div>
      <el-form-item label="CPU" required>
        <el-slider
          v-model="value.cpu"
          class="el-input"
          :min="1"
          :max="quota.cpu === 0 ? defaultMax.cpu : quota.cpu"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
      <el-form-item label="メモリ(GB)" required>
        <el-slider
          v-model="value.memory"
          class="el-input"
          :min="1"
          :max="quota.memory === 0 ? defaultMax.memory : quota.memory"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
      <el-form-item label="GPU" required>
        <el-slider
          v-model="value.gpu"
          class="el-input"
          :min="0"
          :max="quota.gpu === 0 ? defaultMax.gpu : quota.gpu"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
    </el-row>
  </div>
</template>

<script>
import KqiAllocatableNodeInfo from '@/components/KqiAllocatableNodeInfo'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('cluster')

export default {
  components: {
    KqiAllocatableNodeInfo,
  },
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
      // 各リソース値の最大ノード情報
      maxCpuNode: null,
      maxMemoryNode: null,
      maxGpuNode: null,
      // エラーメッセージ
      errors: [],
    }
  },
  computed: {
    ...mapGetters(['nodes']),
  },
  async created() {
    await this.getAllocatableNodes()

    if (this.nodes && this.nodes.length > 0) {
      // 1つ目の要素をもとに最大値を求める。
      this.maxCpuNode = this.nodes[0]
      this.maxMemoryNode = this.nodes[0]
      this.maxGpuNode = this.nodes[0]
      this.nodes.forEach(node => {
        if (node.allocatableCpu > this.maxCpuNode.allocatableCpu) {
          this.maxCpuNode = node
        }
        if (node.allocatableMemory > this.maxMemoryNode.allocatableMemory) {
          this.maxMemoryNode = node
        }
        if (node.allocatableGpu > this.maxGpuNode.allocatableGpu) {
          this.maxGpuNode = node
        }
      })
    }
    this.resourceValidator()
  },
  methods: {
    ...mapActions(['fetchNodes']),
    // 利用可能なノード一覧を取得する。
    async getAllocatableNodes() {
      await this.fetchNodes()
    },
    // 要求リソース確認
    resourceValidator() {
      // エラーメッセージを初期化
      this.errors = []

      // 利用可能なノードがあるか確認
      if (!this.nodes || this.nodes.length < 1) {
        this.errors.push('利用可能なノードがありません。')
        this.errors.push('システム管理者に確認してください。')
      } else {
        // 利用可能なノードがあれば設定リソース値を確認
        let message = []
        // 組み合わせ確認フラグ
        let combinationCheckFlg = true

        // CPUの設定値をもとにチェックする。
        if (this.maxCpuNode) {
          if (this.maxCpuNode.allocatableCpu < this.value.cpu) {
            message.push('CPU')
          } else if (
            this.maxCpuNode.allocatableMemory < this.value.memory ||
            this.maxCpuNode.allocatableGpu < this.value.gpu
          ) {
            // 実行可能な組み合わせではないため、ユーザにお知らせする。
            combinationCheckFlg = false
          }
        }
        // メモリの設定値をもとにチェックする。
        if (this.maxMemoryNode) {
          if (this.maxMemoryNode.allocatableMemory < this.value.memory) {
            message.push('メモリ')
          } else if (
            this.maxMemoryNode.allocatableCpu < this.value.cpu ||
            this.maxMemoryNode.allocatableGpu < this.value.gpu
          ) {
            // 実行可能な組み合わせではないため、ユーザにお知らせする。
            combinationCheckFlg = false
          }
        }
        // GPUの設定値をもとにチェックする。
        if (this.maxGpuNode) {
          if (this.maxGpuNode.allocatableGpu < this.value.gpu) {
            message.push('GPU')
          } else if (
            this.maxGpuNode.allocatableCpu < this.value.cpu ||
            this.maxGpuNode.allocatableMemory < this.value.memory
          ) {
            // 実行可能な組み合わせではないため、ユーザにお知らせする。
            combinationCheckFlg = false
          }
        }
        // メッセージがあればエラーメッセージに格納し表示する。
        if (message.length > 0) {
          this.errors.push(
            'ノードに要求分の' + message.join(',') + 'のリソースがありません。',
          )
        } else if (!combinationCheckFlg) {
          this.errors.push('要求分のリソースで実行可能なノードがありません。')
        }
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.error-message {
  color: red;
}
</style>
