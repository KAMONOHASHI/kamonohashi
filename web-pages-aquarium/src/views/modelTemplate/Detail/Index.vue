<template>
  <div>
    <el-row>
      <el-col :span="12"
        ><h2>テンプレート詳細＞ {{ detail.name }}</h2></el-col
      >
      <el-col :span="12" style="padding-top:15px">
        <el-select
          v-model="versionValue"
          placeholder="Select"
          @change="currentChange"
        >
          <el-option
            v-for="item in versions"
            :key="item.value"
            :label="item.version"
            :value="item.id"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>
    <el-tabs v-model="activeName">
      <el-tab-pane label="基本設定" name="baseSetting">
        <base-setting v-if="baseForm" v-model="baseForm" />
      </el-tab-pane>
      <el-tab-pane label="前処理" name="preprocessing">
        <preprocessing
          v-if="preprocForm"
          ref="preprocessing"
          v-model="preprocForm"
          :create-teplate="false"
          :required-form="false"
          :form-type="'前処理'"
        />
      </el-tab-pane>
      <el-tab-pane label="学習" name="train">
        <training
          v-if="trainingForm"
          ref="training"
          v-model="trainingForm"
          :create-teplate="false"
          :required-form="true"
          :form-type="'学習'"
        />
      </el-tab-pane>
      <el-tab-pane label="推論" name="evaluation">
        <evaluation
          v-if="evaluationForm"
          ref="evaluation"
          v-model="evaluationForm"
          :create-teplate="false"
          :required-form="false"
          :form-type="'推論'"
        />
      </el-tab-pane>
    </el-tabs>
    <el-button type="primary" plain @click="submit()">
      更新
    </el-button>
    <el-button plain @click="deleteVersionDialog = true">
      テンプレートバージョン削除
    </el-button>
    <el-button plain @click="deleteDialog = true">
      テンプレート削除
    </el-button>
    <el-dialog title="" :visible.sync="deleteVersionDialog" width="30%">
      <span v-if="versions.length == 1">
        このテンプレートにはバージョン"{{
          versionDetail.version
        }}"しか存在しないため<br />
        テンプレートバージョン削除することができません。<br />
        <br />
        削除をしたい場合はテンプレート削除を選択してください。
      </span>
      <span v-else>
        <span>
          テンプレートバージョン"{{ versionDetail.version }}"を削除しますか？
        </span>
      </span>
      <span v-if="versions.length == 1" slot="footer" class="dialog-footer">
        <el-button @click="deleteVersionDialog = false">OK</el-button>
      </span>
      <span v-else slot="footer" class="dialog-footer">
        <el-button @click="deleteVersionDialog = false">Cancel</el-button>
        <el-button type="primary" @click="deleteTemplateVersion()">
          削除
        </el-button>
      </span>
    </el-dialog>
    <el-dialog title="" :visible.sync="deleteDialog" width="30%">
      <span>
        テンプレートを削除すると全てのテンプレートバージョンが失われます。<br />
        テンプレートを削除しますか？
      </span>

      <span slot="footer" class="dialog-footer">
        <el-button @click="deleteDialog = false">Cancel</el-button>
        <el-button type="primary" @click="deleteTemplate()">
          削除
        </el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import BaseSetting from './BaseSetting'
import AqContainerSettings from '@/components/AqContainerSettings'

const { mapGetters, mapActions } = createNamespacedHelpers('template')

