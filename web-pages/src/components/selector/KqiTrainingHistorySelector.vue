<!--name: 学習履歴セレクタ,-->
<!--description: 学習履歴を選択するドロップダウンメニュー。選択すると詳細がホバーで出る。,-->
<template>
  <el-form-item label="親学習" prop="training">
    <el-popover
      ref="detail-popover"
      :disabled="Object.keys(detail).length === 0"
      title="親学習詳細"
      trigger="hover"
      width="350"
      placement="right"
    >
      <kqi-training-history-details :training="detail" />
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
          v-for="item in histories"
          :key="item.id"
          :label="item.fullName"
          :value="item"
        >
        </el-option>
      </el-select>
    </div>
  </el-form-item>
</template>

<script>
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('training')

export default {
  components: {
    KqiTrainingHistoryDetails,
  },
  data() {
    return {
      training: null,
    }
  },
  computed: {
    ...mapGetters(['histories', 'detail']),
  },
  methods: {
    async onChange(training) {
      if (training === '') {
        // clearボタンが押下された場合
        this.$emit('input', null)
      } else {
        this.$emit('input', training.id)
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
