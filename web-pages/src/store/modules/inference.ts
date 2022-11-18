import { GetterTree, ActionTree, MutationTree } from 'vuex'
import api from '@/api/api'
import Util from '@/util/util'
import * as gen from '@/api/api.generate'
import { RootState } from '../index'

interface StateType {
  histories: Array<
    gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
  >
  total: number
  historiesToMount: Array<
    gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
  >
  selections: [] //TODO 使用されていない？
  detail: gen.NssolPlatypusApiModelsInferenceApiModelsInferenceDetailsOutputModel
  events: gen.NssolPlatypusInfrastructureInfosContainerEventInfo
  uploadedFiles: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsAttachedFileOutputModel
  >
  fileList: Array<{
    isDirectory: boolean
    name: string
    url?: string
    size?: string
    lastModified?: string
  }>
}
// initial state
const state: StateType = {
  histories: [],
  total: 0,
  historiesToMount: [],
  selections: [],
  detail: {},
  events: {},
  uploadedFiles: [],
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
  historiesToMount(state) {
    return state.historiesToMount
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
    return state.tensorboard //TODO 存在しないのでここを消したい
  },
  fileList(state) {
    return state.fileList
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchHistories(
    { commit },
    params: gen.InferenceApiApiV2InferencesGetRequest,
  ) {
    let response = await api.inference.get(params)
    let histories = response.data
    let total = response.headers['x-total-count']
    commit('setHistories', { histories })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchHistoriesToMount(
    { commit },
    params: gen.InferenceApiApiV2InferencesMountGetRequest,
  ) {
    let historiesToMount = (await api.inference.getMount(params)).data
    commit('setHistoriesToMount', { historiesToMount })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.inference.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchEvents({ commit }, id: number) {
    let events = (await api.inference.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchUploadedFiles({ commit }, id: number) {
    let uploadedFiles = (
      await api.inference.getFilesById({
        id: id,
        withUrl: true,
      })
    ).data
    commit('setUploadedFiles', { uploadedFiles })
  },

  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsInferenceApiModelsCreateInputModel,
  ) {
    return await api.inference.post({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsTrainingApiModelsEditInputModel
    },
  ) {
    return await api.inference.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id: number) {
    return await api.inference.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ commit }, id: number) {
    return await api.inference.postUserCancelById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postFiles(
    // eslint-disable-next-line no-unused-vars
    { commit },
    {
      id,
      fileInfo,
    }: {
      id: number
      fileInfo: Array<gen.NssolPlatypusApiModelsComponentsAddFileInputModel> //TODO 型を編集する必要がある？
    },
  ) {
    for (let i = 0; i < fileInfo.length; i++) {
      fileInfo[i].FileName = fileInfo[i].name //TODO name は存在しない?
      await api.inference.postFilesById({
        id: id,
        body: fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    await api.inference.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }: { id: number; fileId: number }) {
    await api.inference.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  async fetchFileList(
    { commit },
    params: gen.InferenceApiApiV2InferencesIdContainerFilesGetRequest,
  ) {
    let response = (await api.inference.getContainerFilesById(params)).data
    let newList: Array<{
      isDirectory: boolean
      name: string
      url?: string
      size?: string
      lastModified?: string
    }> = []
    response.dirs!.forEach(d =>
      newList.push({
        isDirectory: true,
        name: d.dirName!,
      }),
    )
    response.files!.forEach(f =>
      newList.push({
        isDirectory: false,
        name: f.fileName!,
        url: f.url!,
        size: Util.getByteString(f.size),
        lastModified: f.lastModified,
      }),
    )
    commit('setFileList', newList)
  },

  // eslint-disable-next-line no-unused-vars
  async fetchFileSize({ state }, params) {
    return (await api.inference.getFileSize(params)).data.fileSize
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
