<!--name: 学習モデルのGit情報の入力コンポーネント,-->
<!--description: リポジトリ名、ブランチ名を指定するドロップダウンをそれぞれ表示する。そのままだとHEADになるので、任意でコミットIDも直接指定可能,-->
<!--props: {  value: デフォルト値にするGit情報,}-->
<!--events: { input(GitModel): 入力されたGit情報。各入力値が変更されるたびに発火される。,}-->
<template>
  <div class="el-input">
    <el-row>
      <!-- サーバの選択 -->
      <el-col :span="6" :offset="1">Gitサーバ</el-col>
      <el-col :span="12">
        <el-select
          v-model="selectedGitId"
          size="small"
          filterable
          clearable
          remote
          :loading="listLoading"
          :disabled="serverDisabled"
          @change="changeGitId"
        >
          <el-option
            v-for="item in gits"
            :key="item.id"
            :label="item.name"
            :value="item.id"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>
    <el-row>
      <!-- リポジトリの選択 -->
      <el-col :span="6" :offset="1">リポジトリ</el-col>
      <el-col :span="12">
        <el-select
          v-model="selectedRepository"
          size="small"
          filterable
          clearable
          allow-create
          default-first-option
          :loading="listLoading"
          value-key="fullName"
          :disabled="repositoryDisabled"
          @change="changeRepository"
        >
          <el-option
            v-for="item in repositories"
            :key="item.fullName"
            :label="item.fullName"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
      <el-col :span="2">
        <i v-if="disableRepositorySelector" class="el-icon-loading"></i>
      </el-col>
    </el-row>
    <el-row>
      <!-- ブランチの選択 -->
      <el-col :span="6" :offset="1">ブランチ</el-col>
      <el-col :span="12">
        <el-select
          v-model="selectedBranch"
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="branchDisabled"
          :loading="listLoading"
          value-key="branchName"
          @change="changeBranch"
        >
          <el-option
            v-for="item in branches"
            :key="item.branchName"
            :label="item.branchName"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>
    <el-row>
      <!-- コミットIDの選択。ブランチで選択する場合は表示されない。 -->
      <el-col :span="6" :offset="1">コミットID</el-col>
      <el-col v-if="enableCommitIdSelecter" :span="12">
        <el-popover
          ref="commitDetail"
          :disabled="selectedCommit.commitId === null"
          title="コミット詳細"
          trigger="hover"
          width="350"
          placement="right"
        >
          <span>
            <pl-display-text-form
              label="コミットID"
              :value="selectedCommit.commitId"
            ></pl-display-text-form>
            <pl-display-text-form
              label="コミッター"
              :value="selectedCommit.committerName"
            ></pl-display-text-form>
            <pl-display-text-form
              label="コミット日時"
              :value="selectedCommit.commitAt"
            ></pl-display-text-form>
            <pl-display-text-form
              label="コメント"
              :value="selectedCommit.comment"
            ></pl-display-text-form>
          </span>
        </el-popover>
        <el-select
          v-model="selectedCommit"
          v-popover:commitDetail
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="commitDisabled"
          :loading="listLoading"
          value-key="commitId"
          @change="changeCommit"
        >
          <!-- 選択解除用 -->
          <el-option key="HEAD" label="HEAD" :value="null" />
          <el-option
            v-for="item in commits"
            :key="item.commitId"
            :label="item.commitId"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
      <el-col v-else :span="12">
        <span>
          HEAD
        </span>
        <el-button
          size="mini"
          :disabled="selectedCommitIdDisabled"
          @click="showCommitIdSelect"
        >
          コミットIDを指定
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'

