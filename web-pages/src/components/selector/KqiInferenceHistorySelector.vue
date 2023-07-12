<!--name: 学習履歴セレクタ,-->
<!--description: 学習履歴を選択するドロップダウンメニュー。選択すると詳細がホバーで出る。,-->
<template>
  <el-form-item label="マウントする推論" prop="inference">
    <el-popover
      v-if="!multiple"
      ref="detail-popover"
      :disabled="value.length !== 1"
      title="推論詳細"
      trigger="hover"
      width="350"
      placement="right"
    >
      <kqi-training-history-details :training="computedValue" />
    </el-popover>
    <div class="el-input">
      <el-select
        v-popover:detail-popover
        filterable
        value-key="id"
        remote
        clearable
        :value="computedValue"
        :multiple="multiple"
        @change="onChange"
      >
        <el-option
          v-for="item in histories"
          :key="item.id"
          :label="item.fullName"
          :value="item"
        />
      </el-select>
    </div>
  </el-form-item>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails.vue'
import { PropType } from 'vue'
import * as gen from '@/api/api.generate'
export default Vue.extend({
  components: {
    KqiTrainingHistoryDetails,
  },
  props: {
    // 学習履歴一覧
    histories: {
      type: Array as PropType<
        Array<
          gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
        >
      >,
      default: () => {
        return []
      },
    },
    // 複数選択可能かどうか
    multiple: {
      type: Boolean,
      default: false,
    },
    // 選択項目を表す配列
    value: {
      type: Array as PropType<
        Array<
          gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
        >
      >,
      default: () => {
        return []
      },
    },
  },
  computed: {
    computedValue: function() {
      if (this.multiple) {
        // 複数選択可能な場合はObjectの配列を表示
        return this.value
      } else {
        // 単体選択の場合はObject単体を配列から取り出して表示
        return this.value[0]
      }
    },
  },
  methods: {
    async onChange(
      inference:
        | string
        | gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
        | Array<
            gen.NssolPlatypusApiModelsInferenceApiModelsInferenceIndexOutputModel
          >,
    ) {
      if (inference === '') {
        // clearボタンが押下された場合、空配列でemit
        this.$emit('input', [])
        return
      }

      if (this.multiple) {
        // 複数選択の場合はObjectの配列のため、そのままemit
        this.$emit('input', inference)
      } else {
        // 単一選択の場合は単体Objectであるため、配列に格納してemit
        this.$emit('input', [inference])
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
