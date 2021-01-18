import Vue from 'vue'
import Vuex from 'vuex'
import account from './modules/account'
import cluster from './modules/cluster'
import data from './modules/data'
import dataSet from './modules/dataSet'
import aquariumDataSet from './modules/aquariumDataSet'
import git from './modules/git'
import gitSelector from './modules/gitSelector'
import inference from './modules/inference'
import menu from './modules/menu'
import node from './modules/node'
import notebook from './modules/notebook'
import preprocessing from './modules/preprocessing'
import quota from './modules/quota'
import registry from './modules/registry'
import registrySelector from './modules/registrySelector'
import resource from './modules/resource'
import role from './modules/role'
import storage from './modules/storage'
import tenant from './modules/tenant'
import training from './modules/training'
import user from './modules/user'
import version from './modules/version'
import template from './modules/template'

Vue.use(Vuex)
export default new Vuex.Store({
  modules: {
    account,
    cluster,
    data,
    dataSet,
    aquariumDataSet,
    git,
    gitSelector,
    inference,
    menu,
    node,
    notebook,
    preprocessing,
    quota,
    registry,
    registrySelector,
    resource,
    role,
    storage,
    tenant,
    training,
    user,
    version,
    template,
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
