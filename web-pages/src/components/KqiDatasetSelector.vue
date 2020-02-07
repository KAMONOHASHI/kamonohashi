<template>
  <el-form-item label="データセット" prop="dataSet">
    <el-popover
      ref="detail-popover"
      :disabled="Object.keys(detail).length === 0"
      title="データセット詳細"
      trigger="hover"
      width="350"
      placement="right"
    >
      <kqi-data-set-details :data-set="detail" />
    </el-popover>
    <div class="el-input">
      <el-select
        v-popover:detail-popover
        filterable
        value-key="id"
        remote
        clearable
        :value="detail"
        @change="onChange"
      >
        <el-option
          v-for="item in dataSets"
          :key="item.id"
          :label="item.name"
          :value="item"
        >
          <span class="dataset-name">{{ item.name }}</span>
          <span class="dataset-memo">{{ item.memo }}</span>
        </el-option>
      </el-select>
    </div>
  </el-form-item>
</template>

<script>
import KqiDataSetDetails from '@/components/KqiDatasetDetails.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('dataSet')

export default {
  components: {
    KqiDataSetDetails,
  },
  data() {
    return {
      dataSet: null,
    }
  },
  computed: {
    ...mapGetters(['dataSets', 'detail']),
  },
  methods: {
    async onChange(dataSet) {
      if (dataSet === '') {
        // clearボタンが押下された場合
        this.$emit('input', null)
      } else {
        this.$emit('input', dataSet.id)
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.el-select {
  width: 100% !important;
}
.dataset-name {
  float: left;
  max-width: 40vw;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
.dataset-memo {
  float: right;
  color: #8492a6;
  font-size: 13px;
  max-width: 40vw;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
