import Vue from 'vue'
import Router from 'vue-router'

import ManageRoleIndex from '@/components/tenant-manage/role/Index'
import ManageRoleCreate from '@/components/tenant-manage/role/Create'
import ManageRoleEdit from '@/components/tenant-manage/role/Edit'

import AccountLogin from '@/components/account/Login'
import DashBoardIndex from '@/components/dashboard/Index'
import ManageMenuIndex from '@/components/tenant-manage/menu/Index'
import Error from '@/components/error/Error'
import ManageResourceIndex from '@/components/tenant-manage/resource/Index'
import VersionIndex from '@/components/version/Index'

import account from '@/router/account'
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

Vue.use(Router)

let router = new Router({
  routes: [
    ...account,
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
    {
      path: '/login',
      name: 'Login',
      component: AccountLogin,
    },
    {
      path: '/',
      name: 'DashBoard',
      component: DashBoardIndex,
    },
    {
      path: '/manage/role',
      name: 'ManageRole',
      component: ManageRoleIndex,
      children: [
        {
          path: 'create',
          component: ManageRoleCreate,
        },
        {
          path: ':id',
          component: ManageRoleEdit,
          props: true,
        },
      ],
    },
    {
      path: '/manage/menu',
      name: 'ManageMenu',
      component: ManageMenuIndex,
    },
    {
      path: '/manage/resource',
      name: 'ManageResource',
      component: ManageResourceIndex,
    },
    {
      path: '/error',
      name: 'error',
      component: Error,
    },
    {
      path: '/version',
      name: 'Version',
      component: VersionIndex,
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
