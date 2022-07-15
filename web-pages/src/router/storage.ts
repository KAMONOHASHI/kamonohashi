import Index from '@/views/system-setting/storage/Index'
import Edit from '@/views/system-setting/storage/Edit'

export default [
  {
    path: '/storage',
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
