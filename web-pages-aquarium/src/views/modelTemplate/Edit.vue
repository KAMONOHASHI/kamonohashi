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
            <el-step title="Step 1" description="基本設定"></el-step>
            <el-step title="Step 2" description="前処理"></el-step>
            <el-step title="Step 3" description="学習"></el-step>
            <el-step title="Step 4" description="評価"></el-step>
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
          <el-form v-if="active === 0" ref="form0" :model="form" :rules="rules">
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
              ><br />
              <el-radio-group v-model="form.accessLevel">
                <el-radio :label="1" style="margin:10px"
                  >現在のテナント </el-radio
                ><br />
                <el-radio :label="2" style="margin:10px"
                  >全テナントに公開</el-radio
                >
              </el-radio-group>
            </el-form-item>
          </el-form>
          <!-- step 2 -->
          <el-form v-if="active === 1" ref="form1" :model="form" :rules="rules">
            <el-row :gutter="20">
              <el-col :span="12">
                <kqi-container-selector
                  v-model="form.preprocess.containerImage"
                  :disabled="isPatch"
                  :registries="registries"
                  :images="images"
                  :tags="tags"
                  heading="前処理実行コンテナイメージ"
                  @selectRegistry="selectPreprocessRegistry"
                  @selectImage="selectPreprocessImage"
                />

                <kqi-git-selector
                  v-model="form.preprocess.gitModel"
                  :disabled="isPatch"
                  :gits="gits"
                  :repositories="repositories"
                  :branches="branches"
                  :commits="commits"
                  :loading-repositories="loadingRepositories"
                  heading="スクリプト"
                  @selectGit="selectPreprocessGit"
                  @selectRepository="selectPreprocessRepository"
                  @selectBranch="selectPreprocessBranch"
                />
                <el-form-item label="実行コマンド" prop="entryPoint">
                  <el-input
                    v-model="form.preprocess.entryPoint"
                    type="textarea"
                    :autosize="{ minRows: 2 }"
                    :disabled="isPatch"
                  />
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <kqi-resource-selector
                  v-model="form.preprocess.resource"
                  :quota="quota"
                />
              </el-col>
            </el-row>
          </el-form>
          <!-- step 3 -->
          <el-form v-if="active === 2" ref="form2" :model="form" :rules="rules">
            <el-row :gutter="20">
              <el-col :span="12">
                <kqi-container-selector
                  v-model="form.training.containerImage"
                  :disabled="isPatch"
                  :registries="registries"
                  :images="images"
                  :tags="tags"
                  heading="学習・推論コンテナイメージ"
                  @selectRegistry="selectTrainingRegistry"
                  @selectImage="selectTrainingImage"
                />
                <kqi-git-selector
                  v-model="form.training.gitModel"
                  :disabled="isPatch"
                  :gits="gits"
                  :repositories="repositories"
                  :branches="branches"
                  :commits="commits"
                  :loading-repositories="loadingRepositories"
                  heading="スクリプト"
                  @selectGit="selectTrainingGit"
                  @selectRepository="selectTrainingRepository"
                  @selectBranch="selectTrainingBranch"
                />
                <el-form-item label="実行コマンド" prop="entryPoint">
                  <el-input
                    v-model="form.training.entryPoint"
                    type="textarea"
                    :autosize="{ minRows: 2 }"
                    :disabled="isPatch"
                  />
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <kqi-resource-selector
                  v-model="form.training.resource"
                  :quota="quota"
                />
              </el-col>
            </el-row>
          </el-form>
          <!-- step 4 -->
          <el-form v-if="active === 3" ref="form3" :model="form" :rules="rules">
            <el-row :gutter="20">
              <el-col :span="12">
                <kqi-container-selector
                  v-model="form.evaluation.containerImage"
                  :disabled="isPatch"
                  :registries="registries"
                  :images="images"
                  :tags="tags"
                  heading="評価コンテナイメージ"
                  @selectRegistry="selectEvaluationRegistry"
                  @selectImage="selectEvaluationImage"
                />
                <kqi-git-selector
                  v-model="form.evaluation.gitModel"
                  :disabled="isPatch"
                  :gits="gits"
                  :repositories="repositories"
                  :branches="branches"
                  :commits="commits"
                  :loading-repositories="loadingRepositories"
                  heading="スクリプト"
                  @selectGit="selectEvaluationGit"
                  @selectRepository="selectEvaluationRepository"
                  @selectBranch="selectEvaluationBranch"
                />
                <el-form-item label="実行コマンド" prop="entryPoint">
                  <el-input
                    v-model="form.evaluation.entryPoint"
                    type="textarea"
                    :autosize="{ minRows: 2 }"
                    :disabled="isPatch"
                  />
                </el-form-item>
              </el-col>

              <el-col :span="12">
                <kqi-resource-selector
                  v-model="form.evaluation.resource"
                  :quota="quota"
                />
              </el-col>
            </el-row>
          </el-form>
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

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiContainerSelector from '@/components/selector/KqiContainerSelector'
import KqiGitSelector from '@/components/selector/KqiGitSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import registrySelectorUtil from '@/util/registrySelectorUtil'
import gitSelectorUtil from '@/util/gitSelectorUtil'
import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiContainerSelector,
    KqiGitSelector,
    KqiResourceSelector,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      active: 0,
      form: {
        name: null,
        entryPoint: null,
        memo: null,
        accessLevel: 1,
        preprocess: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
          },
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 0,
          },
          entryPoint: null,
        },
        training: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
          },
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 0,
          },
          entryPoint: null,
        },
        evaluation: {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
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
      isCreateDialog: false,
      isEditDialog: false,
      isPatch: false, // 利用済み前処理の場合name, memo, resourceのみ更新可能

      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
    }
  },
  computed: {
    ...mapGetters({
      registries: ['registrySelector/registries'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
      images: ['registrySelector/images'],
      tags: ['registrySelector/tags'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      repositories: ['gitSelector/repositories'],
      branches: ['gitSelector/branches'],
      commits: ['gitSelector/commits'],
      commitDetail: ['gitSelector/commitDetail'],
      loadingRepositories: ['gitSelector/loadingRepositories'],
      detail: ['template/detail'],
      quota: ['cluster/quota'],
    }),
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
      'template/getByIdVersionsByVersionId2',
      'template/getByIdVersions2',
      'template/postByIdVersions2',
      'template/post2',
      'template/put2',

      'template/fetchDetail',
      'template/post',
      'template/put',
      'template/delete',
      'registrySelector/fetchRegistries',
      'registrySelector/fetchImages',
      'registrySelector/fetchTags',
      'gitSelector/fetchGits',
      'gitSelector/fetchRepositories',
      'gitSelector/fetchBranches',
      'gitSelector/fetchCommits',
      'gitSelector/fetchCommitDetail',
      'cluster/fetchQuota',
    ]),
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[3] // ["", "preprocessing", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = '新しいテンプレートの登録'
          this.isCreateDialog = true
          this.isCopyCreation = this.id !== null
          this.isEditDialog = false
          break
        case 'edit':
          this.title = 'テンプレートの詳細'
          this.isCreateDialog = false
          this.isCopyCreation = false
          this.isEditDialog = true
          break
      }

      // クォータ情報を取得
      await this['cluster/fetchQuota']()

      // 指定に必要な情報を取得
      // レジストリ一覧を取得し、デフォルトレジストリを設定
      await this['registrySelector/fetchRegistries']()
      this.form.preprocess.containerImage.registry = this.registries.find(
        registry => {
          return registry.id === this.defaultRegistryId
        },
      )
      this.form.training.containerImage.registry = this.registries.find(
        registry => {
          return registry.id === this.defaultRegistryId
        },
      )

      await this.selectPreprocessRegistry(this.defaultRegistryId) //TODO defaultRegistryId？
      await this.selectTrainingRegistry(this.defaultRegistryId) //TODO defaultRegistryId？

      // gitサーバ一覧を取得し、デフォルトgitサーバを設定
      await this['gitSelector/fetchGits']()
      this.form.preprocess.gitModel.git = this.gits.find(git => {
        return git.id === this.defaultGitId
      })
      this.form.training.gitModel.git = this.gits.find(git => {
        return git.id === this.defaultGitId
      })
      await this['gitSelector/fetchRepositories'](this.defaultGitId)
    },
    async next() {
      let form = null
      switch (this.active) {
        case 0:
          form = this.$refs.form0
          break
        case 1:
          form = this.$refs.form1
          break
        case 2:
          form = this.$refs.form2
          break
      }
      await form.validate(async valid => {
        if (valid) {
          this.active++
        }
      })
    },
    previous() {
      if (this.active-- < 0) {
        this.active = 0
      }
    },
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            if (this.isPatch) {
              // 名称・メモ・リソースのみ更新
              await this.patchTemplates()
            } else {
              // コンテナイメージの指定
              // イメージとタグが指定されている場合、コンテナイメージを指定して登録
              // イメージとタグが指定されていない場合、コンテナイメージは未指定(null)として登録
              ////前処理
              let preprocessContainerImage = null
              if (
                this.form.preprocess.containerImage.image !== null &&
                this.form.preprocess.containerImage.tag !== null
              ) {
                preprocessContainerImage = {
                  registryId: this.form.preprocess.containerImage.registry.id,
                  image: this.form.preprocess.containerImage.image,
                  tag: this.form.preprocess.containerImage.tag,
                }
              }
              ////学習
              let trainingContainerImage = null
              if (
                this.form.training.containerImage.image !== null &&
                this.form.training.containerImage.tag !== null
              ) {
                trainingContainerImage = {
                  registryId: this.form.training.containerImage.registry.id,
                  image: this.form.training.containerImage.image,
                  tag: this.form.training.containerImage.tag,
                }
              }

              ////評価
              let evaluationContainerImage = null
              if (
                this.form.evaluation.containerImage.image !== null &&
                this.form.evaluation.containerImage.tag !== null
              ) {
                evaluationContainerImage = {
                  registryId: this.form.evaluation.containerImage.registry.id,
                  image: this.form.evaluation.containerImage.image,
                  tag: this.form.evaluation.containerImage.tag,
                }
              }
              // gitモデルの指定
              // リポジトリとブランチが指定されている場合、gitモデルを指定して登録
              // リポジトリとブランチが指定されていない場合、gitモデルは未指定(null)として登録
              ////前処理
              let preprocessGitModel = null
              if (
                this.form.preprocess.gitModel.repository !== null &&
                this.form.preprocess.gitModel.branch !== null
              ) {
                // HEAD指定の時はcommitsの先頭要素をcommitIDに指定する。コピー実行時の再現性を担保するため
                preprocessGitModel = {
                  gitId: this.form.preprocess.gitModel.git.id,
                  repository: this.form.preprocess.gitModel.repository.name,
                  owner: this.form.preprocess.gitModel.repository.owner,
                  branch: this.form.preprocess.gitModel.branch.branchName,
                  commitId: this.form.preprocess.gitModel.commit
                    ? this.form.preprocess.gitModel.commit.commitId
                    : this.commits[0].commitId,
                }
              }
              ////学習
              let trainingGitModel = null
              if (
                this.form.training.gitModel.repository !== null &&
                this.form.training.gitModel.branch !== null
              ) {
                // HEAD指定の時はcommitsの先頭要素をcommitIDに指定する。コピー実行時の再現性を担保するため
                trainingGitModel = {
                  gitId: this.form.training.gitModel.git.id,
                  repository: this.form.training.gitModel.repository.name,
                  owner: this.form.training.gitModel.repository.owner,
                  branch: this.form.training.gitModel.branch.branchName,
                  commitId: this.form.training.gitModel.commit
                    ? this.form.training.gitModel.commit.commitId
                    : this.commits[0].commitId,
                }
              }
              ////評価
              let evaluationGitModel = null
              if (
                this.form.evaluation.gitModel.repository !== null &&
                this.form.evaluation.gitModel.branch !== null
              ) {
                // HEAD指定の時はcommitsの先頭要素をcommitIDに指定する。コピー実行時の再現性を担保するため
                evaluationGitModel = {
                  gitId: this.form.evaluation.gitModel.git.id,
                  repository: this.form.evaluation.gitModel.repository.name,
                  owner: this.form.evaluation.gitModel.repository.owner,
                  branch: this.form.evaluation.gitModel.branch.branchName,
                  commitId: this.form.evaluation.gitModel.commit
                    ? this.form.evaluation.gitModel.commit.commitId
                    : this.commits[0].commitId,
                }
              }

              //基本設定情報
              let templateParams = {
                name: this.form.name,
                memo: this.form.memo,
                accessLevel: this.form.accessLevel,
              }

              //バージョン管理情報
              let versionParams = {
                preprocessEntryPoint: this.form.preprocess.entryPoint,
                preprocessContainerImage: preprocessContainerImage,
                preprocessGitModel: preprocessGitModel,
                preprocessCpu: this.form.preprocess.resource.cpu,
                preprocessMemory: this.form.preprocess.resource.memory,
                preprocessGpu: this.form.preprocess.resource.gpu,

                trainingEntryPoint: this.form.training.entryPoint,
                trainingContainerImage: trainingContainerImage,
                trainingGitModel: trainingGitModel,
                trainingCpu: this.form.training.resource.cpu,
                trainingMemory: this.form.training.resource.memory,
                trainingGpu: this.form.training.resource.gpu,

                evaluationEntryPoint: this.form.evaluation.entryPoint,
                evaluationContainerImage: evaluationContainerImage,
                evaluationGitModel: evaluationGitModel,
                evaluationCpu: this.form.evaluation.resource.cpu,
                evaluationMemory: this.form.evaluation.resource.memory,
                evaluationGpu: this.form.evaluation.resource.gpu,
              }
              //
              if (this.isCreateDialog) {
                // 新規作成
                //await this['template/post'](templateParams)
               
                //新規テンプレート登録後、テンプレートバージョンの登録をする
                let ret = await this['template/post2'](templateParams)
                let params = { id: ret.data.id, model: versionParams }
                await this['template/postByIdVersions2'](params)
              } else {
                // 編集
              }
            }

            this.$emit('done')
            this.error = null
            //dialogを閉じる
            this.dialogVisible = false
          } catch (e) {
            this.error = e
            //dialogを閉じる
            this.dialogVisible = false
          }
        }
      })
    },

    async patchTemplates() {
      let params = {
        name: this.form.name,
        memo: this.form.memo,
        cpu: this.form.resource.cpu,
        memory: this.form.resource.memory,
        gpu: this.form.resource.gpu,
      }
      await this['modelTemplate/patch']({
        id: this.id,
        params: params,
      })
    },
    async deleteTemplate() {
      try {
        await this['modelTemplate/delete'](this.id)
        this.$emit('done', 'delete')
      } catch (e) {
        this.error = e
      }
    },
    // コンテナイメージ
    async selectPreprocessRegistry(registryId) {
      await registrySelectorUtil.selectRegistry(
        this.form.preprocess,
        this['registrySelector/fetchImages'],
        registryId,
      )
    },
    async selectTrainingRegistry(registryId) {
      await registrySelectorUtil.selectRegistry(
        this.form.training,
        this['registrySelector/fetchImages'],
        registryId,
      )
    },
    async selectEvaluationRegistry(registryId) {
      await registrySelectorUtil.selectRegistry(
        this.form.evaluation,
        this['registrySelector/fetchImages'],
        registryId,
      )
    },
    async selectPreprocessImage(image) {
      await registrySelectorUtil.selectImage(
        this.form.form.preprocess,
        this['registrySelector/fetchTags'],
        this.form.preprocess.containerImage.registry.id,
        image,
      )
      this.form
    },
    async selectTrainingImage(image) {
      await registrySelectorUtil.selectImage(
        this.form.training,
        this['registrySelector/fetchTags'],
        this.form.training.containerImage.registry.id,
        image,
      )
    },
    async selectEvaluationImage(image) {
      await registrySelectorUtil.selectImage(
        this.form.evaluation,
        this['registrySelector/fetchTags'],
        this.form.evaluation.containerImage.registry.id,
        image,
      )
    },
    // モデル
    async selectPreprocessGit(gitId) {
      await gitSelectorUtil.selectGit(
        this.form.preprocess,
        this['gitSelector/fetchRepositories'],
        gitId,
        this.$store,
      )
    },
    async selectTrainingGit(gitId) {
      await gitSelectorUtil.selectGit(
        this.form.training,
        this['gitSelector/fetchRepositories'],
        gitId,
        this.$store,
      )
    },
    async selectEvaluationGit(gitId) {
      await gitSelectorUtil.selectGit(
        this.form.evaluation,
        this['gitSelector/fetchRepositories'],
        gitId,
        this.$store,
      )
    },
    // repositoryの型がstring：手入力, object: 選択
    async selectPreprocessRepository(repository) {
      try {
        await gitSelectorUtil.selectRepository(
          this.form.preprocess,
          this['gitSelector/fetchBranches'],
          repository,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }
    },
    async selectTrainingRepository(repository) {
      try {
        await gitSelectorUtil.selectRepository(
          this.form.training,
          this['gitSelector/fetchBranches'],
          repository,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }
    },
    async selectEvaluationRepository(repository) {
      try {
        await gitSelectorUtil.selectRepository(
          this.form.evaluation,
          this['gitSelector/fetchBranches'],
          repository,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }
    },
    async selectPreprocessBranch(branchName) {
      await gitSelectorUtil.selectBranch(
        this.form.preprocess,
        this['gitSelector/fetchCommits'],
        branchName,
      )
    },
    async selectTrainingBranch(branchName) {
      await gitSelectorUtil.selectBranch(
        this.form.training,
        this['gitSelector/fetchCommits'],
        branchName,
      )
    },
    async selectEvaluationBranch(branchName) {
      await gitSelectorUtil.selectBranch(
        this.form.evaluation,
        this['gitSelector/fetchCommits'],
        branchName,
      )
    },
  },
}
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
