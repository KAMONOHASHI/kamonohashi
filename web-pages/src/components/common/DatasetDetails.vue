<template>
  <div>
    <pl-display-text-form label="データセットID" :value="dataSet.id" />
    <pl-display-text-form label="データセット名" :value="dataSet.name" />
    <pl-display-text-form label="メモ" :value="dataSet.memo" />
  </div>
</template>

<script>
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'

export default {
  name: 'DataSetDetails',
  components: {
    'pl-display-text-form': DisplayTextForm,
  },
  props: {
    dataSet: Object,
  },
}
</script>

<style lang="scss" scoped></style>
