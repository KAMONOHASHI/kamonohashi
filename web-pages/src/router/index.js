import Vue from 'vue'
import Router from 'vue-router'

import ManageRoleIndex from '@/components/tenant-manage/role/Index'
import ManageRoleCreate from '@/components/tenant-manage/role/Create'
import ManageRoleEdit from '@/components/tenant-manage/role/Edit'

import ClusterResource from '@/components/system-setting/cluster-resource/Index'
import ClusterResourceEdit from '@/components/system-setting/cluster-resource/Edit'
import ClusterResourceNode from '@/components/system-setting/cluster-resource/Node'
import ClusterResourceTenant from '@/components/system-setting/cluster-resource/Tenant'
import ClusterResourceContainerList from '@/components/system-setting/cluster-resource/ContainerList'

import AccountLogin from '@/components/account/Login'
import AccountSetting from '@/components/account/Setting'
import DashBoardIndex from '@/components/dashboard/Index'
import ManageMenuIndex from '@/components/tenant-manage/menu/Index'
import Error from '@/components/error/Error'
import ManageResourceIndex from '@/components/tenant-manage/resource/Index'
import VersionIndex from '@/components/version/Index'

import data from '@/router/data'
import dataSet from '@/router/dataSet'
import preprocessing from '@/router/preprocessing'
import notebook from '@/router/notebook'
import training from '@/router/training'
import inference from '@/router/inference'
import manageTenant from '@/router/manageTenant'
import manageUser from '@/router/manageUser'
import git from '@/router/git'
import menu from '@/router/menu'
import node from '@/router/node'
import quota from '@/router/quota'
import registry from '@/router/registry'
import role from '@/router/role'
import storage from '@/router/storage'
import tenant from '@/router/tenant'
import user from '@/router/user'

Vue.use(Router)

let router = new Router({
  routes: [
    ...data,
    ...dataSet,
    ...preprocessing,
    ...notebook,
    ...training,
    ...inference,
    ...manageTenant,
    ...manageUser,
    ...git,
    ...menu,
    ...node,
    ...quota,
    ...registry,
    ...role,
    ...storage,
    ...tenant,
    ...user,
    {
      path: '/login',
      name: 'Login',
      component: AccountLogin,
    },
    {
      path: '/setting',
      name: 'Setting',
      component: AccountSetting,
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
      path: '/cluster-resource',
      name: 'ClusterResource',
      component: ClusterResource,
      children: [
        {
          path: '',
          component: ClusterResourceNode,
          children: [
            {
              path: ':id/:name',
              component: ClusterResourceEdit,
              props: true,
            },
          ],
        },
        {
          path: 'tenant',
          component: ClusterResourceTenant,
          children: [
            {
              path: ':id/:name',
              component: ClusterResourceEdit,
              props: true,
            },
          ],
        },
        {
          path: 'container-list',
          component: ClusterResourceContainerList,
          children: [
            {
              path: ':id/:name',
              component: ClusterResourceEdit,
              props: true,
            },
          ],
        },
      ],
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
