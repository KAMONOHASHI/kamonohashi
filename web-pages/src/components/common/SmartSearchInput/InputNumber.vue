<template>
  <span>
    <el-input-number
      ref="saveTagInput"
      placeholder="Please input"
      controls-position="right"
      v-model="value"
      size="mini"
      style="width: 150px"
      @blur="handleBlur"/>

    <el-popover trigger="manual" v-model="show">
      <el-table :data="tableData" :show-header="false" @current-change="handleCommand">
        <el-table-column prop="name" width="100"/>
        <el-table-column prop="name" width="100" align="right">
          <template slot-scope="scope">
            <span style="color:grey;font-size:85%;">{{ scope.row.detail }}</span>
          </template>
        </el-table-column>
      </el-table>
    </el-popover>
  </span>
</template>

<script>
  export default {
    name: 'SmartSearchInputNumber',
    props: ['tag'],
    data () {
      return {
        value: undefined,
        show: true,
        tableData: [
          {name: 'と一致', detail: '= equals', symbol: '='},
          {name: '以上', detail: '', symbol: '>='},
          {name: '以下', detail: '', symbol: '<='},
          {name: 'より上', detail: '超過', symbol: '>'},
          {name: 'より下', detail: '未満', symbol: '<'}
        ]
      }
    },
    created () {
      this.$nextTick(_ => {
        this.value = this.getValue()
        this.$refs.saveTagInput.$refs.input.focus()
        this.$nextTick(_ => {
          this.$refs.saveTagInput.$refs.input.select()
        })
      })
    },
    methods: {
      handleCommand (row) {
        if (this.value !== undefined) {
          this.show = false
          let value = row.symbol + this.value
          let display = this.value
          let suffix = row.name
          this.emitDone(value, display, suffix)
        } else {
          this.emitCancel()
        }
      },

      handleBlur () {
        setTimeout(() => {
          if (this.value === undefined) {
            this.emitCancel()
          }
        }, 300)
      },

      getValue () {
        if (this.tag.display) {
          return this.tag.display
        }
        if (this.tag.option && this.tag.option.default) {
          return this.tag.option.default
        }
        return undefined
      },

      emitDone (value, display, suffix) {
        this.$emit('done', {value, display, suffix})
      },
      emitCancel () {
        this.$emit('cancel')
      }
    }
  }
</script>

<style scoped>

</style>
