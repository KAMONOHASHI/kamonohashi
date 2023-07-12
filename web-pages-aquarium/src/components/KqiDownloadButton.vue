<template>
  <span>
    <span class="fileName"> {{ fileName }} </span>
    <el-button class="el-icon-download" size="mini" @click="downloadFile" />
  </span>
</template>

<script lang="ts">
import Vue from 'vue'

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
  },
  methods: {
    downloadFile() {
      const link = document.createElement('a')
      link.href = this.downloadUrl
      document.body.appendChild(link)
      link.click()
    },
  },
})
</script>

<style lang="scss" scoped>
.fileName {
  word-wrap: break-word;
}
</style>
