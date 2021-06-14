<template>
  <div>
    <el-form :model="form" :rules="rules">
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
                style="width:215px"
              />
            </el-col>
          </el-row>
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

          <el-row>
            <el-col :span="6" :offset="1">token</el-col>
            <el-col :span="12">
              <el-input
                v-model="form.gitModel.token"
                size="small"
                type="password"
                style="width:215px"
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
      images: [],
      tags: [],
      repositories: [],
      branches: [],
      commits: [],
      commitDetail: null,
    }
  },
  computed: {
    ...mapGetters({
      registries: ['registrySelector/registries'],
      gits: ['gitSelector/gits'],
      defaultRegistryId: ['registrySelector/defaultRegistryId'],
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
      const formContainerImage = {
        registry: null,
        image: null,
        tag: null,
        token: containerImage.token,
      }

      if (
        containerImage === null ||
        Object.keys(containerImage).length == 0 ||
        containerImage.image === null
      ) {
        return formContainerImage
      }

      await this['registrySelector/fetchRegistries']()

      formContainerImage.registry = this.registries.find(registry => {
        return registry.id == containerImage.registryId
      })

      formContainerImage.image = containerImage.image
      formContainerImage.tag = containerImage.tag

      this.images.push(formContainerImage.image)
      this.tags.push(formContainerImage.tag)

      return formContainerImage
    },
    async setupFormGitModel(gitModel) {
      await this['gitSelector/fetchGits']()

      const formGitModel = {
        git: null,
        repository: null,
        branch: null,
        commit: null,
        token: gitModel.token,
      }

      // gitModelが空 もしくは リポジトリが設定されていない場合はそのまま返却
      if (
        gitModel === null ||
        Object.keys(gitModel).length == 0 ||
        gitModel.repository === null
      ) {
        return formGitModel
      }

      formGitModel.git = this.gits.find(git => {
        return git.id === gitModel.gitId
      })

      let repository = {
        name: gitModel.repository,
        owner: gitModel.owner,
        fullName: `${gitModel.owner}/${gitModel.repository}`,
      }
      this.repositories.push(repository)
      formGitModel.repository = repository

      // ブランチ一覧の取得
      this.branches = await this['gitSelector/getBranches']({
        gitId: formGitModel.git.id,
        repository: formGitModel.repository,
      })
      formGitModel.branch = this.branches.find(branch => {
        return branch.branchName === gitModel.branch
      })

      // commit一覧の取得
      this.commits = await this['gitSelector/getCommits']({
        gitId: formGitModel.git.id,
        repository: formGitModel.repository,
        branchName: formGitModel.branch.branchName,
      })

      // commitを抽出
      let commit = null
      if (this.commits != null) {
        commit = this.commits.find(commit => {
          return commit.commitId === gitModel.commitId
        })
      }

      if (commit !== null) {
        formGitModel.commit = commit
      } else if (commit === null) {
        let commitDetail = await this['gitSelector/getCommitDetail']({
          gitId: formGitModel.git.id,
          repository: formGitModel.repository,
          commitId: gitModel.commitId,
        })
        formGitModel.commit = commitDetail
        this.commits.push(this.commitDetail)
      }
      return formGitModel
    },
    async selectRegistry(registryId) {
      this.images = await registrySelectorUtil.selectRegistry(
        this.form,
        this['registrySelector/getImages'],
        registryId,
      )
    },
    async selectImage(image) {
      this.tags = await registrySelectorUtil.selectImage(
        this.form,
        this['registrySelector/getTags'],
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
          this.repositories,
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
      this.form.gitModel.commit = this.commits[0]
    },
    ...mapActions([
      'modelTemplate/fetchDetail',
      'modelTemplate/post',
      'modelTemplate/put',
      'modelTemplate/delete',
      'registrySelector/fetchRegistries',
      'gitSelector/fetchGits',
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
