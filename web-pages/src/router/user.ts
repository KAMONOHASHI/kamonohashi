import Index from '@/views/system-setting/user/Index.vue'
import Edit from '@/views/system-setting/user/Edit.vue'
import SyncLdap from '@/views/system-setting/user/SyncLdap.vue'

export default [
  {
    path: '/user',
    component: Index,
    children: [
      {
        path: 'edit/:id?',
        component: Edit,
        props: true,
      },
      {
        path: 'sync-ldap',
        component: SyncLdap,
        props: true,
      },
    ],
  },
]
