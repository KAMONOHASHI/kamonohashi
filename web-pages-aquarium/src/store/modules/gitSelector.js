import api from '@/api/api'

// initial state
const state = {
  gits: [],
  defaultGitId: null,
  repositories: [],
  branches: [],
  commits: [],
  commitDetail: null,

  // リポジトリのロードは時間がかかる可能性があるためフラグを設けて管理
  loadingRepositories: false,
}

// getters
const getters = {
  gits(state) {
    return state.gits
  },
  defaultGitId(state) {
    return state.defaultGitId
  },
  repositories(state) {
    return state.repositories
  },
  branches(state) {
    return state.branches
  },
  commits(state) {
    return state.commits
  },
  commitDetail(state) {
    return state.commitDetail
  },

  loadingRepositories(state) {
    return state.loadingRepositories
  },
}

// actions
const actions = {
  async fetchGits({ commit }) {
    let response = (await api.account.getGits()).data
    let gits = response.gits
    let defaultGitId = response.defaultGitId
    commit('setGits', { gits })
    commit('setDefaultGitId', defaultGitId)
  },

  async fetchRepositories({ commit }, gitId) {
    commit('setLoadingRepositories', true)
    try {
      let repositories = (await api.git.getRepos({ gitId: gitId })).data
      commit('setRepositories', { repositories })
    } catch {
      // gitサーバに接続できないエラーはaxios-ext側でハンドリングし、表示する
      commit('setRepositories', [])
    }
    commit('setLoadingRepositories', false)
  },

  async getRepositories({ commit }, gitId) {
    commit('setLoadingRepositories', true)
    let repositories = []
    try {
      repositories = (await api.git.getRepos({ gitId: gitId })).data
    } catch {
      repositories = []
    }
    commit('setLoadingRepositories', false)
    return repositories
  },

  async fetchBranches(context, { gitId, repository, manualInput }) {
    // 手入力された場合
    if (manualInput) {
      // リポジトリ一覧にない場合は追加
      let repositories = context.state.repositories
      if (
        !repositories.some(
          r =>
            r.owner === repository.owner &&
            r.name === repository.name &&
            r.fullName === repository.fullName,
        )
      ) {
        repositories.push(repository)
        context.commit('setRepositories', { repositories })
      }
    }
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
    }
    try {
      let branches = (await api.git.getBranches(params)).data
      context.commit('setBranches', { branches })
    } catch {
      context.commit('setBranches', [])
    }
  },

  // eslint-disable-next-line no-unused-vars
  async getBranches({ state }, { gitId, repository }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
    }
    try {
      let branches = (await api.git.getBranches(params)).data
      return branches
    } catch {
      return []
    }
  },

  async fetchCommits({ commit }, { gitId, repository, branchName }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
      branch: branchName,
    }
    try {
      let commits = (await api.git.getCommits(params)).data
      commit('setCommits', { commits })
    } catch {
      commit('setCommits', [])
    }
  },

  // eslint-disable-next-line no-unused-vars
  async getCommits({ state }, { gitId, repository, branchName }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
      branch: branchName,
    }
    try {
      let commits = (await api.git.getCommits(params)).data
      return commits
    } catch {
      return []
    }
  },

  async fetchCommitDetail({ commit }, { gitId, repository, commitId }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
      commitId: commitId,
    }
    try {
      let commitDetail = (await api.git.getCommit(params)).data
      commit('setCommitDetail', { commitDetail })
    } catch {
      commit('setCommitDetail', null)
    }
  },

  // eslint-disable-next-line no-unused-vars
  async getCommitDetail({ state }, { gitId, repository, commitId }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
      commitId: commitId,
    }
    try {
      let commitDetail = (await api.git.getCommit(params)).data
      return commitDetail
    } catch {
      return null
    }
  },
}

// mutations
const mutations = {
  setGits(state, { gits }) {
    state.gits = gits
  },
  setDefaultGitId(state, defaultGitId) {
    state.defaultGitId = defaultGitId
  },
  setRepositories(state, { repositories }) {
    state.repositories = repositories
  },
  setBranches(state, { branches }) {
    state.branches = branches
  },
  setCommits(state, { commits }) {
    state.commits = commits
  },
  setCommitDetail(state, { commitDetail }) {
    state.commitDetail = commitDetail
  },

  setLoadingRepositories(state, flag) {
    state.loadingRepositories = flag
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
