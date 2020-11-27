import Index from '@/views/dataSet/Index'
import Edit from '@/views/dataSet/Edit'
import Detail from '@/views/dataSet/Detail/Index'

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
  {
    path: '/dataset/detail/:id',
    component: Detail,
    children: [],
  },
]
