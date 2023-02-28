import api from '@/api/api'
//const API_URL = 'http://' + process.env.VUE_APP_API_HOST //process.env

// initial state
type stateType = {
  data: Array<string>
  total: number
  detail: { name?: string }
  uploadedFiles: Array<string | null>
  tenantTags: Array<string | null>
}
const state: stateType = {
  data: [],
  total: 0,
  detail: {},
  uploadedFiles: [],
  tenantTags: [],
}

// getters
const getters = {
  data(state: stateType) {
    return state.data
  },

  total(state: stateType) {
    return state.total
  },

  detail(state: stateType) {
    return state.detail
  },

  uploadedFiles(state: stateType) {
    return state.uploadedFiles
  },

  tenantTags(state: stateType) {
    return state.tenantTags
  },
}

// actions
const actions = {
  async fetchData(
    { commit },
    params: {
      perPage: number
      page: number
      withTotal: boolean
      id?: string
      name?: string
      memo?: string
      createdAt?: string
      createdBy?: string
      tag?: string[]
    },
  ) {
    let response = await api.data.get(params)
    //let response = await dataApi.apiV2DataGet(params)

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
    params: { name: string; memo?: string; tags?: [string] },
  ) {
    return await api.data.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: { name: string; memo?: string; tags?: [string] },
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
    }: { id: number; fileInfo: [{ name: string; storedPath: string }] },
  ) {
    let body: { files: [{ fileName: string; storedPath: string }] } = {
      files: [],
    }
    for (let i = 0; i < fileInfo.length; i++) {
      body.files.push({
        fileName: fileInfo[i].name,
        storedPath: fileInfo[i].storedPath,
      })
    }
    await api.data.putFilesById({
      id: id,
      body: body,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id: number) {
    await api.data.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }: { id: number; fileId: string }) {
    await api.data.deleteFilesById({
      id: id,
      fileId: fileId,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async fetchFileSize({ state }, params: { id: number; name: string }) {
    return (await api.data.getFileSize(params)).data.fileSize
  },
}

// mutations
const mutations = {
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
