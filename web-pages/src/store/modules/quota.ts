import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  quotas: Array<gen.NssolPlatypusApiModelsClusterApiModelsQuotaOutputModel>
}

// initial state
const state: StateType = {
  quotas: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  quotas(state) {
    return state.quotas
  },
}

// action
const actions: ActionTree<StateType, RootState> = {
  async fetchQuotas({ commit }) {
    let response = await await api.quotas.get()
    let quotas = response.data
    commit('setQuotas', { quotas })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      body: gen.NssolPlatypusApiModelsClusterApiModelsQuotaInputModel[]
    },
  ) {
    return await api.quotas.post(params)
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setQuotas(state, { quotas }) {
    state.quotas = quotas
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
