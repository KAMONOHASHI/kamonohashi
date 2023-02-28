import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  serviceTypes: Array<gen.NssolPlatypusInfrastructureInfosEnumInfo>
  registries: Array<gen.NssolPlatypusApiModelsRegistryApiModelsIndexOutputModel>
  detail: gen.NssolPlatypusApiModelsRegistryApiModelsDetailsOutputModel
}
// initial state
const state: StateType = {
  serviceTypes: [],
  registries: [],
  detail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  serviceTypes(state) {
    return state.serviceTypes
  },

  registries(state) {
    return state.registries
  },

  detail(state) {
    return state.detail
  },
}

// action
const actions: ActionTree<StateType, RootState> = {
  async fetchRegistries({ commit }) {
    let registries = (await api.registry.admin.get()).data
    commit('setRegistries', { registries })
  },

  async fetchTenantRegistries({ commit }) {
    //TODO テナントIDを渡していたが受け取らないAPIなので削除
    let registries = (await api.registry.tenant.getEndpoints()).data
    commit('setRegistries', { registries })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.registry.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.registry.admin.getType()).data
    commit('setServiceTypes', { serviceTypes })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsRegistryApiModelsCreateInputModel,
  ) {
    return await api.registry.admin.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsRegistryApiModelsCreateInputModel
    },
  ) {
    return await api.registry.admin.putById({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.registry.admin.deleteById({
      id: id,
    })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setServiceTypes(state, { serviceTypes }) {
    state.serviceTypes = serviceTypes
  },

  setRegistries(state, { registries }) {
    state.registries = registries
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
