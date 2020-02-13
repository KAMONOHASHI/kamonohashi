// initial state
const state = {
  variables: [{ key: '', value: '' }],
}

// getters
const getters = {
  variables(state) {
    return state.variables
  },
}

// actions
const actions = {}

// mutations
const mutations = {
  addVariables(state, kvp) {
    state.variables.push(kvp)
  },

  removeVariables(state, index) {
    state.variables.splice(index, 1)
  },

  setVariables(state, variables) {
    state.variables = variables
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
