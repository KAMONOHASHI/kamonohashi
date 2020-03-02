import Index from '@/views/dataSet/Index'
import Edit from '@/views/dataSet/Edit'
import Create from '@/views/dataSet/Create'

export default [
  {
    path: '/dataset',
    name: 'DataSet',
    component: Index,
    children: [
      {
        path: 'create/:parentId?',
        component: Create,
        props: true,
      },
      {
        path: ':id',
        component: Edit,
        props: true,
      },
    ],
  },
]
