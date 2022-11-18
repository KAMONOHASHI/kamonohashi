<template>
  <div>
    <kqi-display-text-form
      label="ID"
      :value="inference ? String(inference.id) : '0'"
    />
    <kqi-display-text-form
      label="推論名"
      :value="inference ? inference.name : ''"
    />
    <kqi-display-text-form
      label="ステータス"
      :value="inference ? inference.status : ''"
    />
    <kqi-display-text-form
      label="メモ"
      :value="inference ? inference.memo : ''"
    />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import { PropType } from 'vue'
export default Vue.extend({
  components: {
    KqiDisplayTextForm,
  },
  props: {
    inference: {
      type: Object as PropType<{
        id: number
        name: string
        status: string
        memo: string
      }>,
      default: () => ({
        id: 0,
        name: '',
        status: '',
        memo: '',
      }),
    },
  },
})
</script>

<style lang="scss" scoped></style>
