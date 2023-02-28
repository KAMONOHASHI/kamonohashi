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
        <kqi-display-error :error="error" />

        <el-col class="container base">
          <h3>テナント情報</h3>
          <div class="margin">
            <kqi-display-text-form label="ID" :value="form.id" />
            <kqi-display-text-form label="テナント名" :value="form.name" />
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
          <kqi-git-endpoint-selector
            v-model="form.gitEndpoint"
            :endpoints="gitEndpoints"
          />

          <kqi-registry-endpoint-selector
            v-model="form.registry"
            :registries="registryEndpoints"
          />

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

<script lang="ts">
import Vue from 'vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiGitEndpointSelector from '@/components/selector/KqiGitEndpointSelector.vue'
import KqiRegistryEndpointSelector from '@/components/selector/KqiRegistryEndpointSelector.vue'
import { mapGetters, mapActions } from 'vuex'
import validator from '@/util/validator'
interface DataType {
  error: null | Error
  form: {
    id: null | string
    name: string
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
    availableInfiniteTimeNotebook: boolean
  }
  rules: {
    displayName: Array<typeof formRule>
    gitEndpoint: {
      required: boolean
      trigger: string
      validator: Function
    }
    registry: {
      required: boolean
      validator: Function
      trigger: string
    }
  }
}
const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default Vue.extend({
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiGitEndpointSelector,
    KqiRegistryEndpointSelector,
  },
  data(): DataType {
    return {
      error: null,

      form: {
        id: null,
        name: '',
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
      },
    }
  },
  computed: {
    ...mapGetters({
      //@ts-ignore
      tenant: ['tenant/detail'],
      gitEndpoints: ['git/endpoints'],
      registryEndpoints: ['registry/registries'],
    }),
  },
  async created() {
    await this.retrieveData()
    await this['git/fetchTenantEndpoints'](this.tenant.id)
    await this['registry/fetchTenantRegistries'](this.tenant.id)
  },
  methods: {
    ...mapActions([
      'git/fetchTenantEndpoints',
      'registry/fetchTenantRegistries',
      'tenant/fetchCurrentTenant',
      'tenant/putCurrentTenant',
    ]),
    async retrieveData() {
      try {
        await this['tenant/fetchCurrentTenant']()
        this.form.id = String(this.tenant.id)
        this.form.name = this.tenant.name
        this.form.displayName = this.tenant.displayName
        this.form.gitEndpoint.selectedIds = this.tenant.gitIds
        this.form.gitEndpoint.defaultId = this.tenant.defaultGitId
        this.form.storageId = this.tenant.storageId
        this.form.registry.defaultId = this.tenant.defaultRegistryId
        this.form.registry.selectedIds = this.tenant.registryIds
        this.form.availableInfiniteTimeNotebook = this.tenant.availableInfiniteTimeNotebook
        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
    async saveData() {
      let form = this.$refs.editForm
      //@ts-ignore
      await form.validate(async valid => {
        if (valid) {
          try {
            let param = {
              body: {
                displayName: this.form.displayName,
                gitIds: this.form.gitEndpoint.selectedIds,
                defaultGitId: this.form.gitEndpoint.defaultId,
                storageId: this.form.storageId,
                defaultRegistryId: this.form.registry.defaultId,
                registryIds: this.form.registry.selectedIds,
                availableInfiniteTimeNotebook: this.form
                  .availableInfiniteTimeNotebook,
              },
            }
            await this['tenant/putCurrentTenant'](param)
            this.showSuccessMessage()
            this.retrieveData()
            this.error = null
          } catch (e) {
            if (e instanceof Error) this.error = e
          }
        }
      })
    },
  },
})
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

/deep/ label {
  font-weight: bold !important;
}
</style>
