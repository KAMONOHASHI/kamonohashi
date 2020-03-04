<template>
  <el-dialog
    class="dialog"
    title="コンテナ情報"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-form ref="form">
      <kqi-display-error :error="error" />

      <el-row>
        <el-col :span="8">
          <kqi-display-text-form
            label="コンテナ名"
            :value="detail ? detail.name : ''"
          />
        </el-col>
        <el-col :offset="1" :span="7">
          <kqi-display-text-form
            label="CPU"
            :value="detail ? String(detail.cpu) : '0'"
          />
          <kqi-display-text-form
            label="メモリ(GB)"
            :value="detail ? String(detail.memory) : '0'"
          />
          <kqi-display-text-form
            label="GPU"
            :value="detail ? String(detail.gpu) : '0'"
          />
        </el-col>
        <el-col :offset="1" :span="7">
          <kqi-display-text-form
            label="ノード"
            :value="detail ? detail.nodeName : ''"
          />
          <kqi-display-text-form
            label="テナント"
            :value="detail ? detail.displayName : ''"
          />
          <kqi-display-text-form
            label="ユーザ"
            :value="detail ? detail.createdBy : ''"
          />
        </el-col>
      </el-row>

      <h3>コンテナ実行結果</h3>
      <el-card>
        <kqi-display-text-form
          label="ステータス"
          :value="detail ? detail.status : ''"
        />
        <div v-if="detail && detail.conditionNote !== ``" class="k8s-event">
          {{ detail.conditionNote }}
        </div>
        <div v-if="events.length" class="k8s-event">
          <el-collapse accordion>
            <el-collapse-item title="ステータス詳細ログ">
              <div v-for="(event, index) in events" :key="index">
                <div v-if="event.isError">message:{{ event.message }}</div>
              </div>
            </el-collapse-item>
          </el-collapse>
        </div>

        <el-form-item v-if="detail && detail.nodeName" label="ログ">
          <br clear="all" />
          <span v-if="!filename">
            <el-button icon="el-icon-download" @click="handleDonwloadLog">
              取得
            </el-button>
          </span>
          <span>
            {{ filename }}
            <a id="download">
              <el-button
                v-if="filename && exists"
                icon="el-icon-download"
                size="mini"
              />
            </a>
          </span>
        </el-form-item>
      </el-card>

      <el-row>
        <el-col class="button-group">
          <el-button
            class="pull-right btn-cancel"
            icon="el-icon-close"
            @click="handleCancel"
          >
            キャンセル
          </el-button>
          <kqi-delete-button
            class="pull-left btn-update"
            @delete="handleRemove"
          />
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiDeleteButton from '@/components/KqiDeleteButton.vue'
import { createNamespacedHelpers } from 'vuex'

const { mapGetters, mapActions } = createNamespacedHelpers('resource')

export default {
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiDeleteButton,
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
  data() {
    return {
      dialogVisible: true,
      error: null,
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
        this.error = e
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
          tenantId: this.id,
          name: this.name,
        }
        await this.delete(params)
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
      this.showSuccessMessage()
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style lang="scss" scoped>
.k8s-event {
  color: #909399;
}

.button-group {
  text-align: right;
  padding-top: 10px;
}

.btn-update {
  margin-left: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.pull-right {
  float: right !important;
}

.pull-left {
  float: left !important;
}
</style>
