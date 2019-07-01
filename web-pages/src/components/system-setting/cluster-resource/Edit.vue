<template>
  <el-dialog class="dialog"
             title="コンテナ情報"
             :visible="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form ref="form">
      <pl-display-error :error="error"/>

      <el-row>
        <el-col :span="8">
          <pl-display-text label="コンテナ名" :value="form.name"/>
        </el-col>
        <el-col :offset="1" :span="7">
          <pl-display-text label="CPU" :value="form.cpu"/>
          <pl-display-text label="メモリ(GB)" :value="form.memory"/>
          <pl-display-text label="GPU" :value="form.gpu"/>
        </el-col>
        <el-col :offset="1" :span="7">
          <pl-display-text label="ノード" :value="form.node"/>
          <pl-display-text label="テナント" :value="form.displayName"/>
          <pl-display-text label="ユーザ" :value="form.user"/>
        </el-col>
      </el-row>
      <h3>コンテナ実行結果</h3>

      <el-card>
        <pl-display-text label="ステータス" :value="form.status"/>
        <div class="k8s-event" v-if="conditionNote !== `` ">{{ conditionNote}}</div>
        <div class="k8s-event" v-if="events.length">
          <el-collapse accordion>
            <el-collapse-item title="ステータス詳細ログ">
              <div v-for="(event, index) in events" :key="index">
                <div v-if="event.isError">
                  message:{{ event.message}}
                </div>
              </div>
            </el-collapse-item>
          </el-collapse>
        </div>

        <el-form-item label="ログ">
          <br clear="all"/>
          <span v-if="!filename">
            <el-button icon="el-icon-download" @click="handleDonwloadLog">取得</el-button>
          </span>
          <span>
            {{filename}}
            <a id="download">
              <el-button v-if="filename" icon="el-icon-download" size="mini"></el-button>
            </a>
          </span>
        </el-form-item>
      </el-card>

      <el-row>
        <el-col class="button-group">
          <el-button @click="handleCancel" class="pull-right btn-cancel" icon="el-icon-close">キャンセル</el-button>
          <pl-delete-button class="pull-left btn-update" @delete="handleRemove"/>
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
      'pl-delete-button': DeleteButton
    },
    props: {
      'id': String,
      'name': String
    },
    data () {
      return {
        dialogVisible: true,
        error: null,
        filename: '',
        conditionNote: '',
        events: [],
        form: {
          node: '',
          tenant: '',
          displayName: '',
          user: '',
          id: '',
          name: '',
          status: '',
          cpu: '',
          memory: '',
          gpu: '',
          activeName: '1'
        }
      }
    },

    async created () {
      await this.changeId()
    },

    watch: {
      async dataName () {
        await this.changeId()
      }
    },

    methods: {
      async changeId () {
        try {
          let params = {
            tenantId: this.id,
            name: this.name
          }
          let data = (await api.resource.admin.getContainerByName(params)).data
          this.form.node = data.nodeName
          this.form.tenant = data.tenantName
          this.form.displayName = data.displayName
          this.form.user = data.createdBy
          this.form.name = data.name
          this.form.status = data.status
          this.form.cpu = data.cpu
          this.form.memory = data.memory
          this.form.gpu = data.gpu
          this.conditionNote = data.conditionNote
          if (this.statusType !== 'Closed' && this.statusType !== 'Failed' && this.statusType !== 'None') {
            this.events = (await api.resource.admin.getContainerEventsByName(params)).data
          }
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleDonwloadLog () {
        try {
          let params = {
            tenantId: this.id,
            name: this.name
          }
          let content = (await api.resource.admin.getContainerLogByName(params)).data

          if (!this.form.node) {
            // ノードが振り分けられていない場合
            this.filename = this.form.tenant + '_' + this.form.name + '.log'
          } else {
            this.filename = this.form.node + '_' + this.form.tenant + '_' + this.form.name + '.log'
          }

          let a = document.getElementById('download')
          a.download = this.filename
          a.href = 'data:application/octet-stream,' + encodeURIComponent(content)
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleRemove () {
        try {
          let params = {
            tenantId: this.id,
            name: this.name
          }
          await api.resource.admin.deleteContainerByName(params)
          this.emitDone()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
      },

      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      }
    }
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
    font-weight: bold !important
  }

  .pull-right {
    float: right !important;
  }

  .pull-left {
    float: left !important;
  }

</style>
