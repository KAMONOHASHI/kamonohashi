<template>
  <el-dialog class="dialog"
             title="前処理実行"
             :visible.sync="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">
    <el-form ref="preprocessingForm"
             :rules="rules"
             :model="this"
             element-loading-spinner=""
             v-loading="loading"
             element-loading-background="rgba(255, 255, 255, 0.7)">
      <pl-display-error :error="error"/>
      <el-row :gutter="20">
        <el-col :span="12">
          <pl-display-text label="データID" :value="id"></pl-display-text>
          <el-form-item label="前処理" prop="preprocessing">
            <pl-preprocessings-selector v-model="preprocessing" v-on:input="onPreprocessingChanged"/>
          </el-form-item>

          <el-form-item label="環境変数">
            <pl-dynamic-multi-input v-model="options"/>
          </el-form-item>
          <el-form-item label="パーティション" prop="partition">
            <pl-string-selector v-if="partitions"
                                v-model="partition"
                                :valueList="partitions"
            />
          </el-form-item>
        </el-col>

        <el-col :span="12">
          <el-form-item label="CPU" required>
            <el-slider
              class="el-input"
              v-model="cpu"
              :min="1"
              :max="200"
              show-input>
            </el-slider>
          </el-form-item>

          <el-form-item label="メモリ(GB)" required>
            <el-slider
              class="el-input"
              v-model="memory"
              :min="1"
              :max="200"
              show-input>
            </el-slider>
          </el-form-item>

          <el-form-item label="GPU" required>
            <el-slider
              class="el-input"
              v-model="gpu"
              :min="0"
              :max="16"
              show-input>
            </el-slider>
          </el-form-item>
          <el-form-item label="オプション">
            <br/>
            <el-checkbox v-model="checked" size="medium">実行後に履歴を確認する</el-checkbox>
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
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DynamicMultiInputField from '@/components/common/DynamicMultiInputField.vue'
  import DisplayError from '@/components/common/DisplayError'
  import PreprocessingsSelector from '@/components/common/PreprocessingSelector.vue'
  import StringSelector from '@/components/common/StringSelector.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'DataPreprocessing',
    components: {
      'pl-preprocessings-selector': PreprocessingsSelector,
      'pl-string-selector': StringSelector,
      'pl-display-text': DisplayTextForm,
      'pl-dynamic-multi-input': DynamicMultiInputField,
      'pl-display-error': DisplayError
    },
    props: {
      id: String
    },
    data () {
      return {
        rules: {
          preprocessing: [{
            required: true,
            trigger: 'blur',
            validator (rule, value, callback) {
              let exists = false
              for (let key in value) {
                if (value[key].length > 0) {
                  exists = true
                }
              }
              if (exists) {
                callback()
              } else {
                callback(new Error('必須項目です'))
              }
            }
          }]
        },
        dialogVisible: true,
        error: undefined,
        loading: false,
        preprocessing: undefined,
        cpu: undefined,
        memory: undefined,
        gpu: undefined,
        options: undefined,
        partitions: undefined,
        partition: undefined,
        multiSelectedIdList: [],
        updataDataId: undefined,
        preprocessingHistoryIndex: [],
        preprocessingHistoryIdList: [],
        message: undefined,
        checked: true
      }
    },
    async created () {
      this.partitions = (await api.cluster.getPartitions()).data
    },
    methods: {
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.$emit('done')
        this.dialogVisible = false
      },
      emitHistoryPage (id) {
        this.$router.push('/preprocessingHistory/' + id)
        this.$store.commit('setLoading', false)
      },
      closeDialog (done) {
        done()
        this.$emit('close')
      },
      splitmultiSelection (id) {
        this.multiSelectedIdList = id.split(' ')
      },
      onPreprocessingChanged (preprocessing) {
        // リソースサイズをデフォルトで埋める
        this.cpu = preprocessing.cpu
        this.memory = preprocessing.memory
        this.gpu = preprocessing.gpu
      },
      sleep (ms) {
        return new Promise(resolve => setTimeout(resolve, ms))
      },
      async updateData () {
        // 独自ローディング処理のため共通側は無効
        this.$store.commit('setLoading', false)
        this.loading = false
        let params = {
          id: this.updataDataId,
          model: {
            isRaw: false
          }
        }
        await
          api.data.putById(params)
      },
      async runPreprocessing () {
        let form = this.$refs.preprocessingForm
        this.$forceUpdate()
        await form.validate(async (valid) => {
          if (valid) {
            this.error = undefined
            // まず、データIDを分割する
            this.splitmultiSelection(this.id)
            this.$store.commit('setLoading', false)
            this.loading = true
            for (let i = 0; i < this.multiSelectedIdList.length; i++) {
              if (this.multiSelectedIdList[i] === '') {
                //  配列の最後の要素が空になるので、それ以外の要素を順番に前処理をかける
              } else {
                try {
                  this.updataDataId = this.multiSelectedIdList[i]
                  let options = {}
                  // apiのフォーマットに合わせる(配列 => オブジェクト)
                  this.options.forEach((kvp) => {
                    options[kvp.key] = kvp.value
                  })
                  let param = {
                    dataId: this.updataDataId,
                    options: options,
                    cpu: this.cpu,
                    memory: this.memory,
                    gpu: this.gpu,
                    partition: this.partition
                  }
                  await
                    api.preprocessings.runById({id: this.preprocessing.id, model: param})
                  await this.$notify.success({
                    title: 'Success',
                    message: 'ID:' + this.updataDataId + 'の前処理に成功しました'
                  })
                  // 成功した場合、使用したデータのisRawフラグをFalseにする
                  await this.updateData()
                  this.error = null
                } catch (e) {
                  this.error = e
                }
              }
            }
            this.loading = false
            this.$store.commit('setLoading', true)
            await this.sleep(2000)
            // 前処理画面に遷移
            if (this.checked && this.error === null) {
              this.emitHistoryPage(this.preprocessing.id)
            } else if (this.error === null) {
              this.emitDone()
            } else {
              this.$notify.error({
                title: 'Error',
                message: '前処理に失敗しました'
              })
            }
          }
        })
      }
    }
  }
</script>

<style scoped>
  .dialog /deep/ .el-dialog {
    min-width: 800px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }
</style>
