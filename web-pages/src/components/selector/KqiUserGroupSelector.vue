<template>
  <el-form-item label="ユーザグループ情報" prop="userGroupId">
    <div class="left-margin">
      <el-select
        class="selectUserGroup"
        :value="value"
        multiple
        placeholder="Select"
        :clearable="true"
        @change="handleChange"
      >
        <el-option
          v-for="item in userGroups"
          :key="item.id"
          :label="item.name"
          :value="item.id"
        >
          <span style="float: left;">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px;">
            {{ item.dn }}
          </span>
        </el-option>
      </el-select>
    </div>
  </el-form-item>
</template>

<script>
export default {
  props: {
    // 表示するuseGroupsの一覧
    userGroups: {
      type: Array,
      default: () => {
        return []
      },
    },
    // userGroupsの中から選択したid
    value: {
      type: Number,
      default: null,
    },
  },
  methods: {
    async handleChange(userGroupId) {
      if (userGroupId === '') {
        this.$emit('input', null)
      } else {
        this.$emit('input', userGroupId)
      }
    },
  },
}
</script>

<style scoped>
.selectUserGroup {
  width: 100%;
}
.left-margin {
  padding-left: 30px;
}
</style>
