<template>
  <!-- パスワード変更 -->
  <div>
    <el-form ref="passForm" :rules="passRules" :model="passForm">
      <el-form-item
        label="ユーザ表示名"
        prop="displayName"
        :label-width="labelwidth"
      >
        <el-input v-model="passForm.displayName" type="text" />
      </el-form-item>
      <el-form-item
        label="現在のパスワード(変更する場合のみ入力)"
        prop="currentPassword"
        :label-width="labelwidth"
      >
        <el-input v-model="passForm.currentPassword" type="password" />
      </el-form-item>
      <el-form-item
        label="新しいパスワード(変更する場合のみ入力)"
        prop="password"
        :label-width="labelwidth"
      >
        <el-input v-model="passForm.password[0]" type="password" />
        <span
          style="position: relative; left: -200px; top:24px;"
          class="content-color"
        >
          （再入力）
        </span>
        <el-input
          v-model="passForm.password[1]"
          type="password"
          style="position: relative; top:-20px;"
        />
      </el-form-item>
      <el-form-item class="button-group">
        <el-button type="primary" @click="handlePassword">
          更新
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      default: () => ({
        currentPassword: '',
        password: ['', ''],
      }),
    },
    oldDisplayName: { type: String, default: () => null },
  },
  data() {
    return {
      labelwidth: '320px',

      passRules: {
        currentPassword: {
          required: true,
          trigger: 'blur',
          message: '必須項目です',
        },
        password: [
          {
            required: true,
            trigger: 'blur',
            validator: this.passwordValidator,
          },
        ],
      },
    }
  },
  computed: {
    passForm() {
      return this.value === undefined || this.value === null ? {} : this.value
    },
  },

  methods: {
    passwordValidator(rule, value, callback) {
      if (!(value[0] && value[1])) {
        callback(new Error('必須項目です'))
      } else if (!(value[0] === value[1])) {
        callback(new Error('同一のパスワードを入力してください'))
      } else {
        callback()
      }
    },

    handlePassword() {
      if (this.passForm.displayName != this.oldDisplayName) {
        this.$emit('updateDisplayName')
      }
      if (
        this.passForm.password[0] != null &&
        this.passForm.password[0] != ''
      ) {
        this.$refs['passForm'].validate(async valid => {
          if (valid) {
            this.$emit('updatePassword')
          }
        })
      }
    },
  },
}
</script>

<style scoped>
.el-form-item {
  margin-top: 30px;
}

.el-form-item /deep/ .el-form-item__label {
  font-weight: bold !important;
  padding-right: 30px;
  text-align: left;
}

.el-form-item /deep/ .el-form-item__inner {
  text-align: left;
}

.el-form-item /deep/ .el-form-item__content {
  font-weight: bold !important;
  padding-right: 30px;
}

.el-form-item.is-required {
  padding-top: 50px;
  text-align: left;
}

.button-group {
  text-align: right;
  padding-top: 100px;
  padding-right: 30px;
}

.content-color {
  color: #606266;
}
</style>
