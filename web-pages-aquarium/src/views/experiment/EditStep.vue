<template>
  <div>
    <el-row :gutter="20" style="border-bottom:1px solid #CCC">
      <h2>{{ templateName }}</h2>
    </el-row>
    <el-row :gutter="20"><h2>新しいモデルのトレーニング</h2></el-row>
    <el-row :gutter="20">
      <el-col :span="20" style="padding-left:30px">
        <el-form
          ref="createForm"
          :model="form"
          :rules="rules"
          element-loading-background="rgba(255, 255, 255, 0.7)"
        >
          <!-- step 1 -->
          <div class="step-title">
            <div class="el-step__icon is-text ">
              <div class="el-step__icon-inner">1</div>
            </div>
            モデルの定義
          </div>
          <el-form
            ref="form0"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 0px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <kqi-display-error :error="error" />
            <kqi-display-text-form v-if="isEditDialog" label="ID" :value="id" />

            <el-form-item label="モデル名" prop="name">
              <el-input v-model="form.name" @change="step1Disabled = false" />
            </el-form-item>
            <el-button :disabled="step1Disabled" @click="changeName"
              >続行</el-button
            >
          </el-form>
          <!-- step 2 -->
          <div class="step-title">
            <div class="el-step__icon is-text">
              <div class="el-step__icon-inner">2</div>
            </div>
            データセットの選択
          </div>
          <el-form
            ref="form1"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 0px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <el-button :disabled="step2Disabled" @click="drawer = true"
              >データセットを選択</el-button
            >
            <el-form-item label="" prop="dataset">
              <el-input v-model="selectedDataSetVersionName" :disabled="true" />
            </el-form-item>
            <el-drawer
              title="データセットの選択"
              :visible.sync="drawer"
              :direction="direction"
              :before-close="closeDrawer"
            >
              <div style="padding:20px">
                <h3 v-if="listType == 'dataSet'">データセット一覧</h3>
                <h3 v-if="listType == 'version'">
                  <i
                    class="el-icon-arrow-left"
                    style="margin-right:10px;cursor: pointer;"
                    @click="backSelect"
                  ></i
                  >{{ selectedDataSetName }}
                </h3>
                <div
                  style="width:80%;height:450px;padding:20px;border:1px solid #CCC;border-radius:5px;margin-top:5px"
                >
                  <ul v-if="listType == 'dataSet'">
                    <li
                      v-for="item in dataSets"
                      :key="item.id"
                      class="li"
                      style="list-style-type: none;padding-left:10px;cursor: pointer;"
                      @click="selectDataSet(item, $event)"
                    >
                      {{ item.name }}
                    </li>
                  </ul>
                  <ul v-if="listType == 'version'">
                    <li
                      v-for="item in versions"
                      :key="item.id"
                      class="li"
                      style="list-style-type: none;padding-left:10px;cursor: pointer;"
                      @click="selectVersion(item, $event)"
                    >
                      V{{ item.version }}
                    </li>
                  </ul>
                </div>
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
          </el-form>
          <!-- step 3 -->
          <!-- 
          <div class="step-title">
            <div class="el-step__icon is-text">
              <div class="el-step__icon-inner">3</div>
            </div>
            リソースの選択
          </div>
          <el-form
            ref="form2"
            :model="form"
            :rules="rules"
            style="margin:10px;padding: 10px 10px 10px 30px;border-left:2px solid #CCC"
          >
            <el-radio v-model="selectResource" label="1"
              >デフォルトのリソースを使用</el-radio
            ><br />
            <el-radio v-model="selectResource" label="2"
              >手動でリソース量を設定</el-radio
            >

            <el-row v-if="selectResource == 2" style="padding-left:30px">
              <el-col :span="12">
                <kqi-resource-selector v-model="form.resource" :quota="quota" />
              </el-col>
            </el-row>
          </el-form>-->
          <span>
            <el-button type="primary" @click="submit">
              トレーニングを開始する
            </el-button>
          </span>
        </el-form>
      </el-col>
    </el-row>
    <el-row class="step"> </el-row>
  </div>
