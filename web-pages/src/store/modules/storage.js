import api from '@/api/v1/api'

// initial state
const state = {
  storages: [],
  detail: {},
}

// getters
const getters = {
  storages(state) {
    return state.storages
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchStorages({ commit }) {
    let storages = (await api.storage.admin.get()).data
    commit('setStorages', { storages })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.storage.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.storage.admin.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.storage.admin.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.storage.admin.delete({ id: id })
  },
}

// mutations
const mutations = {
  setStorages(state, { storages }) {
    state.storages = storages
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
