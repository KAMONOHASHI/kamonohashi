<template>
  <div class="el-input">
    <el-row v-for="(d, index) in dict" :key="index" type="flex" justify="space-between">
      <el-col :span="10" :offset="1">
        <el-input size="small" placeholder="Key" @input="updateDic" v-model="d.key"/>
      </el-col>
      <el-col :span="10">
        <el-input size="small" placeholder="Value" @input="updateDic" v-model="d.value"/>
      </el-col>
      <el-col :span="2">
        <el-button size="small" type="danger" v-if="index>0" width="100%" @click="clickDel(index)">
          -
        </el-button>
      </el-col>
    </el-row>
    <el-row type="flex" justify="end">
      <el-col :span="2">
        <el-button size="small" type="primary" @click="clickAdd">
          +
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>

  export default {
    name: 'DynamicMultiInputField',
    props: {
      value: Array
    },
    data () {
      // このコンポーネント内ではDictionaryよりもArray<KVP>の方が取り回しが良いため、そちらを採用
      return {
        dict: this.value === undefined || this.value === null
          ? [{key: '', value: ''}]
          : this.value
      }
    },
    created () {
      this.updateDic()
    },
    watch: {
      value: function getData () {
        // もし元データが空の場合、そのまま入れると入力不能になるため、初期値を残す
        if (this.value && this.value.length > 0) {
          this.dict = this.value
        }
      }
    },
    methods: {
      clickAdd () {
        this.dict.push({key: '', value: ''})
        this.updateDic()
      },

      clickDel (index) {
        this.dict.splice(index, 1)
        this.updateDic()
      },

      updateDic () {
        // 変更を親に反映
        this.$emit('input', this.dict)
      }
    }
  }
</script>

<style lang="scss" scoped>
  .button-block {
    text-align: right;
  }

</style>
