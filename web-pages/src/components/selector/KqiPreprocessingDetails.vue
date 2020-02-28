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

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'

export default {
  components: {
    KqiDisplayTextForm,
  },
  props: {
    preprocessing: {
      type: Object,
      default: () => ({
        id: 0,
        name: '',
        memo: '',
      }),
    },
  },
}
</script>

<style lang="scss" scoped></style>
