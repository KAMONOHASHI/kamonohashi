<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    submit-text="作成"
    :delete-params="deleteParams"
    @submit="submit"
    @delete="deleteTenant"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />

      <h3>テナント情報</h3>
      <div class="margin">
        <kqi-display-text-form label="ID" :value="id" />
        <el-form-item v-if="id === null" label="テナント名" prop="tenantName">
          <el-input v-model="form.tenantName" />
        </el-form-item>
        <kqi-display-text-form
          v-else
          label="テナント名"
          :value="form.tenantName"
        />
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

      <h3>ストレージ情報</h3>
      <div class="margin">
        <el-form-item>
          <kqi-storage-endpoint-selector
            :storage-id="form.storageId"
            @changeStorageId="changeStorageId"
          />
        </el-form-item>
      </div>

      <h3>Git情報</h3>
      <div class="margin">
        <kqi-git-endpoint-selector
          :git-ids="form.gitIds"
          :default-id="form.defaultGitId"
          @changeSelectedIds="changeSelectedGitIds"
          @changeDefaultId="changeDefaultGitId"
        />
      </div>

      <h3>Docker Registry 情報</h3>
      <div class="margin">
        <kqi-registry-endpoint-selector
          :registry-ids="form.registryIds"
          :default-id="form.defaultRegistryId"
          @changeSelectedIds="changeSelectedRegistryIds"
          @changeDefaultId="changeDefaultRegistryId"
        />
      </div>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiGitEndpointSelector from '@/components/selector/KqiGitEndpointSelector.vue'
import KqiRegistryEndpointSelector from '@/components/selector/KqiRegistryEndpointSelector.vue'
import KqiStorageEndpointSelector from '@/components/selector/KqiStorageEndpointSelector.vue'
import { mapGetters, mapActions } from 'vuex'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  name: 'TenantEdit',
  components: {
    KqiDialog,
    KqiDisplayTextForm,
    KqiDisplayError,
    KqiGitEndpointSelector,
    KqiRegistryEndpointSelector,
    KqiStorageEndpointSelector,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      title: '',
      dialogVisible: true,
      error: null,
      deleteParams: {},

      form: {
        tenantName: '',
        displayName: '',
        gitIds: [],
        defaultGitId: null,
        registryIds: [],
        defaultRegistryId: null,
        storageId: null,
        availableInfiniteTimeNotebook: false,
      },

      rules: {
        tenantName: [formRule],
        displayName: [formRule],
        gitIds: [formRule],
        defaultGitId: [formRule],
        registryIds: [formRule],
        defaultRegistryId: [formRule],
        storageId: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters({ detail: ['tenant/detail'] }),
  },
  async created() {
    await this['storage/fetchStorages']()
    await this['git/fetchEndpoints']()
    await this['registry/fetchRegistries']()
    if (this.id === null) {
      this.title = 'テナント作成'
    } else {
      this.title = 'テナント編集'
      try {
        await this['tenant/fetchDetail']()
        this.form.tenantName = this.detail.name
        this.form.displayName = this.detail.displayName
        this.form.gitIds = this.detail.gitIds
        this.form.defaultGitId = this.detail.defaultGitId
        this.form.storageId = this.detail.storageId
        this.form.defaultRegistryId = this.detail.defaultRegistryId
        this.form.registryIds = this.detail.registryIds
        this.form.availableInfiniteTimeNotebook = this.detail.availableInfiniteTimeNotebook
        this.error = null
        this.deleteParams = {
          dangerFlag: true,
          screanName: 'tenant',
          name: this.form.tenantName,
        }
      } catch (e) {
        this.error = e
      }
    }
  },
  methods: {
    ...mapActions([
      'storage/fetchStorages',
      'git/fetchEndpoints',
      'registry/fetchRegistries',
      'tenant/fetchDetail',
      'tenant/post',
      'tenant/put',
      'tenant/delete',
    ]),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              model: {
                tenantName: this.form.tenantName,
                displayName: this.form.displayName,
                gitIds: this.form.gitIds,
                defaultGitId: this.form.defaultGitId,
                storageId: this.form.storageId,
                defaultRegistryId: this.form.defaultRegistryId,
                registryIds: this.form.registryIds,
              },
            }
            if (this.id === null) {
              await this['tenant/post'](params)
            } else {
              await this['tenant/put'](params)
            }
            this.error = null
            this.emitDone()
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteTenant() {
      try {
        let params = {
          id: this.id,
          model: {
            data: {
              ignoreMinioBucketDeletion: true,
            },
          },
        }
        await this['tenant/delete'](params)
        this.error = null
        this.emitDone()
      } catch (e) {
        this.error = e
      }
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
    changeStorageId(selectStorageId) {
      this.form.storageId = selectStorageId.value
    },
    async changeSelectedGitIds(selectGitIds) {
      this.form.gitIds = selectGitIds
    },
    changeDefaultGitId(selectDefaultGitIds) {
      this.form.defaultGitId = selectDefaultGitIds.value
    },
    changeSelectedRegistryIds(selectRegistryIds) {
      this.form.registryIds = selectRegistryIds
    },
    changeDefaultRegistryId(selectDefaultRegistryIds) {
      this.form.defaultRegistryId = selectDefaultRegistryIds.value
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

.margin {
  padding-left: 30px;
}

.footer {
  padding-top: 40px;
}
</style>
