import api from '@/api/api'
import { GetterTree, ActionTree, MutationTree } from 'vuex'
import { RootState } from '../index'
import * as gen from '@/api/api.generate'
interface StateType {
  templates: Array<gen.NssolPlatypusApiModelsTemplateApiModelsIndexOutputModel>
  total: number
  versions: Array<
    gen.NssolPlatypusApiModelsTemplateApiModelsVersionIndexOutputModel
  >
  detail: gen.NssolPlatypusApiModelsTemplateApiModelsIndexOutputModel
  versionDetail: gen.NssolPlatypusApiModelsTemplateApiModelsVersionDetailsOutputModel
}

// initial state
const state = {
  templates: [],
  total: 0,
  versions: [],
  detail: {},
  versionDetail: {},
}

// getters
const getters: GetterTree<StateType, RootState> = {
  templates(state) {
    return state.templates
  },
  total(state) {
    return state.total
  },
  versions(state) {
    return state.versions
  },
  detail(state) {
    return state.detail
  },

  versionDetail(state) {
    return state.versionDetail
  },
}

// actions
const actions: ActionTree<StateType, RootState> = {
  async fetchModelTemplate({ commit }, id: number) {
    let detail = (await api.templates.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchModelTemplates(
    { commit },
    params: gen.TemplateApiApiV2TemplatesGetRequest,
  ) {
    let response = await api.templates.admin.get(params)
    let templates = response.data
    let total = response.headers['x-total-count']
    commit('setTemplates', { templates })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchTenantModelTemplates(
    { commit },
    params: gen.TemplateApiApiV2TenantTemplatesGetRequest,
  ) {
    let response = await api.templates.getTenantTemplate(params)
    let templates = response.data
    let total = response.headers['x-total-count']
    commit('setTemplates', { templates })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  // eslint-disable-next-line no-unused-vars
  async put(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsTemplateApiModelsEditInputModel
    },
  ) {
    return await api.templates.admin.put(params)
  },
  // eslint-disable-next-line no-unused-vars
  async post(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: gen.NssolPlatypusApiModelsTemplateApiModelsCreateInputModel,
  ) {
    return await api.templates.admin.post({ body: params })
  },
  async fetchVersions({ commit }, id: number) {
    let versions = (await api.templates.admin.getByIdVersions({ id: id })).data
    commit('setVersions', { versions })
  },
  async fetchDetail(
    { commit },
    params: {
      id: number
      versionId: number
    },
  ) {
    let detail = (await api.templates.admin.getByIdVersionsByVersionId(params))
      .data
    commit('setVersionDetail', { detail })
  },
  // eslint-disable-next-line no-unused-vars
  async postByIdVersions(
    // eslint-disable-next-line no-unused-vars
    { commit },
    params: {
      id: number
      body: gen.NssolPlatypusApiModelsTemplateApiModelsVersionCreateInputModel
    },
  ) {
    return await api.templates.admin.postByIdVersions(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id: number) {
    await api.templates.admin.delete({ id: id })
  },
  // eslint-disable-next-line no-unused-vars
  async deleteVersion(
    // eslint-disable-next-line no-unused-vars
    { state },
    { id, versionId }: { id: number; versionId: number },
  ) {
    await api.templates.admin.deleteVersion({ id: id, versionId: versionId })
  },
}

// mutations
const mutations: MutationTree<StateType> = {
  setTemplates(state, { templates }) {
    state.templates = templates
  },

  setDetail(state, { detail }) {
    state.detail = detail
  },
  setVersionDetail(state, { detail }) {
    state.versionDetail = detail
  },
  setVersions(state, { versions }) {
    state.versions = versions
  },

  setTotal(state, { total }) {
    state.total = total
  },

  clearDetail(state) {
    state.detail = {}
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
