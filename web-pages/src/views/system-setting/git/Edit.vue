<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    :disabled-params="disabledParams"
    @submit="submit"
    @delete="deleteGit"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="名前" prop="name">
        <el-input v-model="form.name" :disabled="isNotEditable" />
      </el-form-item>
      <el-form-item label="種別" prop="serviceType">
        <el-select
          v-model="form.serviceType"
          style="width: 100%"
          :disabled="isNotEditable"
        >
          <el-option
            v-for="(t, idx) in serviceTypes"
            :key="idx"
            :label="t.name"
            :value="t.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="リポジトリURL" prop="repositoryUrl">
        <el-input
          v-model="form.repositoryUrl"
          :disabled="isNotEditable"
          @change="handleChange"
        />
      </el-form-item>
      <el-form-item label="API URL" prop="apiUrl">
        <el-switch v-model="editApiUrl" :disabled="isNotEditable" />
        <el-input
          v-model="form.apiUrl"
          :disabled="isNotEditable || !editApiUrl"
        />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>
<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('git')
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
        serviceType: null,
        repositoryUrl: null,
        apiUrl: null,
      },
      rules: {
        name: [formRule],
        repositoryUrl: [formRule],
        serviceType: [formRule],
        apiUrl: [formRule],
      },
      title: '',
      error: null,
      isNotEditable: false,
      editApiUrl: false,
    }
  },
  computed: {
    ...mapGetters(['detail', 'serviceTypes']),
    disabledParams() {
      return {
        deleteButton: this.isNotEditable,
        submitButton: this.isNotEditable,
      }
    },
  },
  async created() {
    if (this.id === null) {
      this.title = 'Git登録'
    } else {
      this.title = 'Git編集'
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        this.form.serviceType = this.detail.serviceType
        this.form.repositoryUrl = this.detail.repositoryUrl
        this.form.apiUrl = this.detail.apiUrl

        this.isNotEditable = this.detail.isNotEditable
        this.editApiUrl = this.form.repositoryUrl !== this.form.apiUrl
        this.error = null
      } catch (e) {
        this.error = e
      }
    }
    await this.fetchServiceTypes()
  },
  methods: {
    ...mapActions([
      'fetchServiceTypes',
      'fetchDetail',
      'delete',
      'put',
      'post',
    ]),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              repositoryUrl: this.form.repositoryUrl,
              serviceType: this.form.serviceType,
              apiUrl: this.form.apiUrl,
            }
            if (this.id === null) {
              await this.post(params)
            } else {
              await this.put({ id: this.id, params: params })
            }
            this.error = null
            this.emitDone()
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteGit() {
      try {
        await this.delete(this.id)
        this.error = null
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },
    handleChange() {
      if (!this.editApiUrl) {
        this.form.apiUrl = this.form.repositoryUrl
      }
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

<style lang="scss" scoped></style>
