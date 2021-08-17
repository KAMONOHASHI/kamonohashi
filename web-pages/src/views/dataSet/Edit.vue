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

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import DataSetTransfer from './DatasetTransfer/Index'
//import { createNamespacedHelpers } from 'vuex'
//const { mapGetters, mapMutations, mapActions } = createNamespacedHelpers(
//  'dataSet',
//)
import { mapActions, mapMutations, mapGetters } from 'vuex'

export default {
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
  data() {
    return {
      form: {
        name: '',
        memo: '',
        isFlat: false,
        entries: null,
        flatEntries: [],
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
            validator(rule, value, callback) {
              let exists = false
              for (let key in value) {
                if (value[key].length > 0) {
                  exists = true
                }
              }
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
            validator(rule, value, callback) {
              let exists = false
              if (value.selected.length > 0) {
                exists = true
              }
              if (exists) {
                callback()
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
    ...mapGetters({
      data: ['dataSet/data'],
      detail: ['dataSet/detail'],
      dataTypes: ['dataSet/dataTypes'],
      dataTotal: ['dataSet/dataTotal'],
      account: ['account/account'],
    }),
  },
  watch: {
    async $route() {
      // 通常の作成とコピー作成が同一コンポーネントのため、コピー作成の実行はrouterの変化により検知する
      await this.initialize()
    },
  },

  async created() {
    let tenantName = this.$route.query.tenantName
    await this['account/fetchAccount']()
    //テナント名からテナントIDを取得し、セットする
    for (let i in this.account.tenants) {
      if (this.account.tenants[i].name == tenantName) {
        await sessionStorage.setItem(
          '.Platypus.Tenant',
          this.account.tenants[i].id,
        )
      }
    }

    await this.initialize()
  },

  methods: {
    ...mapActions([
      'dataSet/fetchData',
      'dataSet/fetchDetail',
      'dataSet/fetchDataTypes',
      'dataSet/post',
      'dataSet/put',
      'dataSet/patch',
      'dataSet/delete',
      'account/fetchAccount',
    ]),
    ...mapMutations(['dataSet/setDataTypes']),
    async initialize() {
      let url = this.$route.path
      let type = url.split('/')[2] // ["", "dataset", "{type}", "{id}"]
      switch (type) {
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
          await this['dataSet/fetchDataTypes']()
          this.form.entries = {}
          this.form.flatEntries = {}
          this.dataTypes.forEach(type => {
            this.form.entries[type.name] = []
            this.form.flatEntries['selected'] = []
          })
          this.error = null
        } catch (e) {
          this.error = e
        }
      }
      this.dataViewInfo = this.makeViewInfo({
        colorIndex: 0,
        showAssign: true,
      })
      // 編集時/コピー作成時は、既に登録されている情報を各項目を設定
      if (this.isEditDialog || this.isCopyCreation) {
        await this.retrieveData()
      }
    },
    makeViewInfo(optionalProps) {
      return Object.assign(optionalProps, this.defaultViewInfo)
    },
    async retrieveData() {
      this.form.entries = null
      try {
        await this['dataSet/fetchDetail'](this.id)
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        this.form.isFlat = this.detail.isFlat
        let ent = {}
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

        this['dataSet/setDataTypes'](types)
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
        await this['dataSet/post'](params)
      } else {
        // 編集
        if (this.isLocked) {
          // 編集不可の時は、名前とメモのみ編集可
          await this['dataSet/patch']({ id: this.id, params: params })
        } else {
          // 編集可の時は、データも編集可
          await this['dataSet/put']({ id: this.id, params: params })
        }
      }
    },

    async deleteDataSet() {
      try {
        await this['dataSet/delete'](this.id)
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
