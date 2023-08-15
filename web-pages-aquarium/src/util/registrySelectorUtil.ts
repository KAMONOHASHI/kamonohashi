import * as gen from '@/api/api.generate'

type Form = {
  name: string | null
  dataSetId: string | number | null
  entryPoint: string | null
  selectedParent: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  >
  containerImage: {
    registry: {
      id: number
      name: string
    } | null
    image: string | null
    tag: string | null
  }
  gitModel: {
    git: {
      id: number
      name: string
    } | null
    repository: { name: string; owner: string } | string | null
    branch: { branchName: string } | string | null
    commit: null | gen.NssolPlatypusServiceModelsGitCommitModel
  }
  resource: {
    cpu: number
    memory: number
    gpu: number
  }
  variables: Array<{ key: string; value: string }>
  ports: Array<number>
  partition: string | null
  zip: boolean
  localDataSet: boolean
  memo: string | null
  tags: Array<string>
}
export default class RegistrySelectorUtil {
  // registryサーバを選択し、イメージを取得する
  // form: form object
  // fetchImages: 'registrySelector/fetchImages'
  // registryId: 選択したサーバID
  static async selectRegistry(
    form: Form,
    getImages: Function,
    registryId: number | null,
  ) {
    // 過去の選択状態をリセット
    form.containerImage.image = null
    form.containerImage.tag = null
    // clearの場合リセット、レジストリが選択された場合はイメージ取得
    let images = []
    if (registryId !== null) {
      images = getImages(registryId)
    }
    return images
  }

  // イメージを選択し、タグを取得する
  // form: form object
  // fetchTags: 'registrySelector/fetchTags'
  // image: 選択したイメージ
  static async selectImage(
    form: Form,
    getTags: Function,
    registryId: number,
    image: string,
  ) {
    // 過去の選択状態をリセット
    form.containerImage.tag = null

    // clearの場合リセット、イメージが選択された場合はタグ取得
    let tags = []
    if (image !== null) {
      tags = await getTags({
        registryId: registryId,
        image: image,
      })
    }
    return tags
  }
}
