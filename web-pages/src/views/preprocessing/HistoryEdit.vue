<template>
  <el-dialog
    class="dialog"
    title="前処理履歴編集"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-form ref="editForm">
      <pl-display-error :error="error" />
      <pl-display-text label="ID" :value="preprocessingId" />
      <pl-display-text label="データ名" :value="historyDetail.dataName" />
      <pl-display-text label="前処理名" :value="historyDetail.preprocessName" />
      <pl-display-text label="実行日時" :value="historyDetail.createdAt" />
      <el-form-item label="前処理ログ">
        <br />
        <pl-download
          :download-url="logFile.url"
          :file-name="logFile.fileName"
        />
        <el-button size="mini" @click="emitLog">閲覧</el-button>
      </el-form-item>
      <pl-display-text label="ステータス" :value="historyDetail.status" />
      <div v-if="historyDetail.status === 'Running'">
        <el-form-item label="操作">
          <div class="el-input">
            <el-button @click="emitShell">Shell起動</el-button>
          </div>
        </el-form-item>
      </div>
      <div v-if="historyEvents.length">
        <el-collapse accordion>
          <el-collapse-item title="ステータス詳細ログ">
            <div v-for="(event, index) in historyEvents" :key="index">
              <div v-if="event.isError">message:{{ event.message }}</div>
            </div>
          </el-collapse-item>
        </el-collapse>
      </div>
      <el-row>
        <el-col class="button-group">
          <el-button
            class="pull-right btn-cancel"
            icon="el-icon-close"
            @click="handleCancel"
          >
            キャンセル
          </el-button>
          <pl-delete-button
            class="pull-left btn-update"
            message="削除しますか（出力データ数が多い場合、処理に時間がかかります）"
            @delete="handleRemove"
          />
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
import DeleteButton from '@/components/common/DeleteButton.vue'
import DisplayError from '@/components/common/DisplayError'
import DownloadButton from '@/components/common/DownloadButton.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('preprocessing')

export default {
  components: {
    'pl-display-error': DisplayError,
    'pl-display-text': DisplayTextForm,
    'pl-delete-button': DeleteButton,
    'pl-download': DownloadButton,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
    dataId: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      dialogVisible: true,
      preprocessingId: null,
      error: null,
    }
  },
  computed: {
    ...mapGetters(['historyDetail', 'historyEvents', 'logFile']),
  },

  watch: {
    async id() {
      await this.changeValue()
    },
    async dataId() {
      await this.changeValue()
    },
  },

  async created() {
    await this.changeValue()
  },

  methods: {
    ...mapActions([
      'fetchHistoryDetail',
      'fetchHistoryEvents',
      'fetchLogFile',
      'deleteHistory',
    ]),

    async changeValue() {
      this.dataName = ''
      this.preprocessName = ''
      this.createdAt = ''
      this.status = ''

      if (this.id && this.dataId) {
        try {
          await this.fetchHistoryDetail({ id: this.id, dataId: this.dataId })
          this.preprocessingId = this.historyDetail.key.split('-')[1]
          this.error = null
          if (
            this.historyDetail.statusType === 'Running' ||
            this.historyDetail.statusType === 'Error'
          ) {
            await this.fetchHistoryEvents({ id: this.id, dataId: this.dataId })
          }
          await this.fetchLogFile({ id: this.id, dataId: this.dataId })
        } catch (e) {
          this.error = e
        }
      }
    },

    async handleRemove() {
      try {
        await this.deleteHistory({ id: this.id, dataId: this.dataId })
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleCancel() {
      this.emitCancel()
    },
    emitShell() {
      this.$emit('shell', { id: this.preprocessingId })
    },
    emitLog() {
      this.$emit('log', { id: this.id, dataId: this.dataId })
    },
    emitDone() {
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.btn-update {
  margin-left: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.pull-right {
  float: right !important;
}

.pull-left {
  float: left !important;
}
</style>
