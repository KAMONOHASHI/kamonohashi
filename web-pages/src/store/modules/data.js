import api from '@/api/v1/api'

// initial state
const state = {
  data: [],
  total: 0,
  detail: {},
  uploadedFiles: [],
  tenantTags: [],
}

// getters
const getters = {
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
const actions = {
  async fetchData({ commit }, params) {
    let response = await api.data.get(params)
    let data = response.data
    let total = response.headers['x-total-count']
    commit('setData', { data })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id) {
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

  async fetchUploadedFiles({ commit }, id) {
    let uploadedFiles = (
      await api.data.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setUploadedFiles', { uploadedFiles })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ state }, params) {
    return await api.data.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.data.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putFile({ commit }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.data.putFilesById({
        id: id,
        model: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.data.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }) {
    await api.data.deleteFilesById({
      id: id,
      fileId: fileId,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async putDataTags({ commit }, params) {
    await api.data.putDataTags(params)
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
