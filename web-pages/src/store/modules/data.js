import api from '@/api/v1/api'

// initial state
const state = {
  data: {},
  detail: {},
  uploadedFiles: [],
  tags: [],
}

// getters
const getters = {
  data(state) {
    return state.data
  },

  detail(state) {
    return state.detail
  },

  uploadedFiles(state) {
    return state.uploadedFiles
  },

  tags(state) {
    return state.tags
  },
}

// actions
const actions = {
  async fetchData({ commit }, params) {
    let data = (await api.data.get(params)).data
    commit('setData', { data })
  },

  async fetchDetail({ commit }, id) {
    if (id === null) {
      commit('clearDetail')
    } else {
      let detail = (await api.data.getById({ id: id })).data
      commit('setDetail', { detail })
    }
  },

  async fetchDataTags({ commit }) {
    let tags = (await api.data.getDataTags()).data
    commit('setTags', tags)
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
  async put({ rootState }, params) {
    return await api.data.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putFile({ rootState }, { id, fileInfo }) {
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
  async deleteFile({ rootState }, { id, fileId }) {
    await api.data.deleteFilesById({
      id: id,
      fileId: fileId,
    })
  },
}

// mutations
const mutations = {
  setData(state, { data }) {
    state.data = data
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

  setTags(state, tags) {
    state.tags = tags
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
