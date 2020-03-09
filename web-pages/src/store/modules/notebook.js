import api from '@/api/v1/api'
import Util from '@/util/util'

// initial state
const state = {
  histories: [],
  total: 0,
  detail: {},
  availableInfiniteTime: false,
  endpoint: null,
  events: {},
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
  detail(state) {
    return state.detail
  },
  availableInfiniteTime(state) {
    return state.availableInfiniteTime
  },
  endpoint(state) {
    return state.endpoint
  },
  events(state) {
    return state.events
  },
  partitions(state) {
    return state.partitions
  },
  fileList(state) {
    return state.fileList
  },
}

// actions
const actions = {
  async fetchHistories({ commit }, params) {
    let response = await api.notebook.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    commit('setTotal', parseInt(total))
  },

  async fetchDetail({ commit }, id) {
    let detail = (await api.notebook.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchAvailableInfiniteTime({ commit }) {
    let availableInfiniteTime = (await api.notebook.getAvailableInfiniteTime())
      .data
    commit('setAvailableInfiniteTime', availableInfiniteTime)
  },

  async fetchEndpoint({ commit }, id) {
    let endpoint = (await api.notebook.getEndpointById({ id: id })).data
    commit('setEndpoint', endpoint.url)
  },

  async fetchEvents({ commit }, id) {
    let events = (await api.notebook.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchPartitions({ commit }) {
    let partitions = (await api.cluster.getPartitions()).data
    commit('setPartitions', { partitions })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.notebook.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postRerun({ commit }, { id, params }) {
    return await api.notebook.postRerun({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.notebook.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id) {
    return await api.notebook.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.notebook.deleteById({ id: id })
  },

  async fetchFileList({ commit }, params) {
    let response = (await api.notebook.getContainerFilesById(params)).data
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

  setAvailableInfiniteTime(state, availableInfiniteTime) {
    state.availableInfiniteTime = availableInfiniteTime
  },

  setEndpoint(state, endpoint) {
    state.endpoint = endpoint
  },

  setEvents(state, { events }) {
    state.events = events
  },

  clearDetail(state) {
    state.detail = {}
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
