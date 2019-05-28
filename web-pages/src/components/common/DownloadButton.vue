<template>
  <span>
    <span class="fileName">  {{fileName}} </span>
    <el-button @click="downloadFile" class="el-icon-download" size="mini"/>
  </span>
</template>

<script>

  export default {
    name: 'DownloadButton',
    props: {
      downloadUrl: String,
      fileName: String
    },
    methods: {
      async downloadFile () {
        const link = document.createElement('a')
        link.href = this.downloadUrl
        document.body.appendChild(link)
        link.click()
      }
    }
  }
</script>

<style lang="scss" scoped>
  .fileName {
    word-wrap: break-word;
  }
</style>
