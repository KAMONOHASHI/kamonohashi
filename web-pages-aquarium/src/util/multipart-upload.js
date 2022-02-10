import api from '@/api/api'
import Axios from 'axios'

const AxiosInst = Axios.create({
  headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
})

export default class MultiPartUpload {
  static chunkSize = 50 * 1024 ** 2 // MB
  start = 0
  end = 0
  total = 0

  file

  constructor(file) {
    this.file = file
    this.total = Math.ceil(file.size / this.constructor.chunkSize)
  }

  chunkedFile() {
    this.start = this.end
    this.end += this.constructor.chunkSize
    if (this.end > this.file.size) {
      this.end = this.file.size
    }
    return this.file.slice(this.start, this.end)
  }

  async getSignedUrlAsync(fileInfo) {
    let result = await api.storage.getUploadParameter(fileInfo)
    return result.data
  }

  async completeUploadAsync(completeInfo) {
    let result = await api.storage.postUploadComplete(completeInfo)
    return result.statusText
  }

  // 呼び出されるごとに1partずつアップロードする
  // アップロード途中はアップロードしたpart数、完了時は保存先情報を返す
  async *uploadAsync(type, caller) {
    let fileInfo = {
      fileName: this.file.name,
      partSum: Math.ceil(this.file.size / this.constructor.chunkSize),
      type: type,
    }

    let uploadInfo = await this.getSignedUrlAsync(fileInfo)
    let etags = []

    for (let i = 0; i < uploadInfo.partsSum; i++) {
      let f = this.chunkedFile()
      let result

      try {
        // `multipart upload request: ${new Date()} ${uploadInfo.uris[i]}`,
        result = await AxiosInst.put(uploadInfo.uris[i], f)
      } catch (error) {
        // `multipart upload failure: ${new Date()} ${uploadInfo.uris[i]}`,
        throw error
      }
      let etag = result.headers.etag
      etags.push(`${i + 1}+${etag}`)
      yield Math.round(((i + 1) / this.total) * 100)
      // プログレスバーを更新する
      caller.partCount += 1
      caller.progress = Math.ceil((caller.partCount / caller.totalPart) * 100)
    }
    let completeInfo = {
      body: {
        key: uploadInfo.key,
        uploadId: uploadInfo.uploadId,
        partETags: etags,
      },
    }
    await this.completeUploadAsync(completeInfo)
    return {
      name: this.file.name,
      storedPath: uploadInfo.storedPath,
    }
  }
}
