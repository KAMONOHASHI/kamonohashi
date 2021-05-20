import ManageTenant from '@/views/tenant-setting/tenant/Setting'
import ManageUserIndex from '@/views/tenant-setting/user/Index'
import ManageUserEdit from '@/views/tenant-setting/user/Edit'
import ManageResourceIndex from '@/views/tenant-setting/resource/Index'
import ManageResourceNode from '@/views/tenant-setting/resource/Node'
import ManageResourceContainerList from '@/views/tenant-setting/resource/ContainerList'
import ManageResourceEdit from '@/views/tenant-setting/resource/Edit'

export default [
  {
    path: '/manage/tenant',
    component: ManageTenant,
  },
  {
    path: '/manage/user',
    component: ManageUserIndex,
    children: [
      {
        path: ':id',
        component: ManageUserEdit,
        props: true,
      },
    ],
  },
  {
    path: '/manage/resource',
    component: ManageResourceIndex,
    children: [
      {
        path: '',
        component: ManageResourceNode,
        children: [
          {
            path: ':nodeName/:name',
            component: ManageResourceEdit,
            props: true,
          },
        ],
      },
      {
        path: 'container-list',
        component: ManageResourceContainerList,
        children: [
          {
            path: ':nodeName/:name',
            component: ManageResourceEdit,
            props: true,
          },
        ],
      },
    ],
  },
]
