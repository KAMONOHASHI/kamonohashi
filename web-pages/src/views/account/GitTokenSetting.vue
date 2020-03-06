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
            :value="value.name"
            placeholder="Select"
            @change="selectedGitChange"
          >
            <el-option
              v-for="item in gits"
              :key="item.id"
              :label="item.name"
              :value="item.name"
            />
          </el-select>
          <br />
          <br />
          <div class="content-color">Gitサーバ</div>
          <div>{{ value.name }}</div>
          <div class="content-color">トークン</div>
          <el-input
            :value="value.token"
            type="password"
            show-password
            @input="tokenChange"
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
    // git一覧
    gits: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択したgit情報
    value: {
      type: Object,
      default: () => ({
        id: 0,
        name: '',
        token: '',
      }),
    },
  },
  methods: {
    selectedGitChange(gitName) {
      let form = Object.assign({}, this.value)
      form.name = gitName

      for (const data of this.gits) {
        if (data.name === gitName) {
          form.id = data.id
          form.token = data.token
        }
      }
      this.$emit('input', form)
    },

    tokenChange(token) {
      let form = Object.assign({}, this.value)
      form.token = token
      this.$emit('input', form)
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
  padding-top: 100px;
  padding-right: 30px;
}

.content-color {
  color: #606266;
}
</style>
