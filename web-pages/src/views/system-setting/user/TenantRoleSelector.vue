<template>
  <span>
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
        <el-table-column label="ロール" :width="getRoleDisplayWidth()">
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
          // カスタムロールの判別
          tmpRoles = tmpRoles.filter(
            role => role.tenantId == null || role.tenantId == tenant.id,
          )
          tmpRoles.forEach(role => {
            if (
              role.tenantId != null &&
              role.displayName.indexOf('(カスタム)') == -1
            ) {
              role.displayName = role.displayName + '(カスタム)'
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
      let isDefaultSelected = false
      select.selectedTenants.forEach(tenant => {
        if (tenant.default) {
          isDefaultSelected = true
        }
      })
      if (!isDefaultSelected && select.selectedTenants.length > 0) {
        select.selectedTenants[0].default = true
      }
      this.$emit('input', select)
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
    },

    // ロールの文字数に応じて表示幅を取得する
    getRoleDisplayWidth() {
      let wordCount = 0
      for (let i = 0; i < this.roles.length; i++) {
        wordCount = wordCount + this.roles[i].displayName.length
      }
      return wordCount * 10
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}
</style>
