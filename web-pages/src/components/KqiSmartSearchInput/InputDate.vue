<template>
  <span>
    <el-date-picker
      ref="saveTagInput"
      v-model="value"
      type="date"
      placeholder="Please input"
      size="mini"
      format="yyyy-MM-dd"
      style="width: 150px"
      @change="handleChange"
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
            <span style="color:grey;font-size:85%;">{{
              scope.row.detail
            }}</span>
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
      value: null,
      show: false,
      tableData: [
        // e.g) 2018/10/1を選択した場合
        { name: 'のみ', detail: '', symbol: '=' }, // =2018/10/1
        { name: '以降', detail: '', symbol: '>=', days: 0 }, // >=2018/10/1
        { name: '以前', detail: '', symbol: '<', days: 1 }, // <2018/10/2
        { name: 'より後', detail: '', symbol: '>=', days: 1 }, // >=2018/10/2
        { name: 'より前', detail: '', symbol: '<', days: 0 }, // <2018/10/1
      ],
    }
  },
  created() {
    this.$nextTick(() => {
      this.value = this.getValue()
      setTimeout(() => {
        this.$refs.saveTagInput.focus()
      }, 600)
    })
  },
  methods: {
    // 検索する基準値入力後に検索範囲のドロップダウンを表示
    handleChange() {
      this.show = true
    },

    // 検索条件を指定し、検索
    handleCommand(row) {
      if (this.value) {
        this.show = false
        let value = row.symbol + this.getNowYMD(this.value, row.days)
        let display = this.getNowYMD(this.value)
        let suffix = row.name
        this.emitDone(value, display, suffix)
      } else {
        this.emitCancel()
      }
    },

    // フォーカスアウト時に値が何も入力されていない場合検索タグを消す
    handleBlur() {
      setTimeout(() => {
        if (!this.value) {
          this.emitCancel()
        }
      }, 300)
    },

    // 入力された値の整形
    getNowYMD(date, days) {
      let dt = new Date(date.valueOf())
      if (days) {
        dt.setDate(dt.getDate() + days)
      }

      var y = dt.getFullYear()
      if (y >= 10000) {
        // 10000年以上の場合、最大日付を返す
        return '9999/12/31'
      }
      var m = ('00' + (dt.getMonth() + 1)).slice(-2)
      var d = ('00' + dt.getDate()).slice(-2)
      var result = y + '/' + m + '/' + d
      return result
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
}
</script>

<style scoped></style>
