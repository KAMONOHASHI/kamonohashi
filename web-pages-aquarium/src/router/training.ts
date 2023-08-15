import Index from '@/views/training/Index.vue'
import Edit from '@/views/training/Edit.vue'
import Create from '@/views/training/Create.vue'
import FileList from '@/views/training/FileList.vue'
import Shell from '@/views/common/Shell.vue'
import LogViewer from '@/views/common/LogViewer.vue'

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
