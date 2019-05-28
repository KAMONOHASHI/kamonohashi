<script src="../../../swagger-to-api.js"></script>
<template>
  <el-dialog class="dialog"
             title="前処理履歴編集"
             :visible="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form :model="this"
             ref="editForm">
      <pl-display-error :error="error"/>
      <pl-display-text label="データ名" :value="dataName"/>
      <pl-display-text label="前処理名" :value="preprocessName"/>
      <pl-display-text label="実行日" :value="createdAt"/>
      <el-form-item label="前処理ログ">
        <br/>
        <pl-download
          :downloadUrl="logFile.url"
          :fileName="logFile.fileName"
        />
        <el-button @click="emitLog" size="mini">閲覧</el-button>

      </el-form-item>
      <pl-display-text label="ステータス" :value="status"/>
      <div v-if="status === 'Running'">
        <el-form-item label="操作">
          <div class="el-input">
            <el-button @click="emitShell">Shell起動</el-button>
          </div>
        </el-form-item>
      </div>
      <div v-if="events.length">
        <el-collapse accordion>
          <el-collapse-item title="ステータス詳細ログ">
            <div v-for="(event, index) in events" :key="index">
              <div v-if="event.isError">
                message:{{ event.message}}
              </div>
            </div>
          </el-collapse-item>
        </el-collapse>
      </div>
      <el-row>
        <el-col class="button-group">
          <el-button @click="handleCancel" class="pull-right btn-cancel" icon="el-icon-close">キャンセル</el-button>
          <pl-delete-button class="pull-left btn-update" @delete="handleRemove"/>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import DisplayError from '@/components/common/DisplayError'
  import FileManager from '@/components/common/FileManager.vue'
  import DownloadButton from '@/components/common/DownloadButton.vue'

  export default {
    name: 'PreprocessHistoryEdit',
    components: {
      'pl-display-error': DisplayError,
      'pl-display-text': DisplayTextForm,
      'pl-delete-button': DeleteButton,
      'pl-file-manager': FileManager,
      'pl-download': DownloadButton
    },
    props: {
      id: String,
      dataId: String
    },
    data () {
      return {
        dialogVisible: true,
        error: null,
        dataName: '',
        preprocessName: '',
        createdAt: '',
        status: '',
        logFile: [],
        events: []
      }
    },

    async created () {
      await this.changeValue()
    },

    watch: {
      async id () {
        await this.changeValue()
      },
      async dataId () {
        await this.changeValue()
      }
    },

    methods: {
      async changeValue () {
        this.dataName = ''
        this.preprocessName = ''
        this.createdAt = ''
        this.status = ''

        if (this.id && this.dataId) {
          try {
            let params = {
              id: this.id,
              dataId: this.dataId
            }
            let data = (await api.preprocessings.getHistroyById(params)).data
            this.dataName = data.dataName
            this.preprocessName = data.preprocessName
            this.createdAt = data.createdAt
            this.status = data.status
            this.error = null
            if (data.statusType === 'Running' || data.statusType === 'Error') {
              this.events = (await api.preprocessings.getEventsById(params)).data
            }
            this.logFile = (await api.preprocessings.getFilesById({
              id: this.id,
              dataId: this.dataId,
              withUrl: true
            })).data
          } catch (e) {
            this.error = e
          }
        }
      },

      async handleRemove () {
        try {
          let params = {
            id: this.id,
            dataId: this.dataId
          }
          await api.preprocessings.deleteHistroyById(params)
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
      },
      emitShell () {
        this.$emit('shell', {id: this.id, dataId: this.dataId})
      },
      emitLog () {
        this.$emit('log', {id: this.id, dataId: this.dataId })
      },
      emitDone () {
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      }
    }
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
    font-weight: bold !important
  }

  .pull-right {
    float: right !important;
  }

  .pull-left {
    float: left !important;
  }
</style>
