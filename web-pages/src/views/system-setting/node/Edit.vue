<template>
  <kqi-dialog
    :title="title"
    :type="id === null ? 'CREATE' : 'EDIT'"
    @submit="submit"
    @delete="deleteNode"
    @close="$emit('cancel')"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="名前" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
      <el-form-item label="メモ">
        <el-input v-model="form.memo" type="textarea" />
      </el-form-item>
      <el-form-item label="パーティション">
        <el-input v-model="form.partition" />
      </el-form-item>
      <el-form-item label="アクセスレベル">
        <el-radio-group v-model="form.accessLevel" style="width: 100%;">
          <el-radio-button :label="0">Disabled</el-radio-button>
          <el-radio-button :label="1">Private</el-radio-button>
          <el-radio-button :label="2">Public</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <transition name="el-fade-in-linear">
        <el-transfer
          v-if="form.accessLevel === 1"
          v-model="form.selectedTenants"
          :data="displayTenants"
          :titles="['アクセス拒否', 'アクセス許可']"
        />
      </transition>
      <el-form-item label="TensorBoard">
        <el-switch
          v-model="form.tensorBoardEnabled"
          style="width: 100%;"
          inactive-text="実行しない"
          active-text="実行する"
        />
      </el-form-item>
      <el-form-item label="Notebook">
        <el-switch
          v-model="form.notebookEnabled"
          style="width: 100%;"
          inactive-text="実行しない"
          active-text="実行する"
        />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDialog from '@/components/KqiDialog.vue'
import KqiDisplayError from '@/components/KqiDisplayError.vue'
import { mapGetters, mapActions } from 'vuex'

import * as gen from '@/api/api.generate'
const formRules = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

interface DataType {
  form: {
    name: null | string
    memo: null | string
    partition: null
    accessLevel: number
    selectedTenants: [] // Selected tenants which can access this node.
    tensorBoardEnabled: null | boolean
    notebookEnabled: null | boolean
  }
  title: string
  displayTenants: Array<{ key?: null | number; label?: null | string }>
  error: null | Error
  rules: {
    name: Array<typeof formRules>
  }
}

export default Vue.extend({
  components: {
    KqiDialog,
    KqiDisplayError,
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
        name: null,
        memo: null,
        partition: null,
        accessLevel: 2,
        selectedTenants: [], // Selected tenants which can access this node.
        tensorBoardEnabled: null,
        notebookEnabled: null,
      },
      title: '',
      displayTenants: [],
      error: null,
      rules: {
        name: [formRules],
      },
    }
  },
  computed: {
    ...mapGetters({
      //@ts-ignore
      detail: ['node/detail'],
      tenants: ['tenant/tenants'],
    }),
  },
  async created() {
    await this['tenant/fetchTenants']()
    this.tenants.sort(
      (
        a: gen.NssolPlatypusApiModelsTenantApiModelsIndexOutputModel,
        b: gen.NssolPlatypusApiModelsTenantApiModelsIndexOutputModel,
      ) => {
        let a_ = a.displayName!.toString().toLowerCase()
        let b_ = b.displayName!.toString().toLowerCase()
        return a_ < b_ ? -1 : 1
      },
    )
    this.tenants.forEach(
      (t: gen.NssolPlatypusApiModelsTenantApiModelsIndexOutputModel) => {
        this.displayTenants.push({
          key: t.id,
          label: t.displayName,
        })
      },
    )

    if (this.id === null) {
      this.title = 'ノード登録'
    } else {
      this.title = 'ノード編集'
      try {
        await this['node/fetchDetail'](this.id)
        this.form.name = this.detail.name
        this.form.memo = this.detail.memo
        this.form.partition = this.detail.partition
        this.form.accessLevel = this.detail.accessLevel
        this.form.selectedTenants = this.detail.assignedTenants
          ? this.detail.assignedTenants.map(
              (
                t: gen.NssolPlatypusApiModelsNodeApiModelsDetailsOutputModelAssignedTenant,
              ) => {
                return t.id
              },
            )
          : []
        this.form.tensorBoardEnabled = this.detail.tensorBoardEnabled
        this.form.notebookEnabled = this.detail.notebookEnabled
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    }
  },
  methods: {
    ...mapActions([
      'node/fetchDetail',
      'node/post',
      'node/put',
      'node/delete',
      'tenant/fetchTenants',
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
              partition: this.form.partition,
              accessLevel: this.form.accessLevel,
              assignedTenantIds:
                this.form.accessLevel === 1 ? this.form.selectedTenants : [],
              tensorBoardEnabled: this.form.tensorBoardEnabled,
              notebookEnabled: this.form.notebookEnabled,
            }
            if (this.id === null) {
              await this['node/post'](params)
            } else {
              await this['node/put']({ id: this.id, params: params })
            }
            this.$emit('done')
            this.error = null
          } catch (e) {
            if (e instanceof Error) this.error = e
          }
        }
      })
    },

    async deleteNode() {
      try {
        await this['node/delete'](this.id)
        this.error = null
        this.$emit('done', 'delete')
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },
  },
})
</script>

<style lang="scss" scoped></style>
