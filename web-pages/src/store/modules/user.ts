import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  users: Array<gen.NssolPlatypusApiModelsUserApiModelsIndexForAdminOutputModel>
  detail: gen.NssolPlatypusApiModelsUserApiModelsIndexForAdminOutputModel
  tenantUsers: Array<
    gen.NssolPlatypusApiModelsUserApiModelsIndexForTenantOutputModel
  >
  tenantUserDetail: gen.NssolPlatypusApiModelsUserApiModelsIndexForTenantOutputModel
}
// initial state
const state: StateType = {
  users: [],
  detail: {},
  tenantUsers: [],
  tenantUserDetail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  users(state) {
    return state.users
  },

  detail(state) {
    return state.detail
  },

  tenantUsers(state) {
    return state.tenantUsers
  },

  tenantUserDetail(state) {
    return state.tenantUserDetail
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchUsers({ commit }) {
    let users = (await api.user.admin.get()).data
    commit('setUsers', { users })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.user.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsUserApiModelsCreateInputModel,
  ) {
    return await api.user.admin.post({ body: params })
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
      params: {
        name: string
        password: string
        systemRoles: Array<number>
        tenants: Array<{
          id: number
          default: boolean
          roles: Array<number>
        }>
        serviceType: number
      }
    },
  ) {
    if (params.serviceType === 1) {
      if (params.displayName) {
        await api.user.admin.putDisplayName({
          id: id,
          body: params.displayName,
        })
      }
      if (params.password) {
        await api.user.admin.putPassword({ id: id, body: params.password })
      }
    }
    return await api.user.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.user.admin.delete({ id: id })
  },

  async fetchTenantUsers({ commit }) {
    let tenantUsers = (await api.user.tenant.get()).data
    commit('setTenantUsers', { tenantUsers })
  },

  async fetchTenantUserDetail({ commit }, id: number) {
    let tenantUserDetail = (await api.user.tenant.getById({ id: id })).data
    commit('setTenantUserDetail', { tenantUserDetail })
  },

  // eslint-disable-next-line no-unused-vars
  async tenantRolesPut(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: Array<number>
    },
  ) {
    return await api.user.tenant.putRoles(params)
  },

  // eslint-disable-next-line no-unused-vars
  async tenantUserDelete({ commit }, id: number) {
    return await api.user.tenant.delete({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async syncLdapUsers(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      body: gen.NssolPlatypusApiModelsUserApiModelsLdapAuthenticationInputModel
    },
  ) {
    return (await api.user.admin.postSyncLdap(params)).data
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setUsers(state, { users }) {
    state.users = users
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setTenantUsers(state, { tenantUsers }) {
    state.tenantUsers = tenantUsers
  },

  setTenantUserDetail(state, { tenantUserDetail }) {
    state.tenantUserDetail = tenantUserDetail
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
