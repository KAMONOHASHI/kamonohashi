<template>
  <!-- Registryトークン設定 -->
  <div v-if="registries.length <= 0">
    Registryが選択されていません。 システム管理者にお問い合わせください。
  </div>
  <div v-else>
    <el-row class="row-element" style="margin-top: 30px;">
      <el-col :span="6" class="content-color">選択中のRegistry</el-col>
      <el-col :span="16">
        <div>
          <el-select
            :value="value.name"
            placeholder="Select"
            @change="selectedRegistryChange"
          >
            <el-option
              v-for="item in registries"
              :key="item.id"
              :label="item.name"
              :value="item.name"
            />
          </el-select>
          <br />
          <br />

          <div class="content-color">ユーザ名/リポジトリ</div>
          <div>{{ value.userName ? value.userName : '--' }}</div>
          <div class="content-color">トークン</div>
          <el-input
            :value="value.password"
            type="password"
            show-password
            @input="tokenChange"
          />
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col class="button-group">
        <el-button type="primary" @click="$emit('updateRegistryToken')">
          更新
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  props: {
    // レジストリ一覧
    registries: {
      type: Array,
      default: () => {
        return []
      },
    },

    // 選択したレジストリ情報
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

  methods: {
    selectedRegistryChange(registryName) {
      let form = Object.assign({}, this.value)
      form.name = registryName
      for (const data of this.registries) {
        if (data.name === registryName) {
          form.id = data.id
          form.password = data.password
          form.userName = data.userName
        }
      }
      this.$emit('input', form)
    },

    tokenChange(password) {
      let form = Object.assign({}, this.value)
      form.password = password
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
