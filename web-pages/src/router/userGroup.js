import Index from '@/views/system-setting/userGroup/Index'
import Edit from '@/views/system-setting/userGroup/Edit'

export default [
  {
    path: '/userGroup',
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
