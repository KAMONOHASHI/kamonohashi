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
      <div class="left-margin">
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
      </div>
      <el-form-item label="ノートブック無期限実行" required>
        <el-switch
          v-model="form.availableInfiniteTimeNotebook"
          style="width: 100%;"
          inactive-text="禁止"
          active-text="許可"
          class="left-margin"
        />
      </el-form-item>

      <kqi-storage-endpoint-selector
        v-model="form.storageId"
        :storages="storageEndpoints"
      />

      <kqi-git-endpoint-selector
        v-model="form.gitEndpoint"
        :endpoints="gitEndpoints"
      />

      <kqi-registry-endpoint-selector
        v-model="form.registry"
        :registries="registryEndpoints"
      />

      <kqi-user-group-selector
        v-model="form.userGroupIds"
        :user-groups="userGroups"
      />
    </el-form>
  </kqi-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDialog from '@/components/KqiDialog.vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiStorageEndpointSelector from '@/components/selector/KqiStorageEndpointSelector.vue'
import KqiGitEndpointSelector from '@/components/selector/KqiGitEndpointSelector.vue'
import KqiRegistryEndpointSelector from '@/components/selector/KqiRegistryEndpointSelector.vue'
import KqiUserGroupSelector from '@/components/selector/KqiUserGroupSelector.vue'
import { mapGetters, mapActions } from 'vuex'
import validator from '@/util/validator'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}
//import * as gen from '@/api/api.generate'
interface DataType {
  title: string
  error: null | Error
  deleteButtonParams:
    | {}
    | {
        isDanger: boolean
        warningText: string
        confirmText: string
      }
  form: {
    tenantName: string
    displayName: string
    gitEndpoint: {
      selectedIds: Array<number>
      defaultId: null | number
    }
    registry: {
      selectedIds: Array<number>
      defaultId: null | number
    }
    storageId: null | number
    availableInfiniteTimeNotebook: false
    userGroupIds: Array<number>
  }

  rules: {
    tenantName: Array<typeof formRule>
    displayName: Array<typeof formRule>
    gitEndpoint: {
      required: true
      trigger: string
      validator: typeof validator.gitEndpointValidator
    }
    registry: {
      required: true
      validator: typeof validator.regystryEndpointValidator
      trigger: string
    }
    storageId: Array<typeof formRule>
  }
}

export default Vue.extend({
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiStorageEndpointSelector,
    KqiGitEndpointSelector,
    KqiRegistryEndpointSelector,
    KqiUserGroupSelector,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
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
        userGroupIds: [],
      },

      rules: {
        tenantName: [formRule],
        displayName: [formRule],
        gitEndpoint: {
          required: true,
          trigger: 'blur',
          validator: validator.gitEndpointValidator,
        },
        registry: {
          required: true,
          validator: validator.regystryEndpointValidator,
          trigger: 'blur',
        },
        storageId: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters({
      detail: ['tenant/detail'],
      gitEndpoints: ['git/endpoints'],
      registryEndpoints: ['registry/registries'],
      storageEndpoints: ['storage/storages'],
      userGroups: ['userGroup/userGroups'],
    }),
  },
  async created() {
    await this['storage/fetchStorages']()
    await this['git/fetchEndpoints']()
    await this['registry/fetchRegistries']()
    await this['userGroup/fetchUserGroups']()
    if (this.id === null) {
      this.title = 'テナント作成'
    } else {
      this.title = 'テナント編集'
      try {
        await this['tenant/fetchDetail'](this.id)
        this.form.tenantName = this.detail.name
        this.form.displayName = this.detail.displayName
        this.form.storageId = this.detail.storageId
        this.form.gitEndpoint.selectedIds = this.detail.gitIds
        this.form.gitEndpoint.defaultId = this.detail.defaultGitId
        this.form.registry.selectedIds = this.detail.registryIds
        this.form.registry.defaultId = this.detail.defaultRegistryId
        this.form.availableInfiniteTimeNotebook = this.detail.availableInfiniteTimeNotebook
        this.form.userGroupIds = this.detail.userGroupIds
        this.error = null
        this.deleteButtonParams = {
          isDanger: true,
          warningText:
            'テナントを削除すると、テナントに紐づくデータが失われます。処理を続けるにはテナント名を入力してください。',
          confirmText: this.form.tenantName,
        }
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    }
  },
  methods: {
    ...mapActions([
      'storage/fetchStorages',
      'git/fetchEndpoints',
      'registry/fetchRegistries',
      'userGroup/fetchUserGroups',
      'tenant/fetchDetail',
      'tenant/post',
      'tenant/put',
      'tenant/delete',
    ]),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          if (await this.showConfirm()) {
            try {
              let params = {
                tenantName: this.form.tenantName,
                displayName: this.form.displayName,
                storageId: this.form.storageId,
                gitIds: this.form.gitEndpoint.selectedIds,
                defaultGitId: this.form.gitEndpoint.defaultId,
                registryIds: this.form.registry.selectedIds,
                defaultRegistryId: this.form.registry.defaultId,
                availableInfiniteTimeNotebook: this.form
                  .availableInfiniteTimeNotebook,
                userGroupIds: this.form.userGroupIds,
              }
              if (this.id === null) {
                await this['tenant/post'](params)
              } else {
                await this['tenant/put']({ id: this.id, params: params })
              }
              this.error = null
              this.emitDone()
            } catch (e) {
              if (e instanceof Error) this.error = e
            }
          }
        }
      })
    },
    async deleteTenant() {
      try {
        await this['tenant/delete'](this.id)
        this.error = null
        this.emitDone()
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
    async showConfirm() {
      // 新規作成時は確認ダイアログを表示しない
      if (this.id === null) {
        return true
      }
      if (this.checkUserGroupsChange()) {
        return true
      }
      let confirmMessage =
        '紐づけが解除されたユーザグループに属するユーザはこのテナントに参加できなくなります。変更を保存しますか？'
      try {
        await this.$confirm(confirmMessage, 'Warning', {
          confirmButtonText: 'はい',
          cancelButtonText: 'キャンセル',
          type: 'warning',
        })
        return true
      } catch (e) {
        return false
      }
    },
    // ユーザグループの紐づけが解除されているか判定する
    checkUserGroupsChange() {
      if (!this.detail.userGroupIds) {
        return true
      }
      let count = 0
      this.detail.userGroupIds.forEach(id => {
        if (this.form.userGroupIds.includes(id)) count++
      })
      return this.detail.userGroupIds.length == count
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
})
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.left-margin {
  padding-left: 30px;
}

.footer {
  padding-top: 40px;
}
</style>
