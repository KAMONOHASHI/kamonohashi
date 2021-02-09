<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    :submit-text="submitText"
    @submit="submit"
    @delete="deleteDataSet"
    @close="$emit('cancel')"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-row>
        <el-form-item label="データセット名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
      </el-row>

      <el-row>
        <h3 style="margin-bottom:10px;margin-top:30px">
          パソコンから画像をアップロード
        </h3>
        <div style="margin:20px">
          パソコンから画像をアップロード
          任意のファイル形式がアップロードできます。画像の表示はJPG,PNGがサポートされています。<br />
          一回のアップロードで最大{{
            fileNumLimit
          }}ファイル送信できます。アップロードしたファイルはKAMONOHASHIのデータ、データセットに保存されます。
        </div>
        <kqi-upload-form ref="uploadForm" title="File" :type="type" />
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiUploadForm from '@/components/KqiUploadForm'
import { mapActions, mapGetters } from 'vuex'
//import { createNamespacedHelpers } from 'vuex'
//const { mapGetters, mapActions } = createNamespacedHelpers('aquariumDataSet')

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiUploadForm,
  },

  data() {
    return {
      submitText: '新規登録',
      type: 'Data',
      fileNumLimit: 10000,
      form: {
        name: '',
        datalist: [],
      },
      title: '',
      isCreateDialog: false,
      isCopyCreation: false,
      isLocked: false,
      dialogVisible: true,
      error: null,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
    }
  },
  computed: {
    ...mapGetters({
      registries: ['aquariumDataSet/detail'],
      uploadedFiles: ['data/uploadedFiles'],
    }),
  },

  watch: {
    async $route() {
      // 通常の作成とコピー作成が同一コンポーネントのため、コピー作成の実行はrouterの変化により検知する
      await this.initialize()
    },
  },

  async created() {
    await this.initialize()
  },

  methods: {
    ...mapActions([
      'aquariumDataSet/post',
      'aquariumDataSet/postByIdVersions',
      'data/post',
      'data/put',
      'data/putFile',
      'dataSet/post',
      'data/uploadedFiles',
    ]),

    async deleteAttachedFile(fileId) {
      try {
        await this.deleteFile({
          id: this.id,
          fileId: fileId,
        })
        this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[3] // ["", "dataset", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = '新しいデータセットの作成'
          this.isCreateDialog = true
          this.isCopyCreation = this.id !== null
          this.isLocked = false
          break
      }
    },

    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          if (
            this.$refs.uploadForm._data.selectedFiles == null ||
            this.$refs.uploadForm._data.selectedFiles.length == 0
          ) {
            this.error = new Error('ファイルを選択してください')
            return
          }
          try {
            await this.postDataSet()
            this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    // ファイルリストの×を押下した時
    handleRemove: function(file, fileList) {
      this.form.datalist = fileList
    },
    // ファイルを追加した時
    handleAdd: function(file, fileList) {
      this.form.datalist = fileList
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
      let dataset = null

      //ローカルからのデータリストを登録する
      let dataId = await this.uploadFile(this.form.name)

      //カモノハシのデータセットを登録する
      let datasetparams = {
        entries: {},
        flatEntries: [{ id: dataId }],
        isFlat: true,
        name: 'aquqrium_' + this.form.name,
        memo: '',
      }

      dataset = await this['dataSet/post'](datasetparams)

      //アクアリウムデータセットを登録する
      let aquariumDataSetparams = {
        name: this.form.name,
      }
      let aqDataset = await this['aquariumDataSet/post'](aquariumDataSetparams)

      //アクアリウムデータセットバージョンを登録する

      this['aquariumDataSet/postByIdVersions']({
        id: aqDataset.data.id,
        model: { datasetId: dataset.data.id },
      })
    },

    async deleteDataSet() {
      try {
        await this.delete(this.id)
        this.$emit('done', 'delete')
      } catch (e) {
        this.error = e
      }
    },

    handleShowData(id) {
      this.$router.push(`/data/edit/${id}`)
    },
  },
}
</script>

<style lang="scss" scoped>
@media screen and (max-width: 1500px) {
  .dialog /deep/ .el-dialog {
    width: 750px;
  }
}

@media screen and (min-width: 1500px) {
  .dialog /deep/ .el-dialog {
    width: 1450px;
  }
}
.model-type-list {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: flex-start;
  align-content: flex-start;
  margin-bottom: 40px;
}

.model-type {
  border-radius: 10px;
  border: solid 1px #ebeef5;
  float: left;
  margin: 0px 20px 10px 0;
  position: relative;
  width: 300px;
  height: 300px;
}

.model-type-image {
  border-radius: 10px 10px 0px 0px;
  height: 150px;
  background-color: #aaa;
  color: #666;
  text-align: center;
  padding-top: 30px;
}
.model-type-label {
  padding: 20px;
}
.model-type-description {
  padding: 20px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.right-button-group {
  padding-top: 0px;
  text-align: right;
}
</style>
