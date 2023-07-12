import Index from '@/views/system-setting/git/Index.vue'
import Edit from '@/views/system-setting/git/Edit.vue'

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
