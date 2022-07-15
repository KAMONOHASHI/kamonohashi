import Index from '@/views/system-setting/git/Index'
import Edit from '@/views/system-setting/git/Edit'

export default [
  {
    path: '/git',
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
