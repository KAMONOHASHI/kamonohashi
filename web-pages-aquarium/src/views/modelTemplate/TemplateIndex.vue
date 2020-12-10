<template>
  <div>
    <h2>テンプレート詳細</h2>
    <!-- TODO -->
    <!-- 詳細画面のデザイン -->
    <!-- テンプレート名、説明文、バージョン、公開設定、前処理コンテナ・学習コンテナの情報を表示 -->
    <!-- Indexの詳細ボタン(...またはeditアイコン)を押したらこの画面に遷移するようにする -->
    <div>
      <div style="padding:20px">
        <el-button type="primary">Pytorch</el-button>
        <el-tag type="primary" class="tag"> {{ template.tag }}</el-tag>
      </div>
      <h2>モデルの説明</h2>
      <span>{{ template.explanation }}</span>
      <h2>使い方</h2>
      <div>トレーニング時:{{ template.training }}</div>

      <div>推論時：{{ template.reasoning }}</div>
    </div>
    <div>
      <el-button type="primary" plain
        >このテンプレートを使って新しくモデルをトレーニングする</el-button
      ><el-button type="primary" plain
        >このテンプレートを使って推論を行う</el-button
      >
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('preprocessing')

export default {
  title: 'テンプレート詳細',
  components: {},
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      template: {
        name: 'A●●工場分類',
        explanation: 'A工場の●●を分類するモデル',
        training: 'トレーニング時の使い方',
        reasoning: '推論時の使い方',
        tag: 'Classification',
      },
      tableData: [],
      error: null,
    }
  },
  computed: {
    // TODO templateAPIに変更
    ...mapGetters(['histories']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    // TODO templateAPIに変更
    ...mapActions(['fetchHistories']),

    async retrieveData() {
      // TODO templateAPIに変更
      if (this.id) {
        try {
          await this.fetchHistories(this.id)
          this.error = null
        } catch (e) {
          this.error = e
        }
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.back {
  padding-top: 20px;
  cursor: pointer;
  :hover {
    color: #409eff;
  }
}
.tag {
  border-radius: 15px;
}
</style>
