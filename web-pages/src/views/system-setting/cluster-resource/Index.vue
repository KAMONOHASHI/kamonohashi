<template>
  <div>
    <h2>リソース利用状況</h2>

    <el-row>
      <el-col :span="12">
        <el-radio-group
          v-model="mode"
          class="switch-group"
          @change="handleModeChange"
        >
          <el-radio-button label="">ノード別</el-radio-button>
          <el-radio-button label="tenant">テナント別</el-radio-button>
          <el-radio-button label="container-list">コンテナ一覧</el-radio-button>
          <el-radio-button label="data-download">
            データダウンロード
          </el-radio-button>
        </el-radio-group>
      </el-col>

      <el-col :span="12" align="right">
        <el-button
          v-if="mode != 'data-download'"
          icon="el-icon-refresh"
          type="primary"
          plain
          @click="handleReload"
        >
          リロード
        </el-button>
      </el-col>
    </el-row>
    <router-view />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

interface DataType {
  mode?: string
  dialogVisible: boolean
}

export default Vue.extend({
  data(): DataType {
    return {
      mode: '',
      dialogVisible: false,
    }
  },
  watch: {
    // テナント別リソース利用状況を表示中→メニューからリソース管理を選択した場合に
    $route() {
      this.setMode()
    },
  },
  created() {
    this.setMode()
  },
  methods: {
    setMode() {
      let path = this.$route.path
      let lastElement = path.split('/').pop() // urlの最後の要素を取り出す('cluster-resource'か'tenant'か'container-list')
      // cluster-resource(この画面のトップページの場合、ノード別画面を表示)
      if (lastElement === 'cluster-resource') {
        this.mode = ''
      } else {
        this.mode = lastElement
      }
    },
    handleModeChange() {
      switch (this.mode) {
        case '':
          this.$router.push('/cluster-resource')
          break
        case 'tenant':
          this.$router.push('/cluster-resource/tenant')
          break
        case 'container-list':
          this.$router.push('/cluster-resource/container-list')
          break
        case 'data-download':
          this.$router.push('/cluster-resource/data-download')
          break
      }
    },
    handleReload() {
      //@ts-ignore
      this.$router.go()
    },
  },
})
</script>

<style lang="scss" scoped>
.switch-group {
  margin-bottom: 20px;
}
</style>
