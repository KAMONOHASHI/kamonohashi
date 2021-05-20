import Index from '@/views/modelTemplate/Index'
import Edit from '@/views/modelTemplate/Edit'
//import TemplateIndex from '@/views/modelTemplate/TemplateIndex'
import Detail from '@/views/modelTemplate/Detail/Index'

export default [
  {
    path: '/aquarium/model-template',
    component: Index,
    children: [
      {
        path: 'create',
        component: Edit,
        props: true,
      },

      {
        path: 'edit/:id/',
        component: Edit,
        props: true,
      },
    ],
  },
  // モデルテンプレート詳細
  // {
  //   path: '/model-template/:id',
  //   component: TemplateIndex,
  //   props: true,
  // },
  // TODO:APIができたら↑に修正
  {
    path: '/aquarium/model-template/:id',
    component: Detail,
    props: true,
  },
  //{
  //  path: '/aquarium/model-template/1',
  //  component: TemplateIndex,
  //  props: true,
  //},
]
