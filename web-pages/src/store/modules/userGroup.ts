import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  userGroups: Array<
    gen.NssolPlatypusApiModelsUserGroupApiModelsIndexOutputModel
  >
  detail: gen.NssolPlatypusApiModelsUserGroupApiModelsDetailsOutputModel
}

// initial state
const state: StateType = {
  userGroups: [],
  detail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  userGroups(state) {
    return state.userGroups
  },
  detail(state) {
    return state.detail
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchUserGroups({ commit }) {
    let userGroups = (await api.userGroup.admin.get()).data
    commit('setUserGroups', { userGroups })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.userGroup.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsUserGroupApiModelsCreateInputModel,
  ) {
    return await api.userGroup.admin.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsUserGroupApiModelsCreateInputModel
    },
  ) {
    return await api.userGroup.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.userGroup.admin.delete({ id: id })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setUserGroups(state, { userGroups }) {
    state.userGroups = userGroups
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
