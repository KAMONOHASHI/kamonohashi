import api from '@/api/v1/api'

// initial state
const state = {
  serviceTypes: [],
  registrys: [],
  detail: {},
}

// getters
const getters = {
  serviceTypes(state) {
    return state.serviceTypes
  },

  registrys(state) {
    return state.registrys
  },

  detail(state) {
    return state.detail
  },
}

// action
const actions = {
  async fetchRegistrys({ commit }) {
    let serviceTypes = (await api.registry.admin.getType()).data
    let registrys = (await api.registry.admin.get()).data
    for (let i = 0; i < registrys.length; i++) {
      let serviceTypeId = registrys[i].serviceType
      // ServiceTypeの数値から表示名に変換
      registrys[i] = {
        ...registrys[i],
        serviceType: serviceTypes.find(s => s.id === serviceTypeId).name,
      }
    }
    commit('setRegistrys', { registrys })
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

  setRegistrys(state, { registrys }) {
    state.registrys = registrys
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
