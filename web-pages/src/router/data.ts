import Index from '@/views/data/Index.vue'
import Edit from '@/views/data/Edit.vue'
import Preprocessing from '@/views/common/RunPreprocessing.vue'

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
    ],
  },
]
