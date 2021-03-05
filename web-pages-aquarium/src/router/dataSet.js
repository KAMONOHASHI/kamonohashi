import Index from '@/views/dataSet/Index'
import Edit from '@/views/dataSet/Edit'
import Detail from '@/views/dataSet/Detail/Index'

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
