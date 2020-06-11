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
          <div v-if="deleteButtonParams.isDanger">
            <kqi-danger-button
              :warning-text="deleteButtonParams.warningText"
              :confirm-text="deleteButtonParams.confirmText"
              @delete="emitDelete"
            />
          </div>
          <div v-else>
            <kqi-delete-button
              v-if="!disabledParams.deleteButton"
              @delete="emitDelete"
            />
          </div>
        </el-col>
        <el-col class="right-button-group">
          <el-button @click="emitClose">キャンセル</el-button>
          <el-button
            v-if="!disabledParams.submitButton"
            type="primary"
            @click="emitSubmit"
          >
            保存
          </el-button>
        </el-col>
      </div>
    </el-row>
  </el-dialog>
</template>

<script>
import KqiDeleteButton from '@/components/KqiDeleteButton'
import KqiDangerButton from '@/components/KqiDangerButton'

export default {
  components: {
    KqiDeleteButton,
    KqiDangerButton,
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
    disabledParams: {
      type: Object,
      default() {
        return {
          deleteButton: false,
          submitButton: false,
        }
      },
    },
    deleteButtonParams: {
      type: Object,
      default() {
        return {
          isDanger: false,
          warningText: null,
          confirmText: null,
        }
      },
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
  float: initial;
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
