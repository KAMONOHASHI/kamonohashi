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

  async fetchModelTemplate2({ commit }, id) {
    let detail = (await api.templates.admin.getById2({ id: id })).data
    commit('setDetail', { detail })
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

  async fetchDetail({ commit }, id) {
    let detail = (await api.templates.admin.getById({ id: id })).data
    commit('setDetail', { detail })
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.templates.admin.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, { id, params }) {
    return await api.templates.admin.put({ id: id, model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.templates.admin.delete({ id: id })
  },
  async fetchModelTemplates2({ commit }, params) {
    let response = await api.templates.admin.get2(params)
    let templates = response.data
    let total = response.headers['x-total-count']
    commit('setTemplates', { templates })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  async fetchTenantModelTemplates2({ commit }, params) {
    let response = await api.templates.getTenantTemplate2(params)
    let templates = response.data
    let total = response.headers['x-total-count']
    commit('setTemplates', { templates })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },
  // eslint-disable-next-line no-unused-vars
  async put2({ commit }, params) {
    return await api.templates.admin.put2(params)
  },
  // eslint-disable-next-line no-unused-vars
  async post2({ commit }, params) {
    return await api.templates.admin.post2({ model: params })
  },
  async fetchVersions2({ commit }, id) {
    let versions = (await api.templates.admin.getByIdVersions2({ id: id })).data
    commit('setVersions', { versions })
  },
  async fetchDetail2({ commit }, params) {
    let detail = (await api.templates.admin.getByIdVersionsByVersionId2(params))
      .data
    commit('setVersionDetail', { detail })
  },
  // eslint-disable-next-line no-unused-vars
  async postByIdVersions2({ commit }, params) {
    return await api.templates.admin.postByIdVersions2(params)
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
