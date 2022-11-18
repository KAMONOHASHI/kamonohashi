<template>
  <el-form-item label="前処理" prop="preprocessing">
    <el-popover
      ref="detail-popover"
      :disabled="!value"
      title="前処理詳細"
      trigger="hover"
      width="350"
      placement="right"
    >
      <kqi-preprocessing-details :preprocessing="detail" />
    </el-popover>
    <div class="el-input">
      <el-select
        v-popover:detail-popover
        :value="detail"
        filterable
        value-key="id"
        remote
        clearable
        @change="onChange"
      >
        <el-option
          v-for="item in preprocessings"
          :key="item.id"
          :label="item.name"
          :value="item"
        >
          <span style="float: left;">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px;">
            {{ item.memo }}
          </span>
        </el-option>
      </el-select>
    </div>
  </el-form-item>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiPreprocessingDetails from '@/components/selector/KqiPreprocessingDetails.vue'
import { PropType } from 'vue'
import * as gen from '@/api/api.generate'
export default Vue.extend({
  components: {
    KqiPreprocessingDetails,
  },
  props: {
    // 前処理一覧
    preprocessings: {
      type: Array as PropType<
        Array<gen.NssolPlatypusApiModelsPreprocessingApiModelsIndexOutputModel>
      >,
      default: () => {
        return []
      },
    },
    // 選択された前処理ID
    value: {
      type: String,
      default: null,
    },
  },
  computed: {
    // 選択されている前処理を返す
    detail() {
      return this.preprocessings.find(
        preprocessing => String(preprocessing.id) === this.value,
      )
    },
  },
  methods: {
    onChange(
      preprocessing:
        | string
        | gen.NssolPlatypusApiModelsPreprocessingApiModelsIndexOutputModel,
    ) {
      if (preprocessing === '') {
        this.$emit('input', null)
      } else {
        this.$emit('input', String(preprocessing.id))
      }
    },
  },
})
</script>

<style lang="scss" scoped>
.el-select {
  width: 100% !important;
}
</style>
