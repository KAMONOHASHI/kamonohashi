<!--name: 学習履歴セレクタ,-->
<!--description: 学習履歴を選択するドロップダウンメニュー。選択すると詳細がホバーで出る。,-->
<template>
  <el-form-item :label="title" prop="training">
    <el-popover
      v-if="!multiple"
      ref="detail-popover"
      :disabled="value.length !== 1"
      title="学習詳細"
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

<script>
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails'

export default {
  components: {
    KqiTrainingHistoryDetails,
  },
  props: {
    // 学習履歴一覧
    histories: {
      type: Array,
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
      type: Array,
      default: () => {
        return []
      },
    },
    // tensorBoardの設定で使用されているかのフラグ
    tensorBoardFlag: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      title: 'マウントする学習',
    }
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
  created() {
    if (this.tensorBoardFlag) {
      this.title = '追加する学習結果'
    }
  },
  methods: {
    async onChange(training) {
      if (training === '') {
        // clearボタンが押下された場合、空配列でemit
        this.$emit('input', [])
        return
      }

      if (this.multiple) {
        // 複数選択の場合はObjectの配列のため、そのままemit
        this.$emit('input', training)
      } else {
        // 単一選択の場合は単体Objectであるため、配列に格納してemit
        this.$emit('input', [training])
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.el-select {
  width: 100% !important;
}
</style>
