<!--name: 確認ダイヤログ付きの削除ボタン,-->
<!--description: ボタンを押すと確認が出て、そこでConfirmを押せば指定したURLにDELETEリクエストを投げる,-->
<!--props: {-->
<!--value: デフォルトで選択されている文字列-->
<!--valueList: 文字列リスト-->
<!--},-->
<!--events: {-->
<!--input(label:string): 選択された文字列-->
<!--}-->
<template>
  <el-select class="el-input" @change="$emit('input',partition)" filterable v-model="partition" :clearable="true">
    <el-option
      v-for="item in items"
      :key="item"
      :label="item"
      :value="item">
    </el-option>
  </el-select>
</template>

<script>
  export default {
    name: 'StringSelector',
    props: {
      valueList: Array,
      value: String
    },
    data () {
      return {
        items: [],
        partition: this.value === undefined || this.value === null
          ? ''
          : this.value
      }
    },
    created () {
      this.items = this.valueList
    },
    watch: {
      value: function getData () {
        this.partition = this.value
      }
    }

  }
</script>

<style lang="scss" scoped>
</style>
