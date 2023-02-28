import { GetterTree, ActionTree, MutationTree } from 'vuex'
import api from '@/api/api'
import Util from '@/util/util'
import * as gen from '@/api/api.generate'
import { RootState } from '../index'
interface StateType {
  histories: Array<gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel>
  total: number
  historiesToMount: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  >
  selections: any //TODO 使用されていない？
  detail: gen.NssolPlatypusApiModelsTrainingApiModelsDetailsOutputModel
  events: gen.NssolPlatypusInfrastructureInfosContainerEventInfo
  uploadedFiles: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsAttachedFileOutputModel
  >
  tenantTags: Array<string>
  tensorboard: gen.NssolPlatypusApiModelsTrainingApiModelsTensorBoardOutputModel
  fileList: Array<{
    isDirectory: boolean
    name: string
    url?: string
    size?: string
    lastModified?: string
  }>
  searchHistories: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsSearchHistoryOutputModel
  >
  searchFill: gen.NssolPlatypusApiModelsTrainingApiModelsTrainingSearchFillOutputModel
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
  tenantTags: [],
  tensorboard: {},
  fileList: [],
  searchHistories: [],
  searchFill: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
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
const actions: ActionTree<StateType, RootState> = {
  async fetchHistories(
    { commit },
    params: gen.TrainingApiApiV2TrainingGetRequest,
  ) {
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
  async fetchTrainHistories(
    { commit },
    params: gen.TrainingApiApiV2TrainingSearchGetRequest,
  ) {
    let response = await api.training.getSearch(params)
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
    params: gen.TrainingApiApiV2TrainingMountGetRequest,
  ) {
    let historiesToMount = (await api.training.getMount(params)).data
    commit('setHistoriesToMount', { historiesToMount })
  },

  async fetchDetail({ commit }, id: number) {
    let detail = (await api.training.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchEvents({ commit }, id: number) {
    let events = (await api.training.getEventsById({ id: id })).data
    commit('setEvents', { events })
  },

  async fetchUploadedFiles({ commit }, id: number) {
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
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTrainingApiModelsCreateInputModel,
  ) {
    return await api.training.post({ body: params })
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
    return await api.training.putById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postHalt({ commit }, id: number) {
    return await api.training.postHaltById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async postUserCancel({ commit }, id: number) {
    return await api.training.postUserCancelById({ id: id })
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
      fileInfo: Array<{
        name: string
        storedPath: string
      }>
    },
  ) {
    for (let i = 0; i < fileInfo.length; i++) {
      //fileInfo[i].FileName = fileInfo[i].name
      await api.training.postFilesById({
        id: id,
        body: {
          fileName: fileInfo[i].name,
          storedPath: fileInfo[i].storedPath,
        },
        //body:fileInfo[i],
      })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id: number) {
    await api.training.deleteById({ id: id })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteFile({ commit }, { id, fileId }: { id: number; fileId: number }) {
    await api.training.deleteByIdFilesByFileId({
      id: id,
      fileId: fileId,
    })
  },

  // tensorboard関連
  async fetchTensorboard({ commit }, id: number) {
    let tensorboard = (
      await api.training.getTensorboardById({
        id: id,
        //@ts-ignore
        $config: { apiDisabledLoading: true },
      })
    ).data
    commit('setTensorboard', { tensorboard })
  },

  // eslint-disable-next-line no-unused-vars
  async putTensorboard(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsTrainingApiModelsTensorBoardInputModel
    },
  ) {
    await api.training.putTensorboardById(params)
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTensorboard({ commit }, id: number) {
    await api.training.deleteTensorboardById({ id: id })
  },

  async fetchFileList(
    { commit },
    params: gen.TrainingApiApiV2TrainingIdContainerFilesGetRequest,
  ) {
    let response = (await api.training.getContainerFilesById(params)).data
    let newList: Array<{
      isDirectory: boolean
      name: string
      url?: string | undefined
      size?: string | undefined
      lastModified?: string | undefined
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
        size: Util.getByteString(f.size!),
        lastModified: f.lastModified,
      }),
    )
    commit('setFileList', newList)
  },

  // eslint-disable-next-line no-unused-vars
  async fetchFileSize(
    // eslint-disable-next-line no-unused-vars
    { state },
    params: gen.TrainingApiApiV2TrainingIdFilesNameSizeGetRequest,
  ) {
    return (await api.training.getFileSize(params)).data.fileSize
  },

  async fetchSearchHistories({ commit }) {
    let response = await api.training.getSearchHistory()
    let searchHistories = response.data
    commit('setSearchHistories', searchHistories)
  },

  // eslint-disable-next-line no-unused-vars
  async postTags(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTrainingApiModelsTagsInputModel,
  ) {
    await api.training.postTags({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteTags(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTrainingApiModelsTagsInputModel,
  ) {
    await api.training.deleteTags({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postSearchHistory(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTrainingApiModelsSearchHistoryInputModel,
  ) {
    return await api.training.postSearchHistory({ body: params })
  },

  // eslint-disable-next-line no-unused-vars
  async deleteSearchHistory({ commit }, id: number) {
    await api.training.deleteSearchHistoryById({ id: id })
  },

  async fetchSearchFill({ commit }) {
    let response = await api.training.getSearchFill()
    let searchFill = response.data
    commit('setSearchFill', searchFill)
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
