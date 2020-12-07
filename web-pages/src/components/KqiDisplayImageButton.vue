<!-- ToDo : 画像のデータサイズが非常に大きい場合、画像を表示しない処理を追加 必要? -->
<template>
  <span>
    <el-popover placement="right" trigger="click" :title="fileName">
      <div v-if="show">
        <el-image class="image-size" :src="downloadUrl" />
      </div>
      <div v-else>
        データが大きすぎるため表示できません。<br />
        ダウンロードして確認してください。
      </div>
      <el-button slot="reference" size="mini" @click="openImage()">
        画像を表示
      </el-button>
    </el-popover>
  </span>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapActions } = createNamespacedHelpers('data')

export default {
  props: {
    downloadUrl: {
      type: String,
      default: '',
    },
    fileName: {
      type: String,
      default: '',
    },
    id: {
      type: Number,
      default: 0,
    },
    showImage: {
      type: Boolean,
      default: false,
    },
    fileSize: {
      type: Number,
      default: null,
    },
  },
  data: function() {
    return {
      show: this.showImage,
      size: this.fileSize,
    }
  },
  methods: {
    ...mapActions(['fetchFileSize']),
    async openImage() {
      if (!this.show && this.size === null) {
        let params = { id: this.id, name: this.fileName }
        this.size = await this.fetchFileSize(params)
        // 10485760 Byte = 10 MB
        if (this.size < 10485760) this.show = true
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.image-size {
  max-width: 300pt;
  height: auto;
}
</style>
