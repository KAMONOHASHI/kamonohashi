<!--name: 学習モデルのGit情報の入力コンポーネント,-->
<!--description: リポジトリ名、ブランチ名を指定するドロップダウンをそれぞれ表示する。そのままだとHEADになるので、任意でコミットIDも直接指定可能,-->

<template>
  <el-form-item label="モデル" prop="gitModel">
    <el-row></el-row>
    <el-row>
      <!-- サーバの選択 -->
      <el-col :span="6" :offset="1">Gitサーバ</el-col>
      <el-col :span="12">
        <el-select
          :value="value.git"
          size="small"
          filterable
          value-key="id"
          clearable
          remote
          :disabled="disabled"
          @change="changeGit"
        >
          <el-option
            v-for="item in gits"
            :key="item.id"
            :label="item.name"
            :value="item"
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
          :value="value.repository"
          size="small"
          filterable
          clearable
          allow-create
          default-first-option
          remote
          :value-key="repositoryValueKey"
          :disabled="!value.git || loadingRepositories || disabled"
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
        <i v-if="loadingRepositories" class="el-icon-loading"></i>
      </el-col>
    </el-row>
    <el-row>
      <!-- ブランチの選択 -->
      <el-col :span="6" :offset="1">ブランチ</el-col>
      <el-col :span="12">
        <el-select
          :value="value.branch"
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="!value.repository || disabled"
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
          :disabled="value.commit === null"
          title="コミット詳細"
          trigger="hover"
          width="350"
          placement="right"
        >
          <span>
            <kqi-display-text-form
              label="コミットID"
              :value="value.commit ? value.commit.commitId : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コミッター"
              :value="value.commit ? value.commit.committerName : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コミット日時"
              :value="value.commit ? value.commit.commitAt : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コメント"
              :value="value.commit ? value.commit.comment : ''"
            ></kqi-display-text-form>
          </span>
        </el-popover>
        <el-select
          v-popover:commitDetail
          :value="value.commit"
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="!value.branch || disabled"
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
          :disabled="!value.branch || disabled"
          @click="enableCommitIdSelecter = true"
        >
          コミットIDを指定
        </el-button>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'

export default {
  components: {
    KqiDisplayTextForm,
  },

  props: {
    // gitサーバ一覧
    gits: {
      type: Array,
      default: () => {
        return []
      },
    },
    // リポジトリ一覧
    repositories: {
      type: Array,
      default: () => {
        return []
      },
    },
    // ブランチ一覧
    branches: {
      type: Array,
      default: () => {
        return []
      },
    },
    // コミット一覧
    commits: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択されたgitサーバ、リポジトリ、ブランチ、コミットをvalueで保持
    value: {
      type: Object,
      default: () => {
        return {
          git: null,
          repository: null,
          branch: null,
          tag: null,
        }
      },
    },
    // リポジトリ取得中フラグ
    loadingRepositories: {
      type: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },

  data() {
    return {
      // popover（コミットID一覧等）の「ローディング中」 文字列の表示制御
      listLoading: false,
      enableCommitIdSelecter: false,
      // リポジトリ名が手入力されたかどうかを表すフラグ
      repositoryCreated: false,
      repositoryValueKey: 'fullName',
    }
  },

  methods: {
    changeGit(git) {
      let gitModel = this.value
      if (git === '') {
        // clearボタンが押下された場合
        gitModel.git = null
      } else {
        gitModel.git = git
      }
      this.$emit('input', gitModel)
      this.$emit('selectGit', git === '' ? null : git.id)
    },

    // 選択しているリポジトリが切り替わった時に呼ばれるイベントハンドラ。
    changeRepository(repository) {
      let gitModel = this.value
      if (repository === '') {
        // clearボタンが押下された場合
        gitModel.repository = null
      } else {
        gitModel.repository = repository
      }
      this.$emit('input', gitModel)
      this.$emit('selectRepository', repository === '' ? null : repository)

      if (typeof repository === 'string') {
        // リポジトリ名を手入力された
        this.repositoryValueKey = 'value'
      } else {
        this.repositoryValueKey = 'fullName'
      }
    },

    // 選択しているブランチが切り替わった時に呼ばれるイベントハンドラ。
    changeBranch(branch) {
      let gitModel = this.value
      if (branch === '') {
        // clearボタンが押下された場合
        gitModel.branch = null
      } else {
        gitModel.branch = branch
      }
      this.$emit('input', gitModel)
      this.$emit('selectBranch', branch === '' ? null : branch.name)
    },

    // 選択しているコミットが切り替わった時に呼ばれるイベントハンドラ。
    changeCommit(commit) {
      let gitModel = this.value

      if (commit === '') {
        // clearボタンが押下された場合
        gitModel.commit = null
      } else {
        gitModel.commit = commit
      }
      this.$emit('input', gitModel)
    },
  },
}
</script>

<style lang="scss" scoped></style>
