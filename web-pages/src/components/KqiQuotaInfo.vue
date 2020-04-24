<template>
  <div>
    <kqi-display-text-form
      label="CPU"
      :value="quota.cpu ? quota.cpu : '無制限'"
    />
    <kqi-display-text-form
      label="メモリ"
      :value="quota.memory ? quota.memory + ' GB' : '無制限'"
    />
    <kqi-display-text-form
      label="GPU"
      :value="quota.gpu ? quota.gpu : '無制限'"
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
    quota: {
      type: Object,
      default: () => ({
        cpu: null,
        memory: null,
        gpu: null,
      }),
    },
  },
}
</script>

<style lang="scss" scoped></style>
