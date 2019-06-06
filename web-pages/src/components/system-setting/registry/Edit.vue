<template>
  <el-dialog class="dialog"
             title="Dockerレジストリ編集"
             :visible.sync="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">
    <el-form ref="createForm" :model="this" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="レジストリ名" prop="name">
        <el-input v-model="name"/>
      </el-form-item>

      <h3>レジストリ情報</h3>
      <div style="padding-left: 30px; padding-right: 10px;">
        <el-form-item label="種別" prop="serviceType">
          <el-select v-model="serviceType" style="width: 100%;">
            <el-option
              v-for="service in serviceTypes"
              :key="service.id"
              :label="service.name"
              :value="service.id"/>
          </el-select>
        </el-form-item>
        <el-row>
          <el-col :span="16">
            <el-form-item label="ホスト名" prop="host">
              <el-input v-model="host"/>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="ポート" prop="portNo">
              <el-input-number v-model="portNo" :min="1" :max="65535" controls-position="right" style="width: 100%;"/>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="API URL" prop="apiUrl">
          <el-input v-model="apiUrl"/>
        </el-form-item>
        <el-form-item label="URL" prop="registryUrl">
          <el-input v-model="registryUrl"/>
        </el-form-item>
        <div v-if="serviceType === 2">
          <el-form-item label="プロジェクト名" prop="projectName">
            <el-input v-model="projectName"/>
          </el-form-item>
        </div>
        <div v-else></div>

      </div>

      <el-row :gutter="20" class="footer">
        <el-col :span="12">
          <pl-delete-button @delete="deleteRegistry"/>
        </el-col>
        <el-col class="right-button-group" :span="12">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="updateRegistry">保存</el-button>
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
    name: 'RegistryEdit',
    components: {
      'pl-delete-button': DeleteButton,
      'pl-display-error': DisplayError
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: undefined,
        name: undefined,
        host: undefined,
        portNo: undefined,
        projectName: undefined,
        serviceType: undefined,
        registryUrl: undefined,
        apiUrl: undefined,
        serviceTypes: Array, // /api/v1/admin/registry/types の結果
        rules: {
          name: [{required: true, message: '必須項目です'}],
          host: [{required: true, message: '必須項目です'}],
          apiUrl: [{required: true, message: '必須項目です'}],
          registryUrl: [{required: true, message: '必須項目です'}],
          portNo: [{required: true, message: '必須項目です'}],
          serviceType: [{required: true, message: '必須項目です'}],
          projectName: [{required: true, message: '必須項目です'}]
        }
      }
    },
    async created () {
      this.serviceTypes = (await api.registry.admin.getType()).data
      await this.retrieveData()
    },
    methods: {
      async updateRegistry () {
        let form = this.$refs.createForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              let params = {
                id: this.id,
                model: {
                  name: this.name,
                  host: this.host,
                  portNo: this.portNo,
                  serviceType: this.serviceType,
                  projectName: this.projectName,
                  registryUrl: this.registryUrl,
                  apiUrl: this.apiUrl
                }
              }
              await api.registry.admin.putById(params)
              this.emitDone()
              this.error = undefined
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async retrieveData () {
        let result = (await api.registry.admin.getById({id: this.id})).data
        this.name = result.name
        this.host = result.host
        this.portNo = result.portNo
        this.password = result.password
        this.registryUrl = result.registryUrl
        this.apiUrl = result.apiUrl
        this.projectName = result.projectName
        this.serviceType = result.serviceType
      },
      async deleteRegistry () {
        try {
          await api.registry.admin.deleteById({id: this.id})
          this.emitDone()
        } catch (e) {
          this.error = e
        }
      },
      emitDone () {
        this.$emit('done')
        this.dialogVisible = false
      },
      emitCancel () {
        this.$emit('cancel')
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
