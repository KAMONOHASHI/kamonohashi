import api from '@/api/v1/api'
import Util from '@/util/util'

// initial state
const state = {
  histories: [],
  total: 0,
  selections: [],
  detail: {},
  parent: {},
  events: {},
  uploadedFiles: [],
  partitions: [],
  fileList: [],
}

// getters
const getters = {
  histories(state) {
    return state.histories
  },
  total(state) {
    return state.total
  },
  selections(state) {
    return state.selections
  },
  detail(state) {
    return state.detail
  },
  parent(state) {
    return state.parent
  },
  events(state) {
    return state.events
  },
  uploadedFiles(state) {
    return state.uploadedFiles
  },
  partitions(state) {
    return state.partitions
  },
  tensorboard(state) {
    return state.tensorboard
  },
  fileList(state) {
    return state.fileList
  },
}

// actions
const actions = {
  async fetchHistories({ commit }, params) {
    let response = await api.inference.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.inference.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // async fetchParent({ commit }, id) {
  //   let parent = (await api.inference.getById({ id: id })).data
  //   commit('setParent', { parent })
  // },

  async fetchEvents({ commit }, id) {
    let events = (await api.inference.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchUploadedFiles({ commit }, id) {
    let uploadedFiles = (
      await api.inference.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setUploadedFiles', { uploadedFiles })
  },

  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ rootState }, params) {
    return await api.inference.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.inference.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ state }, id) {
    return await api.inference.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ state }, id) {
    return await api.inference.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles({ state }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.inference.postFilesById({
        id: id,
        model: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.inference.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ state }, { id, fileId }) {
    await api.inference.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  async fetchFileList({ commit }, params) {
    let response = (await api.inference.getContainerFilesById(params)).data
    let newList = []
    response.dirs.forEach(d =>
      newList.push({
        isDirectory: true,
        name: d.dirName,
      }),
    )
    response.files.forEach(f =>
      newList.push({
        isDirectory: false,
        name: f.fileName,
        url: f.url,
        size: Util.getByteString(f.size),
        lastModified: f.lastModified,
      }),
    )
    commit('setFileList', newList)
  },
}

// mutations
const mutations = {
  setHistories(state, { histories }) {
    state.histories = histories
  },

  setTotal(state, total) {
    state.total = total
  },

  setSelections(state, selections) {
    state.selections = selections
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },

  setParent(state, { parent }) {
    state.parent = parent
  },

  setEvents(state, { events }) {
    state.events = events
  },

  setUploadedFiles(state, { uploadedFiles }) {
    state.uploadedFiles = uploadedFiles
  },

  clearDetail(state) {
    state.detail = {}
  },

  clearParent(state) {
    state.parent = {}
  },

  setPartitions(state, { partitions }) {
    state.partitions = partitions
  },

  setFileList(state, fileList) {
    state.fileList = fileList
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
