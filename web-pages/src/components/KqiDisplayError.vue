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

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'
interface DataType {
  title: string | Error | null
  errors: Array<Error> | null
}
export default Vue.extend({
  props: {
    error: {
      type: Object as PropType<Error | any>,
      default: null,
    },
  },

  data(): DataType {
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
})
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
