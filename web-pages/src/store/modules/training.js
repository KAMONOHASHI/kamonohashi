import api from '@/api/v1/api'

// initial state
const state = {
  histories: [],
  total: 0,
  selections: [],
}

// getters
const getters = {
  histories(state) {
    return state.histories
  },
  total(state) {
    return state.total
  },
  selections(state) {
    return state.selections
  },
}

// actions
const actions = {
  async fetchHistories({ commit }, params) {
    let response = await api.training.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    commit('setTotal', parseInt(total))
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.training.deleteById({ id: id })
  },
}

// mutations
const mutations = {
  setHistories(state, { histories }) {
    state.histories = histories
  },
  setTotal(state, total) {
    state.total = total
  },
  setSelections(state, selections) {
    state.selections = selections
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
