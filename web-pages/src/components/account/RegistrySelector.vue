<template>
  <div>
    <el-select v-model="registryForm.name" placeholder="Select"
               v-if="registryForm.name"
               @change="selectedRegistryChange"
    >
      <el-option
        v-for="(r, index) in registryForm.registries"
        :key="index"
        :label="r.displayName"
        :value="r.name">
      </el-option>
    </el-select>
    <br/>
    <br/>
    <pl-display-text-form label="ユーザ名/リポジトリ" :value="registryForm.userName"/>
    <el-form-item label="パスワード">
      <el-input @change="tokenChange" v-model="registryForm.password" type="password" show-password/>
    </el-form-item>
  </div>
</template>

<script>
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'RegistrySelector',
    components: {
      'pl-display-text-form': DisplayTextForm
    },
    props: {
      value: Object
    },
    data () {
      return {
        registryForms: [],
        registryForm: this.value === undefined || this.value === null
          ? {}
          : this.value
      }
    },
    async created () {
      await this.getRegistryForms()
    },
    methods: {
      async getRegistryForms () {
        this.registryForms = (await api.account.getRegistries()).data
        for (let i = 0; i < this.registryForms.registries.length; i++) {
          if (this.registryForms.defaultRegistryId === this.registryForms.registries[i].id) {
            this.registryForm.registryId = this.registryForms.registries[i].id
            this.registryForm.userName = this.registryForms.registries[i].userName
            this.registryForm.password = this.registryForms.registries[i].password
          }
        }
      },
      selectedRegistryChange () {
        for (let i = 0; i < this.registryForms.registries.length; i++) {
          if (this.registryForm.name === this.registryForms.registries[i].name) {
            this.registryForm.registryId = this.registryForms.registries[i].id
            this.registryForm.userName = this.registryForms.registries[i].userName
            this.registryForm.password = this.registryForms.registries[i].password
          }
        }
        this.value = this.registryForm
        this.$emit('input', this.value)
      },
      tokenChange (password) {
        this.registryForm.password = password
        this.value = this.registryForm
        for (let i = 0; i < this.registryForms.registries.length; i++) {
          if (this.registryForm.registryId === this.registryForms.registries[i].id) {
            this.registryForms.registries[i].password = this.registryForm.password
          }
        }
        this.$emit('input', this.value)
      }
    }
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
