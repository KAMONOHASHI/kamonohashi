<template>
  <el-dialog
    class="dialog"
    title="学習実行"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <kqi-display-error :error="error" />
    <div v-if="isCopyCreation">
      <el-form ref="runForm" :rules="rules" :model="form">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="学習名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <kqi-training-history-selector
              v-model="form.selectedParent"
              :histories="trainingHistories"
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
            <kqi-expose-ports v-model="form.ports" />
            <el-form-item label="タグ">
              <kqi-tag-editor
                v-model="form.tags"
                :registered-tags="tenantTags"
              />
            </el-form-item>
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
            @click="runTrain"
          >
            実行
          </el-button>
        </el-row>
      </el-form>
    </div>
    <div v-else>
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step title="Step 1" description="学習名 & データセット" />
          <el-step title="Step 2" description="コンテナ & モデル" />
          <el-step title="Step 3" description="リソース" />
          <el-step title="Step 4" description="オプション" />
        </el-steps>
        <div class="element">
          <!-- step 1 -->
          <el-form v-if="active === 0" ref="form0" :model="form" :rules="rules">
            <el-col :span="12">
              <el-form-item label="学習名" prop="name">
                <el-input v-model="form.name" />
              </el-form-item>
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
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
              <kqi-expose-ports v-model="form.ports" />
              <el-form-item label="タグ">
                <kqi-tag-editor
                  v-model="form.tags"
                  :registered-tags="tenantTags"
                />
              </el-form-item>
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
          @click="runTrain"
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
import KqiContainerSelector from '@/components/selector/KqiContainerSelector'
import KqiGitSelector from '@/components/selector/KqiGitSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiEnvironmentVariables from '@/components/KqiEnvironmentVariables'
import KqiExposePorts from '@/components/KqiExposePorts'
import KqiTagEditor from '@/components/KqiTagEditor'
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
    KqiContainerSelector,
    KqiGitSelector,
    KqiResourceSelector,
    KqiEnvironmentVariables,
    KqiExposePorts,
    KqiTagEditor,
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
        ports: [],
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
      detail: ['training/detail'],
      partitions: ['cluster/partitions'],
      quota: ['cluster/quota'],
      tenantTags: ['training/tenantTags'],
    }),
  },
  async created() {
    this.isCopyCreation = this.originId !== null

    // 指定に必要な情報を取得
    await this['training/fetchHistoriesToMount']({
      status: [
        'Completed',
        'UserCanceled',
        'Killed',
        'Failed',
        'None',
        'Error',
      ],
    })
    await this['cluster/fetchPartitions']()
    await this['cluster/fetchQuota']()
    await this['dataSet/fetchDataSets']()
    await this['training/fetchTenantTags']()

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

    // コピー実行時はコピー元情報を各項目を設定
    if (this.isCopyCreation) {
      await this['training/fetchDetail'](this.originId)

      this.form.name = this.detail.name
      this.form.dataSetId = String(this.detail.dataSet.id)
      this.form.entryPoint = this.detail.entryPoint
      this.form.zip = this.detail.zip
      this.form.localDataSet = this.detail.localDataSet
      this.form.memo = this.detail.memo
      this.form.selectedParent = []
      if (this.detail.parents) {
        this.trainingHistories.forEach(history => {
          this.detail.parents.forEach(parent => {
            if (history.id === parent.id) {
              this.form.selectedParent.push(parent)
            }
          })
        })
      }
      this.form.resource.cpu = this.detail.cpu
      this.form.resource.memory = this.detail.memory
      this.form.resource.gpu = this.detail.gpu
      this.form.variables =
        this.detail.options.length === 0
          ? [{ key: '', value: '' }]
          : this.detail.options
      this.form.ports = this.detail.ports
      this.form.partition = this.detail.partition
      this.form.tags = this.detail.tags

      // レジストリの設定
      this.form.containerImage.registry = {
        id: this.detail.containerImage.registryId,
        name: this.detail.containerImage.name,
      }
      await this.selectRegistry(this.detail.containerImage.registryId)
      this.form.containerImage.image = this.detail.containerImage.image
      await this.selectImage(this.detail.containerImage.image)
      this.form.containerImage.tag = this.detail.containerImage.tag

      // gitモデルの設定
      this.form.gitModel.git = {
        id: this.detail.gitModel.gitId,
        name: this.detail.gitModel.name,
      }
      await this.selectGit(this.detail.gitModel.gitId)
      this.form.gitModel.repository = `${this.detail.gitModel.owner}/${this.detail.gitModel.repository}`
      await this.selectRepository(this.form.gitModel.repository)
      this.form.gitModel.branch = this.detail.gitModel.branch
      await this.selectBranch(this.detail.gitModel.branch)
      // commitsから該当commitを抽出
      let commit = this.commits.find(commit => {
        return commit.commitId === this.detail.gitModel.commitId
      })
      if (commit) {
        this.form.gitModel.commit = commit
      } else {
        // コミット一覧に含まれないコミットなので、コミット情報を新たに取得する
        await this['gitSelector/fetchCommitDetail']({
          gitId: this.form.gitModel.git.id,
          repository: this.form.gitModel.repository,
          commitId: this.detail.gitModel.commitId,
        })
        this.form.gitModel.commit = this.commitDetail
      }
    }
  },
  methods: {
    ...mapActions([
      'training/fetchHistoriesToMount',
      'training/fetchDetail',
      'training/fetchTenantTags',
      'training/post',
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
    async runTrain() {
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
              Ports: this.form.ports,
              Zip: this.form.zip,
              LocalDataSet: this.form.localDataSet,
              Partition: this.form.partition,
              Memo: this.form.memo,
              tags: this.form.tags,
            }
            await this['training/post'](params)
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
      // 過去の選択状態をリセット
      this.form.gitModel.commit = null
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
