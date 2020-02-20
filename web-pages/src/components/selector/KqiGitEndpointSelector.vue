<template>
  <div>
    <el-form-item label="Git" prop="gitIds">
      <el-select
        class="selectGit"
        :value="selectedIds"
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
          <span style="float: right; color: #8492a6; font-size: 13px">{{
            item.repositoryUrl
          }}</span>
        </el-option>
      </el-select>
    </el-form-item>

    <el-form-item label="デフォルト" prop="defaultGitId">
      <el-select
        class="selectGit"
        :value="defaultId"
        placeholder="Select"
        :clearable="true"
        @change="handleChangeDefaultId"
      >
        <el-option
          v-for="item in selectedEndpoints"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{
            item.repositryUrl
          }}</span>
        </el-option>
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('git')

export default {
  data() {
    return {
      selectedIds: [],
      selectedEndpoints: [],
      defaultId: null,
    }
  },
  computed: {
    ...mapGetters(['endpoints']),
  },
  methods: {
    async handleChange(selectedIds) {
      this.selectedIds = selectedIds
      this.selectedEndpoints = []
      this.endpoints.forEach(endpoint => {
        if (selectedIds.some(id => id === endpoint.id)) {
          // 選択中だったらリストに追加
          this.selectedEndpoints.push(endpoint)
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
.selectGit {
  width: 100%;
}
</style>
