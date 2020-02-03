<template>
  <el-dialog
    class="dialog"
    title="テナント作成"
    :visible="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <pl-display-error :error="error" />

      <h3>テナント情報</h3>
      <div class="margin">
        <el-form-item label="テナント名" prop="tenantName">
          <el-input v-model="form.tenantName" />
        </el-form-item>
        <el-form-item label="表示名" prop="displayName">
          <el-input v-model="form.displayName" />
        </el-form-item>
      </div>

      <h3>ストレージ情報</h3>
      <div class="margin">
        <el-form-item label="ストレージ" prop="storageId">
          <pl-storage-endpoint-selector v-model="form.storageId" />
        </el-form-item>
      </div>

      <h3>Git情報</h3>
      <div class="margin">
        <pl-git-endpoint-selector
          v-model="form.gitIds"
          :default-id="form.defaultGitId"
          @changeDefaultId="form.defaultGitId = $event"
        />
      </div>

      <h3>Docker Registry 情報</h3>
      <div class="margin">
        <pl-registry-endpoint-selector
          v-model="form.registryIds"
          :default-id="form.defaultRegistryId"
          @changeDefaultId="form.defaultRegistryId = $event"
        />
      </div>

      <el-row>
        <el-col class="button-group">
          <el-button @click="closeDialog">キャンセル</el-button>
          <el-button type="primary" @click="createData">作成</el-button>
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

export default {
  name: 'TenantCreate',
  components: {
    'pl-display-error': DisplayError,
    'pl-git-endpoint-selector': GitEndpointSelector,
    'pl-registry-endpoint-selector': RegistryEndpointSelector,
    'pl-storage-endpoint-selector': StorageEndpointSelector,
  },
  data() {
    return {
      dialogVisible: true,
      error: null,

      form: {
        tenantName: '',
        displayName: '',
        gitIds: [],
        defaultGitId: null,
        registryIds: [],
        defaultRegistryId: null,
        storageId: null,
      },

      rules: {
        tenantName: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
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
        storageId: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
      },
    }
  },
  methods: {
    async createData() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            await this.postTenant()
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async postTenant() {
      let param = {
        model: this.form,
      }
      await api.tenant.admin.post(param)
    },
    closeDialog() {
      this.emitCancel()
    },
    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.$emit('done')
    },
  },
}
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.margin {
  padding-left: 30px;
}
</style>
