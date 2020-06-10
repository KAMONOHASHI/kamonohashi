<template>
  <div>
    <kqi-display-text-form
      label="ID"
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
    <el-form-item label="タグ">
      <br clear="all" />
      <span style="display: block; line-height: normal;">
        <el-tag
          v-for="(tag, i) in training.tags"
          :key="i"
          size="mini"
          style="margin-right: 3px;"
        >
          {{ tag }}
        </el-tag>
      </span>
    </el-form-item>
  </div>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'

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

<style lang="scss" scoped>
.el-tag--mini {
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
</style>
