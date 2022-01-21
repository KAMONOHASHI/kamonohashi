<template>
  <!-- データセット編集画面において、データ一覧を表示するコンポーネント -->
  <div :style="'margin-right: 10px; width:' + width + 'px;'">
    <div>
      <div class="title" :class="'color-' + viewInfo.colorIndex">
        <el-row type="flex" justify="space-between">
          <el-col class="header-label">
            <el-checkbox v-model="isAllChecked" @change="handleAllCheck" />
            {{ viewInfo.entryName }}
          </el-col>
          <el-col class="header-action" :span="6">
            <el-dropdown @command="handleSelectedCommand">
              <el-button size="mini"><i class="el-icon-more"/></el-button>
              <el-dropdown-menu slot="dropdown">
                <span
                  v-for="(moveMessage, entryName) in moveList"
                  :key="entryName"
                >
                  <span v-if="entryName !== viewInfo.entryName">
                    <el-dropdown-item :command="entryName" :disabled="disabled">
                      {{ moveMessage }}
                    </el-dropdown-item>
                  </span>
                </span>
              </el-dropdown-menu>
            </el-dropdown>
          </el-col>
        </el-row>
        <el-row type="flex" justify="end">
          <el-col class="header-action">
            <kqi-smart-search-input
              v-model="searchCondition"
              :configs="searchConfigs"
              :mini="true"
              @search="emitFilter"
            />
          </el-col>
        </el-row>
      </div>
    </div>
    <Container
      v-if="isAllChecked != null"
      class="container"
      :group-name="'1'"
      :get-child-payload="getChildPayload"
      drag-class="card-ghost"
      drop-class="card-ghost-drop"
      @drop="handleDrop('view', $event)"
    >
      <Draggable v-for="data in dataList" :key="data.id">
        <div class="draggable-data">
          <el-row>
            <el-col :span="checkSpanSize">
              <el-checkbox
                v-if="isAllChecked"
                v-model="data.checked"
                checked
                @change="handleCheck()"
              />
              <el-checkbox
                v-else
                v-model="data.checked"
                @change="handleCheck()"
              />
            </el-col>
            <el-col :span="idSpanSize">
              <el-checkbox v-model="data.checked" @change="handleCheck()">
                {{ data.id }}
              </el-checkbox>
            </el-col>
            <el-col :span="nameSpanSize">
              <div class="syouryaku">
                <p>{{ data.name }}</p>
              </div>
            </el-col>

            <el-col :span="dataSpanSize">{{ data.createdAt }}</el-col>
            <el-col :span="tagSpanSize" style="line-height: normal;">
              <el-tag
                v-for="(tag, i) in data.tags"
                :key="i"
                size="mini"
                style="margin-right: 3px;"
              >
                {{ tag }}
              </el-tag>
              <!-- タグ存在時にiボタンがずれるのを防止するためのスペース -->
              <p style="white-space: pre-wrap;" v-text="emptyspace" />
            </el-col>

            <el-col v-if="viewInfo.showAssign" :span="assignSpanSize">
              <div class="tag" :class="'color-' + data.colorIndex">
                {{ data.assign }}
              </div>
            </el-col>

            <el-col :span="infoSpanSize" style="text-align: center;">
              <el-popover placement="right" width="500" trigger="hover">
                <div class="popover">
                  <div
                    v-if="$store.getters['account/isAvailableData']"
                    class="right-button-group"
                  >
                    <el-button @click="showData(data)">データ編集</el-button>
                  </div>
                  <el-row>
                    <el-col :span="15">
                      <kqi-display-text-form
                        label="ID"
                        :value="data.id ? String(data.id) : null"
                      />
                      <kqi-display-text-form
                        label="データ名"
                        :value="data.name"
                      />
                    </el-col>
                    <el-col :offset="1" :span="8">
                      <kqi-display-text-form
                        label="登録日時"
                        :value="data.createdAt"
                      />
                      <kqi-display-text-form
                        label="登録者"
                        :value="data.createdBy"
                      />
                    </el-col>
                  </el-row>
                  <kqi-display-text-form label="メモ" :value="data.memo" />
                  <el-form-item label="タグ">
                    <br clear="all" />
                    <span style="display: block; line-height: normal;">
                      <el-tag
                        v-for="(tag, i) in data.tags"
                        :key="i"
                        size="mini"
                        style="margin-right: 3px;"
                      >
                        {{ tag }}
                      </el-tag>
                    </span>
                  </el-form-item>
                </div>
                <i slot="reference" class="el-icon-info" />
              </el-popover>
            </el-col>
          </el-row>
        </div>
      </Draggable>
    </Container>
    <div class="footer" :class="'color-' + viewInfo.colorIndex">
      <el-pagination
        ref="pagination"
        style="position: relative; top: 6px;"
        layout="total,prev,next,jumper"
        :page-size="viewInfo.currentPageSize"
        :current-page="viewInfo.currentPage"
        :total="viewInfo.filteredTotal"
        @current-change="
          page =>
            emitPaging(
              viewInfo.entryName,
              page,
              searchCondition,
              dataList,
              isAllChecked,
              isAllCheckedOld,
            )
        "
      />
    </div>
  </div>
