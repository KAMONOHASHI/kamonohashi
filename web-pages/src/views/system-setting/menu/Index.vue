<template>
  <div>
    <h2>メニューアクセス管理</h2>
    <kqi-display-error :error="error" />
    <el-row>
      <el-table
        height="calc(100vh - 250px)"
        class="data-table pl-index-table"
        :data="tableData"
        border
      >
        <el-table-column prop="id" label="ID" width="80px" />
        <el-table-column prop="name" label="メニュー名" width="200px" />
        <el-table-column
          prop="description"
          label="説明"
          min-width="400px"
          width="auto"
        />
        <el-table-column prop="menuType" label="種別" width="120px">
          <template slot-scope="prop">
            {{ displayTypeName(prop.row.menuType) }}
          </template>
        </el-table-column>
        <el-table-column
          prop="roles"
          label="アクセス許可ロール"
          :width="getRoleDisplayWidth()"
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
                  :disabled="r.isSystemRole"
                >
                  {{ r.displayName }}
                </el-checkbox-button>
              </el-checkbox-group>
            </div>
            <div v-else-if="prop.row.menuType === 3">
              <el-checkbox-group
                v-model="prop.row.roles"
                style="white-space: nowrap;"
              >
                <el-checkbox-button
                  v-for="r in roleTypes"
                  :key="r.id"
                  :label="r.id"
                  :disabled="!r.isSystemRole"
                >
                  {{ r.displayName }}
                </el-checkbox-button>
              </el-checkbox-group>
            </div>
            <div v-else>
              <span style="color: red;">
                Not found : MenuType={{ prop.row.menuType }}
              </span>
            </div>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <el-row :gutter="20">
      <el-col class="right-buttom-button">
        <el-button
          icon="el-icon-edit-outline"
          type="primary"
          @click="handleUpdate"
        >
          更新
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'

import KqiDisplayError from '@/components/KqiDisplayError.vue'
import { mapGetters, mapActions } from 'vuex'

import * as gen from '@/api/api.generate'
interface DataType {
  error: null | Error
  tableData: Array<
    gen.NssolPlatypusApiModelsMenuApiModelsMenuForAdminOutputModel
  >
  roleTypes: Array<gen.NssolPlatypusApiModelsRoleApiModelsIndexOutputModel>
}

export default Vue.extend({
  components: {
    KqiDisplayError,
  },
  data(): DataType {
    return {
      error: null,
      tableData: [],
      roleTypes: [],
    }
  },
  computed: {
    ...mapGetters({
      //@ts-ignore
      menus: ['menu/menus'],
      menuTypes: ['menu/types'],
      roles: ['role/roles'],
    }),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'menu/fetchMenus',
      'menu/fetchTypes',
      'menu/put',
      'role/fetchRoles',
    ]),
    async retrieveData() {
      // ロールの取得
      await this['role/fetchRoles']()
      this.roleTypes = []
      this.roles.forEach(
        (role: gen.NssolPlatypusApiModelsRoleApiModelsIndexOutputModel) => {
          if (!role.tenantId) {
            // テナント固有のロールは除外
            this.roleTypes.push(role)
          }
        },
      )

      // メニュー種別の取得
      await this['menu/fetchTypes']()

      // メニューとロールのマッピング情報の取得
      await this['menu/fetchMenus']()
      this.tableData = this.menus
    },

    async handleUpdate() {
      try {
        for (const data of this.tableData) {
          let params = {
            id: data.id,
            body: data.roles,
          }
          await this['menu/put'](params)
        }
        this.showSuccessMessage()
        this.error = null
      } catch (e) {
        if (e instanceof Error) this.error = e
      }
    },

    displayTypeName(id: number) {
      let type = this.menuTypes.find(
        (s: gen.NssolPlatypusInfrastructureInfosEnumInfo) => s.id === id,
      )
      if (type) {
        return type.name
      } else {
        return ''
      }
    },

    // ロールの文字数に応じて表示幅を取得する
    getRoleDisplayWidth() {
      let wordCount = 0
      for (let i = 0; i < this.roleTypes.length; i++) {
        wordCount = wordCount + this.roleTypes[i].displayName!.length
      }
      return wordCount * 15
    },
  },
})
</script>

<style lang="scss" scoped>
.right-buttom-button {
  text-align: right;
  padding-top: 10px;
}
</style>
