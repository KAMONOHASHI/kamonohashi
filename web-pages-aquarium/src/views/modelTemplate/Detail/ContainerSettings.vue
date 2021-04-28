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
          <el-form-item>
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
          </el-form-item>
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
            registryId: 0,
            image: 'string',
            tag: 'string',
            token: 'string',
          },
          gitModel: {
            gitId: 0,
            repository: 'string',
            owner: 'string',
            branch: 'string',
            commitId: 'string',
            token: 'string',
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
    }
  },
  computed: {
    ...mapGetters({
      registries: ['registrySelector/registries'],
      images: ['registrySelector/images'],
      tags: ['registrySelector/tags'],
      gits: ['gitSelector/gits'],
      defaultGitId: ['gitSelector/defaultGitId'],
      repositories: ['gitSelector/repositories'],
      branches: ['gitSelector/branches'],
      quota: ['cluster/quota'],

      loadingRepositories: ['gitSelector/loadingRepositories'],
      commits: ['gitSelector/commits'],
      commitDetail: ['gitSelector/commitDetail'],
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
      const formGitModel = {}
      const repositoryName = gitModel.repository
      await this['gitSelector/fetchGits']()

      formGitModel.git = this.gits.find(git => {
        return git.id === gitModel.gitId
      })

      formGitModel.repository = `${gitModel.owner}/${repositoryName}`

      formGitModel.branch = gitModel.branch
      formGitModel.token = gitModel.token
      const fetchCommitArg = {
        gitId: gitModel.gitId,
        commitId: gitModel.commitId,
        repository: {
          owner: gitModel.owner,
          name: repositoryName,
        },
        branchName: gitModel.branch,
      }

      await this['gitSelector/fetchCommits'](fetchCommitArg)

      let commit = null
      if (this.commits != null) {
        commit = this.commits.find(commit => {
          return commit.commitId === gitModel.commitId
        })
      }
      if (fetchCommitArg.gitId != null) {
        await this['gitSelector/fetchCommitDetail'](fetchCommitArg)
      }

      if (commit == null) {
        formGitModel.commit = null
      } else if (commit) {
        formGitModel.commit = commit
      } else {
        formGitModel.commit = this.commitDetail
      }
      return formGitModel
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
