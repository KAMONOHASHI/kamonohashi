<template>
  <div>
    <pl-display-text-form label="学習ID" :value="id"/>
    <pl-display-text-form label="学習名" :value="name"/>
    <pl-display-text-form label="ステータス" :value="status"/>
    <pl-display-text-form label="メモ" :value="memo"/>
  </div>
</template>

<script >
import DisplayTextForm from '@/components/common/DisplayTextForm.vue'

export default {
  name: 'TrainingHistorySelector',
  components: {
    'pl-display-text-form': DisplayTextForm
  },
  props: {
    id: Number,
    name: String,
    status: String,
    memo: String
  }
}
</script>

<style lang="scss" scoped>
</style>
