import Vue from 'vue'
import Router from 'vue-router'

import Shell from '@/components/common/Shell.vue'
import LogViewer from '@/components/common/LogViewer.vue'

import Data from '@/components/data/Index'
import EditData from '@/components/data/Edit'
import CreateData from '@/components/data/Create'
import PreprocessData from '@/components/data/Preprocessing'

import DataSet from '@/components/dataset/Index'
import EditDataSet from '@/components/dataset/Edit'
import CreateDataSet from '@/components/dataset/Create'

import Preprocessing from '@/components/preprocessing/Index'
import CreatePreprocessing from '@/components/preprocessing/Create'
import EditPreprocessing from '@/components/preprocessing/Edit'
import RunPreprocessing from '@/components/preprocessing/Run'
import PreprocessingHistory from '@/components/preprocessing/PreprocessingHistory'
import PreprocessingHistoryEdit from '@/components/preprocessing/PreprocessingHistoryEdit'

import JupyterIndex from '@/components/jupyter/Index'
import CreateJupyter from '@/components/jupyter/Create.vue'
import EditJupyter from '@/components/jupyter/Edit.vue'

import TrainingIndex from '@/components/training/Index'
import CreateJob from '@/components/training/Create.vue'
import EditTrain from '@/components/training/Edit.vue'
import FileIndex from '@/components/training/FileIndex.vue'

import InferenceIndex from '@/components/inference/Index'
import CreateInference from '@/components/inference/Create'
import EditInference from '@/components/inference/Edit'
import FileIndexInference from '@/components/inference/FileIndex'

import ManageRoleIndex from '@/components/tenant-manage/role/Index'
import ManageRoleCreate from '@/components/tenant-manage/role/Create'
import ManageRoleEdit from '@/components/tenant-manage/role/Edit'

import ManageUserIndex from '@/components/tenant-manage/user/Index'
import ManageUserEdit from '@/components/tenant-manage/user/Edit'

import TenantIndex from '@/components/system-setting/tenant/Index'
import TenantCreate from '@/components/system-setting/tenant/Create'
import TenantEdit from '@/components/system-setting/tenant/Edit'

import GitIndex from '@/components/system-setting/git/Index'
import GitCreate from '@/components/system-setting/git/Create'
import GitEdit from '@/components/system-setting/git/Edit'

import RegistryIndex from '@/components/system-setting/registry/Index'
import RegistryCreate from '@/components/system-setting/registry/Create'
import RegistryEdit from '@/components/system-setting/registry/Edit'

import StorageIndex from '@/components/system-setting/storage/Index'
import StorageCreate from '@/components/system-setting/storage/Create'
import StorageEdit from '@/components/system-setting/storage/Edit'

import QuotaIndex from '@/components/system-setting/quota/Index'

import RoleIndex from '@/components/system-setting/role/Index'
import RoleCreate from '@/components/system-setting/role/Create'
import RoleEdit from '@/components/system-setting/role/Edit'

import NodeIndex from '@/components/system-setting/node/Index'
import NodeCreate from '@/components/system-setting/node/Create'
import NodeEdit from '@/components/system-setting/node/Edit'

import UserIndex from '@/components/system-setting/user/Index'
import UserCreate from '@/components/system-setting/user/Create'
import UserEdit from '@/components/system-setting/user/Edit'

import ClusterResource from '@/components/system-setting/cluster-resource/Index'
import ClusterResourceEdit from '@/components/system-setting/cluster-resource/Edit'
import ClusterResourceNode from '@/components/system-setting/cluster-resource/Node'
import ClusterResourceTenant from '@/components/system-setting/cluster-resource/Tenant'
import ClusterResourceContainerList from '@/components/system-setting/cluster-resource/ContainerList'

import AccountLogin from '@/components/account/Login'
import AccountSetting from '@/components/account/Setting'
import DashBoardIndex from '@/components/dashboard/Index'
import MenuIndex from '@/components/system-setting/menu/Index'
import ManageMenuIndex from '@/components/tenant-manage/menu/Index'
import Error from '@/components/error/Error'
import ManageResourceIndex from '@/components/tenant-manage/resource/Index'
import VersionIndex from '@/components/version/Index'

Vue.use(Router)

