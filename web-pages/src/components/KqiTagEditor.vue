<!--name: タグ編集コンポーネント,-->
<!--description: タグの改廃をするためのエディタ。まだタグの補完はできない。-->
<!--props: {    value: 初期状態のタグ配列。監視対象で、変更されたら反映する。}-->
<!--events: {    input(tags: Array<string>): タグ入力が一つ終わるごとに発火, }-->
<template>
  <div class="el-input">
    <el-tag
      v-for="tag in value"
      :key="tag"
      closable
      :disable-transitions="false"
      @close="removeTag(tag)"
    >
      <span class="tag-ellipsis">{{ tag }}</span>
    </el-tag>
    <el-select
      v-if="selectBoxVisible"
      ref="saveTagInput"
      v-model="tagValue"
      filterable
      default-first-option
      allow-create
      class="input-new-tag"
      size="mini"
      @change="handleInputConfirm"
    >
      <el-option v-for="t in registeredTags" :key="t" :label="t" :value="t" />
    </el-select>
    <el-button v-else class="button-new-tag" size="small" @click="showTagInput">
      + 新規タグ
    </el-button>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'
export default Vue.extend({
  props: {
    // 選択中のタグ
    value: {
      type: Array as PropType<Array<string>>,
      default: () => {
        return []
      },
    },
    // テナントの登録済みのタグ
    registeredTags: {
      type: Array as PropType<Array<string>>,
      default: () => {
        return []
      },
    },
  },
  data() {
    return {
      selectBoxVisible: false, // 新規タグの入力エリアの表示有無
      tagValue: '', // 新規タグの入力値
    }
  },
  methods: {
    //タグ入力ボックスを表示する
    showTagInput() {
      this.selectBoxVisible = true
      this.$nextTick(() =>
        // 新しいタグ入力テキストボックスを出したら、すぐに入力開始できるよう、フォーカスする
        this.$refs.saveTagInput.focus(),
      )
    },
    //タグを追加しemit
    handleInputConfirm() {
      if (this.tagValue) {
        let tags = this.value
        tags.push(this.tagValue)
        this.$emit('input', tags)
      }
      this.selectBoxVisible = false
      this.tagValue = ''
    },
    // タグを削除しemit
    async removeTag(tag: string) {
      let tags = this.value
      tags.splice(tags.indexOf(tag), 1)
      this.$emit('input', tags)
    },
  },
})
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
