<!--name: 編集不可項目の表示用コンポーネント,-->
<!--description: 単にラベルと値を表示するだけなのだけれど、入力可能項目と同じようなレイアウトで配置するコンポーネントがなかったので、自作した。-->
<!--props: {-->
<!--label: ラベル-->
<!--value: 値-->
<!--}-->
<!--slots: {  action: 値に加えて、何か表示要素を追加したいときに使う,}-->
<template>
  <div class="pl-form">
    <label class="el-form-item__label">{{label}}</label>
    <div class="pl-form-item-value">
      <span v-if="isShow">
        {{value}}
      </span>
      <span v-else>
        --
      </span>
      <slot name="action"></slot>
    </div>
  </div>
</template>

<script>
  export default {
    props: ['label', 'value'],

    computed: {
      isShow () {
        if (this.value === null) return false
        if (this.value === undefined) return false
        return true
      }
    }
  }
</script>

<style lang="scss" scoped>
  .pl-form-item-value {
    display: inline-block;
    width: 100%;
    height: 40px;
    padding-left: 3px;
    word-wrap: break-word;
  }

  .pl-form {
    margin-bottom: 12px;
  }

</style>
