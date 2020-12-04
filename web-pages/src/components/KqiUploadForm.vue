<!--name: ファイルのアップロード用フォーム。-->
<!--description: refで uploadFile を呼び出すとマルチパートアップロード実施-->
<!--props: {    type: アップロード時に指定するファイルタイプ（Data等）。指定可能項目はマルチパートアップロードAPIを参照}-->
<template>
  <el-progress
    v-if="showProgress"
    type="circle"
    :percentage="progress"
    class="progress"
  />

  <div v-else>
    <input
      ref="uploadInputTag"
      type="file"
      name="files"
      multiple
      @change="selectFiles"
    />
  </div>
</template>

<script>
import MultiPartUpload from '@/util/multipart-upload'
import Util from '@/util/util'

export default {
  name: 'UploadForm',
  props: {
    type: {
      type: String,
      default: '',
    },
  },
  data: function() {
    return {
      progress: 0,
      selectedFiles: undefined,
      showProgress: false,
      filesArray: [],
      partCount: 0,
      totalPart: 0,
    }
  },
  methods: {
    async uploadFile() {
      try {
        // 後の処理で undefined と比較するためnullではなくundefinedを返却
        if (!this.selectedFiles) return undefined

        this.showProgress = true
        this.getTotalPart(this.selectedFiles)
        for (let i = 0; i < this.selectedFiles.length; i++) {
          let uploader = new MultiPartUpload(this.selectedFiles[i])
          let progress = uploader.uploadAsync(this.type, this)
          for (let p = await progress.next(); ; p = await progress.next()) {
            if (p.done) {
              // 配列に1ファイルを格納
              this.filesArray.push(p.value)
              break
            }
          }
        }
        await Util.wait(1000) // progressが100に行くのを見えるようにするため
        return this.filesArray // 全てのファイルを格納した配列を返す
      } catch (error) {
        // エラー時はprogress表示解除
        this.showProgress = false
        throw error
      }
    },

    async selectFiles(e) {
      e.preventDefault()
      let files = e.target.files
      // filesは{"0": fileObject1, "1": fileObject2,...}という構造のFileListオブジェクトになっている
      // 取り回しづらいため配列に変換する
      this.selectedFiles = Object.values(files)
    },

    selectedFilesLength() {
      return this.selectedFiles === undefined ? 0 : this.selectedFiles.length
    },

    // ファイルの分割数の合計を取得する
    getTotalPart(selectedFiles) {
      if (!selectedFiles) this.totalPart = 0

      for (let i = 0; i < selectedFiles.length; i++) {
        this.totalPart += Math.ceil(
          selectedFiles[i].size / MultiPartUpload.chunkSize,
        )
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.progress {
  z-index: 2047 !important; // 2046がloadingのz-indexなので、loading時も見えるようにそれより大きな値を設定
}
</style>
