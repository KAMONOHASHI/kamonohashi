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
            v-model="form.containerImage"
            :disabled="isPatch"
            :registries="registries"
            :images="images"
            :tags="tags"
            @selectRegistry="selectRegistry"
            @selectImage="selectImage"
          />
          <kqi-git-selector
            v-model="form.gitModel"
            :disabled="isPatch"
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
import { mapActions, mapGetters } from 'vuex'

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

    // 指定に必要な情報を取得
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

      // レジストリの設定
      this.form.containerImage.registry = {
        id: this.detail.containerImage.registryId,
        name: this.detail.containerImage.name,
      }
      await this.selectRegistry(this.detail.containerImage.registryId)
      this.form.containerImage.image = this.detail.containerImage.image
      await this.selectImage()
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
      this.form.gitModel.commit = this.detail.gitModel.commitId

      this.form.resource.cpu = this.detail.cpu
      this.form.resource.memory = this.detail.memory
      this.form.resource.gpu = this.detail.gpu
    }
  },
  methods: {
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
                  registryId: this.form.containerImage.registry.id,
                  image: this.form.containerImage.image,
                  tag: this.form.containerImage.tag,
                }
              }
              let gitModel = null
              if (this.repository !== null && this.branch !== null) {
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
