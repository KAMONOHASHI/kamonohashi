<template>
  <div>
    <h2>DEBUG</h2>
    <div class="confusion-matrix">
      <el-row style="margin-bottom:10px">
        <el-col :span="6">実験ID</el-col>
        <el-col :span="18"> {{ id }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">実験前処理ID</el-col>
        <el-col :span="18"> {{ experimentPreprocessHistoryId }}</el-col>
      </el-row>
      <el-row style="margin-bottom:10px">
        <el-col :span="6">前処理ログ</el-col>
        <el-col :span="18">
          <div class="el-input">
            <div v-if="preprocessLogFiles && preprocessLogFiles.length > 0">
              <div
                v-for="(preprocessLog, index) in preprocessLogFiles"
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
            <div v-if="logFiles && logFiles.length > 0">
              <div v-for="(logFile, index) in logFiles" :key="index">
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
        <el-col :span="18">{{ value.status }}</el-col>
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
const { mapGetters, mapActions } = createNamespacedHelpers('experiment')

export default {
  title: '実験結果',
  components: { KqiDownloadButton },
  props: {
    id: {
      type: String,
      default: null,
    },
    experimentPreprocessHistoryId: {
      type: Number,
      default: null,
    },
    value: {
      type: Object,
      default: () => ({
        id: null,
        name: '',
        createdAt: '',
        createdBy: '',
        dataSetId: null,
        dataSetVersion: null,
        dataSetURL: '',
        templateURL: '',
      }),
    },
  },
  data() {
    return {
      importfile: null,
      logFileData: [],
      preprocessLogFileData: [],
    }
  },
  computed: {
    ...mapGetters(['detail', 'logFiles', 'preprocessLogFiles']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchLogFiles', 'fetchPreprocessLogFiles', 'fetchDetail']),
    async retrieveData() {
      await this.fetchLogFiles(String(this.id))
      if (this.experimentPreprocessHistoryId !== null) {
        await this.fetchPreprocessLogFiles(String(this.id))
      }
      for (let i in this.logFiles) {
        var req = new XMLHttpRequest()
        req.open('GET', this.logFiles[i].url, true)
        req.responseType = 'blob'

        var that = this
        //読込終了後の処理
        req.onload = function(e) {
          //テキストエリアに表示する
          var blob = e.target.response

          var reader = new FileReader()
          //テキスト形式で読み込む
          reader.readAsText(blob)

          //読込終了後の処理
          reader.onload = function() {
            //テキストエリアに表示する
            that.logFileData.push(reader.result)
          }
        }
        req.send()
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
