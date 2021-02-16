<!--name: アクアリウムログファイルの管理,-->
<!--description: 実験の前処理ログ、学習ログを表示-->

<template>
  <div class="el-input">
    <div v-if="logFiles && logFiles.length > 0">
      <div v-for="(logFile, index) in logFiles" :key="index">
        <kqi-download-button
          :download-url="logFile.url"
          :file-name="logFile.fileName"
        />
      </div>
    </div>
    <div v-else>
      参照できるログファイルはありません
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiDownloadButton from '../../../components/KqiDownloadButton.vue'
const { mapGetters, mapActions } = createNamespacedHelpers('experiment')

export default {
  components: { KqiDownloadButton },
  props: {
    id: {
      type: String,
      default: null,
    },
  },

  computed: {
    ...mapGetters(['logFiles']),
  },

  async created() {
    await this.retrieveData()
  },

  methods: {
    ...mapActions(['fetchLogFiles']),
    async retrieveData() {
      await this.fetchLogFiles(String(this.id))
    },
  },
}
</script>

<style lang="scss" scoped></style>
