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
      <div v-if="warnMessage" class="error-message">
        {{ warnMessage }}
      </div>
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
          :max="setMaxCpu()"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
      <el-form-item label="メモリ(GB)" required>
        <el-slider
          v-model="value.memory"
          class="el-input"
          :min="1"
          :max="setMaxMemory()"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
      <el-form-item label="GPU" required>
        <el-slider
          v-model="value.gpu"
          class="el-input"
          :min="0"
          :max="setMaxGpu()"
          show-input
          @input="resourceValidator()"
        />
      </el-form-item>
    </el-row>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiAllocatableNodeInfo from '@/components/KqiAllocatableNodeInfo.vue'
import { createNamespacedHelpers } from 'vuex'
import * as gen from '@/api/api.generate'
import { PropType } from 'vue'
const { mapGetters, mapActions } = createNamespacedHelpers('cluster')
interface DataType {
  // デフォルトの最大値
  defaultMax: {
    cpu: number
    memory: number
    gpu: number
  }
  // 各リソース値の最大ノード情報
  maxCpuNode: null | gen.NssolPlatypusApiModelsClusterApiModelsNodeResourceOutputModel
  maxMemoryNode: null | gen.NssolPlatypusApiModelsClusterApiModelsNodeResourceOutputModel
  maxGpuNode: null | gen.NssolPlatypusApiModelsClusterApiModelsNodeResourceOutputModel
  // エラーメッセージ
  errors: Array<Error | null | string> | null
  // 元々の要求値から変更があった項目リスト
  originChangedList: Array<string>
  // 要求リソースの最大値
  maxResource: {
    cpu: number
    memory: number
    gpu: number
  }
}
export default Vue.extend({
  components: {
    KqiAllocatableNodeInfo,
  },
  props: {
    // cpu, memory, gpuのリソース量
    value: {
      type: Object as PropType<{ cpu: number; memory: number; gpu: number }>,
      default: (): { cpu: number; memory: number; gpu: number } => {
        return { cpu: 1, memory: 1, gpu: 0 }
      },
    },
    // 接続中テナントのクォータ情報
    quota: {
      type: Object as PropType<{ cpu: number; memory: number; gpu: number }>,
      default: () => {
        return { cpu: 0, memory: 0, gpu: 0 }
      },
    },
  },
  data(): DataType {
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
      // 元々の要求値から変更があった項目リスト
      originChangedList: [],
      // 要求リソースの最大値
      maxResource: {
        cpu: 250,
        memory: 500,
        gpu: 16,
      },
    }
  },
  computed: {
    ...mapGetters(['nodes']),
    // 警告メッセージ
    warnMessage(): null | string {
      if (this.originChangedList.length > 0) {
        return (
          '元々の要求リソースから' +
          this.originChangedList.join(',') +
          'の値が変更されています。'
        )
      }
      return null
    },
  },
  async created() {
    await this.getAllocatableNodes()

    if (this.nodes && this.nodes.length > 0) {
      // 1つ目の要素をもとに最大値を求める。
      this.maxCpuNode = this.nodes[0]
      this.maxMemoryNode = this.nodes[0]
      this.maxGpuNode = this.nodes[0]
      this.nodes.forEach(
        (
          node: gen.NssolPlatypusApiModelsClusterApiModelsNodeResourceOutputModel | null,
        ) => {
          if (node!.allocatableCpu! > this.maxCpuNode!.allocatableCpu!) {
            this.maxCpuNode = node
          }
          if (
            node!.allocatableMemory! > this.maxMemoryNode!.allocatableMemory!
          ) {
            this.maxMemoryNode = node
          }
          if (node!.allocatableGpu! > this.maxGpuNode!.allocatableGpu!) {
            this.maxGpuNode = node
          }
        },
      )
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
        // 各リソースの最大値を持つノードに対して、それ以上のリソースを指定していればエラーを出す
        let message = []
        // CPUの設定値をもとにチェックする。
        if (this.maxCpuNode) {
          if (this.maxCpuNode.allocatableCpu! < this.value.cpu) {
            message.push('CPU')
          }
        }
        // メモリの設定値をもとにチェックする。
        if (this.maxMemoryNode) {
          if (this.maxMemoryNode.allocatableMemory! < this.value.memory) {
            message.push('メモリ')
          }
        }
        // GPUの設定値をもとにチェックする。
        if (this.maxGpuNode) {
          if (this.maxGpuNode.allocatableGpu! < this.value.gpu) {
            message.push('GPU')
          }
        }
        // メッセージがあればエラーメッセージに格納し表示する。
        if (message.length > 0) {
          this.errors.push(
            'ノードに要求分の' + message.join(',') + 'のリソースがありません。',
          )

          // エラーを出力したら抜ける
          return
        }

        // 各リソースの組み合わせに対して、条件を満たすノードが1つ以上存在するか判定する
        var allocatableNodes = this.nodes.filter(
          (n: {
            name?: string | null
            memo?: string | null
            partition?: string | null
            accessLevel?: gen.NssolPlatypusInfrastructureNodeAccessLevel
            allocatableCpu?: number
            allocatableMemory?: number
            allocatableGpu?: number
          }) =>
            n.allocatableCpu! >= this.value.cpu &&
            n.allocatableMemory! >= this.value.memory &&
            n.allocatableGpu! >= this.value.gpu,
        )
        if (allocatableNodes.length == 0) {
          this.errors.push('要求分のリソースで実行可能なノードがありません。')

          // エラーを出力したら抜ける
          return
        }
      }
    },
    // スライダーに設定するCPUの最大値
    setMaxCpu() {
      // 最大値として設定する値
      this.maxResource.cpu =
        this.quota.cpu === 0 ? this.defaultMax.cpu : this.quota.cpu
      // 最大値より元々の要求値が超えている場合はリストに追加する
      if (this.maxResource.cpu < this.value.cpu) {
        if (!this.originChangedList.includes('CPU')) {
          this.originChangedList.push('CPU')
        }
      }
      return this.maxResource.cpu
    },
    // スライダーに設定するメモリの最大値
    setMaxMemory() {
      // 最大値として設定する値
      this.maxResource.memory =
        this.quota.memory === 0 ? this.defaultMax.memory : this.quota.memory
      // 最大値より元々の要求値が超えている場合はリストに追加する
      if (this.maxResource.memory < this.value.memory) {
        if (!this.originChangedList.includes('メモリ')) {
          this.originChangedList.push('メモリ')
        }
      }
      return this.maxResource.memory
    },
    // スライダーに設定するGPUの最大値
    setMaxGpu() {
      // 最大値として設定する値
      this.maxResource.gpu =
        this.quota.gpu === 0 ? this.defaultMax.gpu : this.quota.gpu
      // 最大値より元々の要求値が超えている場合はリストに追加する
      if (this.maxResource.gpu < this.value.gpu) {
        if (!this.originChangedList.includes('GPU')) {
          this.originChangedList.push('GPU')
        }
      }
      return this.maxResource.gpu
    },
  },
})
</script>

<style lang="scss" scoped>
.error-message {
  color: red;
}
</style>
