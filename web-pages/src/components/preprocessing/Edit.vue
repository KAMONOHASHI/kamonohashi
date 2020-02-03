<template>
  <el-dialog
    class="dialog"
    title="前処理情報"
    :visible="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-row type="flex" justify="end">
      <el-col :span="24" class="right-button-group">
        <el-button @click="handleCopy">コピー</el-button>
      </el-col>
    </el-row>
    <el-form
      ref="editForm"
      v-loading="loading || loadingGit"
      :model="this"
      :rules="rules"
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
      <pl-display-error :error="error" />
      <pl-display-text label="ID" :value="id" />
      <el-form-item label="前処理名" prop="name">
        <el-input v-model="name"></el-input>
      </el-form-item>
      <el-form-item label="実行コマンド" prop="entryPoint">
        <div v-if="isPatch">
          <el-input
            v-model="entryPoint"
            type="textarea"
            :autosize="{ minRows: 2 }"
            :readonly="true"
          />
        </div>
        <div v-else>
          <el-input
            v-model="entryPoint"
            type="textarea"
            :autosize="{ minRows: 2 }"
          />
        </div>
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="memo" type="textarea"> </el-input>
      </el-form-item>
      <el-row :gutter="20">
        <el-col :span="12">
          <div v-if="isPatch">
            <pl-display-text-form label="コンテナ" :value="containerUrl" />
            <el-form-item label="モデル">
              <div class="el-input">
                <span v-if="gitModel" style="padding-left: 3px">
                  <a :href="gitModel.url" target="_blank">
                    {{ gitModel.owner }}/{{ gitModel.repository }}/{{
                      gitModel.branch
                    }}
                  </a>
                </span>
                <span v-else>
                  None
                </span>
              </div>
            </el-form-item>
          </div>
          <div v-else>
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
          </div>
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
          <el-button
            class="pull-right btn-update"
            type="primary"
            icon="el-icon-edit-outline"
            @click="handleUpdate"
            >保存
          </el-button>
          <el-button
            class="pull-right btn-cancel"
            icon="el-icon-close"
            @click="handleCancel"
            >キャンセル</el-button
          >
          <pl-delete-button
            class="pull-left btn-update"
            :disabled="isPatch"
            @delete="handleRemove"
          />
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import ContainerSelector from '../common/ContainerSelector'
import GitSelector from '../common/GitSelector'
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
import DeleteButton from '@/components/common/DeleteButton.vue'
import DisplayError from '@/components/common/DisplayError'
import api from '@/api/v1/api'

export default {
  name: 'PreprocessingEdit',
  components: {
    'pl-display-error': DisplayError,
    'pl-git-selector': GitSelector,
    'pl-container-selector': ContainerSelector,
    'pl-display-text': DisplayTextForm,
    'pl-delete-button': DeleteButton,
    'pl-display-text-form': DisplayTextForm,
  },
  props: {
    id: String,
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
      isPatch: true, // true: nameとmemoのみ更新、false: 全て更新
      containerUrl: '',
      gitModel: '',
      cpu: 0,
      memory: 0,
      gpu: 0,

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

  watch: {
    async id() {
      await this.changeId()
    },
  },

  async created() {
    await this.changeId()
  },

  methods: {
    async changeId() {
      if (this.id) {
        this.loading = true
        try {
          let params = {
            id: this.id,
          }
          // 履歴情報が存在する場合、nameとmemoのみ編集可能
          let [historyData] = api.f.data(
            await api.preprocessings.getHistory(params),
          )
          this.isPatch = historyData.length !== 0
          // 前処理情報を取得
          let [model] = api.f.data(await api.preprocessings.getById(params))
          this.name = model.name
          this.memo = model.memo
          this.cpu = model.cpu
          this.memory = model.memory
          this.gpu = model.gpu
          this.entryPoint = model.entryPoint
          if (model.containerImage !== null) {
            this.selectedContainer = model.containerImage
            this.containerUrl = model.containerImage.url
          }
          this.gitModel = model.gitModel
          this.selectedGit = model.gitModel || {}
          this.error = null
        } catch (e) {
          this.error = e
        } finally {
          this.loading = false
        }
      }
    },
    /* eslint-disable */
    async handleClose(done) {
      this.close(false);
    },
    /* eslint-enable */

    async handleUpdate() {
      let form = this.$refs.editForm
      await form.validate(async valid => {
        if (valid) {
          this.loading = true
          try {
            if (this.isPatch) {
              await this.patchPreprocessings() // 名称・メモのみ更新
            } else {
              await this.putPreprocessings() // 全部更新（ID以外）
            }
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          } finally {
            this.loading = false
          }
        }
      })
    },
    // 全部更新（ID以外）
    async putPreprocessings() {
      let param = {
        id: this.id,
        model: {
          name: this.name,
          entryPoint: this.entryPoint,
          containerImage: this.containerImageEmptyCheck(this.selectedContainer),
          gitModel: this.repositoryEmptyCheck(this.selectedGit),
          memo: this.memo,
          cpu: this.cpu,
          memory: this.memory,
          gpu: this.gpu,
        },
      }
      await api.preprocessings.put(param)
    },
    containerImageEmptyCheck(obj) {
      if (!obj) return null
      if (!obj.image) return null
      return obj
    },
    repositoryEmptyCheck(obj) {
      if (!obj) return null
      if (!obj.repository) return null
      return obj
    },
    // 名称・メモ・リソースのみ更新
    async patchPreprocessings() {
      let param = {
        id: this.id,
        model: {
          name: this.name,
          memo: this.memo,
          cpu: this.cpu,
          memory: this.memory,
          gpu: this.gpu,
        },
      }
      await api.preprocessings.patch(param)
    },

    async handleRemove() {
      this.loading = true
      try {
        let params = {
          id: this.id,
        }
        await api.preprocessings.delete(params)
        this.emitDone()
        this.error = null
      } catch (e) {
        this.error = e
      } finally {
        this.loading = false
      }
    },
    async handleCancel() {
      this.emitCancel()
    },
    async handleCopy() {
      this.emitCopy(this.id)
    },
    emitCopy(id) {
      this.$emit('copy', id)
    },
    emitDone() {
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
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

.btn-update {
  margin-left: 10px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.pull-right {
  float: right !important;
}

.pull-left {
  float: left !important;
}
</style>
