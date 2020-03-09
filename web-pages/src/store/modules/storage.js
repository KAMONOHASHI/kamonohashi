import api from '@/api/v1/api'

// initial state
const state = {
  storages: [],
  detail: {},
  logUrl: null,
}

// getters
const getters = {
  storages(state) {
    return state.storages
  },

  detail(state) {
    return state.detail
  },

  logUrl(state) {
    return state.logUrl
  },
}

// actions
const actions = {
  async fetchStorages({ commit }) {
    let storages = (await api.storage.admin.get()).data
    commit('setStorages', { storages })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.storage.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.storage.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.storage.admin.put(params)
  },

  async delete({ rootState }) {
    return await api.storage.admin.delete({ id: rootState.route.params.id })
  },

  async fetchLogUrl({ commit }, params) {
    let logUrl = (await api.storage.getDownloadUrl(params)).data.url
    commit('setLogUrl', { logUrl })
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

  setLogUrl(state, { logUrl }) {
    state.logUrl = logUrl
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
