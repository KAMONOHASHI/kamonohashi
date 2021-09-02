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
  ],
})
router.beforeEach((to, from, next) => {
  if (!to.matched.length) {
    next('/error?url=' + to.path)
  } else {
    if (to.query.tenantId === undefined && from.query.tenantId !== undefined) {
      next({
        ...to,
        query: { ...to.query, tenantId: from.query.tenantId },
      })
    } else {
      next()
    }
  }
})

// clear notification
router.afterEach(() => {
  let vue = new Vue()
  vue.$notify.closeAll()
})

export { router as default }
