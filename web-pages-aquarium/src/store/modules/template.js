import api from '@/api/api'

// initial state
const state = {
  templates: [],
  total: 0,
  versions: [],
  detail: {},
  versionDetail: {},
}

// getters
const getters = {
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
const actions = {
  async fetchModelTemplate({ commit }, id) {
    let detail = (await api.templates.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  async fetchModelTemplates({ commit }, params) {
    let response = await api.templates.admin.get(params)
    let templates = response.data
    let total = response.headers['x-total-count']
    commit('setTemplates', { templates })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchTenantModelTemplates({ commit }, params) {
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
  async put({ commit }, params) {
    return await api.templates.admin.put(params)
  },
  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.templates.admin.post({ model: params })
  },
  async fetchVersions({ commit }, id) {
    let versions = (await api.templates.admin.getByIdVersions({ id: id })).data
    commit('setVersions', { versions })
  },
  async fetchDetail({ commit }, params) {
    let detail = (await api.templates.admin.getByIdVersionsByVersionId(params))
      .data
    commit('setVersionDetail', { detail })
  },
  // eslint-disable-next-line no-unused-vars
  async postByIdVersions({ commit }, params) {
    return await api.templates.admin.postByIdVersions(params)
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ state }, id) {
    await api.templates.admin.delete({ id: id })
  },
  // eslint-disable-next-line no-unused-vars
  async deleteVersion({ state }, { id: id, versionId: versionId }) {
    await api.templates.admin.deleteVersion({ id: id, versionId: versionId })
  },
}

// mutations
const mutations = {
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
