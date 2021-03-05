import Index from '@/views/system-setting/cluster-resource/Index'
import Edit from '@/views/system-setting/cluster-resource/Edit'
import Node from '@/views/system-setting/cluster-resource/Node'
import Tenant from '@/views/system-setting/cluster-resource/Tenant'
import ContainerList from '@/views/system-setting/cluster-resource/ContainerList'

export default [
  {
    path: '/cluster-resource',
    component: Index,
    children: [
      {
        path: '',
        component: Node,
        children: [
          {
            path: ':id/:name',
            component: Edit,
            props: true,
          },
        ],
      },
      {
        path: 'tenant',
        component: Tenant,
        children: [
          {
            path: ':id/:name',
            component: Edit,
            props: true,
          },
        ],
      },
      {
        path: 'container-list',
        component: ContainerList,
        children: [
          {
            path: ':id/:name',
            component: Edit,
            props: true,
          },
        ],
      },
    ],
  },
]
