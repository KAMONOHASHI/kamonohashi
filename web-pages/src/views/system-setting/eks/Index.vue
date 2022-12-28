<template>
  <div>
    <h2>EKS管理</h2>

    <el-form
      ref="createForm"
      v-loading="loading"
      :model="form"
      :rules="rules"
      element-loading-spinner=" "
      element-loading-background="rgba(255, 255, 255, 0.7)"
    >
    </el-form>
  </div>
</template>

<script></script>

<style lang="scss" scoped></style>
