import api from '@/api/v1/api'

// initial state
const state = {
  nodes: [],
  tenants: [],
  containerLists: [],
  detail: {},
  events: [],
  containerLog: [],
}

// getters
const getters = {
  nodes(state) {
    return state.nodes
  },
  tenants(state) {
    return state.tenants
  },
  containerLists(state) {
    return state.containerLists
  },
  detail(state) {
    return state.detail
  },
  events(state) {
    return state.events
  },
  containerLog(state) {
    return state.containerLog
  },
}

// action
const actions = {
  async fetchNodes({ commit }) {
    let response = await api.resource.admin.getNodes()
    let nodes = response.data
    commit('setNodes', { nodes })
  },

  async fetchTenants({ commit }) {
    let response = await api.resource.admin.getTenants()
    let tenants = response.data
    commit('setTenants', { tenants })
  },

  async fetchContainerLists({ commit }) {
    let response = await api.resource.admin.getContainers()
    let containerLists = response.data
    commit('setContainerLists', { containerLists })
  },

  async fetchDetail({ commit }, params) {
    let detail = (await api.resource.admin.getContainerByName(params)).data
    commit('setDetail', { detail })
    let events = (await api.resource.admin.getContainerEventsByName(params))
      .data
    commit('setEvents', { events })
  },

  async fetchContainerLog({ commit }, params) {
    let response = await api.resource.admin.getContainerLogByName(params)
    let containerLog = response.data
    commit('setContainerLog', { containerLog })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ rootState }, params) {
    return await api.resource.admin.deleteContainerByName(params)
  },
}

// mutations
const mutations = {
  setNodes(state, { nodes }) {
    state.nodes = nodes
  },
  setTenants(state, { tenants }) {
    state.tenants = tenants
  },
  setContainerLists(state, { containerLists }) {
    state.containerLists = containerLists
  },
  setDetail(state, { detail }) {
    state.detail = detail
  },
  setEvents(state, { events }) {
    state.events = events
  },
  setContainerLog(state, { containerLog }) {
    state.containerLog = containerLog
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
