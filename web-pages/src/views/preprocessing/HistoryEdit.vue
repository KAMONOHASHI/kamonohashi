<template>
  <el-dialog
    class="dialog"
    title="前処理履歴詳細"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-form ref="editForm">
      <kqi-display-error :error="error" />
      <kqi-display-text-form label="前処理履歴ID" :value="preprocessingId" />
      <kqi-display-text-form
        label="データID"
        :value="String(historyDetail.dataId)"
      />
      <kqi-display-text-form label="データ名" :value="historyDetail.dataName" />
      <kqi-display-text-form
        label="前処理名"
        :value="historyDetail.preprocessName"
      />
      <kqi-display-text-form
        label="実行日時"
        :value="historyDetail.createdAt"
      />
      <el-form-item label="前処理ログ">
        <br />
        <kqi-download-button
          :download-url="logFile.url"
          :file-name="logFile.fileName"
        />
        <el-button size="mini" @click="emitLog">閲覧</el-button>
      </el-form-item>
      <kqi-display-text-form label="ステータス" :value="historyDetail.status" />
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
      <el-form-item v-if="outputDataIds" label="出力データID">
        <div v-if="outputDataIds.length >= 11">
          <el-button type="primary" @click="viewDataIds = !viewDataIds">
            {{ viewDataIds ? 'Hide DataIds' : 'View All DataIds' }}
          </el-button>
        </div>
        <div v-if="outputDataIds.length <= 10 || viewDataIds">
          <span
            v-for="(outputDataId, index) in outputDataIds"
            :key="index"
            class="outputDataId"
          >
            <el-link type="primary" @click="redirectDataEdit(outputDataId)">{{
              outputDataId
            }}</el-link>
          </span>
        </div>
      </el-form-item>
      <el-row>
        <el-col class="button-group">
          <el-button
            class="pull-right btn-cancel"
            icon="el-icon-close"
            @click="handleCancel"
          >
            閉じる
          </el-button>
          <kqi-delete-button
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
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiDeleteButton from '@/components/KqiDeleteButton'
import KqiDownloadButton from '@/components/KqiDownloadButton'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('preprocessing')

export default {
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiDeleteButton,
    KqiDownloadButton,
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
      viewDataIds: false,
      outputDataIds: null,
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
          if (this.historyDetail.outputDataIds.length !== 0) {
            this.outputDataIds = this.historyDetail.outputDataIds
          }
        } catch (e) {
          this.error = e
        }
      }
    },

    async handleRemove() {
      try {
        await this.deleteHistory({ id: this.id, dataId: this.dataId })
        this.$emit('done', 'delete')
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
    redirectDataEdit(dataId) {
      this.$router.push('/data/edit/' + dataId)
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
.outputDataId {
  margin: 10px;
}
</style>
