import Index from '@/views/system-setting/user/Index'
import Edit from '@/views/system-setting/user/Edit'
import SyncLdap from '@/views/system-setting/user/SyncLdap'

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
