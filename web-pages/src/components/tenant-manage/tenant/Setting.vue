<template>
  <div>
    <h2> 接続テナント設定 </h2>
    <el-card>
      <el-form :model="form"
                :rules="rules"
                ref="editForm"
                class="parent-container">
        <pl-display-error :error="error"/>

        <el-col class="container base">
          <h3>テナント情報</h3>
          <div class="margin">
              <pl-display-text label="ID" :value="form.id"/>
              <pl-display-text label="テナント名" :value="form.name"/>
              <el-form-item label="表示名" prop="displayName">
                <el-input v-model="form.displayName"/>
              </el-form-item>
          </div>
        </el-col>

        <el-col class="container detail">
          <h3>Git情報</h3>
          <div class="margin">
              <pl-git-endpoint-selector v-model="form.gitIds"
                                      v-bind:defaultId="form.defaultGitId"
                                      v-bind:tenantId="form.id"
                                      v-on:changeDefaultId="form.defaultGitId = $event"/>
          </div>

          <h3>Docker Registry 情報</h3>
          <div class="margin">
              <pl-registry-endpoint-selector v-model="form.registryIds"
                                          v-bind:defaultId="form.defaultRegistryId"
                                          v-bind:tenantId="form.id"
                                          v-on:changeDefaultId="form.defaultRegistryId = $event"/>
          </div>

          <el-row :gutter="20">
            <el-col class="right-button-group">
              <el-button @click="saveData" type="primary">
              保存
              </el-button>
            </el-col>
          </el-row>
        </el-col>
      </el-form>
    </el-card>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import GitEndpointSelector from '@/components/tenant-manage/tenant/GitEndpointSelector.vue'
  import RegistryEndpointSelector from '@/components/tenant-manage/tenant/RegistryEndpointSelector.vue'

  export default {
    name: 'TenantSetting',
    title: '接続テナント設定',
    components: {
      'pl-display-error': DisplayError,
      'pl-display-text': DisplayTextForm,
      'pl-git-endpoint-selector': GitEndpointSelector,
      'pl-registry-endpoint-selector': RegistryEndpointSelector
    },
    data () {
      return {
        error: null,

        form: {
          id: null,
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
          defaultRegistryId: [{required: true, trigger: 'blur', message: '必須項目です'}]
        }
      }
    },
    async created () {
      await this.init()
    },
    methods: {
      async init () {
        try {
          let data = (await api.account.get()).data
          let params = {
              id: data.selectedTenant.id
          }
          let [model] = api.f.data(await api.tenant.admin.getById(params))
          this.form.id = data.selectedTenant.id
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
      },
      async saveData () {
        let form = this.$refs.editForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.putTenant()
              this.init()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async putTenant () {
        let param = {
          id: this.form.id,
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
      }
    }
  }
</script>

<style lang="scss" scoped>
  .parent-container {
    display: grid;
    grid-template-rows: 100px 500px;
    grid-template-columns: 550px 1fr;
  }

  .container {
    margin-top: 10px;
  }

  .base {
    grid-row: 1 / 3;
    grid-column: 1 / 2;
    padding-right: 30px;
  }

  .detail {
    grid-row: 1 / 3;
    grid-column: 2 / 3;
    padding-right: 40px;
  }

  .right-button-group {
    text-align: right;
    padding-top: 50px;
  }

  .margin {
    padding-left: 30px;
  }

</style>
