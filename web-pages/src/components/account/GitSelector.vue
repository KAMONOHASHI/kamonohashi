<template>
  <div>
    <el-select
      v-if="gitForm.name"
      v-model="gitForm.name"
      placeholder="Select"
      @change="selectedGitChange"
    >
      <el-option
        v-for="(r, index) in gitForm.gits"
        :key="index"
        :label="r.displayName"
        :value="r.name"
      >
      </el-option>
    </el-select>
    <br />
    <br />
    <pl-display-text-form label="Gitサーバ" :value="gitForm.name" />
    <el-form-item label="トークン">
      <el-input
        v-model="gitForm.token"
        type="password"
        show-password
        @change="tokenChange"
      />
    </el-form-item>
  </div>
</template>

<script>
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
import api from '@/api/v1/api'

export default {
  name: 'GitSelector',
  components: {
    'pl-display-text-form': DisplayTextForm,
  },
  props: {
    value: Object,
  },
  data() {
    return {
      gitForms: [],
      gitForm:
        this.value === undefined || this.value === null ? {} : this.value,
    }
  },
  async created() {
    await this.getGitForms()
  },
  methods: {
    async getGitForms() {
      this.gitForms = (await api.account.getGits()).data
      for (let i = 0; i < this.gitForms.gits.length; i++) {
        if (this.gitForm.defaultGitId === this.gitForms.gits[i].id) {
          this.gitForm.gitId = this.gitForms.gits[i].id
          this.gitForm.name = this.gitForms.gits[i].name
          this.gitForm.serviceType = this.gitForms.gits[i].serviceType
          this.gitForm.token = this.gitForms.gits[i].token
        }
      }
    },
    selectedGitChange() {
      for (let i = 0; i < this.gitForms.gits.length; i++) {
        if (this.gitForm.name === this.gitForms.gits[i].name) {
          this.gitForm.gitId = this.gitForms.gits[i].id
          this.gitForm.serviceType = this.gitForms.gits[i].serviceType
          this.gitForm.token = this.gitForms.gits[i].token
        }
      }
      this.value = this.gitForm
      this.$emit('input', this.value)
    },
    tokenChange(token) {
      this.gitForm.token = token
      this.value = this.gitForm
      for (let i = 0; i < this.gitForms.gits.length; i++) {
        if (this.gitForm.gitId === this.gitForms.gits[i].id) {
          this.gitForms.gits[i].token = this.gitForm.token
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
