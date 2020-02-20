<template>
  <div>
    <el-form-item label="レジストリ" prop="registryIds">
      <el-select
        class="selectRegistry"
        :value="selectedIds"
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

    <el-form-item label="デフォルト" prop="defaultRegistryId">
      <el-select
        class="selectRegistry"
        :value="defaultId"
        placeholder="Select"
        :clearable="true"
        @change="handleChangeDefaultId"
      >
        <el-option
          v-for="item in selectedRegistries"
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
  data() {
    return {
      selectedIds: [],
      selectedRegistries: [],
      defaultId: null,
    }
  },
  computed: {
    ...mapGetters(['registries']),
  },
  methods: {
    async handleChange(selectedIds) {
      this.selectedIds = selectedIds
      this.selectedRegistries = []
      this.registries.forEach(registry => {
        if (selectedIds.some(id => id === registry.id)) {
          // 選択中だったらリストに追加
          this.selectedRegistries.push(registry)
        }
      })
      this.$emit('changeSelectedIds', this.selectedIds)
    },
    async handleChangeDefaultId(defaultId) {
      this.defaultId = defaultId
      this.$emit('changeDefaultId', this.defaultId)
    },
  },
}
</script>

<style scoped>
.selectRegistry {
  width: 100%;
}
</style>
