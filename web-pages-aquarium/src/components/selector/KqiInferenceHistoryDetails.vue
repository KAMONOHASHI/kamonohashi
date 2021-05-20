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

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'

export default {
  components: {
    KqiDisplayTextForm,
  },
  props: {
    inference: {
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
