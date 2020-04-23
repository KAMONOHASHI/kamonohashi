<template>
  <div>
    <h2>テナントリソース管理</h2>
    <br />
    <h3>クォーター情報</h3>
    <el-table class="data-table pl-index-table" :data="form" border>
      <el-table-column prop="limitCpu" label="CPU" width="200px" />
      <el-table-column prop="limitMemory" label="Memory" width="200px" />
      <el-table-column prop="limitGpu" label="GPU" width="200px" />
    </el-table>
    <br />
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
    </el-row>
    <router-view />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  title: 'テナントリソース管理',
  data: function() {
    return {
      form: [
        {
          limitCpu: null,
          limitMemory: null,
          limitGpu: null,
        },
      ],
      mode: '',
    }
  },
  computed: {
    ...mapGetters(['tenantQuota']),
  },
  async created() {
    await this.fetchTenantQuota()
    this.form[0].limitCpu =
      this.tenantQuota.limitCpu != null ? this.tenantQuota.limitCpu : '無制限'
    this.form[0].limitMemory =
      this.tenantQuota.limitMemory != null
        ? this.tenantQuota.limitMemory
        : '無制限'
    this.form[0].limitGpu =
      this.tenantQuota.limitGpu != null ? this.tenantQuota.limitGpu : '無制限'
    this.setMode()
  },
  methods: {
    ...mapActions(['fetchTenantQuota']),
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
}
</script>

<style lang="scss" scoped></style>
