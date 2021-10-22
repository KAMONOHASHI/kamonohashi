import Vue from 'vue'
import Router from 'vue-router'

import login from '@/router/login'
import account from '@/router/account'
import error from '@/router/error'
import dashboard from '@/router/dashboard'
import data from '@/router/data'
import dataSet from '@/router/dataSet'
import preprocessing from '@/router/preprocessing'
import notebook from '@/router/notebook'
import training from '@/router/training'
import inference from '@/router/inference'
import tenantsetting from '@/router/tenant-setting'
import tenant from '@/router/tenant'
import git from '@/router/git'
import registry from '@/router/registry'
import storage from '@/router/storage'
import role from '@/router/role'
import quota from '@/router/quota'
import node from '@/router/node'
import user from '@/router/user'
import menu from '@/router/menu'
import clusterResource from '@/router/cluster-resource'
import version from '@/router/version'
import modelTemplate from '@/router/model-template'
import experiment from '@/router/experiment'

Vue.use(Router)

let router = new Router({
  routes: [
    ...login,
    ...account,
    ...error,
    ...dashboard,
    ...data,
    ...dataSet,
    ...preprocessing,
    ...notebook,
    ...training,
    ...inference,
    ...tenantsetting,
    ...tenant,
    ...git,
    ...registry,
    ...storage,
    ...role,
    ...quota,
    ...node,
    ...user,
    ...menu,
    ...clusterResource,
    ...version,
    ...modelTemplate,
    ...experiment,
  ],
})
router.beforeEach(async (to, from, next) => {
  let storeTenantId = router.app.$store.getters['account/getTenantId']
  // storeにテナントIDがあれば文字列に変換する（後ほどURLのテナントIDと比較する際に文字列である必要があるため）
  storeTenantId = storeTenantId === undefined ? null : String(storeTenantId)
  let token = router.app.$store.getters['account/token']
  // note: nextを使うと無視して良い次のエラーが出る
  // Redirected when going from ... to ... via a navigation guard.
  // https://github.com/vuejs/vue-router/issues/2881#issuecomment-520554378
  if (!to.matched.length) {
    next('/error?url=' + to.path)
    return
  }
  // バージョン情報
  if (to.path === '/version') {
    next()
    return
  }
  // 未ログイン
  if (!token) {
    if (to.path !== '/login') {
      next('/login')
    } else {
      next()
    }
    return
  }
  // URLのテナントが未指定
  if (to.query.tenantId === undefined) {
    let tenantId = storeTenantId ?? from.query.tenantId
    if (tenantId !== undefined) {
      next({
        ...to,
        query: { ...to.query, tenantId },
      })
    } else {
      await router.app.$store.dispatch('account/fetchAccount')
      next({
        ...to,
        query: {
          ...to.query,
          tenantId:
            router.app.$store.getters['account/account'].selectedTenant.id,
        },
      })
    }
    return
  }
  // URLの指定とstoreのテナントが異なる => store側をURLに合わせる
  if (storeTenantId !== to.query.tenantId) {
    await router.app.$store.dispatch('account/switchTenant', {
      tenantId: to.query.tenantId,
    })
  }
  // ログイン済みのとき、URLのテナントIDを直接変更した場合に備え、リロード処理を行う
  if (to.path === from.path && to.query.tenantId !== from.query.tenantId) {
    // リロード処理
    location.reload()
  }
  next()
})

// clear notification
router.afterEach(() => {
  let vue = new Vue()
  vue.$notify.closeAll()
})

export { router as default }
