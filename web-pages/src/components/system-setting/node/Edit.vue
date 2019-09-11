<template>
  <el-dialog class="dialog"
             title="ノード編集"
             :visible.sync="dialogVisible"
             :before-close="closeDialog"
             :close-on-click-modal="false">
    <el-form ref="createForm" :model="this" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="名前" prop="name">
        <el-input v-model="name"/>
      </el-form-item>
      <el-form-item label="メモ">
        <el-input type="textarea" v-model="memo"/>
      </el-form-item>
      <el-form-item label="パーティション">
        <el-input v-model="partition"/>
      </el-form-item>
      <el-form-item label="アクセスレベル">
        <el-radio-group v-model="accessLevel" style="width:100%;">
          <el-radio-button :label="0">Disabled</el-radio-button>
          <el-radio-button :label="1">Private</el-radio-button>
          <el-radio-button :label="2">Public</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <transition name="el-fade-in-linear">
        <el-transfer v-model="selectedTenants" :data="tenants" :titles="titles"
                     v-if="this.accessLevel === 1"></el-transfer>
      </transition>
      <el-form-item label="TensorBoard">
        <el-switch v-model="tensorBoardEnabled"
                   style="width: 100%;"
                   inactive-text="実行しない"
                   active-text="実行する"/>
      </el-form-item>
      <el-form-item label="Notebook">
        <el-switch v-model="notebookEnabled"
                   style="width: 100%;"
                   inactive-text="実行しない"
                   active-text="実行する"/>
      </el-form-item>
      <el-row :gutter="20" class="footer">
        <el-col :span="12">
          <pl-delete-button @delete="deleteNode"/>
        </el-col>
        <el-col class="right-button-group" :span="12">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button type="primary" @click="updateNode">保存</el-button>
        </el-col>
      </el-row>
    </el-form>

  </el-dialog>
</template>

<script>
  import DisplayError from '@/components/common/DisplayError'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'NodeEdit',
    components: {
      'pl-display-error': DisplayError,
      'pl-delete-button': DeleteButton
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: undefined,
        name: undefined,
        memo: undefined,
        partition: undefined,
        accessLevel: undefined,
        selectedTenants: [], // Selected tenants which can access this node.
        tenants: [], // Tenants to display on a transfer component.
        titles: ['アクセス拒否', 'アクセス許可'], // The title of the transfer component.
        tensorBoardEnabled: undefined,
        notebookEnabled: undefined,
        rules: {
          name: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }]
        }
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async updateNode () {
        let form = this.$refs.createForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              let params = {
                id: this.id,
                model: {
                  name: this.name,
                  memo: this.memo,
                  partition: this.partition,
                  accessLevel: this.accessLevel,
                  assignedTenantIds: this.accessLevel === 1 ? this.selectedTenants : [],
                  tensorBoardEnabled: this.tensorBoardEnabled,
                  notebookEnabled: this.notebookEnabled
                }
              }
              await api.nodes.admin.put(params)
              this.emitDone()
              this.error = undefined
            } catch (e) {
              this.error = e
            }
          }
        })
      },
      async retrieveData () {
        let result = (await api.nodes.admin.getById({id: this.id})).data
        this.name = result.name
        this.memo = result.memo
        this.partition = result.partition
        this.accessLevel = result.accessLevel
        this.selectedTenants = result.assignedTenants ? result.assignedTenants.map(t => {
          return t.id
        }) : []
        this.tensorBoardEnabled = result.tensorBoardEnabled
        this.notebookEnabled = result.notebookEnabled

        // retrieve tenant to set up a transfer list.
        let allTenants = (await api.tenant.admin.get()).data
        allTenants.forEach(t => {
          if (this.selectedTenants.every(s => s.id !== t.id)) {
            this.tenants.push({
              key: t.id,
              label: t.displayName
            })
          }
        })
      },
      async deleteNode () {
        try {
          await api.nodes.admin.delete({id: this.id})
          this.emitDone()
          this.error = undefined
        } catch (e) {
          this.error = e
        }
      },
      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
        this.dialogVisible = false
      },
      emitCancel () {
        this.$emit('cancel')
      },
      closeDialog (done) {
        done()
        this.emitCancel()
      }
    }
  }
</script>

<style lang="scss" scoped>
  .right-button-group {
    text-align: right;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .footer {
    padding-top: 40px;
  }

</style>
