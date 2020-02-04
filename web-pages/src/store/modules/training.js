import api from '@/api/v1/api'

// initial state
const state = {
  histories: [],
  total: 0,
  selections: [],
  detail: {},
  partitions: [],
}

// getters
const getters = {
  histories(state) {
    return state.histories
  },
  total(state) {
    return state.total
  },
  selections(state) {
    return state.selections
  },
  detail(state) {
    return state.detail
  },
  partitions(state) {
    return state.partitions
  },
}

// actions
const actions = {
  async fetchHistories({ commit }, params) {
    let response = await api.training.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (await api.training.getById({ id: rootState.route.params.id }))
      .data
    commit('setDetail', { detail })
  },

  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.training.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.training.deleteById({ id: id })
  },
}

// mutations
const mutations = {
  setHistories(state, { histories }) {
    state.histories = histories
  },
  setTotal(state, total) {
    state.total = total
  },
  setSelections(state, selections) {
    state.selections = selections
  },
  setDetail(state, { detail }) {
    state.detail = detail
  },
  setPartitions(state, { partitions }) {
    state.partitions = partitions
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
