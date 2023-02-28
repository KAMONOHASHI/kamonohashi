import * as gen from '@/api/api.generate'
type Form = {
  name?: string | null
  dataSetId?: string | number | null
  entryPoint?: string | null
  selectedParent?: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  >
  selectedParentInference?: Array<
    gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
  >
  containerImage: {
    registry: {
      id?: number | null
      name?: string
    } | null
    image: string | null
    tag: string | null
  }
  gitModel: {
    git: {
      id?: number | null
      name?: string
    } | null
    repository:
      | string
      | { name?: string; owner?: string; fullName?: string }
      | null
    branch?: string | { branchName: string } | null
    commit: gen.NssolPlatypusServiceModelsGitCommitModel | null
  }

  jupyterLabVersion?: string | null
  resource: {
    cpu?: number
    memory?: number
    gpu?: number
  }
  expiresIn?: number
  withExpiresInSetting?: boolean
  variables?: Array<{ key: string; value: string }>
  partition?: string | null
  zip?: boolean
  localDataSet?: boolean
  memo?: string | null
}

export default class GitSelectorUtil {
  // gitサーバを選択し、リポジトリを取得する
  // form: form object
  // fetchRepositories: 'gitSelector/fetchRepositories'
  // gitId: 選択したサーバID
  // store this.$store
  static async selectGit(
    form: Form,
    fetchRepositories: Function,
    gitId: number,
    store: any,
  ) {
    // 過去の選択状態をリセット
    form.gitModel.repository = null
    form.gitModel.branch = null
    form.gitModel.commit = null

    // clearの場合リセット、gitサーバが選択された場合はリポジトリ取得
    if (gitId !== null) {
      // 独自ローディング処理のため共通側は無効
      store.commit('setLoading', false)
      await fetchRepositories(gitId)
      // 共通側ローディングを再度有効化
      store.commit('setLoading', true)
    }
  }

  // リポジトリを選択し、ブランチを取得する
  // form: form object
  // fetchBranches: 'gitSelector/fetchBranches'
  // repository: 選択したリポジトリ: 手入力の場合はstring, 選択肢から選んだ場合はobject
  static async selectRepository(
    form: Form,
    fetchBranches: Function,
    repository:
      | string
      | { owner: string; name: string; fulName?: string }
      | null,
  ) {
    // 過去の選択状態をリセット
    form.gitModel.branch = null
    form.gitModel.commit = null

    let manualInput = false
    let argRepository = {}
    if (typeof repository === 'string') {
      manualInput = true
      let repositoryName = repository
      let index = repositoryName.indexOf('/')
      if (index > 0) {
        argRepository = {
          owner: repositoryName.substring(0, index),
          name: repositoryName.substring(index + 1),
          fullName: repositoryName,
        }
        form.gitModel.repository = argRepository
      } else {
        // 構文エラー
        form.gitModel.repository = null
        throw '{owner}/{name}の形式で入力してください。例：KAMONOHASHI/tutorial'
      }
    } else {
      argRepository = repository!
    }
    // clearの場合リセット、リポジトリが選択された場合はブランチ取得
    if (repository !== null) {
      try {
        await fetchBranches({
          gitId: form.gitModel.git!.id,
          repository: argRepository,
          manualInput: manualInput,
        })
      } catch (e) {
        form.gitModel.repository = null
      }
    }
  }

  // ブランチを選択し、コミットを取得する
  // form: form object
  // fetchCommits: 'gitSelector/fetchCommits'
  // branchName: 選択したブランチ名
  // page: ページ番号
  static async selectBranch(
    form: Form,
    fetchCommits: Function,
    branchName: string,
    page: number,
  ) {
    // clearの場合リセット、ブランチが選択された場合はコミット取得
    if (branchName !== null) {
      await fetchCommits({
        gitId: form.gitModel.git!.id,
        repository: form.gitModel.repository,
        branchName: branchName,
        page: page,
      })
    }
  }
}
