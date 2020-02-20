import api from '@/api/v1/api'

// initial state
const state = {
  partitions: [],
  partition: null,
}

// getters
const getters = {
  partitions(state) {
    return state.partitions
  },

  partition(state) {
    return state.partition
  },
}

// actions
const actions = {
  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },
}

// mutations
const mutations = {
  setPartitions(state, { partitions }) {
    state.partitions = partitions
  },

  setPartition(state, partition) {
    state.partition = partition
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
