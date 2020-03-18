<template>
  <div>
    <h2>バージョン情報</h2>
    <el-card class="box-card">
      <div slot="header">
        <span>About</span>
      </div>
      <div class="item">
        <el-row>
          <el-col :span="4">
            <img
              class="logo"
              src="@/assets/logo_A.png"
              alt=""
              width="150px"
              height="126px"
            />
          </el-col>
          <el-col :span="20">
            <br />
            <div class="title">
              {{ 'KAMONOHASHI web-pages' }}
            </div>
            <br />
            <div class="text">
              {{ 'web UI of KAMONOHASHI' }}
            </div>
          </el-col>
        </el-row>
        <br />
        <br />
        <el-row class="version">
          <el-col :span="4" style="text-align: center;">
            {{ 'バージョン :' }}
          </el-col>
          <el-col :span="20">
            <div style="margin-bottom: 20px;">
              {{ version.version }}
            </div>
            <div
              v-for="(message, index) in version.messages"
              :key="index"
              style="margin-bottom: 10px;"
            >
              {{ message }}
            </div>
          </el-col>
        </el-row>
        <br />
        <br />
        <br />
        <div class="text" style="text-align: right;">
          <a href="https://kamonohashi.ai/" target="_blank">
            KAMONOHASHI
          </a>
          {{ 'is made possible as an' }}
          <a href="https://github.com/KAMONOHASHI/kamonohashi" target="_blank">
            open source project
          </a>
          {{ '.' }}
        </div>
      </div>
    </el-card>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('version')

export default {
  title: 'バージョン情報',
  computed: {
    ...mapGetters(['version']),
  },
  async created() {
    await this.fetchVersion()
  },
  methods: {
    ...mapActions(['fetchVersion']),
  },
}
</script>

<style lang="scss" scoped>
.logo {
  display: block;
  margin-left: auto;
  margin-right: auto;
}

.title {
  font-size: 18px;
}

.text {
  font-size: 12px;
}

.version {
  font-size: 18px;
}

.item {
  margin-bottom: 18px;
}

.box-card {
  width: 100%;
}

a {
  color: #1abfd5;
  text-decoration: inherit;
}
</style>
