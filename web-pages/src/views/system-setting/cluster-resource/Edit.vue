<template>
  <el-dialog
    class="dialog"
    title="コンテナ情報"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <kqi-display-error :error="error" />

    <container-info
      :detail="detail"
      :events="events"
      :filename="filename"
      :exists="exists"
      @download="handleDonwloadLog"
      @cancel="handleCancel"
      @remove="handleRemove"
    />
  </el-dialog>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import ContainerInfo from '@/views/common/ContainerInfo.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('resource')

interface DataType {
  error: null | Error
  dialogVisible: boolean
  filename: string
  exists: boolean
}

export default Vue.extend({
  components: {
    KqiDisplayError,
    ContainerInfo,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
    name: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    return {
      error: null,
      dialogVisible: true,
      filename: '',
      exists: false,
    }
  },

  computed: {
    ...mapGetters(['detail', 'events', 'containerLog']),
  },
  async created() {
    await this.changeId()
  },

  methods: {
    ...mapActions(['fetchDetail', 'fetchContainerLog', 'delete']),
    async changeId() {
      try {
        let params = {
          tenantId: this.id,
          name: this.name,
        }
        await this.fetchDetail(params)
        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },

    async handleDonwloadLog() {
      try {
        let params = {
          tenantId: this.id,
          name: this.name,
        }
        await this.fetchContainerLog(params)

        if (this.containerLog !== '') {
          this.exists = true
          if (!this.detail.nodeName) {
            // ノードが振り分けられていない場合
            this.filename =
              this.detail.tenantName + '_' + this.detail.name + '.log'
          } else {
            this.filename =
              this.detail.nodeName +
              '_' +
              this.detail.tenantName +
              '_' +
              this.detail.name +
              '.log'
          }

          let a = document.getElementById('download')
          a!.download = this.filename
          a!.href =
            'data:application/octet-stream,' +
            encodeURIComponent(this.containerLog)
        } else {
          this.exists = false
          this.filename = '取得可能なログファイルはありません。'
        }

        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },

    async handleRemove() {
      try {
        let params = {
          tenantId: this.id,
          name: this.name,
        }
        await this.delete(params)
        this.emitDone()
        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },

    async handleCancel() {
      this.emitCancel()
    },

    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
    },
  },
})
</script>

<style lang="scss" scoped>
.dialog /deep/ label {
  font-weight: bold !important;
}
</style>