export default {
  components: {
    'pl-display-text-form': DisplayTextForm,
  },

  props: ['value', 'disabled', 'loading'],

  data() {
    return {
      model: this.value === undefined || this.value === null ? {} : this.value,

      // popover（コミットID一覧等）の「ローディング中」 文字列の表示制御
      listLoading: false,
      // Gitサーバ一覧
      gits: [],
      // リポジトリ一覧
      repositories: [],
      // ブランチ一覧
      branches: [],
      // コミット一覧
      commits: [],

      // 選択中のGitId
      selectedGitId: null,
      // 選択中のリポジトリ
      selectedRepository: {
        owner: null,
        name: null,
        fullName: null,
      },
      // 選択中のブランチ
      selectedBranch: {
        branchName: null,
        commitId: null,
      },
      // 選択中のコミット
      selectedCommit: {
        commitId: null, // コミットID。未設定の場合はHEADになる。
        committerName: null,
        commitAt: null,
        comment: null,
      },

      enableCommitIdSelecter: false,
      disableRepositorySelector: false, // ロード中に選択されると変なことになるので、処理の長いリポジトリ選択は表示切替フラグを付ける
    }
  },
  computed: {
    serverDisabled() {
      return this.disabled
    },
    repositoryDisabled() {
      return this.disabled || this.disableRepositorySelector
    },
    branchDisabled() {
      return this.disabled || this.branches.length === 0
    },
    commitDisabled() {
      return this.disabled || this.commits.length === 0
    },
    selectedCommitIdDisabled() {
      return this.disabled || this.commits.length === 0
    },
  },

  // 親コンポーネントで、this.valueが変更された場合に呼ばれる処理
  watch: {
    async value() {
      this.setSelectedParameter()
      await this.getData()
    },
  },

  async mounted() {
    // 初期値が入っている場合はそれに伴いパラメータを取得
    if (this.value) {
      this.setSelectedParameter()
    }
    await this.getData()
  },

  methods: {
    setSelectedParameter() {
      this.model = this.value

      // git server
      if (this.value.gitId !== undefined) {
        this.selectedGitId = this.value.gitId
      }

      // repository
      if (
        this.value.owner !== undefined &&
        this.value.repository !== undefined
      ) {
        this.selectedRepository = {
          owner: this.value.owner,
          name: this.value.repository,
          fullName: `${this.value.owner}/${this.value.repository}`,
        }
      }

      // branch
      if (this.value.branch !== undefined) {
        this.selectedBranch.branchName = this.value.branch
      }

      // commit id
      if (this.value.commitId !== undefined) {
        this.selectedCommit.commitId = this.value.commitId
        if (this.value.commitId !== null) {
          this.enableCommitIdSelecter = true
        }
      }
    },
    async getData() {
      await this.getGits() // Gitサーバ一覧の取得

      if (this.selectedGitId !== null) {
        await this.getRepositories() // リポジトリ一覧の再取得
      }

      if (
        this.selectedRepository.owner !== null &&
        this.selectedRepository.name !== null
      ) {
        await this.getBranches() // ブランチ一覧の再取得
      }
      if (this.selectedBranch.branchName !== null) {
        await this.getCommits() // コミット一覧の再取得
      }
    },
    async getGits() {
      if (this.gits && this.gits.length > 0) {
        // 一度取得していたら再取得はしない
        return
      }

      try {
        this.listLoading = true
        let [result] = api.f.data(await api.account.getGits())
        this.gits = result.gits
        if (this.selectedGitId === null) {
          this.selectedGitId = result.defaultGitId
          this.model.gitId = result.defaultGitId
        }
      } finally {
        this.listLoading = false
      }
    },

    async getRepositories() {
      if (this.repositories && this.repositories.length > 0) {
        // 一度取得していたら再取得はしない
        return
      }

      try {
        this.listLoading = true
        this.disableRepositorySelector = true
        let params = {
          gitId: this.selectedGitId,
          $config: { apiDisabledLoading: true },
        }
        let [list] = api.f.data(await api.git.getRepos(params))
        if (this.repositories.length === 0) {
          // await中に別の値がセットされる可能性がある。その時は無視。
          this.repositories = list
          if (
            this.selectedRepository.fullName &&
            this.repositories.some(
              r => r.fullName === this.selectedRepository.fullName,
            ) === false
          ) {
            // 前に入力した値が一覧の中にない＝以前は手入力で入れた
            // 入力を足す
            this.repositories.push({
              owner: this.value.owner,
              name: this.value.repository,
              fullName: this.value.owner + '/' + this.value.repository,
            })
          }
        }
        this.disableRepositorySelector = false
      } finally {
        this.listLoading = false
      }
    },

    async getBranches() {
      if (this.branches && this.branches.length > 0) {
        // 一度取得していたら再取得はしない
        return
      }

      try {
        this.listLoading = true
        let params = {
          gitId: this.selectedGitId,
          owner: this.selectedRepository.owner,
          repositoryName: this.selectedRepository.name,
        }
        let [list] = api.f.data(await api.git.getBranches(params))
        this.branches = list
        // } catch (e) {
        //   this.$notify.error("Couldn't get")
      } finally {
        this.listLoading = false
      }
    },

    async getCommits() {
      if (this.commits && this.commits.length > 0) {
        // 一度取得していたら再取得はしない
        return
      }

      try {
        this.listLoading = true
        let params = {
          gitId: this.selectedGitId,
          owner: this.selectedRepository.owner,
          repositoryName: this.selectedRepository.name,
          branch: this.selectedBranch.branchName,
        }
        let [list] = api.f.data(await api.git.getCommits(params))
        this.commits = list
        // } catch (e) {
        //  this.$notify.error("Couldn't get")
      } finally {
        this.listLoading = false

        if (this.selectedCommit.commitId) {
          // 初めてコミット一覧を取得するのに、既にIDが選択済み＝既定値が設定されている＝popover用にコピーが必要
          let commit = this.commits.find(value => {
            return value.commitId === this.selectedCommit.commitId
          })
          if (commit) {
            this.selectedCommit.committerName = commit.committerName
            this.selectedCommit.commitAt = commit.commitAt
            this.selectedCommit.comment = commit.comment
          }
        }
      }
    },

    // @Emit()
    input(value) {
      this.$emit('input', value)
    },

    // リポジトリ以下の選択状態をリセットする
    resetRepository() {
      this.repositories = []
      this.selectedRepository = {
        owner: null,
        name: null,
        fullName: null,
      }
      this.model.repository = null

      this.resetBranch()
    },

    // ブランチ以下の選択状態をリセットする
    resetBranch() {
      this.branches = []
      this.selectedBranch = {
        branchName: null,
        commitId: null,
      }
      this.model.branch = null

      this.resetCommit()
    },

    // コミットIDの選択状態をリセットする
    resetCommit() {
      this.commits = [] // new Array<GitCommitModel>();
      this.selectedCommit = {
        commitId: null,
        committerName: null,
        commitAt: null,
        comment: null,
      }
      this.enableCommitIdSelecter = false
      this.model.commitId = null
    },

    changeGitId(gitId) {
      // Gitが変わったので、リポジトリ以下の選択状態をリセットする
      this.resetRepository()

      this.model.gitId = gitId
      if (gitId) {
        // バツボタンを押してリポジトリを削除した場合はFalseになる
        this.input(this.model)
        this.getRepositories() // リポジトリ一覧の再取得
      }
    },

    // 選択しているリポジトリが切り替わった時に呼ばれるイベントハンドラ。
    changeRepository(repository) {
      // リポジトリが変わったので、ブランチ以下の選択状態をリセットする
      this.resetBranch()

      if (repository) {
        // リポジトリが変更された場合
        if (typeof repository === 'string') {
          // リポジトリ名を手入力された
          let index = repository.indexOf('/')
          if (index > 0) {
            this.model.repository = repository.substring(index + 1)
            this.model.owner = repository.substring(0, index)
            this.selectedRepository = {
              name: this.model.repository,
              owner: this.model.owner,
            }
            this.input(this.model)
            this.getBranches() // ブランチ一覧の再取得
          } else {
            // 構文エラー
          }
        } else {
          this.model.repository = this.selectedRepository.name
          this.model.owner = this.selectedRepository.owner
          this.input(this.model)
          this.getBranches() // ブランチ一覧の再取得
        }
      } else {
        // バツボタンを押してリポジトリを削除した場合
        this.model.repository = null
        this.model.owner = null
      }
    },

    // 選択しているブランチが切り替わった時に呼ばれるイベントハンドラ。
    changeBranch(branch) {
      // リポジトリが変わったので、コミットIDの選択状態をリセットする
      this.resetCommit()

      if (branch) {
        this.model.branch = this.selectedBranch.branchName
        this.input(this.model)
        this.getCommits() // コミット一覧の再取得
      } else {
        this.model.branch = null
      }
    },

    // 選択しているコミットが切り替わった時に呼ばれるイベントハンドラ。
    changeCommit(commit) {
      if (commit) {
        this.selectedCommit = commit
        this.model.commitId = this.selectedCommit.commitId
        this.input(this.model)
      } else {
        // HEADが選択された場合の処理
        this.selectedCommit = {
          commitId: null,
          committerName: null,
          commitAt: null,
          comment: null,
        }
        this.enableCommitIdSelecter = false
        this.model.commitId = null
      }
    },

    // コミットID選択用のドロップダウンを表示する
    async showCommitIdSelect() {
      await this.getCommits()
      this.enableCommitIdSelecter = true
    },
  },
}
</script>

<style lang="scss" scoped></style>
