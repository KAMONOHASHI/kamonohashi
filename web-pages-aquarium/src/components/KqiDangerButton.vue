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
  props: {
    buttonLabel: { type: String, default: '' },
    warningText: {
      type: String,
      default: '',
    },
    confirmText: {
      type: String,
      default: '',
    },
  },
  methods: {
    validateInput(input) {
      if (input === this.confirmText) {
        return true
      } else return '入力内容が不一致です'
    },
    showConfirm() {
      this.$prompt(this.warningText, 'Warning', {
        confirmButtonText: '確定',
        cancelButtonText: 'キャンセル',
        inputValidator: this.validateInput,
        inputErrorMessage: 'Invalid Name',
      })
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

<style lang="scss" scoped></style>
