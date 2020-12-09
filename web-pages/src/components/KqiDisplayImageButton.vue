<template>
  <span>
    <el-popover placement="right" trigger="click" :title="fileName">
      <div v-if="show">
        <el-image class="image-size" :src="downloadUrl" />
      </div>
      <div v-else-if="size === null">
        <i class="icon-size el-icon-loading" />
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
  data() {
    return {
      show: this.showImage,
      size: this.fileSize,
    }
  },
  methods: {
    ...mapActions(['fetchFileSize']),
    async openImage() {
      // filesizeがnullの場合にだけshowの切り替えを行う
      // (同じファイルのサイズを複数回取得しようとしない)
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
.icon-size {
  width: 100pt;
  height: 100pt;
}
</style>
