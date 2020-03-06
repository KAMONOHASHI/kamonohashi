import api from '@/api/v1/api'

// initial state
const state = {
  token: {},
  account: {},
  menuList: {},
  menuTree: {},
}

// getters
const getters = {
  token(state) {
    return state.token
  },
  account(state) {
    return state.account
  },
  menuList(state) {
    return state.menuList
  },
  menuTree(state) {
    return state.menuTree
  },
}

// action
const actions = {
  async fetchAccount({ commit }) {
    let response = await api.account.get()
    let account = response.data
    commit('setAccount', { account })
  },

  async fetchMenuList({ commit }) {
    let response = await api.menuList.getMenuList()
    let menuList = response.data
    commit('setMenuList', { menuList })
  },

  async fetchMenuTree({ commit }) {
    let response = await api.account.getTreeMenus()
    let menuTree = response.data
    commit('setMenuTree', { menuTree })
  },

  // eslint-disable-next-line no-unused-vars
  async put({ rootState }, params) {
    return await api.account.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putPassword({ rootState }, params) {
    return await api.account.putPassword(params)
  },

  // eslint-disable-next-line no-unused-vars
  async postTokenTenants({ commit }, params) {
    let login = (await api.account.postTokenTenants(params)).data
    let token = login.token
    commit('setToken', { token })
  },

  // eslint-disable-next-line no-unused-vars
  async putGitToken({ rootState }, params) {
    return await api.account.putGits(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putRegistryToken({ rootState }, params) {
    return await api.account.putRegistries(params)
  },
}

// mutations
const mutations = {
  setToken(state, { token }) {
    state.token = token
  },
  setAccount(state, { account }) {
    state.account = account
  },
  setMenuList(state, { menuList }) {
    state.menuList = menuList
  },
  setMenuTree(state, { menuTree }) {
    state.menuTree = menuTree
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
