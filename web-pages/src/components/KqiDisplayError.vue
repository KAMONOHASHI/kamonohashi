<template>
  <span>
    <div v-if="title" class="error-message">
      <b>{{ title }}</b>
    </div>
    <div v-if="errors" class="error-message">
      <div v-for="(err, index) in errors" :key="index">
        {{ err }}
      </div>
    </div>
  </span>
</template>

<script>
export default {
  props: {
    error: {
      type: Object,
      default: null,
    },
  },

  data() {
    return {
      title: '',
      errors: null,
    }
  },

  watch: {
    async error() {
      await this.showError()
    },
  },

  async mounted() {
    await this.showError()
  },

  methods: {
    showError() {
      if (this.error) {
        if (
          typeof this.error === 'object' &&
          'response' in this.error &&
          'data' in this.error.response &&
          'title' in this.error.response.data
        ) {
          this.title = this.error.response.data.title
          this.errors = this.error.response.data.errors
        } else if (typeof this.error === 'object' && 'message' in this.error) {
          this.title = this.error.message
          this.errors = null
        } else {
          this.title = this.error
          this.errors = null
        }
      } else {
        this.title = null
        this.errors = null
      }
    },
  },
}
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
