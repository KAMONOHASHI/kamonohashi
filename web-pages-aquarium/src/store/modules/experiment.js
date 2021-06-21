import api from '@/api/api'

// initial state
const state = {
  experiments: [],
  total: 0,
  detail: {},
  events: {},
  tensorboard: {},
  preprocessHistories: {},
  logFiles: {},
  preprocessLogFiles: {},
  evaluations: [],
}

// getters
const getters = {
  experiments(state) {
    return state.experiments
  },
  detail(state) {
    return state.detail
  },

  preprocessHistories(state) {
    return state.preprocessHistories
  },
  total(state) {
    return state.total
  },

  events(state) {
    return state.events
  },
  logFiles(state) {
    return state.logFiles
  },
  preprocessLogFiles(state) {
    return state.preprocessLogFiles
  },
  tensorboard(state) {
    return state.tensorboard
  },

  evaluations(state) {
    return state.evaluations
  },
}

// actions
const actions = {
  async fetchExperiments({ commit }, params) {
    let response = await api.experiment.get(params)
    let experiments = response.data
    let total = response.headers['x-total-count']
    commit('setExperiments', { experiments })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchDetail({ commit }, id) {
    let detail = (await api.experiment.getById({ id: id })).data
    commit('setDetail', { detail })
  },
  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.experiment.post({ model: params })
  },
  // eslint-disable-next-line no-unused-vars
  async postPreprocessingComplete({ commit }, { id, params }) {
    return await api.experiment.postPreprocessingCompleteById({
      id: id,
      model: params,
    })
  },

  async fetchPreprocessHistories({ commit }, id) {
    let response = await api.experiment.getPreprocessById(id)
    let preprocessHistories = response.data
    commit('setPreprocessHistories', { preprocessHistories })
  },

  async fetchEvents({ commit }, id) {
    let events = (await api.experiment.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchLogFiles({ commit }, id) {
    commit('clearLogfiles')
    let logFiles = (
      await api.experiment.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setLogFiles', { logFiles })
  },
  async fetchPreprocessLogFiles({ commit }, id) {
    let logFiles = (
      await api.experiment.getPreprocessFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setPreprocessLogFiles', { logFiles })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.experiment.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id) {
    return await api.experiment.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ commit }, id) {
    return await api.experiment.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles({ commit }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.experiment.postFilesById({
        id: id,
        model: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.experiment.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }) {
    await api.experiment.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  async fetchEvaluations({ commit }, id) {
    let response = await api.experiment.getEvoluationsById({ id: id })
    let evaluations = response.data
    commit('setEvaluations', { evaluations })
  },

  // eslint-disable-next-line no-unused-vars
  async postEvoluations({ commit }, { id, params }) {
    return await api.experiment.postEvoluationsById({
      id: id,
      model: params,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteEvoluations({ commit }, { id, evaluationId }) {
    await api.experiment.deleteByIdEvoluationsByEvaluationId({
      id: id,
      evaluationId: evaluationId,
    })
  },
}

// mutations
const mutations = {
  setExperiments(state, { experiments }) {
    state.experiments = experiments
  },

  setHistories(state, { histories }) {
    state.histories = histories
  },

  setTotal(state, total) {
    state.total = total
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },
  setPreprocessHistories(state, { preprocessHistories }) {
    state.preprocessHistories = preprocessHistories
  },
  setEvents(state, { events }) {
    state.events = events
  },
  setLogFiles(state, { logFiles }) {
    state.logFiles = logFiles
  },
  setPreprocessLogFiles(state, { logFiles }) {
    state.preprocessLogFiles = logFiles
  },
  clearDetail(state) {
    state.detail = {}
  },
  clearLogfiles(state) {
    state.preprocessLogFiles = {}
    state.logFiles = {}
  },

  setTensorboard(state, { tensorboard }) {
    state.tensorboard = tensorboard
  },
  setEvaluations(state, { evaluations }) {
    state.evaluations = evaluations
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
