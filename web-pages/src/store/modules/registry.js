import api from '@/api/v1/api'

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
    let serviceTypes = (await api.registry.admin.getType()).data
    let registries = (await api.registry.admin.get()).data
    for (let i = 0; i < registries.length; i++) {
      let serviceTypeId = registries[i].serviceType
      // ServiceTypeの数値から表示名に変換
      registries[i] = {
        ...registries[i],
        serviceType: serviceTypes.find(s => s.id === serviceTypeId).name,
      }
    }
    commit('setRegistries', { registries })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.registry.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  async fetchServiceTypes({ commit }) {
    let serviceTypes = (await api.registry.admin.getType()).data
    commit('setServiceTypes', { serviceTypes })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.registry.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.registry.admin.putById(params)
  },
  async delete({ rootState }) {
    return await api.registry.admin.deleteById({
      id: rootState.route.params.id,
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
