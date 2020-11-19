import Index from '@/views/modelTemplate/Index'
import Edit from '@/views/modelTemplate/Edit'

export default [
  {
    path: '/aquarium/model-template',
    component: Index,
    children: [
      {
        path: 'create',
        component: Edit,
      },
      {
        path: 'edit/:id/',
        component: Edit,
        props: true,
      },
    ],
  },
]
