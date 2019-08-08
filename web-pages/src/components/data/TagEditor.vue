<!--name: タグ編集コンポーネント,-->
<!--description: タグの改廃をするためのエディタ。まだタグの補完はできない。-->
<!--props: {    value: 初期状態のタグ配列。監視対象で、変更されたら反映する。}-->
<!--events: {    input(tags: Array<string>): タグ入力が一つ終わるごとに発火, }-->
<template>
  <div class="el-input">
    <el-tag
      :key="tag"
      v-for="tag in tags"
      closable
      :disable-transitions="false"
      @close="removeTag(tag)">
      <span class="tag-ellipsis">{{tag}}</span>
    </el-tag>
    <el-select
      filterable
      default-first-option
      allow-create
      class="input-new-tag"
      v-if="inputTagVisible"
      v-model="inputTagValue"
      ref="saveTagInput"
      size="mini"
      @change="handleInputConfirm">
      <el-option
        v-for="t in registeredTags"
        :key="t"
        :label="t"
        :value="t"/>
    </el-select>
    <el-button v-else class="button-new-tag" size="small" @click="showTagInput">+ 新規タグ</el-button>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'TagEditor',
    data () {
      return {
        inputTagVisible: false, // 新規タグの入力エリアの表示有無
        inputTagValue: '', // 新規タグの入力値
        tags: [],
        registeredTags: []
      }
    },
    props: {
      value: Array
    },
    async mounted () {
      this.registeredTags = (await api.data.getDataTags()).data
    },
    watch: {
      value: function setCurrentTags () {
        this.tags = this.value
        this.inputTagVisible = false
        this.inputTagValue = ''
      }
    },
    methods: {
      async removeTag (tag) {
        this.tags.splice(this.tags.indexOf(tag), 1)
      },
      showTagInput () {
        this.inputTagVisible = true
        this.$nextTick(() =>
          // 新しいタグ入力テキストボックスを出したら、すぐに入力開始できるよう、フォーカスする
          (this.$refs.saveTagInput).focus()
        )
      },
      handleInputConfirm (newTag) {
        let inputValue = this.inputTagValue
        if (inputValue) {
          this.tags.push(inputValue)
          this.$emit('input', this.tags)
        }
        this.inputTagVisible = false
        this.inputTagValue = ''
      }
    }
  }
</script>

<style lang="scss" scoped>
  .el-tag + .el-tag {
    margin-left: 10px;
  }

  .button-new-tag {
    margin-left: 10px;
    height: 32px;
    line-height: 30px;
    padding-top: 0;
    padding-bottom: 0;
  }

  .input-new-tag {
    width: 90px;
    margin-left: 10px;
    vertical-align: bottom;
  }

  .tag-ellipsis {
    max-width: 98%;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    display: block;
    padding-right: initial;
  }

  .el-tag {
    max-width: 100%;
    display: -webkit-inline-box;
  }
</style>
