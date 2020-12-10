<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    :submitText="submitText"
    @submit="submit"
    @delete="deleteDataSet"
    @close="$emit('cancel')"
  >
    <el-row v-if="isEditDialog">
      <el-col :span="24" class="right-button-group">
        <el-button @click="$emit('copy', id)">コピー</el-button>
      </el-col>
    </el-row>

    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-row v-if="isCreateDialog">
        <el-form-item label="データセット名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
      </el-row>

      <el-form-item label="モデルの目的を選択してください" prop="entries" />
      <div class="model-type-list ">
        <div
          v-for="(model, index) in modelList"
          :key="'t' + index"
          class="model-type"
        >
          <div class="model-type-image"><span>IMAGE</span></div>
          <div class="model-type-label ">
            <el-radio v-model="form.modeltype" :label="model.name">{{
              model.label
            }}</el-radio>
            <div class="model-type-description ">{{ model.description }}</div>
          </div>
        </div>
      </div>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers(
  'dataSet',
)

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      submitText: '新規登録',

      modelList: [
        {
          name: 'ingleLabel',
          label: '単一ラベル分類',
          description: '画像に割り当てる正しいラベルを１つ予測します。',
        },
        {
          name: 'objectDetection',
          label: 'オブジェクト検出',
          description: '関心のあるオブジェクトのすべての位置を予測します。',
        },
        {
          name: 'segmentation',
          label: 'セグメンテーション',
          description: '関心のあるオブジェクトのすべての領域を予測します。',
        },
        { name: 'anomaly', label: '異常検知', description: '' },
        { name: 'regression', label: '回帰', description: '' },
      ],
      form: {
        name: '',

        modeltype: null,
      },
      title: '',
      isCreateDialog: false,
      isCopyCreation: false,
      isEditDialog: false,
      isLocked: false,
      dialogVisible: true,
      error: null,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
        modeltype: [
          {
            required: true,
            trigger: 'blur',
            validator(rule, value, callback) {
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
            },
          },
        ],
      },
    }
  },
  computed: {
    ...mapGetters(['detail', 'dataTypes']),
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
      'fetchDetail',
      'fetchDataTypes',
      'post',
      'put',
      'patch',
      'delete',
    ]),
    ...mapMutations(['setDataTypes']),
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[2] // ["", "dataset", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = '新しいデータセットの作成'
          this.isCreateDialog = true
          this.isCopyCreation = this.id !== null
          this.isEditDialog = false
          this.isLocked = false
          break

        case 'edit':
          this.title = 'データセット編集'
          this.isCreateDialog = false
          this.isCopyCreation = false
          this.isEditDialog = true
          break
      }

      // 新規作成時はデータタイプを設定
      if (this.isCreateDialog && !this.isCopyCreation) {
        try {
          await this.fetchDataTypes()
          this.form.entries = {}
          this.dataTypes.forEach(type => {
            this.form.entries[type.name] = []
          })
          this.error = null
        } catch (e) {
          this.error = e
        }
      }

      // 編集時/コピー作成時は、既に登録されている情報を各項目を設定
      if (this.isEditDialog || this.isCopyCreation) {
        await this.retrieveData()
      }
    },
    async retrieveData() {
      this.form.entries = null
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        let ent = {}
        let types = []
        for (let key in this.detail.entries) {
          ent[key] = this.detail.entries[key]
          types.push({ name: key })
        }
        this.form.entries = ent
        if (this.isEditDialog) {
          // 編集時は編集可否を設定
          this.isLocked = this.detail.isLocked
        }

        this.setDataTypes(types)
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async submit() {
      let form = this.$refs.createForm
      await form.validate(async valid => {
        if (valid) {
          try {
            await this.postDataSet()
            this.$emit('done')
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    async postDataSet() {
      let postEntries = {}
      for (let key in this.form.entries) {
        postEntries[key] = []
        this.form.entries[key].forEach(entry => {
          postEntries[key].push({
            id: entry.id,
          })
        })
      }
      let params = {
        entries: postEntries,
        name: this.form.name,
        memo: this.form.memo,
      }
      if (this.isCreateDialog) {
        // 新規作成
        await this.post(params)
      } else {
        // 編集
        if (this.isLocked) {
          // 編集不可の時は、名前とメモのみ編集可
          await this.patch({ id: this.id, params: params })
        } else {
          // 編集可の時は、データも編集可
          await this.put({ id: this.id, params: params })
        }
      }
    },

    async deleteDataSet() {
      try {
        await this.delete(this.id)
        this.$emit('done', 'delete')
      } catch (e) {
        this.error = e
      }
    },

    handleShowData(id) {
      this.$router.push(`/data/edit/${id}`)
    },
  },
}
</script>

<style lang="scss" scoped>
@media screen and (max-width: 1500px) {
  .dialog /deep/ .el-dialog {
    width: 750px;
  }
}

@media screen and (min-width: 1500px) {
  .dialog /deep/ .el-dialog {
    width: 1450px;
  }
}
.model-type-list {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: start;
  align-content: flex-start;
  margin-bottom: 40px;
}

.model-type {
  border-radius: 10px;
  border: solid 1px #ebeef5;
  float: left;
  margin: 0px 20px 10px 0;
  position: relative;
  width: 300px;
  height: 300px;
}

.model-type-image {
  border-radius: 10px 10px 0px 0px;
  height: 150px;
  background-color: #aaa;
  color: #666;
  text-align: center;
  padding-top: 30px;
}
.model-type-label {
  padding: 20px;
}
.model-type-description {
  padding: 20px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.right-button-group {
  padding-top: 0px;
  text-align: right;
}
</style>
