import api from '@/api/v1/api'

// initial state
const state = {
  templates: [],
  detail: {},
}

// getters
const getters = {
  templates(state) {
    return state.templates
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchPreprocessings({ commit }, params) {
    let response = await api.preprocessings.get(params)
    let preprocessings = response.data
    let total = response.headers['x-total-count']
    commit('setPreprocessings', { preprocessings })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
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
  async post({ commit }, params) {
    return await api.preprocessings.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.preprocessings.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.preprocessings.delete({ id: id })
  },
}

// mutations
const mutations = {
  setPreprocessings(state, { preprocessings }) {
    state.preprocessings = preprocessings
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  clearDetail(state) {
    state.detail = {}
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
