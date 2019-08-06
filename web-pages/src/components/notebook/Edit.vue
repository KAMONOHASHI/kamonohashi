<template>
  <div>
    <el-dialog class="dialog"
               title="notebook詳細"
               :visible.sync="dialogVisible"
               :before-close="closeDialog"
               :close-on-click-modal="false">
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
            <el-form-item label="ノートブック名" prop="name">
              <el-input v-model="name"/>
            </el-form-item>
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
            <div class="k8s-event" v-if="conditionNote !== `` ">{{conditionNote}}</div>
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
                  <pl-delete-button buttonLabel="ジョブ停止" @delete="haltNotebook"/>
                </div>
                <div v-if="status === 'Running'">
                  <div class="el-input" style="padding: 10px 0">
                    <el-button @click="emitShell">Shell起動</el-button>
                  </div>
                  <div>
                    <el-button type="plain" @click="openNotebook" icon="el-icon-document" >ノートブックを開く</el-button>
                  </div>
                </div>
              </el-form-item>
            </div>
            <el-form-item label="コンテナ出力ファイル">
              <br/>
              <el-button @click="emitFiles">ファイル一覧</el-button>
            </el-form-item>
            <el-form-item label="ログファイル">
              <br/>
              <el-button @click="emitLog" size="mini">ログファイル閲覧</el-button>
            </el-form-item>
            <el-form-item label="メモ">
              <el-input
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 4}"
                v-model="memo">
              </el-input>
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20" class="footer">
          <el-col :span="12">
            <pl-delete-button @delete="deleteNotebook"/>
          </el-col>
          <el-col class="right-button-group" :span="12">
            <el-button @click="emitCancel">キャンセル</el-button>
            <el-button type="primary" @click="onSubmit">更新</el-button>
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
  import DataSetDetails from '@/components/common/DatasetDetails.vue'
  import api from '@/api/v1/api'
  export default {
    name: 'EditNotebook',
    components: {
      'pl-delete-button': DeleteButton,
      'pl-display-text-form': DisplayTextForm,
      'pl-display-error': DisplayError,
      'pl-dataset-details': DataSetDetails
    },
    props: {
      id: String
    },
    data () {
      return {
        rules: {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}]
        },
        notebookId: undefined,
        dialogVisible: true,
        error: undefined,
        containerUrl: undefined, // コンテナの表示用URL
        name: undefined,
        dataSet: undefined,
        gitModel: undefined,
        createdBy: undefined,
        createdAt: undefined,
        completedAt: undefined,
        memo: undefined,
        options: undefined,
        cpu: undefined,
        memory: undefined,
        gpu: undefined,
        partition: undefined,
        endpoint: undefined,
        // スクリプトがこけたときなどに"failed"になる
        status: undefined,
        // コンテナの生死等
        statusType: undefined,
        conditionNote: '',
        favorite: false,
        events: [],
        waitingTime: undefined,
        executionTime: undefined
      }
    },
    async created () {
      this.notebookId = this.id
      await this.getDetail()
    },
    methods: {
      async getDetail () {
        let data = (await api.notebook.getById({id: this.notebookId})).data
        this.setTrainDetails(data)
        this.containerUrl = data.containerImage.url
      },
      async setTrainDetails (data) {
        this.name = data.name
        this.notebookId = data.id
        this.dataSet = data.dataSet
        this.gitModel = data.gitModel
        this.createdBy = data.createdBy
        this.createdAt = data.createdAt
        this.completedAt = data.completedAt
        this.memo = data.memo
        this.options = data.options
        this.cpu = data.cpu
        this.memory = data.memory
        this.gpu = data.gpu
        this.partition = data.partition
        this.statusDetail = data.statusDetail
        this.endpoint = data.notebookEndpoint
        this.status = data.status === data.statusType
          ? data.status : (data.statusType + ' (' + data.status + ')')
        this.statusType = data.statusType
        this.conditionNote = data.conditionNote
        this.favorite = data.favorite
        if (this.statusType === 'Running' || this.statusType === 'Error') {
          this.events = (await api.notebook.getEventsById({id: data.id})).data
        }
        this.waitingTime = data.waitingTime
        this.executionTime = data.executionTime
      },
      emitDone () {
        this.$emit('done')
        this.dialogVisible = false
      },
      emitCancel () {
        this.$emit('cancel')
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      },
      emitFiles () {
        this.$emit('files', this.notebookId)
      },
      emitShell () {
        this.$emit('shell', this.notebookId)
      },
      emitLog () {
        this.$emit('log', this.notebookId)
      },
      async openNotebook () {
        let endpoint = await api.notebook.getEndpointById({id: this.notebookId})
        let notebookUrl = endpoint.data.url
        window.open(notebookUrl)
      },
      async updateHistory () {
        let putData = {
          name: this.name,
          memo: this.memo,
          favorite: this.favorite
        }
        await api.notebook.putById({id: this.notebookId, model: putData})
      },
      async onSubmit () {
        let form = this.$refs.updateForm

        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.updateHistory()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async haltNotebook () {
        try {
          await api.notebook.postHaltById({id: this.notebookId})
          await this.getDetail()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async deleteNotebook () {
        try {
          await api.notebook.deleteById({id: this.notebookId})
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .right-button-group {
    text-align: right;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .footer {
    padding-top: 40px;
  }

</style>
