<template>
  <el-dialog class="dialog"
             title="データ編集"
             :visible.sync="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">
    <el-row type="flex" justify="end">
      <el-col :span="24" class="right-button-group">
        <el-button @click="openPreprocessingDialog">前処理実行</el-button>
      </el-col>
    </el-row>

    <!-- 薄い網掛けを表示する。ローディングバーは表示しないのでloading-spinnerはスペース -->
    <el-form element-loading-spinner=" "
             v-loading="loading"
             element-loading-background="rgba(255, 255, 255, 0.7)">
      <pl-display-error :error="error"/>
      <pl-display-text label="ID" :value="id"/>
      <el-form-item label="データ名" prop="name">
        <el-input v-model="name"/>
      </el-form-item>
      <el-form-item label="タグ">
        <pl-tag-editor v-model="tags"/>
      </el-form-item>
      <el-form-item label="メモ">
        <el-input type="textarea" v-model="memo"/>
      </el-form-item>
      <pl-display-text label="登録者" :value="createdBy"/>
      <pl-display-text label="登録日時" :value="createdAt"/>

      <el-form-item label="ファイル一覧">
        <br/>
        <div v-if="allFiles.length  === 0">
        </div>
        <div v-else-if="allFiles.length === 1">
          <pl-file-manager :uploadedFiles="allFiles"
                           type="Data"
                           @delete="deleteFile"
                           :deletable="true"/>
        </div>
        <div v-else>
          <el-button type="primary" @click="toggleFeatures" class="toggle-features">{{ featuresOpen ? 'Hide Files' :
            'View All Files'
            }}
          </el-button>
          <div v-if="featuresOpen" class="features">
            <pl-file-manager :uploadedFiles="allFiles"
                             type="Data"
                             @delete="deleteFile"
                             :deletable="true"/>
          </div>
        </div>
        <pl-file-manager ref="uploadFile" type="Data"/>

      </el-form-item>

      <el-row :gutter="20" class="footer">
        <el-col :span="8">
          <pl-delete-button @delete="deleteData"/>
        </el-col>
        <el-col class="right-button-group" :span="16">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="handleSubmit">保存</el-button>
        </el-col>
      </el-row>
    </el-form>

  </el-dialog>
</template>

<script>
  import FileManager from '@/components/common/FileManager.vue'
  import TagEditor from '@/components/data/TagEditor.vue'
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import DisplayError from '@/components/common/DisplayError'
  import api from '@/api/v1/api'

  export default {
    name: 'DataEdit',
    components: {
      'pl-file-manager': FileManager,
      'pl-tag-editor': TagEditor,
      'pl-display-text': DisplayTextForm,
      'pl-display-error': DisplayError,
      'pl-delete-button': DeleteButton
    },
    props: {
      id: String
    },
    data () {
      return {
        allFiles: [],
        memo: undefined,
        createdBy: undefined,
        createdAt: undefined,
        name: undefined,
        tags: undefined,
        dialogVisible: true,
        error: undefined,
        loading: false,
        featuresOpen: false
      }
    },
    async created () {
      await this.retrieveData()
      this.dialogVisible = true
    },
    methods: {
      toggleFeatures () {
        this.featuresOpen = !this.featuresOpen
      },
      async updateData () {
        // 独自ローディング処理のため共通側は無効
        this.$store.commit('setLoading', false)
        this.loading = true
        let params = {
          id: this.id,
          model: {
            name: this.name,
            memo: this.memo,
            tags: this.tags
          }
        }
        await api.data.putById(params)
        // 共通側ローディングを再度有効化
        this.loading = false
        this.$store.commit('setLoading', true)
      },
      async uploadFile () {
        // 独自ローディング処理のため共通側は無効
        this.$store.commit('setLoading', false)
        this.loading = true

        let uploader = this.$refs.uploadFile
        let allFilesInfo = await uploader.uploadFile()

        if (allFilesInfo !== undefined) {
          for (let i = 0; i < allFilesInfo.length; i++) {
            allFilesInfo[i].FileName = allFilesInfo[i].name
            await api.data.putFilesById({id: this.id, model: allFilesInfo[i]})
          }
        }

        // 共通側ローディングを再度有効化
        this.loading = false
        this.$store.commit('setLoading', true)
      },
      async handleSubmit () {
        try {
          await this.updateData()
          await this.uploadFile()
          this.emitDone()
          this.error = null
        } catch (error) {
          this.$notify.error({
            title: error.message,
            message: 'ファイルアップロードに失敗しました',
            duration: 0
          })
          this.error = error
          // 共通側ローディングを再度有効化
          this.loading = false
          this.$store.commit('setLoading', true)
        }
      },
      async retrieveFiles () {
        let param = {
          id: this.id,
          withUrl: true
        }
        let result = (await api.data.getFilesById(param)).data
        this.allFiles = result
      },
      async retrieveData () {
        let result = (await api.data.getById({id: this.id})).data
        this.attribute = result.attribute
        this.memo = result.memo
        this.createdBy = result.createdBy
        this.createdAt = result.createdAt
        this.name = result.name
        this.tags = result.tags
        this.featuresOpen = false

        this.retrieveFiles()
      },
      async deleteData () {
        try {
          await api.data.deleteById({id: this.id})
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async deleteFile (fileId) {
        try {
          await api.data.deleteFilesById({id: this.id, fileId: fileId})
          this.retrieveData()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
        this.dialogVisible = false
      },
      emitCancel () {
        this.$emit('cancel')
      },
      openPreprocessingDialog () {
        this.$router.push('/data/' + this.id + '/preprocessing')
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      }
    }
  }
</script>

<style lang="scss" scoped>
  .right-button-group {
    text-align: right;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .footer {
    padding-top: 40px;
  }

</style>
