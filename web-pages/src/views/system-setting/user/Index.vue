<template>
  <div>
    <h2>ユーザ管理</h2>
    <el-row :gutter="20">
      <el-col class="right-top-button">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          plain
          @click="openCreateDialog"
        >
          新規作成
        </el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-table
        class="data-table pl-index-table"
        :data="users"
        @row-click="openEditDialog"
        @expand-change="handleToggleExpand"
      >
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-table
              :data="props.row.tenants"
              :show-header="false"
              @row-click="openEditDialog(props.row)"
            >
              <el-table-column label="dummy" width="300px" />
              <el-table-column label="dummy" width="150px" />
              <el-table-column prop="displayName" label="テナント" width="auto">
                <template slot-scope="scope">
                  <span
                    class="tenant"
                    :class="{
                      'tenant-default': scope.row.default,
                      'not-origin': !scope.row.isOrigin,
                    }"
                  >
                    {{ scope.row.displayName }}
                  </span>
                </template>
              </el-table-column>
              <el-table-column prop="roles" label="ロール" width="auto">
                <template slot-scope="scope">
                  <span v-for="(role, index) in scope.row.roles" :key="index">
                    <el-tag v-if="role.isCustomed" type="info" class="role-tag">
                      {{ role.displayName }}
                    </el-tag>
                    <el-tag
                      v-else
                      class="role-tag"
                      :class="{
                        'not-origin': !role.isOrigin,
                      }"
                      :type="role.isOrigin ? '' : 'success'"
                    >
                      {{ role.displayName }}
                    </el-tag>
                  </span>
                </template>
              </el-table-column>
            </el-table>
          </template>
        </el-table-column>

        <el-table-column prop="name" label="ユーザ名" width="300px" />
        <el-table-column prop="serviceType" label="認証タイプ" width="150px">
          <template slot-scope="scope">
            <span v-if="scope.row.serviceType === 1">ローカル</span>
            <span v-else-if="scope.row.serviceType === 2">LDAP</span>
            <span v-else>{{ serviceType }}</span>
          </template>
        </el-table-column>
        <el-table-column label="テナント" width="auto">
          <template slot-scope="scope">
            <div v-if="showTenants[scope.row.id]">
              <span v-for="(t, index) in scope.row.tenants" :key="index">
                <span
                  class="tenant"
                  :class="{
                    'tenant-default': t.default,
                    'not-origin': !t.isOrigin,
                  }"
                >
                  {{ t.displayName }}
                </span>
              </span>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="systemRoles" label="ロール" width="auto">
          <template slot-scope="scope">
            <span v-for="role in scope.row.systemRoles" :key="role.id">
              <el-tag type="warning" class="role-tag">
                {{ role.displayName }}
              </el-tag>
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <router-view @cancel="closeDialog()" @done="done()" />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('user')

export default {
  title: 'ユーザ管理',
  data() {
    return { showTenants: {} }
  },
  computed: {
    ...mapGetters(['users']),
  },
  async created() {
    await this.initialize()
  },
  methods: {
    ...mapActions(['fetchUsers']),
    async initialize() {
      await this.fetchUsers()
      // add data
      this.showTenants = {}
      this.users.forEach(d => {
        this.$set(this.showTenants, d.id, true)
      })
    },
    async handleToggleExpand(row) {
      this.showTenants[row.id] = !this.showTenants[row.id]
    },
    openCreateDialog() {
      this.$router.push('/user/edit')
    },
    openEditDialog(row) {
      if (row) {
        this.$router.push('/user/edit/' + row.id)
      }
    },
    async done() {
      await this.initialize()
      this.closeDialog()
      this.showSuccessMessage()
    },
    closeDialog() {
      this.$router.push('/user')
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

.data-table /deep/ .el-table__expanded-cell {
  padding-top: 0px !important;
  padding-right: 0px !important;
}

.role-tag {
  margin-right: 8px;
}

.tenant {
  margin-right: 5px;
}

.tenant-default {
  font-weight: bold !important;
  color: #409eff;
}
.not-origin {
  color: #40ff79c4;
}
</style>
