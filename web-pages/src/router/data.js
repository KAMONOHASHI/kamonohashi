import Index from '@/views/data/Index'
import Edit from '@/views/data/Edit'
import Preprocessing from '@/views/common/RunPreprocessing'

export default [
  {
    path: '/data',
    name: 'Data',
    component: Index,
    children: [
      {
        path: 'edit/:id?',
        component: Edit,
        props: true,
      },
      {
        path: ':idArray/preprocessing',
        component: Preprocessing,
        props: true,
      },
    ],
  },
]
