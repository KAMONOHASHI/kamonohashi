import api from '@/api/v1/api'

// initial state
const state = {
  nodeData: [],
  detail: {},
  tenant: {},
}

// getters
const getters = {
  nodeData(state) {
    return state.nodeData
  },

  detail(state) {
    return state.detail
  },

  tenant(state) {
    return state.tenant
  },
}

// action
const actions = {
  async fetchNodeData({ commit }) {
    let params = {}
    params.withTotal = true
    let nodeData = await api.nodes.admin.get(params)
    commit('setNodeData', { nodeData })
  },

  async fetchDetail({ commit, rootState }) {
    let detail = (
      await api.nodes.admin.getById({ id: rootState.route.params.id })
    ).data
    commit('setDetail', { detail })
  },

  async fetchTenant({ commit }) {
    let tenant = await api.tenant.admin.get()
    commit('setTenant', { tenant })
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
  setNodeData(state, { nodeData }) {
    state.nodeData = nodeData
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

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
