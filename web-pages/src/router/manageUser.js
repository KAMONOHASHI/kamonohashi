import ManageUserIndex from '@/views/tenant-manage/user/Index'
import ManageUserEdit from '@/views/tenant-manage/user/Edit'

export default [
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
]
