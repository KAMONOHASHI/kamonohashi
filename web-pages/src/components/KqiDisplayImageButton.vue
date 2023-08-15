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

<script lang="ts">
import Vue from 'vue'

import { mapActions } from 'vuex'
interface DataType {
  show: boolean
  size: number
}

export default Vue.extend({
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
    type: {
      type: String,
      default: '',
    },
  },
  data(): DataType {
    return {
      show: this.showImage,
      size: this.fileSize,
    }
  },

  methods: {
    ...mapActions([
      'data/fetchFileSize',
      'training/fetchFileSize',
      'inference/fetchFileSize',
    ]),
    async openImage() {
      // filesizeがnullの場合にだけshowの切り替えを行う
      // (同じファイルのサイズを複数回取得しようとしない)
      if (!this.show && this.size === null) {
        let params = { id: this.id, name: this.fileName }

        // ファイルタイプによってAPIの呼び出し先を変える
        switch (this.type) {
          case 'Data':
            this.size = await this['data/fetchFileSize'](params)
            break
          case 'TrainingHistoryAttachedFiles':
            this.size = await this['training/fetchFileSize'](params)
            break
          case 'InferenceHistoryAttachedFiles':
            this.size = await this['inference/fetchFileSize'](params)
            break
        }

        // 10485760 Byte = 10 MB
        if (this.size < 10485760 && this.size !== null) this.show = true
      }
    },
  },
})
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
