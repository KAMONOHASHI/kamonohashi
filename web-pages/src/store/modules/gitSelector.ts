import { GetterTree, ActionTree, MutationTree } from 'vuex'
import api from '@/api/api'
import * as gen from '@/api/api.generate'
import { RootState } from '../index'

interface StateType {
  gits: Array<
    gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialOutputModel
  >
  defaultGitId: number | null
  repositories: Array<gen.NssolPlatypusServiceModelsGitRepositoryModel>
  branches: Array<gen.NssolPlatypusServiceModelsGitBranchModel>
  commits: Array<gen.NssolPlatypusServiceModelsGitCommitModel>
  commitDetail: gen.NssolPlatypusServiceModelsGitCommitModel | null
  // リポジトリのロードは時間がかかる可能性があるためフラグを設けて管理
  loadingRepositories: boolean
}
// initial state
const state: StateType = {
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
const getters: GetterTree<StateType, RootState> = {
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
const actions: ActionTree<StateType, RootState> = {
  async fetchGits({ commit }) {
    let response = (await api.account.getGits()).data
    let gits = response.gits
    let defaultGitId = response.defaultGitId
    commit('setGits', { gits })
    commit('setDefaultGitId', defaultGitId)
  },

  async fetchRepositories({ commit }, gitId: number) {
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

  async fetchBranches(
    context,
    {
      gitId,
      repository,
      manualInput,
    }: {
      gitId: number
      repository: gen.NssolPlatypusServiceModelsGitRepositoryModel
      manualInput: boolean
    },
  ) {
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
      owner: repository.owner!,
      repositoryName: repository.name!,
    }
    try {
      let branches = (await api.git.getBranches(params)).data
      context.commit('setBranches', { branches })
    } catch {
      context.commit('setBranches', [])
    }
  },

  async fetchCommits(
    { commit },
    {
      gitId,
      repository,
      branchName,
      page,
    }: {
      gitId: number
      repository: gen.NssolPlatypusServiceModelsGitRepositoryModel
      branchName: gen.NssolPlatypusServiceModelsGitBranchModel
      page: number
    },
  ) {
    let params = {
      gitId: gitId,
      owner: repository.owner!,
      repositoryName: repository.name!,
      branch: branchName.branchName!,
      page: String(page),
    }
    try {
      let commits = (await api.git.getCommits(params)).data
      commit('setCommits', { commits })
    } catch {
      commit('setCommits', [])
    }
  },

  async fetchCommitDetail(
    { commit },
    {
      gitId,
      repository,
      commitId,
    }: {
      gitId: number
      repository: gen.NssolPlatypusServiceModelsGitRepositoryModel
      commitId: string
    },
  ) {
    let params = {
      gitId: gitId,
      owner: repository.owner!,
      repositoryName: repository.name!,
      commitId: commitId,
    }
    try {
      let commitDetail = (await api.git.getCommit(params)).data
      commit('setCommitDetail', { commitDetail })
    } catch {
      let commitDetail = null
      commit('setCommitDetail', { commitDetail })
    }
  },
}

// mutations
const mutations: MutationTree<StateType> = {
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
