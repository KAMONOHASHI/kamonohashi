import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)
export default new Vuex.Store({
  state: {
    // ログイン情報
    loginName: '',
    loginTenant: '',
    // 画面ブロック（ローディング）情報
    loading: true,
    loadingCnt: 0
  },
  getters: {
    getLoginTenant: state => () => state.loginTenant,
    getLoadingCnt: state => () => state.loadingCnt,
    getLoading: state => () => state.loading
  },
  mutations: {
    setLoading (state, loading) {
      state.loading = loading
    },
    incrementLoading (state) {
      state.loadingCnt++
    },
    decrementLoading (state) {
      state.loadingCnt--
    },
    setLogin (state, {name, tenant}) {
      state.loginName = name
      state.loginTenant = tenant
    }
  },
  actions: {
    incrementLoading (context) {
      context.commit('incrementLoading')
    },
    decrementLoading (context) {
      setTimeout(() => {
        context.commit('decrementLoading')
      }, 300) // delay 300ms
    }
  }
})
