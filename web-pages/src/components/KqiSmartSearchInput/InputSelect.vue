<template>
  <span>
    <el-input
      ref="saveTagInput"
      v-model="value"
      placeholder="Please input"
      size="mini"
      style="width: auto;"
      @blur="handleBlur"
    />

    <el-popover v-model="show" trigger="manual">
      <el-table
        :data="selectData"
        :show-header="false"
        @current-change="handleCommand"
      >
        <el-table-column prop="label" width="150" />
        <el-table-column prop="name" width="100" align="right">
          <template slot-scope="scope">
            <span style="color: grey; font-size: 85%;">
              {{ scope.row.detail }}
            </span>
          </template>
        </el-table-column>
      </el-table>
    </el-popover>
  </span>
</template>

<script>
export default {
  props: {
    tag: {
      type: Object,
      default: () => {},
    },
  },
  data() {
    return {
      value: '',
      show: true,
      selectData: [
        { label: 'テスト1', detail: '詳細１', value: '1' },
        { label: 'テスト2', detail: '詳細２', value: '2' },
        { label: 'テスト3', detail: '詳細３', value: '3' },
        { label: 'テスト4', detail: '詳細４', value: '4' },
      ],
    }
  },
  created() {
    this.$nextTick(() => {
      this.value = this.getValue()
      this.selectData = this.getSelectData()
      this.$refs.saveTagInput.$refs.input.focus()
      this.$nextTick(() => {
        this.$refs.saveTagInput.$refs.input.select()
      })
    })
  },
  methods: {
    // 検索条件を指定し、検索
    handleCommand(row) {
      this.show = false
      this.value = row.label
      let value = row.value
      let display = row.label
      let suffix = ''
      this.emitDone(value, display, suffix)
    },

    // フォーカスアウト時に値が何も入力されていない場合検索タグを消す
    handleBlur() {
      setTimeout(() => {
        if (this.value === '') {
          this.emitCancel()
        }
      }, 300)
    },

    // 表示する値の取得
    getValue() {
      if (this.tag.display) {
        return this.tag.display
      }
      if (this.tag.option && this.tag.option.default) {
        return this.tag.option.default
      }
      return ''
    },

    // 表示する検索条件の設定
    getSelectData() {
      // なければデフォルト値を設定
      if (this.tag.config.option.items) {
        let items = this.tag.config.option.items
        let ret = []
        items.forEach(i => {
          if (typeof i === 'string') {
            ret.push({ label: i, value: i })
          } else {
            ret.push(i)
          }
        })
        return ret
      }
      return this.selectData
    },

    // 'done'をemitし、検索
    emitDone(value, display, suffix) {
      this.$emit('done', { value, display, suffix })
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
}
</script>

<style scoped></style>
