import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  nodes: Array<
    gen.NssolPlatypusApiModelsResourceApiModelsNodeResourceOutputModel
  >
  tenants: Array<
    gen.NssolPlatypusApiModelsResourceApiModelsTenantResourceOutputModel
  >
  containerLists: Array<
    gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsOutputModel
  >
  detail: gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsOutputModel
  events: gen.SystemIOStream
  containerLog: gen.SystemIOStream
  tenantNodes: Array<
    gen.NssolPlatypusApiModelsResourceApiModelsNodeResourceOutputModel
  >
  tenantContainerLists: Array<
    gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsForTenantOutputModel
  >
  tenantDetail: gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsForTenantOutputModel
  tenantContainerLog: gen.SystemIOStream
  historiesContainersMetadata: gen.NssolPlatypusApiModelsResourceApiModelsHistoryMetadataOutputModel
  historiesContainersData: [] //TODO voidが返る？
  historiesJobsMetadata: gen.NssolPlatypusApiModelsResourceApiModelsHistoryMetadataOutputModel
  historiesJobsData: [] //TODO voidが返る？
}
// initial state
const state: StateType = {
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
  historiesContainersMetadata: {},
  historiesContainersData: [],
  historiesJobsMetadata: {},
  historiesJobsData: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
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
  historiesContainersMetadata(state) {
    return state.historiesContainersMetadata
  },
  historiesContainersData(state) {
    return state.historiesContainersData
  },
  historiesJobsMetadata(state) {
    return state.historiesJobsMetadata
  },
  historiesJobsData(state) {
    return state.historiesJobsData
  },
}

// action
const actions: ActionTree<StateType, RootState> = {
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

  async fetchDetail(
    { commit },
    params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameGetRequest,
  ) {
    let detail = (await api.resource.admin.getContainerByName(params)).data
    commit('setDetail', { detail })
    let events = (await api.resource.admin.getContainerEventsByName(params))
      .data
    commit('setEvents', { events })
  },

  async fetchContainerLog(
    { commit },
    params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameLogGetRequest,
  ) {
    let response = await api.resource.admin.getContainerLogByName(params)
    let containerLog = response.data
    commit('setContainerLog', { containerLog })
  },

  // eslint-disable-next-line no-unused-vars
  async delete(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameDeleteRequest,
  ) {
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

  async fetchTenantDetail(
    { commit },
    params: gen.ResourceApiApiV2TenantResourceContainersNameGetRequest,
  ) {
    let tenantDetail = (await api.resource.tenant.getContainerByName(params))
      .data
    commit('setTenantDetail', { tenantDetail })
  },

  async fetchTenantContainerLog(
    { commit },
    params: gen.ResourceApiApiV2TenantResourceContainersNameLogGetRequest,
  ) {
    let tenantContainerLog = (
      await api.resource.tenant.getContainerLogByName(params)
    ).data
    commit('setTenantContainerLog', { tenantContainerLog })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTenantContainer(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.ResourceApiApiV2TenantResourceContainersNameDeleteRequest,
  ) {
    return await api.resource.tenant.deleteContainerByName(params)
  },

  async fetchHistoriesContainersMetadata({ commit }) {
    //TODO テナントIDを渡していたが受け取らないAPIなので削除
    let historiesContainersMetadata = (
      await api.resource.admin.getHistoriesContainersMetadata()
    ).data
    commit('setHistoriesContainersMetadata', { historiesContainersMetadata })
  },

  async fetchHistoriesContainersData(
    { commit },
    params: gen.ResourceApiApiV2AdminResourceHistoriesContainersDataGetRequest,
  ) {
    let historiesContainersData = await api.resource.admin.getHistoriesContainersData(
      params,
    )
    commit('setHistoriesContainersData', historiesContainersData.data)
  },

  // eslint-disable-next-line no-unused-vars
  async deleteHistoriesContainers(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel,
  ) {
    return await api.resource.admin.deleteHistoriesContainers({ body: params })
  },

  async fetchHistoriesJobsMetadata({ commit }) {
    //TODO テナントIDを渡していたが受け取らないAPIなので削除
    let historiesJobsMetadata = (
      await api.resource.admin.getHistoriesJobsMetadata()
    ).data
    commit('setHistoriesJobsMetadata', { historiesJobsMetadata })
  },

  async fetchHistoriesJobsData(
    { commit },
    params: gen.ResourceApiApiV2AdminResourceHistoriesJobsDataGetRequest,
  ) {
    let historiesJobsData = (
      await api.resource.admin.getHistoriesJobsData(params)
    ).data
    commit('setHistoriesJobsData', historiesJobsData)
  },

  // eslint-disable-next-line no-unused-vars
  async deleteHistoriesJobs(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel,
  ) {
    return await api.resource.admin.deleteHistoriesJobs({ body: params })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
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
  setHistoriesContainersMetadata(state, { historiesContainersMetadata }) {
    state.historiesContainersMetadata = historiesContainersMetadata
  },
  setHistoriesContainersData(state, historiesContainersData) {
    state.historiesContainersData = historiesContainersData
  },
  setHistoriesJobsMetadata(state, { historiesJobsMetadata }) {
    state.historiesJobsMetadata = historiesJobsMetadata
  },
  setHistoriesJobsData(state, historiesJobsData) {
    state.historiesJobsData = historiesJobsData
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
