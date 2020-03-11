<template>
  <el-dialog
    class="dialog"
    title="ノートブック起動"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <kqi-display-error :error="error" />
    <!-- コピー実行 -->
    <div v-if="isCopyCreation">
      <el-form ref="runForm" :rules="rules" :model="form">
        <el-row :gutter="20">
          <div class="element">
            <el-col :span="12">
              <el-form-item label="ノートブック名" prop="name">
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
              <kqi-resource-selector v-model="form.resource" />
              <div v-if="availableInfiniteTime">
                <el-form-item label="起動期間設定">
                  <el-switch
                    v-model="form.withExpiresInSetting"
                    style="width: 100%;"
                    inactive-text="OFF"
                    active-text="ON"
                  />
                </el-form-item>
              </div>
              <div v-show="form.withExpiresInSetting">
                <el-form-item label="起動期間(h)" required>
                  <el-slider
                    v-model="form.expiresIn"
                    class="el-input"
                    :min="1"
                    :max="100"
                    show-input
                  />
                </el-form-item>
              </div>
              <kqi-environment-variables v-model="form.variables" />
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
          </div>
        </el-row>
      </el-form>
    </div>
    <!-- 再実行 -->
    <div v-else-if="isReRunCreation">
      <el-row :gutter="20">
        <div class="element">
          <el-form v-if="active === 0">
            <el-col :span="18" :offset="3">
              <kqi-resource-selector v-model="form.resource" />
            </el-col>
            <el-col :span="18" :offset="3">
              <div v-if="availableInfiniteTime">
                <el-form-item label="起動期間設定">
                  <el-switch
                    v-model="form.withExpiresInSetting"
                    style="width: 100%;"
                    inactive-text="OFF"
                    active-text="ON"
                  />
                </el-form-item>
              </div>
              <div v-show="form.withExpiresInSetting">
                <el-form-item label="起動期間(h)" required>
                  <el-slider
                    v-model="form.expiresIn"
                    class="el-input"
                    :min="1"
                    :max="100"
                    show-input
                  />
                </el-form-item>
              </div>
            </el-col>
            <el-col :span="18" :offset="3">
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
                multiple
              />
              <kqi-data-set-selector
                v-model="form.dataSetId"
                :data-sets="dataSets"
              />
            </el-col>
          </el-form>
        </div>
      </el-row>
    </div>
    <div v-else>
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step title="Step 1" description="ノートブック名" />
          <el-step title="Step 2" description="リソース & 起動期間" />
          <el-step title="Step 3" description="任意項目" />
          <el-step title="Step 4" description="任意項目" />
        </el-steps>
        <br />
        <div class="element">
          <!-- step 1 -->
          <el-form v-if="active === 0" ref="form0" :model="form" :rules="rules">
            <el-col :span="18" :offset="3">
              <el-form-item
                label="ノートブック名"
                prop="name"
                class="is-required"
              >
                <el-input v-model="form.name" />
              </el-form-item>
            </el-col>
          </el-form>

          <!-- step 2 -->
          <el-form v-if="active === 1" ref="form1" :model="form" :rules="rules">
            <el-col :span="18" :offset="3">
              <kqi-resource-selector v-model="form.resource" />
            </el-col>
            <el-col :span="18" :offset="3">
              <div v-if="availableInfiniteTime">
                <el-form-item label="起動期間設定">
                  <el-switch
                    v-model="form.withExpiresInSetting"
                    style="width: 100%;"
                    inactive-text="OFF"
                    active-text="ON"
                  />
                </el-form-item>
              </div>
              <div v-show="form.withExpiresInSetting">
                <el-form-item label="起動期間(h)" required>
                  <el-slider
                    v-model="form.expiresIn"
                    class="el-input"
                    :min="1"
                    :max="100"
                    show-input
                  />
                </el-form-item>
              </div>
            </el-col>
          </el-form>

          <!-- step 3 -->
          <el-form v-if="active === 2" ref="form2" :model="form" :rules="rules">
            <el-col :span="10" :offset="2">
              <kqi-container-selector
                v-model="form.containerImage"
                :registries="registries"
                :images="images"
                :tags="tags"
                @selectRegistry="selectRegistry"
                @selectImage="selectImage"
              />
            </el-col>
            <el-col :span="10">
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
            <el-col :span="18" :offset="3">
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
                multiple
              />
              <kqi-data-set-selector
                v-model="form.dataSetId"
                :data-sets="dataSets"
              />
            </el-col>
          </el-form>

          <!-- step 4 -->
          <el-form v-if="active === 3" ref="form3" :model="form" :rules="rules">
            <el-col>
              <kqi-environment-variables v-model="form.variables" />
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
    </div>
    <el-row class="step">
      <div v-if="isCopyCreation || isReRunCreation">
        <span class="right-step-group">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="runNotebook">起動 </el-button>
        </span>
      </div>
      <div v-else>
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
          <el-button v-if="active === 3" type="primary" @click="runNotebook">
            起動
          </el-button>
        </span>
      </div>
    </el-row>
  </el-dialog>
