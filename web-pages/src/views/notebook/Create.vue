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
            <el-form v-if="active === 0">
              <el-col :span="12">
                <el-form-item label="ノートブック名" prop="name">
                  <el-input v-model="form.name" />
                </el-form-item>
                <kqi-training-history-selector
                  v-model="form.selectedParent"
                  :histories="trainingHistories"
                  multiple
                />
                <kqi-data-set-selector @input="selectDataSet" />
                <kqi-container-selector @input="selectContainer" />
                <kqi-git-selector @input="selectModel" />
              </el-col>
              <el-col :span="12">
                <kqi-resource-selector
                  v-model="form.resource"
                ></kqi-resource-selector>
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
                    >
                    </el-slider>
                  </el-form-item>
                </div>
                <kqi-environment-variables v-model="form.variables" />
                <kqi-partition-selector
                  :partition="form.partition"
                  @input="selectPartition"
                />
                <el-form-item label="メモ">
                  <el-input
                    v-model="form.memo"
                    type="textarea"
                    :autosize="{ minRows: 2, maxRows: 4 }"
                  >
                  </el-input>
                </el-form-item>
              </el-col>
            </el-form>
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
              <kqi-resource-selector
                v-model="form.resource"
              ></kqi-resource-selector>
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
                  >
                  </el-slider>
                </el-form-item>
              </div>
            </el-col>
            <el-col :span="18" :offset="3">
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
                multiple
              />
              <kqi-data-set-selector @input="selectDataSet" />
            </el-col>
          </el-form>
        </div>
      </el-row>
    </div>
    <div v-else>
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step title="Step 1" description="ノートブック名"></el-step>
          <el-step title="Step 2" description="リソース & 起動期間"></el-step>
          <el-step title="Step 3" description="任意項目"></el-step>
          <el-step title="Step 4" description="任意項目"></el-step>
        </el-steps>
        <br />
        <div class="element">
          <!-- step 1 -->
          <el-form v-if="active === 0">
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
          <el-form v-if="active === 1">
            <el-col :span="18" :offset="3">
              <kqi-resource-selector
                v-model="form.resource"
              ></kqi-resource-selector>
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
                  >
                  </el-slider>
                </el-form-item>
              </div>
            </el-col>
          </el-form>

          <!-- step 3 -->
          <el-form v-if="active === 2">
            <el-col :span="10" :offset="2">
              <kqi-container-selector @input="selectContainer" />
            </el-col>
            <el-col :span="10">
              <kqi-git-selector @input="selectModel" />
            </el-col>
            <el-col :span="18" :offset="3">
              <kqi-training-history-selector
                v-model="form.selectedParent"
                :histories="trainingHistories"
                multiple
              />
              <kqi-data-set-selector @input="selectDataSet" />
            </el-col>
          </el-form>

          <!-- step 4 -->
          <el-form v-if="active === 3">
            <el-col>
              <kqi-environment-variables v-model="form.variables" />
              <kqi-partition-selector
                :partition="form.partition"
                @input="selectPartition"
              />
              <el-form-item label="メモ">
                <el-input
                  v-model="form.memo"
                  type="textarea"
                  :autosize="{ minRows: 2, maxRows: 4 }"
                >
                </el-input>
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
          <i class="el-icon-arrow-left"></i>
          Previous step
        </span>
        <span
          v-if="active <= 2"
          class="right-step-group"
          style="margin-top: 12px;"
          @click="next"
        >
          Next step
          <i class="el-icon-arrow-right"></i>
        </span>
        <span class="right-step-group">
          <el-button v-if="active === 3" type="primary" @click="runNotebook"
            >起動
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

