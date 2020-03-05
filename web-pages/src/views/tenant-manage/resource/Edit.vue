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
      :detail="form.detail"
      :filename="filename"
      :exists="exists"
      @download="handleDonwloadLog"
      @cancel="handleCancel"
      @remove="handleRemove"
    ></container-info>
  </el-dialog>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import ContainerInfo from '@/views/common/ContainerInfo'
import { mapGetters, mapActions } from 'vuex'

export default {
  components: {
    KqiDisplayError,
    ContainerInfo,
  },
  props: {
    name: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      error: null,
      dialogVisible: true,
      filename: '',
      exists: false,
      form: {
        detail: {},
      },
    }
  },

  computed: {
    ...mapGetters({
      tenantDetail: ['resource/tenantDetail'],
      containerLog: ['resource/containerLog'],
      tenant: ['tenant/detail'],
    }),
  },
  async created() {
    try {
      let params = {
        name: this.name,
      }
      await this['tenant/fetchCurrentTenant']()
      await this['resource/fetchTenantDetail'](params)
      this.tenantDetail.tenantName = this.tenant.name
      this.tenantDetail.displayName = this.tenant.name
      this.form.detail = this.tenantDetail
      this.error = null
    } catch (e) {
      this.error = e
    }
  },

  methods: {
    ...mapActions([
      'tenant/fetchCurrentTenant',
      'resource/fetchTenantDetail',
      'resource/fetchTenantContainerLog',
      'resource/deleteTenantContainer',
    ]),

    async handleDonwloadLog() {
      try {
        let params = {
          name: this.name,
        }
        await this['resource/fetchTenantContainerLog'](params)

        if (this.containerLog !== '') {
          this.exists = true
          if (!this.tenantDetail.nodeName) {
            // ノードが振り分けられていない場合
            this.filename =
              this.tenantDetail.tenantName +
              '_' +
              this.tenantDetail.name +
              '.log'
          } else {
            this.filename =
              this.tenantDetail.nodeName +
              '_' +
              this.tenantDetail.tenantName +
              '_' +
              this.tenantDetail.name +
              '.log'
          }

          let a = document.getElementById('download')
          a.download = this.filename
          a.href =
            'data:application/octet-stream,' +
            encodeURIComponent(this.containerLog)
        } else {
          this.exists = false
          this.filename = '取得可能なログファイルはありません。'
        }

        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleRemove() {
      try {
        let params = {
          name: this.name,
        }
        await this['resource/deleteTenantContainer'](params)
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleCancel() {
      this.emitCancel()
    },

    emitDone() {
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped>
.dialog /deep/ label {
  font-weight: bold !important;
}
</style>
