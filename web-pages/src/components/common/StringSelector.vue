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
  <el-select
    v-model="partition"
    class="el-input"
    filterable
    :clearable="true"
    @change="$emit('input', partition)"
  >
    <el-option v-for="item in items" :key="item" :label="item" :value="item">
    </el-option>
  </el-select>
</template>

<script>
export default {
  name: 'StringSelector',
  props: {
    valueList: Array,
    value: String,
  },
  data() {
    return {
      items: [],
      partition:
        this.value === undefined || this.value === null ? '' : this.value,
    }
  },
  watch: {
    value: function getData() {
      this.partition = this.value
    },
  },
  created() {
    this.items = this.valueList
  },
}
</script>

<style lang="scss" scoped></style>
