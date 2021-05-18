<template>
  <div>
    <h2>実験情報</h2>
    <el-row style="padding-top:20px;padding-bottom:40px">
      <el-col :span="8">学習の情報を確認できます。</el-col>
      <el-col :span="6">
        <aqualium-tensorboard-handler
          :id="String(id)"
          :visible="tesorboardVisible"
        />
      </el-col>
      <el-col :span="6"
        ><el-button plain @click="deleteExperimentDialog = true"
          >実験削除</el-button
        ></el-col
      >
      <el-dialog title="" :visible.sync="deleteExperimentDialog" width="30%">
        <span>この実験を削除しますか？</span>
        <span slot="footer" class="dialog-footer">
          <el-button @click="deleteExperimentDialog = false">Cancel</el-button>
          <el-button type="primary" @click="deleteExperiment()">
            削除する
          </el-button>
        </span>
      </el-dialog>
    </el-row>

    <el-card
      v-if="value != null"
      style="margin: 10px;border: solid 1px #ebeef5;  width: 550px; height: 300px;"
    >
      <el-row class="">
        <el-col :span="8">
          <div class="info-name">
            概要
          </div>
        </el-col>
      </el-row>
      <div style="padding: 10px; font-size: 14px;">
        <el-row class="">
          <el-col :span="8">
            <div style="margin:20px 0px 20px 0px">実験名</div>
          </el-col>
          <el-col :span="16">
            <div style="margin:20px 0px 20px 0px">
              {{ value.name }}
            </div>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <div style="margin:10px 0px 10px 0px">
              ステータス
            </div>
          </el-col>
          <el-col
            v-if="
              value.status == 'None' && value.experimentPreprocessHistoryId > 0
            "
            :span="16"
          >
            <el-tag :type="tagType(value.preprocessStatus)" class="tag">
              Preprocess:{{ value.preprocessStatus }}
            </el-tag>
          </el-col>
          <el-col v-else :span="16">
            <el-tag :type="tagType(value.status)" class="tag">
              Training:{{ value.status }}
            </el-tag>
          </el-col>
        </el-row>

        <el-row>
          <el-col :span="8">実行日時</el-col>
          <el-col :span="16">{{ value.createdAt }}</el-col>
        </el-row>
        <el-row>
          <el-col :span="8">完了日時</el-col>
          <el-col :span="16">{{ value.completedAt }}</el-col>
        </el-row>
        <el-row>
          <el-col :span="8">実行者</el-col>
          <el-col :span="16">{{ value.createdBy }}</el-col>
        </el-row>
      </div>
    </el-card>

    <el-row v-if="value != null">
      <el-col :span="12">
        <router-link :to="value.dataSetURL">
          <el-card
            class="info"
            style="border: solid 1px #ebeef5;  width: 550px; height: 280px;"
          >
            <div class="info-name">
              データセット
            </div>

            <div
              class="info-description"
              style="padding: 10px; font-size: 14px;"
            >
              ID: {{ value.dataSetId }}
              <br />
              NAME: {{ value.dataSetName }}
              <!-- NAME: -->
              <div style="margin:20px 0px 20px 0px">
                バージョン<el-tag type="info" class="tag">
                  v{{ value.dataSetVersion }}</el-tag
                >
              </div>
            </div>

            <div style="padding: 20px; font-size: 14px;">
              <icon name="pl-arrow-right" scale="1.5" class="menu-icon" />
              詳細
            </div>
          </el-card>
        </router-link>
      </el-col>
      <el-col :span="12">
        <div style="cursor: pointer;" @click="templateClick()">
          <el-card
            class="info"
            style="border: solid 1px #ebeef5;  width: 550px; height: 280px;"
          >
            <div class="info-name">
              テンプレート
            </div>

            <div
              class="info-description"
              style="padding: 10px; font-size: 14px;"
            >
              ID: {{ value.templateId }}
              <br />
              NAME:{{ value.templateName }}
              <div style="margin:20px 0px 20px 0px">
                バージョン<el-tag type="info" class="tag">
                  v{{ value.templateVersion }}</el-tag
                >
              </div>
            </div>
            <div style="padding: 20px; font-size: 14px;">
              <icon name="pl-arrow-right" scale="1.5" class="menu-icon" />
              詳細
            </div>
          </el-card>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import AqualiumTensorboardHandler from './AqualiumTensorboardHandler.vue'
import { mapActions, mapGetters } from 'vuex'
export default {
  title: '実験情報',
  components: { AqualiumTensorboardHandler },
  props: {
    // 選択した実験情報

    id: {
      type: String,
      default: null,
    },

    value: {
      type: Object,
      default: () => ({
        id: null,
        name: '',
        createdAt: '',
        createdBy: '',
        dataSetId: null,
        dataSetVersion: null,
        dataSetURL: '',
        templateURL: '',
      }),
    },
  },
  data() {
    return {
      tesorboardVisible: true,
      deleteExperimentDialog: false,
    }
  },
  computed: {
    ...mapGetters({ detail: ['experiment/detail'] }),
  },

  methods: {
    ...mapActions([
      'experiment/fetchDetail',
      'experiment/postUserCancel',
      'experiment/postFiles',
      'experiment/put',
      'experiment/delete',
      'experiment/deleteFile',
      'cluster/fetchQuota',
    ]),
    async templateClick() {
      let err = false
      try {
        await this['cluster/fetchQuota']()
      } catch (e) {
        err = true
      } finally {
        if (err == false) {
          //エラーが無い場合はテンプレート詳細に遷移する
          this.$router.push(this.value.templateURL)
        } else {
          //エラーがある場合は元の画面に遷移してエラーメッセージを出す
          this.$router.push('/aquarium/experiment/detail/' + this.value.id)
          this.$notify.error({
            title: '権限がありません',
            message:
              'この実験に使用されたテンプレートの詳細にアクセスする権限がありません',
            duration: 0,
          })
        }
      }
    },
    deleteExperiment() {
      this['experiment/delete'](this.detail.id)
    },
    tagType(val) {
      let tag = null
      if (val == 'Running' || val == 'Opened') {
        tag = ''
      } else if (val == 'Completed') {
        tag = 'success'
      } else if (val == 'None' || val == 'Empty') {
        tag = 'info'
      } else if (val == 'Killed' || val == 'UserCanceled' || val == 'Empty') {
        tag = 'warning'
      } else if (
        val == 'Invalid' ||
        val == 'Forbidden' ||
        val == 'Failed' ||
        val == 'Invalid' ||
        val == 'Error'
      ) {
        tag = 'danger'
      }
      return tag
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
.info {
  margin: 10px;
}
a {
  text-decoration: none;
}
.tag {
  border-radius: 15px;
  padding-left: 20px;
  padding-right: 20px;
}
.info {
  &:hover {
    transform: scale(1.05);
  }
}

.info-name {
  font-weight: bold;
  padding: 20px 10px;
  font-size: 18px;
}

.info-description {
  font-weight: lighter;
  border-bottom: 1px solid #ebeef5;
}

.model-template {
  padding-top: 80px;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: space-evenly;
  align-content: flex-start;
}
.card-container {
  float: left;
  margin: 20px 20px 10px 0;
}
</style>
