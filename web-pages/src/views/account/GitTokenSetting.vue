<template>
  <!-- Gitトークン設定 -->
  <div v-if="gits.length <= 0">
    Gitリポジトリが選択されていません。 システム管理者にお問い合わせください。
  </div>
  <div v-else>
    <el-row class="row-element" style="margin-top: 30px;">
      <el-col :span="6" class="content-color">選択中のGitリポジトリ</el-col>
      <el-col :span="16">
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
          <div class="content-color">Gitサーバ</div>
          <div>{{ value.name }}</div>
          <div class="content-color">トークン</div>
          <el-input
            v-model="gitForm.token"
            type="password"
            show-password
            @change="tokenChange"
          />
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col class="button-group">
        <el-button type="primary" @click="$emit('updateGitToken')">
          更新
        </el-button>
      </el-col>
    </el-row>
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

.row-element {
  font-size: 14px;
  line-height: 40px;
  margin-top: 30px;
  font-weight: bold !important;
}

.button-group {
  text-align: right;
  padding-top: 150px;
  padding-right: 30px;
}

.content-color {
  color: #606266;
}
</style>
