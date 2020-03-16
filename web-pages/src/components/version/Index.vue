<template>
  <div>
    <h2>{{$t('title')}}</h2>
    <el-card class="box-card">
      <div slot="header">
        <span>About</span>

      </div>
      <div class="item">
        <el-row>
          <el-col :span="4">
            <img class="logo" src="/static/images/logo_A.png" alt="" width="150" height="126">
          </el-col>
          <el-col :span="20">
            <br>
            <div class="title">
              {{'KAMONOHASHI web-pages' }}
            </div>
            <br>
            <div class="text">
              {{'web UI of KAMONOHASHI' }}
            </div>
          </el-col>
        </el-row>
        <br>
        <br>
        <el-row class="version">
          <el-col :span="4" style="text-align: center">
            {{'バージョン :' }}
          </el-col>
          <el-col :span="20">
            <div style="margin-bottom: 20px;">
              {{ version }}
            </div>
            <div v-for="(message, index) in messages" :key="index" style="margin-bottom: 10px;">
              {{ message }}
            </div>
          </el-col>
        </el-row>
        <br>
        <br>
        <br>
        <el-row class="text" style="text-align: right">
          <a href="https://kamonohashi.ai/">KAMONOHASHI</a>
          {{'is made possible as an ' }}
          <a href="https://github.com/KAMONOHASHI/kamonohashi">open source project</a>
          {{'.' }}
        </el-row>
      </div>
    </el-card>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'VersionIndex',
    data () {
      return {
        version: null,
        messages: null
      }
    },
    title () {
      return this.$t('title')
    },
    i18n: {
      messages: {
        en: {
          title: 'Version'
        },
        ja: {
          title: 'バージョン情報'
        }
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        let response = (await api.version.get()).data
        this.version = response.version
        this.messages = response.messages
      }
    }
  }
</script>

<style scoped>
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
