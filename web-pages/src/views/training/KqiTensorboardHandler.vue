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
        <kqi-display-text-form label="残り時間" :value="remainingTime" />
        <el-form-item
          v-if="selectedMountHistories.length !== 0"
          label="追加表示した学習結果"
        >
          <span class="selected-mount-histories">
            <div
              v-for="selectedMountHistory in selectedMountHistories"
              :key="selectedMountHistory.id"
            >
              <el-popover
                ref="mountDetail"
                title="追加表示した学習詳細"
                trigger="hover"
                width="350"
                placement="left"
              >
                <kqi-training-history-details
                  :training="selectedMountHistory"
                />
                <el-button
                  slot="reference"
                  class="el-input selected-mount-history"
                  @click="showMountedHistory(selectedMountHistory.id)"
                >
                  {{ selectedMountHistory.fullName }}
                </el-button>
              </el-popover>
            </div>
          </span>
        </el-form-item>
      </div>
      <div v-else>
        利用可能リソース待機中...
      </div>
    </span>
    <span v-else-if="statusName === 'None'">
      <el-row>
        <el-col :offset="1" :span="23">
          <el-button type="primary" @click="runTensorBoard">起動</el-button>
          <el-form-item label="起動期間(h)">
            <el-slider
              v-model="expiresIn"
              class="el-input"
              :min="1"
              :max="24"
              show-input
            />
          </el-form-item>
          <kqi-training-history-selector
            v-model="selectedMountHistories"
            :histories="mountedHistories"
            :title="'追加表示する学習結果'"
            multiple
          />
        </el-col>
      </el-row>
    </span>
    <span v-else-if="statusName === 'Starting'">
      起動中...
    </span>
    <span v-else-if="statusName === 'Deleting'">
      停止中...
    </span>
    <span v-else>
      <span>起動失敗</span>
      <el-button type="primary" @click="runTensorBoard">再起動</el-button>
    </span>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm.vue'
import KqiTrainingHistorySelector from '@/components/selector/KqiTrainingHistorySelector.vue'
import KqiTrainingHistoryDetails from '@/components/selector/KqiTrainingHistoryDetails.vue'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')
const kqiHost = process.env.VUE_APP_KAMONOHASHI_HOST || window.location.hostname
import * as gen from '@/api/api.generate'
interface DataType {
  statusName: string | null // 現在のステータス。スクリプト中から適宜変更できるようにstatusとは切り離す。
  tensorboardUrl: string | null // tensorboardへのアクセスURL
  intervalId: number // ポーリングを止めるためにIDを退避しておく
  polling: boolean // ポーリング中かの判定フラグ
  expiresIn: number // 起動期間(h)
  remainingTime: string | null // 残り起動期間の文字列表記('0d 1h 0m')
  selectedMountHistories: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  > // 選択した学習履歴
  mountedHistories: Array<
    gen.NssolPlatypusApiModelsTrainingApiModelsIndexOutputModel
  > // セレクタで表示される学習履歴}
}
export default Vue.extend({
  components: {
    KqiDisplayTextForm,
    KqiTrainingHistorySelector,
    KqiTrainingHistoryDetails,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
    visible: Boolean,
  },
  data(): DataType {
    return {
      statusName: null, // 現在のステータス。スクリプト中から適宜変更できるようにstatusとは切り離す。
      tensorboardUrl: null, // tensorboardへのアクセスURL
      intervalId: -1, // ポーリングを止めるためにIDを退避しておく
      polling: false, // ポーリング中かの判定フラグ
      expiresIn: 3, // 起動期間(h)
      remainingTime: null, // 残り起動期間の文字列表記('0d 1h 0m')
      selectedMountHistories: [], // 選択した学習履歴
      mountedHistories: [], // セレクタで表示される学習履歴
    }
  },
  computed: {
    ...mapGetters(['tensorboard', 'historiesToMount']),
  },
  watch: {
    async $route() {
      // 学習履歴間での画面移動時、tensorboardの表示を更新する
      this.selectedMountHistories = []
      if (this.visible && this.id >= 0) {
        this.checkTensorBoardStatus()
      }
    },
  },

  // 準備ができたらステータスのポーリング開始
  async created() {
    this.selectedMountHistories = []
    await this.fetchHistoriesToMount({
      status: [
        'Running',
        'Completed',
        'UserCanceled',
        'Killed',
        'Failed',
        'None',
        'Error',
      ],
    })
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
    ...mapActions([
      'fetchTensorboard',
      'putTensorboard',
      'deleteTensorboard',
      'fetchHistoriesToMount',
    ]),
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

      this.mountedHistories = []
      this.historiesToMount.forEach(history => {
        if (history.id !== +this.id) {
          this.mountedHistories.push(history)
        }
      })

      if (this.tensorboard.mountedTrainingHistoryIds !== null) {
        this.selectedMountHistories = []
        this.tensorboard.mountedTrainingHistoryIds.forEach(id => {
          let tmp = this.historiesToMount.find(history => history.id === id)
          if (tmp) {
            this.selectedMountHistories.push(tmp)
          }
        })
      }
    },
    // TensorBoard起動
    async runTensorBoard() {
      // statusNameを変更し、"起動中"と表示する
      this.statusName = 'Starting'

      // 追加でマウントする学習がある場合
      let selectedHistoryIds: Array<number> = []
      if (this.selectedMountHistories) {
        this.selectedMountHistories.sort(function(a, b) {
          if (a.id! > b.id!) {
            return 1
          } else {
            return -1
          }
        })
        this.selectedMountHistories.forEach(history => {
          selectedHistoryIds.push(history.id!)
        })
      }

      let params = {
        id: this.id,
        body: {
          expiresIn: this.expiresIn * 60 * 60,
          selectedHistoryIds:
            selectedHistoryIds.length !== 0 ? selectedHistoryIds : null,
        },
      }
      await this.putTensorboard(params)
    },
    // TensorBoardを開く
    openTensorBoard() {
      window.open(this.tensorboardUrl!)
    },
    // TensorBoard削除
    async deleteTensorBoard() {
      // statusNameを変更し、"停止中"と表示する
      this.statusName = 'Deleting'
      await this.deleteTensorboard(this.id)
    },
    async showMountedHistory(selectedMountHistoryId: number) {
      this.$router.push('/training/' + selectedMountHistoryId)
    },
  },
})
</script>

<style lang="scss" scoped>
.selected-mount-histories {
  display: inline-block;
  width: 100%;
}

.selected-mount-history {
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>
