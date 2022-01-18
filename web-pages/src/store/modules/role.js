import api from '@/api/api'

// initial state
const state = {
  roles: [],
  detail: {},
  tenantRoles: [],
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
}

// actions
const actions = {
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
    return await api.role.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.role.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.role.admin.delete({ id: id })
  },

  async fetchTenantRoles({ commit }) {
    let tenantRoles = (await api.role.tenant.get()).data
    commit('setTenantRoles', { tenantRoles })
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
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
