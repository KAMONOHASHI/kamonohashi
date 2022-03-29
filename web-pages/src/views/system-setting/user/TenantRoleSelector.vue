<template>
  <span>
    <el-form-item label="テナント" prop="tenants">
      <el-select
        :value="value.selectedTenantIds"
        class="selectTenant"
        multiple
        placeholder="Select"
        @change="handleTenantChange"
      >
        <el-option
          v-for="item in tenants"
          :key="item.id"
          :label="item.displayName"
          :value="item.id"
        />
      </el-select>
      <div>
        <el-table v-if="rolesOfTenant.length !== 0" :data="rolesOfTenant">
          <el-table-column prop="displayName" label="テナント名" width="200px">
            <template slot-scope="prop">
              <el-radio
                v-model="prop.row.default"
                :label="true"
                style="display: block;"
                @change="handleDefaultChange(prop.row)"
              >
                {{ prop.row.tenantName }}
                <span v-if="prop.row.default" style="font-size: 0.7rem;">
                  (デフォルト)
                </span>
              </el-radio>
            </template>
          </el-table-column>
          <el-table-column label="ロール" width="auto">
            <template slot-scope="prop">
              <el-checkbox-group
                v-model="prop.row.selectedRoleIds"
                style="white-space: nowrap;"
                @change="handleRoleChange(prop.row)"
              >
                <template v-for="role in prop.row.roles">
                  <el-checkbox-button :key="role.id" :label="role.id">
                    {{ role.displayName }}
                  </el-checkbox-button>
                </template>
              </el-checkbox-group>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-form-item>

    <div v-if="noOriginRolesOfTenant.length > 0">
      <br />
      <label>ユーザグループ経由での所属しているテナント</label>
      <el-table :data="noOriginRolesOfTenant">
        <el-table-column prop="displayName" label="テナント名" width="200px">
          <template slot-scope="prop">
            <el-radio
              v-model="prop.row.default"
              :label="true"
              style="display: block;"
              @change="handleDefaultChange(prop.row)"
            >
              {{ prop.row.tenantName }}
              <span v-if="prop.row.default" style="font-size: 0.7rem;">
                (デフォルト)
              </span>
            </el-radio>
          </template>
        </el-table-column>
        <el-table-column label="ロール" width="auto">
          <template slot-scope="prop">
            <el-checkbox-group
              v-model="prop.row.selectedRoleIds"
              style="white-space: nowrap; cursor: not-allowed;"
            >
              <template v-for="role in prop.row.roles">
                <el-checkbox-button
                  :key="role.id"
                  class="checkbox-role"
                  :label="role.id"
                  style="pointer-events: none; opacity: 0.7;"
                >
                  {{ role.displayName }}
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
          selectedTenantIds: [], // 選択されたテナントID一覧
          selectedTenants: [], // 各テナントごとのロール
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
    // LDAP経由で所属しているテナント情報
    notOriginTenants: {
      type: Object,
      default: () => {
        return {
          selectedTenantIds: [],
          selectedTenants: [],
        }
      },
    },
  },
  computed: {
    // テナントごとのロール設定
    // rolesOfTenant: [
    // {
    //   tenantId: Number,
    //   tenantName: String,
    //   roles: [{},...],
    //   selectedRoleIds: [1,...],
    // },
    // ...
    // ]
    rolesOfTenant() {
      let rolesOfTenant = []
      this.tenants.forEach(tenant => {
        // 選択済みのテナントについて、それぞれ設定
        if (this.value.selectedTenantIds.includes(tenant.id)) {
          // システムロール以外のロールの抽出
          let tmpRoles = []
          this.roles.forEach(role => {
            if (!role.isSystemRole) {
              tmpRoles.push(role)
            }
          })

          // 選択済みのロールの設定
          let selectedRoleIds = []
          let isDefaultTenant = false
          this.value.selectedTenants.forEach(selectedTenant => {
            if (selectedTenant.tenantId === tenant.id) {
              selectedTenant.selectedRoleIds.forEach(id => {
                selectedRoleIds.push(id)
              })
              isDefaultTenant = selectedTenant.default
            }
          })

          let element = {
            tenantId: tenant.id,
            tenantName: tenant.displayName,
            roles: tmpRoles,
            selectedRoleIds: selectedRoleIds,
            default: isDefaultTenant,
          }
          rolesOfTenant.push(element)
        }
      })
      return rolesOfTenant
    },
    // LDAP経由で所属しているテナントのロール情報
    noOriginRolesOfTenant() {
      let noOriginRolesOfTenant = []
      this.tenants.forEach(tenant => {
        // 選択済みのテナントについて、それぞれ設定
        if (this.notOriginTenants.selectedTenantIds.includes(tenant.id)) {
          // システムロール以外のロールの抽出
          let tmpRoles = []
          this.roles.forEach(role => {
            if (!role.isSystemRole) {
              tmpRoles.push(role)
            }
          })

          // 選択済みのロールの設定
          let selectedRoleIds = []
          let isDefaultTenant = false
          this.notOriginTenants.selectedTenants.forEach(selectedTenant => {
            if (selectedTenant.tenantId === tenant.id) {
              selectedTenant.selectedRoleIds.forEach(id => {
                selectedRoleIds.push(id)
              })
              isDefaultTenant = selectedTenant.default
            }
          })

          let element = {
            tenantId: tenant.id,
            tenantName: tenant.displayName,
            roles: tmpRoles,
            selectedRoleIds: selectedRoleIds,
            default: isDefaultTenant,
          }
          noOriginRolesOfTenant.push(element)
        }
      })
      return noOriginRolesOfTenant
    },
  },
  methods: {
    async handleTenantChange(selectedIds) {
      // selectedTenantIdsの変更をemit
      let select = this.value
      select.selectedTenantIds = selectedIds

      // selectedTenantsに存在しなければテナント情報を追加する
      selectedIds.forEach(id => {
        let index = select.selectedTenants.findIndex(tenant => {
          return tenant.tenantId === id
        })
        if (index < 0) {
          let tenantInfo = this.rolesOfTenant.find(tenant => {
            return tenant.tenantId === id
          })
          // rolesの一番初めの要素を初期選択ロールとする
          tenantInfo.selectedRoleIds = [tenantInfo.roles[0].id]
          select.selectedTenants.push(tenantInfo)
        }
      })

      // selectedTenantsのIDで、selectedIdsの中に存在しないものがあれば削除する
      let removeTenantIndexList = []
      select.selectedTenants.forEach((tenant, tenantIndex) => {
        let index = selectedIds.indexOf(tenant.tenantId)
        if (index < 0) {
          removeTenantIndexList.push(tenantIndex)
        }
      })
      removeTenantIndexList.forEach(index => {
        select.selectedTenants.splice(index, 1)
      })

      // デフォルトが指定されていなければ、先頭をデフォルトに指定
      let selectNotOrigin = Object.assign({}, this.notOriginTenants)
      let isDefaultSelected = false
      select.selectedTenants.forEach(tenant => {
        if (tenant.default) {
          isDefaultSelected = true
        }
      })
      selectNotOrigin.selectedTenants.forEach(tenant => {
        if (tenant.default) {
          isDefaultSelected = true
          select.selectedTenants.forEach(originTenant => {
            if (originTenant.tenantId === tenant.tenantId) {
              originTenant.default = true
            }
          })
        }
      })
      if (!isDefaultSelected && select.selectedTenants.length > 0) {
        select.selectedTenants[0].default = true
        selectNotOrigin.selectedTenants.forEach(tenant => {
          if (tenant.tenantId === select.selectedTenants[0].tenantId) {
            tenant.default = true
          }
        })
      }
      this.$emit('input', select)
      this.$emit('default', selectNotOrigin)
    },

    handleRoleChange(row) {
      // selectedTenantsの変更をemit
      let select = Object.assign({}, this.value)
      let index = select.selectedTenants.findIndex(tenant => {
        return tenant.tenantId === row.tenantId
      })
      this.$set(select.selectedTenants, index, row)
      this.$emit('input', select)
    },

    handleDefaultChange(row) {
      // defaultの変更をemit
      let select = Object.assign({}, this.value)
      select.selectedTenants.forEach(tenant => {
        tenant.default = false
        if (tenant.tenantId === row.tenantId) {
          tenant.default = true
        }
      })
      this.$emit('input', select)

      // defaultの変更をemit(Ldap経由のテナント)
      let selectNotOrigin = Object.assign({}, this.notOriginTenants)
      selectNotOrigin.selectedTenants.forEach(tenant => {
        tenant.default = false
        if (tenant.tenantId === row.tenantId) {
          tenant.default = true
        }
      })
      this.$emit('default', selectNotOrigin)
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}

.el-select ::v-deep .el-select__tags-text {
  max-width: 40vw;
  overflow: hidden;
  text-overflow: ellipsis;
  display: inline-block;
  vertical-align: middle;
}

.checkbox-role.is-checked::v-deep .el-checkbox-button__inner {
  background-color: #67c23a;
  border-color: #67c23a;
  box-shadow: -1px 0 0 0 #ffffff;
}

:disabled-checkbox {
  color: #fff;
  background-color: #409eff;
  border-color: #409eff;
}
</style>
