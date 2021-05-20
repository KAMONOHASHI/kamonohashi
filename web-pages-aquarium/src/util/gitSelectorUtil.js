export default class GitSelectorUtil {
  // gitサーバを選択し、リポジトリを取得する
  // form: form object
  // fetchRepositories: 'gitSelector/fetchRepositories'
  // gitId: 選択したサーバID
  static async selectGit(form, getRepositories, gitId) {
    // 過去の選択状態をリセット
    form.gitModel.repository = null
    form.gitModel.branch = null
    form.gitModel.commit = null

    // clearの場合リセット、gitサーバが選択された場合はリポジトリ取得
    let repositories = []
    if (gitId !== null) {
      repositories = await getRepositories(gitId)
    }

    return repositories
  }

  // リポジトリを選択し、ブランチを取得する
  // form: form object
  // fetchBranches: 'gitSelector/getBranches'
  // repository: 選択したリポジトリ: 手入力の場合はstring, 選択肢から選んだ場合はobject
  // repositories: リポジトリ一覧
  static async selectRepository(form, getBranches, repository, repositories) {
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
        repositories.push(argRepository)
      } else {
        // 構文エラー
        form.gitModel.repository = null
        throw '{owner}/{name}の形式で入力してください。例：KAMONOHASHI/tutorial'
      }
    } else {
      argRepository = repository
    }
    // clearの場合リセット、リポジトリが選択された場合はブランチ取得
    let branches = []
    if (repository !== null) {
      try {
        branches = await getBranches({
          gitId: form.gitModel.git.id,
          repository: argRepository,
          manualInput: manualInput,
        })
      } catch (e) {
        form.gitModel.repository = null
      }
    }
    return branches
  }

  // ブランチを選択し、コミットを取得する
  // form: form object
  // fetchCommits: 'gitSelector/fetchCommits'
  // branchName: 選択したブランチ名
  static async selectBranch(form, getCommits, branchName) {
    // 過去の選択状態をリセット
    form.gitModel.commit = null

    // clearの場合リセット、ブランチが選択された場合はコミット取得
    let commits
    if (branchName !== null) {
      commits = await getCommits({
        gitId: form.gitModel.git.id,
        repository: form.gitModel.repository,
        branchName: branchName,
      })
    } else {
      commits = []
    }

    return commits
  }
}
