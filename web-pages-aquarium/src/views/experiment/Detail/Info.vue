<template>
  <div>
    <h2>実行条件</h2>
    <div style="width:600px;padding-top:20px;padding-bottom:40px">
      学習の情報を確認できます。
    </div>
    <el-row class="info">
      <el-col :span="3">実行日時</el-col>
      <el-col :span="4">{{ experimentDate }}</el-col>
    </el-row>
    <el-row class="info">
      <el-col :span="3">実行者</el-col>
      <el-col :span="4">{{ userName }}</el-col>
    </el-row>
    <el-row class="info">
      <!-- TODO
        登録したテンプレートをカード形式で表示 -->
      <div class="dashboard">
        <div
          v-for="(template, index) in experimentInfoList"
          :key="index"
          class="card-container"
        >
          <router-link to="/aquarium/model-template/1">
            <el-card
              class="template"
              style="border: solid 1px #ebeef5;  width: 400px; height: 250px;"
            >
              <div class="template-name">
                {{ template.name }}
              </div>

              <div
                class="template-description"
                style="padding: 10px; font-size: 14px;"
              >
                {{ template.memo }}

                <div style="margin:20px 0px 20px 0px">
                  <el-tag type="info" class="tag">
                    {{ template.version }}</el-tag
                  >
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
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: '実験情報',
  components: {},
  data() {
    return {
      importfile: null,
      experimentDate: '2020/12/01 10:00',
      userName: 'UserName',
      experimentInfoList: [
        //TODO 後で消す
        { name: 'データセット', memo: '製造所A部品データ１', version: 'v1' },
        {
          name: 'テンプレート',
          memo: 'A部署異常検知',
          version: 'v1',
        },
      ],
    }
  },
  computed: {
    ...mapGetters(['dataSets', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDataSets']),

    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done() {
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    openCreateDialog() {
      this.$router.push('/dataset/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/dataset/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      await this.retrieveData()
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
.template {
  &:hover {
    transform: scale(1.05);
  }
}

.template-name {
  font-weight: bold;
  padding: 20px 10px;
  font-size: 18px;
}

.template-description {
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
