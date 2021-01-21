<template>
  <div>
    <h2>実行条件</h2>
    <div style="width:600px;padding-top:20px;padding-bottom:40px">
      学習の情報を確認できます。
    </div>
    <el-row class="info">
      <el-col :span="3">実行日時</el-col>
      <el-col :span="4">{{ value.createdAt }}</el-col>
    </el-row>
    <el-row class="info">
      <el-col :span="3">実行者</el-col>
      <el-col :span="4">{{ value.createdBy }}</el-col>
    </el-row>
    <el-row class="info">
      <div>
        <div class="card-container">
          <router-link :to="dataSetUrl">
            <el-card
              class="info"
              style="border: solid 1px #ebeef5;  width: 400px; height: 250px;"
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
                <!-- TODO:アクアリウムデータセット名を取得  -->
                <!-- NAME: -->

                <div style="margin:20px 0px 20px 0px">
                  <el-tag type="info" class="tag">
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
          <router-link :to="templateUrl">
            <el-card
              class="info"
              style="border: solid 1px #ebeef5;  width: 400px; height: 250px;"
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
                  <el-tag type="info" class="tag"> v1</el-tag>
                </div>
              </div>
              <div style="padding: 20px; font-size: 14px;">
                <icon name="pl-arrow-right" scale="1.5" class="menu-icon" />
                詳細
              </div>
            </el-card>
          </router-link>
        </div>
      </div>
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
        createdAt: '',
        createdBy: '',
        dataSetId: null,
        dataSetVersion: null,
      }),
    },
  },
  data() {
    return {
      importfile: null,
      dataSetName: '',
      dataSetUrl: '',
      templateUrl: '',
    }
  },
  computed: {
    ...mapGetters(['detail', 'events']),
  },
  async created() {
    await this.initialize()
  },

  methods: {
    ...mapActions([
      'fetchDetail',
      'fetchEvents',
      'postUserCancel',
      'postFiles',
      'put',
      'delete',
      'deleteFile',
    ]),
    async initialize() {
      this.dataSetUrl = '/aquarium/dataset/' + this.detail.dataSet.id
      this.templateUrl = '/aquarium/model-template/' + this.detail.template.id
    },
    handleClick() {},
    async retrieveData() {
      await this.fetchDetail(this.value.id)
      if (
        this.detail.statusType === 'Running' ||
        this.detail.statusType === 'Error'
      ) {
        await this.fetchEvents(this.value.id)
      }
    },
    async handleHalt() {
      try {
        await this.postHalt(this.value.id) // 異常停止（Status=Killed）
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
    async handleUserCancel() {
      try {
        await this.postUserCancel(this.value.id) // 正常停止（Status=UserCanceled）
        await this.retrieveData()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.importfile-detail {
  padding-top: 50px;
}
.importfile-detail > h3 {
  padding-bottom: 10px;
}
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
