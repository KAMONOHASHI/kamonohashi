<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteTemplate"
    @close="$emit('cancel')"
  >
    <el-form
      ref="createForm"
      :model="form"
      :rules="rules"
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <kqi-display-error :error="error" />
      <kqi-display-text-form v-if="isEditDialog" label="ID" :value="id" />
      <el-form-item label="テンプレート名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="説明文" prop="memo">
        <el-input v-model="form.memo" type="textarea" />
      </el-form-item>
      <el-form-item label="実行コマンド" prop="entryPoint">
        <el-input
          v-model="form.entryPoint"
          type="textarea"
          :autosize="{ minRows: 2 }"
          :disabled="isPatch"
        />
      </el-form-item>
      <el-row :gutter="20">
        <el-col :span="12">
          <kqi-container-selector
            v-model="form.containerImage"
            :disabled="isPatch"
            :registries="registries"
            :images="images"
            :tags="tags"
            heading="前処理実行コンテナイメージ"
            @selectRegistry="selectRegistry"
            @selectImage="selectImage"
          />
          <kqi-container-selector
            v-model="form.containerImage"
            :disabled="isPatch"
            :registries="registries"
            :images="images"
            :tags="tags"
            heading="学習・推論コンテナイメージ"
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
            heading="スクリプト"
            @selectGit="selectGit"
            @selectRepository="selectRepository"
            @selectBranch="selectBranch"
          />
        </el-col>
        <el-col :span="12">
          <kqi-resource-selector v-model="form.resource" :quota="quota" />
        </el-col>
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
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
    KqiDialog,
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
      detail: ['preprocessing/detail'],
      histories: ['preprocessing/histories'],
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
      'modelTemplate/fetchDetail',
      'modelTemplate/post',
      'modelTemplate/put',
      'modelTemplate/delete',
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
                // HEAD指定の時はcommitsの先頭要素をcommitIDに指定する。コピー実行時の再現性を担保するため
                gitModel = {
                  gitId: this.form.gitModel.git.id,
                  repository: this.form.gitModel.repository.name,
                  owner: this.form.gitModel.repository.owner,
                  branch: this.form.gitModel.branch.branchName,
                  commitId: this.form.gitModel.commit
                    ? this.form.gitModel.commit.commitId
                    : this.commits[0].commitId,
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
                await this['model-template/post'](params)
              } else {
                // 編集
                await this['model-template/put']({
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
