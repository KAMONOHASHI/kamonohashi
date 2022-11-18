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
        :data="tableData"
        :show-header="false"
        @current-change="handleCommand"
      >
        <el-table-column prop="name" width="100" />
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

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'
interface DataType {
  value: string
  show: boolean
  tableData: Array<{ name: string; detail: string; symbol: string }>
}
export default Vue.extend({
  props: {
    tag: {
      type: Object as PropType<any>,
      default: () => {},
    },
  },
  data(): DataType {
    return {
      value: '',
      show: true,
      tableData: [
        { name: 'を含む', detail: 'contains', symbol: '' },
        { name: 'を含まない', detail: 'not contains', symbol: '!' },
      ],
    }
  },
  created() {
    this.$nextTick(() => {
      this.value = this.getValue()
      this.$refs.saveTagInput.$refs.input.focus()
      this.$nextTick(() => {
        this.$refs.saveTagInput.$refs.input.select()
      })
    })
  },
  methods: {
    // 検索条件を指定し、検索
    handleCommand(row: { name: string; detail: string; symbol: string }) {
      if (this.value !== '') {
        this.show = false
        let value = row.symbol + this.value
        let display = this.value
        let suffix = row.name
        this.emitDone(value, display, suffix)
      } else {
        this.emitCancel()
      }
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

    // 'done'をemitし、検索
    emitDone(value, display, suffix) {
      this.$emit('done', { value, display, suffix })
    },
    emitCancel() {
      this.$emit('cancel')
    },
  },
})
</script>

<style scoped></style>
