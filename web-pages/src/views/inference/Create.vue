<template>
  <el-dialog
    class="dialog"
    title="推論実行"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <kqi-display-error :error="error" />
    <div v-if="isCopyCreation">
      <el-form ref="runForm" :rules="rules" :model="form">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="推論名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <kqi-training-history-selector
              v-model="form.selectedParent"
              :histories="trainingHistories"
              multiple
            />
            <kqi-inference-history-selector
              v-model="form.selectedParentInference"
              :histories="inferenceHistories"
              multiple
            />
            <kqi-data-set-selector
              v-model="form.dataSetId"
              :data-sets="dataSets"
            />
            <el-form-item label="データセット作成方式">
              <el-switch
                v-model="form.localDataSet"
                style="width: 100%;"
                inactive-text="シンボリックリンク"
                active-text="ローカルコピー"
              />
            </el-form-item>

            <el-form-item label="実行コマンド" prop="entryPoint">
              <el-input
                v-model="form.entryPoint"
                type="textarea"
                :autosize="{ minRows: 10 }"
              />
            </el-form-item>
            <kqi-container-selector
              v-model="form.containerImage"
              :registries="registries"
              :images="images"
              :tags="tags"
              @selectRegistry="selectRegistry"
              @selectImage="selectImage"
            />
            <kqi-git-selector
              v-model="form.gitModel"
              :gits="gits"
              :repositories="repositories"
              :branches="branches"
              :commits="commits"
              :loading-repositories="loadingRepositories"
              @selectGit="selectGit"
              @selectRepository="selectRepository"
              @selectBranch="selectBranch"
            />
          </el-col>
          <el-col :span="12">
            <kqi-resource-selector v-model="form.resource" :quota="quota" />

            <kqi-environment-variables v-model="form.variables" />
            <el-form-item label="結果Zip圧縮">
              <el-switch
                v-model="form.zip"
                style="width: 100%;"
                inactive-text="圧縮しない"
                active-text="圧縮する"
              />
            </el-form-item>
            <kqi-partition-selector
              v-model="form.partition"
              :partitions="partitions"
            />
            <el-form-item label="メモ">
              <el-input
                v-model="form.memo"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 4 }"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row class="right-button-group footer">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button
            v-if="(originId !== undefined) | (active === 3)"
            type="primary"
            @click="runInference"
          >
            実行
          </el-button>
        </el-row>
      </el-form>
    </div>
    <div v-else>
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step title="Step 1" description="推論名 & データセット" />
          <el-step title="Step 2" description="コンテナ & モデル" />
          <el-step title="Step 3" description="リソース" />
          <el-step title="Step 4" description="オプション" />
        </el-steps>
        <div class="element">
          <!-- step 1 -->
          <el-form v-if="active === 0" ref="form0" :model="form" :rules="rules">
            <el-col :span="12">
              <el-form-item label="推論名" prop="name">
                <el-input v-model="form.name" />
              </el-form-item>
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
                multiple
              />
              <kqi-inference-history-selector
                v-model="form.selectedParentInference"
                :histories="inferenceHistories"
                multiple
              />
            </el-col>
            <el-col :span="12">
              <kqi-data-set-selector
                v-model="form.dataSetId"
                :data-sets="dataSets"
              />
              <el-form-item label="データセット作成方式">
                <el-switch
                  v-model="form.localDataSet"
                  style="width: 100%;"
                  inactive-text="シンボリックリンク"
                  active-text="ローカルコピー"
                />
              </el-form-item>
            </el-col>
          </el-form>

          <!-- step 2 -->
          <el-form
            v-else-if="active === 1"
            ref="form1"
            :model="form"
            :rules="rules"
          >
            <el-col :span="12">
              <kqi-container-selector
                v-model="form.containerImage"
                :registries="registries"
                :images="images"
                :tags="tags"
                @selectRegistry="selectRegistry"
                @selectImage="selectImage"
              />
              <kqi-git-selector
                v-model="form.gitModel"
                :gits="gits"
                :repositories="repositories"
                :branches="branches"
                :commits="commits"
                :loading-repositories="loadingRepositories"
                @selectGit="selectGit"
                @selectRepository="selectRepository"
                @selectBranch="selectBranch"
              />
            </el-col>
            <el-col :span="12">
              <el-form-item label="実行コマンド" prop="entryPoint">
                <el-input
                  v-model="form.entryPoint"
                  type="textarea"
                  :autosize="{ minRows: 10 }"
                />
              </el-form-item>
            </el-col>
          </el-form>

          <!-- step 3 -->
          <el-form
            v-else-if="active === 2"
            ref="form2"
            :model="form"
            :rules="rules"
          >
            <el-col :span="18" :offset="3">
              <kqi-resource-selector v-model="form.resource" :quota="quota" />
            </el-col>
          </el-form>

          <!-- step 4 -->
          <el-form
            v-else-if="active === 3"
            ref="form3"
            :model="form"
            :rules="rules"
          >
            <el-col>
              <kqi-environment-variables v-model="form.variables" />
              <el-form-item label="結果Zip圧縮">
                <el-switch
                  v-model="form.zip"
                  style="width: 100%;"
                  inactive-text="圧縮しない"
                  active-text="圧縮する"
                />
              </el-form-item>
              <kqi-partition-selector
                v-model="form.partition"
                :partitions="partitions"
              />
              <el-form-item label="メモ">
                <el-input
                  v-model="form.memo"
                  type="textarea"
                  :autosize="{ minRows: 2, maxRows: 4 }"
                />
              </el-form-item>
            </el-col>
          </el-form>
        </div>
      </el-row>
      <el-row class="step">
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
        <el-button
          v-if="active === 3"
          class="right-step-group"
          type="primary"
          @click="runInference"
        >
          実行
        </el-button>
      </el-row>
    </div>
  </el-dialog>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDataSetSelector from '@/components/selector/KqiDataSetSelector'
