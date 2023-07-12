import * as gen from '@/api/api.generate'
type Form = {
  name?: string | null
  dataSetId?: string | number | null
  entryPoint?: string | null
  selectedParent?: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  >
  selectedParentInference?: Array<
    gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
  >
  containerImage: {
    registry: {
      id?: number | null
      name?: string
    } | null
    image: string | null
    tag: string | null
  }
  gitModel: {
    git: {
      id?: number | null
      name?: string
    } | null
    repository:
      | string
      | { name?: string; owner?: string; fullName?: string }
      | null
    branch?: string | { branchName: string } | null
    commit: gen.NssolPlatypusServiceModelsGitCommitModel | null
  }

  jupyterLabVersion?: string | null
  resource: {
    cpu?: number
    memory?: number
    gpu?: number
  }
  expiresIn?: number
  withExpiresInSetting?: boolean
  variables?: Array<{ key: string; value: string }>
  partition?: string | null
  zip?: boolean
  localDataSet?: boolean
  memo?: string | null
}

export default class RegistrySelectorUtil {
  // registryサーバを選択し、イメージを取得する
  // form: form object
  // fetchImages: 'registrySelector/fetchImages'
  // registryId: 選択したサーバID
  static async selectRegistry(
    form: Form,
    fetchImages: Function,
    registryId: number,
  ) {
    // 過去の選択状態をリセット
    form.containerImage.image = null
    form.containerImage.tag = null
    // clearの場合リセット、レジストリが選択された場合はイメージ取得
    if (registryId !== null) {
      await fetchImages(registryId)
    }
  }

  // イメージを選択し、タグを取得する
  // form: form object
  // fetchTags: 'registrySelector/fetchTags'
  // image: 選択したイメージ
  static async selectImage(
    form: Form,
    fetchTags: Function,
    registryId: number,
    image: string,
  ) {
    // 過去の選択状態をリセット
    form.containerImage.tag = null

    // clearの場合リセット、イメージが選択された場合はタグ取得
    if (image !== null) {
      await fetchTags({
        registryId: registryId,
        image: image,
      })
    }
  }
}
