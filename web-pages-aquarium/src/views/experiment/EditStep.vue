<template>
  <div>
    <el-row :gutter="20"><h2>新しいモデルのトレーニング</h2></el-row>
    <el-row :gutter="20"> <h2>（A工場●●分類）</h2></el-row>
    <el-row :gutter="20">
      <el-col :span="20">
        <el-form
          ref="createForm"
          :model="form"
          :rules="rules"
          element-loading-background="rgba(255, 255, 255, 0.7)"
        >
          <!-- step 1 -->
          <div class="step-title">
            <div class="el-step__icon is-text ">
              <div class="el-step__icon-inner">1</div>
            </div>
            モデルの定義
          </div>
          <el-form
            ref="form0"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 0px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <kqi-display-error :error="error" />
            <kqi-display-text-form v-if="isEditDialog" label="ID" :value="id" />

            <el-form-item label="モデル名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <el-button>続行</el-button>
          </el-form>
          <!-- step 2 -->
          <div class="step-title">
            <div class="el-step__icon is-text">
              <div class="el-step__icon-inner">2</div>
            </div>
            データセットの選択
          </div>
          <el-form
            ref="form1"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 0px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <el-button>データセットを選択</el-button>
            <el-form-item label="" prop="dataset">
              <el-input v-model="form.dataset" />
            </el-form-item>
          </el-form>
          <!-- step 3 -->
          <div class="step-title">
            <div class="el-step__icon is-text">
              <div class="el-step__icon-inner">3</div>
            </div>
            リソースの選択
          </div>
          <el-form
            ref="form2"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 10px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <el-radio v-model="selectResource" label="1"
              >デフォルトのリソースを使用</el-radio
            ><br />
            <el-radio v-model="selectResource" label="2"
              >手動でリソース量を設定</el-radio
            >
          </el-form>
          <span>
            <el-button type="primary" @click="submit">
              トレーニングを開始する
            </el-button>
          </span>
        </el-form>
      </el-col>
    </el-row>
    <el-row class="step"> </el-row>
  </div>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import registrySelectorUtil from '@/util/registrySelectorUtil'
import gitSelectorUtil from '@/util/gitSelectorUtil'
import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      selectResource: 0,
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
      //TODO
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
                await this['experiment/post'](params)
              } else {
                // 編集
                await this['experiment/put']({
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

.step-title {
  font-weight: bold !important;
  color: #409eff;
  border-color: #409eff;
}
</style>
