<template>
  <el-dialog
    class="dialog"
    title="データ登録"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <!-- 薄い網掛けを表示する。ローディングバーは表示しないのでloading-spinnerはスペース -->
    <el-form
      ref="createForm"
      v-loading="loading"
      :model="this"
      :rules="rules"
      element-loading-spinner=" "
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <pl-display-error :error="error" />
      <el-form-item label="データ名" prop="name">
        <el-input v-model="name" />
      </el-form-item>
      <el-form-item label="タグ">
        <pl-tag-editor v-model="tags" />
      </el-form-item>
      <el-form-item label="メモ">
        <el-input v-model="memo" type="textarea" />
      </el-form-item>
      <el-form-item label="データファイル" prop="files">
        <pl-file-manager ref="dataFile" type="Data" :deletable="false" />
      </el-form-item>

      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="createData">登録</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>
<script>
import FileManager from '@/components/common/FileManager.vue'
import TagEditor from '@/components/data/TagEditor.vue'
import DisplayError from '@/components/common/DisplayError'
import api from '@/api/v1/api'

export default {
  name: 'DataCreate',
  components: {
    'pl-file-manager': FileManager,
    'pl-display-error': DisplayError,
    'pl-tag-editor': TagEditor,
  },
  data() {
    let validateFiles = (rule, value, callback) => {
      // アップロードするファイルが存在しない場合はエラーを出す。
      if (this.$refs.dataFile.isFileSelected()) {
        callback()
      } else {
        callback(new Error('ファイルを1つ以上選択してください'))
      }
    }
    return {
      result: [],
      dialogVisible: true,
      dataId: undefined,
      error: undefined,
      inputVisible: false,
      inputValue: '',
      memo: undefined,
      createdBy: undefined,
      createdAt: undefined,
      name: undefined,
      loading: false,
      tags: [],
      files: [],
      rules: {
        name: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
        files: [
          {
            validator: validateFiles,
            trigger: 'blur',
          },
        ],
      },
    }
  },
  methods: {
    async createData() {
      let form = this.$refs.createForm

      await form.validate(async valid => {
        if (valid) {
          // 独自ローディング処理のため共通側は無効
          this.$store.commit('setLoading', false)
          this.loading = true

          try {
            await this.registerData()
            await this.uploadFile()
            this.emitDone()
            this.error = null
          } catch (error) {
            try {
              // データIDが存在する場合、該当のデータを削除する
              if (this.dataId !== undefined) {
                await api.data.deleteById({ id: this.dataId })
              }
            } finally {
              this.$notify.error({
                title: error.message,
                message: 'データ登録に失敗しました',
                duration: 0,
              })
              this.error = error
              // 選択したファイルを削除する
              this.$refs.dataFile.$refs.uploadForm.selectedFiles = undefined
              this.$refs.dataFile.$refs.uploadForm.filesArray = []
            }
          } finally {
            // 共通側ローディングを再度有効化
            this.loading = false
            this.$store.commit('setLoading', true)
          }
        }
      })
    },

    async registerData() {
      let model = {
        name: this.name,
        memo: this.memo,
        tags: this.tags,
        isRaw: true,
      }

      let result = (await api.data.post({ model: model })).data
      this.dataId = result.id
    },

    async uploadFile() {
      let dataFileInfo = await this.$refs.dataFile.uploadFile()
      if (dataFileInfo !== undefined) {
        for (let i = 0; i < dataFileInfo.length; i++) {
          dataFileInfo[i].FileName = dataFileInfo[i].name
          await api.data.putFilesById({
            id: this.dataId,
            model: dataFileInfo[i],
          })
        }
      }
    },

    closeDialog(done) {
      done()
      this.emitCancel()
    },
    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },
    handleInputConfirm() {
      let inputValue = this.inputValue
      if (inputValue) {
        this.tags.push(inputValue)
      }
      this.inputVisible = false
      this.inputValue = ''
    },
    handleClose(tag) {
      this.tags.splice(this.tags.indexOf(tag), 1)
    },
  },
}
</script>

<style lang="scss" scoped>
.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}
</style>
