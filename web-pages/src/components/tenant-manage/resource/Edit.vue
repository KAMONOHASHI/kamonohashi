<template>
  <el-dialog
    class="dialog"
    title="コンテナ情報"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-form ref="form">
      <pl-display-error :error="error" />

      <el-row>
        <el-col :span="8">
          <pl-display-text label="コンテナ名" :value="form.name" />
        </el-col>
        <el-col :offset="1" :span="7">
          <pl-display-text label="CPU" :value="form.cpu" />
          <pl-display-text label="メモリ(GB)" :value="form.memory" />
          <pl-display-text label="GPU" :value="form.gpu" />
        </el-col>
        <el-col :offset="1" :span="7">
          <pl-display-text label="ノード" :value="form.node" />
          <pl-display-text label="テナント" :value="form.tenant" />
          <pl-display-text label="ユーザ" :value="form.user" />
        </el-col>
      </el-row>
      <h3>コンテナ実行結果</h3>
      <el-card>
        <pl-display-text label="ステータス" :value="form.status" />
        <div v-if="conditionNote !== ``" class="k8s-event">
          {{ conditionNote }}
        </div>
        <el-form-item label="ログ">
          <br clear="all" />
          <span v-if="!filename">
            <el-button icon="el-icon-download" @click="handleDownloadLog"
              >取得</el-button
            >
          </span>
          <span>
            {{ filename }}
            <a id="download">
              <el-button
                v-if="filename"
                icon="el-icon-download"
                size="mini"
              ></el-button>
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
            >キャンセル</el-button
          >
          <pl-delete-button
            class="pull-left btn-update"
            @delete="handleRemove"
          />
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import api from '@/api/v1/api'
import DisplayError from '@/components/common/DisplayError'
import DeleteButton from '@/components/common/DeleteButton.vue'
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'

export default {
  name: 'UserEdit',
  components: {
    'pl-display-error': DisplayError,
    'pl-display-text': DisplayTextForm,
    'pl-delete-button': DeleteButton,
  },
  props: {
    dataId: {
      type: Number,
      default: 0,
    },
  },
  data() {
    return {
      dialogVisible: true,
      error: null,
      filename: '',
      conditionNote: '',
      form: {
        node: '',
        tenant: '',
        user: '',
        id: '',
        name: '',
        status: '',
        cpu: '',
        memory: '',
        gpu: '',
      },
    }
  },

  watch: {
    async dataName() {
      await this.changeId()
    },
  },

  async created() {
    await this.changeId()
  },

  methods: {
    async changeId() {
      try {
        let params = {
          name: this.dataId.name,
        }
        let data = (await api.resource.tenant.getContainerByName(params)).data

        this.form.node = data.nodeName
        this.form.user = data.createdBy
        this.form.name = data.name
        this.form.status = data.status
        this.form.cpu = data.cpu
        this.form.memory = data.memory
        this.form.gpu = data.gpu
        this.conditionNote = data.conditionNote
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleDownloadLog() {
      try {
        let params = {
          name: this.dataId.name,
        }
        let content = (await api.resource.tenant.getContainerLogByName(params))
          .data
        this.filename =
          this.form.node +
          '_' +
          this.form.tenant +
          '_' +
          this.form.name +
          '.log'

        let a = document.getElementById('download')
        a.download = this.filename
        a.href = 'data:application/octet-stream,' + encodeURIComponent(content)
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleRemove() {
      try {
        let params = {
          name: this.dataId.name,
        }
        await api.resource.tenant.deleteContainerByName(params)
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
