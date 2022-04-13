import api from '@/api/api'
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
  tenantTags: [],
  tensorboard: {},
  fileList: [],
  searchHistories: [],
  searchFill: {},
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
  tenantTags(state) {
    return state.tenantTags
  },
  tensorboard(state) {
    return state.tensorboard
  },
  fileList(state) {
    return state.fileList
  },
  searchHistories(state) {
    return state.searchHistories
  },
  searchFill(state) {
    return state.searchFill
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

  //新しい検索で取得
  async fetchTrainHistories({ commit }, params) {
    let response = await api.training.getSearch(params)
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

  async fetchTenantTags({ commit }) {
    let tenantTags = (await api.training.getTags()).data
    commit('setTenantTags', tenantTags)
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.training.post({ body: params })
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
        body: fileInfo[i],
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
  async putTensorboard({ commit }, params) {
    await api.training.putTensorboardById(params)
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

  // eslint-disable-next-line no-unused-vars
  async fetchFileSize({ state }, params) {
    return (await api.training.getFileSize(params)).data.fileSize
  },

  async fetchSearchHistories({ commit }, params) {
    let response = await api.training.getSearchHistory(params)
    let searchHistories = response.data
    commit('setSearchHistories', searchHistories)
  },

  // eslint-disable-next-line no-unused-vars
  async postTags({ commit }, params) {
    await api.training.postTags({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTags({ commit }, params) {
    await api.training.deleteTags({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postSearchHistory({ commit }, params) {
    return await api.training.postSearchHistory({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteSearchHistory({ commit }, id) {
    await api.training.deleteSearchHistoryById({ id: id })
  },

  async fetchSearchFill({ commit }) {
    let response = await api.training.getSearchFill()
    let searchFill = response.data
    commit('setSearchFill', searchFill)
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

  setTenantTags(state, tenantTags) {
    state.tenantTags = tenantTags
  },

  setTensorboard(state, { tensorboard }) {
    state.tensorboard = tensorboard
  },

  setFileList(state, fileList) {
    state.fileList = fileList
  },

  setSearchHistories(state, searchHistories) {
    state.searchHistories = searchHistories
  },

  setSearchFill(state, searchFill) {
    state.searchFill = searchFill
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
