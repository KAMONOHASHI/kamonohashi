<template>
  <!-- Registryトークン設定 -->
  <div id="force_tab01" class="cp_tabpanel">
    <div v-if="registries.length <= 0">
      Registryが選択されていません。 システム管理者にお問い合わせください。
    </div>
    <div v-else>
      <el-row>
        <el-col :span="8">選択中のRegistry</el-col>
        <el-col :span="16">
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
        </el-col>
        <br />
        <br />
        <el-col>ユーザ名/リポジトリ</el-col>
        <el-col :offset="8" :span="16">{{ registryForm.userName }}</el-col>
        <el-col>パスワード</el-col>
        <el-col :offset="8" :span="16">
          <el-input
            v-model="registryForm.password"
            type="password"
            show-password
            @change="tokenChange"
          />
        </el-col>
      </el-row>
      <el-col class="button-group">
        <el-button type="primary" @click="$emit('updateRegistryToken')">
          更新
        </el-button>
      </el-col>
    </div>
  </div>
</template>

<script>
export default {
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
