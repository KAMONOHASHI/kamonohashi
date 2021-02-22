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
    <div v-if="preprocessLogFiles && preprocessLogFiles.length > 0">
      <div v-for="(preprocessLog, index) in preprocessLogFiles" :key="index">
        <kqi-download-button
          :download-url="preprocessLog.url"
          :file-name="preprocessLog.fileName"
        />
      </div>
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
    experimentPreprocessHistoryId: {
      type: Number,
      default: null,
    },
    id: {
      type: String,
      default: null,
    },
  },

  computed: {
    ...mapGetters(['logFiles', 'preprocessLogFiles']),
  },

  async created() {
    // idが取れていたらログを取得する
    // このままだとidが取れていない場合に値が更新されず意図しないログが表示される
    if (this.id >= 0) {
      await this.retrieveData()
    }
  },

  methods: {
    ...mapActions(['fetchLogFiles', 'fetchPreprocessFiles']),
    async retrieveData() {
      await this.fetchLogFiles(String(this.id))
      if (this.experimentPreprocessHistoryId != null) {
        await this.fetchPreprocessLogFiles(String(this.id))
      }
    },
  },
}
</script>

<style lang="scss" scoped></style>
