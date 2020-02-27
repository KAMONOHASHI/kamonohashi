<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    submit-text="作成"
    :delete-button-params="deleteButtonParams"
    @submit="submit"
    @delete="deleteTenant"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />

      <h3>テナント情報</h3>
      <div class="margin">
        <kqi-display-text-form v-if="id !== null" label="ID" :value="id" />
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
          <kqi-storage-endpoint-selector v-model="form.storageId" />
        </el-form-item>
      </div>

      <h3>Git情報</h3>
      <div class="margin">
        <kqi-git-endpoint-selector v-model="form.gitEndpoint" />
      </div>

      <h3>Docker Registry 情報</h3>
      <div class="margin">
        <kqi-registry-endpoint-selector v-model="form.registry" />
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
    let gitSelectedIdsValidator = (rule, value, callback) => {
      if (this.form.gitEndpoint.selectedIds.length === 0) {
        callback(new Error('必須項目です'))
      } else {
        callback()
      }
    }
    let gitDefaultIdValidator = (rule, value, callback) => {
      if (this.form.gitEndpoint.defaultId === null) {
        callback(new Error('必須項目です'))
      } else {
        callback()
      }
    }
    let registrySelectedIdsValidator = (rule, value, callback) => {
      if (this.form.registry.selectedIds.length === 0) {
        callback(new Error('必須項目です'))
      } else {
        callback()
      }
    }
    let registryDefaultIdValidator = (rule, value, callback) => {
      if (this.form.registry.defaultId === null) {
        callback(new Error('必須項目です'))
      } else {
        callback()
      }
    }

    return {
      title: '',
      error: null,
      deleteButtonParams: {},

      form: {
        tenantName: '',
        displayName: '',
        gitEndpoint: {
          selectedIds: [],
          defaultId: null,
        },
        registry: {
          selectedIds: [],
          defaultId: null,
        },
        storageId: null,
        availableInfiniteTimeNotebook: false,
      },

      rules: {
        tenantName: [formRule],
        displayName: [formRule],
        gitSelectedIds: [
          {
            required: true,
            validator: gitSelectedIdsValidator,
            trigger: 'blur',
          },
        ],
        gitDefaultId: [
          {
            required: true,
            validator: gitDefaultIdValidator,
            trigger: 'blur',
          },
        ],
        registryselectedIds: [
          {
            required: true,
            validator: registrySelectedIdsValidator,
            trigger: 'blur',
          },
        ],
        registryDefaultId: [
          {
            required: true,
            validator: registryDefaultIdValidator,
            trigger: 'blur',
          },
        ],
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
        this.form.storageId = this.detail.storageId
        this.form.gitEndpoint.selectedIds = this.detail.gitIds
        this.form.gitEndpoint.defaultId = this.detail.defaultGitId
        this.form.registry.selectedIds = this.detail.registryIds
        this.form.registry.defaultId = this.detail.defaultRegistryId
        this.form.availableInfiniteTimeNotebook = this.detail.availableInfiniteTimeNotebook
        this.error = null
        this.deleteButtonParams = {
          isDanger: true,
          warningText:
            'テナントを削除すると、テナントに紐づくデータが失われます。処理を続けるにはテナント名を入力してください。',
          confirmText: this.form.tenantName,
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
                storageId: this.form.storageId,
                gitIds: this.form.gitEndpoint.selectedIds,
                defaultGitId: this.form.gitEndpoint.defaultId,
                registryIds: this.form.registry.selectedIds,
                defaultRegistryId: this.form.registry.defaultId,
                availableInfiniteTimeNotebook: this.form
                  .availableInfiniteTimeNotebook,
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
