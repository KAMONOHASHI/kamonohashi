<template>
  <div>
    <el-form ref="form1" :model="form" :rules="rules">
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

          <kqi-git-selector
            v-model="form.gitModel"
            :gits="gits"
            :repositories="repositories"
            :branches="branches"
            :commits="commits"
            :loading-repositories="loadingRepositories"
            heading="Gitサービス"
            @selectGit="selectGit"
            @selectRepository="selectRepository"
            @selectBranch="selectBranch"
          />
          <el-form-item label="実行コマンド" prop="entryPoint">
            <el-input
              v-model="form.entryPoint"
              type="textarea"
              :autosize="{ minRows: 2 }"
              :disabled="isPatch"
            />
          </el-form-item>
        </el-col>

        <el-col :span="6">
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
import registrySelectorUtil from '@/util/registrySelectorUtil'
import gitSelectorUtil from '@/util/gitSelectorUtil'
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
            registryId: 0,
            image: 'string',
            tag: 'string',
          },
          gitModel: {
            git: null,
            repository: null,
            branch: null,
            commit: null,
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
  },
  data() {
    return {
      rules: {
        containerImage: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
      },
      error: null,
      isPatch: false,
      repositories: [],
      branches: [],
      commits: [],
      commitDetail: null,
    }
  },
  computed: {
    ...mapGetters({
      registries: ['registrySelector/registries'],
      images: ['registrySelector/images'],
      tags: ['registrySelector/tags'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      quota: ['cluster/quota'],

      loadingRepositories: ['gitSelector/loadingRepositories'],
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

  async created() {
    const gitModel = { ...this.value.gitModel }
    const containerImage = { ...this.value.containerImage }

    // クォータ情報を取得
    await this['cluster/fetchQuota']()

    const formContainerImage = await this.setupFromContainerImage(
      containerImage,
    )
    const formGitModel = await this.setupFormGitModel(gitModel)
    this.form = {
      entryPoint: this.value.entryPoint,
      gitModel: formGitModel,
      containerImage: formContainerImage,
      resource: { ...this.value.resource },
    }
  },
  methods: {
    async setupFromContainerImage(containerImage) {
      await this['registrySelector/fetchRegistries']()
      await this['registrySelector/fetchImages'](containerImage.registryId)
      await this['registrySelector/fetchTags']({ ...containerImage })

      const registry = this.registries.find(registry => {
        return registry.id === containerImage.registryId
      })
      return { ...containerImage, registry }
    },
    async setupFormGitModel(gitModel) {
      if (gitModel == null || Object.keys(gitModel).length == 0) {
        return null
      }

      const formGitModel = {
        git: null,
        repository: null,
        branch: null,
        commit: null,
      }

      try {
        await this['gitSelector/fetchGits']()

        formGitModel.git = this.gits.find(git => {
          return git.id === gitModel.gitId
        })

        let repository = {
          name: gitModel.repository,
          owner: gitModel.owner,
          fullName: `${gitModel.owner}/${gitModel.repositoryName}`,
        }
        this.repositories.push(repository)
        formGitModel.repository = repository

        // ブランチ一覧の取得
        this.branches = await this['gitSelector/getBranches']({
          gitId: formGitModel.git.id,
          repository: repository,
        })
        formGitModel.branch = gitModel.branch

        // commit一覧の取得
        this.commits = await this['gitSelector/getCommits'](
          gitModel.git.gitId,
          gitModel.repository,
          gitModel.branch.branchName,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }

      let commit = null
      if (this.commits != null) {
        commit = this.commits.find(commit => {
          return commit.commitId === gitModel.commitId
        })
      }

      const fetchCommitArg = {
        gitId: gitModel.gitId,
        commitId: gitModel.commitId,
        repository: gitModel.repository,
        branchName: gitModel.branch,
      }
      if (fetchCommitArg.gitId != null) {
        this.commitDetail = await this['gitSelector/getCommitDetail'](
          fetchCommitArg,
        )
      }

      if (commit == null) {
        formGitModel.gitModel.commit = null
      } else if (commit) {
        formGitModel.gitModel.commit = commit
      } else {
        formGitModel.gitModel.commit = this.commitDetail
        this.commits.push(this.commitDetail)
      }
      return formGitModel.gitModel
    },
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
    async selectGit(gitId) {
      this.repositories = this.repositories = await gitSelectorUtil.selectGit(
        this.form,
        this['gitSelector/getRepositories'],
        gitId,
        this.$store,
      )
    },
    // repositoryの型がstring：手入力, object: 選択
    async selectRepository(repository) {
      try {
        this.branches = await gitSelectorUtil.selectRepository(
          this.form,
          this['gitSelector/getBranches'],
          repository,
        )
      } catch (message) {
        this.$notify.error({
          message: message,
        })
      }
    },
    async selectBranch(branchName) {
      this.commits = await gitSelectorUtil.selectBranch(
        this.form,
        this['gitSelector/getCommits'],
        branchName,
      )
    },
    ...mapActions([
      'modelTemplate/fetchDetail',
      'modelTemplate/post',
      'modelTemplate/put',
      'modelTemplate/delete',
      'registrySelector/fetchRegistries',
      'registrySelector/fetchImages',
      'registrySelector/fetchTags',
      'gitSelector/fetchGits',
      'gitSelector/getRepositories',
      'gitSelector/getBranches',
      'gitSelector/getCommits',
      'gitSelector/getCommitDetail',
      'cluster/fetchQuota',
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
