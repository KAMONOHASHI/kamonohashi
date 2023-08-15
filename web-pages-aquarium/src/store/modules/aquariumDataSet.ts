import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'

interface StateType {
  dataSets: Array<
    gen.NssolPlatypusApiModelsAquariumDataSetApiModelsIndexOutputModel
  >
  total: number
  versions: Array<
    gen.NssolPlatypusApiModelsAquariumDataSetApiModelsVersionIndexOutputModel
  >
  detailVersion: gen.NssolPlatypusApiModelsAquariumDataSetApiModelsVersionDetailsOutputModel
}
// initial state
const state: StateType = {
  dataSets: [],
  total: 0,
  versions: [],
  detailVersion: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  dataSets(state) {
    return state.dataSets
  },

  total(state) {
    return state.total
  },

  versions(state) {
    return state.versions
  },
  detailVersion(state) {
    return state.detailVersion
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchDataSets(
    { commit },
    params: gen.AquariumDataSetApiApiV2AquariumDatasetsGetRequest,
  ) {
    let response = await api.aquariumDatasets.get(params)
    let dataSets = response.data
    let total = response.headers['x-total-count']
    commit('setDataSets', { dataSets })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchVersions({ commit }, id: number) {
    if (id === null) {
      commit('clearVersions')
    } else {
      let versions = (await api.aquariumDatasets.getByIdVersions({ id: id }))
        .data
      commit('setVersions', { versions })
    }
  },

  async fetchDetailVersion(
    { commit },
    params: {
      id: number
      versionId: number
    },
  ) {
    if (params['id'] === null) {
      commit('clearDetailVersion')
    } else {
      let detailVersion = (
        await api.aquariumDatasets.getByIdVersionsByVersionId(params)
      ).data
      commit('setDetailVersion', { detailVersion })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsAquariumDataSetApiModelsCreateInputModel,
  ) {
    return await api.aquariumDatasets.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postByIdVersions(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: { id: number; body: { datasetId: number } },
  ) {
    return await api.aquariumDatasets.postByIdVersions({
      id: params.id,
      body: { dataSetId: params.body.datasetId },
    })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id: number) {
    await api.aquariumDatasets.delete({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteVersion({ state }, params: { id: number; versionId: number }) {
    await api.aquariumDatasets.deleteVersion({
      id: params.id,
      versionId: params.versionId,
    })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setDataSets(state, { dataSets }) {
    state.dataSets = dataSets
  },

  setTotal(state, total) {
    state.total = total
  },

  setVersions(state, { versions }) {
    state.versions = versions
  },
  setDetailVersion(state, { detailVersion }) {
    state.detailVersion = detailVersion
  },

  clearVersions(state) {
    state.versions = []
  },
  clearDetailVersion(state) {
    state.detailVersion = {}
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
