<template>
  <span v-if="Object.keys(dataSet).length !== 0">
    <el-row style="margin-bottom:5px; padding-left:15px; font-size:0.8em;">
      参考：選択したデータセット【{{ dataSet.name }}】のデータパス、データ名一覧
    </el-row>
    <el-row class="data-list">
      <el-col
        v-if="dataSet.flatEntries == null || dataSet.flatEntries.length == 0"
        :span="24"
      >
        <ul v-for="(datalist, index) in dataSet.entries" :key="index">
          <li style="padding-top:5px; list-style-type:none">{{ index }}:</li>
          <li
            v-for="data in datalist"
            :key="data.id"
            style="padding-top:5px; padding-left:15px; list-style-type:none"
          >
            /kqi/input/{{ index }}/{{ data.id }} 【{{ data.name }}】
          </li>
        </ul>
      </el-col>
      <el-col v-else :span="24">
        <ul>
          <li
            v-for="data in dataSet.flatEntries"
            :key="data.id"
            style="padding-top:5px; list-style-type:none"
          >
            /kqi/input/{{ data.id }} 【{{ data.name }}】
          </li>
        </ul>
      </el-col>
    </el-row>
  </span>
</template>

<script>
export default {
  name: 'PathInfo',
  props: {
    dataSet: {
      type: Object,
      default: () => {},
    },
  },
}
</script>

<style lang="scss" scoped>
.data-list {
  overflow: auto;
  height: 120px;
  padding: 3px 15px 8px 15px;
  border: 1px solid #e4e7ed;
  border-radius: 5px;
  background-color: #f5f7fa;
  color: #999;
  margin-left: 10px;
}
</style>
