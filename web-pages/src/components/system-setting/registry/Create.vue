<template>
  <el-dialog class="dialog"
             title="Dockerレジストリ登録"
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
              <el-input-number v-model="portNo" :min="1" :max="65535"
                               controls-position="right" style="width:100%;"/>
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
            <el-input v-model="projectName" placeholder="ユーザ名/リポジトリ名 or グループ名/リポジトリ名 "></el-input>
          </el-form-item>
        </div>
        <div v-else></div>
      </div>

      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="createRegistry">登録</el-button>
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
      'pl-display-error': DisplayError
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
    },
    methods: {
      async createRegistry () {
        let form = this.$refs.createForm

        await form.validate(async (valid) => {
            if (valid) {
              try {
                let params = {
                  model: {
                    name: this.name,
                    host: this.host,
                    portNo: this.portNo,
                    serviceType: this.serviceType,
                    projectName: this.projectName,
                    apiUrl: this.apiUrl,
                    registryUrl: this.registryUrl
                  }
                }
                await api.registry.admin.post(params)
                this.emitDone()
                this.error = undefined
              } catch (e) {
                this.error = e
              }
            }
          }
        )
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
  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }

</style>
