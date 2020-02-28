<template>
  <div>
    <h2>接続テナント設定</h2>
    <el-card>
      <el-form
        ref="editForm"
        :model="form"
        :rules="rules"
        class="parent-container"
      >
        <pl-display-error :error="error" />

        <el-col class="container base">
          <h3>テナント情報</h3>
          <div class="margin">
            <pl-display-text label="ID" :value="form.id" />
            <pl-display-text label="テナント名" :value="form.name" />
            <el-form-item label="表示名" prop="displayName">
              <el-input v-model="form.displayName" />
            </el-form-item>
            <el-form-item label="ノートブック無期限実行" required>
              <el-switch
                v-model="form.availableInfiniteTimeNotebook"
                style="width: 100%;"
                inactive-text="禁止"
                active-text="許可"
              />
            </el-form-item>
          </div>
        </el-col>

        <el-col class="container detail">
          <h3>Git情報</h3>
          <div class="margin">
            <pl-git-endpoint-selector
              v-model="form.gitIds"
              :default-id="form.defaultGitId"
              :tenant-id="getTenantId"
              @changeDefaultId="form.defaultGitId = $event"
            />
          </div>

          <h3>Docker Registry 情報</h3>
          <div class="margin">
            <pl-registry-endpoint-selector
              v-model="form.registryIds"
              :default-id="form.defaultRegistryId"
              :tenant-id="getTenantId"
              @changeDefaultId="form.defaultRegistryId = $event"
            />
          </div>

          <el-row :gutter="20">
            <el-col class="right-button-group">
              <el-button type="primary" @click="saveData">
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
import GitEndpointSelector from '@/components/common/GitEndpointSelector.vue'
import RegistryEndpointSelector from '@/components/common/RegistryEndpointSelector.vue'

export default {
  name: 'TenantSetting',
  title: '接続テナント設定',
  components: {
    'pl-display-error': DisplayError,
    'pl-display-text': DisplayTextForm,
    'pl-git-endpoint-selector': GitEndpointSelector,
    'pl-registry-endpoint-selector': RegistryEndpointSelector,
  },
  data() {
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
        storageId: null,
        availableInfiniteTimeNotebook: false,
      },
      rules: {
        displayName: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
        gitIds: [{ required: true, trigger: 'blur', message: '必須項目です' }],
        defaultGitId: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
        registryIds: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
        defaultRegistryId: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
      },
    }
  },
  async created() {
    await this.init()
  },
  methods: {
    async init() {
      try {
        let [model] = api.f.data(await api.tenant.get())
        this.form.id = model.id
        this.form.name = model.name
        this.form.displayName = model.displayName
        this.form.gitIds = model.gitIds
        this.form.defaultGitId = model.defaultGitId
        this.form.storageId = model.storageId
        this.form.defaultRegistryId = model.defaultRegistryId
        this.form.registryIds = model.registryIds
        this.form.availableInfiniteTimeNotebook =
          model.availableInfiniteTimeNotebook
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async saveData() {
      let form = this.$refs.editForm
      await form.validate(async valid => {
        if (valid) {
          try {
            await this.putTenant()
            this.showSuccessMessage()
            this.init()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async putTenant() {
      let param = {
        model: {
          displayName: this.form.displayName,
          gitIds: this.form.gitIds,
          defaultGitId: this.form.defaultGitId,
          storageId: this.form.storageId,
          defaultRegistryId: this.form.defaultRegistryId,
          registryIds: this.form.registryIds,
          availableInfiniteTimeNotebook: this.form
            .availableInfiniteTimeNotebook,
        },
      }
      await api.tenant.put(param)
    },
    getTenantId() {
      // 接続中テナントのIDを取得する
      let data = api.account.get().data
      return data.selectedTenant.id
    },
  },
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
