<template>
  <div>
    <h2>テナントメニュー管理</h2>
    <pl-display-error :error="error" />
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          @click="handleUpdate"
        >
          更新
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" border>
        <el-table-column prop="id" label="ID" width="80px" />
        <el-table-column prop="name" label="メニュー名" width="200px" />
        <el-table-column prop="description" label="説明" width="auto" />
        <el-table-column prop="menuType" label="種別" width="120px">
          <template slot-scope="prop">
            {{ displayTypeName(prop.row.menuType) }}
          </template>
        </el-table-column>
        <el-table-column
          prop="roles"
          label="アクセス許可ロール"
          :width="roleTypes.length * 100"
        >
          <template slot-scope="prop">
            <div v-if="prop.row.menuType === 4">
              公開
            </div>
            <div v-else-if="prop.row.menuType === 1">
              ログインユーザに許可
            </div>
            <div v-else-if="prop.row.menuType === 2">
              <el-checkbox-group
                v-model="prop.row.roles"
                style="white-space: nowrap;"
              >
                <el-checkbox-button
                  v-for="r in roleTypes"
                  :key="r.id"
                  :label="r.id"
                  :disabled="!r.tenantId"
                >
                  {{ r.displayName }}
                </el-checkbox-button>
              </el-checkbox-group>
            </div>
            <div v-else>
              <span style="color:red"
                >Not found : MenuType={{ prop.row.menuType }}</span
              >
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
  </div>
</template>

<script>
import api from '@/api/v1/api'
import DisplayError from '@/components/common/DisplayError'

export default {
  name: 'ManageMenuIndex',
  title: 'テナントメニュー管理',
  components: {
    'pl-display-error': DisplayError,
  },
  data() {
    return {
      error: null,
      tableData: [],
      menuTypes: [],
      roleTypes: [],
    }
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    async retrieveData() {
      this.roleTypes = (await api.role.tenant.get()).data
      this.menuTypes = (await api.menu.tenant.getTypes()).data

      // roles の形式を変換して設定
      let tableData = (await api.menu.tenant.get()).data
      tableData.forEach(menu => {
        let roles = []
        menu.roles.forEach(role => {
          roles.push(role.id)
        })
        menu.roles = roles
      })
      this.tableData = tableData
    },

    async handleUpdate() {
      try {
        await this.putMenus()
        this.showSuccessMessage()
        this.error = null
      } catch (e) {
        this.error = e
      }
    },

    async putMenus() {
      // 更新対象となるIDを抽出
      let targetRoleIds = []
      this.roleTypes.forEach(r => {
        if (r.tenantId) {
          targetRoleIds.push(r.id)
        }
      })

      for (let i = 0; i < this.tableData.length; i++) {
        let params = {
          id: this.tableData[i].id,
          roleIds: [],
        }
        this.tableData[i].roles.forEach(roleId => {
          if (targetRoleIds.includes(roleId)) {
            params.roleIds.push(roleId)
          }
        })
        await api.menu.tenant.put(params)
      }
    },

    displayTypeName(id) {
      let type = this.menuTypes.find(s => s.id === id)
      if (type) {
        return type.name
      } else {
        return ''
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
  padding-top: 10px;
}

.search {
  text-align: right;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}

.el-checkbox-button.is-checked.is-disabled /deep/ .el-checkbox-button__inner {
  color: #409eff;
  background: #ecf5ff;
}
</style>
