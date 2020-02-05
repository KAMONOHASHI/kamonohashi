<template>
  <div>
    <el-dialog
      class="dialog"
      title="学習実行"
      :visible.sync="dialogVisible"
      :before-close="closeDialog"
      :close-on-click-modal="false"
    >
      <!-- <el-form ref="runForm" :model="form" :rules="rules"> -->
      <pl-display-error :error="error" />
      <el-row :gutter="20">
        <el-steps :active="active" align-center>
          <el-step
            title="Step 1"
            description="training name & dataset"
          ></el-step>
          <el-step
            title="Step 2"
            description="container image & model"
          ></el-step>
          <el-step title="Step 3" description="resource"></el-step>
          <el-step title="Step 4" description="option"></el-step>
        </el-steps>
        <div class="element">
          <el-form v-if="active === 0" :model="form" :rules="rules">
            <el-col :span="12">
              <el-form-item label="学習名" prop="name">
                <el-input v-model="form.name" />
              </el-form-item>
              <el-form-item label="親学習" prop="parent">
                <pl-training-history-selector v-model="form.parent" />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="データセット" prop="dataSet">
                <pl-dataset-selector v-model="form.dataSet" />
              </el-form-item>
            </el-col>
          </el-form>

          <el-form v-else-if="active === 1" :model="form" :rules="rules">
            <el-col :span="12">
              <el-form-item label="コンテナイメージ">
                <pl-container-selector v-model="form.containerImage" />
              </el-form-item>
              <el-form-item label="モデル">
                <pl-git-selector v-model="form.git" />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="実行コマンド" prop="entryPoint">
                <el-input
                  v-model="form.entryPoint"
                  type="textarea"
                  :autosize="{ minRows: 2 }"
                />
              </el-form-item>
            </el-col>
          </el-form>

          <el-form v-else-if="active === 2" :model="form" :rules="rules">
            <el-col :span="18" :offset="3">
              <kqi-resource-selector
                v-model="form.resource"
              ></kqi-resource-selector>
            </el-col>
          </el-form>

          <el-form v-else-if="active === 3" :model="form" :rules="rules">
            <el-col>
              <el-form-item label="環境変数">
                <pl-dynamic-multi-input v-model="form.options" />
              </el-form-item>
              <el-form-item label="結果Zip圧縮">
                <el-switch
                  v-model="form.zip"
                  style="width: 100%;"
                  inactive-text="圧縮しない"
                  active-text="圧縮する"
                />
              </el-form-item>
              <el-form-item label="パーティション" prop="partition">
                <pl-string-selector
                  v-if="partitions"
                  v-model="form.partition"
                  :value-list="partitions"
                />
              </el-form-item>
              <el-form-item label="メモ">
                <el-input
                  v-model="form.memo"
                  type="textarea"
                  :autosize="{ minRows: 2, maxRows: 4 }"
                >
                </el-input>
              </el-form-item>
            </el-col>
          </el-form>
        </div>
      </el-row>
      <el-row class="step">
        <span
          v-if="active >= 1"
          class="left-step-group"
          style="margin-top: 12px;"
          @click="previous"
        >
          <i class="el-icon-arrow-left"></i>
          Previous step
        </span>
        <span
          v-if="active <= 2"
          class="right-step-group"
          style="margin-top: 12px;"
          @click="next"
        >
          Next step
          <i class="el-icon-arrow-right"></i>
        </span>
        <el-button
          v-if="active === 3"
          class="right-step-group"
          type="primary"
          @click="runTrain"
          >実行
        </el-button>
      </el-row>
      <!-- </el-form> -->
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
import KqiResourceSelector from '@/components/KqiResourceSelector'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  name: 'CreateTrain',
  components: {
    'pl-dataset-selector': DataSetSelector,
    'pl-training-history-selector': TrainingHistorySelector,
    'pl-string-selector': StringSelector,
    'pl-container-selector': ContainerSelector,
    'pl-git-selector': GitSelector,
    'pl-dynamic-multi-input': DynamicMultiInputField,
    'pl-display-error': DisplayError,
    'kqi-resource-selector': KqiResourceSelector,
  },

  data() {
    return {
      form: {
        name: null,
        parent: null,
        dataSet: null,
        containerImage: null,
        git: null,
        entryPoint: null,
        resource: {
          cpu: 1,
          memory: 1,
          gpu: 0,
        },
        options: null,
        zip: true,
        partition: null,
        memo: null,
      },
      rules: {
        name: [formRule],
        dataSet: [formRule],
        entryPoint: [formRule],
        // cpu: [{ required: true, message: '必須項目です' }],
        // memory: [{ required: true, message: '必須項目です' }],
        // gpu: [{ required: true, message: '必須項目です' }],
        // repository: [formRule],
        // branch: [formRule],
        // image: [formRule],
        // tag: [formRule],
        // zip: [{ required: true, message: '必須項目です' }],
      },
      dialogVisible: true,
      error: undefined,
      origin: undefined, // コピー元のオブジェクトはorigin、コピー先の親ジョブオブジェクトはparentとしてそれぞれ格納
      active: 0,
    }
  },
  computed: {
    ...mapGetters(['partitions']),
  },
  async created() {
    await this.fetchPartitions()
  },
  methods: {
    ...mapActions(['fetchPartitions', 'post']),
    async runTrain() {
      // let form = this.$refs.runForm
      // await form.validate(async valid => {
      //   if (valid) {
      try {
        let options = {}
        // apiのフォーマットに合わせる(配列 => オブジェクト)
        this.form.options.forEach(kvp => {
          options[kvp.key] = kvp.value
        })
        let params = {
          Name: this.form.name,
          ContainerImage: this.form.containerImage,
          DataSetId: this.form.dataSet ? this.form.dataSet.id : null,
          ParentId: this.form.parent ? this.form.parent.id : null,
          GitModel: this.form.git,
          EntryPoint: this.form.entryPoint,
          Options: options,
          Cpu: this.form.resource.cpu,
          Memory: this.form.resource.memory,
          Gpu: this.form.resource.gpu,
          Partition: this.form.partition,
          Memo: this.form.memo,
          Zip: this.form.zip,
        }
        await this.post(params)

        // 成功したら、ダイヤログを閉じて更新
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      }
      // }
      // })
    },
    copyFromOrigin() {
      let origin = this.origin

      if (origin) {
        this.name = origin.name
        this.container = origin.containerImage
        this.dataSet = origin.dataSet
        this.git = origin.gitModel
        this.options = origin.options
        this.memo = origin.memo
        this.form.resource.cpu = origin.cpu
        this.form.resource.memory = origin.memory
        this.form.resource.gpu = origin.gpu
        this.partition = origin.partition
        this.containerImage = origin.containerImage
        this.entryPoint = origin.entryPoint
        this.zip = origin.zip
        if (origin.parent) {
          this.parent = origin.parent
        }
      }
    },

    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },
    closeDialog(done) {
      done()
      this.emitCancel()
    },
    next() {
      if (this.active++ > 3) {
        this.active = 0
      }
    },
    previous() {
      if (this.active-- < 0) {
        this.active = 0
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.dialog /deep/ .el-dialog {
  min-width: 800px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
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
