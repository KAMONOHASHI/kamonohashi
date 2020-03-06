<template>
  <div>
    <el-dialog
      class="dialog"
      title="ファイル一覧"
      :visible.sync="dialogVisible"
      :before-close="emitCancel"
      :close-on-click-modal="false"
    >
      <el-row>
        <el-col :span="6">
          <kqi-display-text-form label="学習名" :value="detail.name" />
        </el-col>
        <el-col :span="6">
          <kqi-display-text-form label="開始日時" :value="detail.createdAt" />
        </el-col>
        <el-col :span="6">
          <kqi-display-text-form label="完了日時" :value="detail.completedAt" />
        </el-col>
        <el-col :span="6">
          <kqi-display-text-form
            label="ステータス"
            :value="detail.statusType"
          />
        </el-col>
      </el-row>

      <kqi-file-viewer :file-list="fileList" @updatePath="updatePath" />
      <el-row :gutter="20" class="footer">
        <el-col class="right-button-group" :span="24">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button @click="emitReturn">戻る</el-button>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script>
import KqiFileViewer from '@/components/KqiFileViewer'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')

export default {
  components: {
    KqiFileViewer,
    KqiDisplayTextForm,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },

  data() {
    return {
      dialogVisible: true,
      error: undefined,
      job: {},
      path: '/',
    }
  },
  computed: {
    ...mapGetters(['fileList', 'detail']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDetail', 'fetchFileList']),
    async retrieveData() {
      await this.fetchDetail(this.id)
      let params = {
        id: this.id,
        path: this.path,
        withUrl: true,
      }
      await this.fetchFileList(params)
    },
    async updatePath(path) {
      this.path = path
      await this.retrieveData()
    },

    emitCancel() {
      this.$emit('cancel')
    },
    emitReturn() {
      this.$emit('return')
    },
  },
}
</script>
<style lang="scss" scoped>
.dialog /deep/ .el-dialog {
  min-width: 800px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}
</style>
