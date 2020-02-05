import Index from '@/views/training/Index'
import Edit from '@/views/training/Edit'
import Create from '@/views/training/Create'
import FileIndex from '@/views/training/FileIndex'
import Shell from '@/components/common/Shell.vue'
import LogViewer from '@/components/common/LogViewer.vue'

export default [
  {
    path: '/training',
    name: 'training',
    component: Index,
    children: [
      {
        path: 'run/:originId?',
        component: Create,
        props: true,
      },
      {
        path: ':id',
        component: Edit,
        props: true,
      },
      {
        path: ':id/files',
        component: FileIndex,
        props: true,
      },
      {
        path: ':id/shell',
        component: Shell,
        props: true,
      },
      {
        path: ':id/log',
        component: LogViewer,
        props: true,
      },
    ],
  },
]
