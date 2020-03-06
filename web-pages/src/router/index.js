import Vue from 'vue'
import Router from 'vue-router'

import Error from '@/components/error/Error'

import login from '@/router/login'
import account from '@/router/account'
import dashboard from '@/router/dashboard'
import data from '@/router/data'
import dataSet from '@/router/dataSet'
import preprocessing from '@/router/preprocessing'
import notebook from '@/router/notebook'
import training from '@/router/training'
import inference from '@/router/inference'
import tenantManage from '@/router/tenant-manage'
import git from '@/router/git'
import menu from '@/router/menu'
import node from '@/router/node'
import quota from '@/router/quota'
import registry from '@/router/registry'
import role from '@/router/role'
import storage from '@/router/storage'
import tenant from '@/router/tenant'
import user from '@/router/user'
import clusterResource from '@/router/cluster-resource'
import version from '@/router/version'

Vue.use(Router)

let router = new Router({
  routes: [
    ...login,
    ...account,
    ...dashboard,
    ...data,
    ...dataSet,
    ...preprocessing,
    ...notebook,
    ...training,
    ...inference,
    ...tenantManage,
    ...git,
    ...menu,
    ...node,
    ...quota,
    ...registry,
    ...role,
    ...storage,
    ...tenant,
    ...user,
    ...clusterResource,
    ...version,
    {
      path: '/error',
      component: Error,
    },
  ],
})
/* eslint-disable */
router.beforeEach((to, from, next) => {
  if (!to.matched.length) {
    next('/error?url=' + to.path)
  } else {
    next()
  }
})

// clear notification
router.afterEach((to, from) => {
  let vue = new Vue()
  vue.$notify.closeAll()
})
/* eslint-enable */

export { router as default }
