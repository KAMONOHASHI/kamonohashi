import { GetterTree, ActionTree, MutationTree } from 'vuex'
import api from '@/api/api'
import Util from '@/util/util'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  histories: Array<gen.NssolPlatypusApiModelsNotebookApiModelsIndexOutputModel>
  total: number
  detail: gen.NssolPlatypusApiModelsNotebookApiModelsDetailsOutputModel
  availableInfiniteTime: boolean
  endpoint: gen.NssolPlatypusApiModelsNotebookApiModelsEndPointOutputModel
  events: gen.NssolPlatypusInfrastructureInfosContainerEventInfo
  fileList: Array<{
    isDirectory: boolean
    name?: string | null
    url?: string | null
    size?: string
    lastModified?: string
  }>
}

// initial state
const state = {
  histories: [],
  total: 0,
  detail: {},
  availableInfiniteTime: false,
  endpoint: {},
  events: {},
  fileList: [],
}

// getters
const getters: GetterTree<StateType, RootState> = {
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
  fileList(state) {
    return state.fileList
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchHistories(
    { commit },
    params: gen.NotebookApiApiV2NotebookGetRequest,
  ) {
    let response = await api.notebook.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.notebook.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchAvailableInfiniteTime({ commit }) {
    let availableInfiniteTime = (await api.notebook.getAvailableInfiniteTime())
      .data
    commit('setAvailableInfiniteTime', availableInfiniteTime)
  },

  async fetchEndpoint({ commit }, id: number) {
    let endpoint = (await api.notebook.getEndpointById({ id: id })).data
    commit('setEndpoint', endpoint)
  },

  async fetchEvents({ commit }, id: number) {
    let events = (await api.notebook.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsNotebookApiModelsCreateInputModel,
  ) {
    return await api.notebook.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postRerun(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      params,
    }: {
      id: number
      params: gen.NssolPlatypusApiModelsNotebookApiModelsRerunInputModel
    },
  ) {
    return await api.notebook.postRerun({ id: id, body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsNotebookApiModelsEditInputModel
    },
  ) {
    return await api.notebook.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id: number) {
    return await api.notebook.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    await api.notebook.deleteById({ id: id })
  },

  async fetchFileList(
    { commit },
    params: gen.NotebookApiApiV2NotebookIdContainerFilesGetRequest,
  ) {
    let response = (await api.notebook.getContainerFilesById(params)).data
    let newList: Array<{
      isDirectory: boolean
      name?: string | null
      url?: string | null
      size?: string
      lastModified?: string
    }> = []
    response.dirs!.forEach(d =>
      newList.push({
        isDirectory: true,
        name: d.dirName,
      }),
    )
    response.files!.forEach(f =>
      newList.push({
        isDirectory: false,
        name: f.fileName,
        url: f.url,
        size: Util.getByteString(f.size!),
        lastModified: f.lastModified,
      }),
    )
    commit('setFileList', newList)
  },
}

// mutations
const mutations: MutationTree<StateType> = {
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
