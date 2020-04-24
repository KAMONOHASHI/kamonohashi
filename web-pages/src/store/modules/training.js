import api from '@/api/v1/api'
import Util from '@/util/util'

// initial state
const state = {
  histories: [],
  total: 0,
  historiesToMount: [],
  selections: [],
  detail: {},
  events: {},
  uploadedFiles: [],
  tensorboard: {},
  fileList: [],
}

// getters
const getters = {
  histories(state) {
    return state.histories
  },
  historiesToMount(state) {
    return state.historiesToMount
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
  events(state) {
    return state.events
  },
  uploadedFiles(state) {
    return state.uploadedFiles
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
    let response = await api.training.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchHistoriesToMount({ commit }, params) {
    let historiesToMount = (await api.training.getMount(params)).data
    commit('setHistoriesToMount', { historiesToMount })
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.training.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchEvents({ commit }, id) {
    let events = (await api.training.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchUploadedFiles({ commit }, id) {
    let uploadedFiles = (
      await api.training.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setUploadedFiles', { uploadedFiles })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.training.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.training.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id) {
    return await api.training.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ commit }, id) {
    return await api.training.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles({ commit }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.training.postFilesById({
        id: id,
        model: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.training.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }) {
    await api.training.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  // tensorboard関連
  async fetchTensorboard({ commit }, id) {
    let tensorboard = (
      await api.training.getTensorboardById({
        id: id,
        $config: { apiDisabledLoading: true },
      })
    ).data
    commit('setTensorboard', { tensorboard })
  },

  // eslint-disable-next-line no-unused-vars
  async putTensorboard({ commit }, id) {
    await api.training.putTensorboardById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTensorboard({ commit }, id) {
    await api.training.deleteTensorboardById({ id: id })
  },

  async fetchFileList({ commit }, params) {
    let response = (await api.training.getContainerFilesById(params)).data
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

  setHistoriesToMount(state, { historiesToMount }) {
    state.historiesToMount = historiesToMount
  },

  setSelections(state, selections) {
    state.selections = selections
  },

  setDetail(state, { detail }) {
    state.detail = detail
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

  setTensorboard(state, { tensorboard }) {
    state.tensorboard = tensorboard
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
