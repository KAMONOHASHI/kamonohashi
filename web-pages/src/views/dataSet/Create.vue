<template>
  <el-dialog
    class="dialog"
    title="データセット作成"
    :visible.sync="dialogVisible"
    :before-close="handleCancel"
    :close-on-click-modal="false"
  >
    <el-form ref="createForm" :model="model" :rules="rules">
      <pl-display-error :error="error" />
      <el-form-item label="データセット名" prop="name">
        <el-input v-model="model.name" />
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="model.memo" type="textarea" />
      </el-form-item>
      <el-form-item label="データ" prop="entries"> </el-form-item>
      <el-form-item>
        <pl-dataset-transfer
          v-if="model.entries"
          v-model="model.entries"
        ></pl-dataset-transfer>
      </el-form-item>
      <el-row class="right-button-group footer">
        <el-button @click="handleCancel">キャンセル</el-button>
        <el-button type="primary" @click="handleCreate">作成</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
import DataSetTransfer from '@/components/dataset/DatasetTransfer/Index.vue'
import DisplayError from '@/components/common/DisplayError'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers(
  'dataSet',
)

export default {
  components: {
    'pl-display-error': DisplayError,
    'pl-dataset-transfer': DataSetTransfer,
  },
  props: {
    parentId: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      dialogVisible: true,
      error: null,
      model: {
        name: '',
        memo: '',
        entries: undefined,
      },
      rules: this.createRules(),
    }
  },

  computed: {
    ...mapGetters(['detail', 'dataTypes']),
  },

  async created() {
    await this.getDataTypes()
  },

  methods: {
    ...mapActions(['fetchDetail', 'fetchDataTypes', 'post']),
    ...mapMutations(['setDataTypes']),

    async getDataTypes() {
      try {
        if (!this.parentId) {
          await this.fetchDataTypes()
          this.model.entries = {}
          this.dataTypes.forEach(type => {
            this.model.entries[type.name] = []
          })
        }

        if (this.parentId) {
          await this.fetchDetail(this.parentId)
          this.model.name = this.detail.name
          this.model.memo = this.detail.memo
          let ent = {}
          let types = []
          for (let key in this.detail.entries) {
            ent[key] = this.detail.entries[key]
            types.push({ name: key })
          }
          this.model.entries = ent
          this.setDataTypes(types)
        }
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async handleCreate() {
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
      for (let key in this.model.entries) {
        postEntries[key] = []
        this.model.entries[key].forEach(entry => {
          postEntries[key].push({
            id: entry.id,
          })
        })
      }
      let params = {
        entries: postEntries,
        name: this.model.name,
        memo: this.model.memo,
      }
      await this.post(params)
    },

    handleCancel() {
      this.emitCancel()
    },

    emitDone() {
      this.showSuccessMessage()
      this.$emit('done')
    },

    emitCancel() {
      this.$emit('cancel')
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
