import api from '@/api/v1/api'

// initial state
const state = {
  gits: [],
  defaultGitId: null,
  repositories: [],
  branches: [],
  commits: [],

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
    let repositories = (await api.git.getRepos({ gitId: gitId })).data
    commit('setRepositories', { repositories })
    commit('setLoadingRepositories', false)
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
    let branches = (await api.git.getBranches(params)).data
    context.commit('setBranches', { branches })
  },

  async fetchCommits({ commit }, { gitId, repository, branchName }) {
    let params = {
      gitId: gitId,
      owner: repository.owner,
      repositoryName: repository.name,
      branch: branchName,
    }
    let commits = (await api.git.getCommits(params)).data
    commit('setCommits', { commits })
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
