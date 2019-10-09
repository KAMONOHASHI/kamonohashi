<template>
  <div>
    <el-dialog class="dialog"
               title="学習履歴"
               :visible.sync="dialogVisible"
               :before-close="closeDialog"
               :close-on-click-modal="false">
      <el-row type="flex" justify="end">
        <el-col :span="24" class="right-button-group">
          <el-button @click="emitInferenceCreate">推論実行</el-button>
          <el-button @click="emitCopyCreate">コピー実行</el-button>
        </el-col>
      </el-row>

      <el-form :model="this" :rules="rules" ref="updateForm">
        <pl-display-error :error="error"/>
        <el-row :gutter="20">
          <el-col :span="12">
            <pl-display-text-form label="学習ID" :value="id">
              <span slot="action">
                <div class="el-icon-star-on favorite" v-if="favorite" v-on:click="favorite = false"></div>
                <div class="el-icon-star-off favorite" v-else v-on:click="favorite = true"></div>
              </span>
            </pl-display-text-form>
            <el-form-item label="学習名" prop="name">
              <el-input v-model="name"/>
            </el-form-item>
            <div v-if="parent">
              <el-form-item label="親学習">
                <el-popover
                  ref="parentDetail"
                  title="親学習詳細"
                  trigger="hover"
                  width="350"
                  placement="right">
                  <pl-training-history-details
                    :id="parent.id"
                    :name="parent.name"
                    :status="parent.status"
                    :memo="parent.memo"
                  />
                </el-popover>
                <el-button class="el-input" v-popover:parentDetail @click="showParent">{{parent.name}}</el-button>
              </el-form-item>
            </div>

            <div v-if="dataSet">
              <el-form-item label="データセット">
                <el-popover
                  ref="dataSetDetail"
                  title="データセット詳細"
                  trigger="hover"
                  width="350"
                  placement="right">
                  <pl-dataset-details :dataSet="dataSet"/>
                </el-popover>
                <el-button class="el-input" @click="redirectEditDataSet" v-popover:dataSetDetail>{{dataSet.name}}
                </el-button>
              </el-form-item>
            </div>

            <el-form-item label="モデル">
              <div class="el-input">
            <span v-if="gitModel" style="padding-left: 3px">
              <a :href="gitModel.url" target="_blank">
                {{gitModel.owner}}/{{gitModel.repository}}/{{gitModel.branch}}
              </a>
            </span>
                <span v-else>
              None
            </span>
              </div>
            </el-form-item>

            <pl-display-text-form label="実行者" :value="createdBy"/>
            <pl-display-text-form label="開始日時" :value="createdAt"/>
            <pl-display-text-form label="完了日時" :value="completedAt"/>

            <pl-display-text-form label="待機時間" :value="waitingTime"/>
            <pl-display-text-form label="実行時間" :value="executionTime"/>

            <label>実行コマンド</label>
            <el-input
              type="textarea"
              :autosize="{ minRows: 2}"
              v-model="entryPoint" :readonly="true"/>

            <pl-display-text-form label="ログ概要" :value="logSummary"/>

            <el-form-item label="メモ">
              <el-input
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 4}"
                v-model="memo">
              </el-input>
            </el-form-item>

            <el-form-item label="環境変数" v-if="options">
              <div class="el-input">
                <el-row v-for="option in options" :key="option.key">
                  <el-col :span="8" :offset="1">{{option.key}}</el-col>
                  <el-col :span="12">{{option.value}}</el-col>
                </el-row>
              </div>
            </el-form-item>

            <pl-display-text-form label="コンテナイメージ" :value="containerUrl"/>

          </el-col>

          <el-col :span=12>

            <pl-display-text-form label="CPU" :value="cpu"/>
            <pl-display-text-form label="メモリ(GB)" :value="memory"/>
            <pl-display-text-form label="GPU" :value="gpu"/>
            <pl-display-text-form label="パーティション" :value="partition"/>
            <pl-display-text-form label="ステータス" :value="status"/>
            <div class="k8s-event" v-if="conditionNote !== `` ">{{ conditionNote}}</div>
            <div class="k8s-event" v-if="events.length">
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
            <div v-if="statusType === 'Running'  || statusType === 'Error'">
              <el-form-item label="操作">
                <div class="el-input">
                  <pl-delete-button buttonLabel="ジョブ停止" @delete="showConfirm" message="ジョブを停止しますか"/>
                </div>
                <div v-if="status === 'Running'">
                  <div class="el-input" style="padding: 10px 0">
                    <el-button @click="emitShell">Shell起動</el-button>
                  </div>
                </div>
              </el-form-item>
            </div>
            <el-form-item label="TensorBoard">
              <pl-tensorboard-handler :trainingHistoryId="Number(trainingId)" :visible="dialogVisible"/>
            </el-form-item>

            <el-form-item label="コンテナ出力ファイル">
              <br/>
              <el-button @click="emitFiles">ファイル一覧</el-button>
            </el-form-item>
            <el-form-item  label="一括ダウンロードコマンド" v-if="zip === false">
                  <el-input v-model="downloadFilesCommand" :readonly="true"/>
            </el-form-item >
            <el-form-item label="結果Zip圧縮">
              <el-switch v-model="zip"
                          style="width: 100%;"
                          inactive-text="圧縮しない"
                          active-text="圧縮する"
                          disabled/>
            </el-form-item>
            <el-form-item label="添付ファイル">
              <br/>
              <el-button @click="emitLog" size="mini">ログファイル閲覧</el-button>
              <pl-file-manager v-if="uploadedFiles.length > 0"
                               type="TrainingHistoryAttachedFiles"
                               :uploadedFiles="uploadedFiles"
                               @delete="deleteFile"
                               :deletable="true"
              />
              <pl-file-manager ref="uploadFile" type="TrainingHistoryAttachedFiles"/>
            </el-form-item>

          </el-col>
        </el-row>

        <el-row :gutter="20" class="footer">
          <el-col :span="12">
            <pl-delete-button @delete="deleteJob"/>
          </el-col>
          <el-col class="right-button-group" :span="12">
            <el-button @click="emitCancel">キャンセル</el-button>
            <el-button type="primary" @click="onSubmit">保存</el-button>
          </el-col>
        </el-row>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DisplayError from '@/components/common/DisplayError'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import FileManager from '@/components/common/FileManager.vue'
  import DataSetDetails from '@/components/common/DatasetDetails.vue'
  import TrainingHistoryDetails from '@/components/common/TrainingHistoryDetails.vue'
  import TensorBoardHandler from '@/components/training/TensorboardHandler.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'EditTrain',
    components: {
      'pl-delete-button': DeleteButton,
      'pl-display-text-form': DisplayTextForm,
      'pl-display-error': DisplayError,
      'pl-file-manager': FileManager,
      'pl-dataset-details': DataSetDetails,
      'pl-training-history-details': TrainingHistoryDetails,
      'pl-tensorboard-handler': TensorBoardHandler
    },
    props: {
      id: String
    },
    data () {
      return {
        rules: {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}]
        },
        trainingId: undefined,
        dialogVisible: true,
        error: undefined,
        containerUrl: undefined, // コンテナの表示用URL
        uploadedFiles: [],
        name: undefined,
        parent: undefined,
        dataSet: undefined,
        gitModel: undefined,
        createdBy: undefined,
        createdAt: undefined,
        completedAt: undefined,
        entryPoint: undefined,
        logSummary: undefined,
        memo: undefined,
        options: undefined,
        cpu: undefined,
        memory: undefined,
        gpu: undefined,
        partition: undefined,
        // スクリプトがこけたときなどに"failed"になる
        status: undefined,
        // コンテナの生死等
        statusType: undefined,
        conditionNote: '',
        favorite: false,
        zip: true,
        events: [],
        waitingTime: undefined,
        executionTime: undefined,
        downloadFilesCommand: ''
      }
    },
    async created () {
      this.trainingId = this.id
      await this.getDetail()
    },
    async beforeUpdate () {
      // 子ジョブから親ジョブ詳細に遷移する際にブラウザの進む/戻るボタンを押した場合の対応処理
      // id(routerから受け取るパラメータ)とtrainingId(履歴検索に用いるID)が異なる場合、router側を優先した上で表示内容を更新
      if (this.trainingId.toString() !== this.id.toString()) {
        this.trainingId = this.id
        await this.getDetail()
      }
    },
    methods: {
      async showConfirm () {
        let confirmMessage = '正常停止しますか、異常停止しますか。'
        await this.$confirm(confirmMessage, 'Warning', {
          distinguishCancelAndClose: true,
          confirmButtonText: '正常停止',
          cancelButtonText: '異常停止',
          type: 'warning'
        })
        .then(() => {
          this.userCancelJob() // 正常停止（Status=UserCanceled）
        })
        .catch(action => {
          if (action === 'cancel') {
            this.haltJob() // 異常停止（Status=Killed）
          }
        })
      },
      async haltJob () {
        try {
          await api.training.postHaltById({id: this.trainingId})
          await this.getDetail()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async userCancelJob () {
        try {
          await api.training.postUserCancelById({id: this.trainingId})
          await this.getDetail()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async deleteJob () {
        try {
          await api.training.deleteById({id: this.trainingId})
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async uploadFile () {
        // 独自ローディング処理のため共通側は無効
        this.$store.commit('setLoading', false)
        this.loading = true

        let uploader = this.$refs.uploadFile
        let fileInfo = await uploader.uploadFile()

        if (fileInfo !== undefined) {
          for (let i = 0; i < fileInfo.length; i++) {
            fileInfo[i].FileName = fileInfo[i].name
            await api.training.postFilesById({id: this.trainingId, model: fileInfo[i]})
          }
        }

        // 共通側ローディングを再度有効化
        this.loading = false
        this.$store.commit('setLoading', true)
      },
      async setTrainDetails (data) {
        this.name = data.name
        this.trainingId = data.id
        this.parent = data.parent
        this.dataSet = data.dataSet
        this.gitModel = data.gitModel
        this.createdBy = data.createdBy
        this.createdAt = data.createdAt
        this.completedAt = data.completedAt
        this.logSummary = data.logSummary
        this.memo = data.memo
        this.options = data.options
        this.cpu = data.cpu
        this.memory = data.memory
        this.gpu = data.gpu
        this.partition = data.partition
        this.statusDetail = data.statusDetail
        this.status = data.status === data.statusType
          ? data.status : (data.statusType + ' (' + data.status + ')')
        this.statusType = data.statusType
        this.entryPoint = data.entryPoint
        this.conditionNote = data.conditionNote
        this.favorite = data.favorite
        this.zip = data.zip
        this.downloadFilesCommand = 'kqi training download-container-files ' + this.trainingId + ' -d ./'
        if (this.statusType === 'Running' || this.statusType === 'Error') {
          this.events = (await api.training.getEventsById({id: data.id})).data
        }
        this.waitingTime = data.waitingTime
        this.executionTime = data.executionTime
      },
      async getDetail () {
        let data = (await api.training.getById({id: this.trainingId})).data
        this.setTrainDetails(data)
        this.uploadedFiles = (await api.training.getFilesById({id: this.trainingId, withUrl: true})).data
        this.containerUrl = data.containerImage.url
      },
      async updateHistory () {
        let putData = {
          name: this.name,
          memo: this.memo,
          favorite: this.favorite
        }
        await api.training.putById({id: this.trainingId, model: putData})
      },
      async onSubmit () {
        let form = this.$refs.updateForm

        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.updateHistory()
              await this.uploadFile()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async deleteFile (fileId) {
        try {
          await api.training.deleteByIdFilesByFileId({id: this.trainingId, fileId: fileId})
          await this.getDetail()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      // 親ジョブ履歴の表示指示
      async showParent () {
        // 表示内容の変更は、beforeUpdated内で行う
        this.$router.push('/training/' + this.parent.id)
      },
      redirectEditDataSet () {
        this.$router.push('/dataset/' + this.dataSet.id)
      },
      emitFiles () {
        this.$emit('files', this.trainingId)
      },
      emitShell () {
        this.$emit('shell', this.trainingId)
      },
      emitLog () {
        this.$emit('log', this.trainingId)
      },
      emitCopyCreate () {
        this.$emit('copyCreate', this.trainingId)
      },
      async emitInferenceCreate () {
        let data = (await api.training.getById({id: this.trainingId})).data
        if (data.status === 'Completed' || data.status === 'UserCanceled') {
          this.$router.push('/inference/create/' + this.trainingId + '?origin=train')
        } else {
          this.$notify.info({
            title: 'Information',
            message: 'ステータスがCompletedまたはUserCanceledの学習のみ推論を実行できます。'
          })
        }
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.$emit('done')
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      }
    }
  }

</script>
<style lang="scss" scoped>
  .dialog /deep/ .el-dialog {
    min-width: 800px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
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
