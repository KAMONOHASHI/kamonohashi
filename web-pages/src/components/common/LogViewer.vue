<template>
  <el-dialog class="dialog"
             :title="title"
             :visible="true"
             :before-close="emitCancel"
             width="80%"
             :close-on-click-modal="false">

    <div id="logArea" @scroll="getScrollParam">
      <ul :style="listStyle">
        <li class="logLine" v-for="(log, i) in displayLogList" :key="i">
          {{log}}
        </li>
      </ul>
    </div>

    <el-row :gutter="20" class="footer">
      <el-col class="right-button-group" :span="24">
        <a :href="logUrl" :download="2">
          <el-button icon="el-icon-download"></el-button>
        </a>
        <el-button @click="emitReturn">戻る</el-button>
      </el-col>
    </el-row>

  </el-dialog>

</template>

<script>
  import api from '@/api/v1/api'

  const listItemHeight = 17
  const displayLogCount = 50

  export default {
    name: 'log-viewer',
    props: {
      id: String,
      dataId: String // 前処理履歴用
    },
    computed: {
      displayLogList: function () {
        let startIndex = parseInt(this.scroll / listItemHeight, 10)
        return this.logList.slice(
          Math.max(0, startIndex - displayLogCount),
          Math.min(this.logList.length, startIndex + displayLogCount)
        )
      },
      listStyle: function () {
        return {
          'padding-top': (Math.max(0, this.scroll - listItemHeight * displayLogCount)) + 'px',
          'padding-bottom': (this.scrollMax - this.scroll) + 'px'
        }
      }
    },
    async created () {
      let url = this.$route.path
      this.type = url.split('/')[1] // ["", "{type}", "{id}", "log"]

      // ジョブ/推論ジョブ/前処理に応じてリソースタイプを変更
      let resourceType = ''
      let storedPath = ''
      let fileName = ''

      if (this.type === 'training') {
        resourceType = 'TrainingContainerAttachedFiles'
        fileName = `training_stdout_stderr_${this.id}.log`
        storedPath = `${this.id}/${fileName}`
      } else if (this.type === 'inference') {
        resourceType = 'InferenceContainerAttachedFiles'
        fileName = `inference_stdout_stderr_${this.id}.log`
        storedPath = `${this.id}/${fileName}`
      } else if (this.type === 'preprocessing' || this.type === 'preprocessingHistory') {
        resourceType = 'PreprocContainerAttachedFiles'
        let history = (await api.preprocessings.getHistroyById({id: this.id, dataId: this.dataId})).data
        let key = history.key
        let historyId = key.split('-')[1] // "preproc-{id}" => ["preproc", "{id}"]
        fileName = `preproc_stdout_stderr_${this.id}_${this.dataId}.log`
        storedPath = `${historyId}/${fileName}`
      } else if (this.type === 'notebook') {
        resourceType = 'NotebookContainerAttachedFiles'
        fileName = `notebook_stdout_stderr_${this.id}.log`
        storedPath = `${this.id}/${fileName}`
      }

      // ログファイルのURLを取得
      this.title = fileName
      let params = {
        type: resourceType,
        storedPath: storedPath,
        fileName: fileName,
        secure: false
      }
      this.logUrl = (await api.storage.getDownloadUrl(params)).data.url

      // ログをダウンロードし、logListに格納
      this.$store.dispatch('incrementLoading')
      fetch(this.logUrl, {
        method: 'GET'
      }).then(response => response.text())
        .then(text => {
          this.logList = text.split(/[\n\r]+/)
          this.scrollMax = (this.logList.length) * listItemHeight
        })
        .then(_ => {
          document.getElementById('logArea').scrollTop = this.scrollMax
          this.$store.dispatch('decrementLoading')
        })
    },
    data () {
      return {
        type: undefined,
        title: 'Log',

        logUrl: '',
        logList: [],
        scroll: 0,
        scrollMax: 0
      }
    },
    methods: {
      emitCancel () {
        this.$emit('cancel')
      },
      emitReturn () {
        this.$emit('return')
      },
      getScrollParam (e) {
        if (e.target.scrollTop >= this.scrollMax) {
          e.target.scrollTop = this.scrollMax
        }
        this.scroll = e.target.scrollTop
      }
    }
  }
</script>

<style lang="scss" scoped>
  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .dialog /deep/ .el-dialog__body {
    padding-top: 10px;
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 10px;
  }

  .dialog /deep/ #logArea {
    height: 700px;
    overflow: auto;
    border: 1px solid #dcdfe6;
  }

  .dialog /deep/ .logLine {
    height: 17px;
    font-family: monospace;
    white-space: pre;
  }

</style>
