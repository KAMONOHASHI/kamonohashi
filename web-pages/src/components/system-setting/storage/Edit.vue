<template>
  <el-dialog
    class="dialog"
    title="ストレージ編集"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <el-form ref="createForm" :model="this" :rules="rules">
      <pl-display-error :error="error" />
      <el-form-item label="ストレージ名" prop="name">
        <el-input v-model="name" />
      </el-form-item>

      <h3>ストレージ情報</h3>
      <div style="padding-left: 30px; padding-right: 10px;">
        <el-form-item
          label="サーバ名URL（例: kamonohashi.ai:9000）"
          prop="serverUrl"
        >
          <el-input v-model="serverUrl" />
        </el-form-item>
        <el-form-item label="アクセスキー" prop="accessKey">
          <el-input v-model="accessKey" />
        </el-form-item>
        <el-form-item label="シークレットキー" prop="secretKey">
          <el-input v-model="secretKey" type="password" />
        </el-form-item>
        <el-form-item label="NFSサーバ" prop="nfsServer">
          <el-input v-model="nfsServer" />
        </el-form-item>
        <el-form-item label="NFS共有ルートディレクトリ" prop="nfsRoot">
          <el-input v-model="nfsRoot" />
        </el-form-item>
      </div>

      <el-row :gutter="20" class="footer">
        <el-col :span="12">
          <pl-delete-button @delete="deleteStorage" />
        </el-col>
        <el-col class="right-button-group" :span="12">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="updateStorage">保存</el-button>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import DeleteButton from '@/components/common/DeleteButton.vue'
import DisplayError from '@/components/common/DisplayError'
import api from '@/api/v1/api'

export default {
  name: 'StorageEdit',
  components: {
    'pl-delete-button': DeleteButton,
    'pl-display-error': DisplayError,
  },
  props: {
    id: String,
  },
  data() {
    return {
      dialogVisible: true,
      error: undefined,

      name: undefined,
      serverUrl: undefined,
      accessKey: undefined,
      secretKey: undefined,
      nfsServer: undefined,
      nfsRoot: undefined,
      rules: {
        name: [{ required: true, message: '必須項目です' }],
        serverUrl: [{ required: true, message: '必須項目です' }],
        accessKey: [{ required: true, message: '必須項目です' }],
        secretKey: [{ required: true, message: '必須項目です' }],
        nfsServer: [{ required: true, message: '必須項目です' }],
        nfsRoot: [{ required: true, message: '必須項目です' }],
      },
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async updateStorage() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              model: {
                name: this.name,
                serverUrl: this.serverUrl,
                accessKey: this.accessKey,
                secretKey: this.secretKey,
                nfsServer: this.nfsServer,
                nfsRoot: this.nfsRoot,
              },
            }
            await api.storage.admin.put(params)
            this.emitDone()
            this.error = undefined
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async retrieveData() {
      let result = (await api.storage.admin.getById({ id: this.id })).data
      this.name = result.name
      this.serverUrl = result.serverUrl
      this.accessKey = result.accessKey
      this.secretKey = result.secretKey
      this.nfsServer = result.nfsServer
      this.nfsRoot = result.nfsRoot
    },
    async deleteStorage() {
      try {
        await api.storage.admin.delete({ id: this.id })
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },
    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
      this.dialogVisible = false
    },
    emitCancel() {
      this.$emit('cancel')
    },
    closeDialog(done) {
      done()
      this.emitCancel()
    },
  },
}
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.footer {
  padding-top: 40px;
}
</style>
