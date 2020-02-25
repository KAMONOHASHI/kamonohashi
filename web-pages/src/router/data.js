import Index from '@/views/data/Index'
import Edit from '@/views/data/Edit'
import Create from '@/views/data/Create'
import Preprocessing from '@/views/data/Preprocessing'

export default [
  {
    path: '/data',
    name: 'Data',
    component: Index,
    children: [
      {
        path: 'create',
        component: Create,
      },
      {
        path: ':id',
        component: Edit,
        props: true,
      },
      {
        path: ':id/preprocessing',
        component: Preprocessing,
        props: true,
      },
    ],
  },
]
