<template>
  <div>
    <h2>データセット詳細:{{ name }}</h2>
    <hr />
    <el-row class="tac">
      <el-col :span="10" style="padding:15px">
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
            <span style="float: left">{{ item.version }}</span>
            <span
              style="margin-left:5px;float: right; color: #8492a6; font-size: 12px"
              >{{ item.memo }}</span
            >
          </el-option>
        </el-select>
      </el-col>
      <el-col :span="14" style="padding:15px">
        <el-button type="primary" plain @click="uploadDialog = true"
          >アップロード</el-button
        >
        <el-button plain @click="deleteVersionDialog = true"
          >データセットバージョン削除</el-button
        >
        <el-button plain @click="deleteDataSetDialog = true"
          >データセット削除</el-button
        >
        <el-dialog title="" :visible.sync="deleteVersionDialog" width="30%">
          <span v-if="versions.length == 1">
            このデータセットにはバージョン"{{
              viewVersion.version
            }}"しか存在しないため<br />
            データセットバージョン削除することができません。<br />
            <br />
            削除をしたい場合はデータセット削除を選択してください。</span
          >
          <span v-else
            >バージョン"{{ viewVersion.version }}"を削除しますか？</span
          >
          <span v-if="versions.length == 1" slot="footer" class="dialog-footer">
            <el-button @click="deleteVersionDialog = false">OK</el-button>
          </span>
          <span v-else slot="footer" class="dialog-footer">
            <el-button @click="deleteVersionDialog = false">Cancel</el-button>
            <el-button type="primary" @click="deleteVersion()">
              削除する
            </el-button>
          </span>
        </el-dialog>
        <el-dialog title="" :visible.sync="deleteDataSetDialog" width="30%">
          <span>データセットを削除しますか？</span>
          <span slot="footer" class="dialog-footer">
            <el-button @click="deleteDataSetDialog = false">Cancel</el-button>
            <el-button type="primary" @click="deleteDataSet()">
              削除する
            </el-button>
          </span>
        </el-dialog>
      </el-col>
    </el-row>
    <el-row style="margin:15px">
      <el-col :span="8">
        <h3>メモ</h3>
        <el-input v-model="viewVersion.memo" type="textarea" />
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="8">
        <h3 style="padding:15px">ファイル一覧</h3>
        <el-collapse>
          <div
            v-for="(item, index) in viewVersion.flatEntries"
            :key="index"
            :name="index"
            :index="index"
          >
            <div>
              <span
                style="margin-left:0px;margin-top:5px;padding:5px;display:inline-block;width:400px"
              >
                <span class="data-name" style="" @click="dataClick(item)">
                  <i
                    v-show="!item.show || item.show == null"
                    style="display:inline-block;width:20px"
                    class="el-icon-arrow-down"
                  ></i>
                  <i
                    v-show="item.show"
                    style="display:inline-block;width:20px"
                    class="el-icon-arrow-up"
                  ></i
                  >データ:{{ item.name }}</span
                >
                <i class="el-icon-delete" @click="openDeleteDialog(item)"></i>
              </span>
            </div>
            <div>
              <ul
                v-show="item.show"
                style="margin-left:20px;max-height:200px;overflow:auto;"
              >
                <li
                  v-for="(file, subIndex) in item.list"
                  :key="subIndex"
                  style="list-style-type: none;padding-left:10px;margin-top:5px"
                  :index="subIndex"
                  class="file-li "
                >
                  <div
                    style="float:left;width:90%;padding-top:5px;padding-bottom:5px"
                    @click="fileClick(file, $event)"
                  >
                    {{ file.fileName }}
                  </div>
                  <div
                    style="float:left;width:10%;padding-top:5px;padding-bottom:5px"
                  >
                    <a :href="file.url"><i class="el-icon-download"></i></a>
                  </div>
                </li>
              </ul>
            </div>
          </div>
        </el-collapse>
        <el-dialog
          title="インポートするファイルの選択"
          :visible.sync="uploadDialog"
          width="50%"
        >
          <div style="padding-bottom:20px">
            データセットにデータを追加することができます。
          </div>
          <el-radio
            v-model="importfile"
            label="1"
            style="display:block;padding:10px"
          >
            パソコンから画像をアップロード
          </el-radio>

          <el-radio
            v-model="importfile"
            label="2"
            style="display:block;padding:10px"
          >
            KAMONOHASHIからデータを選択
          </el-radio>
          <hr />
          <div
            v-if="importfile == 1"
            class="importfile-detail"
            style="padding-top:20px"
          >
            <h3>パソコンから画像をアップロード</h3>

            <el-row style="padding:20px">
              任意のファイル形式がアップロードできます。画像の表示はJPG,PNGがサポートされています。<br />
              1回のアップロードで最大10000ファイル送信できます。アップロードしたファイルはKAMONOHASHIのデータ、データセットに保存されます。

              <el-form
                ref="createForm"
                v-loading="loading"
                element-loading-spinner=" "
                element-loading-background="rgba(255, 255, 255, 0.7)"
              >
                <kqi-display-error :error="error" />
                <kqi-upload-form ref="uploadForm" title="File" :type="type" />

                <el-row style="marginTop: 30px">
                  <el-col :span="24">
                    <h3>アップロードメモ</h3>
                    <el-input v-model="uploadMemo" type="textarea" />
                  </el-col>
                </el-row>
                <br />
                <el-button
                  type="plain"
                  plain
                  style="margin-top:10px"
                  @click="submit()"
                >
                  続行
                </el-button>
              </el-form>
            </el-row>
          </div>
          <div
            v-else-if="importfile == 2"
            class="importfile-detail"
            style="padding-top:20px"
          >
            <h3 style="">KAMONOHASHIからデータを選択</h3>

            <div style="padding:20px">
              KAMONOHASHIに登録してあるデータを複数選択できます。<br />
              <kqi-display-error :error="error" />
              <el-row>
                <div
                  style="width:80%;height:250px;padding:20px;border:1px solid #CCC;border-radius:5px;margin-top:5px"
                >
                  <el-checkbox-group v-model="checkList">
                    <div v-for="item in allDatas" :key="item.id">
                      <el-checkbox :label="item.id">{{
                        item.name
                      }}</el-checkbox>
                    </div>
                  </el-checkbox-group>
                </div>
                <kqi-pagination
                  v-model="dataPageStatus"
                  :total="dataTotal"
                  @change="initialize"
                />
              </el-row>
              <el-row style="margin-bottom: 30px;">
                <el-col :span="24">
                  <h3>アップロードメモ</h3>
                  <el-input v-model="uploadMemo" type="textarea" />
                </el-col>
              </el-row>
              <el-row>
                <el-button
                  type="plain"
                  plain
                  style="margin-top:10px"
                  @click="submit"
                >
                  選択
                </el-button>
                <el-button
                  type="plain"
                  plain
                  style="margin-top:10px"
                  @click="closeDialog"
                >
                  キャンセル
                </el-button></el-row
              >
            </div>
          </div>
        </el-dialog>

        <el-dialog title="" :visible.sync="deleteDialog" width="30%">
          <span
            >データ"{{
              selectDeleteData.name
            }}"を削除して新しいデータセットバージョンを作成しますか？</span
          >
          <el-row style="marginTop: 30px">
            <h3>削除メモ</h3>
            <el-input v-model="deleteMemo" type="textarea" />
          </el-row>
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

        <div style="padding:10px;height:600px;overflow:auto;">
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

