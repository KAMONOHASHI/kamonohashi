import api from '@/api/api'

// initial state
const state = {
  version: {},
}

// getters
const getters = {
  version(state) {
    return state.version
  },
}

// action
const actions = {
  async fetchVersion({ commit }) {
    let response = await api.version.get()
    let version = response.data
    commit('setVersion', { version })
  },
}

// mutations
const mutations = {
  setVersion(state, { version }) {
    state.version = version
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
