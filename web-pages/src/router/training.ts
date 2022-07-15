import Index from '@/views/training/Index'
import Edit from '@/views/training/Edit'
import Create from '@/views/training/Create'
import FileList from '@/views/training/FileList'
import Shell from '@/views/common/Shell'
import LogViewer from '@/views/common/LogViewer'

export default [
  {
    path: '/training',
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
        component: FileList,
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
