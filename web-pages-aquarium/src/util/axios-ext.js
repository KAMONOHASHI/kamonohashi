import store from '@/store'
import { Loading } from 'element-ui'
import Vue from 'vue'
import Util from '@/util/util'
import router from '@/router'

// output logs
export function axiosLoggerInterceptors($axios) {
  $axios.interceptors.request.use(
    function(config) {
      return config
    },
    function(error) {
      return Promise.reject(error)
    },
  )
}

// append JWT auth
export function axiosAuthInterceptors($axios) {
  $axios.interceptors.request.use(
    function(config) {
      let tenant = Util.getCookie('.Platypus.Tenant')
      let token = Util.getCookie('.Platypus.Auth.' + tenant)
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
      return config
    },
    function(error) {
      return Promise.reject(error)
    },
  )
}

// append element ui loading
export function axiosLoadingInterceptors($axios) {
  let enableLoading = function(config) {
    return !config.apiDisabledLoading
  }

  $axios.interceptors.request.use(
    function(config) {
      if (enableLoading(config)) {
        store.dispatch('incrementLoading')
      }
      return config
    },
    error => {
      if (enableLoading(error.config)) {
        store.dispatch('incrementLoading')
      }
      return Promise.reject(error)
    },
  )
  $axios.interceptors.response.use(
    response => {
      if (enableLoading(response.config)) {
        store.dispatch('decrementLoading')
      }
      return response
    },
    error => {
      if (enableLoading(error.config)) {
        store.dispatch('decrementLoading')
      }
      return Promise.reject(error)
    },
  )

  let loadingEnabled = store.state.loading
  let loadingInstance = null
  store.watch(store.getters.getLoadingCnt, v => {
    if (loadingEnabled) {
      if (v && !loadingInstance) {
        loadingInstance = Loading.service({
          fullscreen: true,
          text: 'Loading',
          spinner: 'el-icon-loading',
          background: 'rgba(255, 255, 255, 0.6)',
        })
      }
      if (v <= 0 && loadingInstance) {
        loadingInstance.close()
        loadingInstance = null
      }
    }
  })
  store.watch(store.getters.getLoading, f => {
    if (!f && loadingInstance) {
      loadingInstance.close()
      loadingInstance = null
    }
    loadingEnabled = f
  })
}

// append error handling
export function axiosErrorHandlingInterceptors($axios, errorCallback) {
  let vue = new Vue()
  let moveErrorPage = function(status, message) {
    let url =
      '/error?status=' +
      encodeURIComponent(status) +
      '&message=' +
      encodeURIComponent(message)
    router.push(url)
  }
  let success = response => {
    return response
  }
  let failure = error => {
    let handring = true

    if (errorCallback) {
      handring = errorCallback(error)
    }

    if (typeof handring === 'boolean' && handring === false) {
      // callback handringed
    } else {
      if (error.response) {
        let status = error.response.status
        let returnUrl = window.location.hash.slice(1)
        let url =
          '/login?timeout=true&return_url=' + encodeURIComponent(returnUrl)

        // auth check
        if (status === 401) {
          Util.deleteCookie('.Platypus.Auth')
          router.push(url)
          vue.$notify.info({
            title: 'ログインしてください',
            message: '有効な認証情報がありません',
          })
        } else if (status >= 400 && status < 600) {
          // common error
          if (
            typeof error === 'object' &&
            'response' in error &&
            typeof error.response === 'object' &&
            'data' in error.response &&
            typeof error.response.data === 'object' &&
            'title' in error.response.data
          ) {
            if (!error.config.apiDisabledError) {
              let msg = error.response.data.title
              vue.$notify.error({
                title: 'エラーが発生しました',
                dangerouslyUseHTMLString: true,
                message: msg,
              })
            } else {
              if (status === 500) {
                // internal server error
                let msg = '再度操作をお願い致します'
                vue.$notify.error({
                  title: status + 'エラーが発生しました',
                  dangerouslyUseHTMLString: true,
                  message: msg,
                })
              } else if (status === 503) {
                // service temporarily unavailable
                let msg = '再度操作をお願い致します'
                vue.$notify.error({
                  title: status + 'エラーが発生しました',
                  dangerouslyUseHTMLString: true,
                  message: msg,
                })
              }
            }
          } else {
            moveErrorPage(status, error.message)
          }
        } else {
          moveErrorPage(status, error.message)
        }
      } else {
        // can't handring error
        moveErrorPage('None', error.message)
      }

      return Promise.reject(error)
    }
  }
  $axios.interceptors.response.use(success, failure)
}
