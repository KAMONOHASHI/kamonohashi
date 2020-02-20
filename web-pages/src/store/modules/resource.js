// initial state
const state = {
  resource: {
    cpu: 1,
    memory: 0,
    gpu: 0,
  },
}

// getters
const getters = {
  resource(state) {
    return state.resource
  },
}

// actions
const actions = {}

// mutations
const mutations = {
  setResource(state, resource) {
    state.resource = resource
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
