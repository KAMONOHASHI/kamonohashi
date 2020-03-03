import Vue from 'vue'
import Vuex from 'vuex'
import data from './modules/data'
import git from './modules/git'
import notebook from './modules/notebook'
import preprocessing from './modules/preprocessing'
import training from './modules/training'
import inference from './modules/inference'
import menu from './modules/menu'
import node from './modules/node'
import quota from './modules/quota'
import registry from './modules/registry'
import dataSet from './modules/dataSet'
import registrySelector from './modules/registrySelector'
import gitSelector from './modules/gitSelector'
import cluster from './modules/cluster'
import role from './modules/role'
import storage from './modules/storage'
import tenant from './modules/tenant'
import user from './modules/user'

Vue.use(Vuex)
export default new Vuex.Store({
  modules: {
    data,
    dataSet,
    registrySelector,
    gitSelector,
    notebook,
    preprocessing,
    training,
    inference,
    git,
    menu,
    node,
    quota,
    registry,
    cluster,
    role,
    storage,
    tenant,
    user,
  },
  state: {
    // ログイン情報
    loginName: '',
    loginTenant: '',
    // 画面ブロック（ローディング）情報
    loading: true,
    loadingCnt: 0,
  },
  getters: {
    getLoginTenant: state => () => state.loginTenant,
    getLoadingCnt: state => () => state.loadingCnt,
    getLoading: state => () => state.loading,
  },
  mutations: {
    setLoading(state, loading) {
      state.loading = loading
    },
    incrementLoading(state) {
      state.loadingCnt++
    },
    decrementLoading(state) {
      state.loadingCnt--
    },
    setLogin(state, { name, tenant }) {
      state.loginName = name
      state.loginTenant = tenant
    },
  },
  actions: {
    incrementLoading(context) {
      context.commit('incrementLoading')
    },
    decrementLoading(context) {
      setTimeout(() => {
        context.commit('decrementLoading')
      }, 300) // delay 300ms
    },
  },
})
