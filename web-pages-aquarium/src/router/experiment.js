import Index from '@/views/experiment/Index'
import Edit from '@/views/experiment/Edit'
import Detail from '@/views/experiment/Detail/Index'
export default [
  {
    path: '/aquarium/experiment',
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
    path: '/aquarium/experiment/detail/:id',
    component: Detail,
    children: [],
  },
]
