<template>
  <div>
    <h2>DEBUG</h2>
    <div v-if="value != null" class="debug-list">
      <el-row>
        <el-col :span="8">実験ID</el-col>
        <el-col :span="16"> {{ id }}</el-col>
      </el-row>
      <el-row>
        <el-col :span="8">実験前処理ID</el-col>
        <el-col :span="16"> {{ value.preprocessId }}</el-col>
      </el-row>
      <el-row>
        <el-col :span="8">前処理ログ</el-col>
        <el-col :span="16">
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
      <el-row>
        <el-col :span="8">学習ログ</el-col>
        <el-col :span="16">
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
      <el-row>
        <el-col :span="8">前処理ステータス</el-col>
        <el-col :span="16">{{ value.preprocessStatus }}</el-col>
      </el-row>
      <el-row>
        <el-col :span="8">学習ステータス</el-col>
        <el-col :span="16">{{ value.trainingStatus }}</el-col>
      </el-row>
    </div>
    <h2>推論DEBUG</h2>
    <div>
      <el-table :data="evaluationLogFileDatas" style="width: 100%">
        <el-table-column prop="name" label="名前"> </el-table-column>
        <el-table-column prop="log" label="ログ"> </el-table-column>
      </el-table>
    </div>
  </div>
</template>

<script>
import KqiDownloadButton from '../../../components/KqiDownloadButton.vue'
import { mapActions, mapGetters } from 'vuex'

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
      evaluationLogFileDatas: [],
    }
  },
  computed: {
    ...mapGetters({
      uploadedFiles: ['training/uploadedFiles'],
      evaluations: ['experiment/evaluations'],
    }),
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
    ...mapActions([
      'experiment/fetchEvaluations',
      'training/fetchUploadedFiles',
    ]),

    async retrieveData() {
      if (this.value != null && this.value.preprocessId != null) {
        await this['training/fetchUploadedFiles'](
          String(this.value.preprocessId),
        )
        this.preprocessLogFileData = Object.assign({}, this.uploadedFiles)
      }
      if (this.value != null && this.value.trainingId != null) {
        await this['training/fetchUploadedFiles'](String(this.value.trainingId))
        this.trainingLogFileData = Object.assign({}, this.uploadedFiles)
      }
      await this['experiment/fetchEvaluations'](this.id)
      for (let i in this.evaluations) {
        await this['training/fetchUploadedFiles'](
          String(this.evaluations[i].training.id),
        )
        let logdata = Object.assign({}, this.uploadedFiles)
        this.evaluationLogFileDatas.push({
          name: this.evaluations[i].name,
          log: logdata,
        })
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
.debug-list {
  margin: 40px 0;
  width: 60%;
}
.debug-list .el-row {
  padding: 15px;
  border-bottom: 1px solid rgb(235, 238, 245);
}
h3 {
  font-size: 20px;
  margin: 10px 0;
}
.el-table .nonactive-row {
  background: red;
}
</style>
