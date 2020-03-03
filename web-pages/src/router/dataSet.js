import Index from '@/views/dataSet/Index'
import Edit from '@/views/dataSet/Edit'

export default [
  {
    path: '/dataset',
    name: 'DataSet',
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
