<!--name: 学習履歴セレクタ,-->
<!--description: 学習履歴を複数選択するドロップダウンメニュー。,-->
<!--props: { value: デフォルトで選択されている学習履歴情報},-->
<!--events: {  input(parent): 選択された学習履歴情報。選択解除時には null が送られる}-->
<template>
  <div>
    <div class="el-input">
      <el-select
        v-popover:detail
        @change="onChange"
        filterable
        multiple
        :value="value"
        remote
        :clearable="true">
        <el-option
          v-for="item in trainingHistories"
          :key="item.id"
          :label="item.fullName"
          :value="item.id">
        </el-option>
      </el-select>
    </div>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    props: {
      value: Array
    },
    data () {
      return {
        trainingHistories: [],
        parentIds: []
      }
    },
    async created () {
      await this.getTrainingHistories()
    },
    methods: {
      async getTrainingHistories () {
        this.trainingHistories = (await api.training.getSimple()).data
      },
      async onChange (parentIds) {
        this.$emit('input', parentIds)
      }
    }
  }
</script>

<style lang="scss" scoped>
  .el-select {
    width: 100% !important;
  }
</style>
