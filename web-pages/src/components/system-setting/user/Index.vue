<template>
  <div>
    <h2>ユーザ管理</h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button @click="openCreateDialog" icon="el-icon-edit-outline" type="primary" plain>
          新規登録
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table class="data-table pl-index-table" :data="tableData" @row-click="openEditDialog"
                @expand-change="handleToggleExpand">
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-table :data="props.row.tenants" :show-header="false"
                      @row-click="openEditDialog(props.row)">
              <el-table-column label="dummy" width="300px"/>
              <el-table-column label="dummy" width="150px"/>
              <el-table-column prop="displayName" label="テナント" width="auto">
                <template slot-scope="scope">
                  <span class="tenant" v-bind:class="{'tenant-default':scope.row.default}">
                  {{scope.row.displayName}}
                  </span>
                </template>
              </el-table-column>
              <el-table-column prop="roles" label="ロール" width="auto">
                <template slot-scope="scope">
                  <span v-for="(role, index) in scope.row.roles" :key="index">
                    <el-tag v-if="role.isCustomed" type="info">{{ role.displayName }}</el-tag>
                    <el-tag v-else>{{ role.displayName }}</el-tag>
                    <span>&nbsp;</span>
                  </span>
                </template>
              </el-table-column>
            </el-table>
          </template>
        </el-table-column>
        <el-table-column prop="name" label="ユーザ名" width="300px"/>
        <el-table-column prop="serviceType" label="認証タイプ" width="150px">
          <template slot-scope="scope">
            <span v-if="scope.row.serviceType===1">ローカル</span>
            <span v-else-if="scope.row.serviceType===2">LDAP</span>
            <span v-else>{{ serviceType }}</span>
          </template>
        </el-table-column>
        <el-table-column label="テナント" width="auto">
          <template slot-scope="scope">
            <span v-for="(t, index) in scope.row.tenants" :key="index" v-if="scope.row.showTenants">
              <span class="tenant" v-bind:class="{'tenant-default':t.default}">
              {{t.displayName}}
              </span>
            </span>
          </template>
        </el-table-column>
        <el-table-column prop="systemRoles" label="ロール" width="auto">
          <template slot-scope="scope">
            <span v-for="(role) in scope.row.systemRoles" :key="role.id">
              <el-tag type="warning">{{ role.displayName }}</el-tag>&nbsp;
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog" @done="done"></router-view>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'UserIndex',
    title: 'ユーザ管理',
    data () {
      return {
        total: 0,
        tableData: [],
        currentPage: 1,
        currentPageSize: 10
      }
    },
    async created () {
      await this.retrieveData()
    },
    methods: {
      async currentChange (page) {
        this.currentPage = page
        await this.retrieveData()
      },

      async retrieveData () {
        let response = await api.user.admin.get()
        this.tableData = response.data
        this.total = parseInt(response.headers['x-total-count'])

        // add data
        this.tableData.forEach(d => {
          d.showTenants = true
        })
      },

      async handleToggleExpand (row, rows) {
        row.showTenants = !row.showTenants
      },
      selectedArrayByIds (selected) {
        let ret = []

        selected.forEach(s => {
          ret.push(s.id)
        })

        return ret
      },
      openCreateDialog () {
        this.$router.push('/user/create')
      },
      openEditDialog (row) {
        if (row) {
          this.$router.push('/user/' + row.id)
        }
      },
      done () {
        this.retrieveData()
        this.closeDialog()
        this.showSuccessMessage()
      },
      closeDialog () {
        this.$router.push('/user')
      }
    }
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

  .data-table /deep/ .el-table__expanded-cell {
    padding-top: 0px !important;
    padding-right: 0px !important;
  }

  .tenant {
    margin-right: 5px;
  }

  .tenant-default {
    font-weight: bold !important;
    color: #409EFF;
  }
</style>
