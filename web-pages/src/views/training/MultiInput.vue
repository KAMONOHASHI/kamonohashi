<!--name: タグ編集コンポーネント,-->
<!--description: タグの改廃をするためのエディタ。まだタグの補完はできない。-->
<!--props: {    value: 初期状態のタグ配列。監視対象で、変更されたら反映する。}-->
<!--events: {    input(tags: Array<string>): タグ入力が一つ終わるごとに発火, }-->
<template>
  <div class=" box box-small">
    <el-tag
      v-for="item in value"
      :key="item"
      closable
      :disable-transitions="false"
      @close="removeItem(item)"
    >
      <span class="tag-ellipsis">{{ item }}</span>
    </el-tag>
    <el-select
      v-if="selectBoxVisible"
      ref="saveItemInput"
      v-model="itemValue"
      filterable
      default-first-option
      allow-create
      class="input-new-tag"
      size="mini"
      @change="handleInputConfirm"
    >
      <el-option v-for="t in registeredItems" :key="t" :label="t" :value="t" />
    </el-select>
    <el-button
      v-else
      class="button-new-tag"
      size="small"
      @click="showItemInput"
    >
      +
    </el-button>
  </div>
</template>

<script>
export default {
  props: {
    // 選択中のタグ
    value: {
      type: Array,
      default: () => {
        return []
      },
    },
    // テナントの登録済みのタグ
    registeredItems: {
      type: Array,
      default: () => {
        return []
      },
    },
  },
  data() {
    return {
      selectBoxVisible: false, // 新規タグの入力エリアの表示有無
      itemValue: '', // 新規タグの入力値
    }
  },
  methods: {
    //タグ入力ボックスを表示する
    showItemInput() {
      this.selectBoxVisible = true
      this.$nextTick(() =>
        // 新しいタグ入力テキストボックスを出したら、すぐに入力開始できるよう、フォーカスする
        this.$refs.saveItemInput.focus(),
      )
    },
    //タグを追加しemit
    handleInputConfirm() {
      if (this.itemValue && this.itemValue.indexOf(',') == -1) {
        let items = this.value
        items.push(this.itemValue)
        this.$emit('input', items)
      }
      this.selectBoxVisible = false
      this.itemValue = ''
    },
    // タグを削除しemit
    async removeItem(item) {
      let items = this.value
      items.splice(items.indexOf(item), 1)
      this.$emit('input', items)
    },
  },
}
</script>

<style lang="scss" scoped>
.box-small {
  padding: 0px 5px 2px 5px;
}
.box {
  text-align: left;
  border: 1px solid #dddddd;
  border-radius: 5px;
  background-color: white;
}

.box-mini {
  padding: 5px 3px 7px 3px;
}

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
