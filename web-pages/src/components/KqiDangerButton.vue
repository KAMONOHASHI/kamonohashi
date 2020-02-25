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
    params: {
      type: Object,
      default() {
        return {
          dangerFlag: false,
          screanName: null,
          name: null,
        }
      },
    },
  },
  data() {
    return {
      warningText: [],
    }
  },
  created() {
    this.warningText = [
      {
        screanName: 'tenant',
        text:
          'テナントを削除すると、テナントに紐づくデータが失われます。処理を続けるにはテナント名を入力してください。',
      },
      {
        screanName: 'user',
        text:
          'ユーザを削除すると、紐づいているテナントからユーザ情報が失われます。処理を続けるにはユーザ名を入力してください。',
      },
      {
        screanName: 'tenant-manage/user',
        text:
          'ユーザを除外すると、対象ユーザは現在のテナントに入れなくなります。処理を続けるにはユーザ名を入力してください。',
      },
    ]
  },
  methods: {
    validateInput(input) {
      if (input === this.params.name) {
        return true
      } else return 'Value is invalid'
    },
    showConfirm() {
      this.$prompt(
        this.warningText.find(wt => wt.screanName == this.params.screanName)
          .text,
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

<style lang="scss" scoped></style>
