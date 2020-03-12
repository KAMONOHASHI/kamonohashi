<template>
  <kqi-dialog
    :title="title"
    type="EDIT"
    @submit="onSubmit"
    @delete="deleteInferenceJob"
    @close="$emit('cancel')"
  >
    <el-row type="flex" justify="end">
      <el-col :span="24" class="right-button-group">
        <el-button @click="emitCopyCreate">コピー実行</el-button>
      </el-col>
    </el-row>

    <el-form ref="updateForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-row :gutter="20">
        <el-col :span="12">
          <kqi-display-text-form
            label="推論ID"
            :value="detail ? String(detail.id) : '0'"
          >
            <span slot="action">
              <div
                v-if="form.favorite"
                class="el-icon-star-on favorite"
                @click="form.favorite = false"
              />
              <div
                v-else
                class="el-icon-star-off favorite"
                @click="form.favorite = true"
              />
            </span>
          </kqi-display-text-form>
          <el-form-item label="推論名" prop="name">
            <el-input v-model="form.name" />
          </el-form-item>
          <div v-if="detail.parent">
            <el-form-item label="親学習">
              <el-popover
                ref="parent-popover"
                title="親学習詳細"
                trigger="hover"
                width="350"
                placement="right"
              >
                <kqi-training-history-details :training="detail.parent" />
              </el-popover>
              <el-button
                v-popover:parent-popover
                class="el-input"
                @click="showParent"
              >
                {{ detail.parent.name }}
              </el-button>
            </el-form-item>
          </div>

          <div v-if="detail.dataSet">
            <el-form-item label="データセット">
              <el-popover
                ref="dataSetDetail"
                title="データセット詳細"
                trigger="hover"
                width="350"
                placement="right"
              >
                <kqi-data-set-details :data-set="detail.dataSet" />
              </el-popover>
              <el-button
                v-popover:dataSetDetail
                class="el-input"
                @click="redirectEditDataSet"
              >
                {{ detail.dataSet.name }}
              </el-button>
            </el-form-item>
          </div>

          <el-form-item label="モデル">
            <div class="el-input">
              <span v-if="detail.gitModel" style="padding-left: 3px">
                <a :href="detail.gitModel.url" target="_blank">
                  {{ detail.gitModel.owner }}/{{
                    detail.gitModel.repository
                  }}/{{ detail.gitModel.branch }}
                </a>
              </span>
              <span v-else>
                None
              </span>
            </div>
          </el-form-item>

          <kqi-display-text-form
            label="コンテナイメージ"
            :value="detail.containerImage ? detail.containerImage.url : ''"
          />

          <label>実行コマンド</label>
          <el-input
            v-model="detail.entryPoint"
            type="textarea"
            :autosize="{ minRows: 2 }"
            :readonly="true"
          />

          <el-form-item label="メモ">
            <el-input
              v-model="form.memo"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4 }"
            />
          </el-form-item>

          <el-form-item v-if="detail.options" label="環境変数">
            <div class="el-input">
              <el-row v-for="option in detail.options" :key="option.key">
                <el-col :span="8" :offset="1">{{ option.key }}</el-col>
                <el-col :span="12">{{ option.value }}</el-col>
              </el-row>
            </div>
          </el-form-item>

          <kqi-display-text-form label="実行者" :value="detail.createdBy" />
          <kqi-display-text-form label="開始日時" :value="detail.createdAt" />
          <kqi-display-text-form label="完了日時" :value="detail.completedAt" />

          <kqi-display-text-form label="待機時間" :value="detail.waitingTime" />
          <kqi-display-text-form
            label="実行時間"
            :value="detail.executionTime"
          />
        </el-col>

        <el-col :span="12">
          <kqi-display-text-form
            label="CPU"
            :value="detail ? String(detail.cpu) : '0'"
          />
          <kqi-display-text-form
            label="メモリ(GB)"
            :value="detail ? String(detail.memory) : '0'"
          />
          <kqi-display-text-form
            label="GPU"
            :value="detail ? String(detail.gpu) : '0'"
          />
          <kqi-display-text-form
            label="パーティション"
            :value="detail.partition"
          />
          <!-- status: スクリプトがこけたときなどに"failed"になる -->
          <!-- statusType: コンテナの生死等 -->
          <kqi-display-text-form
            label="ステータス"
            :value="
              detail.status === detail.statusType
                ? detail.status
                : detail.statusType + ' (' + detail.status + ')'
            "
          />
          <div v-if="detail.conditionNote !== ``" class="k8s-event">
            {{ detail.conditionNote }}
          </div>
          <div v-if="events.length" class="k8s-event">
            <el-collapse accordion>
              <el-collapse-item title="ステータス詳細ログ">
                <div v-for="(event, index) in events" :key="index">
                  <div v-if="event.isError">message:{{ event.message }}</div>
                </div>
              </el-collapse-item>
            </el-collapse>
          </div>
          <div
            v-if="
              detail.statusType === 'Running' || detail.statusType === 'Error'
            "
          >
            <el-form-item label="操作">
              <div class="el-input">
                <kqi-job-stop-button
                  button-label="ジョブ停止"
                  title="ジョブを停止しますか"
                  @halt="handleHalt"
                  @userCancel="handleUserCancel"
                />
              </div>
              <div v-if="detail.status === 'Running'">
                <div class="el-input" style="padding: 10px 0">
                  <el-button @click="emitShell">Shell起動</el-button>
                </div>
              </div>
            </el-form-item>
          </div>
          <el-form-item label="コンテナ出力ファイル">
            <br />
            <el-button @click="emitFiles">ファイル一覧</el-button>
          </el-form-item>
          <el-form-item
            v-if="detail.zip === false"
            label="一括ダウンロードコマンド"
          >
            <el-input
              :value="
                `kqi inference download-container-files ${detail.id} -d ./`
              "
              :readonly="true"
            />
          </el-form-item>
          <el-form-item label="結果Zip圧縮">
            <div class="el-input">
              <span v-if="detail.zip">圧縮する</span>
              <span v-else>圧縮しない</span>
            </div>
          </el-form-item>
          <el-form-item label="添付ファイル">
            <br />
            <el-button size="mini" @click="emitLog">ログファイル閲覧</el-button>
            <kqi-file-manager
              v-if="uploadedFiles.length > 0"
              type="InferenceHistoryAttachedFiles"
              :uploaded-files="uploadedFiles"
              :deletable="true"
              @delete="deleteAttachedFile"
            />
            <kqi-file-manager
              ref="uploadFile"
              type="InferenceHistoryAttachedFiles"
            />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiJobStopButton from '@/components/KqiJobStopButton'
