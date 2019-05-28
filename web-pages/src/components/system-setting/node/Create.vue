<template>
  <el-dialog class="dialog"
             title="ノード作成"
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
      <el-form-item label="TensorBoard 実行可否">
        <el-switch v-model="tensorBoardEnabled"
                   style="width: 100%;"
                   inactive-text="実行しない"
                   active-text="実行する"/>
      </el-form-item>
      <el-row class="right-button-group footer">
        <el-button @click="emitCancel">キャンセル</el-button>
        <el-button type="primary" @click="createNode">作成</el-button>
      </el-row>

    </el-form>
  </el-dialog>
</template>
<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'NodeCreate',
    components: {
      'pl-display-error': DisplayError
    },
    async created () {
      await this.retrieveData()
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
        rules: {
          name: [{
            required: true,
            trigger: 'blur',
            message: '必須項目です'
          }]
        }
      }
    },
    methods: {
      async createNode () {
        let form = this.$refs.createForm

        await form.validate(async (valid) => {
            if (valid) {
              try {
                let params = {
                  name: this.name,
                  memo: this.memo,
                  partition: this.partition,
                  accessLevel: this.accessLevel,
                  assignedTenantIds: this.accessLevel === 1 ? this.selectedTenants : [],
                  tensorBoardEnabled: this.tensorBoardEnabled
                }
                await api.nodes.admin.post({model: params})
                this.emitDone()
                this.error = undefined
              } catch (e) {
                this.error = e
              }
            }
          }
        )
      },
      async retrieveData () {
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

      closeDialog (done) {
        done()
        this.emitCancel()
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitDone () {
        this.showSuccessMessage()
        this.$emit('done')
      }
    }

  }
</script>

<style lang="scss" scoped>
  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }

</style>
