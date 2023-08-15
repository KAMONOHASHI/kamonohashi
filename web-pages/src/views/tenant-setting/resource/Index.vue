<template>
  <div>
    <h2>テナントリソース管理</h2>
    <el-row>
      <el-col :span="12">
        <el-radio-group
          v-model="mode"
          class="switch-group"
          @change="handleModeChange"
        >
          <el-radio-button label="">ノード別</el-radio-button>
          <el-radio-button label="container-list">コンテナ一覧</el-radio-button>
        </el-radio-group>
      </el-col>
      <el-col :span="12" align="right">
        <el-popover ref="QuotaInfo" title="クォータ情報" trigger="hover">
          <kqi-quota-info :quota="quota" />
        </el-popover>
        <el-button v-popover:QuotaInfo icon="el-icon-info" type="primary" plain>
          クォータ情報
        </el-button>
      </el-col>
    </el-row>
    <router-view />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiQuotaInfo from '@/components/KqiQuotaInfo.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('cluster')

interface DataType {
  mode?: string
}

export default Vue.extend({
  components: {
    KqiQuotaInfo,
  },
  data(): DataType {
    return {
      mode: '',
    }
  },
  computed: {
    ...mapGetters(['quota']),
  },
  async created() {
    await this.fetchQuota()
    this.setMode()
  },
  methods: {
    ...mapActions(['fetchQuota']),
    setMode() {
      let path = this.$route.path
      let lastElement = path.split('/').pop() // urlの最後の要素を取り出す
      if (lastElement === 'resource') {
        this.mode = ''
      } else {
        this.mode = lastElement
      }
    },
    handleModeChange() {
      switch (this.mode) {
        case '':
          this.$router.push('/manage/resource')
          break
        case 'container-list':
          this.$router.push('/manage/resource/container-list')
          break
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.switch-group {
  margin-bottom: 20px;
}
</style>
