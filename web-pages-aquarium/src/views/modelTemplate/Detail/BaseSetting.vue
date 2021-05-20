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

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('template')
export default {
  title: 'モデルテンプレート',
  components: {},
  props: {
    value: {
      type: Object,
      default: () => {
        return {
          name: 'string',
          memo: 'string',
          accessLevel: 0,
          assignedTenants: [],
        }
      },
    },
  },
  data() {
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
      get() {
        return this.value
      },
      set(value) {
        this.$emit('input', value)
      },
    },
  },

  async created() {
    this.form = { ...this.value }
  },
  methods: {
    ...mapActions(['fetchModelTemplates']),
  },
}
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
