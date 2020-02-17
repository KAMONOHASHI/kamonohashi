import api from '@/api/v1/api'

// initial state
const state = {
  registries: [],
  images: [],
  tags: [],
  registry: null,
  image: null,
  tag: null,
}

// getters
const getters = {
  registries(state) {
    return state.registries
  },
  images(state) {
    return state.images
  },
  tags(state) {
    return state.tags
  },

  registry(state) {
    return state.registry
  },
  image(state) {
    return state.image
  },
  tag(state) {
    return state.tag
  },
}

// actions
const actions = {
  async fetchRegistries({ commit }) {
    let response = (await api.account.getRegistries()).data
    let registries = response.registries
    let defaultRegistryId = response.defaultRegistryId
    commit('setRegistries', { registries })

    commit(
      'setRegistry',
      registries.find(registry => {
        return registry.id === defaultRegistryId
      }),
    )
  },

  async fetchImages(context) {
    let registryId = context.state.registry.id
    let images = (await api.registry.getImages({ registryId: registryId })).data
    context.commit('setImages', { images })
  },

  async fetchTags(context) {
    let params = {
      registryId: context.state.registry.id,
      image: context.state.image,
    }
    let tags = (await api.registry.getTags(params)).data
    context.commit('setTags', { tags })
  },
}

// mutations
const mutations = {
  setRegistries(state, { registries }) {
    state.registries = registries
  },
  setImages(state, { images }) {
    state.images = images
  },
  setTags(state, { tags }) {
    state.tags = tags
  },

  setRegistry(state, registry) {
    state.registry = registry
  },
  setImage(state, image) {
    state.image = image
  },
  setTag(state, tag) {
    state.tag = tag
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
