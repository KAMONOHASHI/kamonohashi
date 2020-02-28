import api from '@/api/v1/api'

// initial state
const state = {
  gits: [],
  repositories: [],
  branches: [],
  commits: [],

  git: null,
  repository: null,
  branch: null,
  commit: null,

  // リポジトリのロードは時間がかかる可能性があるためフラグを設けて管理
  loadingRepositories: false,
}

// getters
const getters = {
  gits(state) {
    return state.gits
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

  git(state) {
    return state.git
  },
  repository(state) {
    return state.repository
  },
  branch(state) {
    return state.branch
  },
  commit(state) {
    return state.commit
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

    commit(
      'setGit',
      gits.find(git => {
        return git.id === defaultGitId
      }),
    )
  },

  async fetchRepositories(context) {
    let gitId = context.state.git.id
    context.commit('setLoadingRepositories', true)
    let repositories = (await api.git.getRepos({ gitId: gitId })).data
    context.commit('setRepositories', { repositories })
    context.commit('setLoadingRepositories', false)
  },

  async fetchBranches(context) {
    if (typeof context.state.repository === 'string') {
      // リポジトリ名を手入力された
      let repositoryName = context.state.repository
      let index = repositoryName.indexOf('/')

      if (index > 0) {
        let repository = {
          owner: repositoryName.substring(0, index),
          name: repositoryName.substring(index + 1),
          fullName: repositoryName,
        }
        context.commit('setRepository', repository)
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
      } else {
        //構文エラー
      }
    }
    let params = {
      gitId: context.state.git.id,
      owner: context.state.repository.owner,
      repositoryName: context.state.repository.name,
    }
    let branches = (await api.git.getBranches(params)).data
    context.commit('setBranches', { branches })
  },

  async fetchCommits(context) {
    let params = {
      gitId: context.state.git.id,
      owner: context.state.repository.owner,
      repositoryName: context.state.repository.name,
      branch: context.state.branch.name,
    }
    let commits = (await api.git.getCommits(params)).data
    context.commit('setCommits', { commits })
  },
}

// mutations
const mutations = {
  setGits(state, { gits }) {
    state.gits = gits
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

  setGit(state, git) {
    state.git = git
  },
  setRepository(state, repository) {
    state.repository = repository
  },
  setBranch(state, branch) {
    state.branch = branch
  },
  setCommit(state, commit) {
    state.commit = commit
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
