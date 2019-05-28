<template>
  <el-dialog class="dialog"
             title="ロール編集"
             :visible.sync="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form ref="updateForm" :model="form" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="ロール名" prop="name">
        <el-input v-model="form.name" :disabled="!isCustomRole"/>
      </el-form-item>
      <el-form-item label="表示名" prop="displayName">
        <el-input v-model="form.displayName" :disabled="!isCustomRole"/>
      </el-form-item>
      <el-form-item label="ソート順" prop="sortOrder">
        <br/>
        <el-input-number v-model="form.sortOrder" controls-position="right" :disabled="!isCustomRole"/>
        並び順。小さいほど前に表示される。一意性は不要。
      </el-form-item>
      <el-row v-if="isCustomRole">
        <el-col class="button-group">
          <el-button @click="handleUpdate" class="pull-right btn-update" type="primary" icon="el-icon-edit-outline">保存
          </el-button>
          <el-button @click="handleCancel" class="pull-right btn-cancel" icon="el-icon-close">キャンセル</el-button>
          <pl-delete-button class="pull-left btn-update" @delete="handleDelete"/>
        </el-col>
      </el-row>
      <el-row v-else>
        <el-col class="button-group">
          <el-button @click="handleCancel" class="pull-right btn-cancel" icon="el-icon-close">戻る</el-button>
        </el-col>
      </el-row>
    </el-form>
  </el-dialog>
</template>

<script>
  import DisplayError from '@/components/common/DisplayError'
  import DisplayTextForm from '@/components/common/DisplayTextForm'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'ManageRoleEdit',
    components: {
      'pl-display-text-form': DisplayTextForm,
      'pl-display-error': DisplayError,
      'pl-delete-button': DeleteButton
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: null,
        isCustomRole: false,
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

    async created () {
      await this.retrieveData()
    },

    methods: {
      async retrieveData () {
        try {
          let data = (await api.role.tenant.getById({id: this.id})).data
          this.form.name = data.name
          this.form.displayName = data.displayName
          this.form.sortOrder = data.sortOrder
          this.error = null
          this.isCustomRole = data.tenantId != null
        } catch (e) {
          this.error = e
        }
      },

      async handleUpdate () {
        let form = this.$refs.updateForm
        await form.validate(async (valid) => {
          if (valid) {
            try {
              await this.putRole()
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
          await this.deleteRole()
          this.emitDone()
        } catch (e) {
          this.error = e
        }
      },

      async handleCancel () {
        this.emitCancel()
      },

      async putRole () {
        let params = {
          id: this.id,
          model: {
            name: this.form.name,
            displayName: this.form.displayName,
            sortOrder: this.form.sortOrder
          }
        }
        await api.role.tenant.put(params)
      },

      async deleteRole () {
        let params = {
          id: this.id
        }
        await api.role.tenant.delete(params)
      },

      emitDone () {
        this.$emit('done')
      },

      emitCancel () {
        this.$emit('cancel')
      }
    }
  }
</script>

<style lang="scss" scoped>
  .button-group {
    text-align: right;
    padding-top: 10px;
  }

  .btn-update {
    margin-left: 10px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .pull-right {
    float: right !important;
  }

  .pull-left {
    float: left !important;
  }
</style>
