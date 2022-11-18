import Index from '@/views/system-setting/cluster-resource/Index.vue'
import Edit from '@/views/system-setting/cluster-resource/Edit.vue'
import Node from '@/views/system-setting/cluster-resource/Node.vue'
import Tenant from '@/views/system-setting/cluster-resource/Tenant.vue'
import ContainerList from '@/views/system-setting/cluster-resource/ContainerList.vue'
import DataDL from '@/views/system-setting/cluster-resource/DataDL.vue'

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
      {
        path: 'data-download',
        component: DataDL,
      },
    ],
  },
]