export default {
  title: 'モデルテンプレート',
  components: {
    BaseSetting,
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
  data() {
    return {
      deleteDialog: false,
      deleteVersionDialog: false,
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'type', name: '種類', type: 'text' },
        { prop: 'totalImageNumber', name: 'イメージの総数', type: 'text' },
        {
          prop: 'labeledImageNumber',
          name: 'ラベル付きのイメージ数',
          type: 'text',
        },
        { prop: 'lastModified', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
      ],
      tableData: [],
      activeName: 'baseSetting',
      preprocForm: null,
      trainingForm: null,
      evaluationForm: null,
      baseForm: null,
      versionValue: null,
      changeFlg: false,
    }
  },
  computed: {
    ...mapGetters(['detail', 'versionDetail', 'total', 'versions']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'fetchModelTemplate',
      'fetchVersions',
      'fetchDetail',
      'postByIdVersions',
      'post',
      'put',
      'delete',
      'deleteVersion',
    ]),

    async deleteTemplate() {
      //モデルテンプレート削除
      this.deleteDialog = false
      await this.delete(this.id)
      this.$router.push('/aquarium/model-template')
    },

    async deleteTemplateVersion() {
      //モデルテンプレートバージョン削除

      await this.deleteVersion({ id: this.id, versionId: this.versionValue })
      this.deleteVersionDialog = false
      this.versionValue = null
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
      this.retrieveData()

      //再描画
      this.$forceUpdate()
      await this.$notify.success({
        type: 'Success',
        message: `バージョンを削除しました。`,
      })
    },
    async retrieveData() {
      await this.fetchModelTemplate(this.id)
      await this.fetchVersions(this.id)
      if (this.versionValue == null) {
        for (let i in this.versions) {
          // 最新版を保持させる
          if (this.versions[i].version == this.detail.latestVersion) {
            this.versionValue = this.versions[i].id
          }
        }
      }

      this.baseForm = {
        name: this.detail.name,
        memo: this.detail.memo,
        accessLevel: this.detail.accessLevel,
        assignedTenants: this.detail.assignedTenants,
      }
      //最新バージョンを取得する
      await this.fetchDetail({
        id: this.id,
        versionId: this.versionValue,
      })

      this.preprocForm = {
        name: 'preprocForm',
        containerImage: { ...this.versionDetail.preprocessContainerImage },
        gitModel: { ...this.versionDetail.preprocessGitModel },
        entryPoint: this.versionDetail.preprocessEntryPoint,
        resource: {
          cpu: this.versionDetail.preprocessCpu,
          memory: this.versionDetail.preprocessMemory,
          gpu: this.versionDetail.preprocessGpu,
        },
      }

      this.trainingForm = {
        name: 'trainingForm',
        containerImage: { ...this.versionDetail.trainingContainerImage },
        gitModel: { ...this.versionDetail.trainingGitModel },
        entryPoint: this.versionDetail.trainingEntryPoint,
        resource: {
          cpu: this.versionDetail.trainingCpu,
          memory: this.versionDetail.trainingMemory,
          gpu: this.versionDetail.trainingGpu,
        },
      }

      this.evaluationForm = {
        name: 'evaluationForms',
        containerImage: { ...this.versionDetail.evaluationContainerImage },
        gitModel: { ...this.versionDetail.evaluationGitModel },
        entryPoint: this.versionDetail.evaluationEntryPoint,
        resource: {
          cpu: this.versionDetail.evaluationCpu,
          memory: this.versionDetail.evaluationMemory,
          gpu: this.versionDetail.evaluationGpu,
        },
      }
      this.changeFlg = true
    },

    async currentChange() {
      if (this.changeFlg == false) {
        return
      }
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
      await this.retrieveData()
    },

    async updateBase() {
      const baseParam = {
        name: this.baseForm.name,
        memo: this.baseForm.memo,
        accessLevel: this.baseForm.accessLevel,
      }
      if (
        this.detail.name != baseParam.name ||
        this.detail.memo != baseParam.memo ||
        this.detail.accessLevel != baseParam.accessLevel
      ) {
        //基本情報更新
        await this['put']({ id: this.id, model: baseParam })
        return true
      } else {
        return false
      }
    },

    async submit() {
      // 入力値チェック
      let submitTrainingForm = null
      let submitPreprocForm = null
      let submitEvaluationForm = null
      try {
        // 必須項目の入力チェック
        if (
          // テンプレート名
          this.baseForm.name === null ||
          // 公開設定
          this.baseForm.accessLevel === null
        ) {
          throw '必須項目が入力されていません : テンプレート名、公開設定は必須項目です。'
        }
        // 前処理、学習、評価のフォームの値を取得
        submitTrainingForm = await this.$refs.training.prepareSubmit()
        submitPreprocForm = await this.$refs.preprocessing.prepareSubmit()
        submitEvaluationForm = await this.$refs.evaluation.prepareSubmit()
      } catch (message) {
        this.$notify.error({
          message: message,
        })
        return
      }

      //基本情報更新
      let updatedBase = await this.updateBase()

      //バージョン情報が変更されていれば更新するフラグ
      let update = false

      // 前処理の比較
      if (
        this.versionDetail.preprocessContainerImage === null &&
        submitPreprocForm.containerImage !== null
      ) {
        // 前処理なし -> 前処理ありへの変更
        update = true
      }
      if (this.versionDetail.preprocessContainerImage !== null) {
        // 前処理あり -> 前処理なしへの変更 または 前処理情報の変更
        if (
          submitPreprocForm.containerImage === null ||
          this.versionDetail.preprocessContainerImage.registryId !=
            submitPreprocForm.containerImage.registryId ||
          this.versionDetail.preprocessContainerImage.image !=
            submitPreprocForm.containerImage.image ||
          this.versionDetail.preprocessContainerImage.tag !=
            submitPreprocForm.containerImage.tag ||
          this.versionDetail.preprocessContainerImage.token !=
            submitPreprocForm.containerImage.token ||
          this.versionDetail.preprocessGitModel.gitId !=
            submitPreprocForm.gitModel.gitId ||
          this.versionDetail.preprocessGitModel.repository !=
            submitPreprocForm.gitModel.repository ||
          this.versionDetail.preprocessGitModel.branch !=
            submitPreprocForm.gitModel.branch ||
          this.versionDetail.preprocessGitModel.commitId !=
            submitPreprocForm.gitModel.commitId ||
          this.versionDetail.preprocessGitModel.token !=
            submitPreprocForm.gitModel.token ||
          this.versionDetail.preprocessEntryPoint !=
            submitPreprocForm.entryPoint ||
          this.versionDetail.preprocessCpu != submitPreprocForm.cpu ||
          this.versionDetail.preprocessMemory != submitPreprocForm.memory ||
          this.versionDetail.preprocessGpu != submitPreprocForm.gpu
        ) {
          update = true
        }
      }

      // 学習の比較
      if (
        this.versionDetail.trainingContainerImage === null &&
        submitTrainingForm.containerImage !== null
      ) {
        // 前処理なし -> 前処理ありへの変更
        update = true
      }
      if (this.versionDetail.trainingContainerImage !== null) {
        // 前処理あり -> 前処理なしへの変更 または 前処理情報の変更
        if (
          submitTrainingForm.containerImage === null ||
          this.versionDetail.trainingContainerImage.registryId !=
            submitTrainingForm.containerImage.registryId ||
          this.versionDetail.trainingContainerImage.image !=
            submitTrainingForm.containerImage.image ||
          this.versionDetail.trainingContainerImage.tag !=
            submitTrainingForm.containerImage.tag ||
          this.versionDetail.trainingContainerImage.token !=
            submitTrainingForm.containerImage.token ||
          this.versionDetail.trainingGitModel.gitId !=
            submitTrainingForm.gitModel.gitId ||
          this.versionDetail.trainingGitModel.repository !=
            submitTrainingForm.gitModel.repository ||
          this.versionDetail.trainingGitModel.branch !=
            submitTrainingForm.gitModel.branch ||
          this.versionDetail.trainingGitModel.commitId !=
            submitTrainingForm.gitModel.commitId ||
          this.versionDetail.trainingGitModel.token !=
            submitTrainingForm.gitModel.token ||
          this.versionDetail.trainingEntryPoint !=
            submitTrainingForm.entryPoint ||
          this.versionDetail.trainingCpu != submitTrainingForm.cpu ||
          this.versionDetail.trainingMemory != submitTrainingForm.memory ||
          this.versionDetail.trainingGpu != submitTrainingForm.gpu
        ) {
          update = true
        }
      }

      // 推論の比較
      if (
        this.versionDetail.evaluationContainerImage === null &&
        submitEvaluationForm.containerImage !== null
      ) {
        // 前処理なし -> 前処理ありへの変更
        update = true
      }
      if (this.versionDetail.evaluationContainerImage !== null) {
        // 前処理あり -> 前処理なしへの変更 または 前処理情報の変更
        if (
          submitEvaluationForm.containerImage === null ||
          this.versionDetail.evaluationContainerImage.registryId !=
            submitEvaluationForm.containerImage.registryId ||
          this.versionDetail.evaluationContainerImage.image !=
            submitEvaluationForm.containerImage.image ||
          this.versionDetail.evaluationContainerImage.tag !=
            submitEvaluationForm.containerImage.tag ||
          this.versionDetail.evaluationContainerImage.token !=
            submitEvaluationForm.containerImage.token ||
          this.versionDetail.evaluationGitModel.gitId !=
            submitEvaluationForm.gitModel.gitId ||
          this.versionDetail.evaluationGitModel.repository !=
            submitEvaluationForm.gitModel.repository ||
          this.versionDetail.evaluationGitModel.branch !=
            submitEvaluationForm.gitModel.branch ||
          this.versionDetail.evaluationGitModel.commitId !=
            submitEvaluationForm.gitModel.commitId ||
          this.versionDetail.evaluationGitModel.token !=
            submitEvaluationForm.gitModel.token ||
          this.versionDetail.evaluationEntryPoint !=
            submitEvaluationForm.entryPoint ||
          this.versionDetail.evaluationCpu != submitEvaluationForm.cpu ||
          this.versionDetail.evaluationMemory != submitEvaluationForm.memory ||
          this.versionDetail.evaluationGpu != submitEvaluationForm.gpu
        ) {
          update = true
        }
      }

      if (update == false) {
        if (updatedBase) {
          await this.$notify.success({
            type: 'Success',
            message: `更新しました`,
          })
        } else {
          await this.$notify.error({
            message: '変更点がありません',
          })
        }
        return
      }

      const params = {
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
        evaluationContainerImage: submitEvaluationForm.containerImage,
        evaluationGitModel: submitEvaluationForm.gitModel,
        evaluationEntryPoint: submitEvaluationForm.entryPoint,
        evaluationCpu: submitEvaluationForm.cpu,
        evaluationMemory: submitEvaluationForm.memory,
        evaluationGpu: submitEvaluationForm.gpu,
      }

      //新規バージョン作成
      await this['postByIdVersions']({ id: this.id, model: params })

      await this.$notify.success({
        type: 'Success',
        message: `更新しました`,
      })

      this.changeFlg = false
      this.versionValue = null
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
