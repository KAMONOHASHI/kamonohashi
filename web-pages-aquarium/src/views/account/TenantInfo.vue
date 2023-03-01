<template>
  <!-- 選択中テナント情報 -->
  <el-card class="container base">
    <div class="logo">
      <img src="@/assets/logo_A.png" alt="" />
    </div>
    <el-row class="row-element" style="text-align: center;">
      {{ userName }}
    </el-row>
    <el-row class="row-element">
      <el-col :span="12" class="content-color">選択中のテナント</el-col>
      <el-col v-if="tenant" :span="12">
        {{ tenant.displayName }} (ID: {{ tenant.id }})
      </el-col>
    </el-row>
    <el-row class="row-element">
      <el-col :span="12" class="content-color">ロール</el-col>
      <el-col v-if="tenant" :span="12">
        <div v-for="(r, index) in tenant.roles" :key="index">
          {{ r.displayName }}
        </div>
      </el-col>
    </el-row>
  </el-card>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'
import * as gen from '@/api/api.generate'
export default Vue.extend({
  props: {
    userName: {
      type: String,
      default: '',
    },
    tenant: {
      type: Object as PropType<{
        id: null | number
        displayName: string
        roles: Array<gen.NssolPlatypusInfrastructureInfosRoleInfo>
      }>,
      default: (): {
        id: null | number
        displayName: string
        roles: Array<gen.NssolPlatypusInfrastructureInfosRoleInfo>
      } => {
        return {
          id: null,
          displayName: '',
          roles: [],
        }
      },
    },
  },
})
</script>

<style scoped>
.row-element {
  font-size: 14px;
  line-height: 40px;
  margin-top: 30px;
  font-weight: bold !important;
}

.logo {
  text-align: center;
}

.container {
  margin-top: 10px;
}

.base {
  grid-row: 1 / 3;
  grid-column: 1 / 2;
  margin-right: 20px;
}

.content-color {
  color: #606266;
}
</style>
