import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'

import { RootState } from '../index'
interface StateType {
  version: { version: string; messages: Array<string> } | {}
}

// initial state
const state: StateType = {
  version: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  version(state) {
    return state.version
  },
}

// action
const actions: ActionTree<StateType, RootState> = {
  async fetchVersion({ commit }) {
    let response = await api.version.get()
    let version = response.data
    commit('setVersion', { version })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setVersion(state, { version }) {
    state.version = version
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
