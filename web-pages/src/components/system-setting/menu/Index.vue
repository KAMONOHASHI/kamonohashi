<template>
  <div>

    <h2>メニューアクセス管理</h2>
    <pl-display-error :error="error"/>
    <el-row>
      <el-table height="1000" class="data-table pl-index-table" :data="tableData" border>
        <el-table-column prop="id" label="ID" width="80px"/>
        <el-table-column prop="name" label="メニュー名" width="200px"/>
        <el-table-column prop="description" label="説明" width="auto"/>
        <el-table-column prop="menuType" label="種別" width="120px">
          <template slot-scope="prop">
            {{ displayTypeName(prop.row.menuType) }}
          </template>
        </el-table-column>
        <el-table-column prop="roles" label="アクセス許可ロール" :width="roleTypes.length * 100">
          <template slot-scope="prop">
            <div v-if="prop.row.menuType === 4">
              公開
            </div>
            <div v-else-if="prop.row.menuType === 1">
              ログインユーザに許可
            </div>
            <div v-else-if="prop.row.menuType === 2">
              <el-checkbox-group v-model="prop.row.roles" style="white-space: nowrap;">
                <el-checkbox-button v-for="r in roleTypes" :label="r.id" :key="r.id" :disabled="r.isSystemRole">
                  {{r.displayName}}
                </el-checkbox-button>
              </el-checkbox-group>
            </div>
            <div v-else-if="prop.row.menuType === 3">
              <el-checkbox-group v-model="prop.row.roles" style="white-space: nowrap;">
                <el-checkbox-button v-for="r in roleTypes" :label="r.id" :key="r.id" :disabled="!r.isSystemRole">
                  {{r.displayName}}
                </el-checkbox-button>
              </el-checkbox-group>
            </div>
            <div v-else>
              <span style="color:red">Not found : MenuType={{prop.row.menuType}}</span>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <el-row :gutter="20">
      <el-col class="right-buttom-button">
        <el-button @click="handleUpdate" icon="el-icon-edit-outline" type="primary">
          更新
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
  import api from '@/api/v1/api'
  import DisplayError from '@/components/common/DisplayError'

  export default {
    name: 'MenuIndex',
    title: 'メニューアクセス管理',
    components: {
      'pl-display-error': DisplayError
    },
    data () {
      return {
        error: null,
        tableData: [],
        menuTypes: [],
        roleTypes: []
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async retrieveData () {
        let allRoles = (await api.role.admin.get()).data
        this.roleTypes = []
        allRoles.forEach(role => {
          if (!role.tenantId) { // テナント固有のロールは除外
            this.roleTypes.push(role)
          }
        })
        this.menuTypes = (await api.menu.admin.getTypes()).data

        // roles の形式を変換して設定
        let tableData = (await api.menu.admin.get()).data
        tableData.forEach(menu => {
          let roles = []
          menu.roles.forEach(role => {
            roles.push(role.id)
          })
          menu.roles = roles
        })
        this.tableData = tableData
      },

      async handleUpdate () {
        try {
          await this.putMenus()
          this.showSuccessMessage()
          this.error = null
        } catch (e) {
          this.error = e
        }
      },

      async putMenus () {
        for (let i = 0; i < this.tableData.length; i++) {
          let params = {
            id: this.tableData[i].id,
            roleIds: this.tableData[i].roles
          }
          await api.menu.admin.put(params)
        }
      },

      displayTypeName (id) {
        let type = this.menuTypes.find(s => s.id === id)
        if (type) {
          return type.name
        } else {
          return ''
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  .sticky {
    position: sticky;
    top: 0;
  }

  .right-buttom-button {
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
</style>
