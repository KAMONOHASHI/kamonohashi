<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
    :submit-text="submitText"
    @submit="submit"
    @delete="deleteDataSet"
    @close="$emit('cancel')"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-row>
        <el-form-item label="データセット名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
      </el-row>
      <el-row>
        <el-form-item label="" prop="name">
          <el-radio v-model="form.publishing" label="1" style="display:block"
            >パソコンから画像をアップロード</el-radio
          >
        </el-form-item>
      </el-row>
      <el-row>
        <h3>パソコンから画像をアップロード</h3>
        パソコンから画像をアップロード
        任意のファイル形式がアップロードできまうｓ。画像の表示はJPG,PNG,***がサポートされています。<br />
        一回のアップロードで最大XXXファイル送信できます。アップロードしたファイルはKAMONOHASHIのデータ、データセットに保存されます。
      </el-row>
      <el-row>
        <el-upload
          class="upload-demo"
          :on-preview="handlePreview"
          :on-remove="handleRemove"
          :before-remove="beforeRemove"
          multiple
          :limit="3"
          :on-exceed="handleExceed"
          :file-list="fileList"
        >
          <el-button size="small">ファイルを選択</el-button>
        </el-upload>
      </el-row>
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

      form: {
        name: '',
        publishing: '',
      },
      title: '',
      isCreateDialog: false,
      isCopyCreation: false,
      isLocked: false,
      dialogVisible: true,
      error: null,
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
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
          this.isLocked = false
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
      if (this.isCopyCreation) {
        await this.retrieveData()
      }
    },
    async retrieveData() {
      this.form.entries = null
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        let ent = {}
        let types = []
        for (let key in this.detail.entries) {
          ent[key] = this.detail.entries[key]
          types.push({ name: key })
        }
        this.form.entries = ent

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
  justify-content: flex-start;
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
