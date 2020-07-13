import Index from '@/views/data/Index'
import Edit from '@/views/data/Edit'
import Preprocessing from '@/views/common/RunPreprocessing'
import createTag from '@/components/KqiTagCreateDialog'

export default [
  {
    path: '/data',
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
      {
        path: 'tag',
        name: 'tag',
        component: createTag,
        props: true,
      },
    ],
  },
]
