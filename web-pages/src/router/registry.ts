import Index from '@/views/system-setting/registry/Index.vue'
import Edit from '@/views/system-setting/registry/Edit.vue'

export default [
  {
    path: '/registry',
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
