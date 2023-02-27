import Index from '@/views/dataSet/Index.vue'
import Edit from '@/views/dataSet/Edit.vue'
import Detail from '@/views/dataSet/Detail.vue'
export default [
  {
    path: '/aquarium/dataset',
    component: Index,
    children: [
      {
        path: 'create',
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
  {
    path: '/aquarium/dataset/detail/:id',
    component: Detail,
    props: true,
    children: [],
  },
]
