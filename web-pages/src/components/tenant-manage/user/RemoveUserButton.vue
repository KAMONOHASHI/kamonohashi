<template>
  <span>
    <el-button type="danger" @click="showConfirm">
      <span v-if="buttonLabel">{{ buttonLabel }}</span>
      <span v-else>Remove</span>
    </el-button>
  </span>
</template>

<script>
export default {
  name: 'RemoveUserButton',
  props: {
    buttonLabel: String,
    size: String,
    message: String,
    disabled: Boolean,
    userName: String,
  },
  methods: {
    validateInput(input) {
      if (input === this.userName) {
        return true
      } else return 'Value is invalid'
    },
    showConfirm() {
      this.$prompt(
        'ユーザを除外すると、ユーザ' +
          this.userName +
          'は現在のテナントに入れなくなります。処理を続けるにはユーザ名を入力してください。',
        'Warning',
        {
          confirmButtonText: '除外する',
          cancelButtonText: 'キャンセル',
          inputValidator: this.validateInput,
          inputErrorMessage: 'Invalid Name',
        },
      )
        .then(() => {
          this.$emit('delete')
        })
        .catch(() => {
          this.$notify.info({
            type: 'info',
            message: 'キャンセルされました',
          })
        })
    },
  },
}
</script>

<style lang="scss" scoped>
.cancel-button {
  text-align: left;
}

.ok-button {
  text-align: right;
}
</style>
