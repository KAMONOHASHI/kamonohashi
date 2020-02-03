<template>
  <span>
    <el-button type="danger" @click="showConfirm">
      <span v-if="buttonLabel">{{ buttonLabel }}</span>
      <span v-else class="el-icon-delete" />
    </el-button>
  </span>
</template>

<script>
export default {
  name: 'DeleteUserButton',
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
        'ユーザを削除すると、ユーザに紐づくデータが失われます。処理を続けるにはユーザ名を入力してください。',
        'Warning',
        {
          confirmButtonText: '確定',
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
