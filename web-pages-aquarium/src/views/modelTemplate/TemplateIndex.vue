<template>
  <div>
    <h2>テンプレート詳細</h2>
    <!-- TODO -->
    <!-- 詳細画面のデザイン -->
    <!-- テンプレート名、説明文、バージョン、公開設定、前処理コンテナ・学習コンテナの情報を表示 -->
    <!-- Indexの詳細ボタン(...またはeditアイコン)を押したらこの画面に遷移するようにする -->
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
</style>
