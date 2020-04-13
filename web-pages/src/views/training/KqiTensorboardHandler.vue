<!--name: TensorBoard管理コンポーネント,-->
<!--description: 指定した学習履歴IDのTensorBoardコンテナのステータスをポーリングして、結果に合わせてUIを変える。-->
<!--なし：起動ボタン-->
<!--起動中：起動中のメッセージを表示-->
<!--実行中：参照URLを表示-->
<!--その他：エラー扱い。再起動ボタンを表示。-->
<!--props: {-->
<!--trainingHistoryId: 学習履歴ID,-->
<!--visible: 可視状態。Destroyされていない状態でも非表示であればポーリングを止める。-->
<!--}-->
<template>
  <div class="el-input">
    <span v-if="!statusName">
      状態確認中...
    </span>
    <span v-else-if="statusName === 'Running'">
      <div v-if="tensorboardUrl" class="tensorBoardlink">
        <el-button type="primary" size="small" plain @click="openTensorBoard">
          開く
        </el-button>
        <el-button type="danger" size="small" plain @click="deleteTensorBoard">
          停止
        </el-button>
      </div>
      <div v-else>
        利用可能リソース待機中...
      </div>
    </span>
    <span v-else-if="statusName === 'None'">
      <el-button type="primary" @click="runTensorBoard">起動</el-button>
    </span>
    <span v-else-if="statusName === 'Starting'">
      起動中...
    </span>
    <span v-else-if="statusName === 'Deleting'">
      停止中...
    </span>
    <span v-else>
      <span>起動失敗</span>
      <el-button type="primary" @click="runTensorBoard">再実行</el-button>
    </span>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')
const kqiHost = process.env.VUE_APP_KAMONOHASHI_HOST || window.location.hostname

export default {
  props: {
    id: {
      type: String,
      default: null,
    },
    visible: Boolean,
  },
  data() {
    return {
      statusName: null, // 現在のステータス。スクリプト中から適宜変更できるようにstatusとは切り離す。
      tensorboardUrl: null, // tensorboardへのアクセスURL
      intervalId: -1, // ポーリングを止めるためにIDを退避しておく
      polling: false, // ポーリング中かの判定フラグ
    }
  },
  computed: {
    ...mapGetters(['tensorboard']),
  },
  // 準備ができたらステータスのポーリング開始
  created() {
    this.intervalId = setInterval(() => {
      // 可視状態かつIDがセットされている状態でのみ、ポーリング
      if (this.visible && this.id >= 0) {
        this.checkTensorBoardStatus()
      }
    }, 5000) // 5秒間隔
  },
  // ポーリングし続けることがないように、ライフサイクルに合わせて止める。ただし親がDestroyされないとこのコンポーネントもDestroyされないので注意
  beforeDestroy() {
    clearInterval(this.intervalId)
  },
  methods: {
    ...mapActions(['fetchTensorboard', 'putTensorboard', 'deleteTensorboard']),
    // TensorBoardステータス確認
    async checkTensorBoardStatus() {
      if (this.polling) {
        // 既にポーリング中だったら無視
        return
      }
      // リクエストは非同期なので、サーバ側の応答が遅いと、次のポーリングタイミングが先に来てしまうことがある。
      // 多重問い合わせを回避するために、ポーリング中フラグを立てる。
      this.polling = true

      await this.fetchTensorboard(this.id)
      this.statusName = this.tensorboard.statusType
      this.tensorboardUrl = this.tensorboard.nodePort
        ? `http://${kqiHost}:${this.tensorboard.nodePort}`
        : null

      this.polling = false
    },
    // TensorBoard起動
    async runTensorBoard() {
      // statusNameを変更し、"起動中"と表示する
      this.statusName = 'Starting'
      await this.putTensorboard(this.id)
    },
    // TensorBoardを開く
    openTensorBoard() {
      window.open(this.tensorboardUrl)
    },
    // TensorBoard削除
    async deleteTensorBoard() {
      // statusNameを変更し、"停止中"と表示する
      this.statusName = 'Deleting'
      await this.deleteTensorboard(this.id)
    },
  },
}
</script>

<style lang="scss" scoped></style>
