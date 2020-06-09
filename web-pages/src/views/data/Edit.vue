<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteData"
    @close="$emit('cancel')"
  >
    <el-row
      v-if="isEditDialog && $store.getters['account/isAvailablePreprocessing']"
      type="flex"
      justify="end"
    >
      <el-col :span="24" class="right-button-group">
        <el-button @click="openPreprocessingDialog">前処理実行</el-button>
      </el-col>
    </el-row>

    <!-- 薄い網掛けを表示する。ローディングバーは表示しないのでloading-spinnerはスペース -->
    <el-form
      ref="createForm"
      v-loading="loading"
      :model="form"
      :rules="rules"
      element-loading-spinner=" "
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <kqi-display-error :error="error" />
      <kqi-display-text-form
        v-if="isEditDialog"
        label="ID"
        :value="detail ? String(detail.id) : '0'"
      />
      <el-form-item label="データ名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="タグ">
        <kqi-tag-editor v-model="form.tags" :registered-tags="tenantTags" />
      </el-form-item>
      <el-form-item label="メモ">
        <el-input v-model="form.memo" type="textarea" />
      </el-form-item>
      <kqi-display-text-form
        v-if="isEditDialog"
        label="登録者"
        :value="detail.createdBy"
      />
      <kqi-display-text-form
        v-if="isEditDialog"
        label="登録日時"
        :value="detail.createdAt"
      />
      <el-form-item label="データファイル" prop="files">
        <div v-if="uploadedFiles.length >= 2">
          <el-button type="primary" @click="viewAllFiles = !viewAllFiles">
            {{ viewAllFiles ? 'Hide Files' : 'View All Files' }}
          </el-button>
        </div>
        <div v-if="uploadedFiles.length === 1 || viewAllFiles">
          <kqi-file-manager
            :uploaded-files="uploadedFiles"
            type="Data"
            :deletable="true"
            @delete="deleteAttachedFile"
          />
        </div>
        <kqi-file-manager ref="dataFile" type="Data" :deletable="false" />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiFileManager from '@/components/KqiFileManager'
import KqiTagEditor from '@/components/KqiTagEditor'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers('data')

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiFileManager,
    KqiTagEditor,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    let validateFiles = (rule, value, callback) => {
      // アップロードするファイルが存在しない場合はエラーを出す。
      if (
        this.$refs.dataFile.isFileSelected() ||
        this.uploadedFiles.length >= 1
      ) {
        callback()
      } else {
        callback(new Error('ファイルを1つ以上選択してください'))
      }
    }
    return {
      form: {
        name: null,
        memo: null,
        tags: [],
      },
      isEditDialog: false,
      viewAllFiles: false,
      title: '',
      result: [],
      dialogVisible: true,
      error: null,
      loading: false,
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
  computed: {
    ...mapGetters(['tenantTags', 'detail', 'uploadedFiles']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'データ登録'
      this.clearUploadedFiles()
    } else {
      this.title = 'データ編集'
      this.isEditDialog = true
      await this.retrieveData()
    }
    await this.fetchTenantTags()
  },
  methods: {
    ...mapMutations(['clearUploadedFiles']),
    ...mapActions([
      'fetchDetail',
      'fetchTenantTags',
      'fetchUploadedFiles',
      'put',
      'post',
      'putFile',
      'delete',
      'deleteFile',
    ]),
    async retrieveData() {
      try {
        await this.fetchDetail(this.id)
        await this.fetchUploadedFiles(this.id)
        this.form.name = this.detail.name
        this.form.tags = this.detail.tags
        this.form.memo = this.detail.memo
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async submit() {
      let form = this.$refs.createForm

      await form.validate(async valid => {
        if (valid) {
          // 独自ローディング処理のため共通側は無効
          this.$store.commit('setLoading', false)
          this.loading = true

          let dataId = null
          try {
            dataId = await this.updateData()
            await this.uploadFile(dataId)
            this.$emit('done')
            this.error = null
          } catch (error) {
            try {
              // データIDが存在する場合、該当のデータを削除する
              if (dataId !== null) {
                await this.delete(dataId)
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

    async updateData() {
      let model = {
        name: this.form.name,
        memo: this.form.memo,
        tags: this.form.tags,
        isRaw: true,
      }

      let result = null
      if (this.id === null) {
        result = (await this.post(model)).data
      } else {
        result = (
          await this.put({
            id: this.id,
            model: model,
          })
        ).data
      }
      return result.id
    },

    async uploadFile(dataId) {
      let dataFileInfo = await this.$refs.dataFile.uploadFile()
      if (dataFileInfo !== undefined) {
        this.putFile({ id: dataId, fileInfo: dataFileInfo })
      }
    },

    async deleteData() {
      try {
        await this.delete(this.id)
        this.error = null
        this.$emit('done', 'delete')
      } catch (e) {
        this.error = e
      }
    },

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

    openPreprocessingDialog() {
      this.$router.push('/data/' + this.id + '/preprocessing')
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
