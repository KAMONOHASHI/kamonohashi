import api from '@/api/v1/api'

// initial state
const state = {
  dataSets: [],
  detail: {},
}

// getters
const getters = {
  dataSets(state) {
    return state.dataSets
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchDataSets({ commit }) {
    let dataSets = (await api.datasets.get()).data
    commit('setDataSets', { dataSets })
  },

  async fetchDetail({ commit }, id) {
    if (id === null) {
      commit('clearDetail')
    } else {
      let detail = (await api.datasets.getById({ id: id })).data
      commit('setDetail', { detail })
    }
  },
}

// mutations
const mutations = {
  setDataSets(state, { dataSets }) {
    state.dataSets = dataSets
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  clearDetail(state) {
    state.detail = {}
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
