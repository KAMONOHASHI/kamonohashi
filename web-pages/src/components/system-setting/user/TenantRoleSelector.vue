<template>
  <span>
    <span v-if="multiple">
      <el-select class="selectTenant"
                 v-model="tenants"
                 @change="handleTenantChange"
                 multiple
                 placeholder="Select"
                 value-key="id">
        <el-option v-for="item in tenantTypes" :key="item.id" :label="item.displayName" :value="item"/>
      </el-select>
    </span>
    <span v-else>
      <el-select class="selectTenant"
                 v-model="tenants"
                 @change="handleTenantChange"
                 placeholder="Select"
                 value-key="id"
                 :clearable="true">
        <el-option v-for="item in tenantTypes" :key="item.id" :label="item.displayName" :value="item"/>
      </el-select>
    </span>
    <div v-if="tenantRoles.length">
      <el-table :data="tenantRoles">
        <el-table-column prop="displayName" label="テナント名" width="200px">
          <template slot-scope="prop">
            <el-checkbox v-model="prop.row.default" @change="handleDefaultChange(prop.row)">
              {{prop.row.displayName}}
              <span v-if="prop.row.default" style="font-size:0.7rem;">(デフォルト)</span>
            </el-checkbox>
          </template>
        </el-table-column>
        <el-table-column label="ロール" width="auto">
          <template slot-scope="prop">
            <el-checkbox-group v-model="prop.row.$roles" style="white-space: nowrap;"
                               @change="handleRoleChange(prop.row)">
              <template v-for="r in roleTypes">
                <el-checkbox-button :label="r.id" :key="r.id"
                                    v-if="r.isSystemRole === showSystem && !r.tenantId">
                  {{r.displayName}}
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
  import api from '@/api/v1/api'

  export default {
    name: 'TenantSelector',
    props: ['value', 'multiple'],
    data () {
      return {
        tenantTypes: [], // 選択対象の全テナントリスト
        roleTypes: [],
        showSystem: false, // システムロールを表示するか

        tenants: [], // 選択されているテナント
        tenantRoles: []
      }
    },
    async created () {
      this.tenantTypes = (await api.tenant.admin.get()).data
      this.showSystem = (this.system === true)
      this.roleTypes = (await api.role.admin.get()).data
    },
    methods: {
      async handleTenantChange (v) {
        if (!this.multiple) {
          v = [v]
        }
        // delete
        this.tenantRoles = this.tenantRoles.filter(tr => v.some(vi => vi.id === tr.id))
        // add
        v.forEach(t => {
          let exists = this.tenantRoles.find(tr => t.id === tr.id)
          if (!exists) {
            this.addTenantRoles(t)
          }
        })
        // default check
        if (this.tenantRoles.length >= 1 && this.tenantRoles.every(tr => tr.default === false)) {
          this.tenantRoles[0].default = true
        }
        this.emitInput()
      },
      async handleRoleChange (row) {
        // row.$roles から roles を再構築
        let roles = []
        row.$roles.forEach(r => {
          let role = this.roleTypes.find(rt => rt.id === r)
          if (role) {
            roles.push(role)
          }
        })
        row.roles = roles
        this.emitInput()
      },
      async handleDefaultChange (row) {
        this.tenantRoles.forEach(tr => {
          tr.default = false
        })
        row.default = true
        this.emitInput()
      },
      updateTenants (tenants) {
        tenants.forEach(v => {
          this.tenants.push(v)
          this.addTenantRoles(v)
        })
      },
      addTenantRoles (tenant) {
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
      emitInput () {
        if (this.multiple) {
          this.$emit('input', this.tenantRoles)
        } else {
          this.$emit('input', this.tenantRoles[0])
        }
      }
    }
  }
</script>

<style scoped>
  .selectTenant {
    width: 100%;
  }
</style>
