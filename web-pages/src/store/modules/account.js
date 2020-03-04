import api from '@/api/v1/api'

// initial state
const state = {
  token: {},
  account: {},
}

// getters
const getters = {
  token(state) {
    return state.token
  },
  account(state) {
    return state.account
  },
}

// action
const actions = {
  async fetchAccount({ commit }) {
    let response = await api.account.get()
    let account = response.data
    commit('setAccount', { account })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.account.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putPassword({ rootState }, params) {
    return await api.account.putPassword(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postTokenTenants({ commit }, params) {
    let login = (await api.account.postTokenTenants(params)).data
    let token = login.token
    commit('setToken', { token })
  },

  // eslint-disable-next-line no-unused-vars
  async putGitToken({ rootState }, params) {
    return await api.account.putGits(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putRegistryToken({ rootState }, params) {
    return await api.account.putRegistries(params)
  },
}

// mutations
const mutations = {
  setToken(state, { token }) {
    state.token = token
  },
  setAccount(state, { account }) {
    state.account = account
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
