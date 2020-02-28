import Index from '@/views/notebook/Index'
import Edit from '@/views/notebook/Edit'
import Create from '@/views/notebook/Create'
import FileList from '@/views/notebook/FileList'
import Shell from '@/views/common/Shell'
import LogViewer from '@/views/common/LogViewer'

export default [
  {
    path: '/notebook',
    name: 'notebook',
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
