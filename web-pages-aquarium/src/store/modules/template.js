import api from '@/api/api'

// initial state
const state = {
  templates: [],
  total: 0,
  detail: {},
}

// getters
const getters = {
  templates(state) {
    return state.templates
  },
  total(state) {
    return state.total
  },

  detail(state) {
    return state.detail
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
  async fetchTenantModelTemplates({ commit }, params) {
    let response = await api.templates.admin.getTenantTemplate(params)
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
}

// mutations
const mutations = {
  setTemplates(state, { templates }) {
    state.templates = templates
  },

  setDetail(state, { detail }) {
    state.detail = detail
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
