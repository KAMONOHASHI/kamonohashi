import Index from '@/views/experiment/Index'
import Edit from '@/views/experiment/Edit'
import EditStep from '@/views/experiment/EditStep'
import Detail from '@/views/experiment/Detail/Index'
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
