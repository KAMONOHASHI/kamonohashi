<template>
  <el-form ref="form">
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
          :value="detail && detail.cpu ? String(detail.cpu) : '0'"
        />
        <kqi-display-text-form
          label="メモリ(GB)"
          :value="detail && detail.memory ? String(detail.memory) : '0'"
        />
        <kqi-display-text-form
          label="GPU"
          :value="detail && detail.gpu ? String(detail.gpu) : '0'"
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
          :value="
            detail
              ? detail.displayNameCreatedBy
                ? detail.createdBy + '【' + detail.displayNameCreatedBy + '】'
                : detail.createdBy
              : ''
          "
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
          <el-button icon="el-icon-download" @click="$emit('download')">
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
          @click="$emit('cancel')"
        >
          キャンセル
        </el-button>
        <kqi-delete-button
          class="pull-left btn-update"
          @delete="$emit('remove')"
        />
      </el-col>
    </el-row>
  </el-form>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiDeleteButton from '@/components/KqiDeleteButton'

export default {
  components: {
    KqiDisplayTextForm,
    KqiDeleteButton,
  },
  props: {
    detail: {
      type: Object,
      default: () => {
        return {
          name: '',
          cpu: 0,
          memory: 0,
          gpu: 0,
          nodeName: '',
          displayName: '',
          createdBy: '',
          status: '',
          conditionNote: '',
        }
      },
    },
    events: {
      type: Array,
      default: () => {
        return []
      },
    },
    filename: {
      type: String,
      default: null,
    },
    exists: {
      type: Boolean,
      default: false,
    },
  },
}
</script>

<style scoped>
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

.pull-right {
  float: right !important;
}

.pull-left {
  float: left !important;
}
</style>
