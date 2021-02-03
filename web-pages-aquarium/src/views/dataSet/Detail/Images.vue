<template>
  <div>
    <el-row class="tac">
      <el-col :span="24" style="padding:15px">
        データセットバージョン
        <el-select
          v-model="versionValue"
          placeholder="Select"
          @change="currentChange"
        >
          <el-option
            v-for="item in versions"
            :key="item.value"
            :label="item.version"
            :value="item.id"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <el-col :span="8">
        <h3 style="padding:15px">ファイル一覧</h3>
        <el-collapse>
          <el-collapse-item
            v-for="(item, index) in detailVersion.flatEntries"
            :key="index"
            :name="index"
            :index="index"
          >
            <template slot="title">
              <span style="margin-left:20px;display:inline-block;width:400px">
                <span class="data-name" style="" @click="dataClick(item)"
                  >データ:{{ item.name }}</span
                >
                <i class="el-icon-delete" @click="openDeleteDialog(item)"></i>
              </span>
            </template>
            <ul style="margin-left:20px">
              <li
                v-for="(file, subIndex) in item.list"
                :key="subIndex"
                style="list-style-type: none;padding-left:10px"
                :index="subIndex"
                class="file-li "
                @click="fileClick(file, $event)"
              >
                {{ file.fileName }}
              </li>
            </ul>
          </el-collapse-item>
        </el-collapse>
        <el-dialog title="" :visible.sync="deleteDialog" width="30%">
          <span
            >データ"{{
              selectDeleteData.name
            }}"を削除して新しいデータセットバージョンを作成しますか？</span
          >
          <span slot="footer" class="dialog-footer">
            <el-button @click="deleteDialog = false">Cancel</el-button>
            <el-button type="primary" @click="deleteData()">
              削除する
            </el-button>
          </span>
        </el-dialog>
      </el-col>
      <el-col :span="16">
        <el-row>
          <el-col>
            <h3 style="padding:15px">プレビュー</h3>
          </el-col>
        </el-row>

        <div style="padding:10px">
          <el-card
            v-for="(file, index) in selectImageList"
            :key="index"
            :body-style="{ padding: '0px' }"
            style="width:270px;height:300px ;float: left;"
            :offset="index > 0 ? 2 : 0"
          >
            <img
              :src="file.url"
              class="image"
              style="width:270px;height:270px "
            />
            <div style="padding-left:10px">
              {{ file.fileName }}
            </div>
          </el-card>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
export default {
  title: 'データセット',
  components: {},
  props: {
    id: {
      type: String,
      default: null,
    },
    datasetname: {
      type: String,
      default: null,
    },
    latestVersionId: {
      type: Number,
      default: null,
    },
  },

  data() {
    return {
      deleteDialog: false,
      selectDeleteData: { name: null },
      selectImageList: [],

      versionValue: null,
    }
  },

  computed: {
    ...mapGetters({
      dataSets: ['aquariumDataSet/dataSets'],
      total: ['aquariumDataSet/total'],
      versions: ['aquariumDataSet/versions'],
      detailVersion: ['aquariumDataSet/detailVersion'],
      detail: ['dataSet/detail'],
      datas: ['data/data'],
      dataList: ['data/uploadedFiles'],
    }),
  },

  watch: {
    async latestVersionId() {
      this.versionValue = this.latestVersionId
    },
  },

  async created() {
    await this.retrieveData()
  },

  methods: {
    ...mapActions([
      'aquariumDataSet/post',
      'aquariumDataSet/postByIdVersions',
      'aquariumDataSet/fetchVersions',
      'aquariumDataSet/fetchDataSets',
      'aquariumDataSet/fetchTest',
      'aquariumDataSet/fetchDetailVersion',

      'data/post',
      'data/put',
      'data/fetchData',
      'data/fetchUploadedFiles',
      'data/clearUploadedFiles',

      'data/putFile',
      'dataSet/fetchDetail',
      'dataSet/post',
    ]),
    openDeleteDialog(item) {
      this.selectDeleteData = item
      this.deleteDialog = true
    },
    fileClick(file, e) {
      for (let i in this.selectImageList) {
        if (this.selectImageList[i].fileId == file.fileId) {
          //同じものをクリックした場合、リストから削除する
          this.selectImageList.splice(i, 1)
          e.target.classList.remove('active-datafile')
          return
        }
      }

      e.target.classList.add('active-datafile')
      this.selectImageList.push(file)
    },
    async dataClick(item) {
      //データのファイルリストを取得する
      await this['data/fetchUploadedFiles'](item.id)
      item['list'] = this.dataList.concat()

      this.$forceUpdate()
    },
    async deleteData() {
      this.deleteDialog = false
      let flatEntry = []

      for (let i in this.detailVersion.flatEntries) {
        //selectDeleteData
        if (
          this.selectDeleteData.id != this.detailVersion.flatEntries[i]['id']
        ) {
          flatEntry.push({
            id: this.detailVersion.flatEntries[i]['id'],
          })
        }
      }
      //カモノハシデータセットを新規作成
      //カモノハシのデータセットを登録する
      let datasetparams = {
        entries: {},
        flatEntries: flatEntry,
        isFlat: true,
        name: 'aquqrium_' + this.datasetname,
        memo: '',
      }

      let dataset = await this['dataSet/post'](datasetparams)
      //アクアリウムデータセットバージョンを登録する
      let version = await this['aquariumDataSet/postByIdVersions']({
        //id: aqDataset.data.id,
        id: this.id,
        model: { datasetId: dataset.data.id },
      })
      this.$emit('latestVersionId', version.id)
      this.retrieveData()
      this.versionValue = version.id

      //再描画
      this.$forceUpdate()
    },

    async currentChange(version) {
      let params = {}

      params.versionId = version
      params.id = this.id
      await this['aquariumDataSet/fetchDetailVersion'](params)
    },
    async retrieveData() {
      let params = {}

      params.page = 1
      params.perPage = 10
      params.withTotal = true
      params.id = this.id

      await this['aquariumDataSet/fetchDataSets'](params)
      await this['aquariumDataSet/fetchVersions'](this.id)

      for (let i in this.versions) {
        if (this.versions[i].version == this.dataSets[0].latestVersion) {
          this.versionValue = this.versions[i].id
          this.currentChange(this.versions[i].id)
        }
      }
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    openCreateDialog() {
      this.$router.push('/dataset/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/dataset/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.file-li {
  cursor: pointer;
}
.active-datafile {
  color: #409eff;
  background-color: #40a0ff25;
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

/******** */
.time {
  font-size: 13px;
  color: #999;
}

.bottom {
  margin-top: 13px;
  line-height: 12px;
}

.button {
  padding: 0;
  float: right;
}

.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: '';
}

.clearfix:after {
  clear: both;
}

.data-name {
  margin-left: 0px;
  display: inline-block;
  width: 360px;
}
.data-name:hover {
  color: #409eff;
}
</style>
