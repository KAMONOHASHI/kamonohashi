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
            <kqi-training-history-selector @input="selectParent" />
            <kqi-data-set-selector @input="selectDataSet" />

            <el-form-item label="実行コマンド" prop="entryPoint">
              <el-input
                v-model="form.entryPoint"
                type="textarea"
                :autosize="{ minRows: 10 }"
              />
            </el-form-item>
            <kqi-container-selector @input="selectContainer" />
            <kqi-git-selector @input="selectModel" />
          </el-col>
          <el-col :span="12">
            <kqi-resource-selector
              @input="selectResource"
            ></kqi-resource-selector>

            <kqi-environment-variables v-model="variables" />
            <el-form-item label="結果Zip圧縮">
              <el-switch
                v-model="form.zip"
                style="width: 100%;"
                inactive-text="圧縮しない"
                active-text="圧縮する"
              />
            </el-form-item>
            <kqi-partition-selector @input="selectPartition" />
            <el-form-item label="メモ">
              <el-input
                v-model="form.memo"
                type="textarea"
                :autosize="{ minRows: 2, maxRows: 4 }"
              >
              </el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row class="right-button-group footer">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button
            v-if="(originId !== undefined) | (active === 3)"
            type="primary"
            @click="runTrain"
            >実行
          </el-button>
        </el-row>
      </el-form>
    </div>
    <div v-else>
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step
            title="Step 1"
            description="training name & dataset"
          ></el-step>
          <el-step
            title="Step 2"
            description="container image & model"
          ></el-step>
          <el-step title="Step 3" description="resource"></el-step>
          <el-step title="Step 4" description="option"></el-step>
        </el-steps>
        <div class="element">
          <!-- step 1 -->
          <el-form v-if="active === 0" ref="form0" :model="form" :rules="rules">
            <el-col :span="12">
              <el-form-item label="学習名" prop="name">
                <el-input v-model="form.name" />
              </el-form-item>
              <kqi-training-history-selector @input="selectParent" />
            </el-col>
            <el-col :span="12">
              <kqi-data-set-selector @input="selectDataSet" />
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
              <kqi-container-selector @input="selectContainer" />
              <kqi-git-selector @input="selectModel" />
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
              <kqi-resource-selector
                @input="selectResource"
              ></kqi-resource-selector>
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
              <kqi-environment-variables v-model="variables" />
              <el-form-item label="結果Zip圧縮">
                <el-switch
                  v-model="form.zip"
                  style="width: 100%;"
                  inactive-text="圧縮しない"
                  active-text="圧縮する"
                />
              </el-form-item>
              <kqi-partition-selector @input="selectPartition" />
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
      <el-row class="step">
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
        <el-button
          v-if="active === 3"
          class="right-step-group"
          type="primary"
          @click="runTrain"
          >実行
        </el-button>
      </el-row>
    </div>
  </el-dialog>
</template>

