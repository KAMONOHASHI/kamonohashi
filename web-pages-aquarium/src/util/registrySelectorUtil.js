export default class RegistrySelectorUtil {
  // registryサーバを選択し、イメージを取得する
  // form: form object
  // fetchImages: 'registrySelector/fetchImages'
  // registryId: 選択したサーバID
  static async selectRegistry(form, getImages, registryId) {
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
  static async selectImage(form, getTags, registryId, image) {
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
