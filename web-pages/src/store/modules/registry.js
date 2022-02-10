import api from '@/api/api'

// initial state
const state = {
  serviceTypes: [],
  registries: [],
  detail: {},
}

// getters
const getters = {
  serviceTypes(state) {
    return state.serviceTypes
  },

  registries(state) {
    return state.registries
  },

  detail(state) {
    return state.detail
  },
}

// action
const actions = {
  async fetchRegistries({ commit }) {
    let registries = (await api.registry.admin.get()).data
    commit('setRegistries', { registries })
  },

  async fetchTenantRegistries({ commit }, tenantId) {
    let registries = (await api.registry.tenant.getEndpoints({ id: tenantId }))
      .data
    commit('setRegistries', { registries })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.registry.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.registry.admin.getType()).data
    commit('setServiceTypes', { serviceTypes })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.registry.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.registry.admin.putById({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.registry.admin.deleteById({
      id: id,
    })
  },
}

// mutations
const mutations = {
  setServiceTypes(state, { serviceTypes }) {
    state.serviceTypes = serviceTypes
  },

  setRegistries(state, { registries }) {
    state.registries = registries
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
