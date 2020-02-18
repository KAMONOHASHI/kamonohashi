<template>
  <el-dialog
    class="dialog"
    :title="title"
    :visible.sync="dialogVisible"
    :before-close="emitClose"
    :close-on-click-modal="false"
  >
    <slot></slot>
    <el-row class="footer">
      <div v-if="type === 'CREATE'" class="right-button-group">
        <el-button @click="emitClose">キャンセル</el-button>
        <el-button type="primary" @click="emitSubmit">
          {{ submitText }}
        </el-button>
      </div>
      <div v-else>
        <el-col :span="12">
          <kqi-delete-button :disabled="disabled" @delete="emitDelete" />
        </el-col>
        <el-col :span="12" class="right-button-group">
          <el-button @click="emitClose">キャンセル</el-button>
          <el-button type="primary" :disabled="disabled" @click="emitSubmit"
            >保存</el-button
          >
        </el-col>
      </div>
    </el-row>
  </el-dialog>
</template>

<script>
import KqiDeleteButton from '@/components/KqiDeleteButton'

export default {
  components: {
    KqiDeleteButton,
  },
  props: {
    title: {
      type: String,
      default: 'Dialog Title',
    },
    type: {
      type: String,
      default: 'CREATE',
    },
    submitText: {
      type: String,
      default: '登録',
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return { dialogVisible: true }
  },
  methods: {
    emitClose() {
      this.$emit('close')
    },
    emitDelete() {
      this.$emit('delete')
    },
    emitSubmit() {
      this.$emit('submit')
    },
  },
}
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.footer {
  padding-top: 40px;
}
</style>
