<!--name: ファイルの管理,-->
<!--description: アップロード済ファイル一覧、削除ボタン、アップロードフォーム（アップロード済ファイル未指定時）を表示-->
<!--props: {-->
<!--uploadedFiles: Array,-->
<!--type: アップロード時に指定するファイルタイプ（Data等）。指定可能項目はマルチパートアップロードAPIを参照-->
<!--}-->
<!--events: {-->
<!--delete: 削除ボタンで削除確認に「はい」を選択時に発火-->
<!--}-->

<template>
  <div class="el-input">
    <div v-if="uploadedFiles && uploadedFiles.length > 0">
      <div v-for="(uploadedFile, index) in uploadedFiles" :key="index">
        <pl-download
          class="uploadedFile"
          :download-url="uploadedFile.url"
          :file-name="uploadedFile.fileName"
        />
        <!-- 将来消せないものにはその情報がAPIから渡ってくるので、v-ifはそれに合わせて変更する -->
        <pl-delete-button
          v-if="uploadedFile.fileId > 0 && deletable"
          size="mini"
          @delete="$emit('delete', uploadedFile.fileId)"
        />
      </div>
      <!-- アップロード済みファイルがあるので、それを表示 -->
    </div>
    <div v-else>
      <pl-upload-form ref="uploadForm" title="File" :type="type" />
    </div>
  </div>
</template>

<script>
import DownloadButton from '@/components/common/DownloadButton.vue'
import DeleteButton from '@/components/common/DeleteButton.vue'
import UploadForm from '@/components/common/UploadForm.vue'

export default {
  name: 'FileManager',
  components: {
    'pl-download': DownloadButton,
    'pl-upload-form': UploadForm,
    'pl-delete-button': DeleteButton,
  },
  props: {
    uploadedFiles: Array,
    type: String,
    deletable: Boolean,
  },
  methods: {
    async uploadFile() {
      if (this.$refs.uploadForm) {
        let fileInfo = await this.$refs.uploadForm.uploadFile()
        return fileInfo
      }
    },
    isFileSelected() {
      return this.$refs.uploadForm.isFileSelected()
    },
  },
}
</script>

<style lang="scss" scoped></style>
