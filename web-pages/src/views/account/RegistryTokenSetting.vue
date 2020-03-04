<template>
  <div>
    <el-select
      v-if="registryForm.name"
      v-model="registryForm.name"
      placeholder="Select"
      @change="selectedRegistryChange"
    >
      <el-option
        v-for="(r, index) in registries"
        :key="index"
        :label="r.displayName"
        :value="r.name"
      />
    </el-select>
    <br />
    <br />
    <kqi-display-text-form
      label="ユーザ名/リポジトリ"
      :value="registryForm.userName"
    />
    <el-form-item label="パスワード">
      <el-input
        v-model="registryForm.password"
        type="password"
        show-password
        @change="tokenChange"
      />
    </el-form-item>
  </div>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'

export default {
  components: {
    KqiDisplayTextForm,
  },
  props: {
    registries: {
      type: Array,
      default: () => {
        return []
      },
    },

    value: {
      type: Object,
      default: () => ({
        id: 0,
        name: '',
        userName: '',
        password: '',
      }),
    },
  },
  computed: {
    registryForm() {
      return this.value === undefined || this.value === null ? {} : this.value
    },
  },
  methods: {
    selectedRegistryChange() {
      for (const data of this.registries) {
        if (this.registryForm.name === data.name) {
          this.registryForm.id = data.id
          this.registryForm.userName = data.userName
          this.registryForm.password = data.password
        }
      }
      this.value = this.registryForm
    },

    tokenChange(password) {
      this.registryForm.password = password
      this.value = this.registryForm
      for (const data of this.registries) {
        if (this.registryForm.id === data.id) {
          data.password = this.registryForm.password
        }
      }
      this.$emit('input', this.value)
    },
  },
}
</script>

<style lang="scss" scoped>
.el-select {
  width: 100% !important;
}

.el-input {
  text-align: right;
}
</style>
