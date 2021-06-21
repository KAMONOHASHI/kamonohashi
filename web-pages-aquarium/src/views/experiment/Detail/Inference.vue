<template>
  <div>
    <h2>推論一覧</h2>
    作成したAIの評価の履歴一覧例です。<br />
    ※表示データは体験版用ダミーデータです。
    <el-row>
      <el-col :span="18">
        <el-table :data="evaluations" style="width: 100%">
          <el-table-column prop="id" label="ID" width="180"> </el-table-column>

          <el-table-column
            prop="name"
            label="推論名"
            width="180"
          ></el-table-column>
          <el-table-column
            prop="dataSet.name"
            label="データセット名"
            width="180"
          >
          </el-table-column>
          <el-table-column
            prop="dataSetVersion.version"
            label="データセットversion"
            width="180"
          >
          </el-table-column>
          <el-table-column prop="status" label="結果" width="180">
          </el-table-column>
          <el-table-column prop="tensorboards" label="tensorboards" width="180">
            <template slot-scope="scope">
              <aqualium-tensorboard-handler
                :id="String(scope.row.training.id)"
                :visible="tesorboardVisible"
              />
            </template>
          </el-table-column>

          <el-table-column prop="delete" label="削除">
            <template slot-scope="scope">
              <el-button size="mini" @click="deleteDialog = true"
                >削除する</el-button
              >
              <el-dialog title="" :visible.sync="deleteDialog" width="30%">
                <span>ID:{{ scope.row.id }}の推論を削除しますか？</span>
                <span slot="footer" class="dialog-footer">
                  <el-button @click="deleteDialog = false">Cancel</el-button>
                  <el-button
                    type="primary"
                    @click="deleteInference(scope.$index, scope.row)"
                  >
                    削除
                  </el-button>
                </span>
              </el-dialog>
            </template>
          </el-table-column>
        </el-table>
      </el-col>
    </el-row>
    <el-row>
      <el-col> </el-col>
      <el-button
        type="primary"
        style="margin-top:20px"
        @click="createDialog = true"
      >
        別のデータで推論を実行
      </el-button>
      <el-dialog title="新規推論実行" :visible.sync="createDialog" width="30%">
        <el-form
          ref="createForm"
          v-loading="loading"
          :model="form"
          :rules="rules"
          element-loading-spinner=" "
          element-loading-background="rgba(255, 255, 255, 0.7)"
        >
          <el-form-item label="名前" prop="name">
            <el-input v-model="form.name" />
          </el-form-item>
          <el-form-item label="データセット" prop="name">
            <el-input
              v-model="form.selectedDataSetVersionName"
              :disabled="true"
            />
            <el-button @click="drawer = true">データセットを選択</el-button>
          </el-form-item>
        </el-form>

        <span slot="footer" class="dialog-footer">
          <el-button @click="createDialog = false">Cancel</el-button>
          <el-button type="primary" @click="createInference()">
            実行
          </el-button>
        </span>
      </el-dialog>
      <el-drawer
        title="データセットの選択"
        :visible.sync="drawer"
        :direction="direction"
        :before-close="closeDrawer"
      >
        <div style="overflow:auto;padding:20px;height:100%">
          <h3 v-show="listType == 'dataSet'">データセット一覧</h3>
          <h3 v-show="listType == 'version'">
            <i
              class="el-icon-arrow-left"
              style="margin-right:10px;cursor: pointer;"
              @click="backSelect"
            ></i
            >{{ selectedDataSetName }}
          </h3>
          <div
            style="overflow:auto;width:80%;height:60%;padding:20px;border:1px solid #CCC;border-radius:5px;margin-top:5px"
          >
            <ul v-show="listType == 'dataSet'">
              <li
                v-for="itemD in dataSets"
                :key="itemD.id"
                class="li"
                style="list-style-type: none;padding-left:10px;cursor: pointer;"
                @click="selectDataSet(itemD, $event)"
              >
                id:{{ itemD.id }}, name:{{ itemD.name }}
              </li>
            </ul>
            <ul v-show="listType == 'version'">
              <li
                v-for="itemV in versions"
                :key="itemV.id"
                class="li"
                style="list-style-type: none;padding-left:10px;cursor: pointer;"
                @click="selectVersion(itemV, $event)"
              >
                V{{ itemV.version }}
              </li>
            </ul>
          </div>
          <el-row>
            <kqi-pagination
              v-show="listType == 'dataSet'"
              v-model="pageStatus"
              :total="total"
              @change="retrieveData"
          /></el-row>
          <el-row>
            <el-button
              type="plain"
              plain
              style="margin-top:10px"
              @click="select"
            >
              選択
            </el-button>
            <el-button
              type="plain"
              plain
              style="margin-top:10px"
              @click="closeDrawer"
            >
              キャンセル
            </el-button></el-row
          >
        </div>
      </el-drawer>
    </el-row>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AqualiumTensorboardHandler from './AqualiumTensorboardHandler.vue'
