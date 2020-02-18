<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    :disabled="isNotEditable"
    @submit="submit"
    @delete="deleteRegistry"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="レジストリ名" prop="name">
        <el-input v-model="form.name" :disabled="isNotEditable" />
      </el-form-item>
      <h3>レジストリ情報</h3>
      <div style="padding-left: 30px; padding-right: 10px;">
        <el-form-item label="種別" prop="serviceType">
          <el-select
            v-model="form.serviceType"
            style="width: 100%;"
            :disabled="isNotEditable"
            @change="changeService"
          >
            <el-option
              v-for="service in serviceTypes"
              :key="service.id"
              :label="service.name"
              :value="service.id"
            />
          </el-select>
        </el-form-item>
        <el-row>
          <el-col :span="16">
            <el-form-item label="ホスト名" prop="host">
              <el-input
                v-model="form.host"
                :disabled="isNotEditable"
                @change="handleChange"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="ポート" prop="portNo">
              <el-input-number
                v-model="form.portNo"
                :min="1"
                :max="65535"
                controls-position="right"
                style="width: 100%;"
                :disabled="isNotEditable"
                @change="handleChange"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item label="API URL" prop="apiUrl">
          <el-switch v-model="form.editableApiUrl" :disabled="isNotEditable" />
          <el-input
            v-model="form.apiUrl"
            :disabled="isNotEditable || !form.editableApiUrl"
            @change="handleChange"
          />
        </el-form-item>
        <el-form-item label="URL" prop="registryUrl">
          <el-switch
            v-model="form.editableRegistryUrl"
            :disabled="isNotEditable"
          />
          <el-input
            v-model="form.registryUrl"
            :disabled="isNotEditable || !form.editableRegistryUrl"
          />
        </el-form-item>
        <div v-if="form.serviceType === 2">
          <el-form-item label="プロジェクト名" prop="projectName">
            <el-input v-model="form.projectName" :disabled="isNotEditable" />
          </el-form-item>
        </div>
        <div v-else></div>
      </div>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'

const defaultProtocol = 'https://'
const { mapGetters, mapActions } = createNamespacedHelpers('registry')
const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      form: {
        name: null,
        host: null,
        portNo: null,
        projectName: null,
        serviceType: null,
        apiUrl: null,
        editableApiUrl: false,
        registryUrl: null,
        editableRegistryUrl: false,
      },
      dialogVisible: true,
      error: null,
      isNotEditable: false,
      rules: {
        name: [formRule],
        host: [formRule],
        apiUrl: [formRule],
        registryUrl: [formRule],
        portNo: [formRule],
        serviceType: [formRule],
        projectName: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters(['detail', 'serviceTypes']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'Dockerレジストリ登録'
    } else {
      this.title = 'Dockerレジストリ編集'
      try {
        await this.fetchDetail()
        this.form.name = this.detail.name
        this.form.host = this.detail.host
        this.form.portNo = this.detail.portNo
        this.form.password = this.detail.password
        this.form.projectName = this.detail.projectName
        this.form.serviceType = this.detail.serviceType
        this.form.isNotEditable = this.detail.isNotEditable
        this.form.apiUrl = this.detail.apiUrl
        this.form.editableApiUrl =
          defaultProtocol + this.detail.host !== this.detail.apiUrl
        this.form.registryUrl = this.detail.registryUrl
        this.form.editableRegistryUrl =
          defaultProtocol + this.detail.host + ':' + this.detail.portNo !==
          this.detail.registryUrl
      } catch (e) {
        this.error = e
      }
    }
    await this.fetchServiceTypes()
  },
  methods: {
    ...mapActions([
      'fetchDetail',
      'fetchServiceTypes',
      'delete',
      'post',
      'put',
    ]),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              id: this.id,
              model: {
                name: this.form.name,
                host: this.form.host,
                portNo: this.form.portNo,
                serviceType: this.form.serviceType,
                projectName: this.form.projectName,
                registryUrl: this.form.registryUrl,
                apiUrl: this.form.apiUrl,
              },
            }
            if (this.id === null) {
              await this.post(params)
            } else {
              await this.put(params)
            }
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteRegistry() {
      try {
        await this.delete()
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },
    handleChange() {
      if (!this.form.editableApiUrl) {
        if (this.form.host) {
          this.form.apiUrl = defaultProtocol + this.form.host
        }
      }
      if (!this.form.editableRegistryUrl) {
        if (this.form.host && this.form.portNo) {
          this.form.registryUrl =
            defaultProtocol + this.form.host + ':' + this.form.portNo
        }
      }
    },
    changeService() {
      this.form.projectName = null
    },
    emitDone() {
      this.$emit('done')
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped></style>
