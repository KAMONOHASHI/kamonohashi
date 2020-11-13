import api from '@/api/v1/api'

// initial state
const state = {
  nodes: [],
  tenants: [],
  containerLists: [],
  detail: {},
  events: [],
  containerLog: [],
  tenantNodes: [],
  tenantContainerLists: [],
  tenantDetail: {},
  tenantContainerLog: [],
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
  tenantNodes(state) {
    return state.tenantNodes
  },
  tenantContainerLists(state) {
    return state.tenantContainerLists
  },
  tenantDetail(state) {
    return state.tenantDetail
  },
  tenantContainerLog(state) {
    return state.tenantContainerLog
  },
}

// action
const actions = {
  // admin系
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
  async delete({ commit }, params) {
    return await api.resource.admin.deleteContainerByName(params)
  },

  // tenant系
  async fetchTenantNodes({ commit }) {
    let tenantNodes = (await api.resource.tenant.getNodes()).data
    commit('setTenantNodes', { tenantNodes })
  },

  async fetchTenantContainerLists({ commit }) {
    let tenantContainerLists = (await api.resource.tenant.getContainers()).data
    commit('setTenantContainerLists', { tenantContainerLists })
  },

  async fetchTenantDetail({ commit }, params) {
    let tenantDetail = (await api.resource.tenant.getContainerByName(params))
      .data
    commit('setTenantDetail', { tenantDetail })
  },

  async fetchTenantContainerLog({ commit }, params) {
    let tenantContainerLog = (
      await api.resource.tenant.getContainerLogByName(params)
    ).data
    commit('setTenantContainerLog', { tenantContainerLog })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTenantContainer({ commit }, params) {
    return await api.resource.tenant.deleteContainerByName(params)
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
  setTenantNodes(state, { tenantNodes }) {
    state.tenantNodes = tenantNodes
  },
  setTenantContainerLists(state, { tenantContainerLists }) {
    state.tenantContainerLists = tenantContainerLists
  },
  setTenantDetail(state, { tenantDetail }) {
    state.tenantDetail = tenantDetail
  },
  setTenantContainerLog(state, { tenantContainerLog }) {
    state.tenantContainerLog = tenantContainerLog
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
