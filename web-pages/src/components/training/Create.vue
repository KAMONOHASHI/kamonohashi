<template>
  <div>
    <el-dialog class="dialog"
               title="学習実行"
               :visible.sync="dialogVisible"
               :before-close="closeDialog"
               :close-on-click-modal="false">
      <el-form ref="runForm" :rules="rules" :model="this">
        <pl-display-error :error="error"/>
        <div v-if="originId !== undefined">
          <el-row :gutter="20">
            <el-col :span="12">
              <el-form-item label="学習名" prop="name">
                <el-input v-model="name"/>
              </el-form-item>
              <el-form-item label="親学習" prop="parent">
                <pl-training-history-selector v-model="parent"/>
              </el-form-item>
              <el-form-item label="データセット" prop="dataSet">
                <pl-dataset-selector v-model="dataSet"/>
              </el-form-item>
              <el-form-item label="実行コマンド" prop="entryPoint">
                <el-input type="textarea" :autosize="{ minRows: 2 }" v-model="entryPoint"/>
              </el-form-item>
              <el-form-item label="コンテナイメージ" required>
                <pl-container-selector v-model="containerImage"/>
              </el-form-item>
              <el-form-item label="モデル" required>
                <pl-git-selector v-model="git"/>
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
              <el-form-item label="環境変数">
                <pl-dynamic-multi-input v-model="options"/>
              </el-form-item>

              <el-form-item label="結果Zip圧縮">
                <el-switch v-model="zip"
                          style="width: 100%;"
                          inactive-text="圧縮しない"
                          active-text="圧縮する"/>
              </el-form-item>

              <el-form-item label="パーティション" prop="partition">
                <pl-string-selector v-if="partitions"
                                    v-model="partition"
                                    :valueList="partitions"
                />
              </el-form-item>

              <el-form-item label="メモ">
                <el-input
                  type="textarea"
                  :autosize="{ minRows: 2, maxRows: 4}"
                  v-model="memo">
                </el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-row class="right-button-group footer">
            <el-button @click="emitCancel">キャンセル</el-button>
            <el-button v-if="originId !== undefined | active===3" type="primary"
                       @click="runTrain">実行
            </el-button>
          </el-row>
        </div>
        <div v-else>
          <el-row :gutter="20">
            <el-steps :active="active" align-center>
              <el-step title="Step 1" description="training name & dataset"></el-step>
              <el-step title="Step 2" description="container image & model"></el-step>
              <el-step title="Step 3" description="resource"></el-step>
              <el-step title="Step 4" description="option"></el-step>
            </el-steps>
            <div class="element">
              <el-form v-if="active===0">
                <el-col :span="12">
                  <el-form-item label="学習名" prop="name" class="is-required">
                    <el-input v-model="name"/>
                  </el-form-item>
                  <el-form-item label="親学習" prop="parent">
                    <pl-training-history-selector v-model="parent"/>
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="データセット" prop="dataSet" class="is-required">
                    <pl-dataset-selector v-model="dataSet"/>
                  </el-form-item>
                </el-col>
              </el-form>
              <el-form v-if="active===1">
                <el-col :span="12">
                  <el-form-item label="コンテナイメージ" required>
                    <pl-container-selector v-model="containerImage"/>
                  </el-form-item>
                  <el-form-item label="モデル" required>
                    <pl-git-selector v-model="git"/>
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="実行コマンド" prop="entryPoint" class="is-required">
                    <el-input type="textarea" :autosize="{ minRows: 2 }" v-model="entryPoint"/>
                  </el-form-item>
                </el-col>
              </el-form>
              <el-form v-if="active===2">
                <el-col :span="18" :offset="3">
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
                </el-col>
              </el-form>
              <el-form v-if="active===3">
                <el-col>
                  <el-form-item label="環境変数">
                    <pl-dynamic-multi-input v-model="options"/>
                  </el-form-item>
                  <el-form-item label="結果Zip圧縮">
                    <el-switch v-model="zip"
                              style="width: 100%;"
                              inactive-text="圧縮しない"
                              active-text="圧縮する"/>
                  </el-form-item>
                  <el-form-item label="パーティション" prop="partition">
                    <pl-string-selector
                      v-if="partitions"
                      v-model="partition"
                      :valueList="partitions"
                    />
                  </el-form-item>
                  <el-form-item label="メモ">
                    <el-input
                      type="textarea"
                      :autosize="{ minRows: 2, maxRows: 4}"
                      v-model="memo">
                    </el-input>
                  </el-form-item>
                </el-col>
              </el-form>
            </div>
          </el-row>
          <el-row class="step">
          <span class="left-step-group" v-if="active >= 1" style="margin-top: 12px;"
                @click="previous">
            <i class="el-icon-arrow-left"></i>
            Previous step
          </span>
            <span class="right-step-group" v-if="active <=2" style="margin-top: 12px;" @click="next">
            Next step
            <i class="el-icon-arrow-right"></i>
          </span>
            <el-button class="right-step-group" v-if="originId !== undefined | active===3" type="primary"
                       @click="runTrain">実行
            </el-button>
          </el-row>
        </div>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
  import DataSetSelector from '@/components/common/DatasetSelector.vue'
  import TrainingHistorySelector from '@/components/common/TrainingHistorySelector.vue'
  import DynamicMultiInputField from '@/components/common/DynamicMultiInputField.vue'
  import ContainerSelector from '@/components/common/ContainerSelector.vue'
  import StringSelector from '@/components/common/StringSelector.vue'
  import GitSelector from '@/components/common/GitSelector.vue'
  import DisplayError from '@/components/common/DisplayError'
  import api from '@/api/v1/api'

  export default {
    name: 'CreateTrain',
    components: {
      'pl-dataset-selector': DataSetSelector,
      'pl-training-history-selector': TrainingHistorySelector,
      'pl-string-selector': StringSelector,
      'pl-container-selector': ContainerSelector,
      'pl-git-selector': GitSelector,
      'pl-dynamic-multi-input': DynamicMultiInputField,
      'pl-display-error': DisplayError
    },
    props: {
      originId: String
    },
    data () {
      return {
        rules: {
          dataSet: [{required: true, trigger: 'blur', message: '必須項目です'}],
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          entryPoint: [{required: true, trigger: 'blur', message: '必須項目です'}],
          cpu: [{required: true, message: '必須項目です'}],
          memory: [{required: true, message: '必須項目です'}],
          gpu: [{required: true, message: '必須項目です'}],
          repository: [{required: true, trigger: 'blur', message: '必須項目です'}],
          branch: [{required: true, trigger: 'blur', message: '必須項目です'}],
          image: [{required: true, trigger: 'blur', message: '必須項目です'}],
          tag: [{required: true, trigger: 'blur', message: '必須項目です'}],
          zip: [{required: true, message: '必須項目です'}]
        },
        dialogVisible: true,
        error: undefined,
        origin: undefined, // コピー元のオブジェクトはorigin、コピー先の親ジョブオブジェクトはparentとしてそれぞれ格納
        partitions: undefined,
        partition: undefined,
        parent: undefined,
        name: undefined,
        cpu: undefined,
        memory: undefined,
        gpu: undefined,
        dataSet: undefined,
        containerImage: undefined,
        memo: undefined,
        git: undefined,
        options: undefined,
        entryPoint: undefined,
        zip: true,
        active: 0
      }
    },
    async created () {
      let result = await (api.cluster.getPartitions())
      this.partitions = result.data
      await this.retrieveParentTrain()
    },
    methods: {
      async runTrain () {
        let form = this.$refs.runForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              let options = {}
              // apiのフォーマットに合わせる(配列 => オブジェクト)
              this.options.forEach((kvp) => {
                options[kvp.key] = kvp.value
              })
              let param = {
                Name: this.name,
                ContainerImage: this.containerImage,
                DataSetId: this.dataSet ? this.dataSet.id : null,
                ParentId: this.parent ? this.parent.id : null,
                GitModel: this.git,
                EntryPoint: this.entryPoint,
                Options: options,
                Cpu: this.cpu,
                Memory: this.memory,
                Gpu: this.gpu,
                Partition: this.partition,
                Memo: this.memo,
                Zip: this.zip
              }
              await api.training.post({model: param})

              // 成功したら、ダイヤログを閉じて更新
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      copyFromOrigin () {
        let origin = this.origin

        if (origin) {
          this.name = origin.name
          this.container = origin.containerImage
          this.dataSet = origin.dataSet
          this.git = origin.gitModel
          this.options = origin.options
          this.memo = origin.memo
          this.cpu = origin.cpu
          this.memory = origin.memory
          this.gpu = origin.gpu
          this.partition = origin.partition
          this.containerImage = origin.containerImage
          this.entryPoint = origin.entryPoint
          this.zip = origin.zip
          if (origin.parent) {
            this.parent = origin.parent
          }
        }
      },

      async retrieveParentTrain () {
        // 親が指定されていれば親のジョブ情報取得
        if (this.originId >= 0) {
          this.origin = (await api.training.getById({id: this.originId})).data
          this.copyFromOrigin()
        }
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      },
      next () {
        if (this.active++ > 3) {
          this.active = 0
        }
      },
      previous () {
        if (this.active-- < 0) {
          this.active = 0
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .dialog /deep/ .el-dialog {
    min-width: 800px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
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

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }

  .step {
    padding-top: 20px;
    cursor: pointer;
    :hover {
      color: #409eff;
    }
  }

  .element {
    padding-top: 40px;
  }

</style>
