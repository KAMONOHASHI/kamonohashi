import Index from '@/views/system-setting/storage/Index.vue'
import Edit from '@/views/system-setting/storage/Edit.vue'

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
