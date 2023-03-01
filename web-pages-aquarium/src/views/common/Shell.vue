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
      <div id="terminal" />
      <div>
        コピー: Ctrl+Insert, ペースト: Shift+Insert(Google
        Chromeの場合Ctrl+Shift+Vでペーストも可能)
      </div>
      <el-row :gutter="20" class="footer">
        <el-col class="right-button-group" :span="24">
          <el-button @click="emitReturn()">戻る</el-button>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import '@/xterm.css'
import { Terminal } from 'xterm'
import { AttachAddon } from 'xterm-addon-attach'
import { FitAddon } from 'xterm-addon-fit'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('account')

let socket: WebSocket | null
interface DataType {
  type: null | string
  dialogVisible: boolean
  error: null | Error
  title: null | string
  intervalId: number
}
export default Vue.extend({
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    return {
      type: null,
      dialogVisible: true,
      error: null,
      title: null,
      intervalId: -1,
    }
  },
  computed: {
    ...mapGetters(['account']),
  },
  async mounted() {
    let url = this.$route.path
    this.type = url.split('/')[1] // ["", "{type}", "{id}", "shell"]
    if (this.type === 'preprocessingShell') {
      this.type = 'preproc'
    }
    await this.connectShell()
  },
  methods: {
    ...mapActions(['fetchAccount']),

    emitCancel() {
      this.$emit('cancelShell')
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
      await this.fetchAccount()

      // xtermjsの設定
      let term = new Terminal()
      const fitAddon = new FitAddon()
      term.loadAddon(fitAddon)
      term.setOption('cursorBlink', true)
      term.open(document.getElementById('terminal')!)
      fitAddon.fit()

      try {
        // ジョブ名を作成 例: training-101, preproc-30
        let jobName = `${this.type}-${this.id}`
        this.title = `Shell in ${jobName}`
        let tenantName = this.account.selectedTenant.name

        // ||を合体演算子として使う https://en.wikipedia.org/wiki/Null_coalescing_operator
        // VUE_APP_API_HOST: .envから取得
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
        socket.onclose = function() {
          socket = null
        }
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
})
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
