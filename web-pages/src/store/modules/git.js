import api from '@/api/v1/api'

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
    let serviceTypes = (await api.git.admin.getTypes()).data
    for (let i = 0; i < endpoints.length; i++) {
      let serviceTypeId = endpoints[i].serviceType

      // ServiceTypeの数値から表示名に変換
      endpoints[i] = {
        ...endpoints[i],
        serviceType: serviceTypes.find(s => s.id === serviceTypeId).name,
      }
    }
    commit('setEndpoints', { endpoints })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.git.admin.getTypes()).data
    commit('setServiceTypes', { serviceTypes })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.git.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.git.admin.postEndpoint(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.git.admin.putEndpoint(params)
  },

  async delete({ rootState }) {
    return await api.git.admin.deleteById({ id: rootState.route.params.id })
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
