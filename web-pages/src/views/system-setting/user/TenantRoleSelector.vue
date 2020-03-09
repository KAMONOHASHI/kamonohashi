<template>
  <span>
    <el-select
      :value="selectedTenantIds"
      class="selectTenant"
      multiple
      placeholder="Select"
      @change="handleTenantChange"
    >
      <el-option
        v-for="item in tenants"
        :key="item.id"
        :label="item.name"
        :value="item.id"
      />
    </el-select>
    <div v-if="tenantRoles.length">
      <el-table :data="tenantRoles">
        <el-table-column prop="displayName" label="テナント名" width="200px">
          <template slot-scope="prop">
            <el-radio
              v-model="prop.row.default"
              :label="true"
              style="display: block;"
              @change="handleDefaultChange(prop.row)"
            >
              {{ prop.row.displayName }}
              <span v-if="prop.row.default" style="font-size:0.7rem;">
                (デフォルト)
              </span>
            </el-radio>
          </template>
        </el-table-column>
        <el-table-column label="ロール" width="auto">
          <template slot-scope="prop">
            <el-checkbox-group
              v-model="prop.row.$roles"
              style="white-space: nowrap;"
              @change="handleRoleChange(prop.row)"
            >
              <template v-for="r in roles">
                <el-checkbox-button
                  v-if="r.isSystemRole === false && !r.tenantId"
                  :key="r.id"
                  :label="r.id"
                >
                  {{ r.displayName }}
                </el-checkbox-button>
              </template>
            </el-checkbox-group>
          </template>
        </el-table-column>
      </el-table>
    </div>
  </span>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      default: () => {
        return {
          selectedTenantIds: [],
          selectedTenants: [],
        }
      },
    },
    // 選択可能なテナントの配列
    tenants: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択可能なロールの配列
    roles: {
      type: Array,
      default: () => {
        return []
      },
    },
  },
  data() {
    return {
      tenantRoles: [],
    }
  },
  computed: {
    selectedTenantIds() {
      this.handleTenantChange(this.value.selectedTenantIds)
      return this.value.selectedTenantIds
    },
  },
  methods: {
    async handleTenantChange(selectedIds) {
      // selectedIdsと一致するものだけをtenantRolesから抽出し、存在しないものは削除する
      this.tenantRoles = this.tenantRoles.filter(tr =>
        selectedIds.some(id => id === tr.id),
      )

      // 新規のものを追加する
      selectedIds.forEach(id => {
        let exists = this.tenantRoles.find(tr => id === tr.id)
        if (!exists) {
          let selectTenant = this.tenants.find(ti => ti.id === id)
          this.addTenantRoles(selectTenant)
        }
      })

      // デフォルトテナント設定
      if (
        this.tenantRoles.length >= 1 &&
        this.tenantRoles.every(tr => tr.default === false)
      ) {
        this.tenantRoles[0].default = true
      }

      // テナント追加時のデフォルトロール設定
      if (this.tenantRoles.length > 0) {
        if (this.tenantRoles[this.tenantRoles.length - 1].$roles.length === 0) {
          // テナントロールで一番最初のロールをデフォルトとして設定する
          if (this.roles.filter(rt => rt.isSystemRole === false).length > 0) {
            this.tenantRoles[this.tenantRoles.length - 1].$roles.push(
              this.roles.filter(rt => rt.isSystemRole === false)[0].id,
            )
            this.tenantRoles[
              this.tenantRoles.length - 1
            ].roles = this.roles.filter(rt => rt.isSystemRole === false)
          }
        }
      }

      this.emitInput()
    },
    async handleRoleChange(row) {
      // row.$roles から roles を再構築
      let roles = []
      row.$roles.forEach(r => {
        let role = this.roles.find(rt => rt.id === r)
        if (role) {
          roles.push(role)
        }
      })
      row.roles = roles
      this.emitInput()
    },
    async handleDefaultChange(row) {
      this.tenantRoles.forEach(tr => {
        tr.default = false
      })
      row.default = true
      this.emitInput()
    },
    addTenantRoles(tenant) {
      let roles = []
      if (tenant.roles) {
        tenant.roles.forEach(s => {
          roles.push(s.id)
        })
      }
      let copy = {}
      copy['$roles'] = roles
      copy['roles'] = []
      copy['default'] = false
      for (let key in tenant) {
        copy[key] = tenant[key]
      }

      // 編集時に既に設定されているロールおよびデフォルトテナントを反映する
      this.value.selectedTenants.forEach(selectedTenant => {
        if (selectedTenant.id === tenant.id) {
          selectedTenant.roles.forEach(role => {
            copy['$roles'].push(role.id)
          })
          copy['default'] = selectedTenant.default
        }
      })

      this.tenantRoles.push(copy)
    },
    emitInput() {
      let select = this.value
      select.selectedTenantIds = []
      this.tenantRoles.forEach(s => {
        select.selectedTenantIds.push(s.id)
      })
      select.selectedTenants = this.tenantRoles
      this.$emit('input', select)
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}
</style>
