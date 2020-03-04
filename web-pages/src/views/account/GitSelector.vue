<template>
  <div>
    <el-select
      v-if="gitForm.name"
      v-model="gitForm.name"
      placeholder="Select"
      @change="selectedGitChange"
    >
      <el-option
        v-for="(r, index) in gits"
        :key="index"
        :label="r.displayName"
        :value="r.name"
      />
    </el-select>
    <br />
    <br />
    <kqi-display-text-form label="Gitサーバ" :value="gitForm.name" />
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
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('gitSelector')

export default {
  components: {
    KqiDisplayTextForm,
  },
  props: {
    value: {
      type: Object,
      default: () => ({
        id: 0,
        name: '',
        token: '',
      }),
    },
  },
  data() {
    return {
      gitForm:
        this.value === undefined || this.value === null ? {} : this.value,
    }
  },
  computed: {
    ...mapGetters(['gits', 'git']),
  },
  created() {
    for (const data of this.gits) {
      if (this.git.id === data.id) {
        this.gitForm.id = data.id
        this.gitForm.name = data.name
        this.gitForm.token = data.token
      }
    }
  },
  methods: {
    selectedGitChange() {
      for (const data of this.gits) {
        if (this.gitForm.name === data.name) {
          this.gitForm.id = data.id
          this.gitForm.token = data.token
        }
      }
      this.value = this.gitForm
    },

    tokenChange(token) {
      this.gitForm.token = token
      this.value = this.gitForm
      for (const data of this.gits) {
        if (this.gitForm.id === data.id) {
          data.token = this.gitForm.token
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
