<template>
   <div>
    <el-dialog class="dialog"
               title="ノートブック作成"
               :visible.sync="dialogVisible"
               :before-close="closeDialog"
               :close-on-click-modal="false">
      <el-form ref="runForm" :rules="rules" :model="this">
        <pl-display-error :error="error"/>
        <div>
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
            <el-button class="right-step-group" v-if="active===3" type="primary"
                       @click="runJupyter">起動
            </el-button>
          </el-row>
        </div>
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
    name: 'CreateJupyter',
    components: {
      'pl-dataset-selector': DataSetSelector,
      'pl-string-selector': StringSelector,
      'pl-container-selector': ContainerSelector,
      'pl-git-selector': GitSelector,
      'pl-dynamic-multi-input': DynamicMultiInputField,
      'pl-display-error': DisplayError
    },
    props: {
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
        active: 0
      }
    },
    async created () {
      let result = await (api.cluster.getPartitions())
      this.partitions = result.data
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
                GitModel: this.git,
                Options: options,
                Cpu: this.cpu,
                Memory: this.memory,
                Gpu: this.gpu,
                Partition: this.partition,
                Memo: this.memo
              }
              await api.jupyter.post({model: param})

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
