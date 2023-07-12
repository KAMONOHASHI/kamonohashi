import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import * as gen from '@/api/api.generate'

import { RootState } from '../index'

interface StateType {
  dataSets: Array<gen.NssolPlatypusApiModelsDataSetApiModelsIndexOutputModel>
  total: number
  detail: gen.NssolPlatypusApiModelsDataSetApiModelsIndexOutputModel
  dataTypes: Array<
    gen.NssolPlatypusApiModelsDataSetApiModelsDataTypeOutputModel
  >
}
// initial state
const state: StateType = {
  dataSets: [],
  total: 0,
  detail: {},
  dataTypes: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  dataSets(state) {
    return state.dataSets
  },

  total(state) {
    return state.total
  },

  detail(state) {
    return state.detail
  },

  dataTypes(state) {
    return state.dataTypes
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchDataSets(
    { commit },
    params: gen.DataSetApiApiV2DatasetsGetRequest,
  ) {
    let response = await api.datasets.get(params)
    let dataSets = response.data
    let total = response.headers['x-total-count']
    commit('setDataSets', { dataSets })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id: number) {
    if (id === null) {
      commit('clearDetail')
    } else {
      let detail = (await api.datasets.getById({ id: id })).data
      commit('setDetail', { detail })
    }
  },

  async fetchDataTypes({ commit }) {
    let dataTypes = (await api.datasets.getDatatypes()).data
    commit('setDataTypes', dataTypes)
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsDataSetApiModelsCreateInputModel,
  ) {
    return await api.datasets.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsDataSetApiModelsEditEntriesInputModel
    },
  ) {
    return await api.datasets.put({ id: id, body: params })
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
      params: gen.NssolPlatypusApiModelsDataSetApiModelsEditInputModel
    },
  ) {
    await api.datasets.patch({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    await api.datasets.delete({ id: id })
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

  setDetail(state, { detail }) {
    state.detail = detail
  },

  clearDetail(state) {
    state.detail = {}
  },

  setDataTypes(state, dataTypes) {
    state.dataTypes = dataTypes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
