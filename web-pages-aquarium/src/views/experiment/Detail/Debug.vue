<template>
  <div>
    <h2>DEBUG</h2>
    <div v-if="value != null" class="confusion-matrix">
      <el-row style="margin-bottom:10px">
        <el-col :span="6">実験ID</el-col>
        <el-col :span="18"> {{ id }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">実験前処理ID</el-col>
        <el-col :span="18"> {{ value.preprocessId }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">前処理ログ</el-col>
        <el-col :span="18">
          <div class="el-input">
            <div v-if="preprocessLogFileData">
              <div
                v-for="(preprocessLog, index) in preprocessLogFileData"
                :key="index"
              >
                <kqi-download-button
                  :download-url="preprocessLog.url"
                  :file-name="preprocessLog.fileName"
                />
              </div>
            </div>
            <div v-else>この実験には前処理はありません</div>
          </div>
        </el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">学習ログ</el-col>
        <el-col :span="18">
          <div class="el-input">
            <div v-if="trainingLogFileData">
              <div v-for="(logFile, index) in trainingLogFileData" :key="index">
                <kqi-download-button
                  :download-url="logFile.url"
                  :file-name="logFile.fileName"
                />
              </div>
            </div>
            <div v-else>この実験には学習はありません</div>
          </div>
        </el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">前処理ステータス</el-col>
        <el-col :span="18">{{ value.preprocessStatus }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">学習ステータス</el-col>
        <el-col :span="18">{{ value.trainingStatus }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6"></el-col>
        <el-col :span="18"></el-col>
      </el-row>
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'

import KqiDownloadButton from '../../../components/KqiDownloadButton.vue'
const { mapGetters, mapActions } = createNamespacedHelpers('training')

export default {
  title: '実験結果',
  components: { KqiDownloadButton },
  props: {
    id: {
      type: String,
      default: null,
    },

    value: {
      type: Object,
      default: null,
    },
  },
  data() {
    return {
      importfile: null,
      logFileData: [],
      preprocessLogFileData: null,
      trainingLogFileData: null,
    }
  },
  computed: {
    ...mapGetters(['detail', 'uploadedFiles']),
  },
  watch: {
    async value() {
      await this.retrieveData()
    },
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDetail', 'fetchUploadedFiles']),
    async retrieveData() {
      if (this.value != null && this.value.preprocessId != null) {
        await this.fetchUploadedFiles(String(this.value.preprocessId))
        this.preprocessLogFileData = Object.assign({}, this.uploadedFiles)
      }
      if (this.value != null && this.value.trainingId != null) {
        await this.fetchUploadedFiles(String(this.value.trainingId))
        this.trainingLogFileData = Object.assign({}, this.uploadedFiles)
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.importfile-detail {
  padding-top: 50px;
}
.importfile-detail > h3 {
  padding-bottom: 10px;
}
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
.confusion-matrix {
  margin: 40px 0;
}
h3 {
  font-size: 20px;
  margin: 10px 0;
}
.el-table .nonactive-row {
  background: red;
}
</style>
