import Index from '@/views/system-setting/tenant/Index'
import Edit from '@/views/system-setting/tenant/Edit'

export default [
  {
    path: '/tenant',
    name: 'Tenant',
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
