<template>
  <!-- Gitトークン設定 -->
  <div id="third_tab01" class="cp_tabpanel">
    <div v-if="gits.length <= 0">
      Gitリポジトリが選択されていません。 システム管理者にお問い合わせください。
    </div>
    <div v-else>
      <el-row>
        <el-col :span="8">選択中のGitリポジトリ</el-col>
        <el-col :span="16">
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
        </el-col>
        <br />
        <br />
        <el-col>Gitサーバ</el-col>
        <el-col :offset="8" :span="16">{{ value.name }}</el-col>
        <el-col>トークン</el-col>
        <el-col :offset="8" :span="16">
          <el-input
            v-model="gitForm.token"
            type="password"
            show-password
            @change="tokenChange"
          />
        </el-col>
      </el-row>
      <el-col class="button-group">
        <el-button type="primary" @click="$emit('updateGitToken')">
          更新
        </el-button>
      </el-col>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    gits: {
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
        token: '',
      }),
    },
  },
  computed: {
    gitForm() {
      return this.value === undefined || this.value === null ? {} : this.value
    },
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