import { mapActions, mapMutations, mapGetters } from 'vuex'

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
        selectedParent: [],
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
      dataset: ['dataSet/detail'],
      registry: ['registrySelector/registry'],
      image: ['registrySelector/image'],
      tag: ['registrySelector/tag'],
      git: ['gitSelector/git'],
      repository: ['gitSelector/repository'],
      branch: ['gitSelector/branch'],
      commit: ['gitSelector/commit'],
      availableInfiniteTime: ['notebook/availableInfiniteTime'],
      detail: ['notebook/detail'],
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

    // vuexの情報をリセット
    await this.selectDataSet(null)
    await this.selectContainer({
      type: 'registry',
      value: null,
    })
    await this.selectModel({
      type: 'git',
      value: null,
    })

    // 指定に必要な情報を取得
    await this['training/fetchHistories']()
    await this['cluster/fetchPartitions']()
    await this['dataSet/fetchDataSets']()
    await this['registrySelector/fetchRegistries']()
    await this['registrySelector/fetchImages']()
    await this['gitSelector/fetchGits']()
    await this['gitSelector/fetchRepositories']()
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
        await this.selectDataSet(this.detail.dataSet.id)
      }

      if (this.detail.containerImage.registryId) {
        await this.selectContainer({
          type: 'registry',
          value: {
            id: this.detail.containerImage.registryId,
            name: this.detail.containerImage.name,
          },
        })
        await this.selectContainer({
          type: 'image',
          value: this.detail.containerImage.image,
        })
        await this.selectContainer({
          type: 'tag',
          value: this.detail.containerImage.tag,
        })
      }
      if (this.detail.gitModel.gitId) {
        await this.selectModel({
          type: 'git',
          value: {
            id: this.detail.gitModel.gitId,
            name: this.detail.gitModel.name,
          },
        })
        await this.selectModel({
          type: 'repository',
          value: `${this.detail.gitModel.owner}/${this.detail.gitModel.repository}`,
        })
        await this.selectModel({
          type: 'branch',
          value: this.detail.gitModel.branch,
        })
        await this.selectModel({
          type: 'commit',
          value: this.detail.gitModel.commitId,
        })
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
        await this.selectDataSet(this.detail.dataSet.id)
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
    ...mapMutations([
      'registrySelector/setRegistry',
      'registrySelector/setImage',
      'registrySelector/setTag',
      'gitSelector/setGit',
      'gitSelector/setRepository',
      'gitSelector/setBranch',
      'gitSelector/setCommit',
    ]),
    ...mapActions([
      'training/fetchHistories',
      'notebook/fetchDetail',
      'notebook/post',
      'notebook/postRerun',
      'notebook/fetchAvailableInfiniteTime',
      'cluster/fetchPartitions',
      'dataSet/fetchDetail',
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
            dataSetId: this.dataset ? this.dataset.id : null,
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
        try {
          let options = {}
          // apiのフォーマットに合わせる(配列 => オブジェクト)
          this.form.variables.forEach(kvp => {
            options[kvp.key] = kvp.value
          })
          if (!this.form.withExpiresInSetting) {
            this.form.expiresIn = 0
          }
          // コンテナイメージが指定されていれば設定。指定されていなければnullとし、サーバ側で自動設定
          let containerImage = null
          if (this.image !== null || this.tag !== null) {
            containerImage = {
              registryId: this.registry.id,
              image: this.image,
              tag: this.tag,
            }
          }
          // モデルが指定されていれば設定。指定されていなければnullとし、サーバ側で自動設定
          let gitModel = null
          if (
            this.repository !== null ||
            this.branch !== null ||
            this.commit !== null
          ) {
            gitModel = {
              gitId: this.git.id,
              repository: this.repository ? this.repository.name : null,
              owner: this.repository ? this.repository.owner : null,
              branch: this.branch ? this.branch.branchName : null,
              commitId: this.commit ? this.commit.commitId : 'HEAD',
            }
          }
          // training history ObjectのリストからIDのみを抜き出して格納
          let selectedParentIds = []
          this.form.selectedParent.forEach(parent => {
            selectedParentIds.push(parent.id)
          })
          let params = {
            name: this.form.name,
            dataSetId: this.dataset ? this.dataset.id : null,
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
    next() {
      if (this.active++ > 3) {
        this.active = 0
      }
    },
    previous() {
      if (this.active-- < 0) {
        this.active = 0
      }
    },

    // データセット
    async selectDataSet(dataSetId) {
      await this['dataSet/fetchDetail'](dataSetId)
    },

    // コンテナイメージ
    async selectContainer(arg) {
      // arg:
      // {
      //   type: 'registry' or 'image' or 'tag'
      //   value: id, name
      // }
      switch (arg.type) {
        case 'registry':
          // 過去の選択状態をリセット
          this['registrySelector/setImage'](null)
          this['registrySelector/setTag'](null)

          // clearの場合リセット、レジストリが選択された場合はイメージ取得
          if (arg.value == null) {
            this['registrySelector/setRegistry'](null)
          } else {
            this['registrySelector/setRegistry'](arg.value)
            await this['registrySelector/fetchImages']()
          }
          break

        case 'image':
          // 過去の選択状態をリセット
          this['registrySelector/setTag'](null)

          // clearの場合リセット、イメージが選択された場合はタグ取得
          if (arg.value == null) {
            this['registrySelector/setImage'](null)
          } else {
            this['registrySelector/setImage'](arg.value)
            await this['registrySelector/fetchTags']()
          }
          break

        case 'tag':
          // clearの場合リセット、タグが選択された場合は設定
          if (arg.value == null) {
            this['registrySelector/setTag'](null)
          } else {
            this['registrySelector/setTag'](arg.value)
          }
          break
      }
    },

    // モデル
    async selectModel(arg) {
      // arg:
      // {
      //   type: 'git' or 'repository' or 'branch' or 'commit
      //   value: id, name
      // }
      switch (arg.type) {
        case 'git':
          // 過去の選択状態をリセット
          this['gitSelector/setRepository'](null)
          this['gitSelector/setBranch'](null)
          this['gitSelector/setCommit'](null)

          // clearの場合リセット、gitサーバが選択された場合はリポジトリ取得
          if (arg.value == null) {
            this['gitSelector/setGit'](null)
          } else {
            this['gitSelector/setGit'](arg.value)
            // 独自ローディング処理のため共通側は無効
            this.$store.commit('setLoading', false)
            await this['gitSelector/fetchRepositories']()
            // 共通側ローディングを再度有効化
            this.$store.commit('setLoading', true)
          }
          break

        case 'repository':
          // 過去の選択状態をリセット
          this['gitSelector/setBranch'](null)
          this['gitSelector/setCommit'](null)

          // clearの場合リセット、リポジトリが選択された場合はブランチ取得
          if (arg.value == null) {
            this['gitSelector/setRepository'](null)
          } else {
            this['gitSelector/setRepository'](arg.value)
            await this['gitSelector/fetchBranches']()
          }
          break

        case 'branch':
          // 過去の選択状態をリセット
          this['gitSelector/setCommit'](null)

          // clearの場合リセット、ブランチが選択された場合はコミット取得
          if (arg.value == null) {
            this['gitSelector/setBranch'](null)
          } else {
            this['gitSelector/setBranch'](arg.value)
            await this['gitSelector/fetchCommits']()
          }
          break

        case 'commit':
          // clearの場合リセット、コミットが選択された場合は設定
          if (arg.value == null) {
            this['gitSelector/setCommit'](null)
          } else {
            this['gitSelector/setCommit'](arg.value)
          }
          break
      }
    },

    // パーティション
    selectPartition(partition) {
      this.form.partition = partition
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
