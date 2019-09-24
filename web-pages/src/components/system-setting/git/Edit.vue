<template>
  <el-dialog class="dialog"
             title="Git編集"
             :visible.sync="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">
    <el-form ref="createForm" :model="this" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="名前" prop="name">
        <el-input v-model="name" :disabled="isNotEditable"/>
      </el-form-item>
      <el-form-item label="Git種別" prop="serviceType">
        <el-select v-model="serviceType" style="width: 100%" :disabled="isNotEditable">
          <el-option v-for="(t, idx) in types" :key="idx" :label="t.name" :value="t.id"/>
        </el-select>
      </el-form-item>
      <el-form-item label="リポジトリURL" prop="repositoryUrl">
        <el-input v-model="repositoryUrl" :disabled="isNotEditable" @change="handleChange"/>
      </el-form-item>
      <el-form-item label="API URL" prop="apiUrl">
        <el-switch v-model="editApiUrl"
                   :disabled="isNotEditable"/>
        <el-input v-model="apiUrl" :disabled="isNotEditable || !editApiUrl"/>
      </el-form-item>

      <el-row :gutter="20" class="footer">
        <el-col :span="12">
          <pl-delete-button @delete="deleteGit" :disabled="isNotEditable"/>
        </el-col>
        <el-col class="right-button-group" :span="12">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="updateGit" :disabled="isNotEditable">保存</el-button>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>
<script>
  import api from '@/api/v1/api'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'GitEdit',
    components: {
      'pl-delete-button': DeleteButton,
      'pl-display-error': DisplayError
    },
    props: {
      id: {
        type: String,
        defalut: ''
      }
    },
    data () {
      return {
        dialogVisible: true,
        error: undefined,
        name: undefined,
        repositoryUrl: undefined,
        serviceType: undefined,
        apiUrl: undefined,
        editApiUrl: false,
        isNotEditable: false,
        rules: {
          name: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }],
          repositoryUrl: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }],
          serviceType: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }],
          token: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }],
          apiUrl: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }]
        },
        types: []
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        try {
          this.types = (await api.git.admin.getTypes()).data
          let result = (await api.git.admin.getById({id: this.id})).data
          this.name = result.name
          this.repositoryUrl = result.repositoryUrl
          this.serviceType = result.serviceType
          this.apiUrl = result.apiUrl
          this.isNotEditable = result.isNotEditable
          this.editApiUrl = result.repositoryUrl !== result.apiUrl
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async updateGit () {
        let form = this.$refs.createForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              let params = {
                id: this.id,
                model: {
                  name: this.name,
                  repositoryUrl: this.repositoryUrl,
                  serviceType: this.serviceType,
                  apiUrl: this.apiUrl
                }
              }
              await api.git.admin.putEndpoint(params)
              this.emitDone()
              this.error = undefined
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async deleteGit () {
        try {
          await api.git.admin.deleteById({id: this.id})
          this.emitDone()
          this.error = undefined
        } catch (e) {
          this.error = e
        }
      },
      handleChange () {
        if (!this.editApiUrl) {
          this.apiUrl = this.repositoryUrl
        }
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.$emit('done')
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
