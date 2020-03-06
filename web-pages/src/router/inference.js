import Index from '@/views/inference/Index'
import Edit from '@/views/inference/Edit'
import Create from '@/views/inference/Create'
import FileList from '@/views/inference/FileList'
import Shell from '@/views/common/Shell'
import LogViewer from '@/views/common/LogViewer'

export default [
  {
    path: '/inference',
    component: Index,
    children: [
      {
        path: 'create/:originId?',
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
