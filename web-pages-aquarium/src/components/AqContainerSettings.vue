<!-- eslint-disable vue/no-mutating-props -->
<!-- 
  KQIではコンテナの情報設定で使用するリポジトリ一覧、ブランチ一覧などはVuexのストアを用いて管理しているが、
  アクアリウムではコンポーネント内で直接apiを呼び値を保持することにする。

  理由
  KQIではリポジトリ一覧、ブランチ一覧などは1画面あたり1データしか持たないが、アクアリウムのテンプレート作成と
  テンプレート登録では、前処理、学習、推論の3項目についてそれぞれ独立した値を保持する必要がある。
  そのため、Vuexのストアを用いると意図しない参照が起こるため。
-->
<!--
  多数の@ts-ignoreで型エラーを回避しているのは
  computed句に「...mapGetters」以外の変数があるせいか、
  「this.form」などの箇所がすべて型エラーとなるため。
  当初はthisを(this as any)で回避しようとしたが別のエラーが残り使用できなかった。
-->
<template>
  <div>
    <el-form :model="form">
      <el-row type="flex" style="padding-bottom:10px">
        <el-col>
          <div v-if="formType == '前処理'">
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('train')"
            >
              入力内容を学習からコピー
            </el-button>
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('evaluation')"
            >
              入力内容を推論からコピー
            </el-button>
          </div>
          <div v-if="formType == '学習'">
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('preprocessing')"
            >
              入力内容を前処理からコピー
            </el-button>
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('evaluation')"
            >
              入力内容を推論からコピー
            </el-button>
          </div>
          <div v-if="formType == '推論'">
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('preprocessing')"
            >
              入力内容を前処理からコピー
            </el-button>
            <el-button
              icon="el-icon-document-copy"
              type="info"
              plain
              size="mini"
              @click="copy('train')"
            >
              入力内容を学習からコピー
            </el-button>
          </div>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="12">
          <kqi-container-selector
            v-model="form.containerImage"
            :registries="registries"
            :images="images"
            :tags="tags"
            heading="Dockerレジストリ"
            @selectRegistry="selectRegistry"
            @selectImage="selectImage"
          />
          <el-row>
            <el-col :span="6" :offset="1">token</el-col>
            <el-col :span="12">
              <el-input
                v-model="form.containerImage.token"
                size="small"
                type="password"
                :style="!createTemplate ? 'width:215px' : ''"
              />
            </el-col>
          </el-row>
          <kqi-git-selector
            v-model="form.gitModel"
            :gits="gits"
            :repositories="repositories"
            :branches="branches"
            :commits="commitsList"
            :loading-repositories="loadingRepositories"
            heading="Gitサービス"
            @selectGit="selectGit"
            @selectRepository="selectRepository"
            @selectBranch="selectBranch"
            @searchCommitId="searchCommitId"
            @getMoreCommits="getMoreCommits"
          />

          <el-row>
            <el-col :span="6" :offset="1">token</el-col>
            <el-col :span="12">
              <el-input
                v-model="form.gitModel.token"
                size="small"
                type="password"
                :style="!createTemplate ? 'width:215px' : ''"
              />
            </el-col>
          </el-row>
          <el-form-item label="実行コマンド" prop="entryPoint">
            <el-input
              v-model="form.entryPoint"
              type="textarea"
              :autosize="{ minRows: 2 }"
              :disabled="isPatch"
            />
          </el-form-item>
        </el-col>

        <el-col :span="createTemplate ? 12 : 6">
          <kqi-resource-selector v-model="form.resource" :quota="quota" />
        </el-col>
      </el-row>
    </el-form>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'

