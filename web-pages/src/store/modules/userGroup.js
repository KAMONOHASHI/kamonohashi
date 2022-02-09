import api from '@/api/api'

// initial state
const state = {
  userGroups: [],
  detail: {},
}

// getters
const getters = {
  userGroups(state) {
    return state.userGroups
  },
  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchUserGroups({ commit }) {
    let userGroups = (await api.userGroup.admin.get()).data
    commit('setUserGroups', { userGroups })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.userGroup.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.userGroup.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.userGroup.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.userGroup.admin.delete({ id: id })
  },
}

// mutations
const mutations = {
  setUserGroups(state, { userGroups }) {
    state.userGroups = userGroups
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