<script>
import KqiDataSetSelector from '@/components/selector/KqiDatasetSelector.vue'
import KqiTrainingHistorySelector from '@/components/selector/KqiTrainingHistorySelector.vue'
import KqiContainerSelector from '@/components/selector/KqiContainerSelector.vue'
import KqiGitSelector from '@/components/selector/KqiGitSelector.vue'
import KqiPartitionSelector from '@/components/selector/KqiPartitionSelector.vue'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiEnvironmentVariables from '@/components/KqiEnvironmentVariables'
import KqiDisplayError from '@/components/KqiDisplayError'
import { mapActions, mapMutations, mapGetters } from 'vuex'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  components: {
    KqiDataSetSelector,
    KqiTrainingHistorySelector,
    KqiContainerSelector,
    KqiGitSelector,
    KqiPartitionSelector,
    KqiEnvironmentVariables,
    KqiDisplayError,
    KqiResourceSelector,
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
        entryPoint: null,
        zip: true,
        memo: null,
      },
      rules: {
        name: [formRule],
        entryPoint: [formRule],
      },
      variables: [{ key: '', value: '' }],
      dialogVisible: true,
      error: null,
      active: 0,
      isCopyCreation: false,
    }
  },
  computed: {
    ...mapGetters({
      dataset: ['dataSet/detail'],
      registry: ['registrySelector/registry'],
      image: ['registrySelector/image'],
      tag: ['registrySelector/tag'],
      git: ['gitSelector/git'],
      repository: ['gitSelector/repository'],
      branch: ['gitSelector/branch'],
      commit: ['gitSelector/commit'],
      detail: ['training/detail'],
      parent: ['training/parent'],
      resource: ['resource/resource'],
      partition: ['cluster/partition'],
    }),
  },
  async created() {
    this.isCopyCreation = this.originId !== null
    await this['training/fetchHistories']()
    await this['cluster/fetchPartitions']()
    await this['dataSet/fetchDataSets']()
    await this['registrySelector/fetchRegistries']()
    await this['gitSelector/fetchGits']()

    // vuexの情報をリセット
    await this.selectDataSet(null)
    await this.selectParent(null)
    await this.selectContainer({
      type: 'registry',
      value: null,
    })
    await this.selectModel({
      type: 'git',
      value: null,
    })
    this.selectResource({
      cpu: 1,
      memory: 1,
      gpu: 0,
    })
    this.selectPartition(null)

    // コピー実行時はコピー元情報を各項目を設定
    if (this.isCopyCreation) {
      await this['training/fetchDetail'](this.originId)

      this.form.name = this.detail.name
      this.form.entryPoint = this.detail.entryPoint
      this.form.zip = this.detail.zip
      this.form.memo = this.detail.memo

      await this.selectDataSet(this.detail.dataSet.id)
      await this.selectParent(this.detail.parent ? this.detail.parent.id : null)

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

      this.selectResource({
        cpu: this.detail.cpu,
        memory: this.detail.memory,
        gpu: this.detail.gpu,
      })
      this.variables = this.detail.options
      this.selectPartition(this.detail.partition)
    }
  },
  methods: {
    ...mapMutations([
      'cluster/setPartition',
      'dataSet/clearDetail',
      'training/clearDetail',
      'training/clearParent',
      'registrySelector/setRegistry',
      'registrySelector/setImage',
      'registrySelector/setTag',
      'gitSelector/setGit',
      'gitSelector/setRepository',
      'gitSelector/setBranch',
      'gitSelector/setCommit',
      'resource/setResource',
    ]),
    ...mapActions([
      'training/fetchHistories',
      'training/fetchDetail',
      'training/fetchParent',
      'training/post',
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
    async runTrain() {
      try {
        let options = {}
        // apiのフォーマットに合わせる(配列 => オブジェクト)
        this.variables.forEach(kvp => {
          options[kvp.key] = kvp.value
        })
        let params = {
          Name: this.form.name,
          DataSetId: this.dataset.id,
          ParentId: this.parent.id,
          ContainerImage: {
            registryId: this.registry.id,
            image: this.image,
            tag: this.tag,
          },
          GitModel: {
            gitId: this.git.id,
            repository: this.repository.name,
            owner: this.repository.owner,
            branch: this.branch.branchName,
            commitId: this.commit ? this.commit.commitId : 'HEAD',
          },
          EntryPoint: this.form.entryPoint,
          Cpu: this.resource.cpu,
          Memory: this.resource.memory,
          Gpu: this.resource.gpu,
          Options: options,
          Zip: this.form.zip,
          Partition: this.partition,
          Memo: this.form.memo,
        }
        await this['training/post'](params)
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      }
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
        case 3:
          form = this.$refs.form3
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

    // データセット
    async selectDataSet(dataSetId) {
      if (dataSetId == null) {
        this['dataSet/clearDetail']()
      } else {
        await this['dataSet/fetchDetail'](dataSetId)
      }
    },

    // 親ジョブ
    async selectParent(trainingId) {
      if (trainingId == null) {
        this['training/clearParent']()
      } else {
        await this['training/fetchParent'](trainingId)
      }
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

    // リソース
    selectResource(resource) {
      this['resource/setResource'](resource)
      this.form.resource = this.resource
    },

    // パーティション
    selectPartition(partition) {
      this['cluster/setPartition'](partition)
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
