import api from '@/api/api'

// initial state
const state = {
  partitions: [],
  quota: {},
  nodes: [],
}

// getters
const getters = {
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
const actions = {
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
const mutations = {
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
