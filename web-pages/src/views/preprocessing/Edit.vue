<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deletePreprocessing"
    @close="$emit('cancel')"
  >
    <el-row v-if="isEditDialog" type="flex" justify="end">
      <el-col :span="24" class="right-button-group">
        <el-button @click="$emit('copy', id)">コピー</el-button>
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
        <el-input v-model="form.name" />
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
        <el-input v-model="form.memo" type="textarea" />
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
          <kqi-resource-selector v-model="form.resource" />
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
import gitSelectorUtil from '@/util/gitSelectorUtil'
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
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[2] // ["", "preprocessing", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = '前処理作成'
          this.isCreateDialog = true
          this.isCopyCreation = this.id !== null
          this.isEditDialog = false
          this.isPatch = false
          break
        case 'edit':
          this.title = '前処理編集'
          this.isCreateDialog = false
          this.isCopyCreation = false
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

        // 編集対象/コピー元でレジストリが指定されていればその情報をコピー
        if (this.detail.containerImage !== null) {
          this.form.containerImage.registry = {
            id: this.detail.containerImage.registryId,
            name: this.detail.containerImage.name,
          }
          this.form.containerImage.registry = this.registries.find(registry => {
            return registry.id === this.detail.containerImage.registryId
          })
          await this.selectRegistry(this.detail.containerImage.registryId)
          this.form.containerImage.image = this.detail.containerImage.image
          await this.selectImage()
          this.form.containerImage.tag = this.detail.containerImage.tag
        }

        // 編集対象/コピー元でgitが指定されていればその情報をコピー
        if (this.detail.gitModel !== null) {
          this.form.gitModel.git = {
            id: this.detail.gitModel.gitId,
            name: this.detail.gitModel.name,
          }
          this.form.gitModel.git = this.gits.find(git => {
            return git.id === this.detail.gitModel.gitId
          })
          await this.selectGit(this.detail.gitModel.gitId)
          this.form.gitModel.repository = `${this.detail.gitModel.owner}/${this.detail.gitModel.repository}`
          await this.selectRepository(this.form.gitModel.repository)
          this.form.gitModel.branch = this.branches.find(branch => {
            return branch.branchName == this.detail.gitModel.branch
          })
          await this.selectBranch(this.detail.gitModel.branch)
          // commitIdがHEADの場合はHEADを指定。それ以外の場合はcommitsから該当commitを抽出
          if (this.detail.gitModel.commitId === 'HEAD') {
            this.form.gitModel.commit = 'HEAD'
          } else {
            this.form.gitModel.commit = this.commits.find(commit => {
              return commit.commitId == this.detail.gitModel.commitId
            })
          }
        }

        this.form.resource.cpu = this.detail.cpu
        this.form.resource.memory = this.detail.memory
        this.form.resource.gpu = this.detail.gpu
      }
    },
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            if (this.isPatch) {
              // 名称・メモ・リソースのみ更新
              await this.patchPreprocessing()
            } else {
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

            this.$emit('done')
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
        this.$emit('done', 'delete')
      } catch (e) {
        this.error = e
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
