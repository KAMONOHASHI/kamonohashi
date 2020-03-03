<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deletePreprocessing"
    @close="emitCancel"
  >
    <el-row v-if="isEditDialog" type="flex" justify="end">
      <el-col :span="24" class="right-button-group">
        <el-button @click="emitCopy">コピー</el-button>
      </el-col>
    </el-row>

    <el-form
      ref="createForm"
      :model="form"
      :rules="rules"
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <kqi-display-error :error="error" />
      <kqi-display-text-form v-if="isEditDialog" label="ID" :value="id" />
      <el-form-item label="前処理名" prop="name">
        <el-input v-model="form.name"> </el-input>
      </el-form-item>
      <el-form-item label="実行コマンド" prop="entryPoint">
        <el-input
          v-model="form.entryPoint"
          type="textarea"
          :autosize="{ minRows: 2 }"
          :disabled="isPatch"
        />
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="form.memo" type="textarea"> </el-input>
      </el-form-item>
      <el-row :gutter="20">
        <el-col :span="12">
          <kqi-container-selector
            :disabled="isPatch"
            @input="selectContainer"
          />
          <kqi-git-selector :disabled="isPatch" @input="selectModel" />
        </el-col>
        <el-col :span="12">
          <kqi-resource-selector
            v-model="form.resource"
          ></kqi-resource-selector>
        </el-col>
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiContainerSelector from '@/components/selector/KqiContainerSelector.vue'
import KqiGitSelector from '@/components/selector/KqiGitSelector.vue'
import KqiDisplayError from '@/components/KqiDisplayError'
import { mapActions, mapMutations, mapGetters } from 'vuex'

export default {
  components: {
    KqiDialog,
    KqiDisplayTextForm,
    KqiDisplayError,
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
      form: {
        name: null,
        entryPoint: null,
        memo: null,
        resource: {
          cpu: 1,
          memory: 1,
          gpu: 0,
        },
      },
      title: '',
      dialogVisible: true,
      error: null,
      isCreateDialog: false,
      isCopyCreation: false,
      isEditDialog: false,
      isPatch: false, // 利用済み前処理の場合name, memo, resourceのみ更新可能

      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
    }
  },
  computed: {
    ...mapGetters({
      registry: ['registrySelector/registry'],
      image: ['registrySelector/image'],
      tag: ['registrySelector/tag'],
      git: ['gitSelector/git'],
      repository: ['gitSelector/repository'],
      branch: ['gitSelector/branch'],
      commit: ['gitSelector/commit'],
      detail: ['preprocessing/detail'],
      histories: ['preprocessing/histories'],
    }),
  },

  async created() {
    let url = this.$route.path
    let type = url.split('/')[2] // ["", "preprocessing", "{type}", "{id}"]
    switch (type) {
      case 'create':
        this.title = '前処理作成'
        this.isCreateDialog = true
        this.isCopyCreation = this.id !== null
        break
      case 'edit':
        this.title = '前処理編集'
        this.isEditDialog = true
        break
    }

    // vuexの情報をリセット
    await this.selectContainer({
      type: 'registry',
      value: null,
    })
    await this.selectModel({
      type: 'git',
      value: null,
    })

    // 指定に必要な情報を取得
    await this['registrySelector/fetchRegistries']()
    await this['registrySelector/fetchImages']()
    await this['gitSelector/fetchGits']()
    await this['gitSelector/fetchRepositories']()

    // 編集時で既に前処理で利用されている場合は、patchフラグを立てる
    if (this.isEditDialog) {
      await this['preprocessing/fetchHistories'](this.id)
      if (this.histories.length > 0) {
        this.isPatch = true
      }
    }
    // 編集時/コピー実行時は、既に登録されている情報を各項目を設定
    if (this.isEditDialog || this.isCopyCreation) {
      await this['preprocessing/fetchDetail'](this.id)

      this.form.name = this.detail.name
      this.form.entryPoint = this.detail.entryPoint
      this.form.memo = this.detail.memo

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

      this.form.resource.cpu = this.detail.cpu
      this.form.resource.memory = this.detail.memory
      this.form.resource.gpu = this.detail.gpu
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
      'preprocessing/fetchDetail',
      'preprocessing/post',
      'preprocessing/put',
      'preprocessing/patch',
      'preprocessing/fetchHistories',
      'preprocessing/delete',
      'registrySelector/fetchRegistries',
      'registrySelector/fetchImages',
      'registrySelector/fetchTags',
      'gitSelector/fetchGits',
      'gitSelector/fetchRepositories',
      'gitSelector/fetchBranches',
      'gitSelector/fetchCommits',
    ]),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            if (this.isPatch) {
              // 名称・メモ・リソースのみ更新
              await this.patchPreprocessing()
            } else {
              let containerImage = null
              if (this.image !== null && this.tag !== null) {
                containerImage = {
                  registryId: this.registry.id,
                  image: this.image,
                  tag: this.tag,
                }
              }
              let gitModel = null
              if (this.repository !== null && this.branch !== null) {
                gitModel = {
                  gitId: this.git.id,
                  repository: this.repository.name,
                  owner: this.repository.owner,
                  branch: this.branch.branchName,
                  commitId: this.commit ? this.commit : 'HEAD',
                }
              }
              let params = {
                name: this.form.name,
                entryPoint: this.form.entryPoint,
                ContainerImage: containerImage,
                GitModel: gitModel,
                memo: this.form.memo,
                cpu: this.form.resource.cpu,
                memory: this.form.resource.memory,
                gpu: this.form.resource.gpu,
              }
              if (this.isCreateDialog) {
                // 新規作成
                await this['preprocessing/post'](params)
              } else {
                // 編集
                await this['preprocessing/put']({
                  id: this.id,
                  params: params,
                })
              }
            }

            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    async patchPreprocessing() {
      let params = {
        name: this.form.name,
        memo: this.form.memo,
        cpu: this.form.resource.cpu,
        memory: this.form.resource.memory,
        gpu: this.form.resource.gpu,
      }
      await this['preprocessing/patch']({
        id: this.id,
        params: params,
      })
    },
    async deletePreprocessing() {
      try {
        await this['preprocessing/delete'](this.id)
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },
    closeDialog(done) {
      if (done) {
        done()
      }
      this.emitCancel()
    },
    emitCopy() {
      this.$emit('copy', this.id)
    },
    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
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
</style>
