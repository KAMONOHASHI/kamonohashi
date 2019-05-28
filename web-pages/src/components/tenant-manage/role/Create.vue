<template>
  <el-dialog class="dialog"
             title="ロール作成"
             :visible.sync="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">
    <el-form ref="createForm" :model="form" :rules="rules">
      <pl-display-error :error="error" />
      <el-form-item label="ロール名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="表示名" prop="displayName">
        <el-input v-model="form.displayName" />
      </el-form-item>
      <el-form-item label="ソート順" prop="sortOrder">
        <br />
        <el-input-number v-model="form.sortOrder" controls-position="right" />
        並び順。小さいほど前に表示される。一意性は不要。
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
import DisplayError from '@/components/common/DisplayError'

export default {
  name: 'ManageRoleCreate',
  components: {
    'pl-display-error': DisplayError
  },
  data () {
    return {
      dialogVisible: true,
      error: null,
      roleTypes: [
        { label: 'テナント', value: false }
      ],
      form: {
        name: '',
        displayName: '',
        sortOrder: 0
      },
      rules: {
        name: [{required: true, trigger: 'blur', message: '必須項目です'}],
        displayName: [{required: true, trigger: 'blur', message: '必須項目です'}],
        sortOrder: [{required: true, trigger: 'blur', message: '必須項目です'}]
      }
    }
  },

  methods: {
    async handleCreate () {
      let form = this.$refs.createForm
      await form.validate(async (valid) => {
        if (valid) {
          try {
            await this.postRole()
            this.emitDone()
            this.error = null
          } catch (e) {
            this.error = e
          }
        }
      })
    },

    async postRole () {
      let params = {
        model: {
          name: this.form.name,
          displayName: this.form.displayName,
          sortOrder: this.form.sortOrder
        }
      }
      await api.role.tenant.post(params)
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
    }
  }
}
</script>

<style lang="scss" scoped>
  .dialog /deep/ label {
    font-weight: bold !important
  }
  .right-button-group {
    padding-top: 25px;
    text-align: right;
  }
</style>
