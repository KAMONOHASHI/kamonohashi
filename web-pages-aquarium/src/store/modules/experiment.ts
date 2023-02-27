import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  experiments: Array<
    gen.NssolPlatypusApiModelsExperimentApiModelsIndexOutputModel
  >
  total: number
  detail: gen.NssolPlatypusApiModelsExperimentApiModelsDetailsOutputModel

  logFiles: any //TODO generateにない？
  preprocessLogFiles: any //TODO generateにない？
  evaluations: Array<
    gen.NssolPlatypusApiModelsExperimentApiModelsEvaluationIndexOutputModel
  >
}
// initial state
const state: StateType = {
  experiments: [],
  total: 0,
  detail: {},
  logFiles: {},
  preprocessLogFiles: {},
  evaluations: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  experiments(state) {
    return state.experiments
  },
  detail(state) {
    return state.detail
  },

  total(state) {
    return state.total
  },

  logFiles(state) {
    return state.logFiles
  },
  preprocessLogFiles(state) {
    return state.preprocessLogFiles
  },

  evaluations(state) {
    return state.evaluations
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchExperiments(
    { commit },
    params: gen.ExperimentApiApiV2ExperimentGetRequest,
  ) {
    let response = await api.experiment.get(params)
    let experiments = response.data
    let total = response.headers['x-total-count']
    commit('setExperiments', { experiments })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchDetail({ commit }, id: number) {
    let detail = (await api.experiment.getById({ id: id })).data
    commit('setDetail', { detail })
  },
  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.experiment.post({ body: params })
  },
  /*
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
  async postUserCancel({ commit }, id: number) {
    return await api.experiment.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles({ commit }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.experiment.postFilesById({
        id: id,
        body: fileInfo[i],
      })
    }
  },
*/
  async delete({ commit }, id: number) {
    await api.experiment.deleteById({ id: id })
    commit('clearDetail')
    commit('clearEvaluations')
  },
  /*
  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }) {
    await api.experiment.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },
*/
  async fetchEvaluations({ commit }, id: number) {
    let response = await api.experiment.getEvaluationsById({ id: id })
    let evaluations = response.data
    commit('setEvaluations', { evaluations })
  },

  // eslint-disable-next-line no-unused-vars
  async postEvaluations(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsExperimentApiModelsEvaluationCreateInputModel
    },
  ) {
    return await api.experiment.postEvaluationsById({
      id: id,
      body: params,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteEvaluations(
    // eslint-disable-next-line no-unused-vars
    { commit },
    { id, evaluationId }: { id: number; evaluationId: number },
  ) {
    await api.experiment.deleteByIdEvaluationsByEvaluationId({
      id: id,
      evaluationId: evaluationId,
    })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setExperiments(state, { experiments }) {
    state.experiments = experiments
  },

  setTotal(state, total) {
    state.total = total
  },

  setDetail(state, { detail }) {
    state.detail = detail
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

  setEvaluations(state, { evaluations }) {
    state.evaluations = evaluations
  },
  clearEvaluations(state) {
    state.evaluations = []
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
