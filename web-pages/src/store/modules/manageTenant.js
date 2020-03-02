import api from '@/api/v1/api'

// initial state
const state = {
  tenant: {},
}

// getters
const getters = {
  tenant(state) {
    return state.tenant
  },
}

// actions
const actions = {
  async fetchTenant({ commit }) {
    let tenant = (await api.tenant.get()).data
    commit('setTenant', { tenant })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.tenant.put(params)
  },
}

// mutations
const mutations = {
  setTenant(state, { tenant }) {
    state.tenant = tenant
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
