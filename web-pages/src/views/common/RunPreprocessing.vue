<template>
  <el-dialog
    class="dialog"
    title="前処理実行"
    :visible.sync="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <el-form ref="preprocessingForm" :rules="rules" :model="form">
      <kqi-display-error :error="error" />
      <el-row :gutter="20">
        <el-col :span="12">
          <div v-if="enableDataSelection">
            <el-form-item label="データ" prop="data">
              <div class="el-input">
                <el-select
                  v-model="form.selectedDataId"
                  multiple
                  placeholder="Select Data"
                  filterable
                  value-key="id"
                  remote
                  clearable
                >
                  <el-option
                    v-for="item in data"
                    :key="item.id"
                    :label="item.name"
                    :value="item.id"
                  >
                    <span style="float: left">{{ item.name }}</span>
                    <span
                      style="float: right; margin-right:16px; color: #8492a6; font-size: 13px"
                      >{{ item.memo }}</span
                    >
                  </el-option>
                </el-select>
              </div>
            </el-form-item>
          </div>
          <div v-else>
            <kqi-display-text-form
              label="データID"
              :value="idArray"
            ></kqi-display-text-form>
          </div>

          <kqi-preprocessings-selector
            v-model="form.preprocessingId"
            :preprocessings="preprocessings"
            @input="onPreprocessingChanged"
          />

          <kqi-environment-variables v-model="form.variables" />
          <kqi-partition-selector
            v-model="form.partition"
            :partitions="partitions"
          />
        </el-col>

        <el-col :span="12">
          <kqi-resource-selector
            v-model="form.resource"
          ></kqi-resource-selector>
          <el-form-item label="オプション">
            <br />
            <el-checkbox v-model="form.movePreprocessingPage" size="medium"
              >実行後に履歴を確認する</el-checkbox
            >
          </el-form-item>
        </el-col>
      </el-row>

      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="runPreprocessing">実行</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiPreprocessingsSelector from '@/components/selector/KqiPreprocessingSelector'
import KqiPartitionSelector from '@/components/selector/KqiPartitionSelector'
import KqiResourceSelector from '@/components/selector/KqiResourceSelector'
import KqiEnvironmentVariables from '@/components/KqiEnvironmentVariables'

import { mapActions, mapGetters } from 'vuex'

export default {
  components: {
    KqiPartitionSelector,
    KqiResourceSelector,
    KqiEnvironmentVariables,
    KqiPreprocessingsSelector,
    KqiDisplayTextForm,
    KqiDisplayError,
  },
  props: {
    idArray: {
      type: String,
      default: null,
    },
  },
  data() {
    let dataSelectedIdsValidator = (rule, value, callback) => {
      if (this.enableDataSelection && this.form.selectedDataId.length === 0) {
        callback(new Error('必須項目です'))
      } else {
        callback()
      }
    }

    let preprocessingIdValidator = (rule, value, callback) => {
      if (this.form.preprocessingId === null)
        callback(new Error('必須項目です'))
      else {
        callback()
      }
    }

    return {
      rules: {
        preprocessing: [
          {
            required: true,
            trigger: 'blur',
            validator: preprocessingIdValidator,
          },
        ],
        data: [
          {
            required: true,
            trigger: 'blur',
            validator: dataSelectedIdsValidator,
          },
        ],
      },
      form: {
        selectedDataId: [],
        preprocessingId: null,
        resource: {
          cpu: 1,
          memory: 1,
          gpu: 0,
        },
        variables: [{ key: '', value: '' }],
        partition: null,
        movePreprocessingPage: true,
      },
      enableDataSelection: false,
      dialogVisible: true,
      error: null,
    }
  },
  computed: {
    ...mapGetters({
      preprocessings: ['preprocessing/preprocessings'],
      partitions: ['cluster/partitions'],
      data: ['data/data'],
    }),
  },
  async created() {
    // データ管理画面からの呼び出しの場合、propsのidArrayが格納されている
    // idArrayが格納されていない場合、前処理管理画面からの呼び出し
    if (this.idArray === null) {
      this.enableDataSelection = true
      await this['data/fetchData']()
    }
    await this['cluster/fetchPartitions']()
    await this['preprocessing/fetchPreprocessings']()
  },
  methods: {
    ...mapActions([
      'cluster/fetchPartitions',
      'data/fetchData',
      'data/put',
      'preprocessing/fetchPreprocessings',
      'preprocessing/runById',
    ]),
    async runPreprocessing() {
      let form = this.$refs.preprocessingForm
      await form.validate(async valid => {
        if (valid) {
          let selectedIdList = []
          if (this.idArray === null) {
            // 前処理管理画面からの呼び出しの場合、selectedDataIdを利用
            selectedIdList = this.form.selectedDataId
          } else {
            // データ管理画面からの呼び出しの場合、idArray文字列の分割
            selectedIdList = this.idArray.split(' ')
            if (selectedIdList.length > 1) {
              selectedIdList.pop() // 複数IDの場合、最後の空要素を削除
            }
          }

          // 環境変数の作成
          let options = {}
          // apiのフォーマットに合わせる(配列 => オブジェクト)
          this.form.variables.forEach(kvp => {
            options[kvp.key] = kvp.value
          })

          selectedIdList.forEach(async dataId => {
            try {
              let params = {
                dataId: dataId,
                options: options,
                cpu: this.form.resource.cpu,
                memory: this.form.resource.memory,
                gpu: this.form.resource.gpu,
                partition: this.form.partition,
              }
              await this['preprocessing/runById']({
                id: this.form.preprocessingId,
                params: params,
              })
              await this.$notify.success({
                title: 'Success',
                message: `ID:${dataId}の前処理に成功しました`,
              })
              // 成功した場合、使用したデータのisRawフラグをFalseにする
              await this.updateData(dataId)
              this.error = null
            } catch (e) {
              this.error = e
            }
          })
          await this.sleep(1000)
          // エラーがない場合、前処理履歴画面に遷移
          if (this.form.movePreprocessingPage && this.error === null) {
            this.emitHistoryPage(this.form.preprocessingId)
          } else if (this.error === null) {
            this.emitDone()
          } else {
            this.$notify.error({
              title: 'Error',
              message: '前処理に失敗しました',
            })
          }
        }
      })
    },
    async updateData(id) {
      let params = {
        id: id,
        model: {
          isRaw: false,
        },
      }
      await this['data/put'](params)
    },
    sleep(ms) {
      return new Promise(resolve => setTimeout(resolve, ms))
    },

    onPreprocessingChanged() {
      // リソースサイズのデフォルト値を選択された前処理の値とする
      let preprocessing = this.preprocessings.find(
        preprocessing => String(preprocessing.id) === this.form.preprocessingId,
      )
      this.form.resource.cpu = preprocessing.cpu
      this.form.resource.memory = preprocessing.memory
      this.form.resource.gpu = preprocessing.gpu
    },

    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.$emit('done')
      this.dialogVisible = false
    },
    emitHistoryPage(id) {
      this.$router.push('/preprocessingHistory/' + id)
      this.$store.commit('setLoading', false)
    },
    closeDialog(done) {
      done()
      this.$emit('close')
    },
  },
}
</script>

<style scoped>
.dialog /deep/ .el-dialog {
  min-width: 800px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}

.el-select {
  width: 100% !important;
}
</style>
