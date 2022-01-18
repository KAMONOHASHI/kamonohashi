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
          グラフを開く
        </el-button>
        <el-button type="danger" size="small" plain @click="deleteTensorBoard">
          終了
        </el-button>
        <kqi-display-text-form label="残り時間" :value="remainingTime" />
      </div>
      <div v-else>
        グラフの準備中...
      </div>
    </span>
    <span v-else-if="statusName === 'None'">
      <el-row>
        <el-col :span="23">
          <el-button type="primary" @click="runTensorBoard">
            詳細をグラフで見る
          </el-button>
        </el-col>
      </el-row>
    </span>
    <span v-else-if="statusName === 'Starting'">
      グラフの準備中...
    </span>
    <span v-else-if="statusName === 'Deleting'">
      停止中...
    </span>
    <span v-else>
      <span>起動失敗</span>
      <el-button type="primary" @click="runTensorBoard"
        >再度グラフの表示を試す</el-button
      >
    </span>
  </div>
</template>

<script>
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')
const kqiHost = process.env.VUE_APP_KAMONOHASHI_HOST || window.location.hostname

export default {
  components: {
    KqiDisplayTextForm,
  },
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
      expiresIn: 30 * 60, // 起動期間(30分)
      remainingTime: null, // 残り起動期間の文字列表記('0d 1h 0m')
    }
  },
  computed: {
    ...mapGetters(['tensorboard']),
  },
  // 準備ができたらステータスのポーリング開始
  async created() {
    // 起動時の状態を確認する
    if (this.visible && this.id >= 0) {
      this.checkTensorBoardStatus()
    }
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
      // statusNameがStarting(起動中)でstatusTypeがNoneの場合は、起動ボタン押下後でtensorboardがまだ立っていないのでstatusNameを更新しない
      if (
        !(
          this.statusName === 'Starting' &&
          this.tensorboard.statusType === 'None'
        )
      ) {
        this.statusName = this.tensorboard.statusType
      }
      this.remainingTime = this.tensorboard.remainingTime
      this.tensorboardUrl = this.tensorboard.nodePort
        ? `http://${kqiHost}:${this.tensorboard.nodePort}`
        : null

      this.polling = false
    },
    // TensorBoard起動
    async runTensorBoard() {
      // statusNameを変更し、"起動中"と表示する
      this.statusName = 'Starting'

      let params = {
        id: this.id,
        body: {
          expiresIn: this.expiresIn,
        },
      }
      await this.putTensorboard(params)
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
