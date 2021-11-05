import api from '@/api/api'

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

  async fetchDetail({ commit }, id) {
    let detail = (await api.user.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.user.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    if (params.serviceType === 1) {
      if (params.password) {
        await api.user.admin.putPassword({ id: id, password: params.password })
      }
    }
    return await api.user.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.user.admin.delete({ id: id })
  },

  async fetchTenantUsers({ commit }) {
    let tenantUsers = (await api.user.tenant.get()).data
    commit('setTenantUsers', { tenantUsers })
  },

  async fetchTenantUserDetail({ commit }, id) {
    let tenantUserDetail = (await api.user.tenant.getById({ id: id })).data
    commit('setTenantUserDetail', { tenantUserDetail })
  },

  // eslint-disable-next-line no-unused-vars
  async tenantRolesPut({ commit }, params) {
    return await api.user.tenant.putRoles(params)
  },

  // eslint-disable-next-line no-unused-vars
  async tenantUserDelete({ commit }, id) {
    return await api.user.tenant.delete({ id: id })
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
