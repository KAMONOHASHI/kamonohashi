<template>
  <div>
    <el-form-item label="レジストリ" prop="registryselectedIds">
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
    </el-form-item>

    <el-form-item label="デフォルト" prop="registryDefaultId">
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
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('registry')

export default {
  props: {
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
    ...mapGetters(['registries']),
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
