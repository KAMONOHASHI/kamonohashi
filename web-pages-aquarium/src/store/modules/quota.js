import api from '@/api/api'

// initial state
const state = {
  quotas: [],
}

// getters
const getters = {
  quotas(state) {
    return state.quotas
  },
}

// action
const actions = {
  async fetchQuotas({ commit }) {
    let response = await await api.quotas.get()
    let quotas = response.data
    commit('setQuotas', { quotas })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.quotas.post(params)
  },
}

// mutations
const mutations = {
  setQuotas(state, { quotas }) {
    state.quotas = quotas
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
