<template>
  <div>
    <el-form-item label="レジストリ" prop="registryIds">
      <el-select
        class="selectRegistry"
        :value="value"
        multiple
        placeholder="Select"
        :clearable="true"
        @change="handleChange"
      >
        <el-option
          v-for="item in list"
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
        :value="selectedDefaultId"
        placeholder="Select"
        :clearable="true"
        @change="handleChangeDefaultId"
      >
        <el-option
          v-for="item in selectedList"
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
import api from '@/api/v1/api'

export default {
  name: 'RegistryEndpointSelector',
  props: ['value', 'defaultId', 'tenantId'],
  data() {
    return {
      list: [], // 全レジストリ
    }
  },
  computed: {
    selectedList: function() {
      // 現在フォーム内で選択中のレジストリ
      let selectedList = [] // 初期化
      this.list.forEach(reg => {
        if (this.value.some(r => r === reg.id)) {
          // 選択中だったらリストに追加
          selectedList.push(reg)
        }
      })
      return selectedList
    },
    selectedDefaultId: function() {
      // 現在フォーム内で選択中のデフォルトレジストリ
      if (this.value.some(r => r === this.defaultId) === false) {
        // デフォルトに指定中のものが選択から外されたら、デフォルトも外す
        this.handleChangeDefaultId(null) // 親にも反映
        return null
      }
      return this.defaultId
    },
  },
  watch: {
    async tenantId() {
      await this.init()
    },
  },
  async created() {
    await this.init()
  },
  methods: {
    async init() {
      if (this.tenantId) {
        this.list = (
          await api.registry.tenant.getEndpoints({ id: this.tenantId })
        ).data
      } else {
        this.list = (await api.registry.admin.get()).data
      }
    },
    async handleChange(v) {
      this.$emit('input', v)
    },
    async handleChangeDefaultId(v) {
      this.$emit('changeDefaultId', v)
    },
  },
}
</script>

<style scoped>
.selectRegistry {
  width: 100%;
}
</style>
