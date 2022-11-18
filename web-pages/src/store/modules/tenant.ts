import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  tenants: Array<gen.NssolPlatypusApiModelsTenantApiModelsIndexOutputModel>
  detail: gen.NssolPlatypusApiModelsTenantApiModelsDetailsOutputModel
}
// initial state
const state: StateType = {
  tenants: [],
  detail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  tenants(state) {
    return state.tenants
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchTenants({ commit }) {
    let tenants = (await api.tenant.admin.get()).data
    commit('setTenants', { tenants })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.tenant.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchCurrentTenant({ commit }) {
    let detail = (await api.tenant.get()).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTenantApiModelsCreateInputModel,
  ) {
    return await api.tenant.admin.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsTenantApiModelsEditInputModel
    },
  ) {
    return await api.tenant.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async putCurrentTenant(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      body: gen.NssolPlatypusApiModelsTenantApiModelsEditInputModel
    },
  ) {
    return await api.tenant.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.tenant.admin.delete({ id: id })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setTenants(state, { tenants }) {
    state.tenants = tenants
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
