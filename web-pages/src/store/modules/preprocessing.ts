import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  preprocessings: Array<
    gen.NssolPlatypusApiModelsPreprocessingApiModelsIndexOutputModel
  >
  total: number
  detail: gen.NssolPlatypusApiModelsPreprocessingApiModelsDetailsOutputModel
  histories: Array<
    gen.NssolPlatypusApiModelsPreprocessingApiModelsHistoriesOutputModel
  >
  historyDetail: gen.NssolPlatypusApiModelsPreprocessingApiModelsHistoryDetailsOutputModel
  historyEvents: gen.NssolPlatypusApiModelsPreprocessingApiModelsHistoriesOutputModel
  logFile: gen.NssolPlatypusApiModelsPreprocessingApiModelsPreprocessAttachedFileOutputModel
}

// initial state
const state: StateType = {
  preprocessings: [],
  total: 0,
  detail: {},
  histories: [],
  historyDetail: {},
  historyEvents: {},
  logFile: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
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
const actions: ActionTree<StateType, RootState> = {
  async fetchPreprocessings(
    { commit },
    params: gen.PreprocessingApiApiV2PreprocessingsGetRequest,
  ) {
    let response = await api.preprocessings.get(params)
    let preprocessings = response.data
    let total = response.headers['x-total-count']
    commit('setPreprocessings', { preprocessings })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.preprocessings.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchHistories({ commit }, id: number) {
    let histories = (await api.preprocessings.getHistory({ id: id })).data
    commit('setHistories', histories)
  },

  async fetchHistoryDetail(
    { commit },
    { id, dataId }: { id: number; dataId: number },
  ) {
    let historyDetail = (
      await api.preprocessings.getHistroyById({
        id: id,
        dataId: dataId,
      })
    ).data
    commit('setHistoryDetail', historyDetail)
  },

  async fetchHistoryEvents(
    { commit },
    { id, dataId }: { id: number; dataId: number },
  ) {
    let historyEvents = (
      await api.preprocessings.getEventsById({
        id: id,
        dataId: dataId,
      })
    ).data
    commit('setHistoryEvents', historyEvents)
  },

  async fetchLogFile(
    { commit },
    { id, dataId }: { id: number; dataId: number },
  ) {
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
  async runById(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsPreprocessingApiModelsRunPreprocessHistoryInputModel
    },
  ) {
    return await api.preprocessings.runById({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel,
  ) {
    return await api.preprocessings.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel
    },
  ) {
    return await api.preprocessings.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async patch(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsPreprocessingApiModelsEditInputModel
    },
  ) {
    await api.preprocessings.patch({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    await api.preprocessings.delete({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteHistory(
    // eslint-disable-next-line no-unused-vars
    { commit },
    { id, dataId }: { id: number; dataId: number },
  ) {
    await api.preprocessings.deleteHistroyById({
      id: id,
      dataId: dataId,
    })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
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
