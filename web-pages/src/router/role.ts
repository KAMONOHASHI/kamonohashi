import Index from '@/views/system-setting/role/Index.vue'
import Edit from '@/views/system-setting/role/Edit.vue'

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
