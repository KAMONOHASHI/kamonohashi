<template>
  <div>
    <el-form-item label="レジストリ" prop="registryIds">
      <el-select
        class="selectRegistry"
        :value="registryIds"
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
  props: {
    registryIds: {
      type: Array,
      default: () => [],
    },
    defaultId: {
      type: Number,
      default: null,
    },
  },
  data() {
    return {
      selectedRegistries: [],
    }
  },
  computed: {
    ...mapGetters(['registries']),
  },
  watch: {
    async defaultId() {
      await this.selectedDefaultIds(this.registryIds)
    },
  },
  methods: {
    async handleChange(selectedIds) {
      this.selectedDefaultIds(selectedIds)
      this.$emit('changeSelectedIds', selectedIds)
    },
    async selectedDefaultIds(ids) {
      this.selectedRegistries = []
      this.registries.forEach(registry => {
        if (ids.some(id => id === registry.id)) {
          // 選択中だったらリストに追加
          this.selectedRegistries.push(registry)
        }
      })
    },
    async handleChangeDefaultId(defaultId) {
      if (defaultId === '') {
        this.$emit('changeDefaultId', { value: null })
      } else {
        this.$emit('changeDefaultId', { value: defaultId })
      }
    },
  },
}
</script>

<style scoped>
.selectRegistry {
  width: 100%;
}
</style>
