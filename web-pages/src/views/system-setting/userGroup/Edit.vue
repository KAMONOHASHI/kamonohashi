<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteUserGroup"
    @close="emitCancel"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <kqi-display-text-form v-if="id !== null" label="ID" :value="id" />
      <el-form-item label="ユーザグループ名" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="種別" prop="isGroup">
        <el-switch
          v-model="form.isGroup"
          style="width: 100%;"
          inactive-text="OU"
          active-text="グループ"
          class="left-margin"
        />
      </el-form-item>
      <el-form-item label="DN" prop="dn">
        <el-input v-model="form.dn" />
      </el-form-item>
      <el-form-item label="DN直下のユーザのみ許可するか" prop="isDirect">
        <el-switch
          v-model="form.isDirect"
          style="width: 100%;"
          inactive-text="No"
          active-text="Yes"
          class="left-margin"
        />
      </el-form-item>
      <el-form-item label="テナントロール" prop="roleIds">
        <kqi-role-selector
          v-model="form.roleIds"
          :roles="tenantRoles"
          :show-system-role="false"
        />
      </el-form-item>
      <el-form-item label="メモ" prop="memo">
        <el-input v-model="form.memo" type="textarea" />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDialog from '@/components/KqiDialog.vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiRoleSelector from '@/components/selector/KqiRoleSelector.vue'
import { mapGetters, mapActions } from 'vuex'
import * as gen from '@/api/api.generate'

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

interface DataType {
  form: {
    name: string
    memo: string
    isGroup: boolean
    dn: string
    isDirect: boolean
    roleIds: Array<number>
  }
  title: string
  error: null | Error
  rules: {
    name: Array<typeof formRule>
    memo: null | string
    isGroup: Array<typeof formRule>
    dn: Array<typeof formRule>
    isDirect: Array<typeof formRule>
    roleIds: Array<typeof formRule>
  }
}

export default Vue.extend({
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiRoleSelector,
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
        isGroup: true,
        dn: '',
        isDirect: true,
        roleIds: [],
      },
      title: '',
      error: null,
      rules: {
        name: [formRule],
        memo: null,
        isGroup: [formRule],
        dn: [formRule],
        isDirect: [formRule],
        roleIds: [formRule],
      },
    }
  },
  computed: {
    ...mapGetters({
      //@ts-ignore
      detail: ['userGroup/detail'],
      tenantRoles: ['role/tenantRoles'],
    }),
  },
  async created() {
    if (this.id === null) {
      this.title = 'ユーザグループ登録'
    } else {
      this.title = 'ユーザグループ編集'
      try {
        await this['userGroup/fetchDetail'](this.id)
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        this.form.isGroup = this.detail.isGroup
        this.form.dn = this.detail.dn
        this.form.isDirect = this.detail.isDirect
        this.detail.roles.forEach(
          (r: gen.NssolPlatypusInfrastructureInfosRoleInfo) => {
            this.form.roleIds.push(r.id!)
          },
        )
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    }
    this['role/fetchTenantCommonRoles']()
  },
  methods: {
    ...mapActions([
      'userGroup/fetchDetail',
      'userGroup/delete',
      'userGroup/put',
      'userGroup/post',
      'role/fetchTenantCommonRoles',
    ]),
    async submit() {
      let form = this.$refs.createForm
      //@ts-ignore
      await form.validate(async valid => {
        if (valid) {
          try {
            let params = {
              name: this.form.name,
              memo: this.form.memo,
              isGroup: this.form.isGroup,
              dn: this.form.dn,
              isDirect: this.form.isDirect,
              roleIds: this.form.roleIds,
            }
            if (this.id === null) {
              await this['userGroup/post'](params)
            } else {
              await this['userGroup/put']({ id: this.id, params: params })
            }
            this.error = null
            this.emitDone()
          } catch (e) {
            if (e instanceof Error) this.error = e
          }
        }
      })
    },
    async deleteUserGroup() {
      try {
        await this['userGroup/delete'](this.id)
        this.error = null
        this.emitDone()
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
    emitDone() {
      this.$emit('done')
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
})
</script>
