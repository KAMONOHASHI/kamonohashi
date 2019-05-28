<template>
  <el-dialog class="dialog"
             title="データセット作成"
             :visible.sync="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">
    <el-form ref="createForm" :model="model" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="データセット名" prop="name">
        <el-input v-model="model.name"/>
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="model.memo" type="textarea"/>
      </el-form-item>
      <el-form-item label="データ" prop="entries">
      </el-form-item>
      <el-form-item>
        <pl-dataset-transfer v-model="model.entries" v-if="model.entries"></pl-dataset-transfer>
      </el-form-item>
      <el-row class="right-button-group footer">
        <el-button @click="handleCancel">キャンセル</el-button>
        <el-button type="primary" @click="handleCreate">作成</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
  import api from '@/api/v1/api'
  import DataSetTransfer from '@/components/dataset/DatasetTransfer/Index.vue'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'DataSetCreate',
    props: {
      parentId: {
        type: String,
        default: null
      }
    },
    components: {
      'pl-display-error': DisplayError,
      'pl-dataset-transfer': DataSetTransfer
    },
    data () {
      return {
        dialogVisible: true,
        error: null,
        model: {
          name: '',
          memo: '',
          entries: undefined
        },
        dataTypes: [], // new Array<DataType>();
        rules: this.createRules()
      }
    },

    async created () {
      await this.getDataTypes()
    },

    methods: {
      async getDataTypes () {
        try {
          if (!this.parentId) {
            this.dataTypes = (await api.datasets.getDatatypes()).data
            this.model.entries = {}
            this.dataTypes.forEach(type => {
              this.model.entries[type.name] = []
            })
          }

          if (this.parentId) {
            let data = (await api.datasets.getById({id: this.parentId})).data
            this.model.name = data.name
            this.model.memo = data.memo
            let ent = {}
            let types = []
            for (let key in data.entries) {
              ent[key] = data.entries[key]
              types.push({name: key})
            }
            this.model.entries = ent
            this.dataTypes = types
          }
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async handleCreate () {
        let form = this.$refs.createForm
        await form.validate(async (valid) => {
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

      async postDataSet () {
        let postEntries = {}
        for (let key in this.model.entries) {
          postEntries[key] = []
          this.model.entries[key].forEach(entry => {
            postEntries[key].push({
              id: entry.id
            })
          })
        }
        let params = {
          model: {
            entries: postEntries,
            name: this.model.name,
            memo: this.model.memo
          }
        }
        await api.datasets.post(params)
      },

      handleCancel () {
        this.emitCancel()
      },

      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      },

      createRules () {
        return {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          entries: [{
            required: true,
            trigger: 'blur',
            validator (rule, value, callback) {
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
            }
          }]
        }
      }
    }
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
    font-weight: bold !important
  }

  .right-button-group {
    padding-top: 0px;
    text-align: right;
  }
</style>
