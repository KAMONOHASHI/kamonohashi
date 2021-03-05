import Index from '@/views/system-setting/role/Index'
import Edit from '@/views/system-setting/role/Edit'

export default [
  {
    path: '/role',
    component: Index,
    children: [
      {
        path: 'edit/:id?',
        component: Edit,
        props: true,
      },
    ],
  },
]
