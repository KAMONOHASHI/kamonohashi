<template>
  <span>
    <el-input
      ref="saveTagInput"
      v-model="value"
      placeholder="Please input"
      size="mini"
      style="width:auto;"
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
  name: 'SmartSearchInputText',
  props: ['tag'],
  data() {
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
    /* eslint-disable */
    this.$nextTick(_ => {
      this.value = this.getValue();
      this.$refs.saveTagInput.$refs.input.focus();
      this.$nextTick(_ => {
        this.$refs.saveTagInput.$refs.input.select();
      });
    });
    /* eslint-enable */
  },
  methods: {
    handleCommand(row) {
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

    handleBlur() {
      setTimeout(() => {
        if (this.value === '') {
          this.emitCancel()
        }
      }, 300)
    },

    getValue() {
      if (this.tag.display) {
        return this.tag.display
      }
      if (this.tag.option && this.tag.option.default) {
        return this.tag.option.default
      }
      return ''
    },

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
