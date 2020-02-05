<!--name: 学習履歴セレクタ,-->
<!--description: 学習履歴を選択するドロップダウンメニュー。選択すると詳細がホバーで出る。,-->
<!--props: { value: デフォルトで選択されている学習履歴情報},-->
<!--events: {  input(parent): 選択された学習履歴情報。選択解除時には null が送られる}-->
<template>
  <div>
    <!-- ポップオーバーは編集中も表示可能なようにホバーで出す -->
    <div>
      <!-- disabledで表示有無を切り替える -->
      <el-popover
        ref="detail"
        :disabled="!(parent && parent.id > -1)"
        title="親学習詳細"
        trigger="hover"
        width="350"
        placement="right"
      >
        <pl-training-history-details
          :id="parent.id"
          :name="parent.name"
          :status="parent.status"
          :memo="parent.memo"
        />
      </el-popover>
    </div>
    <div class="el-input">
      <el-select
        v-model="parent"
        v-popover:detail
        filterable
        value-key="id"
        remote
        :clearable="true"
        @change="onChange"
      >
        <el-option
          v-for="item in trainingHistories"
          :key="item.id"
          :label="item.fullName"
          :value="item"
        >
        </el-option>
      </el-select>
    </div>
  </div>
</template>

<script>
import TrainingHistoryDetails from '@/components/common/TrainingHistoryDetails.vue'
import api from '@/api/v1/api'

export default {
  components: {
    'pl-training-history-details': TrainingHistoryDetails,
  },
  props: {
    value: Object,
  },
  data() {
    return {
      trainingHistories: [],
      parent:
        this.value === undefined || this.value === null
          ? {
              id: undefined,
              name: undefined,
              status: undefined,
              memo: undefined,
            }
          : {
              id: this.value.id,
              name: this.value.name,
              status: this.value.status,
              memo: this.value.memo,
            },
    }
  },
  watch: {
    value: function getData() {
      if (this.value !== undefined) {
        this.parent = this.value
      }
    },
  },
  async created() {
    await this.getTrainingHistories()
  },
  methods: {
    async getTrainingHistories() {
      this.trainingHistories = (await api.training.getSimple()).data
    },
    async onChange(parent) {
      if (parent.id) {
        this.parent = (await api.training.getById({ id: parent.id })).data
        this.$emit('input', this.parent)
      } else {
        // 空文字選択のときは選択解除のために初期化する
        this.parent = {
          id: undefined,
          name: undefined,
          status: undefined,
          memo: undefined,
        }
        this.$emit('input', undefined)
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
