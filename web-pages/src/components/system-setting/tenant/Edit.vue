<template>
  <el-dialog class="dialog"
             title="テナント編集"
             :visible="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">

    <el-form :model="form"
             :rules="rules"
             ref="createForm">
      <pl-display-error :error="error"/>

      <h3>テナント情報</h3>
      <div class="margin">
        <pl-display-text label="ID" :value="id"/>
        <pl-display-text label="テナント名" :value="form.name"/>
        <el-form-item label="表示名" prop="displayName">
          <el-input v-model="form.displayName"/>
        </el-form-item>
      </div>

      <h3>ストレージ情報</h3>
      <div class="margin">
        <el-form-item label="ストレージ" prop="storageId">
          <pl-storage-endpoint-selector v-model="form.storageId"/>
        </el-form-item>
      </div>

      <h3>Git情報</h3>
      <div class="margin">
        <pl-git-endpoint-selector v-model="form.gitIds"
                                  v-bind:defaultId="form.defaultGitId"
                                  v-on:changeDefaultId="form.defaultGitId = $event"/>
      </div>

      <h3>Docker Registry 情報</h3>
      <div class="margin">
        <pl-registry-endpoint-selector v-model="form.registryIds"
                                       v-bind:defaultId="form.defaultRegistryId"
                                       v-on:changeDefaultId="form.defaultRegistryId = $event"/>
      </div>

      <el-row :gutter="20" class="footer">
        <el-col :span="8">
          <pl-delete-button @delete="deleteTenant" v-bind:tenant-name="form.name"/>
        </el-col>
        <el-col class="right-button-group" :span="16">
          <el-button @click="closeDialog">キャンセル</el-button>
          <el-button type="primary" @click="saveData">保存</el-button>
        </el-col>
      </el-row>

    </el-form>

  </el-dialog>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'
  import GitEndpointSelector from '@/components/common/GitEndpointSelector.vue'
  import RegistryEndpointSelector from '@/components/common/RegistryEndpointSelector.vue'
  import StorageEndpointSelector from '@/components/system-setting/tenant/StorageEndpointSelector.vue'
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DeleteTenantButton from '@/components/system-setting/tenant/DeleteTenantButton.vue'

  export default {
    name: 'TenantEdit',
    components: {
      'pl-display-error': DisplayError,
      'pl-git-endpoint-selector': GitEndpointSelector,
      'pl-registry-endpoint-selector': RegistryEndpointSelector,
      'pl-storage-endpoint-selector': StorageEndpointSelector,
      'pl-display-text': DisplayTextForm,
      'pl-delete-button': DeleteTenantButton
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: null,

        form: {
          name: '',
          displayName: '',
          gitIds: [],
          defaultGitId: null,
          registryIds: [],
          defaultRegistryId: null,
          storageId: null
        },

        rules: {
          displayName: [{required: true, trigger: 'blur', message: '必須項目です'}],
          gitIds: [{required: true, trigger: 'blur', message: '必須項目です'}],
          defaultGitId: [{required: true, trigger: 'blur', message: '必須項目です'}],
          registryIds: [{required: true, trigger: 'blur', message: '必須項目です'}],
          defaultRegistryId: [{required: true, trigger: 'blur', message: '必須項目です'}],
          storageId: [{required: true, trigger: 'blur', message: '必須項目です'}]
        }
      }
    },
    async created () {
      await this.changeId()
    },
    watch: {
      async id () {
        await this.changeId()
      }
    },
    methods: {
      async changeId () {
        if (this.id) {
          try {
            let params = {
              id: this.id
            }
            let [model] = api.f.data(await api.tenant.admin.getById(params))
            this.form.name = model.name
            this.form.displayName = model.displayName
            this.form.gitIds = model.gitIds
            this.form.defaultGitId = model.defaultGitId
            this.form.storageId = model.storageId
            this.form.defaultRegistryId = model.defaultRegistryId
            this.form.registryIds = model.registryIds
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      },
      async saveData () {
        let form = this.$refs.createForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.putTenant()
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async putTenant () {
        let param = {
          id: this.id,
          model: {
            displayName: this.form.displayName,
            gitIds: this.form.gitIds,
            defaultGitId: this.form.defaultGitId,
            storageId: this.form.storageId,
            defaultRegistryId: this.form.defaultRegistryId,
            registryIds: this.form.registryIds
          }
        }
        await api.tenant.admin.put(param)
      },
      async deleteTenant () {
        try {
          let params = {
            id: this.id
          }
          let msg = (await api.tenant.admin.delete(params)).data.containerWarnMsg
          if (msg) {
            // コンテナ起動に失敗した場合、警告メッセージを表示する
            this.emitError(msg)
          } else {
            this.emitDone()
          }
        } catch (e) {
          this.error = e
        }
      },
      closeDialog () {
        this.emitCancel()
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.$emit('done')
      },
      emitError (msg) {
        this.$emit('error', msg)
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

  .margin {
    padding-left: 30px;
  }

  .footer {
    padding-top: 40px;
  }
</style>
