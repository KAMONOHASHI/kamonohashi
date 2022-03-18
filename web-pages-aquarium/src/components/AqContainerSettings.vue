<!-- 
  KQIではコンテナの情報設定で使用するリポジトリ一覧、ブランチ一覧などはVuexのストアを用いて管理しているが、
  アクアリウムではコンポーネント内で直接apiを呼び値を保持することにする。

  理由
  KQIではリポジトリ一覧、ブランチ一覧などは1画面あたり1データしか持たないが、アクアリウムのテンプレート作成と
  テンプレート登録では、前処理、学習、推論の3項目についてそれぞれ独立した値を保持する必要がある。
  そのため、Vuexのストアを用いると意図しない参照が起こるため。
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

<script>
import KqiContainerSelector from '@/components/selector/KqiContainerSelector'
import KqiGitSelector from '@/components/selector/KqiGitSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import api from '@/api/api'
import { mapActions, mapGetters } from 'vuex'
export default {
  title: 'モデルテンプレート',
  components: {
    KqiContainerSelector,
    KqiGitSelector,
    KqiResourceSelector,
  },
  props: {
    value: {
      type: Object,
      default: () => {
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
  data() {
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
    ...mapGetters({
      registries: ['registrySelector/registries'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      quota: ['cluster/quota'],
      loadingRepositories: ['gitSelector/loadingRepositories'],
      searchCommitDetail: ['gitSelector/commitDetail'],
    }),
    form: {
      get() {
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      },
    },
  },
  watch: {
    async value() {
      this.updateValue = true
      this.retrieveData()
    },
    'value.gitModel': async function() {
      await this.selectRepository(
        this.value.gitModelrepository.owner +
          '/' +
          this.value.gitModel.repository,
      )
      await this.selectBranch(this.value.gitModel.branch.branchName)
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
        this.form.containerImage.registry = this.registries.find(registry => {
          return registry.id === this.defaultRegistryId
        })
        this.form.gitModel.git = this.gits.find(git => {
          return git.id === this.defaultGitId
        })
        XMLHttpRequestEventTarget
        if (this.updateValue == false) {
          return
        }
      }

      // 以下はテンプレート詳細画面で開かれた場合
      const gitModel = { ...this.value.gitModel }
      const containerImage = { ...this.value.containerImage }

      await this.setupFromContainerImage(containerImage)
      await this.setupFormGitModel(gitModel)

      this.form.entryPoint = this.value.entryPoint
      this.form.resource = { ...this.value.resource }
      this.updateValue = false
    },
    copy(from) {
      let formtype = ''
      if (this.formType == '前処理') {
        formtype = 'preprocessing'
      } else if (this.formType == '学習') {
        formtype = 'train'
      } else if (this.formType == '推論') {
        formtype = 'evaluation'
      }

      this.$emit('copy', { from: from, to: formtype })
    },
    // テンプレート詳細画面の場合にコンテナイメージの初期情報を設定する
    async setupFromContainerImage(containerImage) {
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
        containerImage.image === null
      ) {
        this.form.containerImage.registry = this.registries.find(registry => {
          return registry.id == this.defaultRegistryId
        })
        return
      }

      // レジストリの初期値を選択
      if (containerImage.registryId != null) {
        await this.selectRegistry(containerImage.registryId)
      } else {
        await this.selectRegistry(containerImage.registry.id)
      }
      await this.selectImage(containerImage.image)
      // タグ一覧に該当のタグがない場合、タグを追加して選択
      if (!this.tags.includes(containerImage.tag)) {
        this.tags.push(containerImage.tag)
      }
      this.form.containerImage.tag = containerImage.tag
    },
    // テンプレート詳細画面の場合にgit情報の詳細を設定する
    async setupFormGitModel(gitModel) {
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
        gitModel.repository === null
      ) {
        this.form.gitModel.git = this.gits.find(git => {
          return git.id == this.defaultGitId
        })
        return
      }
      let commitId = null
      if (gitModel.gitId != null) {
        await this.selectGit(gitModel.gitId)
        await this.selectRepository(gitModel.owner + '/' + gitModel.repository)
        await this.selectBranch(gitModel.branch)
        commitId = gitModel.commitId
      } else {
        await this.selectGit(gitModel.git.id)
        await this.selectRepository(
          gitModel.repository.owner + '/' + gitModel.repository.name,
        )
        await this.selectBranch(gitModel.branch.branchName)
        commitId = gitModel.commit.commitId
      }

      // commitを抽出
      let commit = null
      if (this.commits != null) {
        commit = this.commits.find(commit => {
          return commit.commitId === commitId
        })
      }

      if (commit) {
        this.form.gitModel.commit = commit
      } else {
        // コミット一覧に含まれないコミットなので、コミット情報を新たに取得する
        let params = {
          gitId: this.form.gitModel.git.id,
          owner: this.form.gitModel.repository.owner,
          repositoryName: this.form.gitModel.repository.name,
          commitId: commitId,
        }
        let commitDetail = (await api.git.getCommit(params)).data
        this.form.gitModel.commit = commitDetail
      }
    },
    // レジストリサーバーを選択する
    async selectRegistry(registryId) {
      // 過去の選択を削除
      this.form.containerImage.registry = null
      this.form.containerImage.image = null
      this.form.containerImage.tag = null
      this.images = []
      this.tags = []

      // 選択したレジストリをフォームに設定し、イメージの一覧をapi経由で取得する
      if (registryId !== null) {
        this.form.containerImage.registry = this.registries.find(registry => {
          return registry.id == registryId
        })
        this.images = (
          await api.registry.getImages({ registryId: registryId })
        ).data
      }
    },
    // イメージを選択
    async selectImage(image) {
      // 過去の選択を削除
      this.form.containerImage.imatge = null
      this.form.containerImage.tag = null
      this.tags = []

      // クリアされた場合は何も設定しない
      if (image === null || image === '') {
        return
      }

      // イメージ一覧に該当のimageがない場合、イメージのリストに追加されたイメージを追加
      if (!this.images.includes(image)) {
        this.images.push(image)
      }
      this.form.containerImage.image = image

      // tagの一覧をapi経由で取得する
      let params = {
        registryId: this.form.containerImage.registry.id,
        image: image,
      }
      this.tags = (await api.registry.getTags(params)).data
    },
    // gitサーバーを選択
    async selectGit(gitId) {
      // 過去の選択をリセット
      this.form.gitModel.git = null
      this.form.gitModel.repository = null
      this.form.gitModel.branch = null
      this.form.gitModel.commit = null
      this.repositories = []
      this.branches = []
      this.commits = []

      // クリアされた場合は何もしない
      // そうでない場合はgitサーバーを設定してapi経由でリポジトリ一覧を取得
      if (gitId !== null) {
        this.form.gitModel.git = this.gits.find(git => {
          return git.id == gitId
        })
        this.repositories = (await api.git.getRepos({ gitId: gitId })).data
      }
    },
    // リポジトリを選択
    async selectRepository(repository) {
      // 過去の選択をリセット
      this.form.gitModel.repository = null
      this.form.gitModel.branch = null
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
          this.repositories.push(argRepository)
        } else {
          // 構文エラー
          this.$notify.error({
            message:
              '{owner}/{name}の形式で入力してください。例：KAMONOHASHI/tutorial',
          })
          return
        }
      } else {
        argRepository = repository
      }
      this.form.gitModel.repository = argRepository

      // ブランチの一覧をapi経由で取得する
      let params = {
        gitId: this.form.gitModel.git.id,
        owner: argRepository.owner,
        repositoryName: argRepository.name,
      }
      this.branches = (await api.git.getBranches(params)).data
    },
    // ブランチを選択
    async selectBranch(branchName) {
      this.pacommitsPagege = 1
      // 過去の選択をリセット
      this.form.gitModel.branch = null
      this.form.gitModel.commit = null
      this.commits = []

      // クリアでない場合には設定してコミット一覧を取得する
      if (branchName !== null) {
        this.form.gitModel.branch = this.branches.find(branch => {
          return branch.branchName == branchName
        })
        let params = {
          gitId: this.form.gitModel.git.id,
          owner: this.form.gitModel.repository.owner,
          repositoryName: this.form.gitModel.repository.name,
          branch: branchName,
          page: this.commitsPage,
        }
        this.commits = (await api.git.getCommits(params)).data
        this.commitsList = [...this.commits]
      }
    },
    async searchCommitId(commitId) {
      await this['gitSelector/fetchCommitDetail']({
        gitId: this.form.gitModel.git.id,
        repository: this.form.gitModel.repository,
        commitId: commitId,
      })

      if (this.searchCommitDetail != null) {
        this.form.gitModel.commit = this.searchCommitDetail
      }
    },

    async getMoreCommits() {
      this.commitsPage++
      let params = {
        gitId: this.form.gitModel.git.id,
        owner: this.form.gitModel.repository.owner,
        repositoryName: this.form.gitModel.repository.name,
        branch: this.form.gitModel.branch.branchName,
        page: this.commitsPage,
      }
      this.commits = (await api.git.getCommits(params)).data

      this.commitsList = this.commitsList.concat(this.commits)
    },
    // apiでテンプレートを登録するための入力値チェックを行い、formの中身を成形する
    async prepareSubmit() {
      // 入力値チェック
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
      if (this.form.containerImage.image === null) {
        return {
          entryPoint: null,
          containerImage: null,
          gitModel: null,
          cpu: this.form.resource.cpu,
          memory: this.form.resource.memory,
          gpu: this.form.resource.gpu,
        }
      }

      // フォームを成形して返却
      return {
        entryPoint: this.form.entryPoint,
        containerImage: {
          token: this.form.containerImage.token,
          registryId: this.form.containerImage.registry.id,
          image: this.form.containerImage.image,
          tag: this.form.containerImage.tag,
        },
        gitModel: {
          token: this.form.gitModel.token,
          gitId: this.form.gitModel.git.id,
          repository: this.form.gitModel.repository.name,
          owner: this.form.gitModel.repository.owner,
          branch: this.form.gitModel.branch.branchName,
          commitId:
            this.form.gitModel.commit === null
              ? this.commitsList[0].commitId
              : this.form.gitModel.commit.commitId,
        },
        cpu: this.form.resource.cpu,
        memory: this.form.resource.memory,
        gpu: this.form.resource.gpu,
      }
    },
    // 必須項目 (学習) に関する入力値チェック
    async prepareSubmitRequired() {
      if (
        // 学習コンテナイメージ設定
        this.form.containerImage === null ||
        this.form.containerImage.registry === null ||
        this.form.containerImage.image === null ||
        this.form.containerImage.tag === null ||
        // 学習Git設定
        this.form.gitModel === null ||
        this.form.gitModel.git === null ||
        this.form.gitModel.repository === null ||
        this.form.gitModel.branch === null ||
        // 実行コマンド
        this.form.entryPoint === null ||
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
        (this.form.containerImage.image !== null ||
          this.form.gitModel.repository !== null ||
          (this.form.entryPoint !== null && this.form.entryPoint !== '')) &&
        // コンテナイメージ設定
        (this.form.containerImage === null ||
          this.form.containerImage.registry === null ||
          this.form.containerImage.image === null ||
          this.form.containerImage.tag === null ||
          // Git設定
          this.form.gitModel === null ||
          this.form.gitModel.git === null ||
          this.form.gitModel.repository === null ||
          this.form.gitModel.branch === null ||
          // 実行コマンド
          this.form.entryPoint === null ||
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
}
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
