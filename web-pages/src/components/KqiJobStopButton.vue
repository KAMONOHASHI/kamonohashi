<template>
  <span>
    <el-button type="danger" @click="dialogVisible = true">
      <span v-if="buttonLabel">{{ buttonLabel }}</span>
      <span v-else class="el-icon-delete" />
      <el-dialog
        id="dialog"
        :title="title"
        :visible.sync="dialogVisible"
        width="20%"
        center
        append-to-body
      >
        <br />
        <div style="text-align: center;">
          <el-button @click="dialogVisible = false">キャンセル</el-button>
          <el-button type="danger" @click="$emit('halt')">異常停止</el-button>
          <el-button type="success" @click="$emit('userCancel')">
            正常停止
          </el-button>
        </div>
      </el-dialog>
    </el-button>
  </span>
</template>

<script>
export default {
  props: {
    title: { type: String, default: 'title' },
    buttonLabel: { type: String, default: '' },
  },
  data() {
    return {
      dialogVisible: false,
    }
  },
}
</script>

<style lang="scss" scoped>
#dialog {
  margin-top: 300px;
}
</style>
