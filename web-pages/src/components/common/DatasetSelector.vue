<template>
  <div>
    <el-popover
      :disabled="!(dataSet && dataSet.id>0)"
      ref="detail"
      title="データセット詳細"
      trigger="hover"
      width="350"
      placement="right">
      <pl-dataset-details :dataSet="dataSet"/>
    </el-popover>
    <div class="el-input">
      <el-select
        v-popover:detail
        @change="onChange"
        filterable
        value-key="id"
        v-model="dataSet"
        remote
        :clearable="true">
        <el-option
          v-for="item in dataSets"
          :key="item.id"
          :label="item.name"
          :value="item">
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.memo }}</span>
        </el-option>
      </el-select>
    </div>
  </div>
</template>

<script>
  import DataSetDetails from '@/components/common/DatasetDetails.vue'
  import api from '@/api/v1/api'

  export default {
    name: 'DataSetSelector',
    components: {
      'pl-dataset-details': DataSetDetails
    },
    props: {
      value: Object
    },
    data () {
      return {
        dataSets: [],
        dataSet: this.value === undefined || this.value === null
          ? {}
          : this.value
      }
    },
    async created () {
      await this.getDataSets()
    },
    methods: {
      async getDataSets () {
        this.dataSets = (await api.datasets.get()).data
      },
      onChange (dataSet) {
        this.dataSet = dataSet
        this.$emit('input', dataSet)
      }
    },
    watch: {
      value: function getData () {
        this.dataSet = this.value
      }
    }
  }
</script>

<style lang="scss" scoped>
  .el-select {
    width: 100% !important;
  }
</style>
