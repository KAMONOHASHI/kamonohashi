import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  roles: Array<gen.NssolPlatypusApiModelsRoleApiModelsIndexOutputModel>
  detail: gen.NssolPlatypusApiModelsRoleApiModelsDetailsOutputModel
  tenantRoles: Array<gen.NssolPlatypusApiModelsRoleApiModelsIndexOutputModel>
}
// initial state
const state: StateType = {
  roles: [],
  detail: {},
  tenantRoles: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  roles(state) {
    return state.roles
  },

  detail(state) {
    return state.detail
  },

  tenantRoles(state) {
    return state.tenantRoles
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchRoles({ commit }) {
    let roles = (await api.role.admin.get()).data
    commit('setRoles', { roles })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.role.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsRoleApiModelsCreateInputModel,
  ) {
    return await api.role.admin.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsRoleApiModelsEditInputModel
    },
  ) {
    return await api.role.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.role.admin.delete({ id: id })
  },

  async fetchTenantCommonRoles({ commit }) {
    let tenantRoles = (await api.role.admin.getTenantCommonRoles()).data
    commit('setTenantRoles', { tenantRoles })
  },

  async fetchTenantRoles({ commit }) {
    let tenantRoles = (await api.role.tenant.get()).data
    commit('setTenantRoles', { tenantRoles })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setRoles(state, { roles }) {
    state.roles = roles
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setTenantRoles(state, { tenantRoles }) {
    state.tenantRoles = tenantRoles
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
