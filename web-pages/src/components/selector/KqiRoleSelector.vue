<template>
  <span>
    <el-select
      class="selectTenant"
      :value="value"
      multiple
      placeholder="Select"
      :disabled="isDisabled"
      @change="handleChange"
    >
      <template v-for="item in roles">
        <el-option
          v-if="item.isSystemRole === showSystemRole"
          :key="item.id"
          :label="item.displayName"
          :value="item.id"
        />
      </template>
    </el-select>
  </span>
</template>

<script>
export default {
  props: {
    // 選択したロールIDの配列
    value: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択肢となるロール
    roles: {
      type: Array,
      default: () => {
        return []
      },
    },
    // システムロールの表示かどうか。
    // trueの場合: isSystemRoleがtrueのものを表示
    // falseの場合: isSystemRoleがfalseのものを表示
    showSystemRole: {
      type: Boolean,
      default: true,
    },
    // 非活性かどうか
    isDisabled: {
      type: Boolean,
      default: () => {
        return false
      },
    },
  },
  methods: {
    async handleChange(selectedRoleIds) {
      let updateValue = this.value
      if (selectedRoleIds === '') {
        this.$emit('input', null)
      } else {
        updateValue = selectedRoleIds
        this.$emit('input', updateValue)
      }
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}

.el-select ::v-deep .el-select__tags-text {
  max-width: 35vw;
  overflow: hidden;
  text-overflow: ellipsis;
  display: inline-block;
  vertical-align: middle;
}
</style>
