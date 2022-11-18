import Index from '@/views/system-setting/userGroup/Index.vue'
import Edit from '@/views/system-setting/userGroup/Edit.vue'

export default [
  {
    path: '/usergroup',
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
