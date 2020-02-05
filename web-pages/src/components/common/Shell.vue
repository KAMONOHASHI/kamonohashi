<template>
  <div>
    <el-dialog
      class="dialog"
      :title="title"
      :visible.sync="dialogVisible"
      :before-close="emitCancel"
      width="80%"
      :close-on-click-modal="false"
    >
      <div id="terminal"></div>
      <div>
        コピー: Ctrl+Insert, ペースト: Shift+Insert(Google
        Chromeの場合Ctrl+Shift+Vでペーストも可能)
      </div>
      <el-row :gutter="20" class="footer">
        <el-col class="right-button-group" :span="24">
          <el-button @click="emitReturn">戻る</el-button>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import '@/xterm.css'
import { Terminal } from 'xterm'
import { AttachAddon } from 'xterm-addon-attach'
import { FitAddon } from 'xterm-addon-fit'

let socket
export default {
  name: 'Shell',
  props: {
    id: String,
    dataId: String, // 前処理履歴用
  },
  data() {
    return {
      type: undefined,
      dialogVisible: true,
      error: undefined,
      title: undefined,
      intervalId: -1,
    }
  },
  async mounted() {
    let url = this.$route.path
    this.type = url.split('/')[1] // ["", "{type}", "{id}", "shell"]
    await this.connectShell()
  },
  methods: {
    emitCancel() {
      this.$emit('cancel')
      this.closeSocket()
    },
    emitReturn() {
      this.$emit('return')
      this.closeSocket()
    },
    closeSocket() {
      if (socket) {
        clearInterval(this.intervalId)
        socket.close()
      }
    },

    async connectShell() {
      // テナント名を取得
      let res = await api.account.get()
      let tenantName = res.data.selectedTenant.name

      // xtermjsの設定
      let term = new Terminal()
      const fitAddon = new FitAddon()
      term.loadAddon(fitAddon)
      term.setOption('cursorBlink', true)
      term.open(document.getElementById('terminal'))
      fitAddon.fit()

      try {
        // ジョブ名を取得
        let jobName = ''
        if (this.type === 'training') {
          jobName = (await api.training.getById({ id: this.id })).data.key
        } else if (this.type === 'inference') {
          jobName = (await api.inference.getById({ id: this.id })).data.key
        } else if (
          this.type === 'preprocessing' ||
          this.type === 'preprocessingHistory'
        ) {
          jobName = (
            await api.preprocessings.getHistroyById({
              id: this.id,
              dataId: this.dataId,
            })
          ).data.key
        } else if (this.type === 'notebook') {
          jobName = (await api.notebook.getById({ id: this.id })).data.key
        }
        this.title = `Shell in ${jobName}`

        // ||を合体演算子として使う https://en.wikipedia.org/wiki/Null_coalescing_operator
        // API_HOST: webpackのdefine pluginから渡ってくる。
        let websocketServer =
          process.env.VUE_APP_API_HOST || window.location.hostname

        socket = new WebSocket(
          'ws://' +
            websocketServer +
            '/ws?jobName=' +
            jobName +
            '&tenantName=' +
            tenantName,
        )

        const attachAddon = new AttachAddon(socket)

        socket.onopen = function() {
          term.loadAddon(attachAddon)
        }
        /* eslint-disable */
        socket.onclose = function(evt) {
          socket = null;
        };
        /* eslint-enable */
        if (socket) {
          this.intervalId = setInterval(() => {
            socket.send('')
          }, 30000)
        }
      } catch (e) {
        this.title = `Shell`
      }
    },
  },
}
</script>
<style lang="scss" scoped>
#terminal {
  height: 700px;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
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
</style>
