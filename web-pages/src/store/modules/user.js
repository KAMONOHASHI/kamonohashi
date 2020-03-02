import api from '@/api/v1/api'

// initial state
const state = {
  users: [],
  detail: {},
}

// getters
const getters = {
  users(state) {
    return state.users
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchUsers({ commit }) {
    let users = (await api.user.admin.get()).data
    commit('setUsers', { users })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.user.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.user.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    if (params.serviceType === 1) {
      if (params.password) {
        await api.user.admin.putPassword(params)
      }
    }
    return await api.user.admin.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ rootState }, params) {
    return await api.user.admin.delete(params)
  },
}

// mutations
const mutations = {
  setUsers(state, { users }) {
    state.users = users
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
