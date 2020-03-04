import api from '@/api/v1/api'

// initial state
const state = {
  users: [],
  detail: {},
  tenantUsers: [],
  tenantUserDetail: {},
}

// getters
const getters = {
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
const actions = {
  async fetchUsers({ commit }) {
    let users = (await api.user.admin.get()).data
    commit('setUsers', { users })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.user.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.user.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    if (params.serviceType === 1) {
      if (params.password) {
        await api.user.admin.putPassword(params)
      }
    }
    return await api.user.admin.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ rootState }, params) {
    return await api.user.admin.delete(params)
  },

  async fetchTenantUsers({ commit }) {
    let tenantUsers = (await api.user.tenant.get()).data
    commit('setTenantUsers', { tenantUsers })
  },

  async fetchTenantUserDetail({ commit, rootState }) {
    let tenantUserDetail = (
      await api.user.tenant.getById({ id: rootState.route.params.id })
    ).data
    commit('setTenantUserDetail', { tenantUserDetail })
  },

  // eslint-disable-next-line no-unused-vars
  async tenantRolesPut({ rootState }, params) {
    return await api.user.tenant.putRoles(params)
  },

  // eslint-disable-next-line no-unused-vars
  async tenantUserDelete({ rootState }, params) {
    return await api.user.tenant.delete(params)
  },
}

// mutations
const mutations = {
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
