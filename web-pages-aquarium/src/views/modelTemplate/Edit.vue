<template>
  <el-dialog
    class="dialog"
    title="新しいテンプレートの登録"
    :visible.sync="dialogVisible"
    :close-on-click-modal="false"
    @delete="deleteTemplate"
    @close="$emit('cancel')"
  >
    <el-row :gutter="20">
      <el-col :span="4">
        <div style="height: 400px;">
          <el-steps direction="vertical" :active="active">
            <el-step title="Step 1" description="基本設定" />
            <el-step title="Step 2" description="前処理" />
            <el-step title="Step 3" description="学習" />
            <el-step title="Step 4" description="推論" />
          </el-steps>
        </div>
      </el-col>
      <el-col :span="20">
        <el-form
          ref="createForm"
          :model="form"
          :rules="rules"
          element-loading-background="rgba(255, 255, 255, 0.7)"
        >
          <!-- step 1 -->
          <el-form
            v-show="active === 0"
            ref="form0"
            :model="form"
            :rules="rules"
          >
            <kqi-display-error :error="error" />
            <kqi-display-text-form v-if="isEditDialog" label="ID" :value="id" />

            <el-form-item label="テンプレート名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="説明文" prop="memo">
              <el-input v-model="form.memo" type="textarea" />
            </el-form-item>
            <el-form-item
              label="公開設定 "
              prop="accessLevel"
              style="display:block"
            >
              <br />
              <el-radio-group v-model="form.accessLevel">
                <el-radio :label="1" style="margin:10px">
                  現在のテナント
                </el-radio>
                <br />
                <el-radio :label="2" style="margin:10px">
                  全テナントに公開
                </el-radio>
              </el-radio-group>
            </el-form-item>
          </el-form>
          <!-- step 2 -->
          <preprocessing
            v-show="active === 1"
            ref="preprocessing"
            v-model="form.preprocForm"
            :create-template="true"
            :required-form="false"
            :form-type="'前処理'"
            @copy="copyAqContainer"
          />
          <!-- step 3 -->
          <training
            v-show="active === 2"
            ref="training"
            v-model="form.trainingForm"
            :create-template="true"
            :required-form="true"
            :form-type="'学習'"
            @copy="copyAqContainer"
          />
          <!-- step 4 : 推論 -->
          <evaluation
            v-show="active === 3"
            ref="evaluation"
            v-model="form.evaluationForm"
            :create-template="true"
            :required-form="false"
            :form-type="'推論'"
            @copy="copyAqContainer"
          />
        </el-form>
      </el-col>
    </el-row>
    <el-row class="step">
      <div>
        <span
          v-if="active >= 1"
          class="left-step-group"
          style="margin-top: 12px;"
          @click="previous"
        >
          <i class="el-icon-arrow-left" />
          Previous step
        </span>
        <span
          v-if="active <= 2"
          class="right-step-group"
          style="margin-top: 12px;"
          @click="next"
        >
          Next step
          <i class="el-icon-arrow-right" />
        </span>
        <span class="right-step-group">
          <el-button v-if="active === 3" type="primary" @click="submit">
            新規登録
          </el-button>
        </span>
      </div>
    </el-row>
  </el-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import AqContainerSettings from '@/components/AqContainerSettings.vue'
import { mapActions } from 'vuex'
import * as gen from '@/api/api.generate'
interface Form {
  containerImage: {
    registry:
      | string
      | null
      | {
          id: number
          name: string
        }
    registryId?: number
    image: string | null
    tag: string | null
    token: string | null
  }
  gitModel: {
    git: null | gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialOutputModel
    gitId?: number
    repository: null | gen.NssolPlatypusServiceModelsGitRepositoryModel | string
    owner?: string | null
    branch: null | gen.NssolPlatypusServiceModelsGitBranchModel | string
    commit: null | gen.NssolPlatypusServiceModelsGitCommitModel
    token: null | string
  }
  resource: {
    cpu: number
    memory: number
    gpu: number
  }
  entryPoint: string | null
}
interface DataType {
  active: number
  form: {
    name: string | null
    entryPoint: string | null
    memo: string | null
    accessLevel: number
    preprocForm: Form
    trainingForm: Form
    evaluationForm: Form
  }
  title: string
  dialogVisible: boolean
  error: null | Error
  isEditDialog: boolean
  rules: {
    name: Array<{ required: boolean; trigger: string; message: string }>
  }
}

