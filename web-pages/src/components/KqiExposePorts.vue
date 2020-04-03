<template>
  <el-form-item label="開放ポート番号(TCP)">
    <el-row></el-row>
    <el-tag
      v-for="port in value"
      :key="port"
      closable
      :disable-transactions="false"
      @close="handleClose(port)"
    >
      {{ port }}
    </el-tag>
    <el-input
      v-if="inputVisible"
      ref="savePortInput"
      v-model="inputValue"
      class="input-new-port"
      size="mini"
      @keyup.enter.native="handleInputConfirm"
      @blur="handleInputConfirm"
    >
    </el-input>
    <el-button v-else class="button-new-port" size="small" @click="showInput"
      >+ Port</el-button
    >
  </el-form-item>
</template>

<script>
export default {
  props: {
    // 開放するポート番号(number)の配列
    value: {
      type: Array,
      default: () => {
        return []
      },
    },
  },
  data() {
    return {
      inputVisible: false,
      inputValue: '',
    }
  },
  methods: {
    handleClose(port) {
      let ports = this.value
      ports.splice(ports.indexOf(port), 1)
      this.$emit('input', ports)
    },

    showInput() {
      this.inputVisible = true
      // eslint-disable-next-line no-unused-vars
      this.$nextTick(_ => {
        this.$refs.savePortInput.$refs.input.focus()
      })
    },

    handleInputConfirm() {
      let inputValue = this.inputValue
      if (inputValue && !isNaN(inputValue)) {
        let ports = this.value
        let port = Number(inputValue)
        if (this.portValidator(port) && ports.indexOf(port) === -1) {
          ports.push(port)
          this.$emit('input', ports)
        }
      }
      this.inputVisible = false
      this.inputValue = ''
    },

    portValidator(port) {
      return 0 <= port && port <= 65535
    },
  },
}
</script>

<style lang="scss" scoped>
.el-tag + .el-tag {
  margin-left: 10px;
}
.button-new-port {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}
.input-new-port {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>
