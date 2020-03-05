<template>
  <div>
    <el-form-item prop="registry">
      <el-col :span="6">レジストリ</el-col>
      <el-select
        class="selectRegistry"
        :value="value.selectedIds"
        multiple
        placeholder="Select"
        :clearable="true"
        @change="handleChange"
      >
        <el-option
          v-for="item in registries"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{
            item.registryPath
          }}</span>
        </el-option>
      </el-select>

      <el-col :span="6">デフォルト</el-col>
      <el-select
        class="selectRegistry"
        :value="value.defaultId"
        placeholder="Select"
        :clearable="true"
        @change="handleChangeDefaultId"
      >
        <el-option
          v-for="item in availableRegistries"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{
            item.registryPath
          }}</span>
        </el-option>
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
export default {
  props: {
    // 表示するレジストリの一覧
    registries: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択されたレジストリIDの配列とその中から選んだデフォルトのID
    value: {
      type: Object,
      default: () => {
        return {
          selectedIds: [], // 選択中のregistry idの配列
          defaultId: 0, // selectedIdsの中からデフォルトとして設定したregistry id
        }
      },
    },
  },
  computed: {
    availableRegistries: function() {
      // selectedIdsとendpointsを突き合わせて該当するものを抜き出し、表示に用いる配列を作成する。
      let registryList = []
      this.registries.forEach(registry => {
        if (this.value.selectedIds.some(id => id === registry.id)) {
          registryList.push(registry)
        }
      })
      return registryList
    },
  },
  methods: {
    async handleChange(selectedIds) {
      let updateValue = this.value
      updateValue.selectedIds = selectedIds
      // selectedIdsに含まれないものがdefaultIdに指定されていた場合はdefaultIdをリセット
      if (!selectedIds.some(id => id === updateValue.defaultId)) {
        updateValue.defaultId = null
      }
      this.$emit('input', updateValue)
    },
    async handleChangeDefaultId(defaultId) {
      let updateValue = this.value
      if (defaultId === '') {
        updateValue.defaultId = null
      } else {
        updateValue.defaultId = defaultId
      }
      this.$emit('input', updateValue)
    },
  },
}
</script>

<style scoped>
.selectRegistry {
  width: 100%;
}
</style>
