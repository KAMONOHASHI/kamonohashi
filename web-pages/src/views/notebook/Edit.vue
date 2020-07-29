<template>
  <kqi-dialog
    :title="title"
    type="EDIT"
    @submit="onSubmit"
    @delete="deleteJob"
    @close="emitCancel"
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
            label="ID"
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
          <el-form-item label="ノートブック名" prop="name">
            <el-input v-model="form.name" />
          </el-form-item>
          <div v-if="detail.parents && detail.parents.length > 0">
            <el-form-item label="マウントした学習">
              <br />
              <div :class="{ scroll: detail.parents.length > 3 }">
                <div v-for="parent in detail.parents" :key="parent.id">
                  <el-popover
                    ref="parentDetail"
                    title="マウントした学習詳細"
                    trigger="hover"
                    width="350"
                    placement="right"
                  >
                    <kqi-training-history-details :training="parent" />
                    <el-button
                      v-if="$store.getters['account/isAvailableTraining']"
                      slot="reference"
                      class="el-input"
                      @click="showParent(parent.id)"
                    >
                      {{ parent.fullName }}
                    </el-button>
                    <el-button v-else slot="reference" class="el-input">
                      {{ parent.fullName }}
                    </el-button>
                  </el-popover>
                </div>
              </div>
            </el-form-item>
          </div>
          <div v-if="detail.inferences && detail.inferences.length > 0">
            <el-form-item label="マウントした推論">
              <br />
              <div :class="{ scroll: detail.inferences.length > 3 }">
                <div v-for="inference in detail.inferences" :key="inference.id">
                  <el-popover
                    ref="parentDetail"
                    title="マウントした推論詳細"
                    trigger="hover"
                    width="350"
                    placement="right"
                  >
                    <kqi-inference-history-details :inference="inference" />
                    <el-button
                      v-if="$store.getters['account/isAvailableInference']"
                      slot="reference"
                      class="el-input"
                      @click="showInference(inference.id)"
                    >
                      {{ inference.fullName }}
                    </el-button>
                    <el-button v-else slot="reference" class="el-input">
                      {{ inference.fullName }}
                    </el-button>
                  </el-popover>
                </div>
              </div>
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
                v-if="$store.getters['account/isAvailableDataSet']"
                v-popover:dataSetDetail
                class="el-input button"
                @click="redirectEditDataSet"
              >
                {{ detail.dataSet.name }}
              </el-button>
              <el-button v-else v-popover:dataSetDetail class="el-input">
                {{ detail.dataSet.name }}
              </el-button>
            </el-form-item>
            <el-form-item label="データセット作成方式">
              <div class="el-input">
                <span v-if="detail.localDataSet">ローカルコピー</span>
                <span v-else>シンボリックリンク</span>
              </div>
            </el-form-item>
          </div>
          <el-form-item label="モデル">
            <div class="el-input">
              <span
                v-if="detail.gitModel && detail.gitModel.url !== null"
                style="padding-left: 3px;"
              >
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

          <el-form-item label="起動時実行コマンド">
            <el-input
              v-model="detail.entryPoint"
              type="textarea"
              :autosize="{ minRows: 2 }"
              :readonly="true"
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

          <kqi-display-text-form label="作成者" :value="detail.createdBy" />
          <kqi-display-text-form label="開始日時" :value="detail.startedAt" />
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
          <div v-if="detail">
            <kqi-display-text-form
              v-if="detail.expiresIn !== 0"
              label="起動期間(h)"
              :value="String(detail.expiresIn / 60 / 60)"
            />
            <kqi-display-text-form v-else label="起動期間" value="無期限" />
          </div>
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
                <kqi-delete-button
                  button-label="ジョブ停止"
                  message="停止しますか"
                  @delete="haltNotebook"
                />
              </div>
              <div v-if="detail.status === 'Running'">
                <div class="el-input" style="padding: 10px 0;">
                  <el-button @click="emitShell">Shell起動</el-button>
                </div>
                <div>
                  <el-button
                    type="plain"
                    icon="el-icon-document"
                    @click="openNotebook"
                  >
                    ノートブックを開く
                  </el-button>
                </div>
              </div>
            </el-form-item>
          </div>
          <el-form-item label="コンテナ出力ファイル">
            <br />
            <el-button @click="emitFiles">ファイル一覧</el-button>
          </el-form-item>
          <el-form-item label="ログファイル">
            <br />
            <el-button size="mini" @click="emitLog">ログファイル閲覧</el-button>
          </el-form-item>
          <el-form-item label="メモ">
            <el-input
              v-model="form.memo"
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4 }"
            />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiDeleteButton from '@/components/KqiDeleteButton'
import KqiDataSetDetails from '@/components/selector/KqiDataSetDetails'
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails'
import KqiInferenceHistoryDetails from '@/components/selector/KqiInferenceHistoryDetails'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('notebook')
const kqiHost = process.env.VUE_APP_KAMONOHASHI_HOST || window.location.hostname

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiDeleteButton,
    KqiDataSetDetails,
    KqiTrainingHistoryDetails,
    KqiInferenceHistoryDetails,
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
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
      form: {
        name: null,
        favorite: false,
        memo: null,
      },
      title: '',
      error: null,
    }
  },
  computed: {
    ...mapGetters(['detail', 'events', 'endpoint']),
  },
  async created() {
    this.title = 'ノートブック履歴'
    await this.retrieveData()
    this.form.name = this.detail.name
    this.form.favorite = this.detail.favorite
    this.form.memo = this.detail.memo
  },
  methods: {
    ...mapActions([
      'fetchDetail',
      'fetchEvents',
      'fetchEndpoint',
      'postHalt',
      'put',
      'delete',
    ]),
    async retrieveData() {
      await this.fetchDetail(this.id)
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
            this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async haltNotebook() {
      try {
        await this.postHalt(this.detail.id)
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async deleteJob() {
      try {
        await this.delete(this.detail.id)
        this.$emit('done', 'delete')
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async openNotebook() {
      await this.fetchEndpoint(this.detail.id)
      window.open(
        `http://${kqiHost}:${this.endpoint.nodePort}${this.endpoint.token}`,
      )
    },

    // 親ジョブ履歴の表示
    showParent(parentId) {
      // 表示内容の変更は、beforeUpdated内で行う
      this.$router.push('/training/' + parentId)
    },
    showInference(inferenceId) {
      // 表示内容の変更は、beforeUpdated内で行う
      this.$router.push('/inference/' + inferenceId)
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
    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.footer {
  padding-top: 40px;
}

.favorite {
  font-size: 20px;
  color: rgb(230, 162, 60);
}
.scroll {
  height: 125px;
  overflow-y: scroll;
}
</style>
