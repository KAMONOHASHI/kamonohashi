<template>
   <div>
    <el-dialog class="dialog"
               title="ノートブック起動"
               :visible.sync="dialogVisible"
               :before-close="closeDialog"
               :close-on-click-modal="false">
      <el-form ref="runForm" :rules="rules" :model="this">
        <pl-display-error :error="error"/>
        <div v-if="originId !== undefined">
          <el-row :gutter="20">
            <div class="element">
              <el-form v-if="active===0">
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
            </div>
          </el-row>
        </div>
        <div v-else>
          <el-row :gutter="20">
            <el-steps :active="active" align-center>
              <el-step title="Step 1" description="notebook name & dataset"></el-step>
              <el-step title="Step 2" description="container image & model"></el-step>
              <el-step title="Step 3" description="resource"></el-step>
              <el-step title="Step 4" description="option"></el-step>
            </el-steps>
            <div class="element">
              <el-form v-if="active===0">
                <el-col :span="12">
                  <el-form-item label="ノートブック名" prop="name" class="is-required">
                    <el-input v-model="name"/>
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="データセット" prop="dataSet" >
                    <pl-dataset-selector v-model="dataSet"/>
                  </el-form-item>
                </el-col>
              </el-form>
              <el-form v-if="active===1">
                <el-col :span="12">
                  <el-form-item label="コンテナイメージ" required>
                    <pl-container-selector v-model="containerImage"/>
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="モデル">
                    <pl-git-selector v-model="git"/>
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
        </div>
        <el-row class="step">
          <span class="left-step-group" v-if="active >= 1 && originId === undefined" style="margin-top: 12px;"
                @click="previous">
            <i class="el-icon-arrow-left"></i>
            Previous step
          </span>
          <span class="right-step-group" v-if="active <=2 && originId === undefined" style="margin-top: 12px;" @click="next">
            Next step
            <i class="el-icon-arrow-right"></i>
          </span>
          <el-button class="right-step-group" v-if="active===3" type="primary"
                     @click="runNotebook">起動
          </el-button>
          <el-button class="right-step-group" v-if="originId !== undefined" type="primary"
                     @click="reRunNotebook">再実行
          </el-button>
        </el-row>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
  import DataSetSelector from '@/components/common/DatasetSelector.vue'
  import DynamicMultiInputField from '@/components/common/DynamicMultiInputField.vue'
  import ContainerSelector from '@/components/common/ContainerSelector.vue'
  import StringSelector from '@/components/common/StringSelector.vue'
  import GitSelector from '@/components/common/GitSelector.vue'
  import DisplayError from '@/components/common/DisplayError'
  import api from '@/api/v1/api'

  export default {
    name: 'CreateNotebook',
    components: {
      'pl-dataset-selector': DataSetSelector,
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
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          cpu: [{required: true, message: '必須項目です'}],
          memory: [{required: true, message: '必須項目です'}],
          gpu: [{required: true, message: '必須項目です'}],
          image: [{required: true, trigger: 'blur', message: '必須項目です'}],
          tag: [{required: true, trigger: 'blur', message: '必須項目です'}]
        },
        dialogVisible: true,
        error: undefined,
        origin: undefined, // コピー元のオブジェクトはorigin、コピー先の親ジョブオブジェクトはparentとしてそれぞれ格納
        partitions: undefined,
        partition: undefined,
        name: undefined,
        cpu: undefined,
        memory: undefined,
        gpu: undefined,
        dataSet: undefined,
        containerImage: undefined,
        memo: undefined,
        git: undefined,
        options: undefined,
        active: 0,
        expiresIn: 0
      }
    },
    async created () {
      let result = await (api.cluster.getPartitions())
      this.partitions = result.data
      await this.retrieveOriginNotebook()
    },
    methods: {
      async runNotebook () {
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
                name: this.name,
                containerImage: this.containerImage,
                dataSetId: this.dataSet ? this.dataSet.id : null,
                gitModel: this.git,
                options: options,
                cpu: this.cpu,
                memory: this.memory,
                gpu: this.gpu,
                partition: this.partition,
                memo: this.memo,
                expiresIn: this.expiresIn
              }
              await api.notebook.post({model: param})

              // 成功したら、ダイヤログを閉じて更新
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async reRunNotebook () {
        let form = this.$refs.runForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              let param = {
                  cpu: this.cpu,
                  memory: this.memory,
                  gpu: this.gpu,
                  expiresIn: this.expiresIn
              }
              await api.notebook.postRerun({id: this.originId, model: param})

              // 成功したら、ダイヤログを閉じて更新
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      emitDone () {
        this.$emit('done')
        this.dialogVisible = false
      },
      emitCancel () {
        this.$emit('cancel')
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
      },
      async retrieveOriginNotebook () {
        if (this.originId >= 0) {
          this.origin = (await api.notebook.getById({id: this.originId})).data
          this.copyFromOrigin()
        }
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
          if (origin.parent) {
            this.parent = origin.parent
          }
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .right-button-group {
    text-align: right;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .footer {
    padding-top: 40px;
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

</style>
