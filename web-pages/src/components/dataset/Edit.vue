<template>
  <el-dialog class="dialog"
             title="データセット編集"
             :visible.sync="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-row>
      <el-col :span="24" class="right-button-group">
        <el-button @click="handleCopy">コピー</el-button>
      </el-col>
    </el-row>

    <el-form ref="updateForm" :model="model" :rules="rules">
      <pl-display-error :error="error"/>
      <el-row>
        <el-col :span="12">
          <el-form-item label="ID">
            <pl-display-text-form v-model="id"/>
          </el-form-item>
          <el-form-item label="データセット名" prop="name">
            <el-input v-model="model.name"/>
          </el-form-item>
          <el-form-item label="メモ" prop="memo">
            <el-input v-model="model.memo" type="textarea"/>
          </el-form-item>
        </el-col>
        <el-col :offset="1" :span="11">
          <el-form-item label="編集可否">
            <pl-display-text-form value='不可' v-if="model.isLocked"/>
            <pl-display-text-form value='可能' v-else/>
          </el-form-item>
          <el-form-item label="登録者">
            <pl-display-text-form v-model="model.createdBy"/>
          </el-form-item>
          <el-form-item label="登録日時">
            <pl-display-text-form v-model="model.createdAt"/>
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item label="データ" prop="entries">
      </el-form-item>
      <el-form-item v-if="model.entries">
        <pl-dataset-transfer v-model="model.entries" :disabled="model.isLocked"></pl-dataset-transfer>
      </el-form-item>
      <el-row class="footer">
        <el-col class="left-button-group" :span="12">
          <pl-delete-button @delete="handleDelete"/>
        </el-col>
        <el-col class="right-button-group" :span="12">
          <el-button @click="handleCancel">キャンセル</el-button>
          <el-button type="primary" @click="handleUpdate">更新</el-button>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
  import DataSetTransfer from '@/components/dataset/DatasetTransfer/Index.vue'
  import DisplayError from '@/components/common/DisplayError'
  import DisplayTextForm from '@/components/common/DisplayTextForm'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'DataSetCreate',
    components: {
      'pl-display-text-form': DisplayTextForm,
      'pl-display-error': DisplayError,
      'pl-dataset-transfer': DataSetTransfer,
      'pl-delete-button': DeleteButton
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: null,
        model: {
          name: '',
          memo: '',
          createdBy: '',
          createdAt: '',
          entries: undefined,
          isLocked: false
        },
        rules: {
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
    },

    async created () {
      await this.retrieveData()
    },

    watch: {
      id: async function () {
        await this.retrieveData()
      }
    },

    methods: {
      async retrieveData () {
        try {
          let data = (await api.datasets.getById({id: this.id})).data
          this.model.name = data.name
          this.model.memo = data.memo
          this.model.createdBy = data.createdBy
          this.model.createdAt = data.createdAt
          this.model.entries = data.entries
          this.model.isLocked = data.isLocked
        } catch (e) {
          this.error = e
        }
      },

      async handleUpdate () {
        let form = this.$refs.updateForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              if (this.model.isLocked) {
                await this.patchDataSet()
              } else {
                await this.putDataSet()
              }
              this.emitDone()
              this.error = null
            } catch (e) {
              this.error = e
            }
          }
        })
      },

      async handleDelete () {
        try {
          await this.deleteDataSet()
          this.emitDone()
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
      },

      async putDataSet () {
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
          id: this.id,
          model: {
            entries: postEntries,
            name: this.model.name,
            memo: this.model.memo
          }
        }
        await api.datasets.put(params)
      },

      async patchDataSet () {
        let params = {
          id: this.id,
          model: {
            name: this.model.name,
            memo: this.model.memo
          }
        }
        await api.datasets.patch(params)
      },

      async deleteDataSet () {
        let params = {
          id: this.id
        }
        await api.datasets.delete(params)
      },

      async handleCopy () {
        this.emitCopy(this.id)
      },

      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      },

      emitCopy (id) {
        this.$emit('copy', id)
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
