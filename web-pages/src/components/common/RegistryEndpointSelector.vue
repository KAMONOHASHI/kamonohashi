<template>
  <div>
    <el-select class="selectRegistry" :value="value" @change="handleChange" multiple placeholder="Select" :clearable="true">
      <el-option
        v-for="item in list"
        :key="item.id"
        :label="item.name"
        :value="item.id">
        <span style="float: left">{{ item.name }}</span>
        <span style="float: right; color: #8492a6; font-size: 13px">{{ item.registryPath }}</span>
      </el-option>
    </el-select>
    <label class="el-form-item__label">デフォルト</label>
    <el-select class="selectRegistry" :value="selectedDefaultId" @change="handleChangeDefaultId" placeholder="Select" :clearable="true">
      <el-option
        v-for="item in selectedList"
        :key="item.id"
        :label="item.name"
        :value="item.id">
        <span style="float: left">{{ item.name }}</span>
        <span style="float: right; color: #8492a6; font-size: 13px">{{ item.registryPath }}</span>
      </el-option>
    </el-select>
  </div>
</template>

<script>
import api from '@/api/v1/api'

export default {
  name: 'RegistryEndpointSelector',
  props: ['value', 'defaultId'],
  data () {
    return {
      list: [] // 全レジストリ
    }
  },
  async created () {
    await this.init()
  },
  computed: {
    selectedList: function () { // 現在フォーム内で選択中のレジストリ
      let selectedList = [] // 初期化
      this.list.forEach((reg) => {
        if (this.value.some(r => r === reg.id)) {
          // 選択中だったらリストに追加
          selectedList.push(reg)
        }
      })
      return selectedList
    },
    selectedDefaultId: function () { // 現在フォーム内で選択中のデフォルトレジストリ
      if (this.value.some(r => r === this.defaultId) === false) {
        // デフォルトに指定中のものが選択から外されたら、デフォルトも外す
        this.handleChangeDefaultId(null) // 親にも反映
        return null
      }
      return this.defaultId
    }
  },
  methods: {
    async init () {
      this.list = (await api.registry.admin.get()).data
    },
    async handleChange (v) {
      this.$emit('input', v)
    },
    async handleChangeDefaultId (v) {
      this.$emit('changeDefaultId', v)
    }
  }
}
</script>

<style scoped>
  .selectRegistry {
    width: 100%;
  }
</style>