import KqiContainerSelector from '@/components/selector/KqiContainerSelector.vue'
import KqiGitSelector from '@/components/selector/KqiGitSelector.vue'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector.vue'
import api from '@/api/api'
import { mapActions, mapGetters } from 'vuex'
import * as gen from '@/api/api.generate'
type ContainerImage = {
  registry:
    | string
    | null
    | {
        id: number
        name: string
      }
  registryId?: number
  image: string | null
  tag: string | null
  token: string | null
}
type GitModel = {
  git: null | gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialOutputModel
  gitId?: number
  repository: null | gen.NssolPlatypusServiceModelsGitRepositoryModel | string
  owner?: string | null
  branch: null | gen.NssolPlatypusServiceModelsGitBranchModel | string
  commit: null | gen.NssolPlatypusServiceModelsGitCommitModel
  token: null | string
  commitId?: string
}

interface DataType {
  commitsList: Array<gen.NssolPlatypusServiceModelsGitCommitModel>
  commitsPage: number
  rules: {
    containerImage: Array<{
      required: boolean
      trigger: string
      message: string
    }>
  }
  error: null | Error
  isPatch: boolean
  images: Array<string>
  tags: Array<string>
  repositories: Array<gen.NssolPlatypusServiceModelsGitRepositoryModel>
  branches: Array<gen.NssolPlatypusServiceModelsGitBranchModel>
  commits: Array<gen.NssolPlatypusServiceModelsGitCommitModel>
  commitDetail: null | gen.NssolPlatypusServiceModelsGitCommitModel
  updateValue: boolean
}