import KqiDisplayError from '@/components/KqiDisplayError'
import KqiUploadForm from '@/components/KqiUploadForm'

import KqiPagination from '@/components/KqiPagination'
export default {
  title: 'データセット',

  components: { KqiUploadForm, KqiDisplayError, KqiPagination },
  props: {
    id: {
      type: String,
      default: null,
    },
  },

  data() {
    return {
      deleteDialog: false,
      uploadDialog: false,
      deleteVersionDialog: false,

      deleteDataSetDialog: false,
      selectDeleteData: { name: null },
      selectImageList: [],
      versionValue: null,
      version: null,
      type: 'Data',
      loading: false,
      drawer: false,
      direction: 'rtl',
      importfile: null,
      searchCondition: {},
      dataPageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      checkList: [],
      datas: [],
      error: null,
      name: null,
      memo: '',
      uploadMemo: '',
      deleteMemo: '',
      viewVersion: { memo: null, flatEntries: null },
      errVersion: null,
    }
  },

  computed: {
    ...mapGetters({
      dataSets: ['aquariumDataSet/dataSets'],
      total: ['aquariumDataSet/total'],
      versions: ['aquariumDataSet/versions'],
      detailVersion: ['aquariumDataSet/detailVersion'],
      detail: ['dataSet/detail'],
      dataTotal: ['data/total'],
      allDatas: ['data/data'],
      dataList: ['data/uploadedFiles'],
      tenantDetail: ['tenant/detail'],
      account: ['account/account'],
    }),
  },

  async created() {
    let tenantName = this.$route.query.tenantName
    await this['account/fetchAccount']()
    //テナント名からテナントIDを取得し、セットする
    for (let i in this.account.tenants) {
      if (this.account.tenants[i].name == tenantName) {
        await sessionStorage.setItem(
          '.Platypus.Tenant',
          this.account.tenants[i].id,
        )
      }
    }
    await this['tenant/fetchCurrentTenant']()

    let tab = this.$route.query.tab
    if (tab != null) {
      this.activeName = tab
    }

    let version = this.$route.query.version
    if (version != null) {
      this.version = Number(version)
    }
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
      'aquariumDataSet/deleteVersion',
      'aquariumDataSet/delete',
      'data/post',
      'data/put',
      'data/fetchData',
      'data/fetchUploadedFiles',
      'data/clearUploadedFiles',
      'data/putFile',
      'dataSet/fetchDetail',
      'dataSet/post',
      'tenant/fetchCurrentTenant',
      'account/fetchAccount',
    ]),
    async deleteVersion() {
      let param = { id: this.id, versionId: this.versionValue }
      await this['aquariumDataSet/deleteVersion'](param)

      this.deleteVersionDialog = false
      //storeのversionをクリア
      await this['aquariumDataSet/fetchVersions'](null)
      this.version = null
      this.retrieveData()

      //再描画
      this.$forceUpdate()
      await this.$notify.success({
        type: 'Success',
        message: `バージョンを削除しました。`,
      })
    },
    async deleteDataSet() {
      this['aquariumDataSet/delete'](this.id)
      this.deleteDataSetDialog = false
      this.$router.push('/aquarium/dataset')
    },

    openDeleteDialog(item) {
      this.selectDeleteData = item
      this.deleteDialog = true
    },
    fileClick(file, e) {
      let imgList = ['png', 'PNG', 'jpg', 'JPG', 'jpeg', 'JPEG']
      let nm = file.key.split('.').slice(-1)[0]
      let filenm = file.fileName.split('.').slice(-1)[0]
      if (imgList.indexOf(nm) == -1 && imgList.indexOf(filenm) == -1) {
        //画像ファイルではないときは何も表示しない
        return
      }
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

      if (item['show'] == null) {
        item['show'] = true
        await this['data/fetchUploadedFiles'](item.id)
        item['list'] = this.dataList.concat()
      } else if (item['show'] == true) {
        item['show'] = false
      } else if (item['show'] == false) {
        item['show'] = true
      }
      this.$forceUpdate()
    },
    async deleteData() {
      this.deleteDialog = false
      let flatEntry = []

      for (let i in this.viewVersion.flatEntries) {
        //selectDeleteData
        if (this.selectDeleteData.id != this.viewVersion.flatEntries[i]['id']) {
          flatEntry.push({
            id: this.viewVersion.flatEntries[i]['id'],
          })
        }
      }
      //カモノハシデータセットを新規作成
      //カモノハシのデータセットを登録する
      let datasetparams = {
        entries: {},
        flatEntries: flatEntry,
        isFlat: true,
        name: 'aquqrium_' + this.name,
        memo: this.deleteMemo,
      }

      let dataset = await this['dataSet/post'](datasetparams)
      //アクアリウムデータセットバージョンを登録する
      let version = await this['aquariumDataSet/postByIdVersions']({
        //id: aqDataset.data.id,
        id: this.id,
        model: { datasetId: dataset.data.id },
      })
      this.version = null
      this.retrieveData()
      this.versionValue = version.id

      //再描画
      this.$forceUpdate()
      await this.$notify.success({
        type: 'Success',
        message: `データを削除して新しいデータセットバージョンを作成しました。`,
      })
    },

    async currentChange(version) {
      //選択されたバージョンのデータセットバージョンを取得する
      let params = {}
      params.versionId = version
      params.id = this.id
      await this['aquariumDataSet/fetchDetailVersion'](params)

      this.viewVersion = Object.assign({}, this.detailVersion)
      let tenantName = this.$route.query.tenantName
      this.$router.replace({
        query: { version: this.viewVersion.version, tenantName: tenantName },
      })
    },
    async retrieveData() {
      //アクアリウムデータセットバージョン情報を取得
      let params = {}
      params.page = 1
      params.perPage = 10
      params.withTotal = true
      params.id = this.id
      await this['aquariumDataSet/fetchDataSets'](params)
      await this['aquariumDataSet/fetchVersions'](this.id)
      let latestVersionId = null
      let URLVerExistFlg = false
      for (let i in this.versions) {
        if (this.versions[i].version == this.version) {
          URLVerExistFlg = true
          this.versionValue = this.versions[i].id
        } else if (this.versions[i].version == this.dataSets[0].latestVersion) {
          latestVersionId = this.versions[i].id
        }
        //バージョンごとのメモを取得する
        let param = {}
        param.versionId = this.versions[i].id
        param.id = this.id

        await this['aquariumDataSet/fetchDetailVersion'](param)
        if (this.detailVersion.memo.length > 30) {
          this.versions[i].memo = this.detailVersion.memo.substr(0, 30) + '...'
        } else {
          this.versions[i].memo = this.detailVersion.memo
        }
      }
      if (!URLVerExistFlg && this.version != null) {
        //URLのversionが存在しなかった場合
        this.errVersion = this.version

        this.versionValue = latestVersionId
      } else if (!URLVerExistFlg && this.version == null) {
        //URLにversionパラメタが存在しなかった場合
        this.versionValue = latestVersionId
      }

      this.name = this.dataSets[0].name

      //データセットバージョンを取得
      await this['aquariumDataSet/fetchDetailVersion']({
        id: this.id,
        versionId: this.versionValue,
      })

      this.viewVersion = Object.assign({}, this.detailVersion)
      //アクアリウムデータセットに追加するためのデータリスト取得
      let params2 = this.searchCondition
      params2.page = this.dataPageStatus.currentPage
      params2.perPage = this.dataPageStatus.currentPageSize
      params2.withTotal = true
      await this['data/fetchData'](params2)
      this.datas = []
      for (let i in this.allDatas) {
        //最新のバージョンに存在するDataはカモノハシデータリストのdatasに入れない
        let same = false
        for (let j in this.viewVersion.flatEntries) {
          if (this.allDatas[i].id == this.viewVersion.flatEntries[j].id) {
            same = true
            break
          }
        }

        if (!same) {
          this.datas.push(this.allDatas[i])
        }
      }
      this.selectImageList = []
      let tenantName = this.$route.query.tenantName
      this.$router
        .replace({
          query: { version: this.viewVersion.version, tenantName: tenantName },
        })
        .catch(function() {})

      if (this.errVersion != null) {
        await this.$notify.error({
          type: 'Error',
          message:
            'version:' +
            this.errVersion +
            'のデータセットバージョンは見つかりませんでした。最新のデータセットバージョンを表示します。',
        })
        this.errVersion = null
      }
    },
    async initialize() {
      //ページを変えてデータリストを取得
      let params = this.searchCondition
      params.page = this.dataPageStatus.currentPage
      params.perPage = this.dataPageStatus.currentPageSize
      params.withTotal = true
      await this['data/fetchData'](params)
    },
    //----------------

    async selectData(dataId) {
      //セレクトボックスからデータを選択
      await this['data/fetchUploadedFiles'](dataId)
    },
    closeDialog() {
      this.uploadDialog = false
      this['data/clearUploadedFiles']()
      this.checkList = []
    },

    //ローカルからアップロード
    async submit() {
      this.$store.commit('setLoading', false)

      this.loading = true
      try {
        await this.postDataSet()
        this.error = null
        this.closeDialog()
        this.version = null
        this.retrieveData()
        await this.$notify.success({
          type: 'Success',
          message: `データを追加して新しいデータセットバージョンを作成しました。`,
        })
      } catch (e) {
        this.error = e
      } finally {
        // 共通側ローディングを再度有効化
        this.loading = false
        this.$store.commit('setLoading', true)
        this['data/clearUploadedFiles']()
        if (this.$refs.uploadForm != null) {
          this.$refs.uploadForm.showProgress = false
        }
        if (this.importfile == 1) {
          // 選択したファイルを削除する
          this.$refs.uploadForm.selectedFiles = undefined
          this.$refs.uploadForm.filesArray = []
        }
      }
      // エラーがない場合、詳細イメージタブ画面に遷移

      if (this.error) {
        this.$notify.error({
          title: 'Error',
          message: 'データ追加に失敗しました',
        })
      }
    },
    // データファイルのアップロード
    async updateData(filename) {
      let model = {
        name: filename,
        memo: this.uploadMemo,
        tags: ['aquarium'],
        isRaw: true,
      }
      let result = null
      result = (await this['data/post'](model)).data
      return result.id
    },

    async uploadFile(name) {
      //データファイルのアップロード
      let dataFileInfos = await this.$refs.uploadForm.uploadFile()

      let dataId = null
      let date = new Date().toLocaleString()
      dataId = await this.updateData(`${name} ${date}`)
      await this['data/putFile']({
        id: dataId,
        fileInfo: dataFileInfos,
      })
      return dataId
    },
    async postDataSet() {
      //選択されているバージョンのデータセットのデータリストに追加する
      let datas = this.viewVersion.flatEntries
      let flatEntry = []
      for (let i in datas) {
        flatEntry.push({ id: datas[i]['id'] })
      }
      if (this.importfile == 1) {
        if (
          this.$refs.uploadForm._data.selectedFiles == null ||
          this.$refs.uploadForm._data.selectedFiles.length == 0
        ) {
          throw new Error('ファイルを選択してください')
        }
        //ローカルからのデータリストを登録する
        let dataId = await this.uploadFile(this.name)
        //新しく追加したデータをデータリストに追加
        flatEntry.push({ id: dataId })
      } else if (this.importfile == 2) {
        //カモノハシのデータを登録する
        if (this.checkList.length == 0) {
          throw new Error('データを選択してください')
        }
        for (let i in this.checkList) {
          flatEntry.push({ id: this.checkList[i] })
        }
      }
      //カモノハシのデータセットを登録する
      let datasetparams = {
        entries: {},
        flatEntries: flatEntry,
        isFlat: true,
        name: 'aquqrium_' + this.name,
        memo: this.uploadMemo,
      }

      let dataset = await this['dataSet/post'](datasetparams)

      //アクアリウムデータセットは登録しない

      //アクアリウムデータセットバージョンを登録する
      await this['aquariumDataSet/postByIdVersions']({
        //id: aqDataset.data.id,
        id: this.id,
        model: { datasetId: dataset.data.id },
      })

      // アップロード完了後の初期化
      this.uploadMemo = ''
      if (this.importfile == 1) {
        this.$refs.uploadForm._data.selectedFiles = null
      }
    },

    //-----------------
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
