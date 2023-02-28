<template>
  <kqi-dialog
    :title="title"
    :type="isCreateDialog ? 'CREATE' : 'EDIT'"
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
        <el-form-item label="メモ" prop="memo">
          <el-input v-model="form.memo" type="textarea" />
        </el-form-item>
        <el-form-item label="配置種別">
          <el-switch v-model="form.isFlat"></el-switch>フラット
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
            <kqi-display-text-form v-if="isLocked" value="不可" />
            <kqi-display-text-form v-else value="可" />
          </el-form-item>
          <el-form-item label="登録者">
            <kqi-display-text-form v-model="detail.createdBy" />
          </el-form-item>
          <el-form-item label="登録日時">
            <kqi-display-text-form v-model="detail.createdAt" />
          </el-form-item>
          <el-form-item label="配置種別">
            <el-switch v-model="detail.isFlat" :disabled="isEditDialog">
            </el-switch
            >フラット
          </el-form-item>
        </el-col>
      </el-row>

      <div v-if="form.isFlat">
        <el-form-item label="データ" prop="flatEntries" />
        <el-form-item>
          <pl-flat-dataset-transfer
            v-if="form.flatEntries"
            v-model="form.flatEntries"
            :is-flat="true"
            :disabled="isLocked"
            @showData="handleShowData"
          />
        </el-form-item>
      </div>
      <div v-else>
        <el-form-item label="データ" prop="entries" />
        <el-form-item>
          <pl-dataset-transfer
            v-if="form.entries"
            v-model="form.entries"
            :is-flat="false"
            :disabled="isLocked"
            @showData="handleShowData"
          />
        </el-form-item>
      </div>
    </el-form>
  </kqi-dialog>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiDialog from '@/components/KqiDialog.vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import DataSetTransfer from './DatasetTransfer/Index.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers(
  'dataSet',
)

import * as gen from '@/api/api.generate'

interface DataType {
  form: {
    name: string | null
    memo: string | null
    isFlat: boolean
    entries: {
      [key: string]: Array<
        gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel
      >
    } | null
    flatEntries: {
      selected: Array<gen.NssolPlatypusApiModelsDataApiModelsIndexOutputModel>
    }
  }
  title: string
  isCreateDialog: boolean
  isCopyCreation: boolean
  isEditDialog: boolean
  isLocked: boolean
  dialogVisible: boolean
  error: Error | null
  rules: {
    [key: string]: [
      {
        required: boolean
        that?: any
        trigger: string
        message?: string
        validator?: Function
      },
    ]
  }
}
export default Vue.extend({
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    'pl-dataset-transfer': DataSetTransfer,
    'pl-flat-dataset-transfer': DataSetTransfer,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data(): DataType {
    return {
      form: {
        name: '',
        memo: '',
        isFlat: false,
        entries: null,
        flatEntries: { selected: [] },
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
        entries: [
          {
            required: true,
            that: this,
            trigger: 'blur',
            validator(rule: any, value: any, callback: Function) {
              let exists = false
              for (let key in value) {
                if (value[key].length > 0) {
                  exists = true
                }
              }
              //@ts-ignore
              if (exists || this.that.form.isFlat) {
                callback()
              } else {
                callback(new Error('必須項目です'))
              }
            },
          },
        ],
        flatEntries: [
          {
            required: true,
            trigger: 'blur',
            that: this,
            validator(rule: any, value: any, callback: Function) {
              let exists = false
              if (value.selected.length > 0) {
                exists = true
              }
              if (exists) {
                callback()
                //@ts-ignore
              } else if (this.that.form.isFlat) {
                callback(new Error('必須項目です'))
              }
            },
          },
        ],
      },
    }
  },
  computed: {
    ...mapGetters(['data', 'detail', 'dataTypes', 'dataTotal']),
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
      'fetchData',
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
      let dialogType = url.split('/')[2] // ["", "dataset", "{dialogType}", "{id}"]
      switch (dialogType) {
        case 'create':
          this.title = 'データセット作成'
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

          this.form.flatEntries = { selected: [] }
          this.form.entries = {}
          this.dataTypes.forEach(
            (
              type: gen.NssolPlatypusApiModelsDataSetApiModelsDataTypeOutputModel,
            ) => {
              this.form.entries![type.name!] = []
            },
          )
          this.error = null
        } catch (e) {
          if (e instanceof Error) {
            this.error = e
          }
        }
      }

      // 編集時/コピー作成時は、既に登録されている情報を各項目を設定
      if (this.isEditDialog || this.isCopyCreation) {
        await this.retrieveData()
      }
    },

    async retrieveData() {
      try {
        await this.fetchDetail(this.id)
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        this.form.isFlat = this.detail.isFlat
        let ent: typeof this.form.entries = {}
        let types = []

        for (let key in this.detail.entries) {
          ent[key] = this.detail.entries[key]
          types.push({ name: key })
        }

        this.form.entries = ent
        this.form.flatEntries = {
          selected: this.detail.flatEntries,
        }
        if (this.isEditDialog) {
          // 編集時は編集可否を設定
          this.isLocked = this.detail.isLocked
        }

        this.setDataTypes(types)
        this.error = null
      } catch (e) {
        if (e instanceof Error) {
          this.error = e
        }
      }
    },

    async submit() {
      let form = this.$refs.createForm
      //@ts-ignore
      await form.validate(async valid => {
        if (valid) {
          try {
            await this.postDataSet()
            this.$emit('done')
            this.error = null
          } catch (e) {
            if (e instanceof Error) {
              this.error = e
            }
          }
        }
      })
    },

    async postDataSet() {
      let postEntries: {
        [key: string]: Array<
          gen.NssolPlatypusApiModelsDataSetApiModelsCreateInputModelEntry
        >
      } = {}
      for (let key in this.form.entries) {
        postEntries[key] = []
        this.form.entries[key].forEach(entry => {
          if (entry.id != undefined) {
            postEntries[key].push({
              id: entry.id,
            })
          }
        })
      }
      let postFlatEntries = this.form.flatEntries.selected

      let params = {
        isFlat: this.form.isFlat,
        flatEntries: postFlatEntries,
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
        if (e instanceof Error) {
          this.error = e
        }
      }
    },

    handleShowData(id: number) {
      this.$router.push(`/data/edit/${id}`)
    },
  },
})
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
.el-transfer {
  width: 100%;
}
.el-transfer > :nth-child(3),
.el-transfer > :nth-child(1) {
  /*.el-transfer-panel で適用させたかったが何故かできない…*/
  width: 40% !important;
}

div.el-transfer
  > :nth-child(1)
  > :nth-child(2)
  > :nth-child(2)
  > :nth-child(n + 1),
div.el-transfer
  > :nth-child(3)
  > :nth-child(2)
  > :nth-child(2)
  > :nth-child(n + 1) {
  /*label.el-checkbox で適用させたかったが何故かできない…*/
  margin-right: 0px !important;
}
</style>
