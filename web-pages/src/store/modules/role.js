import api from '@/api/v1/api'

// initial state
const state = {
  roles: [],
  detail: {},
  tenantRoles: [],
  tenantRoleDetail: {},
}

// getters
const getters = {
  roles(state) {
    return state.roles
  },
  detail(state) {
    return state.detail
  },
  tenantRoles(state) {
    return state.tenantRoles
  },
  tenantRoleDetail(state) {
    return state.tenantRoleDetail
  },
}

// actions
const actions = {
  // admin
  async fetchRoles({ commit }) {
    let roles = (await api.role.admin.get()).data
    commit('setRoles', { roles })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.role.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.role.admin.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.role.admin.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.role.admin.delete({ id: id })
  },

  // tenant
  async fetchTenantRoles({ commit }) {
    let tenantRoles = (await api.role.tenant.get()).data
    commit('setTenantRoles', { tenantRoles })
  },

  async fetchTenantRoleDetail({ commit }, id) {
    let tenantRoleDetail = (await api.role.tenant.getById({ id: id })).data
    commit('setTenantRoleDetail', { tenantRoleDetail })
  },

  // eslint-disable-next-line no-unused-vars
  async postTenantRole({ commit }, params) {
    return await api.role.tenant.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async putTenantRole({ commit }, { id, params }) {
    return await api.role.tenant.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTenantRole({ commit }, id) {
    return await api.role.tenant.delete({ id: id })
  },
}

// mutations
const mutations = {
  setRoles(state, { roles }) {
    state.roles = roles
  },
  setDetail(state, { detail }) {
    state.detail = detail
  },
  setTenantRoles(state, { tenantRoles }) {
    state.tenantRoles = tenantRoles
  },
  setTenantRoleDetail(state, { tenantRoleDetail }) {
    state.tenantRoleDetail = tenantRoleDetail
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
