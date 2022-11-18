import Index from '@/views/system-setting/node/Index.vue'
import Edit from '@/views/system-setting/node/Edit.vue'

export default [
  {
    path: '/node',
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
