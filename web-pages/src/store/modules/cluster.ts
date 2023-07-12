import { GetterTree, ActionTree, MutationTree } from 'vuex'
import api from '@/api/api'
import * as gen from '@/api/api.generate'
import { RootState } from '../index'
interface StateType {
  partitions: Array<string>
  quota: gen.NssolPlatypusApiModelsClusterApiModelsQuotaOutputModel | null
  nodes: Array<
    gen.NssolPlatypusApiModelsClusterApiModelsNodeResourceOutputModel
  >
}
// initial state
const state: StateType = {
  partitions: [],
  quota: null,
  nodes: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  partitions(state) {
    return state.partitions
  },
  quota(state) {
    return state.quota
  },
  nodes(state) {
    return state.nodes
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },

  async fetchQuota({ commit }) {
    let quota = (await api.cluster.getQuota()).data
    commit('setQuota', { quota })
  },

  async fetchNodes({ commit }) {
    let nodes = (await api.cluster.getTenantNodes()).data
    commit('setNodes', { nodes })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setPartitions(state, { partitions }) {
    state.partitions = partitions
  },
  setQuota(state, { quota }) {
    state.quota = quota
  },
  setNodes(state, { nodes }) {
    state.nodes = nodes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
