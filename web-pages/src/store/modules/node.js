import api from '@/api/v1/api'

// initial state
const state = {
  nodes: [],
  total: 0,
  detail: {},
  tenants: {},
}

// getters
const getters = {
  nodes(state) {
    return state.nodes
  },

  total(state) {
    return state.total
  },

  detail(state) {
    return state.detail
  },

  tenants(state) {
    return state.tenants
  },
}

// action
const actions = {
  async fetchNodes({ commit }, params) {
    let response = await api.nodes.admin.get(params)
    let nodes = response.data
    let total = response.headers['x-total-count']
    commit('setNodes', { nodes })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.nodes.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  async fetchTenants({ commit }) {
    let tenants = (await api.tenant.admin.get()).data
    commit('setTenants', { tenants })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.nodes.admin.post(params)
  },

  async put({ rootState }, params) {
    params['id'] = rootState.route.params.id
    return await api.nodes.admin.put(params)
  },

  async delete({ rootState }) {
    return await api.nodes.admin.delete({ id: rootState.route.params.id })
  },
}

// mutations
const mutations = {
  setNodes(state, { nodes }) {
    state.nodes = nodes
  },

  setTotal(state, total) {
    state.total = total
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setTenants(state, { tenants }) {
    state.tenants = tenants
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
