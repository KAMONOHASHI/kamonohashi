import api from '@/api/api'
import Util from '@/util/util'

const cookieTokenKey = '.Platypus.Auth'

// initial state
const state = {
  loginData: {},
  token: Util.getCookie(cookieTokenKey),
  account: {},
  menuList: [],
  menuTree: [],
  logined: false,
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
  getTenantId(state) {
    return state.loginData.tenantId
  },
  getTenantName(state) {
    return state.loginData.tenantName
  },
  getUserName(state) {
    return state.loginData.userName
  },
  isLogined() {
    return state.logined
  },
  // データ管理のアクセス権があるかどうか
  isAvailableData(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/data'
    })
  },
  // データセット管理のアクセス権があるかどうか
  isAvailableDataSet(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/dataset'
    })
  },
  // 前処理管理のアクセス権があるかどうか
  isAvailablePreprocessing(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/preprocessing'
    })
  },
  // 学習管理のアクセス権があるかどうか
  isAvailableTraining(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/training'
    })
  },
  // 推論管理のアクセス権があるかどうか
  isAvailableInference(state) {
    return state.menuTree.some(menu => {
      return menu.url === '/inference'
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

  async fetchMenu({ dispatch }) {
    dispatch('fetchMenuList')
    dispatch('fetchMenuTree')
  },

  // eslint-disable-next-line no-unused-vars
  async put({ commit }, params) {
    return await api.account.put(params)
  },

  // eslint-disable-next-line no-unused-vars
  async putPassword({ commit }, params) {
    return await api.account.putPassword(params)
  },
  // eslint-disable-next-line no-unused-vars
  async login({ commit, dispatch }, { userName, password }) {
    let params = {
      $config: { apiDisabledError: true },
      model: {
        userName: userName,
        password: password,
      },
    }
    let response = await api.account.postLogin(params)
    let loginData = response.data
    let token = loginData.token
    commit('setToken', { token })
    commit('setLoginData', { loginData })
    Util.setCookie(cookieTokenKey, token)
    await dispatch('fetchAccount')
    await dispatch('fetchMenu')
    commit('setLogined')
  },

  logout({ commit }) {
    commit('setLogout')
    Util.deleteCookie(cookieTokenKey)
  },

  async switchTenant({ commit, dispatch }, { tenantId }) {
    let loginData = (await api.account.postTokenTenants({ tenantId })).data
    let token = loginData.token
    commit('setToken', { token })
    commit('setLoginData', { loginData })
    Util.setCookie(cookieTokenKey, token)
    await dispatch('fetchAccount')
    await dispatch('fetchMenu')
  },

  async postTokenTenants({ commit, dispatch }, params) {
    let loginData = (await api.account.postTokenTenants(params)).data
    let token = loginData.token
    commit('setToken', { token })
    commit('setLoginData', { loginData })
    Util.setCookie(cookieTokenKey, token)
    dispatch('fetchMenu')
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
    state.logined = true
    state.account = account
  },
  setMenuList(state, { menuList }) {
    state.menuList = menuList
  },
  setMenuTree(state, { menuTree }) {
    state.menuTree = menuTree
  },
  setLogined(state) {
    state.logined = true
  },
  setLogout(state) {
    state.loginData = {}
    state.menuList = []
    state.menuTree = []
    state.token = null

    state.logined = false
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
