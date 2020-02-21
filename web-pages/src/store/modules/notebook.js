import api from '@/api/v1/api'

// initial state
const state = {
  histories: [],
  total: 0,
  detail: {},
  events: {},
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
  detail(state) {
    return state.detail
  },
  events(state) {
    return state.events
  },
  partitions(state) {
    return state.partitions
  },
}

// actions
const actions = {
  async fetchHistories({ commit }, params) {
    let response = await api.notebook.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.notebook.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchEvents({ commit }, id) {
    let events = (await api.notebook.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.notebook.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.notebook.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ state }, id) {
    return await api.notebook.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.notebook.deleteById({ id: id })
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

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setEvents(state, { events }) {
    state.events = events
  },

  clearDetail(state) {
    state.detail = {}
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
