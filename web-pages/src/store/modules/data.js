import api from '@/api/v1/api'

// initial state
const state = {
  data: {},
  detail: {},
}

// getters
const getters = {
  data(state) {
    return state.data
  },

  detail(state) {
    return state.detail
  },
}

// actions
const actions = {
  async fetchData({ commit }, params) {
    let data = (await api.data.get(params)).data
    commit('setData', { data })
  },

  async fetchDetail({ commit }, id) {
    if (id === null) {
      commit('clearDetail')
    } else {
      let detail = (await api.data.getById({ id: id })).data
      commit('setDetail', { detail })
    }
  },
}

// mutations
const mutations = {
  setData(state, { data }) {
    state.data = data
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
