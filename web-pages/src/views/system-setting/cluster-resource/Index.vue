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
        </el-radio-group>
      </el-col>
      <el-col :span="12" align="right">
        <el-button
          icon="el-icon-refresh"
          type="primary"
          plain
          @click="handleReload"
        >
          リロード
        </el-button>
      </el-col>
    </el-row>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  title: 'リソース利用状況',
  data: function() {
    return {
      mode: '',
    }
  },
  watch: {
    // eslint-disable-next-line no-unused-vars
    $route(to, from) {
      this.setMode()
    },
  },
  created() {
    this.setMode()
  },
  methods: {
    setMode() {
      let path = this.$route.path
      this.mode = path.split('/').pop() // urlの最後の要素を取り出す('cluster-resource'か'tenant'か'list')

      // cluster-resource(この画面のトップページの場合、ノード別画面を表示)
      if (this.mode === 'cluster-resource') {
        this.mode = ''
      }
    },
    handleModeChange() {
      if (this.mode === '') {
        this.$router.push('/cluster-resource')
      }
      if (this.mode === 'tenant') {
        this.$router.push('/cluster-resource/tenant')
      }
      if (this.mode === 'container-list') {
        this.$router.push('/cluster-resource/container-list')
      }
    },
    handleReload() {
      this.$router.go()
    },
  },
}
</script>

<style lang="scss" scoped>
.switch-group {
  margin-bottom: 20px;
}
</style>
