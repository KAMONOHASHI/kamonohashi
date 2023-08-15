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
      :detail="containerInfo"
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
import { mapGetters, mapActions } from 'vuex'

import * as gen from '@/api/api.generate'
interface DataType {
  error: null | Error
  dialogVisible: boolean
  filename: string
  exists: boolean
  containerInfo: NssolPlatypusApiModelsResourceApiModelsContainerDetailsForTenantOutputModel
}
type NssolPlatypusApiModelsResourceApiModelsContainerDetailsForTenantOutputModel = gen.NssolPlatypusApiModelsResourceApiModelsContainerDetailsForTenantOutputModel & {
  tenantName?: string
  displayName?: string
}

export default Vue.extend({
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
  data(): DataType {
    return {
      error: null,
      dialogVisible: true,
      filename: '',
      exists: false,
      containerInfo: {},
    }
  },

  computed: {
    ...mapGetters({
      //@ts-ignore
      account: ['account/account'],
      tenantDetail: ['resource/tenantDetail'],
      tenantContainerLog: ['resource/tenantContainerLog'],
    }),
  },
  async created() {
    try {
      await this['account/fetchAccount']()
      let params = {
        name: this.name,
      }
      await this['resource/fetchTenantDetail'](params)
      this.containerInfo = this.tenantDetail
      this.containerInfo.tenantName = this.account.selectedTenant.name
      this.containerInfo.displayName = this.account.selectedTenant.displayName
      this.error = null
    } catch (e) {
      if (e instanceof Error) this.error = e
    }
  },

  methods: {
    ...mapActions([
      'account/fetchAccount',
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

        if (this.tenantContainerLog !== '') {
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
          //@ts-ignore
          a!.download = this.filename
          //@ts-ignore
          a!.href =
            'data:application/octet-stream,' +
            encodeURIComponent(this.tenantContainerLog)
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
          name: this.name,
        }
        await this['resource/deleteTenantContainer'](params)
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
