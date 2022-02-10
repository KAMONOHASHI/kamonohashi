import api from '@/api/api'

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
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.nodes.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.nodes.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.nodes.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    return await api.nodes.admin.delete({ id: id })
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
