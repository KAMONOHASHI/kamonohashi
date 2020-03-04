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
  created() {
    let path = this.$route.path
    this.mode = path.split('/').pop() // urlの最後の要素を取り出す('cluster-resource'か'tenant'か'list')
  },
  methods: {
    handleModeChange(mode) {
      switch (mode) {
        case '':
          this.$router.push('/cluster-resource')
          break
        case 'tenant':
          this.$router.push('/cluster-resource/tenant')
          break
        case 'container-list':
          this.$router.push('/cluster-resource/container-list')
          break
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