let router = new Router({
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: AccountLogin
    },
    {
      path: '/setting',
      name: 'Setting',
      component: AccountSetting
    },
    {
      path: '/',
      name: 'DashBoard',
      component: DashBoardIndex
    },
    {
      path: '/data',
      name: 'Data',
      component: Data,
      children: [
        {
          path: 'create',
          component: CreateData
        },
        {
          path: ':id',
          component: EditData,
          props: true
        },
        {
          path: ':id/preprocessing',
          component: PreprocessData,
          props: true
        }
      ]
    },
    {
      path: '/dataset',
      name: 'DataSet',
      component: DataSet,
      children: [
        {
          path: 'create/:parentId?',
          component: CreateDataSet,
          props: true
        },
        {
          path: ':id',
          component: EditDataSet,
          props: true
        }
      ]
    },
    {
      path: '/preprocessing',
      name: 'Preprocessing',
      component: Preprocessing,
      children: [
        {
          path: 'create',
          component: CreatePreprocessing
        },
        {
          path: 'create/:originId?',
          component: CreatePreprocessing,
          props: true
        },
        {
          path: ':id/edit',
          component: EditPreprocessing,
          props: true
        },
        {
          path: 'run',
          component: RunPreprocessing,
          props: true
        },
        {
          path: ':id/:dataId',
          component: PreprocessingHistoryEdit,
          props: true
        },
        {
          path: ':id/:dataId/shell',
          component: Shell,
          props: true
        }
      ]

    },
    {
      path: '/preprocessingHistory/:id',
      name: 'preprocessingHistory',
      component: PreprocessingHistory,
      props: true,
      children: [
        {
          path: ':dataId',
          component: PreprocessingHistoryEdit,
          props: true
        },
        {
          path: ':dataId/shell',
          component: Shell,
          props: true
        },
        {
          path: ':dataId/log',
          component: LogViewer,
          props: true
        }
      ]
    },
    {
      path: '/jupyter',
      name: 'jupyter',
      component: JupyterIndex,
      children: [
        {
          path: 'run/:originId?',
          component: CreateJupyter,
          props: true
        },
        {
          path: ':id',
          component: EditJupyter,
          props: true
        }
      ]
    },
    {
      path: '/training',
      name: 'training',
      component: TrainingIndex,
      children: [
        {
          path: 'run/:originId?',
          component: CreateJob,
          props: true
        },
        {
          path: ':id',
          component: EditTrain,
          props: true
        },
        {
          path: ':id/files',
          component: FileIndex,
          props: true
        },
        {
          path: ':id/shell',
          component: Shell,
          props: true
        },
        {
          path: ':id/log',
          component: LogViewer,
          props: true
        }
      ]
    },
    {
      path: '/inference',
      name: 'inference',
      component: InferenceIndex,
      children: [
        {
          path: 'create/:originId?',
          component: CreateInference,
          props: true
        },
        {
          path: ':id',
          component: EditInference,
          props: true
        },
        {
          path: ':id/files',
          component: FileIndexInference,
          props: true
        },
        {
          path: ':id/shell',
          component: Shell,
          props: true
        },
        {
          path: ':id/log',
          component: LogViewer,
          props: true
        }
      ]
    },
    {
      path: '/manage/role',
      name: 'ManageRole',
      component: ManageRoleIndex,
      children: [
        {
          path: 'create',
          component: ManageRoleCreate
        },
        {
          path: ':id',
          component: ManageRoleEdit,
          props: true
        }
      ]
    },
    {
      path: '/manage/user',
      name: 'ManageUser',
      component: ManageUserIndex,
      children: [
        {
          path: ':id',
          component: ManageUserEdit,
          props: true
        }
      ]
    },
    {
      path: '/manage/menu',
      name: 'ManageMenu',
      component: ManageMenuIndex
    },
    {
      path: '/manage/resource',
      name: 'ManageResource',
      component: ManageResourceIndex
    },
    {
      path: '/tenant',
      name: 'Tenant',
      component: TenantIndex,
      children: [
        {
          path: 'create',
          component: TenantCreate
        },
        {
          path: ':id',
          component: TenantEdit,
          props: true
        }
      ]
    },
    {
      path: '/git',
      name: 'Git',
      component: GitIndex,
      children: [
        {
          path: 'create',
          component: GitCreate
        },
        {
          path: ':id',
          component: GitEdit,
          props: true
        }
      ]
    },
    {
      path: '/registry',
      name: 'Registry',
      component: RegistryIndex,
      children: [
        {
          path: 'create',
          component: RegistryCreate
        },
        {
          path: ':id',
          component: RegistryEdit,
          props: true
        }
      ]
    },
    {
      path: '/storage',
      name: 'Storage',
      component: StorageIndex,
      children: [
        {
          path: 'create',
          component: StorageCreate
        },
        {
          path: ':id',
          component: StorageEdit,
          props: true
        }
      ]
    },
    {
      path: '/role',
      name: 'Role',
      component: RoleIndex,
      children: [
        {
          path: 'create',
          component: RoleCreate
        },
        {
          path: ':id',
          component: RoleEdit,
          props: true
        }
      ]
    },
    {
      path: '/quota',
      name: 'Quota',
      component: QuotaIndex
    },
    {
      path: '/node',
      name: 'Node,',
      component: NodeIndex,
      children: [
        {
          path: 'create',
          component: NodeCreate
        },
        {
          path: ':id',
          component: NodeEdit,
          props: true
        }
      ]
    },
    {
      path: '/user',
      name: 'User',
      component: UserIndex,
      children: [
        {
          path: 'create',
          component: UserCreate
        },
        {
          path: ':id',
          component: UserEdit,
          props: true
        }
      ]
    },
    {
      path: '/menu',
      name: 'Menu',
      component: MenuIndex
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
              props: true
            }
          ]
        },
        {
          path: 'tenant',
          component: ClusterResourceTenant,
          children: [
            {
              path: ':id/:name',
              component: ClusterResourceEdit,
              props: true
            }
          ]
        },
        {
          path: 'container-list',
          component: ClusterResourceContainerList,
          children: [
            {
              path: ':id/:name',
              component: ClusterResourceEdit,
              props: true
            }
          ]
        }
      ]
    },
    {
      path: '/error',
      name: 'error',
      component: Error
    },
    {
      path: '/version',
      name: 'Version',
      component: VersionIndex
    }
  ]
})

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

export {router as default}
