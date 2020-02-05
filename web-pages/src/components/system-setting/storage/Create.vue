<template>
  <el-dialog
    class="dialog"
    title="ストレージ登録"
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

      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="createStorage">登録</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import api from '@/api/v1/api'
import DisplayError from '@/components/common/DisplayError'

export default {
  name: 'RegistryCreate',
  components: {
    'pl-display-error': DisplayError,
  },
  props: {
    serviceTypes: Array, // /api/v1/admin/registry/types の結果
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
  methods: {
    async createStorage() {
      let form = this.$refs.createForm

      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              model: {
                name: this.name,
                serverUrl: this.serverUrl,
                accessKey: this.accessKey,
                secretKey: this.secretKey,
                nfsServer: this.nfsServer,
                nfsRoot: this.nfsRoot,
              },
            }
            await api.storage.admin.post(params)
            this.emitDone()
            this.error = undefined
          } catch (e) {
            this.error = e
          }
        }
      })
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
