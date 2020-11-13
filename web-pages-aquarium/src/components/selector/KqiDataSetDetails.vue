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

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'

export default {
  components: {
    KqiDisplayTextForm,
  },
  props: {
    dataSet: {
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
