import Vue from 'vue'
import Vuex from 'vuex'
import account from './modules/account'
import cluster from './modules/cluster'
import data from './modules/data'
import dataSet from './modules/dataSet'
import aquariumDataSet from './modules/aquariumDataSet'
import gitSelector from './modules/gitSelector'
import registrySelector from './modules/registrySelector'
import training from './modules/training'
import version from './modules/version'
import template from './modules/template'
import experiment from './modules/experiment'

export interface RootState {
  version?: string
  loading?: boolean
  loadingCnt?: number
}
Vue.use(Vuex)
export default new Vuex.Store({
  modules: {
    account,
    cluster,
    data,
    dataSet,
    aquariumDataSet,
    gitSelector,
    registrySelector,
    training,
    version,
    template,
    experiment,
  },
  state: {
    // 画面ブロック（ローディング）情報
    loading: true,
    loadingCnt: 0,
  },
  getters: {
    getLoadingCnt: state => () => state.loadingCnt,
    getLoading: state => () => state.loading,
  },
  mutations: {
    setLoading(state, loading) {
      state.loading = loading
    },
    incrementLoading(state) {
      state.loadingCnt!++
    },
    decrementLoading(state) {
      state.loadingCnt!--
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
