import api from '@/api/api'

// initial state
const state = {
  partitions: [],
  quota: {},
}

// getters
const getters = {
  partitions(state) {
    return state.partitions
  },
  quota(state) {
    return state.quota
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
}

// mutations
const mutations = {
  setPartitions(state, { partitions }) {
    state.partitions = partitions
  },
  setQuota(state, { quota }) {
    state.quota = quota
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
