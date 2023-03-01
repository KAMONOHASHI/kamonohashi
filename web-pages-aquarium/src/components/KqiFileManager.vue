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
        <kqi-download-button
          class="uploadedFile"
          :download-url="uploadedFile.url"
          :file-name="uploadedFile.fileName"
        />
        <!-- 将来消せないものにはその情報がAPIから渡ってくるので、v-ifはそれに合わせて変更する -->
        <kqi-delete-button
          v-if="uploadedFile.fileId > 0 && deletable"
          size="mini"
          @delete="$emit('delete', uploadedFile.fileId)"
        />
      </div>
      <!-- アップロード済みファイルがあるので、それを表示 -->
    </div>
    <div v-else>
      <kqi-upload-form ref="uploadForm" title="File" :type="type" />
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'

import KqiDownloadButton from '@/components/KqiDownloadButton.vue'
import KqiDeleteButton from '@/components/KqiDeleteButton.vue'
import KqiUploadForm from '@/components/KqiUploadForm.vue'
import * as gen from '@/api/api.generate'

export default Vue.extend({
  name: 'FileManager',
  components: {
    KqiDownloadButton,
    KqiDeleteButton,
    KqiUploadForm,
  },
  props: {
    uploadedFiles: {
      type: Array as PropType<
        Array<gen.NssolPlatypusApiModelsDataApiModelsDataFileOutputModel>
      >,
      default: () => {
        return []
      },
    },
    type: {
      type: String,
      default: '',
    },
    deletable: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    async uploadFile() {
      if (this.$refs.uploadForm) {
        //@ts-ignore
        let fileInfo = await this.$refs.uploadForm.uploadFile()
        return fileInfo
      }
    },
    isFileSelected() {
      //@ts-ignore
      return this.$refs.uploadForm.isFileSelected()
    },
  },
})
</script>

<style lang="scss" scoped></style>