</template>

<script>
import KqiDataSetSelector from '@/components/selector/KqiDataSetSelector'
import KqiTrainingHistorySelector from '@/components/selector/KqiTrainingHistorySelector'
import KqiContainerSelector from '@/components/selector/KqiContainerSelector'
import KqiGitSelector from '@/components/selector/KqiGitSelector'
import KqiPartitionSelector from '@/components/selector/KqiPartitionSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiEnvironmentVariables from '@/components/KqiEnvironmentVariables'
import KqiDisplayError from '@/components/KqiDisplayError'
import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    KqiDataSetSelector,
    KqiResourceSelector,
    KqiTrainingHistorySelector,
    KqiContainerSelector,
    KqiGitSelector,
    KqiPartitionSelector,
    KqiEnvironmentVariables,
    KqiDisplayError,
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
        expiresIn: 8,
        withExpiresInSetting: true,
        variables: [{ key: '', value: '' }],
        partition: null,
        memo: null,
      },
      rules: {
        name: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
      },
      dialogVisible: true,
      error: undefined,
      active: 0,
      isCopyCreation: false,
      isReRunCreation: false,
    }
  },
  computed: {
    ...mapGetters({
      trainingHistories: ['training/histories'],
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
      loadingRepositories: ['gitSelector/loadingRepositories'],
      availableInfiniteTime: ['notebook/availableInfiniteTime'],
      detail: ['notebook/detail'],
      partitions: ['cluster/partitions'],
    }),
  },
  async created() {
    if (this.originId !== null) {
      if (Object.keys(this.$route.query).length !== 0) {
        this.isCopyCreation = this.$route.query.run.indexOf('copy') !== -1
      } else {
        this.isReRunCreation = true
      }
    }

    // 指定に必要な情報を取得
    await this['training/fetchHistories']()
    await this['cluster/fetchPartitions']()
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

    await this['notebook/fetchAvailableInfiniteTime']()

    // コピー実行時はコピー元情報を各項目を設定
    if (this.isCopyCreation) {
      await this['notebook/fetchDetail'](this.originId)

      this.form.name = this.detail.name
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

      if (this.detail.dataSet) {
        this.form.dataSetId = String(this.detail.dataSet.id)
      }

      // レジストリの設定
      if (this.detail.containerImage.registryId) {
        this.form.containerImage.registry = {
          id: this.detail.containerImage.registryId,
          name: this.detail.containerImage.name,
        }
        await this.selectRegistry(this.detail.containerImage.registryId)
        this.form.containerImage.image = this.detail.containerImage.image
        await this.selectImage()
        this.form.containerImage.tag = this.detail.containerImage.tag
      }

      // gitモデルの設定
      if (this.detail.gitModel.gitId) {
        this.form.gitModel.git = {
          id: this.detail.gitModel.gitId,
          name: this.detail.gitModel.name,
        }
        await this.selectGit(this.detail.gitModel.gitId)
        this.form.gitModel.repository = `${this.detail.gitModel.owner}/${this.detail.gitModel.repository}`
        await this.selectRepository(this.form.gitModel.repository)
        this.form.gitModel.branch = this.detail.gitModel.branch
        await this.selectBranch(this.detail.gitModel.branch)
        this.form.gitModel.commit = this.detail.gitModel.commitId
      }

      this.form.resource.cpu = this.detail.cpu
      this.form.resource.memory = this.detail.memory
      this.form.resource.gpu = this.detail.gpu
      if (this.detail.expiresIn === 0) {
        if (this.availableInfiniteTime) {
          this.withExpiresInSetting = false
        }
        this.expiresIn = 8
      } else {
        this.expiresIn = origin.expiresIn / 60 / 60
      }

      this.form.variables =
        this.detail.options.length === 0
          ? [{ key: '', value: '' }]
          : this.detail.options
      this.form.partition = this.detail.partition
    } else if (this.isReRunCreation) {
      // 再実行時は親、データセット、リソース情報をコピー
      await this['notebook/fetchDetail'](this.originId)

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

      if (this.detail.dataSet) {
        this.form.dataSetId = String(this.detail.dataSet.id)
      }
      this.form.resource.cpu = this.detail.cpu
      this.form.resource.memory = this.detail.memory
      this.form.resource.gpu = this.detail.gpu
      if (this.detail.expiresIn === 0) {
        if (this.availableInfiniteTime) {
          this.withExpiresInSetting = false
        }
        this.expiresIn = 8
      } else {
        this.expiresIn = origin.expiresIn / 60 / 60
      }
    }
  },

  methods: {
    ...mapActions([
      'training/fetchHistories',
      'notebook/fetchDetail',
      'notebook/post',
      'notebook/postRerun',
      'notebook/fetchAvailableInfiniteTime',
      'cluster/fetchPartitions',
      'dataSet/fetchDataSets',
      'registrySelector/fetchRegistries',
      'registrySelector/fetchImages',
      'registrySelector/fetchTags',
      'gitSelector/fetchGits',
      'gitSelector/fetchRepositories',
      'gitSelector/fetchBranches',
      'gitSelector/fetchCommits',
    ]),
    async runNotebook() {
      if (this.isReRunCreation) {
        // 再実行
        try {
          if (!this.form.withExpiresInSetting) {
            this.form.expiresIn = 0
          }
          // training history ObjectのリストからIDのみを抜き出して格納
          let selectedParentIds = []
          this.form.selectedParent.forEach(parent => {
            selectedParentIds.push(parent.id)
          })
          let params = {
            dataSetId: this.form.dataSetId,
            parentIds: selectedParentIds,
            cpu: this.form.resource.cpu,
            memory: this.form.resource.memory,
            gpu: this.form.resource.gpu,
            expiresIn: this.form.expiresIn * 60 * 60,
          }
          await this['notebook/postRerun']({
            id: this.originId,
            params: params,
          })

          // 成功したら、ダイヤログを閉じて更新
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      } else {
        // 実行
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
              if (!this.form.withExpiresInSetting) {
                this.form.expiresIn = 0
              }
              // コンテナイメージの指定
              // イメージとタグが指定されている場合、コンテナイメージを指定して登録
              // イメージとタグが指定されていない場合、コンテナイメージは未指定(null)として登録
              let containerImage = null
              if (
                this.form.containerImage.image !== null &&
                this.form.containerImage.tag !== null
              ) {
                containerImage = {
                  registryId: this.form.containerImage.registry.id,
                  image: this.form.containerImage.image,
                  tag: this.form.containerImage.tag,
                }
              }

              // gitモデルの指定
              // リポジトリとブランチが指定されている場合、gitモデルを指定して登録
              // リポジトリとブランチが指定されていない場合、gitモデルは未指定(null)として登録
              let gitModel = null
              if (
                this.form.gitModel.repository !== null &&
                this.form.gitModel.branch !== null
              ) {
                gitModel = {
                  gitId: this.form.gitModel.git.id,
                  repository: this.form.gitModel.repository.name,
                  owner: this.form.gitModel.repository.owner,
                  branch: this.form.gitModel.branch.branchName,
                  commitId: this.form.gitModel.commit
                    ? this.form.gitModel.commit.commitId
                    : 'HEAD',
                }
              }
              // training history ObjectのリストからIDのみを抜き出して格納
              let selectedParentIds = []
              this.form.selectedParent.forEach(parent => {
                selectedParentIds.push(parent.id)
              })
              let params = {
                name: this.form.name,
                dataSetId: this.form.dataSetId,
                parentIds: selectedParentIds,
                ContainerImage: containerImage,
                GitModel: gitModel,
                cpu: this.form.resource.cpu,
                memory: this.form.resource.memory,
                gpu: this.form.resource.gpu,
                expiresIn: this.form.expiresIn * 60 * 60,
                options: options,
                partition: this.form.partition,
                memo: this.form.memo,
              }
              await this['notebook/post'](params)
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      }
    },

    emitDone() {
      this.$emit('done')
      this.dialogVisible = false
    },
    emitCancel() {
      this.$emit('cancel')
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
      // 過去の選択状態をリセット
      this.form.containerImage.image = null
      this.form.containerImage.tag = null
      // clearの場合リセット、レジストリが選択された場合はイメージ取得
      if (this.form.containerImage.registry !== null) {
        await this['registrySelector/fetchImages'](registryId)
      }
    },
    async selectImage() {
      // 過去の選択状態をリセット
      this.form.containerImage.tag = null

      // clearの場合リセット、イメージが選択された場合はタグ取得
      if (this.form.containerImage.image !== null) {
        await this['registrySelector/fetchTags']({
          registryId: this.form.containerImage.registry.id,
          image: this.form.containerImage.image,
        })
      }
    },

    // モデル
    async selectGit(gitId) {
      // 過去の選択状態をリセット
      this.form.gitModel.repository = null
      this.form.gitModel.branch = null
      this.form.gitModel.commit = null

      // clearの場合リセット、gitサーバが選択された場合はリポジトリ取得
      if (this.form.gitModel.git !== null) {
        // 独自ローディング処理のため共通側は無効
        this.$store.commit('setLoading', false)
        await this['gitSelector/fetchRepositories'](gitId)
        // 共通側ローディングを再度有効化
        this.$store.commit('setLoading', true)
      }
    },
    // repositoryの型がstring：手入力, object: 選択
    async selectRepository(repository) {
      // 過去の選択状態をリセット
      this.form.gitModel.branch = null
      this.form.gitModel.commit = null

      let manualInput = false
      let argRepository = {}
      if (typeof repository === 'string') {
        manualInput = true
        let repositoryName = repository
        let index = repositoryName.indexOf('/')
        if (index > 0) {
          argRepository = {
            owner: repositoryName.substring(0, index),
            name: repositoryName.substring(index + 1),
            fullName: repositoryName,
          }
          this.form.gitModel.repository = argRepository
        } else {
          //構文エラー
        }
      } else {
        argRepository = repository
      }
      // clearの場合リセット、リポジトリが選択された場合はブランチ取得
      if (this.form.gitModel.repository !== null) {
        await this['gitSelector/fetchBranches']({
          gitId: this.form.gitModel.git.id,
          repository: argRepository,
          manualInput: manualInput,
        })
      }
    },
    async selectBranch(branchName) {
      // 過去の選択状態をリセット
      this.form.gitModel.commit = null

      // clearの場合リセット、ブランチが選択された場合はコミット取得
      if (this.form.gitModel.branch !== null) {
        await this['gitSelector/fetchCommits']({
          gitId: this.form.gitModel.git.id,
          repository: this.form.gitModel.repository,
          branchName: branchName,
        })
      }
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
.el-step__description {
  font-size: 14px;
}
</style>