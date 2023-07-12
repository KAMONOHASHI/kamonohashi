import { GetterTree, ActionTree, MutationTree } from 'vuex'
import * as gen from '@/api/api.generate'
import api from '@/api/api'
import { RootState } from '../index'

interface StateType {
  data: Array<gen.NssolPlatypusApiModelsDataSetApiModelsIndexOutputModel>
  total: number
  detail: gen.NssolPlatypusApiModelsDataApiModelsDetailsOutputModel
  uploadedFiles: Array<
    gen.NssolPlatypusApiModelsDataApiModelsDataFileOutputModel
  >
  tenantTags: Array<string>
}
// initial state
const state = {
  data: [],
  total: 0,
  detail: {},
  uploadedFiles: [],
  tenantTags: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
  data(state) {
    return state.data
  },

  total(state) {
    return state.total
  },

  detail(state) {
    return state.detail
  },

  uploadedFiles(state) {
    return state.uploadedFiles
  },

  tenantTags(state) {
    return state.tenantTags
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchData({ commit }, params: gen.DataApiApiV2DataGetRequest) {
    let response = await api.data.get(params)
    let data = response.data
    let total = response.headers['x-total-count']
    commit('setData', { data })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id: number) {
    if (id === null) {
      commit('clearDetail')
    } else {
      let detail = (await api.data.getById({ id: id })).data
      commit('setDetail', { detail })
    }
  },

  async fetchTenantTags({ commit }) {
    let tenantTags = (await api.data.getDataTags()).data
    commit('setTenantTags', tenantTags)
  },
  async clearUploadedFiles({ commit }) {
    commit('clearUploadedFiles')
  },
  async fetchUploadedFiles({ commit }, id: number) {
    let uploadedFiles = (
      await api.data.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setUploadedFiles', { uploadedFiles })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { state },
    params: gen.NssolPlatypusApiModelsDataApiModelsCreateInputModel,
  ) {
    return await api.data.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsDataApiModelsEditInputModel
    },
  ) {
    return await api.data.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putFile(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      fileInfo,
    }: {
      id: number
      fileInfo: Array<{
        name: string
        storedPath: string | null | undefined
      }>
    },
  ) {
    let model: {
      files: Array<{
        fileName: string
        storedPath: string
      }>
    } = { files: [] }
    for (let i = 0; i < fileInfo.length; i++) {
      model.files.push({
        fileName: fileInfo[i].name,
        storedPath: fileInfo[i].storedPath!,
      })
    }
    await api.data.putFilesById({
      id: id,
      body: model,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id: number) {
    await api.data.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }: { id: number; fileId: number }) {
    await api.data.deleteFilesById({
      id: id,
      fileId: fileId,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async fetchFileSize(
    // eslint-disable-next-line no-unused-vars
    { state },
    params: gen.DataApiApiV2DataIdFilesNameSizeGetRequest,
  ) {
    return (await api.data.getFileSize(params)).data.fileSize
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setData(state, { data }) {
    state.data = data
  },

  setTotal(state, total) {
    state.total = total
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  clearDetail(state) {
    state.detail = {}
  },

  setUploadedFiles(state, { uploadedFiles }) {
    state.uploadedFiles = uploadedFiles
  },

  clearUploadedFiles(state) {
    state.uploadedFiles = []
  },

  setTenantTags(state, tenantTags) {
    state.tenantTags = tenantTags
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
