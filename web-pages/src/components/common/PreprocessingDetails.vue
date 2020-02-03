<template>
  <div>
    <pl-display-text-form label="前処理ID" :value="preprocessing.id" />
    <pl-display-text-form label="前処理名" :value="preprocessing.name" />
    <pl-display-text-form label="メモ" :value="preprocessing.memo" />
  </div>
</template>

<script>
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'

export default {
  name: 'PreprocessingDetails',
  components: {
    'pl-display-text-form': DisplayTextForm,
  },
  props: {
    preprocessing: Object,
  },
}
</script>

<style lang="scss" scoped></style>
