<template>
  <el-dialog
    class="dialog"
    title="LDAPサーバ同期実行"
    :visible.sync="dialogVisible"
    :before-close="emitCancel"
    :close-on-click-modal="false"
  >
    <p>
      LDAPサーバに問い合わせを行い、全ユーザのテナント所属と権限情報の更新を行います。
    </p>
    <p>同期処理には時間がかかる場合があります。</p>
    <el-row class="footer">
      <el-form ref="createForm" :model="form" :rules="rules">
        <kqi-display-error :error="error" />

        <el-form-item label="LDAPユーザID" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="LDAPパスワード" prop="password">
          <el-input v-model="form.password" type="password" />
        </el-form-item>
      </el-form>
      <el-col :span="24" class="right-button-group">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="syncLdap">同期開始</el-button>
      </el-col>
    </el-row>
  </el-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDisplayError from '@/components/KqiDisplayError.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapActions } = createNamespacedHelpers('user')

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}
interface DataType {
  form: {
    name: string
    password: string
  }
  rules: {
    name: Array<typeof formRule>
    password: Array<typeof formRule>
  }
  dialogVisible: boolean
  error: null | Error
}

export default Vue.extend({
  components: {
    KqiDisplayError,
  },
  data(): DataType {
    return {
      form: {
        name: '',
        password: '',
      },
      rules: {
        name: [formRule],
        password: [formRule],
      },
      dialogVisible: true,
      error: null,
    }
  },
  methods: {
    ...mapActions(['syncLdapUsers']),
    async syncLdap() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          if (await this.showConfirm()) {
            try {
              let params = {
                body: {
                  userName: this.form.name,
                  password: this.form.password,
                },
              }
              await this.syncLdapUsers(params)
              this.emitDone()
              this.error = null
            } catch (e) {
              if (e instanceof Error) this.error = e
            }
          }
        }
      })
    },
    async showConfirm() {
      let confirmMessage =
        '同期処理を開始しますか？ユーザ数が多い場合、処理完了までに時間がかかる場合があります。'
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
    emitDone() {
      this.$emit('done')
      this.dialogVisible = false
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
})
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}
.footer {
  padding-top: 40px;
}
p {
  padding-top: 5px;
}
</style>
