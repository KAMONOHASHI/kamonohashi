import api from '@/api/api'
import Util from '@/util/util'

// initial state
const state = {
  histories: [],
  total: 0,
  detail: {},
  events: {},
  tensorboard: {},
  fileList: [],
  preprocessHistories: {},
}

// getters
const getters = {
  histories(state) {
    return state.histories
  },
  preprocessHistories(state) {
    return state.preprocessHistories
  },
  total(state) {
    return state.total
  },
  detail(state) {
    return state.detail
  },
  events(state) {
    return state.events
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
    let response = await api.experiment.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchPreprocessHistories({ commit }, id) {
    let response = await api.experiment.getPreprocessById(id)
    let preprocessHistories = response.data
    commit('setPreprocessHistories', { preprocessHistories })
  },
  async fetchDetail({ commit }, id) {
    let detail = (await api.experiment.getById({ id: id })).data
    commit('setDetail', { detail })
  },
  async fetchEvents({ commit }, id) {
    let events = (await api.training.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.experiment.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.experiment.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id) {
    return await api.experiment.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ commit }, id) {
    return await api.experiment.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles({ commit }, { id, fileInfo }) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name
      await api.experiment.postFilesById({
        id: id,
        model: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.experiment.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }) {
    await api.experiment.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  // tensorboard関連
  async fetchTensorboard({ commit }, id) {
    let tensorboard = (
      await api.experiment.getTensorboardById({
        id: id,
        $config: { apiDisabledLoading: true },
      })
    ).data
    commit('setTensorboard', { tensorboard })
  },

  // eslint-disable-next-line no-unused-vars
  async putTensorboard({ commit }, params) {
    await api.experiment.putTensorboardById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTensorboard({ commit }, id) {
    await api.experiment.deleteTensorboardById({ id: id })
  },

  async fetchFileList({ commit }, params) {
    let response = (await api.experiment.getContainerFilesById(params)).data
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

  setDetail(state, { detail }) {
    state.detail = detail
  },
  setPreprocessHistories(state, { preprocessHistories }) {
    state.preprocessHistories = preprocessHistories
  },
  setEvents(state, { events }) {
    state.events = events
  },
  clearDetail(state) {
    state.detail = {}
  },

  setTensorboard(state, { tensorboard }) {
    state.tensorboard = tensorboard
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
