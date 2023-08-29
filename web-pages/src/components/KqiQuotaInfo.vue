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

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'

import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import * as gen from '@/api/api.generate'
export default Vue.extend({
  components: {
    KqiDisplayTextForm,
  },
  props: {
    quota: {
      type: Object as PropType<
        gen.NssolPlatypusApiModelsClusterApiModelsQuotaOutputModel
      >,
      default: () => ({
        cpu: null,
        memory: null,
        gpu: null,
      }),
    },
  },
})
</script>

<style lang="scss" scoped></style>
