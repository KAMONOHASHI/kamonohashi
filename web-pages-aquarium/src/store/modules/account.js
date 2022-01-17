import api from '@/api/api'

// initial state
const state = {
  loginData: {},
  token: {},
  account: {},
  menuList: [],
  menuTree: [],
  webhook: {},
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
  webhook(state) {
    return state.webhook
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

  async fetchWebhook({ commit }) {
    let response = await api.account.getWebhook()
    let webhook = response.data
    commit('setWebhook', { webhook })
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

  // eslint-disable-next-line no-unused-vars
  async putWebhook({ commit }, params) {
    return await api.account.putWebhook(params)
  },

  // eslint-disable-next-line no-unused-vars
  async sendNotification({ commit }, params) {
    return await api.account.postWebhookTest(params)
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
  setWebhook(state, { webhook }) {
    state.webhook = webhook
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}
