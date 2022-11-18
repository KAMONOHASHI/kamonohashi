import Index from '@/views/dataSet/Index.vue'
import Edit from '@/views/dataSet/Edit.vue'

export default [
  {
    path: '/dataset',
    component: Index,
    children: [
      {
        path: 'create/:id?',
        component: Edit,
        props: true,
      },
      {
        path: 'edit/:id',
        component: Edit,
        props: true,
      },
    ],
  },
]
