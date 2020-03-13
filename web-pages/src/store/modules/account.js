import api from '@/api/v1/api'

// initial state
const state = {
  loginData: {},
  token: {},
  account: {},
  menuList: [],
  menuTree: [],
}

// getters
const getters = {
  loginData(state) {
    return state.loginData
  },
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

  // 前処理管理のアクセス権があるかどうか
  isPreprocessingAvailable(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/preprocessing'
    })
  },
  // データ管理のアクセス権があるかどうか
  isDataAvailable(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/data'
    })
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
  async put({ commit }, params) {
    return await api.account.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putPassword({ commit }, params) {
    return await api.account.putPassword(params)
  },

  async postLogin({ commit }, params) {
    let response = await api.account.postLogin(params)
    let loginData = response.data
    commit('setLoginData', { loginData })
  },

  async postTokenTenants({ commit }, params) {
    let loginData = (await api.account.postTokenTenants(params)).data
    let token = loginData.token
    commit('setToken', { token })
    commit('setLoginData', { loginData })
  },

  // eslint-disable-next-line no-unused-vars
  async putGitToken({ commit }, params) {
    return await api.account.putGits(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putRegistryToken({ commit }, params) {
    return await api.account.putRegistries(params)
  },
}

// mutations
const mutations = {
  setLoginData(state, { loginData }) {
    state.loginData = loginData
  },
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
