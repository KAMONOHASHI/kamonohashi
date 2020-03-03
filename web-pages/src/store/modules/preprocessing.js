import api from '@/api/v1/api'

// initial state
const state = {
  preprocessings: [],
  total: 0,
  detail: {},
  histories: {},
  historyDetail: {},
  historyEvents: {},
  logFile: {},
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

  histories(state) {
    return state.histories
  },

  historyDetail(state) {
    return state.historyDetail
  },

  historyEvents(state) {
    return state.historyEvents
  },

  logFile(state) {
    return state.logFile
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

  async fetchHistories({ commit }, id) {
    let histories = (await api.preprocessings.getHistory({ id: id })).data
    commit('setHistories', histories)
  },

  async fetchHistoryDetail({ commit }, { id, dataId }) {
    let historyDetail = (
      await api.preprocessings.getHistroyById({
        id: id,
        dataId: dataId,
      })
    ).data
    commit('setHistoryDetail', historyDetail)
  },

  async fetchHistoryEvents({ commit }, { id, dataId }) {
    let historyEvents = (
      await api.preprocessings.getEventsById({
        id: id,
        dataId: dataId,
      })
    ).data
    commit('setHistoryEvents', historyEvents)
  },

  async fetchLogFile({ commit }, { id, dataId }) {
    let logFile = (
      await api.preprocessings.getFilesById({
        id: id,
        dataId: dataId,
        withUrl: true,
      })
    ).data
    commit('setLogFile', logFile)
  },

  // eslint-disable-next-line no-unused-vars
  async runById({ rootState }, { id, params }) {
    return await api.preprocessings.runById({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.preprocessings.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, { id, params }) {
    return await api.preprocessings.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async patch({ rootState }, { id, params }) {
    await api.preprocessings.patch({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.preprocessings.delete({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteHistory({ state }, { id, dataId }) {
    await api.preprocessings.deleteHistroyById({
      id: id,
      dataId: dataId,
    })
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

  setHistories(state, histories) {
    state.histories = histories
  },

  setHistoryDetail(state, historyDetail) {
    state.historyDetail = historyDetail
  },

  setHistoryEvents(state, historyEvents) {
    state.historyEvents = historyEvents
  },

  setLogFile(state, logFile) {
    state.logFile = logFile
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
