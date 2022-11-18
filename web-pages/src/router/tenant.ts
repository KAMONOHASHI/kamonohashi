import Index from '@/views/system-setting/tenant/Index.vue'
import Edit from '@/views/system-setting/tenant/Edit.vue'

export default [
  {
    path: '/tenant',
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
