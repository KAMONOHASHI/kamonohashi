<template>
  <div>
    <el-row>
      <el-col :span="12">
        <el-form>
          <el-form :model="form" :rules="rules">
            <el-form-item label="テンプレート名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="説明文" prop="memo">
              <el-input v-model="form.memo" type="textarea" />
            </el-form-item>
            <el-form-item label="公開設定 " prop="accessLevel"
              ><br />
              <div style="display:block">
                <el-radio
                  v-model.number="form.accessLevel"
                  :label="1"
                  style="display:block"
                  >現在のテナント
                </el-radio>
                <el-radio
                  v-model.number="form.accessLevel"
                  :label="2"
                  style="display:block"
                  >全テナントに公開</el-radio
                >
              </div>
            </el-form-item>
          </el-form>
        </el-form>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { PropType } from 'vue'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('template')
interface DataType {
  rules: {
    name: [{ required: boolean; trigger: string; message: string }]
  }
  error: null | Error
  isPatch: boolean
}

export default Vue.extend({
  props: {
    value: {
      type: Object as PropType<{
        name: string
        memo: string
        accessLevel: number
        assignedTenants: Array<any>
      }>,
      default: (): {
        name: string
        memo: string
        accessLevel: number
        assignedTenants: Array<any>
      } => {
        return {
          name: 'string',
          memo: 'string',
          accessLevel: 0,
          assignedTenants: [], //TODO 使用していない？
        }
      },
    },
  },
  data(): DataType {
    return {
      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
      error: null,
      isPatch: false,
    }
  },
  computed: {
    ...mapGetters(['templates']),
    form: {
      get: function() {
        return (this as any).value
      },
      set: function(value) {
        ;(this as any).$emit('input', value)
      },
    },
  },

  async created() {
    ;(this as any).form = { ...(this as any).value }
  },
  methods: {
    ...mapActions(['fetchModelTemplates']),
  },
})
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
