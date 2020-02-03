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
      <div v-if="current.url" class="tensorBoardlink">
        <el-button type="primary" size="small" plain @click="openTensorBoard"
          >開く</el-button
        >
        <el-button type="danger" size="small" plain @click="deleteTensorBoard"
          >停止</el-button
        >
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
import api from '@/api/v1/api'

export default {
  name: 'TensorBoardHandler',
  props: {
    trainingHistoryId: Number,
    visible: Boolean,
  },
  data() {
    return {
      current: {
        status: undefined,
        statusType: undefined,
      }, // 現在のステータス詳細
      statusName: undefined, // 現在のステータス。スクリプト中から適宜変更できるようにstatusとは切り離す。
      intervalId: -1, // ポーリングを止めるためにIDを退避しておく
      polling: undefined, // ポーリング中かの判定フラグ
    }
  },
  // 準備ができたらステータスのポーリング開始
  created() {
    let self = this
    this.intervalId = setInterval(() => {
      // 可視状態かつIDがセットされている状態でのみ、ポーリング
      if (self.visible && self.trainingHistoryId >= 0) {
        self.checkTensorBoardStatus()
      }
    }, 5000) // 5秒間隔
  },
  // ポーリングし続けることがないように、ライフサイクルに合わせて止める。ただし親がDestroyされないとこのコンポーネントもDestroyされないので注意
  beforeDestroy() {
    clearInterval(this.intervalId)
  },
  methods: {
    // TensorBoardステータス確認
    async checkTensorBoardStatus() {
      if (this.polling) {
        // 既にポーリング中だったら無視
        return
      }
      // リクエストは非同期なので、サーバ側の応答が遅いと、次のポーリングタイミングが先に来てしまうことがある。
      // 多重問い合わせを回避するために、ポーリング中フラグを立てる。
      this.polling = true

      let next = (
        await api.training.getTensorboardById({
          id: this.trainingHistoryId,
          $config: { apiDisabledLoading: true },
        })
      ).data
      if (
        this.current === null ||
        this.current.statusType !== next.statusType
      ) {
        // TS側で変えたステータスが再度上書きされないように、返り値のステータスが変わっていた場合のみ更新する
        this.statusName = next.statusType
      }
      this.current = next // status名以外が変わっている可能性があるので、更新自体はする
      this.polling = false
    },
    // TensorBoard起動
    async runTensorBoard() {
      this.statusName = 'Starting'
      // currentの更新はしない（名前がNoneのまま残る）
      await api.training.putTensorboardById({ id: this.trainingHistoryId })
    },
    // TensorBoardを開く
    openTensorBoard() {
      window.open(this.current.url)
    },
    // TensorBoard削除
    async deleteTensorBoard() {
      this.statusName = 'Deleting'
      // currentの更新はしない（名前がRunningのまま残る）
      await api.training.deleteTensorboardById({ id: this.trainingHistoryId })
    },
  },
}
</script>

<style lang="scss" scoped></style>
