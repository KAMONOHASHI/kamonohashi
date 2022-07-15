import api from '@/api/api'

// initial state
const state = {
  tenants: [],
  detail: {},
}

// getters
const getters = {
  tenants(state) {
    return state.tenants
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchTenants({ commit }) {
    let tenants = (await api.tenant.admin.get()).data
    commit('setTenants', { tenants })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.tenant.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchCurrentTenant({ commit }) {
    let detail = (await api.tenant.get()).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.tenant.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.tenant.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async putCurrentTenant({ commit }, params) {
    return await api.tenant.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.tenant.admin.delete({ id: id })
  },
}

// mutations
const mutations = {
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
