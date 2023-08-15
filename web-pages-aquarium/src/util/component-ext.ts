export default {
  mounted() {
    //@ts-ignore
    let { title } = this.$options
    if (title) {
      title = typeof title === 'function' ? title.call(this) : title
      document.title = `${title} - KAMONOHASHI`
    }
  },
  methods: {
    showSuccessMessage: function(msg: string) {
      if (msg) {
        //@ts-ignore
        this.$notify.success({ title: msg })
      } else {
        //@ts-ignore
        this.$notify.success({ title: '正常に処理されました' })
      }
    },
  },
}
