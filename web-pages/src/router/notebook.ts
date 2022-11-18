import Index from '@/views/notebook/Index.vue'
import Edit from '@/views/notebook/Edit.vue'
import Create from '@/views/notebook/Create.vue'
import FileList from '@/views/notebook/FileList.vue'
import Shell from '@/views/common/Shell.vue'
import LogViewer from '@/views/common/LogViewer.vue'

export default [
  {
    path: '/notebook',
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
