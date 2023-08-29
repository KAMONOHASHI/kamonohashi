import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'

interface StateType {
  storages: Array<gen.NssolPlatypusApiModelsStorageApiModelsIndexOutputModel>
  detail: gen.NssolPlatypusApiModelsStorageApiModelsDetailsOutputModel
  logUrl: null | string
}

// initial state
const state: StateType = {
  storages: [],
  detail: {},
  logUrl: null,
}

// getters
const getters: GetterTree<StateType, RootState> = {
  storages(state) {
    return state.storages
  },

  detail(state) {
    return state.detail
  },

  logUrl(state) {
    return state.logUrl
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchStorages({ commit }) {
    let storages = (await api.storage.admin.get()).data
    commit('setStorages', { storages })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.storage.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsStorageApiModelsCreateInputModel,
  ) {
    return await api.storage.admin.post({ body: params })
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
      params: gen.NssolPlatypusApiModelsStorageApiModelsCreateInputModel
    },
  ) {
    return await api.storage.admin.put({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    return await api.storage.admin.delete({ id: id })
  },

  async fetchLogUrl(
    { commit },
    params: gen.StorageApiApiV2DownloadUrlGetRequest,
  ) {
    //@ts-ignore
    let logUrl: string = (await api.storage.getDownloadUrl(params)).data.url
    commit('setLogUrl', { logUrl })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setStorages(state, { storages }) {
    state.storages = storages
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setLogUrl(state, { logUrl }) {
    state.logUrl = logUrl
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
