import api from '@/api/v1/api'

// initial state
const state = {
  dataSets: [],
  total: 0,
  detail: {},
}

// getters
const getters = {
  dataSets(state) {
    return state.dataSets
  },

  total(state) {
    return state.total
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchDataSets({ commit }, params) {
    let response = await api.datasets.get(params)
    let dataSets = response.data
    let total = response.headers['x-total-count']
    commit('setDataSets', { dataSets })
    commit('setTotal', parseInt(total))
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

  setTotal(state, total) {
    state.total = total
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