</template>

<script>
import { Container, Draggable } from 'vue-smooth-dnd'
import KqiDisplayTextForm from '@/components/KqiDisplayTextForm'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('data')
export default {
  components: {
    Container,
    Draggable,
    KqiDisplayTextForm,
    KqiSmartSearchInput,
  },

  props: {
    // dataのpaging情報やentry自体の表示情報
    viewInfo: {
      type: Object,
      default: () => {
        return {
          visible: true,
          currentPage: 1,
          currentPageSize: 100,
          width: 330,
          filter: null, // 検索条件
        }
      },
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    // Entry名とドロップダウンに表示する移動メッセージの連想配列
    // "選択をtrainingに移動"など
    moveList: {
      type: Object,
      default: () => {
        return {}
      },
    },
    // 表示するデータの配列
    dataList: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 横幅
    width: {
      type: Number,
      default: 330,
    },
  },

  data() {
    return {
      emptyspace: ' ',
      searchCondition: {}, // 検索条件
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データ名', type: 'text' },
        { prop: 'memo', name: 'メモ', type: 'text' },
        { prop: 'createdAt', name: '登録日時', type: 'date' },
        { prop: 'createdBy', name: '登録者', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
      ],
      checkSpanSize: 2,
      idSpanSize: 0,
      nameSpanSize: 14,
      dataSpanSize: 0,
      tagSpanSize: 0,
      assignSpanSize: 5,
      infoSpanSize: 3,
      isAllChecked: false, // 全選択ボタンを押されているかどうか
      isAllCheckedOld: false, // 全選択ボタンを押されていたかどうか
    }
  },
  computed: {
    ...mapGetters(['data', 'total']),
  },
  watch: {
    async width() {
      this.bind()
      this.$forceUpdate()
    },
    async dataList() {
      this.$forceUpdate()
    },
  },
  created() {
    this.bind()

    this.$forceUpdate()
  },
  beforeUpdate() {
    for (let i = 0; i < this.$refs.pagination.$children.length; i++) {
      if (
        this.$refs.pagination.$children[i].$el.className ===
        'el-pagination__jump'
      ) {
        this.$refs.pagination.$children[i].$el.lastChild.textContent =
          '／' + this.getTotalPages(this.viewInfo) + 'ページ目へ'
      }
    }
  },

  methods: {
    ...mapActions(['fetchData']),
    // レスポンシブ対応
    bind() {
      if (this.width > 1000) {
        this.checkSpanSize = 0
        this.idSpanSize = 2
        this.nameSpanSize = this.viewInfo.showAssign ? 13 : 15
        this.dataSpanSize = 3
        this.tagSpanSize = 3
        this.assignSpanSize = this.viewInfo.showAssign ? 2 : 0
        this.infoSpanSize = 1
      } else if (this.width > 600) {
        this.checkSpanSize = 0
        this.idSpanSize = 4
        this.nameSpanSize = this.viewInfo.showAssign ? 10 : 13
        this.dataSpanSize = 0
        this.tagSpanSize = 6
        this.assignSpanSize = this.viewInfo.showAssign ? 3 : 0
        this.infoSpanSize = 1
      } else if (this.width > 400) {
        this.checkSpanSize = 2
        this.idSpanSize = 0
        this.nameSpanSize = this.viewInfo.showAssign ? 16 : 20
        this.dataSpanSize = 0
        this.tagSpanSize = 0
        this.assignSpanSize = this.viewInfo.showAssign ? 4 : 0
        this.infoSpanSize = 2
      } else {
        this.checkSpanSize = 2
        this.idSpanSize = 0
        this.nameSpanSize = this.viewInfo.showAssign ? 13 : 20
        this.dataSpanSize = 0
        this.tagSpanSize = 0
        this.assignSpanSize = this.viewInfo.showAssign ? 7 : 0
        this.infoSpanSize = 2
      }
    },
    // 全EntryListにイベントが送られる。
    // 対象コンテナでremovedIndex か addedIndexが設定され、それ以外でnullを受け取る
    handleDrop(collection, dropResult) {
      const { addedIndex, removedIndex, payload } = dropResult

      // 同じ列内の操作の場合は何もしない
      if (addedIndex !== null && removedIndex !== null) return

      // 編集不可の場合
      if (this.disabled) return

      if (addedIndex !== null) {
        let addedInfo = {
          data: payload,
          addedIndex,
          entryName: this.viewInfo.entryName,
        }
        this.emitAdd(addedInfo)
      }

      if (removedIndex !== null) {
        let removedInfo = { data: payload, entryName: this.viewInfo.entryName }
        this.emitRemove(removedInfo)
      }
    },

    getChildPayload(index) {
      return this.viewInfo.dataList[index]
    },

    handleAllCheck(checked) {
      this.$emit('allCheck', { entryName: this.viewInfo.entryName })

      this.dataList.forEach(x => {
        x.checked = checked
      })
      this.isAllCheckedOld = checked
      this.$forceUpdate()
    },

    async handleSelectedCommand(command) {
      let moveDataList = []
      if (this.viewInfo.entryName == 'all　data') {
        if (this.isAllChecked) {
          let params = {}
          //全データとってくるようにする
          let maxpage = Math.ceil(this.total / 100)
          for (let i = 1; i <= maxpage; i++) {
            params.page = i
            params.perPage = 100
            params.withTotal = true
            await this.fetchData(params)
            moveDataList = moveDataList.concat(this.data)
          }
          let dataList = this.dataList.filter(x => x.checked)
          dataList.forEach(originalData => {
            originalData.checked = false
          })
          await this.$emit('addAll', {
            from: this.viewInfo.entryName,
            to: command,
          })
        } else if (this.isAllCheckedOld) {
          moveDataList = []
          await this.$emit('addAllDataOld', {
            to: command,
            datalist: this.viewInfo.dataList,
          })
          moveDataList = this.dataList.filter(x => x.checked)
          moveDataList.forEach(originalData => {
            originalData.checked = false
          })
          this.$forceUpdate()
        } else {
          moveDataList = []
          await this.$emit('addAllData', {
            to: command,
            datalist: this.viewInfo.dataList,
          })
          moveDataList = this.dataList.filter(x => x.checked)
          moveDataList.forEach(originalData => {
            originalData.checked = false
          })
          this.$forceUpdate()
        }
      } else {
        if (this.isAllChecked) {
          moveDataList = []
          await this.$emit('addAll', {
            from: this.viewInfo.entryName,
            to: command,
          })
          this.$forceUpdate()
          let dataList = this.dataList.filter(x => x.checked)
          dataList.forEach(originalData => {
            originalData.checked = false
          })
        } else if (this.isAllCheckedOld) {
          let noChecklist = []
          for (let i in this.viewInfo.dataList) {
            if (
              this.viewInfo.dataList[i].checked == null ||
              this.viewInfo.dataList[i].checked == false
            ) {
              noChecklist.push(this.viewInfo.dataList[i])
            }
          }
          moveDataList = []
          await this.$emit('addAllOld', {
            from: this.viewInfo.entryName,
            to: command,
            noChecklist: noChecklist,
          })
          this.$forceUpdate()
          moveDataList = this.dataList.filter(x => x.checked)
        } else {
          moveDataList = []
          moveDataList = this.dataList.filter(x => x.checked)
        }
      }
      moveDataList.forEach(originalData => {
        originalData.checked = false
        if (command !== originalData.assign) {
          // 移動先(command)と現在(data.assign)が一致していたら何もしない
          // add の後に remove するが、 addによってoriginalDataのassignが変わってしまい
          // remove時に assignが取れない。 コピーを作成して assignが変わらずに伝わるようにする
          let data = Object.assign({}, originalData)
          let addedInfo = { data, addedIndex: 0, entryName: command } // 移動先に追加
          this.emitAdd(addedInfo)
          let removedInfo = { data, entryName: this.viewInfo.entryName } // このリストから削除
          this.emitRemove(removedInfo)
          if (this.isAllChecked) {
            removedInfo = { data, entryName: command } //このリスト以外から削除
            this.$emit('removeAll', removedInfo)
          }
        } else {
          this.$forceUpdate() // そのままだとチェックボックスの変更が反映されないので、強制更新
        }
      })
      this.isAllChecked = false // 全選択ボタンは非選択状態にする
      this.isAllCheckedOld = false
    },
    handleCheck() {
      this.isAllChecked = false
      this.$forceUpdate()
    },
    async showData(data) {
      this.$emit('showData', data.id)
    },
    // addとremoveはドラッグ, moveはドロップダウンから呼ばれる
    emitAdd(info) {
      this.$emit('add', info)
    },
    emitRemove(info) {
      this.$emit('remove', info)
    },
    emitFilter() {
      this.$emit('filter', {
        entryName: this.viewInfo.entryName,
        filter: this.searchCondition,
      })
    },
    emitPaging(
      entryName,
      page,
      searchCondition,
      dataList,
      isAllChecked,
      isAllCheckedOld,
    ) {
      this.$emit('paging', {
        entryName,
        page,
        searchCondition,
        dataList,
        isAllChecked,
        isAllCheckedOld,
      })
    },
    getTotalPages() {
      if (
        this.viewInfo.filteredTotal !== undefined &&
        this.viewInfo.filteredTotal !== 0 &&
        this.viewInfo.filteredTotal > this.viewInfo.currentPageSize
      ) {
        return Math.ceil(
          this.viewInfo.filteredTotal / this.viewInfo.currentPageSize,
        )
      } else {
        return 1
      }
    },
  },
}
</script>

<style lang="scss" scoped>
$border-color: #c0c0c0;
$assign-0: #1ad58e;
$assign-1: #1abfd5;
$assign-2: #1a62d5;
$assign-3: #d58e1a;
$assign-4: #aaaaaa;
$assign-5: #aaaaaa;
$assign-6: #aaaaaa;
$assign-7: #aaaaaa;
$assign-8: #aaaaaa;
$assign-9: #aaaaaa;
$assign-10: #aaaaaa;
$assign-other: #aaaaaa;

.container {
  border: 1px solid $border-color;
  border-width: 0px 1px 1px 1px;
  border-radius: 0px 0px 2px 2px;
  height: 400px;
  overflow: auto;
}

.draggable-data {
  background-color: white;
  border-bottom: 1px solid $border-color;
  margin: 1px 1px 1px 1px;
  padding-left: 5px;
}

.tag {
  color: white;
  background-color: $assign-other;
  text-align: center;
  vertical-align: middle;
  border-radius: 2px 2px 2px 2px;
  margin-top: 8px;
  line-height: 30px;
}

.el-tag--mini {
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.color-0 {
  background-color: $assign-0 !important;
}

.color-1 {
  background-color: $assign-1 !important;
}

.color-2 {
  background-color: $assign-2 !important;
}

.color-3 {
  background-color: $assign-3 !important;
}

.color-4 {
  background-color: $assign-4 !important;
}

.color-5 {
  background-color: $assign-5 !important;
}

.color-6 {
  background-color: $assign-6 !important;
}

.color-7 {
  background-color: $assign-7 !important;
}

.color-8 {
  background-color: $assign-8 !important;
}

.color-9 {
  background-color: $assign-9 !important;
}

.color-10 {
  background-color: $assign-10 !important;
}

.syouryaku {
  overflow: hidden;

  p {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    line-height: normal !important;
  }
}

.card-ghost {
  border: 1px solid $border-color;
  transition: transform 0.18s ease;
  transform: rotateZ(5deg);
}

.card-ghost-drop {
  transition: transform 0.18s ease-in-out;
  transform: rotateZ(0deg);
}

.popover /deep/ label {
  font-weight: bold !important;
}

.footer /deep/ button.btn-prev,
.footer /deep/ button.btn-next {
  border-radius: 5px !important;
  margin-right: 3px !important;
}

.footer /deep/ .el-pagination__jump {
  margin-left: 5px !important;
}

.footer /deep/ span {
  font-weight: bold;
  color: white;
}

.footer {
  color: white;
  background-color: $assign-other;

  border: 1px solid $border-color;
  border-width: 0px 1px 1px 1px;
  border-radius: 0px 0px 5px 5px;
}

.title {
  width: inherit;
  color: white;
  background-color: $assign-other;

  border: 1px solid $border-color;
  border-width: 1px 1px 1px 1px;
  border-radius: 5px 5px 0px 0px;
}

.header-label {
  padding-left: 10px;
}

.header-action {
  text-align: right;
  padding-right: 10px;
}

.right-button-group {
  padding-top: 0px;
  text-align: right;
}
</style>
