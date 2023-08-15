import Index from '@/views/inference/Index.vue'
import Edit from '@/views/inference/Edit.vue'
import Create from '@/views/inference/Create.vue'
import FileList from '@/views/inference/FileList.vue'
import Shell from '@/views/common/Shell.vue'
import LogViewer from '@/views/common/LogViewer.vue'

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
