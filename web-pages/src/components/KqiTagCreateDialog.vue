<template>
  <kqi-dialog
    title="タグ作成"
    type="CREATE"
    @submit="submit"
    @close="$emit('cancel')"
  >
    <el-form ref="createForm" :model="form" :rules="rules">
      <kqi-display-error :error="error" />
      <el-form-item label="データ名" prop="name">
        <br />
        <div :class="{ scroll: Object.keys(form).length > 10 }">
          <div v-for="(datum, index) in form" :key="index">
            <kqi-display-text-form :value="datum.name" />
          </div>
        </div>
      </el-form-item>
      <el-form-item label="タグ">
        <kqi-tag-editor v-model="tags" />
      </el-form-item>
    </el-form>
  </kqi-dialog>
</template>

<script>
import KqiDialog from '@/components/KqiDialog'
import KqiDisplayError from '@/components/KqiDisplayError'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiTagEditor from '@/components/KqiTagEditor'
import { createNamespacedHelpers } from 'vuex'
const { mapActions } = createNamespacedHelpers('data')

const formRule = {
  required: true,
  trigger: 'blur',
  message: '必須項目です',
}

export default {
  components: {
    KqiDialog,
    KqiDisplayError,
    KqiDisplayTextForm,
    KqiTagEditor,
  },
  props: {
    selectedData: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      form: {},
      tags: [],
      error: null,
      rules: {
        tags: [formRule],
      },
    }
  },
  created() {
    this.form = Object.assign({}, this.selectedData)
  },
  methods: {
    ...mapActions(['putDataTags']),
    submit() {
      let dataIds = []
      this.selectedData.forEach(datum => {
        dataIds.push(datum.id)
      })
      let params = {
        dataIds: dataIds,
        tags: this.tags,
      }
      this.putDataTags(params)
      this.$emit('done')
      this.error = null
    },
  },
}
</script>

<style lang="scss" scoped>
.right-button-group {
  text-align: right;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.footer {
  padding-top: 40px;
}

.scroll {
  height: 500px;
  overflow-x: hidden;
  overflow-y: scroll;
}
</style>
