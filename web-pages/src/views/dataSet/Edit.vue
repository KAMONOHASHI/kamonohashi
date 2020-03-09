<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteDataSet"
    @close="emitCancel"
  >
    <el-row v-if="isEditDialog">
      <el-col :span="24" class="right-button-group">
        <el-button @click="emitCopy">コピー</el-button>
      </el-col>
    </el-row>

    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-row v-if="isCreateDialog">
        <el-form-item label="データセット名" prop="name">
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="メモ" prop="memo">
          <el-input v-model="form.memo" type="textarea" />
        </el-form-item>
      </el-row>
      <el-row v-else>
        <el-col :span="12">
          <el-form-item label="ID">
            <kqi-display-text-form v-model="id" />
          </el-form-item>
          <el-form-item label="データセット名" prop="name">
            <el-input v-model="form.name" />
          </el-form-item>
          <el-form-item label="メモ" prop="memo">
            <el-input v-model="form.memo" type="textarea" />
          </el-form-item>
        </el-col>
        <el-col v-if="isEditDialog" :offset="1" :span="11">
          <el-form-item label="編集可否">
            <kqi-display-text-form v-if="detail.isLocked" value="不可" />
            <kqi-display-text-form v-else value="可能" />
          </el-form-item>
          <el-form-item label="登録者">
            <kqi-display-text-form v-model="detail.createdBy" />
          </el-form-item>
          <el-form-item label="登録日時">
            <kqi-display-text-form v-model="detail.createdAt" />
          </el-form-item>
        </el-col>
      </el-row>

      <el-form-item label="データ" prop="entries"> </el-form-item>
      <el-form-item>
        <pl-dataset-transfer
          v-if="form.entries"
          v-model="form.entries"
        ></pl-dataset-transfer>
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiDisplayError from '@/components/KqiDisplayError'

import DataSetTransfer from './DatasetTransfer/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers(
  'dataSet',
)

export default {
  components: {
    KqiDialog,
    KqiDisplayTextForm,
    KqiDisplayError,
    'pl-dataset-transfer': DataSetTransfer,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      form: {
        name: '',
        memo: '',
        entries: null,
      },
      title: '',
      isCreateDialog: false,
      isCopyCreation: false,
      isEditDialog: false,
      dialogVisible: true,
      error: null,
      rules: this.createRules(),
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
    ...mapActions(['fetchDetail', 'fetchDataTypes', 'post', 'put', 'delete']),
    ...mapMutations(['setDataTypes']),
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[2] // ["", "dataset", "{type}", "{id}"]
      switch (type) {
        case 'create':
          this.title = 'データセット作成'
          this.isCreateDialog = true
          this.isCopyCreation = this.id !== null
          this.isEditDialog = false
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
            this.emitDone()
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
        await this.put({ id: this.id, params: params })
      }
    },

    async deleteDataSet() {
      try {
        await this.delete(this.id)
        this.emitDone()
      } catch (e) {
        this.error = e
      }
    },

    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
    },

    emitCopy() {
      this.$emit('copy', this.id)
    },

    createRules() {
      return {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
        entries: [
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
      }
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

.dialog /deep/ label {
  font-weight: bold !important;
}

.right-button-group {
  padding-top: 0px;
  text-align: right;
}
</style>