export default Vue.extend({
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
    Preprocessing: AqContainerSettings,
    Training: AqContainerSettings,
    Evaluation: AqContainerSettings,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    return {
      active: 0,
      form: {
        name: null,
        entryPoint: null,
        memo: null,
        accessLevel: 1,
        preprocForm: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
            token: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
            token: null,
          },
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 0,
          },
          entryPoint: null,
        },
        trainingForm: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
            token: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
            token: null,
          },
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 0,
          },
          entryPoint: null,
        },
        evaluationForm: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
            token: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
            token: null,
          },
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 0,
          },
          entryPoint: null,
        },
      },
      title: '',
      dialogVisible: true,
      error: null,
      isEditDialog: false,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
    }
  },
  watch: {
    async $route() {
      // 通常の作成とコピー作成が同一コンポーネントのため、コピー作成の実行はrouterの変化により検知する
      await this.initialize()
    },
  },
  async created() {
    await this.initialize()
  },
  methods: {
    ...mapActions([
      'template/getByIdVersionsByVersionId',
      'template/getByIdVersions',
      'template/postByIdVersions',
      'template/post',
      'template/put',
    ]),
    copyAqContainer(info: { from: string; to: string }) {
      let from = info.from
      let to = info.to
      let fromData = null
      if (from == 'preprocessing') {
        fromData = this.form.preprocForm
      } else if (from == 'train') {
        fromData = this.form.trainingForm
      } else if (from == 'evaluation') {
        fromData = this.form.evaluationForm
      }
      if (to == 'preprocessing') {
        this.form.preprocForm = Object.assign(
          {},
          JSON.parse(JSON.stringify(fromData)),
        )
      } else if (to == 'train') {
        this.form.trainingForm = Object.assign(
          {},
          JSON.parse(JSON.stringify(fromData)),
        )
      } else if (to == 'evaluation') {
        this.form.evaluationForm = Object.assign(
          {},
          JSON.parse(JSON.stringify(fromData)),
        )
      }
    },
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[3] // ["", "preprocessing", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = '新しいテンプレートの登録'
          //this.isCopyCreation = this.id !== null//TODO 不要コード？
          this.isEditDialog = false
          break
      }
    },
    async next() {
      if (this.active < 3) {
        this.active += 1
      }
    },
    async previous() {
      if (this.active-- < 0) {
        this.active = 0
      }
    },
    async submit() {
      let submitTrainingForm = null
      let submitPreprocForm = null
      let submitEvalutionForm = null
      try {
        // 必須項目の入力チェック
        if (
          // テンプレート名
          this.form.name === null ||
          this.form.name === '' ||
          // 公開設定
          this.form.accessLevel === null
        ) {
          throw '必須項目が入力されていません : テンプレート名、公開設定は必須項目です。'
        }

        // 前処理、学習、評価のフォームの値を取得
        //@ts-ignore
        submitPreprocForm = await this.$refs.preprocessing.prepareSubmit()
        //@ts-ignore
        submitTrainingForm = await this.$refs.training.prepareSubmit()
        //@ts-ignore
        submitEvalutionForm = await this.$refs.evaluation.prepareSubmit()

        //基本設定情報
        let templateParams = {
          name: this.form.name,
          memo: this.form.memo,
          accessLevel: this.form.accessLevel,
        }

        let versionParams = {
          //前処理
          preprocessContainerImage: submitPreprocForm.containerImage,
          preprocessGitModel: submitPreprocForm.gitModel,
          preprocessEntryPoint: submitPreprocForm.entryPoint,
          preprocessCpu: submitPreprocForm.cpu,
          preprocessMemory: submitPreprocForm.memory,
          preprocessGpu: submitPreprocForm.gpu,
          // 学習
          trainingContainerImage: submitTrainingForm.containerImage,
          trainingGitModel: submitTrainingForm.gitModel,
          trainingEntryPoint: submitTrainingForm.entryPoint,
          trainingCpu: submitTrainingForm.cpu,
          trainingMemory: submitTrainingForm.memory,
          trainingGpu: submitTrainingForm.gpu,
          // 推論
          evaluationContainerImage: submitEvalutionForm.containerImage,
          evaluationGitModel: submitEvalutionForm.gitModel,
          evaluationEntryPoint: submitEvalutionForm.entryPoint,
          evaluationCpu: submitEvalutionForm.cpu,
          evaluationMemory: submitEvalutionForm.memory,
          evaluationGpu: submitEvalutionForm.gpu,
        }

        //新規テンプレート登録後、テンプレートバージョンの登録をする
        let ret = await this['template/post'](templateParams)
        let params = { id: ret.data.id, body: versionParams }
        await this['template/postByIdVersions'](params)

        this.$emit('done')
        this.error = null
        //dialogを閉じる
        this.dialogVisible = false
      } catch (message) {
        //@ts-ignore
        this.$notify.error({
          message: message,
        })
        return
      }
    },

    async deleteTemplate() {
      try {
        //@ts-ignore
        await this['modelTemplate/delete'](this.id)
        this.$emit('done', 'delete')
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.left-step-group {
  text-align: left;
  float: left;
  z-index: 2;
}
.right-step-group {
  text-align: right;
  float: right;
  z-index: 2;
}
</style>
