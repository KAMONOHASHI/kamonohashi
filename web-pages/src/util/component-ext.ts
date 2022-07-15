export default {
  mounted() {
    let { title } = this.$options
    if (title) {
      title = typeof title === 'function' ? title.call(this) : title
      document.title = `${title} - KAMONOHASHI`
    }
  },
  methods: {
    showSuccessMessage: function(msg) {
      if (msg) {
        this.$notify.success({ title: msg })
      } else {
        this.$notify.success({ title: '正常に処理されました' })
      }
    },
  },
}