export default {
  title: '推論',
  components: { AqualiumTensorboardHandler },
  props: {
    // 選択した実験情報
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      tesorboardVisible: true,
      loading: false,
      oldDataSetE: null,
      oldVersionE: null,
      listType: 'dataSet',
      direction: 'rtl',
      selectedDataSet: null,
      selectedDataSetName: null,
      selectedVersion: null,
      selectedVersionName: null,
      searchCondition: {},
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      drawer: false,
      templateName: null,
      selectResource: 0,
      form: {
        name: null,
        selectedDataSetVersionName: null,
      },
      title: '',
      dialogVisible: true,
      error: null,
      isCreateDialog: false,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
        selectedDataSetVersionName: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
        templateVersionValue: [
          { required: true, trigger: 'blur', message: '必須項目です' },
        ],
      },
      deleteDialog: false,
      createDialog: false,
      importfile: null,
      inferenceList: [
        {
          id: '1',
          name: '名前',
          dataSet: { name: '体験版用ダミー01' },
          dataSetVersion: { version: 1 },
          status: '完了',
          training: { id: 1 },
        },
      ],
    }
  },
  computed: {
    ...mapGetters({
      versions: ['aquariumDataSet/versions'],
      dataSets: ['aquariumDataSet/dataSets'],
      total: ['aquariumDataSet/total'],
      evaluations: ['experiment/evaluations'],
    }),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'template/fetchDetail',
      'template/fetchModelTemplate',
      'template/fetchVersions',
      'template/post',
      'template/put',
      'template/delete',
      'experiment/post',
      'aquariumDataSet/fetchDataSets',
      'aquariumDataSet/fetchVersions',
      'experiment/fetchEvaluations',
      'experiment/postEvoluations',
      'experiment/deleteEvoluations',
    ]),
    async createInference() {
      //推論を新規作成
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              dataSetId: this.selectedDataSet.id,
              dataSetVersionId: this.selectedVersion.id,
            }
            await this['experiment/postEvoluations']({
              id: this.id,
              params: params,
            })
            //this.$router.push('/aquarium/experiment')
            //this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async deleteInference(idx, row) {
      //推論を削除
      await this['experiment/deleteEvoluations']({
        id: this.id,
        evaluationId: row.id,
      })
    },
    async retrieveData() {
      //アクアリウムデータセットリストを取得
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this['aquariumDataSet/fetchDataSets'](params)
      //推論一覧を取得
      await this['experiment/fetchEvaluations'](this.id)
    },
    closeDrawer() {
      this.listType = 'dataSet'
      this.selectedVersion = null
      this.selectedVersionName = null
      this.selectedDataSet = null
      this.selectedDataSetName = null
      this.drawer = false
      this.form.selectedDataSetVersionName = null
    },
    //バージョン一覧から戻るをクリック
    backSelect() {
      if (this.oldVersionE) {
        this.oldVersionE.target.classList.remove('active-li')
      }
      if (this.oldDataSetE) {
        this.oldDataSetE.target.classList.remove('active-li')
      }
      this.listType = 'dataSet'
      this.selectedVersion = null
      this.selectedVersionName = null
      this.selectedDataSet = null
      this.selectedDataSetName = null
    },
    //データセット一覧からデータセットを選択
    selectDataSet(item, e) {
      //元のactiveを外す
      if (this.oldDataSetE) {
        this.oldDataSetE.target.classList.remove('active-li')
      }
      e.target.classList.add('active-li')
      this.oldDataSetE = e
      this.selectedDataSet = item
      this.selectedDataSetName = item.name
    },
    selectVersion(item, e) {
      //元のactiveを外す
      if (this.oldVersionE) {
        this.oldVersionE.target.classList.remove('active-li')
      }
      e.target.classList.add('active-li')
      this.oldVersionE = e
      this.selectedVersion = item
      this.selectedVersionName = item.version
    },
    //選択ボタン押下
    async select() {
      if (this.listType == 'dataSet') {
        if (this.selectedDataSet.id != null) {
          await this['aquariumDataSet/fetchVersions'](this.selectedDataSet.id)
          this.selectedVersionName = null

          this.listType = 'version'
        }
      } else if (this.listType == 'version') {
        if (this.selectedVersionName != null) {
          this.form.selectedDataSetVersionName =
            'dataset id:' +
            this.selectedDataSet.id +
            ' dataset name:' +
            this.selectedDataSetName +
            ' version:' +
            this.selectedVersionName

          this.drawer = false

          this.listType = 'dataSet'
        }
      }
    },

    open() {
      this.$alert('推論機能は製品版で使用可能予定です', 'お知らせ', {
        confirmButtonText: 'OK',
      })
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

.step-title {
  font-weight: bold !important;
  color: #409eff;
  border-color: #409eff;
}
.active-li {
  color: #409eff;
  background-color: #40a0ff25;
}
</style>
