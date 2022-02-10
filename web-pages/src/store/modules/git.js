import api from '@/api/api'

// initial state
const state = {
  serviceTypes: [],
  endpoints: [],
  detail: {},
}

// getters
const getters = {
  endpoints(state) {
    return state.endpoints
  },

  detail(state) {
    return state.detail
  },

  serviceTypes(state) {
    return state.serviceTypes
  },
}

// actions
const actions = {
  async fetchEndpoints({ commit }) {
    let endpoints = (await api.git.admin.getEndpoints()).data
    commit('setEndpoints', { endpoints })
  },

  async fetchTenantEndpoints({ commit }, tenantId) {
    let endpoints = (await api.git.tenant.getEndpoints({ id: tenantId })).data
    commit('setEndpoints', { endpoints })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.git.admin.getTypes()).data
    commit('setServiceTypes', { serviceTypes })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.git.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.git.admin.postEndpoint({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.git.admin.putEndpoint({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.git.admin.deleteById({ id: id })
  },
}

// mutations
const mutations = {
  setEndpoints(state, { endpoints }) {
    state.endpoints = endpoints
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setServiceTypes(state, { serviceTypes }) {
    state.serviceTypes = serviceTypes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
