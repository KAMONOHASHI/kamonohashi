<template>
  <span>
    <el-select
      :value="selectedTenantsId"
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
              <span v-if="prop.row.default" style="font-size:0.7rem;"
                >(デフォルト)</span
              >
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
import { mapGetters } from 'vuex'
export default {
  props: {
    value: {
      type: Object,
      default: () => {
        return {
          selectedTenantsId: [],
          selectedTenants: [],
        }
      },
    },
  },
  data() {
    return {
      tenantRoles: [],
    }
  },
  computed: {
    ...mapGetters({
      tenants: ['tenant/tenants'],
      roles: ['role/roles'],
    }),
    selectedTenantsId() {
      this.value.selectedTenants.forEach(st => {
        if (st.id !== this.tenantRoles.id) {
          this.addTenantRoles(st)
        }
      })
      return this.value.selectedTenantsId
    },
  },
  methods: {
    async handleTenantChange(v) {
      // delete
      this.tenantRoles = this.tenantRoles.filter(tr =>
        v.some(vi => vi === tr.id),
      )
      // add
      v.forEach(t => {
        let exists = this.tenantRoles.find(tr => t === tr.id)
        if (!exists) {
          let selectTenant = this.tenants.find(ti => ti.id === t)
          this.addTenantRoles(selectTenant)
        }
      })
      // default check
      if (
        this.tenantRoles.length >= 1 &&
        this.tenantRoles.every(tr => tr.default === false)
      ) {
        this.tenantRoles[0].default = true
      }
      // テナント追加時のデフォルトロール設定
      if (this.tenantRoles.length > 0) {
        if (this.tenantRoles[this.tenantRoles.length - 1].roles.length === 0) {
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
      this.tenantRoles.push(copy)
    },
    emitInput() {
      let select = this.value
      select.selectedTenantsId = []
      select.selectedTenants = []
      this.tenantRoles.forEach(s => {
        select.selectedTenantsId.push(s.id)
        select.selectedTenants.push(this.tenantRoles.filter(t => t.id === s.id))
      })
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
