<template>
  <el-dialog class="dialog"
             title="ロール編集"
             :visible.sync="dialogVisible"
             :before-close="handleCancel"
             :close-on-click-modal="false">

    <el-form ref="updateForm" :model="form" :rules="rules">
      <pl-display-error :error="error"/>
      <el-form-item label="ロール名" prop="name">
        <el-input v-model="form.name"/>
      </el-form-item>
      <el-form-item label="表示名" prop="displayName">
        <el-input v-model="form.displayName"/>
      </el-form-item>
      <el-form-item label="種別" prop="isSystemRole">
        <el-input v-if="tenantName" v-model="tenantName" :disabled="true"/>
        <el-select v-else v-model="form.isSystemRole" placeholder="Select" style="width:100%;" :clearable="true"
                   disabled>
          <el-option
            v-for="item in roleTypes"
            :key="item.value"
            :label="item.label"
            :value="item.value">
          </el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="ソート順" prop="sortOrder">
        <br/>
        <el-input-number v-model="form.sortOrder" controls-position="right" style="vertical-align: middle;"/>
        並び順。小さいほど前に表示される。一意性は不要。
      </el-form-item>
      <el-row :gutter="20" class="footer">
        <el-col :span="12">
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
  import DisplayError from '@/components/common/DisplayError'
  import DisplayTextForm from '@/components/common/DisplayTextForm'
  import DeleteButton from '@/components/common/DeleteButton.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'DataSetCreate',
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
        tenantName: null,
        error: null,
        roleTypes: [
          {label: 'テナント(共通)', value: false},
          {label: 'システム', value: true}
        ],
        form: {
          name: '',
          displayName: '',
          isSystemRole: false,
          sortOrder: 0
        },
        rules: {
          name: [{required: true, trigger: 'blur', message: '必須項目です'}],
          displayName: [{required: true, trigger: 'blur', message: '必須項目です'}],
          isSystemRole: [{required: true, trigger: 'blur', message: '必須項目です'}],
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
          let data = (await api.role.admin.getById({id: this.id})).data
          this.form.name = data.name
          this.form.displayName = data.displayName
          this.form.isSystemRole = data.isSystemRole
          this.form.sortOrder = data.sortOrder
          this.error = null
          if (data.tenantName) {
            this.tenantName = 'テナント(カスタム) / ' + data.tenantName
          }
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
            isSystemRole: this.form.isSystemRole,
            sortOrder: this.form.sortOrder
          }
        }
        await api.role.admin.put(params)
      },

      async deleteRole () {
        let params = {
          id: this.id
        }
        await api.role.admin.delete(params)
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
  .dialog /deep/ label {
    font-weight: bold !important
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }
</style>