import KqiFileManager from '@/components/KqiFileManager.vue'
import KqiDataSetDetails from '@/components/selector/KqiDataSetDetails.vue'
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('inference')

export default {
  components: {
    KqiDialog,
    KqiJobStopButton,
    KqiDisplayError,
    KqiFileManager,
    KqiDataSetDetails,
    KqiTrainingHistoryDetails,
    KqiDisplayTextForm,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },

  data() {
    return {
      rules: {
        name: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
      },
      form: {
        name: null,
        favorite: false,
        memo: null,
      },
      title: '',
      dialogVisible: true,
      error: null,
    }
  },
  computed: {
    ...mapGetters(['detail', 'events', 'uploadedFiles']),
  },
  async created() {
    this.title = '推論履歴'
    await this.retrieveData()
    this.form.name = this.detail.name
    this.form.favorite = this.detail.favorite
    this.form.memo = this.detail.memo
  },
  methods: {
    ...mapActions([
      'fetchDetail',
      'fetchEvents',
      'fetchUploadedFiles',
      'postHalt',
      'postUserCancel',
      'postFiles',
      'put',
      'delete',
      'deleteFile',
    ]),
    async retrieveData() {
      await this.fetchDetail(this.id)
      await this.fetchUploadedFiles(this.detail.id)
      if (
        this.detail.statusType === 'Running' ||
        this.detail.statusType === 'Error'
      ) {
        await this.fetchEvents(this.detail.id)
      }
    },
    async updateHistory() {
      let params = {
        id: this.detail.id,
        model: {
          name: this.form.name,
          memo: this.form.memo,
          favorite: this.form.favorite,
        },
      }
      await this.put(params)
    },
    async onSubmit() {
      let form = this.$refs.updateForm

      await form.validate(async valid => {
        if (valid) {
          try {
            await this.updateHistory()
            await this.uploadFile()
            this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async handleHalt() {
      try {
        await this.postHalt(this.detail.id) // 異常停止（Status=Killed）
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async handleUserCancel() {
      try {
        await this.postUserCancel(this.detail.id) // 正常停止（Status=UserCanceled）
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async deleteInferenceJob() {
      try {
        await this.delete(this.detail.id)
        this.$emit('done', 'delete')
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async uploadFile() {
      // 独自ローディング処理のため共通側は無効
      this.$store.commit('setLoading', false)
      this.loading = true

      let uploader = this.$refs.uploadFile
      let fileInfo = await uploader.uploadFile()

      if (fileInfo !== undefined) {
        await this.postFiles({
          id: this.detail.id,
          fileInfo: fileInfo,
        })
      }

      // 共通側ローディングを再度有効化
      this.loading = false
      this.$store.commit('setLoading', true)
    },

    async deleteAttachedFile(fileId) {
      try {
        await this.deleteFile({
          id: this.detail.id,
          fileId: fileId,
        })
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    // 親ジョブ履歴の表示指示
    async showParent() {
      this.$router.push('/training/' + this.detail.parent.id)
    },
    redirectEditDataSet() {
      this.$router.push('/dataset/edit/' + this.detail.dataSet.id)
    },
    emitFiles() {
      this.$emit('files', this.detail.id)
    },
    emitShell() {
      this.$emit('shell', this.detail.id)
    },
    emitLog() {
      this.$emit('log', this.detail.id)
    },
    emitCopyCreate() {
      this.$emit('copyCreate', this.detail.id)
    },
    async emitInferenceCreate() {
      if (
        this.detail.status === 'Completed' ||
        this.detail.status === 'UserCanceled'
      ) {
        this.$router.push(
          '/inference/create/' + this.detail.id + '?origin=train',
        )
      } else {
        this.$notify.info({
          title: 'Information',
          message:
            'ステータスがCompletedまたはUserCanceledの学習のみ推論を実行できます。',
        })
      }
    },
  },
}
</script>
<style lang="scss" scoped>
.dialog /deep/ .el-dialog {
  min-width: 800px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}

.entry-point {
  padding-left: 10px;
}

.k8s-event {
  color: #909399;
}

.favorite {
  font-size: 20px;
  color: rgb(230, 162, 60);
}
</style>
