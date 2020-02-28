import api from '@/api/v1/api'

// initial state
const state = {
  roles: [],
  detail: {},
}

// getters
const getters = {
  roles(state) {
    return state.roles
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchRoles({ commit }) {
    let roles = (await api.role.admin.get()).data
    commit('setRoles', { roles })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.role.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.role.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.role.admin.put(params)
  },

  async delete({ rootState }) {
    return await api.role.admin.delete({ id: rootState.route.params.id })
  },
}

// mutations
const mutations = {
  setRoles(state, { roles }) {
    state.roles = roles
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