import api from '@/api/v1/api'

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

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.tenant.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  async fetchCurrentTenant({ commit }) {
    let detail = (await api.tenant.get()).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.tenant.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.tenant.admin.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putCurrentTenant({ rootState }, params) {
    return await api.tenant.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ rootState }, params) {
    return await api.tenant.admin.delete(params)
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
