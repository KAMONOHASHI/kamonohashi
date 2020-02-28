import api from '@/api/v1/api'

// initial state
const state = {
  preprocessings: [],
  total: 0,
  detail: {},
  history: {},
}

// getters
const getters = {
  preprocessings(state) {
    return state.preprocessings
  },
  total(state) {
    return state.total
  },
  detail(state) {
    return state.detail
  },
  history(state) {
    return state.history
  },
}

// actions
const actions = {
  async fetchPreprocessings({ commit }, params) {
    let response = await api.preprocessings.get(params)
    let preprocessings = response.data
    let total = response.headers['x-total-count']
    commit('setPreprocessings', { preprocessings })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.preprocessings.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchHistory({ commit }, id) {
    let history = (await api.preprocessings.getHistory({ id: id })).data
    commit('setHistory', history)
  },

  // eslint-disable-next-line no-unused-vars
  async runById({ rootState }, { id, params }) {
    return await api.preprocessings.runById({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.preprocessings.post(params)
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.preprocessings.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.preprocessings.delete({ id: id })
  },
}

// mutations
const mutations = {
  setPreprocessings(state, { preprocessings }) {
    state.preprocessings = preprocessings
  },

  setTotal(state, total) {
    state.total = total
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  clearDetail(state) {
    state.detail = {}
  },

  setHistory(state, history) {
    state.history = history
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
