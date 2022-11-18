import ManageTenant from '@/views/tenant-setting/tenant/Setting.vue'
import ManageUserIndex from '@/views/tenant-setting/user/Index.vue'
import ManageUserEdit from '@/views/tenant-setting/user/Edit.vue'
import ManageResourceIndex from '@/views/tenant-setting/resource/Index.vue'
import ManageResourceNode from '@/views/tenant-setting/resource/Node.vue'
import ManageResourceContainerList from '@/views/tenant-setting/resource/ContainerList.vue'
import ManageResourceEdit from '@/views/tenant-setting/resource/Edit.vue'

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