import KqiTrainingHistorySelector from '@/components/selector/KqiTrainingHistorySelector'
import KqiInferenceHistorySelector from '@/components/selector/KqiInferenceHistorySelector'
import KqiContainerSelector from '@/components/selector/KqiContainerSelector'
import KqiGitSelector from '@/components/selector/KqiGitSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiEnvironmentVariables from '@/components/KqiEnvironmentVariables'
import KqiPartitionSelector from '@/components/selector/KqiPartitionSelector'
import validator from '@/util/validator'
import registrySelectorUtil from '@/util/registrySelectorUtil'
import gitSelectorUtil from '@/util/gitSelectorUtil'
import { mapActions, mapGetters } from 'vuex'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  components: {
    KqiDisplayError,
    KqiDataSetSelector,
    KqiTrainingHistorySelector,
    KqiInferenceHistorySelector,
    KqiContainerSelector,
    KqiGitSelector,
    KqiResourceSelector,
    KqiEnvironmentVariables,
    KqiPartitionSelector,
  },
  props: {
    originId: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      form: {
        name: null,
        dataSetId: null,
        entryPoint: null,
        selectedParent: [],
        selectedParentInference: [],
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
        variables: [{ key: '', value: '' }],
        partition: null,
        zip: true,
        localDataSet: false,
        memo: null,
      },
      rules: {
        name: [formRule],
        dataSetId: [formRule],
        entryPoint: [formRule],
        containerImage: {
          required: true,
          trigger: 'blur',
          validator: validator.containerImageValidator,
        },
        gitModel: {
          required: true,
          trigger: 'blur',
          validator: validator.gitModelValidator,
        },
      },
      dialogVisible: true,
      error: null,
      active: 0,
      isCopyCreation: false,
    }
  },
  computed: {
    ...mapGetters({
      trainingHistories: ['training/historiesToMount'],
      inferenceHistories: ['inference/historiesToMount'],
      trainingDetail: ['training/detail'],
      dataSets: ['dataSet/dataSets'],
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
      inferenceDetail: ['inference/detail'],
      partitions: ['cluster/partitions'],
      quota: ['cluster/quota'],
    }),
  },
  async created() {
    let origin = this.$route.query.origin
    let fromTrainView = origin === 'train'

    // originIdが設定されている場合、コピー実行とみなして実行
    this.isCopyCreation = this.originId !== null

    // 指定に必要な情報を取得
    await this['training/fetchHistoriesToMount']({
      status: ['Completed', 'UserCanceled'],
    })
    await this['inference/fetchHistoriesToMount']({
      status: ['Completed', 'UserCanceled', 'Killed'],
    })
    await this['cluster/fetchPartitions']()
    await this['cluster/fetchQuota']()
    await this['dataSet/fetchDataSets']()

    // レジストリ一覧を取得し、デフォルトレジストリを設定
    await this['registrySelector/fetchRegistries']()
    this.form.containerImage.registry = this.registries.find(registry => {
      return registry.id === this.defaultRegistryId
    })
    await this.selectRegistry(this.defaultRegistryId)

    // gitサーバ一覧を取得し、デフォルトgitサーバを設定
    await this['gitSelector/fetchGits']()
    this.form.gitModel.git = this.gits.find(git => {
      return git.id === this.defaultGitId
    })
    await this['gitSelector/fetchRepositories'](this.defaultGitId)

    // 学習からの遷移の場合は、マウントする学習を設定
    if (fromTrainView) {
      // originIdは学習のIDとなっている
      this.form.selectedParent = []
      this.trainingHistories.forEach(history => {
        if (String(history.id) === this.originId) {
          this.form.selectedParent = [history]
        }
      })
    }

    // 学習からの遷移、もしくはコピー実行時はコピー元情報を各項目を設定
    if (this.isCopyCreation) {
      let detail = {}
      if (fromTrainView) {
        // 学習からの遷移
        await this['training/fetchDetail'](this.originId)
        detail = this.trainingDetail
      } else {
        // 推論のコピー実行
        await this['inference/fetchDetail'](this.originId)
        detail = this.inferenceDetail
        this.form.name = detail.name
        this.form.entryPoint = detail.entryPoint
        this.form.zip = detail.zip
        this.form.localDataSet = detail.localDataSet
        this.form.memo = detail.memo
        this.form.selectedParent = []
        if (detail.parents) {
          this.trainingHistories.forEach(history => {
            detail.parents.forEach(parent => {
              if (history.id === parent.id) {
                this.form.selectedParent.push(parent)
              }
            })
          })
        }
        this.form.selectedParentInference = []
        if (detail.inferences) {
          this.inferenceHistories.forEach(history => {
            detail.inferences.forEach(parent => {
              if (history.id === parent.id) {
                this.form.selectedParentInference.push(parent)
              }
            })
          })
        }
        this.form.resource.cpu = detail.cpu
        this.form.resource.memory = detail.memory
        this.form.resource.gpu = detail.gpu

        this.form.variables =
          detail.options.length === 0
            ? [{ key: '', value: '' }]
            : detail.options
        this.form.partition = detail.partition
      }

      // 下記は学習からの遷移/コピー実行に関わらずコピー
      this.form.dataSetId = String(detail.dataSet.id)
      // レジストリの設定
      this.form.containerImage.registry = {
        id: detail.containerImage.registryId,
        name: detail.containerImage.name,
      }
      await this.selectRegistry(detail.containerImage.registryId)
      this.form.containerImage.image = detail.containerImage.image
      await this.selectImage(detail.containerImage.image)
      this.form.containerImage.tag = detail.containerImage.tag

      // gitモデルの設定
      this.form.gitModel.git = {
        id: detail.gitModel.gitId,
        name: detail.gitModel.name,
      }
      await this.selectGit(detail.gitModel.gitId)
      this.form.gitModel.repository = `${detail.gitModel.owner}/${detail.gitModel.repository}`
      await this.selectRepository(this.form.gitModel.repository)
      this.form.gitModel.branch = detail.gitModel.branch
      await this.selectBranch(detail.gitModel.branch)
      // commitsから該当commitを抽出
      let commit = this.commits.find(commit => {
        return commit.commitId === detail.gitModel.commitId
      })
      if (commit) {
        this.form.gitModel.commit = commit
      } else {
        // コミット一覧に含まれないコミットなので、コミット情報を新たに取得する
        await this['gitSelector/fetchCommitDetail']({
          gitId: this.form.gitModel.git.id,
          repository: this.form.gitModel.repository,
          commitId: detail.gitModel.commitId,
        })
        this.form.gitModel.commit = this.commitDetail
      }
    }
  },
  methods: {
    ...mapActions([
      'training/fetchHistoriesToMount',
      'inference/fetchHistoriesToMount',
      'training/fetchDetail',
      'inference/fetchDetail',
      'inference/post',
      'cluster/fetchPartitions',
      'cluster/fetchQuota',
      'dataSet/fetchDataSets',
      'registrySelector/fetchRegistries',
      'registrySelector/fetchImages',
      'registrySelector/fetchTags',
      'gitSelector/fetchGits',
      'gitSelector/fetchRepositories',
      'gitSelector/fetchBranches',
      'gitSelector/fetchCommits',
      'gitSelector/fetchCommitDetail',
    ]),
    async runInference() {
      let form = this.$refs.runForm
      if (this.active !== 0) {
        form = this.$refs.form3
      }
      await form.validate(async valid => {
        if (valid) {
          try {
            let options = {}
            // apiのフォーマットに合わせる(配列 => オブジェクト)
            this.form.variables.forEach(kvp => {
              options[kvp.key] = kvp.value
            })
            // training history ObjectのリストからIDのみを抜き出して格納
            let selectedParentIds = []
            this.form.selectedParent.forEach(parent => {
              selectedParentIds.push(parent.id)
            })
            let selectedParentInferenceIds = []
            this.form.selectedParentInference.forEach(inference => {
              selectedParentInferenceIds.push(inference.id)
            })
            // ブランチ未指定の際はcommitIdも未指定で実行
            // ブランチ指定時、HEADが指定された際はcommitsの先頭要素をcommitIDに指定する。コピー実行時の再現性を担保するため
            let commitId = null
            if (this.form.gitModel.branch) {
              commitId = this.form.gitModel.commit
                ? this.form.gitModel.commit.commitId
                : this.commits[0].commitId
            }
            // コピー時ブランチを切り替えずに実行
            // パラメータに格納する際の形を統一するため整形を行う
            if (typeof this.form.gitModel.branch.branchName === 'undefined') {
              let branch = { branchName: this.form.gitModel.branch }
              this.form.gitModel.branch = branch
            }

            let params = {
              Name: this.form.name,
              DataSetId: this.form.dataSetId,
              ParentIds: selectedParentIds,
              inferenceIds: selectedParentInferenceIds,
              ContainerImage: {
                registryId: this.form.containerImage.registry.id,
                image: this.form.containerImage.image,
                tag: this.form.containerImage.tag,
              },
              GitModel: {
                gitId: this.form.gitModel.git.id,
                repository: this.form.gitModel.repository.name,
                owner: this.form.gitModel.repository.owner,
                branch: this.form.gitModel.branch
                  ? this.form.gitModel.branch.branchName
                  : null,
                commitId: commitId,
              },
              EntryPoint: this.form.entryPoint,
              Cpu: this.form.resource.cpu,
              Memory: this.form.resource.memory,
              Gpu: this.form.resource.gpu,
              Options: options,
              Zip: this.form.zip,
              LocalDataSet: this.form.localDataSet,
              Partition: this.form.partition,
              Memo: this.form.memo,
            }
            await this['inference/post'](params)
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.$emit('done')
    },
    closeDialog(done) {
      done()
      this.emitCancel()
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

    // コンテナイメージ
    async selectRegistry(registryId) {
      await registrySelectorUtil.selectRegistry(
        this.form,
        this['registrySelector/fetchImages'],
        registryId,
      )
    },
    async selectImage(image) {
      await registrySelectorUtil.selectImage(
        this.form,
        this['registrySelector/fetchTags'],
        this.form.containerImage.registry.id,
        image,
      )
    },

    // モデル
    async selectGit(gitId) {
      await gitSelectorUtil.selectGit(
        this.form,
        this['gitSelector/fetchRepositories'],
        gitId,
        this.$store,
      )
    },
    // repositoryの型がstring：手入力, object: 選択
    async selectRepository(repository) {
      try {
        await gitSelectorUtil.selectRepository(
          this.form,
          this['gitSelector/fetchBranches'],
          repository,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }
    },
    async selectBranch(branchName) {
      await gitSelectorUtil.selectBranch(
        this.form,
        this['gitSelector/fetchCommits'],
        branchName,
      )
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

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}

.step {
  padding-top: 20px;
  cursor: pointer;
  :hover {
    color: #409eff;
  }
}

.element {
  padding-top: 40px;
}
</style>
