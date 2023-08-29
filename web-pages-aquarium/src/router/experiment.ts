import Index from '@/views/experiment/Index.vue'
import Edit from '@/views/experiment/Edit.vue'
import EditStep from '@/views/experiment/EditStep.vue'
import Detail from '@/views/experiment/Detail/Index.vue'
export default [
  {
    path: '/aquarium/experiment',
    component: Index,
    children: [],
    props: true,
  },
  {
    path: '/aquarium/experiment/create',
    component: Edit,
    children: [],
    props: true,
  },
  {
    path: '/aquarium/experiment/createStep/:templateId',
    component: EditStep,
    children: [],
    props: true,
  },
  {
    path: '/aquarium/experiment/detail/:id',
    component: Detail,
    children: [],
    props: true,
  },
]
