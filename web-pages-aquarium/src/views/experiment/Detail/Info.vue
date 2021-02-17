<template>
  <div>
    <h2>実験情報</h2>
    <div style="width:600px;padding-top:20px;padding-bottom:40px">
      学習の情報を確認できます。
    </div>
    <el-card
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

    <el-row>
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
        <router-link :to="value.templateURL">
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
        </router-link>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('experiment')

export default {
  title: '実験情報',
  components: {},
  props: {
    // 選択した実験情報
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

  computed: {
    ...mapGetters(['detail']),
  },

  methods: {
    ...mapActions([
      'fetchDetail',
      'postUserCancel',
      'postFiles',
      'put',
      'delete',
      'deleteFile',
    ]),
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