export default Vue.extend({
  components: {
    KqiContainerSelector,
    KqiGitSelector,
    KqiResourceSelector,
  },
  props: {
    value: {
      type: Object as PropType<{
        containerImage: ContainerImage | null
        gitModel: GitModel | null
        entryPoint: string
        resource: {
          cpu: number
          memory: number
          gpu: number
        }
      }>,
      default: (): {
        containerImage: ContainerImage | null
        gitModel: GitModel | null
        entryPoint: string
        resource: {
          cpu: number
          memory: number
          gpu: number
        }
      } => {
        return {
          containerImage: {
            registry: null,
            image: null,
            tag: null,
            token: null,
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
            token: null,
          },
          entryPoint: '',
          resource: {
            cpu: 1,
            memory: 1,
            gpu: 1,
          },
        }
      },
    },
    // 必須 (学習) -> true, 必須でない (前処理、推論) -> false
    requiredForm: {
      type: Boolean,
      default: false,
    },
    // 新規作成 -> true, テンプレート詳細画面 -> false
    createTemplate: {
      type: Boolean,
      default: false,
    },
    // 学習、前処理、推論などの画面上の名称
    formType: {
      type: String,
      default: '',
    },
  },
  data(): DataType {
    return {
      commitsList: [],
      commitsPage: 1,
      rules: {
        containerImage: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
      },
      error: null,
      isPatch: false,
      images: [],
      tags: [],
      repositories: [],
      branches: [],
      commits: [],
      commitDetail: null,
      updateValue: false,
    }
  },
  computed: {
    // レジストリサーバ、gitサーバ周りはコンテナ単位で共通のためstoreされた値を使用する
    form: {
      get() {
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      },
    },
    ...mapGetters({
      //@ts-ignore
      registries: ['registrySelector/registries'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      quota: ['cluster/quota'],
      loadingRepositories: ['gitSelector/loadingRepositories'],
      searchCommitDetail: ['gitSelector/commitDetail'],
    }),
  },
  watch: {
    async value() {
      this.updateValue = true
      this.retrieveData()
    },
  },

  async created() {
    this.retrieveData()
  },
  methods: {
    async retrieveData() {
      // クォータ情報、レジストリサーバー一覧、gitサーバー一覧を取得
      await this['cluster/fetchQuota']()
      await this['registrySelector/fetchRegistries']()
      await this['gitSelector/fetchGits']()

      // 新規作成の場合レジストリサーバーとgitサーバーのみ設定されたデフォルトを使用し、
      // 残りの項目はnullのままにする
      if (this.createTemplate) {
        // 更新フラグがfalseの時、新規作成と判断する。（formのコピーは実行されていない）
        if (!this.updateValue) {
          //@ts-ignore
          this.form.containerImage.registry = this.registries.find(
            (
              registry: gen.NssolPlatypusApiModelsAccountApiModelsRegistryCredentialOutputModel,
            ) => {
              //@ts-ignore
              return registry.id === this.defaultRegistryId
            },
          )
          //@ts-ignore
          this.form.gitModel.git = this.gits.find(
            (
              git: gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialOutputModel,
            ) => {
              //@ts-ignore
              return git.id === this.defaultGitId
            },
          )
        }
      }

      // 以下はテンプレート詳細画面で開かれた場合
      const gitModel = { ...this.value.gitModel! }
      const containerImage = { ...this.value.containerImage! }

      await this.setupFromContainerImage(containerImage)
      //@ts-ignore
      await this.setupFormGitModel(gitModel)
      //@ts-ignore
      this.form.entryPoint = this.value.entryPoint
      //@ts-ignore
      this.form.resource = { ...this.value.resource }
      this.updateValue = false
    },
    copy(from: string) {
      let formtype = ''
      if (this.formType == '前処理') {
        formtype = 'preprocessing'
      } else if (this.formType == '学習') {
        formtype = 'train'
      } else if (this.formType == '推論') {
        formtype = 'evaluation'
      }
      // 更新フラグをtrueにする
      this.updateValue = true
      // コミットページを元に戻す
      this.commitsPage = 1
      this.$emit('copy', { from: from, to: formtype })
    },
    // テンプレート詳細画面の場合にコンテナイメージの初期情報を設定する
    async setupFromContainerImage(containerImage: ContainerImage) {
      //@ts-ignore
      this.form.containerImage = {
        registry: null,
        image: null,
        tag: null,
        token: containerImage.token,
      }

      // 前処理や評価で指定されていなかった場合はregistryだけ設定
      if (
        containerImage === null ||
        Object.keys(containerImage).length == 0 ||
        containerImage.registry === null
      ) {
        //@ts-ignore
        this.form.containerImage.registry = this.registries.find(
          (
            registry: gen.NssolPlatypusApiModelsAccountApiModelsRegistryCredentialOutputModel,
          ) => {
            //@ts-ignore
            return registry.id == this.defaultRegistryId
          },
        )
        return
      }

      // レジストリの初期値を選択
      if (containerImage.registryId != null) {
        await this.selectRegistry(containerImage.registryId)
      } else {
        if (typeof containerImage.registry != 'string') {
          await this.selectRegistry(containerImage.registry.id!)
        }
      }
      await this.selectImage(containerImage.image)
      // タグ一覧に該当のタグがない場合、タグを追加して選択
      if (
        !this.tags.includes(containerImage.tag!) &&
        containerImage.tag !== null
      ) {
        this.tags.push(containerImage.tag)
      }
      //@ts-ignore
      this.form.containerImage.tag = containerImage.tag
    },
    // テンプレート詳細画面の場合にgit情報の詳細を設定する
    async setupFormGitModel(gitModel: GitModel) {
      //@ts-ignore
      this.form.gitModel = {
        git: null,
        repository: null,
        branch: null,
        commit: null,
        token: gitModel.token,
      }

      // gitModelが空 もしくは リポジトリが設定されていない場合はデフォルトのgitサーバーだけ登録して返却
      if (
        gitModel === null ||
        Object.keys(gitModel).length == 0 ||
        gitModel.git === null
      ) {
        //@ts-ignore
        this.form.gitModel.git = this.gits.find(
          (
            git: gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialOutputModel,
          ) => {
            //@ts-ignore
            return git.id == this.defaultGitId
          },
        )
        return
      }
      let commitId: string | null = null
      if (gitModel.gitId != null) {
        await this.selectGit(gitModel.gitId)
        await this.selectRepository(gitModel.owner + '/' + gitModel.repository)
        await this.selectBranch(gitModel.branch as string)
        commitId = gitModel.commitId!
      } else {
        await this.selectGit(gitModel.git.id!)
        if (typeof gitModel.repository != 'string') {
          await this.selectRepository(
            gitModel.repository!.owner + '/' + gitModel.repository!.name,
          )
        }
        await this.selectBranch(
          (gitModel.branch as gen.NssolPlatypusServiceModelsGitBranchModel)
            .branchName!,
        )
        commitId = gitModel.commit!.commitId!
      }

      // commitを抽出
      let commit = null
      if (this.commits != null) {
        commit = this.commits.find(commit => {
          return commit.commitId === commitId
        })
      }

      if (commit) {
        //@ts-ignore
        this.form.gitModel.commit = commit
      } else {
        // コミット一覧に含まれないコミットなので、コミット情報を新たに取得する
        let params = {
          //@ts-ignore
          gitId: this.form.gitModel.git.id,
          //@ts-ignore
          owner: this.form.gitModel.repository.owner,
          //@ts-ignore
          repositoryName: this.form.gitModel.repository.name,
          commitId: commitId,
        }

        //@ts-ignore
        let commitDetail = ((await api.git.getCommit(params)).data(
          this as any,
          //@ts-ignore
        ).form.gitModel.commit = commitDetail)
      }
    },
    // レジストリサーバーを選択する
    async selectRegistry(registryId: null | number) {
      // 過去の選択を削除

      //@ts-ignore
      this.form.containerImage.registry = null
      //@ts-ignore
      this.form.containerImage.image = null(
        this as any,
      ).form.containerImage.tag = null
      this.images = []
      this.tags = []

      // 選択したレジストリをフォームに設定し、イメージの一覧をapi経由で取得する
      if (registryId !== null) {
        //@ts-ignore
        this.form.containerImage.registry = this.registries.find(
          (registry: { id: number }) => {
            return registry.id == registryId
          },
        )
        this.images = (
          await api.registry.getImages({ registryId: registryId })
        ).data
      }
    },
    // イメージを選択
    async selectImage(image: null | string) {
      // 過去の選択を削除

      //@ts-ignore
      this.form.containerImage.image = null(
        this as any,
      ).form.containerImage.tag = null
      this.tags = []

      // クリアされた場合は何も設定しない
      if (image === null || image === '') {
        return
      }

      // イメージ一覧に該当のimageがない場合、イメージのリストに追加されたイメージを追加
      if (!this.images.includes(image)) {
        this.images.push(image)
      }
      //@ts-ignore
      this.form.containerImage.image = image

      // tagの一覧をapi経由で取得する
      let params = {
        //@ts-ignore
        registryId: this.form.containerImage.registry.id,
        image: image,
      }
      this.tags = (await api.registry.getTags(params)).data
    },
    // gitサーバーを選択
    async selectGit(gitId: null | number) {
      // 過去の選択をリセット
      //@ts-ignore
      this.form.gitModel.git = null
      //@ts-ignore
      this.form.gitModel.repository = null
      //@ts-ignore
      this.form.gitModel.branch = null
      //@ts-ignore
      this.form.gitModel.commit = null
      this.repositories = []
      this.branches = []
      this.commits = []

      // クリアされた場合は何もしない
      // そうでない場合はgitサーバーを設定してapi経由でリポジトリ一覧を取得
      if (gitId !== null) {
        //@ts-ignore
        this.form.gitModel.git = this.gits.find((git: { id: number }) => {
          return git.id == gitId
        })
        this.repositories = (await api.git.getRepos({ gitId: gitId })).data
      }
    },
    // リポジトリを選択
    async selectRepository(repository: string | null) {
      // 過去の選択をリセット
      //@ts-ignore
      this.form.gitModel.repository = null
      //@ts-ignore
      this.form.gitModel.branch = null
      //@ts-ignore
      this.form.gitModel.commit = null
      this.branches = []
      this.commits = []

      if (repository === null || repository === '') {
        return
      }

      // repositoryの設定
      // repositoryの型がstring：手入力, object: 選択でstringの場合はobjectに変換する
      let argRepository = null
      if (typeof repository === 'string') {
        let repositoryName = repository
        let index = repositoryName.indexOf('/')
        if (index > 0) {
          argRepository = {
            owner: repositoryName.substring(0, index),
            name: repositoryName.substring(index + 1),
            fullName: repositoryName,
          }
          // リポジトリ一覧に該当のリポジトリがない場合、リポジトリのリストに追加されたリポジトリを追加
          if (!this.repositories.find(r => r.fullName == repositoryName)) {
            this.repositories.push(argRepository)
          }
        } else {
          // 構文エラー
          //@ts-ignore
          this.$notify.error({
            message:
              '{owner}/{name}の形式で入力してください。例：KAMONOHASHI/tutorial',
          })
          return
        }
      } else {
        argRepository = repository
      }
      //@ts-ignore
      this.form.gitModel.repository = argRepository

      // ブランチの一覧をapi経由で取得する
      let params = {
        //@ts-ignore
        gitId: this.form.gitModel.git.id,
        owner: argRepository.owner,
        repositoryName: argRepository.name,
      }
      this.branches = (await api.git.getBranches(params)).data
    },
    // ブランチを選択
    async selectBranch(branchName: null | string) {
      // コミットページを元に戻す
      //@ts-ignore
      this.commitsPage = 1
      // 過去の選択をリセット
      //@ts-ignore
      this.form.gitModel.branch = null

      //@ts-ignorethis.form.gitModel.commit = null
      this.commits = []

      // クリアでない場合には設定してコミット一覧を取得する
      if (branchName !== null) {
        //@ts-ignore
        this.form.gitModel.branch = this.branches.find(
          (branch: gen.NssolPlatypusServiceModelsGitBranchModel) => {
            return branch.branchName == branchName
          },
        )
        let params = {
          //@ts-ignore
          gitId: this.form.gitModel.git.id,
          //@ts-ignore
          owner: this.form.gitModel.repository.owner,
          //@ts-ignore
          repositoryName: this.form.gitModel.repository.name,
          branch: branchName,
          page: String(this.commitsPage),
        }
        this.commits = (await api.git.getCommits(params)).data
        this.commitsList = [...this.commits]
      }
    },
    async searchCommitId(commitId: string) {
      await this['gitSelector/fetchCommitDetail']({
        //@ts-ignore
        gitId: this.form.gitModel.git.id,
        //@ts-ignore
        repository: this.form.gitModel.repository,
        commitId: commitId,
      })

      //@ts-ignore
      if (this.searchCommitDetail != null) {
        //@ts-ignore
        this.form.gitModel.commit = this.searchCommitDetail
      }
    },

    async getMoreCommits() {
      this.commitsPage++
      let params = {
        //@ts-ignore
        gitId: this.form.gitModel.git.id,
        //@ts-ignore
        owner: this.form.gitModel.repository.owner,
        //@ts-ignore
        repositoryName: this.form.gitModel.repository.name,
        //@ts-ignore
        branch: this.form.gitModel.branch.branchName,
        page: String(this.commitsPage),
      }
      this.commits = (await api.git.getCommits(params)).data

      this.commitsList = this.commitsList.concat(this.commits)
    },
    // apiでテンプレートを登録するための入力値チェックを行い、formの中身を成形する
    async prepareSubmit() {
      // 入力値チェック
      // eslint-disable-next-line no-useless-catch
      try {
        if (this.requiredForm) {
          await this.prepareSubmitRequired()
        } else {
          await this.prepareSubmitNotRequired()
        }
      } catch (message) {
        // 入力値に異常があった場合はエラーをそのまま投げる
        throw message
      }

      // 入力なしの場合はnullを返却する
      //@ts-ignore
      if (this.form.containerImage.image === null) {
        return {
          entryPoint: null,
          containerImage: null,
          gitModel: null,
          //@ts-ignore
          cpu: this.form.resource.cpu,
          //@ts-ignore
          memory: this.form.resource.memory,
          //@ts-ignore
          gpu: this.form.resource.gpu,
        }
      }

      // フォームを成形して返却
      return {
        //@ts-ignore
        entryPoint: this.form.entryPoint,
        containerImage: {
          //@ts-ignore
          token: this.form.containerImage.token,
          //@ts-ignore
          registryId: this.form.containerImage.registry.id,
          //@ts-ignore
          image: this.form.containerImage.image,
          //@ts-ignore
          tag: this.form.containerImage.tag,
        },
        gitModel: {
          //@ts-ignore
          token: this.form.gitModel.token,
          //@ts-ignore
          gitId: this.form.gitModel.git.id,
          //@ts-ignore
          repository: this.form.gitModel.repository.name,
          //@ts-ignore
          owner: this.form.gitModel.repository.owner,
          //@ts-ignore
          branch: this.form.gitModel.branch.branchName,
          commitId:
            //@ts-ignore
            this.form.gitModel.commit === null
              ? this.commitsList[0].commitId
              : //@ts-ignore
                this.form.gitModel.commit.commitId,
        },
        //@ts-ignore
        cpu: this.form.resource.cpu,
        //@ts-ignore
        memory: this.form.resource.memory,
        //@ts-ignore
        gpu: this.form.resource.gpu,
      }
    },
    // 必須項目 (学習) に関する入力値チェック
    async prepareSubmitRequired() {
      if (
        // 学習コンテナイメージ設定
        //@ts-ignore
        this.form.containerImage === null ||
        //@ts-ignore
        this.form.containerImage.registry === null ||
        //@ts-ignore
        this.form.containerImage.image === null ||
        //@ts-ignore
        this.form.containerImage.tag === null ||
        // 学習Git設定
        //@ts-ignore
        this.form.gitModel === null ||
        //@ts-ignore
        this.form.gitModel.git === null ||
        //@ts-ignore
        this.form.gitModel.repository === null ||
        //@ts-ignore
        this.form.gitModel.branch === null ||
        // 実行コマンド
        //@ts-ignore
        this.form.entryPoint === null ||
        //@ts-ignore
        this.form.entryPoint === ''
      ) {
        throw '必須項目が入力されていません : ' +
          this.formType +
          ' の設定は必須項目です。'
      }
    },
    // 任意項目 (前処理、評価) に関する入力値チェック
    async prepareSubmitNotRequired() {
      if (
        //@ts-ignore
        (this.form.containerImage.image !== null ||
          //@ts-ignore
          this.form.gitModel.repository !== null ||
          //@ts-ignore
          (this.form.entryPoint !== null && this.form.entryPoint !== '')) &&
        // コンテナイメージ設定
        //@ts-ignore
        (this.form.containerImage === null ||
          //@ts-ignore
          this.form.containerImage.registry === null ||
          //@ts-ignore
          this.form.containerImage.image === null ||
          //@ts-ignore
          this.form.containerImage.tag === null ||
          //@ts-ignore
          // Git設定
          this.form.gitModel === null ||
          //@ts-ignore
          this.form.gitModel.git === null ||
          //@ts-ignore
          this.form.gitModel.repository === null ||
          //@ts-ignore
          this.form.gitModel.branch === null ||
          //@ts-ignore
          // 実行コマンド
          this.form.entryPoint === null ||
          //@ts-ignore
          this.form.entryPoint === '')
      ) {
        throw this.formType +
          ' の設定を確認してください : イメージ、リポジトリ、実行コマンドのいずれかが入力済みなら他の項目も入力必須です。'
      }
    },
    ...mapActions([
      'registrySelector/fetchRegistries',
      'gitSelector/fetchGits',
      'cluster/fetchQuota',
      'gitSelector/fetchCommitDetail',
    ]),
  },
})
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
