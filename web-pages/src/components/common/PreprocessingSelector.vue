<template>
  <div>
    <el-popover
      ref="detail"
      :disabled="!(preprocessing && preprocessing.id > 0)"
      title="前処理詳細"
      trigger="hover"
      width="350"
      placement="right"
    >
      <pl-preprocessing-details :preprocessing="preprocessing" />
    </el-popover>
    <div class="el-input">
      <el-select
        v-model="preprocessing"
        v-popover:detail
        filterable
        value-key="id"
        remote
        :clearable="true"
        @change="onChange"
      >
        <el-option
          v-for="item in preprocessingList"
          :key="item.id"
          :label="item.name"
          :value="item"
        >
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{
            item.memo
          }}</span>
        </el-option>
      </el-select>
    </div>
  </div>
</template>

<script>
import PreprocessingDetails from '@/components/common/PreprocessingDetails.vue'
import api from '@/api/v1/api'

export default {
  name: 'PreprocessingSelector',
  components: {
    'pl-preprocessing-details': PreprocessingDetails,
  },
  props: {
    value: Object,
  },
  data() {
    return {
      preprocessing: {},
      preprocessingList: [],
    }
  },
  watch: {
    value: function getData() {
      this.preprocessing = this.value
    },
  },
  async created() {
    await this.getPreprocessings()
  },
  methods: {
    async getPreprocessings() {
      this.preprocessingList = (await api.preprocessings.get()).data
    },
    onChange(preprocessing) {
      if (typeof preprocessing === 'string') {
        // 削除すると空文字列が渡ってくるので、空の連想配列に変換
        this.preprocessing = {}
      } else {
        this.preprocessing = preprocessing
      }
      this.$emit('input', this.preprocessing)
    },
  },
}
</script>

<style lang="scss" scoped>
.el-select {
  width: 100% !important;
}
</style>
