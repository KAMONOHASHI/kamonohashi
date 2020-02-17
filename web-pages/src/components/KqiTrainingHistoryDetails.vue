<template>
  <div>
    <kqi-display-text-form
      label="学習ID"
      :value="training ? String(training.id) : '0'"
    />
    <kqi-display-text-form
      label="学習名"
      :value="training ? training.name : ''"
    />
    <kqi-display-text-form
      label="ステータス"
      :value="training ? training.status : ''"
    />
    <kqi-display-text-form
      label="メモ"
      :value="training ? training.memo : ''"
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
    training: {
      type: Object,
      default: () => ({
        id: 0,
        name: '',
        status: '',
        memo: '',
      }),
    },
  },
}
</script>

<style lang="scss" scoped></style>
