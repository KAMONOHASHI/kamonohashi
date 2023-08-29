<template>
  <div>
    <kqi-display-text-form
      label="ID"
      :value="dataSet ? String(dataSet.id) : '0'"
    />
    <kqi-display-text-form
      label="データセット名"
      :value="dataSet ? dataSet.name : ''"
    />
    <kqi-display-text-form label="メモ" :value="dataSet ? dataSet.memo : ''" />
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'

export default Vue.extend({
  components: {
    KqiDisplayTextForm,
  },
  props: {
    dataSet: {
      type: Object as PropType<{
        id: number
        name: string
        memo: string
      }>,
      default: (): {
        id: number
        name: string
        memo: string
      } => ({
        id: 0,
        name: '',
        memo: '',
      }),
    },
  },
})
</script>

<style lang="scss" scoped></style>
