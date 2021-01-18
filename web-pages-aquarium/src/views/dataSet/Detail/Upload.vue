<template>
  <div>
    <h2>インポートするファイルの選択</h2>
    <div style="width:600px;padding-top:20px;padding-bottom:40px">
      カスタムモデルを作成するには、最初に一連の画像をインポートしてトレーニングする必要があります。各画像はラベルで分類する必要があります（ラベルは画像を特定する方法をモデルに伝えるために不可欠です）
    </div>
    <el-radio v-model="importfile" label="1" style="display:block;padding:10px">
      パソコンから画像をアップロード
    </el-radio>
    <el-radio v-model="importfile" label="2" style="display:block;padding:10px">
      KAMONOHASHIからデータセットを選択
    </el-radio>
    <div v-if="importfile == 1" class="importfile-detail">
      <h3>パソコンから画像をアップロード</h3>
      任意のファイル形式がアップロードできます。画像の表示はJPG,PNG,ZIPがサポートされています。<br />
      1回のアップロードでさいだいXXXファイル送信できます。アップロードしたファイルはKAMONOHASHIのデータ、データセットに保存されます。

      <el-row>
        <kqi-upload-form ref="uploadForm" title="File" :type="type" />
        <el-link
          type="info"
          style="display:block;margin-top:10px ;padding:15px"
          @click="submit()"
        >
          続行
        </el-link>
      </el-row>
    </div>
    <div v-else-if="importfile == 2" class="importfile-detail">
      <h3>KAMONOHASHIからデータセットを選択</h3>
      <el-button
        type="plain"
        plain
        style="display:block;margin-top:10px"
        @click="openDialog()"
      >
        データを選択
      </el-button>
    </div>
  </div>
</template>

<script>
import KqiUploadForm from '@/components/KqiUploadForm'
import { mapActions, mapGetters } from 'vuex'
export default {
  title: 'データセット',
  components: { KqiUploadForm },
  data() {
    return {
      type: 'Data',
      importfile: null,
      searchCondition: {},
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
    }
  },
  computed: {
    ...mapGetters({
      dataSets: ['aquariumDataSet/dataSets'],
      total: ['aquariumDataSet/total'],
      versions: ['aquariumDataSet/versions'],
      detail: ['dataSet/detail'],
    }),
  },
  props: {
    id: {
      type: String,
      default: null,
    },
    datasetname: {
      type: String,
      default: null,
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

      'data/post',
      'data/put',

      'data/putFile',
      'dataSet/fetchDetail',
      'dataSet/post',
      'data/uploadedFiles',
    ]),
    async submit() {
      try {
        await this.postDataSet()
        this.$emit('done')
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    // データファイルのアップロード
    async updateData(filename) {
      let model = {
        name: filename,
        memo: '',
        tags: '',
        isRaw: true,
      }
      let result = null
      result = (await this['data/post'](model)).data
      return result.id
    },

    async uploadFile(datasetname) {
      //データファイルのアップロード
      let dataFileInfos = await this.$refs.uploadForm.uploadFile()
      let dataId = null
      dataId = await this.updateData('aquarium_' + datasetname)
      await this['data/putFile']({
        id: dataId,
        fileInfo: dataFileInfos,
      })
      return dataId
    },
    async postDataSet() {
      //アクアリウムデータセットバージョン一覧を取得し、最新バージョンのデータセットIdを取得する
      let newVer = null
      let dataSetId = null
      for (let i in this.versions) {
        if (newVer == null) {
          newVer = this.versions[i]['version']
          dataSetId = this.versions[i]['dataSetId']
        } else if (newVer <= this.versions[i]['version']) {
          newVer = this.versions[i]['version']
          dataSetId = this.versions[i]['dataSetId']
        }
      }
      //最新バージョンのデータセットのデータリストを取得する
      await this['dataSet/fetchDetail'](dataSetId)
      let datas = this.detail.flatEntries
      let flatEntry = []
      for (let i in datas) {
        flatEntry.push({ id: datas[i]['id'] })
      }
      //ローカルからのデータリストを登録する
      let dataId = await this.uploadFile(this.datasetname)

      //新しく追加したデータをデータリストに追加
      flatEntry.push({ id: dataId })
      //カモノハシのデータセットを登録する
      let datasetparams = {
        entries: {},
        flatEntries: flatEntry,
        isFlat: true,
        name: 'aquqrium_' + this.datasetname,
        memo: '',
      }

      let dataset = await this['dataSet/post'](datasetparams)

      //アクアリウムデータセットは登録しない

      //アクアリウムデータセットバージョンを登録する
      this['aquariumDataSet/postByIdVersions']({
        //id: aqDataset.data.id,
        id: this.id,
        model: { datasetId: dataset.data.id },
      })
    },
    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      params.id = this.id
      await this['aquariumDataSet/fetchDataSets'](params)
      await this['aquariumDataSet/fetchVersions'](this.id)
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
</style>
