<template>
  <div>
    <kqi-display-text-form
      label="前処理ID"
      :value="preprocessing ? String(preprocessing.id) : '0'"
    />
    <kqi-display-text-form
      label="前処理名"
      :value="preprocessing ? preprocessing.name : ''"
    />
    <kqi-display-text-form
      label="メモ"
      :value="preprocessing ? preprocessing.memo : ''"
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
    preprocessing: {
      type: Object as PropType<{ id: number; name: string; memo: string }>,
      default: () => ({
        id: 0,
        name: '',
        memo: '',
      }),
    },
  },
})
</script>

<style lang="scss" scoped></style>
