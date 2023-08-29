import { GetterTree, ActionTree, MutationTree } from 'vuex'

import api from '@/api/api'
import * as gen from '@/api/api.generate'
import { RootState } from '../index'

interface StateType {
  dataSets: Array<gen.NssolPlatypusApiModelsDataSetApiModelsIndexOutputModel>
  total: number
  dataTotal: number
  detail: gen.NssolPlatypusApiModelsDataSetApiModelsDetailsOutputModel
  dataTypes: Array<
    gen.NssolPlatypusApiModelsDataSetApiModelsDataTypeOutputModel
  >
  data: Array<gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel>
}

// initial state
const state = {
  dataSets: [],
  total: 0,
  dataTotal: 0,
  detail: {},
  dataTypes: [],
  data: [],
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

  data(state) {
    return state.data
  },

  dataTotal(state) {
    return state.dataTotal
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchData({ commit }, params: gen.DataApiApiV2DataGetRequest) {
    let response = await api.data.get(params)
    let data = response.data
    let total = response.headers['x-total-count']
    commit('setData', { data })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setDataTotal', parseInt(total))
    }
  },
  async fetchDataSets({ commit }, params) {
    let response = await api.datasets.get(params)
    let dataSets = response.data
    let total = response.headers['x-total-count']
    commit('setDataSets', { dataSets })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id) {
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
  async post({ commit }, params) {
    return await api.datasets.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.datasets.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async patch({ commit }, { id, params }) {
    await api.datasets.patch({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.datasets.delete({ id: id })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setData(state, { data }) {
    state.data = data
  },

  setDataSets(state, { dataSets }) {
    state.dataSets = dataSets
  },

  setTotal(state, total) {
    state.total = total
  },
  setDataTotal(state, total) {
    state.dataTotal = total
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
