import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  nodes: Array<gen.NssolPlatypusApiModelsNodeApiModelsIndexOutputModel>
  total: number
  detail: gen.NssolPlatypusApiModelsNodeApiModelsDetailsOutputModel
  tenants: {}
}

// initial state
const state: StateType = {
  nodes: [],
  total: 0,
  detail: {},
  tenants: {}, //TODO 使用されていない
}

// getters
const getters: GetterTree<StateType, RootState> = {
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
const actions: ActionTree<StateType, RootState> = {
  async fetchNodes({ commit }, params: gen.NodeApiApiV2AdminNodesGetRequest) {
    let response = await api.nodes.admin.get(params)
    let nodes = response.data
    let total = response.headers['x-total-count']
    commit('setNodes', { nodes })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.nodes.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsNodeApiModelsCreateInputModel,
  ) {
    return await api.nodes.admin.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsNodeApiModelsCreateInputModel
    },
  ) {
    return await api.nodes.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.nodes.admin.delete({ id: id })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
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
