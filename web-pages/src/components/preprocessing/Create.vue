<template>
  <el-dialog
    class="dialog"
    title="前処理作成"
    :visible="dialogVisible"
    :before-close="closeDialog"
    :close-on-click-modal="false"
  >
    <el-form
      ref="createForm"
      v-loading="loading || loadingGit"
      :model="this"
      :rules="rules"
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <pl-display-error :error="error" />
      <el-form-item label="前処理名" prop="name">
        <el-input v-model="name"> </el-input>
      </el-form-item>
      <el-form-item label="実行コマンド" prop="entryPoint">
        <el-input
          v-model="entryPoint"
          type="textarea"
          :autosize="{ minRows: 2 }"
        />
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="memo" type="textarea"> </el-input>
      </el-form-item>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item label="コンテナ" prop="selectedContainer">
            <pl-container-selector
              v-model="selectedContainer"
            ></pl-container-selector>
          </el-form-item>
          <el-form-item label="Git" prop="selectedGit">
            <pl-git-selector
              v-model="selectedGit"
              :loading.sync="loadingGit"
            ></pl-git-selector>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="CPU" required>
            <el-slider
              v-model="cpu"
              class="el-input"
              :min="1"
              :max="200"
              show-input
            >
            </el-slider>
          </el-form-item>
          <el-form-item label="メモリ(GB)" required>
            <el-slider
              v-model="memory"
              class="el-input"
              :min="1"
              :max="200"
              show-input
            >
            </el-slider>
          </el-form-item>
          <el-form-item label="GPU" required>
            <el-slider
              v-model="gpu"
              class="el-input"
              :min="0"
              :max="16"
              show-input
            >
            </el-slider>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col class="button-group">
          <el-button @click="closeDialog(null)">キャンセル</el-button>
          <el-button type="primary" @click="createData">作成</el-button>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import api from '@/api/v1/api'
import ContainerSelector from '../common/ContainerSelector'
import GitSelector from '../common/GitSelector'
import DisplayError from '@/components/common/DisplayError'

export default {
  name: 'PreprocessingCreate',
  components: {
    'pl-display-error': DisplayError,
    'pl-git-selector': GitSelector,
    'pl-container-selector': ContainerSelector,
  },
  props: {
    originId: String,
  },
  data() {
    return {
      dialogVisible: true,
      loading: false,
      loadingGit: false,
      error: null,

      name: '',
      memo: '',
      entryPoint: '',
      selectedContainer: undefined,
      selectedGit: undefined,
      cpu: 0,
      memory: 0,
      gpu: 0,
      origin: undefined,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
        selectedContainer: [
          {
            trigger: 'blur',
            validator(rule, value, callback) {
              if (value && value.image && !value.tag) {
                callback(new Error('タグを選択してください'))
              } else {
                callback()
              }
            },
          },
        ],
        selectedGit: [
          {
            trigger: 'blur',
            validator(rule, value, callback) {
              if (value && value.owner && value.repository && !value.branch) {
                callback(new Error('ブランチを選択してください'))
              } else {
                callback()
              }
            },
          },
        ],
      },
    }
  },
  async created() {
    await this.retrieveOriginPreproc()
  },
  methods: {
    async retrieveOriginPreproc() {
      // コピー元が指定されていれば親の前処理情報取得
      if (this.originId >= 0) {
        this.origin = (
          await api.preprocessings.getById({ id: this.originId })
        ).data
        this.copyFromOrigin()
      }
    },
    copyFromOrigin() {
      let origin = this.origin
      if (origin) {
        this.name = origin.name
        this.memo = origin.memo
        this.entryPoint = origin.entryPoint
        if (origin.containerImage !== null) {
          this.selectedContainer = origin.containerImage
        }
        if (origin.gitModel.repository !== null) {
          this.selectedGit = origin.gitModel
        }
        this.cpu = origin.cpu
        this.memory = origin.memory
        this.gpu = origin.gpu
      }
    },
    async createData() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            await this.postPreprocessings()
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },
    async postPreprocessings() {
      let param = {
        model: {
          name: this.name,
          entryPoint: this.entryPoint,
          containerImage: this.emptyCheck(this.selectedContainer),
          gitModel: this.emptyCheck(this.selectedGit),
          memo: this.memo,
          cpu: this.cpu,
          memory: this.memory,
          gpu: this.gpu,
        },
      }
      await api.preprocessings.post(param)
    },
    emptyCheck(obj) {
      if (!obj) return null
      if (Object.keys(obj).length === 0) return null
      return obj
    },
    closeDialog(done) {
      if (done) {
        done()
      }
      this.emitCancel()
    },
    emitCancel() {
      this.$emit('cancel')
    },
    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },
  },
}
</script>

<style lang="scss" scoped>
.button-group {
  text-align: right;
  padding-top: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}
</style>
