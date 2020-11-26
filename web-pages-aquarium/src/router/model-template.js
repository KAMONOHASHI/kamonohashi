import Index from '@/views/modelTemplate/Index'
import Edit from '@/views/modelTemplate/Edit'
import TemplateIndex from '@/views/modelTemplate/TemplateIndex'

export default [
  {
    path: '/aquarium/model-template',
    component: Index,
    children: [
      {
        path: 'create',
        component: Edit,
      },
      {
        path: 'edit/:id/',
        component: Edit,
        props: true,
      },
      {
        path: ':id',
        component: TemplateIndex,
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
    path: '/aquarium/model-template/1',
    component: TemplateIndex,
    props: true,
  },
]
