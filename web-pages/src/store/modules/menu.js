import api from '@/api/v1/api'

// initial state
const state = {
  menus: [],
  types: [],
}

// getters
const getters = {
  menus(state) {
    return state.menus
  },
  types(state) {
    return state.types
  },
}

// action
const actions = {
  async fetchMenus({ commit }) {
    let response = await api.menu.admin.get()
    let menus = response.data

    // roleオブジェクトの配列であるrolesから、idを抜き出して設定
    menus.forEach(menu => {
      let roles = []
      menu.roles.forEach(role => {
        roles.push(role.id)
      })
      menu.roles = roles
    })

    commit('setMenus', { menus })
  },

  async fetchTypes({ commit }) {
    let response = await api.menu.admin.getTypes()
    let types = response.data
    commit('setTypes', { types })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.menu.admin.put(params)
  },
}

// mutations
const mutations = {
  setMenus(state, { menus }) {
    state.menus = menus
  },
  setTypes(state, { types }) {
    state.types = types
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