</template>

<script>
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'

import { mapActions, mapGetters } from 'vuex'
export default {
  components: {
    KqiDisplayError,
    KqiDisplayTextForm,
  },
  props: {
    templateId: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      step1Disabled: true,
      step2Disabled: true,
      oldDataSetE: null,
      oldVersionE: null,
      listType: 'dataSet',
      direction: 'rtl',
      selectedDataSet: null,
      selectedDataSetName: null,
      selectedVersion: null,
      selectedVersionName: null,
      selectedDataSetVersionName: null,
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

        resource: {
          cpu: 1,
          memory: 1,
          gpu: 0,
        },
      },
      title: '',
      dialogVisible: true,
      error: null,
      isCreateDialog: false,
      isEditDialog: false,

      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
    }
  },
  computed: {
    ...mapGetters({
      versions: ['aquariumDataSet/versions'],
      dataSets: ['aquariumDataSet/dataSets'],

      templateDetail: ['template/detail'],
    }),
  },
  watch: {
    async $route() {
      // 通常の作成とコピー作成が同一コンポーネントのため、コピー作成の実行はrouterの変化により検知する
      await this.initialize()
    },
  },
  async created() {
    await this.initialize()
  },
  methods: {
    ...mapActions([
      'template/fetchDetail',
      'template/post',
      'template/put',
      'template/delete',
      'experiment/post',
      'aquariumDataSet/fetchDataSets',
      'aquariumDataSet/fetchVersions',
    ]),
    async initialize() {
      await this['template/fetchDetail'](this.templateId)

      this.templateName = this.templateDetail.name
      //アクアリウムデータセットリストを取得
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this['aquariumDataSet/fetchDataSets'](params)
    },
    async next() {
      let form = null
      switch (this.active) {
        case 0:
          form = this.$refs.form0
          break
        case 1:
          form = this.$refs.form1
          break
        case 2:
          form = this.$refs.form2
          break
      }
      await form.validate(async valid => {
        if (valid) {
          this.active++
        }
      })
    },
    changeName() {
      if (this.form.name != null && this.form.name != '') {
        this.step2Disabled = false
      }
    },
    previous() {
      if (this.active-- < 0) {
        this.active = 0
      }
    },

    closeDrawer() {
      this.listType = 'dataSet'
      this.selectedVersion = null
      this.selectedVersionName = null
      this.selectedDataSet = null
      this.selectedDataSetName = null
      this.drawer = false
      this.selectedDataSetVersionName = null
    },
    //バージョン一覧から戻るをクリック
    backSelect() {
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
          this.listType = 'version'
          await this['aquariumDataSet/fetchVersions'](this.selectedDataSet.id)
          this.selectedVersionName = null
        }
      } else if (this.listType == 'version') {
        if (this.selectedVersionName != null) {
          this.listType = 'dataSet'
          this.drawer = false
          this.selectedDataSetVersionName =
            'dataset id:' +
            this.selectedDataSet.id +
            ' dataset name:' +
            this.selectedDataSetName +
            ' version:' +
            this.selectedVersionName
        } else {
          this.selectedDataSet = null
          this.selectedVersionName = null
          this.listType = 'dataSet'
        }
        this.listType = 'dataSet'
      }
    },
    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              dataSetId: this.selectedVersion.aquariumDataSetId,
              dataSetVersionId: this.selectedVersion.id,
              templateId: this.templateId,
              options: null,
            }
            await this['experiment/post'](params)
            this.$router.push('/aquarium/experiment')
            this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
  },
}
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.left-step-group {
  text-align: left;
  float: left;
  z-index: 2;
}
.right-step-group {
  text-align: right;
  float: right;
  z-index: 2;
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
