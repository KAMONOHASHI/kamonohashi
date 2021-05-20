<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteStorage"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="ストレージ名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>

      <h3>ストレージ情報</h3>
      <div style="padding-left: 30px; padding-right: 10px;">
        <el-form-item label="ホスト名:ポート" prop="serverUrl">
          <el-input v-model="form.serverUrl" />
        </el-form-item>
        <el-form-item label="アクセスキー" prop="accessKey">
          <el-input v-model="form.accessKey" />
        </el-form-item>
        <el-form-item label="シークレットキー" prop="secretKey">
          <el-input v-model="form.secretKey" type="password" />
        </el-form-item>
        <el-form-item label="NFSサーバ" prop="nfsServer">
          <el-input v-model="form.nfsServer" />
        </el-form-item>
        <el-form-item label="NFSエクスポートポイント" prop="nfsRoot">
          <el-input v-model="form.nfsRoot" />
        </el-form-item>
      </div>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('storage')
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
        serverUrl: null,
        accessKey: null,
        secretKey: null,
        nfsServer: null,
        nfsRoot: null,
      },
      title: '',
      error: null,
      rules: {
        name: [formRule],
        serverUrl: [formRule],
        accessKey: [formRule],
        secretKey: [formRule],
        nfsServer: [formRule],
        nfsRoot: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters(['detail']),
  },
  async created() {
    if (this.id === null) {
      this.title = 'ストレージ登録'
    } else {
      this.title = 'ストレージ編集'
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        this.form.serverUrl = this.detail.serverUrl
        this.form.accessKey = this.detail.accessKey
        this.form.secretKey = this.detail.secretKey
        this.form.nfsServer = this.detail.nfsServer
        this.form.nfsRoot = this.detail.nfsRoot
      } catch (e) {
        this.error = e
      }
    }
  },
  methods: {
    ...mapActions(['fetchDetail', 'delete', 'put', 'post']),
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              serverUrl: this.form.serverUrl,
              accessKey: this.form.accessKey,
              secretKey: this.form.secretKey,
              nfsServer: this.form.nfsServer,
              nfsRoot: this.form.nfsRoot,
            }
            if (this.id === null) {
              await this.post(params)
            } else {
              await this.put({ id: this.id, params: params })
            }
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteStorage() {
      try {
        await this.delete(this.id)
        this.error = null
        this.emitDone()
      } catch (e) {
        this.error = e
      }
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
