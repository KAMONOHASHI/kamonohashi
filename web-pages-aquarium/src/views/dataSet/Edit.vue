<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    :submit-text="submitText"
    @submit="submit"
    @delete="deleteDataSet"
    @close="$emit('cancel')"
  >
    <el-form
      ref="createForm"
      v-loading="loading"
      :model="form"
      :rules="rules"
      element-loading-spinner=" "
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <kqi-display-error :error="error" />
      <el-row>
        <el-form-item label="データセット名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
      </el-row>

      <el-row>
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
                    <el-checkbox :label="item.id">{{ item.name }}</el-checkbox>
                  </div>
                </el-checkbox-group>
              </div>
              <kqi-pagination
                v-model="dataPageStatus"
                :total="dataTotal"
                @change="initialize"
              />
            </el-row>
          </div>
        </div>
      </el-row>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiUploadForm from '@/components/KqiUploadForm'
import { mapActions, mapGetters } from 'vuex'
import KqiPagination from '@/components/KqiPagination'
//import { createNamespacedHelpers } from 'vuex'
//const { mapGetters, mapActions } = createNamespacedHelpers('aquariumDataSet')

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiUploadForm,
    KqiPagination,
  },

  data() {
    return {
      importfile: null,
      submitText: '新規登録',
      type: 'Data',
      loading: false,
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
      searchCondition: {},
      dataPageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      checkList: [],
      datas: [],
    }
  },
  computed: {
    ...mapGetters({
      registries: ['aquariumDataSet/detail'],
      uploadedFiles: ['data/uploadedFiles'],
      dataTotal: ['data/total'],
      allDatas: ['data/data'],
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
      'data/fetchData',
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
      let params = this.searchCondition
      params.page = this.dataPageStatus.currentPage
      params.perPage = this.dataPageStatus.currentPageSize
      params.withTotal = true
      await this['data/fetchData'](params)
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
      if (this.importfile == 1) {
        await form.validate(async valid => {
          if (valid) {
            this.$store.commit('setLoading', false)
            this.loading = true
            if (
              this.$refs.uploadForm._data.selectedFiles == null ||
              this.$refs.uploadForm._data.selectedFiles.length == 0
            ) {
              this.error = new Error('ファイルを選択してください')
              this.loading = false
              this.$store.commit('setLoading', true)
              return
            }
            try {
              await this.postDataSet()
              this.$emit('done')
              this.error = null
            } catch (e) {
              this.error = e
            } finally {
              // 共通側ローディングを再度有効化
              this.loading = false
              this.$store.commit('setLoading', true)
            }
          }
        })
      } else if (this.importfile == 2) {
        await form.validate(async valid => {
          if (valid) {
            this.$store.commit('setLoading', false)
            this.loading = true
            await this.postDataSet()
            this.$emit('done')
            this.error = null
            this.loading = false
            this.$store.commit('setLoading', true)
          }
        })
      }
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
      if (this.importfile == 1) {
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
      } else if (this.importfile == 2) {
        //カモノハシのデータを登録する
        let flatEntry = []
        if (this.checkList.length == 0) {
          throw new Error('データを選択してください')
        }
        for (let i in this.checkList) {
          flatEntry.push({ id: this.checkList[i] })
        }
        let datasetparams = {
          entries: {},
          flatEntries: flatEntry,
          isFlat: true,
          name: 'aquqrium_' + this.form.name,
          memo: '',
        }
        dataset = await this['dataSet/post'](datasetparams)
      }

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

.right-button-group {
  padding-top: 0px;
  text-align: right;
}
.dialog /deep/ label {
  font-weight: 500 !important;
}
</style>
