import Vue from 'vue'
import Router from 'vue-router'

import login from '@/router/login'
import account from '@/router/account'
import error from '@/router/error'
import dashboard from '@/router/dashboard'
import dataSet from '@/router/dataSet'
import training from '@/router/training'
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
    ...dataSet,
    ...training,
    ...version,
    ...modelTemplate,
    ...experiment,
  ],
})
router.beforeEach((to, from, next) => {
  if (!to.matched.length) {
    next('/error?url=' + to.path)
  } else {
    next()
  }
})

// clear notification
router.afterEach(() => {
  let vue = new Vue()
  vue.$notify.closeAll()
})

export { router as default }
