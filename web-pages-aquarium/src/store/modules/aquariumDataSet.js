import api from '@/api/api'

// initial state
const state = {
  dataSets: [],
  total: 0,
  versions: [],
  detailVersion: {},
  dataTypes: [],
}

// getters
const getters = {
  dataSets(state) {
    return state.dataSets
  },

  total(state) {
    return state.total
  },

  versions(state) {
    return state.versions
  },
  detailVersion(state) {
    return state.detailVersion
  },
  dataTypes(state) {
    return state.dataTypes
  },
}

// actions
const actions = {
  async fetchDataSets({ commit }, params) {
    let response = await api.aquariumDatasets.get(params)
    let dataSets = response.data
    let total = response.headers['x-total-count']
    commit('setDataSets', { dataSets })
    // params.withTotal=trueの時は件数が取れているため設定
    if (total !== undefined) {
      commit('setTotal', parseInt(total))
    }
  },

  async fetchVersions({ commit }, id) {
    if (id === null) {
      commit('clearVersions')
    } else {
      let versions = (await api.aquariumDatasets.getByIdVersions({ id: id }))
        .data
      commit('setVersions', { versions })
    }
  },

  async fetchDetailVersion({ commit }, params) {
    if (params['id'] === null) {
      commit('clearDetailVersion')
    } else {
      let detailVersion = (
        await api.aquariumDatasets.getByIdVersionsByVersionId({
          id: params['id'],
          versionId: params['versionId'],
        })
      ).data
      commit('setDetailVersion', { detailVersion })
    }
  },

  // eslint-disable-next-line no-unused-vars
  async post({ commit }, params) {
    return await api.aquariumDatasets.post({ model: params })
  },

  // eslint-disable-next-line no-unused-vars
  async postByIdVersions({ commit }, params) {
    return await api.aquariumDatasets.postByIdVersions({
      id: params.id,
      model: params.model,
    })
  },

  // eslint-disable-next-line no-unused-vars
  async delete({ commit }, id) {
    await api.aquariumDatasets.delete({ id: id })
  },
}

// mutations
const mutations = {
  setDataSets(state, { dataSets }) {
    state.dataSets = dataSets
  },

  setTotal(state, total) {
    state.total = total
  },

  setVersions(state, { versions }) {
    state.versions = versions
  },
  setDetailVersion(state, { detailVersion }) {
    state.detailVersion = detailVersion
  },

  clearVersions(state) {
    state.versions = {}
  },
  clearDetailVersion(state) {
    state.detailVersion = {}
  },
  setDataTypes(state, dataTypes) {
    state.dataTypes = dataTypes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
