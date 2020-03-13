export default class RegistrySelectorUtil {
  // registryサーバを選択し、イメージを取得する
  // form: form object
  // fetchImages: 'registrySelector/fetchImages'
  // registryId: 選択したサーバID
  static async selectRegistry(form, fetchImages, registryId) {
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
  static async selectImage(form, fetchTags, registryId, image) {
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
