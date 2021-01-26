<template>
  <div>
    <el-row>
      <el-col :span="12">
        <el-form>
          <el-form ref="form0" :model="form" :rules="rules">
            <kqi-display-error :error="error" />

            <el-form-item label="テンプレート名" prop="name">
              <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="説明文" prop="memo">
              <el-input v-model="form.memo" type="textarea" />
            </el-form-item>
            <el-form-item label="公開設定 " prop="publishing"
              ><br />
              <div style="display:block">
                <el-radio
                  v-model="form.publishing"
                  label="1"
                  style="display:block"
                  >現在のテナント
                </el-radio>

                <el-radio
                  v-model="form.publishing"
                  label="2"
                  style="display:block"
                  >全テナントに公開</el-radio
                >
              </div>
            </el-form-item>
          </el-form>

          <el-button type="primary" plain @click="openCreateDialog()">
            更新
          </el-button>
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
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      form: { name: null, memo: null, publishing: null },

      rules: {
        name: [{ required: true, trigger: 'blur', message: '必須項目です' }],
      },
      error: null,
      isPatch: false,
    }
  },
  computed: {
    ...mapGetters(['templates']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchModelTemplates']),
    async retrieveData() {
      if (this.detail != null) {
        this.form.name = this.detail.name
        this.form.publishing = this.detail.publishing
        this.form.memo = this.detail.memo
      }
    },
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
