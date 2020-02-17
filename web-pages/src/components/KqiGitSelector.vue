<!--name: 学習モデルのGit情報の入力コンポーネント,-->
<!--description: リポジトリ名、ブランチ名を指定するドロップダウンをそれぞれ表示する。そのままだとHEADになるので、任意でコミットIDも直接指定可能,-->

<template>
  <el-form-item label="モデル">
    <el-row></el-row>
    <el-row>
      <!-- サーバの選択 -->
      <el-col :span="6" :offset="1">Gitサーバ</el-col>
      <el-col :span="12">
        <el-select
          :value="git"
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
          :value="repository"
          size="small"
          filterable
          clearable
          allow-create
          default-first-option
          remote
          :value-key="repositoryValueKey"
          :disabled="!git || loadingRepositories || disabled"
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
          :value="branch"
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="!repository || disabled"
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
          :disabled="commit === null"
          title="コミット詳細"
          trigger="hover"
          width="350"
          placement="right"
        >
          <span>
            <kqi-display-text-form
              label="コミットID"
              :value="commit ? commit.commitId : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コミッター"
              :value="commit ? commit.committerName : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コミット日時"
              :value="commit ? commit.commitAt : ''"
            ></kqi-display-text-form>
            <kqi-display-text-form
              label="コメント"
              :value="commit ? commit.comment : ''"
            ></kqi-display-text-form>
          </span>
        </el-popover>
        <el-select
          v-popover:commitDetail
          :value="commit"
          size="small"
          filterable
          clearable
          remote
          default-first-option
          :disabled="!branch || disabled"
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
          :disabled="!branch || disabled"
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
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('gitSelector')

export default {
  components: {
    KqiDisplayTextForm,
  },

  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
  },

  data() {
    return {
      model: this.value === undefined || this.value === null ? {} : this.value,

      // popover（コミットID一覧等）の「ローディング中」 文字列の表示制御
      listLoading: false,
      enableCommitIdSelecter: false,
      // リポジトリ名が手入力されたかどうかを表すフラグ
      repositoryCreated: false,
      repositoryValueKey: 'fullName',
    }
  },
  computed: {
    ...mapGetters([
      'gits',
      'repositories',
      'branches',
      'commits',
      'git',
      'repository',
      'branch',
      'commit',
      'loadingRepositories',
    ]),
  },

  methods: {
    changeGit(git) {
      if (git === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'git', value: null })
      } else {
        this.$emit('input', { type: 'git', value: git })
      }
    },

    // 選択しているリポジトリが切り替わった時に呼ばれるイベントハンドラ。
    changeRepository(repository) {
      if (repository === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'repository', value: null })
      } else {
        this.$emit('input', { type: 'repository', value: repository })
      }

      if (typeof repository === 'string') {
        // リポジトリ名を手入力された
        this.repositoryValueKey = 'value'
      } else {
        this.repositoryValueKey = 'fullName'
      }
    },

    // 選択しているブランチが切り替わった時に呼ばれるイベントハンドラ。
    changeBranch(branch) {
      if (branch === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'branch', value: null })
      } else {
        this.$emit('input', { type: 'branch', value: branch })
      }
    },

    // 選択しているコミットが切り替わった時に呼ばれるイベントハンドラ。
    changeCommit(commit) {
      if (commit === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'commit', value: null })
      } else {
        this.$emit('input', { type: 'commit', value: commit })
      }
    },
  },
}
</script>

<style lang="scss" scoped></style>
