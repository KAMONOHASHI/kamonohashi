<template>
  <span>
    <el-input
      ref="saveTagInput"
      placeholder="Please input"
      v-model="value"
      size="mini"
      style="width:auto;"
      @blur="handleBlur" />

    <el-popover trigger="manual" v-model="show">
      <el-table :data="selectData" :show-header="false" @current-change="handleCommand">
        <el-table-column prop="label" width="150" />
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
  name: 'SmartSearchInputSelect',
  props: ['tag'],
  data () {
    return {
      value: '',
      show: true,
      selectData: [
        { label: 'テスト1', detail: '詳細１', value: '1' },
        { label: 'テスト2', detail: '詳細２', value: '2' },
        { label: 'テスト3', detail: '詳細３', value: '3' },
        { label: 'テスト4', detail: '詳細４', value: '4' }
      ]
    }
  },
  created () {
    this.$nextTick(_ => {
      // console.log('tag', this.tag)
      this.value = this.getValue()
      this.selectData = this.getSelectData()
      this.$refs.saveTagInput.$refs.input.focus()
      this.$nextTick(_ => {
        this.$refs.saveTagInput.$refs.input.select()
      })
    })
  },
  methods: {
    handleCommand (row) {
      this.show = false
      this.value = row.label
      let value = row.value
      let display = row.label
      let suffix = ''
      this.emitDone(value, display, suffix)
    },

    handleBlur () {
      setTimeout(() => {
        if (this.value === '') {
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
      return ''
    },

    getSelectData () {
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

    emitDone (value, display, suffix) {
      this.$emit('done', { value, display, suffix })
    },
    emitCancel () {
      this.$emit('cancel')
    }
  }
}
</script>

<style scoped>

</style>
