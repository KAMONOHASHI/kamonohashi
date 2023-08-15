import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  serviceTypes: Array<gen.NssolPlatypusInfrastructureInfosEnumInfo>
  endpoints: Array<gen.NssolPlatypusApiModelsGitApiModelsIndexOutputModel>
  detail: gen.NssolPlatypusApiModelsGitApiModelsDetailsOutputModel
}
// initial state
const state: StateType = {
  serviceTypes: [],
  endpoints: [],
  detail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  endpoints(state) {
    return state.endpoints
  },

  detail(state) {
    return state.detail
  },

  serviceTypes(state) {
    return state.serviceTypes
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchEndpoints({ commit }) {
    let endpoints = (await api.git.admin.getEndpoints()).data
    commit('setEndpoints', { endpoints })
  },

  async fetchTenantEndpoints({ commit }) {
    //TODO テナントIDを渡していたが受け取らないAPIなので削除
    let endpoints = (await api.git.tenant.getEndpoints()).data
    commit('setEndpoints', { endpoints })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.git.admin.getTypes()).data
    commit('setServiceTypes', { serviceTypes })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.git.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsGitApiModelsCreateInputModel,
  ) {
    return await api.git.admin.postEndpoint({ body: params })
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
      params: gen.NssolPlatypusApiModelsGitApiModelsCreateInputModel
    },
  ) {
    return await api.git.admin.putEndpoint({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.git.admin.deleteById({ id: id })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setEndpoints(state, { endpoints }) {
    state.endpoints = endpoints
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setServiceTypes(state, { serviceTypes }) {
    state.serviceTypes = serviceTypes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
