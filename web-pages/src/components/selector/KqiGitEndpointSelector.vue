<template>
  <div>
    <el-form-item prop="gitEndpoint">
      <el-col :span="6">Git</el-col>
      <el-select
        class="selectGit"
        :value="value.selectedIds"
        multiple
        placeholder="Select"
        :clearable="true"
        @change="handleChange"
      >
        <el-option
          v-for="item in endpoints"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">
            {{ item.repositoryUrl }}
          </span>
        </el-option>
      </el-select>
      <el-col :span="6">デフォルト</el-col>
      <el-select
        class="selectGit"
        :value="value.defaultId"
        placeholder="Select"
        :clearable="true"
        @change="handleChangeDefaultId"
      >
        <el-option
          v-for="item in availableEndpoints"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">
            {{ item.repositryUrl }}
          </span>
        </el-option>
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
export default {
  props: {
    // 表示するgitエンドポイントの一覧
    endpoints: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択されたgitエンドポイントIDの配列とその中から選んだデフォルトのID
    value: {
      type: Object,
      default: () => {
        return {
          selectedIds: [], // 選択中のgit idの配列
          defaultId: 0, // selectedIdsの中からデフォルトとして設定したgit id
        }
      },
    },
  },
  computed: {
    availableEndpoints: function() {
      // selectedIdsとendpointsを突き合わせて該当するものを抜き出し、表示に用いる配列を作成する。
      let endpointList = []
      this.endpoints.forEach(endpoint => {
        if (this.value.selectedIds.some(id => id === endpoint.id)) {
          endpointList.push(endpoint)
        }
      })
      return endpointList
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
.selectGit {
  width: 100%;
}
</style>
