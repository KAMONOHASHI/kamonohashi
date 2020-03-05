import ManageTenant from '@/views/tenant-manage/tenant/Setting'
import ManageUserIndex from '@/views/tenant-manage/user/Index'
import ManageUserEdit from '@/views/tenant-manage/user/Edit'
import ManageResourceIndex from '@/views/tenant-manage/resource/Index'
import ManageResourceEdit from '@/views/tenant-manage/resource/Edit'

export default [
  {
    path: '/manage/tenant',
    name: 'ManageTenant',
    component: ManageTenant,
  },
  {
    path: '/manage/user',
    name: 'ManageUser',
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
    name: 'ManageResource',
    component: ManageResourceIndex,
    children: [
      {
        path: ':name',
        component: ManageResourceEdit,
        props: true,
      },
    ],
  },
]
