import api from '@/api/v1/api'

// initial state
const state = {
  registries: [],
  defaultRegistryId: null,
  images: [],
  tags: [],
}

// getters
const getters = {
  registries(state) {
    return state.registries
  },
  defaultRegistryId(state) {
    return state.defaultRegistryId
  },
  images(state) {
    return state.images
  },
  tags(state) {
    return state.tags
  },
}

// actions
const actions = {
  async fetchRegistries({ commit }) {
    let response = (await api.account.getRegistries()).data
    let registries = response.registries
    let defaultRegistryId = response.defaultRegistryId
    commit('setRegistries', { registries })
    commit('setDefaultRegistryId', defaultRegistryId)
  },

  async fetchImages({ commit }, registryId) {
    try {
      let images = (await api.registry.getImages({ registryId: registryId }))
        .data
      commit('setImages', { images })
    } catch {
      // トークン未設定などで発生したエラーはaxios-ext側でハンドリングし、表示する
      commit('setImages', [])
    }
  },

  async fetchTags({ commit }, { registryId, image }) {
    try {
      let params = {
        registryId: registryId,
        image: image,
      }
      let tags = (await api.registry.getTags(params)).data
      commit('setTags', { tags })
    } catch {
      // 存在しないイメージ名をコピー実行した際のエラーはaxios-ext側でハンドリングし、表示する
      commit('setTags', [])
    }
  },
}

// mutations
const mutations = {
  setRegistries(state, { registries }) {
    state.registries = registries
  },
  setDefaultRegistryId(state, defaultRegistryId) {
    state.defaultRegistryId = defaultRegistryId
  },
  setImages(state, { images }) {
    state.images = images
  },
  setTags(state, { tags }) {
    state.tags = tags
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
