<template>
  <el-dialog
    class="dialog"
    title="Git登録"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <el-form ref="createForm" :model="this" :rules="rules">
      <pl-display-error :error="error" />
      <el-form-item label="名前" prop="name">
        <el-input v-model="name" />
      </el-form-item>
      <el-form-item label="Git種別" prop="serviceType">
        <el-select v-model="serviceType" style="width: 100%">
          <el-option
            v-for="(t, idx) in types"
            :key="idx"
            :label="t.name"
            :value="t.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="リポジトリURL" prop="repositoryUrl">
        <el-input v-model="repositoryUrl" @change="handleChange" />
      </el-form-item>
      <el-form-item label="API URL" prop="apiUrl">
        <el-switch v-model="editApiUrl" />
        <el-input v-model="apiUrl" :disabled="!editApiUrl" />
      </el-form-item>

      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="createGit">登録</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>
<script>
import DisplayError from '@/components/common/DisplayError'
import api from '@/api/v1/api'

export default {
  name: 'GitCreate',
  components: {
    'pl-display-error': DisplayError,
  },
  data() {
    return {
      dialogVisible: true,
      error: undefined,
      name: undefined,
      repositoryUrl: undefined,
      serviceType: undefined,
      apiUrl: undefined,
      editApiUrl: false,
      rules: {
        name: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
        repositoryUrl: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
        serviceType: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
        apiUrl: [
          {
            required: true,
            trigger: 'blur',
            message: '必須項目です',
          },
        ],
      },
      types: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      try {
        this.types = (await api.git.admin.getTypes()).data
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async createGit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              model: {
                name: this.name,
                repositoryUrl: this.repositoryUrl,
                serviceType: this.serviceType,
                apiUrl: this.apiUrl,
              },
            }
            await api.git.admin.postEndpoint(params)
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    handleChange() {
      if (!this.editApiUrl) {
        this.apiUrl = this.repositoryUrl
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
