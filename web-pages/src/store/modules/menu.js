import api from '@/api/v1/api'

// initial state
const state = {
  menus: [],
  types: [],
  tenantMenus: [],
  tenantTypes: [],
}

// getters
const getters = {
  menus(state) {
    return state.menus
  },
  types(state) {
    return state.types
  },
  tenantMenus(state) {
    return state.tenantMenus
  },
  tenantTypes(state) {
    return state.tenantTypes
  },
}

// action
const actions = {
  // admin
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

  // tenant
  async fetchTenantMenus({ commit }) {
    let response = await api.menu.tenant.get()
    let tenantMenus = response.data

    // roleオブジェクトの配列であるrolesから、idを抜き出して設定
    tenantMenus.forEach(menu => {
      let roles = []
      menu.roles.forEach(role => {
        roles.push(role.id)
      })
      menu.roles = roles
    })

    commit('setTenantMenus', { tenantMenus })
  },

  async fetchTenantTypes({ commit }) {
    let response = await api.menu.tenant.getTypes()
    let tenantTypes = response.data
    commit('setTenantTypes', { tenantTypes })
  },

  // eslint-disable-next-line no-unused-vars
  async putTenant({ commit }, params) {
    return await api.menu.tenant.put(params)
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
  setTenantMenus(state, { tenantMenus }) {
    state.tenantMenus = tenantMenus
  },
  setTenantTypes(state, { tenantTypes }) {
    state.tenantTypes = tenantTypes
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
