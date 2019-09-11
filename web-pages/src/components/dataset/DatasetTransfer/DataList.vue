<template>
  <div :style="'margin-right: 10px; width:' + width + 'px'">
    <div>
      <div class="title" :class="'color-' + viewInfo.colorIndex">
        <el-row type="flex" justify="space-between">
          <el-col class="header-label">
            <el-checkbox v-model="isAllChecked" @change="handleAllCheck"/>
            {{viewInfo.entryName}}
          </el-col>
          <el-col class="header-action" :span="6">
            <el-dropdown @command="handleSelectedCommand">
              <el-button size="mini"><i class="el-icon-more"></i></el-button>
              <el-dropdown-menu slot="dropdown">
              <span v-for="(moveMessage, entryName) in moveList" :key="entryName">
                <span v-if="entryName !== viewInfo.entryName">
                  <el-dropdown-item :command="entryName" :disabled="disabled">{{moveMessage}}</el-dropdown-item>
                </span>
              </span>
              </el-dropdown-menu>
            </el-dropdown>
          </el-col>
        </el-row>
        <el-row type="flex" justify="end">
          <el-col class="header-action">
            <pl-smart-search-input v-model="searchCondition" :configs="searchConfigs" @search="emitFilter"
                                   :mini="true"/>
          </el-col>
        </el-row>
      </div>
    </div>
    <Container class="container"
               :group-name="'1'"
               :get-child-payload="getChildPayload"
               @drop="handleDrop('view', $event)"
               drag-class="card-ghost"
               drop-class="card-ghost-drop">
      <Draggable v-for="data in dataList" :key="data.id">
        <div class="draggable-data">
          <el-row>
            <el-col :span="checkSpanSize">
              <el-checkbox v-model="data.checked" @change="handleCheck(data)"></el-checkbox>
            </el-col>
            <el-col :span="idSpanSize">
              <el-checkbox v-model="data.checked" @change="handleCheck(data)">{{data.id}}</el-checkbox>
            </el-col>
            <el-col :span="nameSpanSize">
              <div class="syouryaku"><p>{{data.name}}</p></div>
            </el-col>

            <el-col :span="dataSpanSize">{{data.createdAt}}</el-col>
            <el-col :span="tagSpanSize" style="line-height: normal;">
              <el-tag v-for="(tag, i) in data.tags" :key="i" size="mini" style="margin-right:3px;">
                {{tag}}
              </el-tag>
              &nbsp;
            </el-col>

            <el-col :span="assignSpanSize" v-if="viewInfo.showAssign">
              <div class="tag" :class="'color-' + data.colorIndex">{{data.assign}}</div>
            </el-col>

            <el-col :span="infoSpanSize" style="text-align: center;">
              <el-popover placement="right" width="500" trigger="hover">
                <div class="popover">
                  <el-row>
                    <el-col :span="15">
                      <pl-display-text label="ID" :value="data.id"/>
                      <pl-display-text label="データ名" :value="data.name"/>
                    </el-col>
                    <el-col :offset="1" :span="8">
                      <pl-display-text label="登録日時" :value="data.createdAt"/>
                      <pl-display-text label="登録者" :value="data.createdBy"/>
                    </el-col>
                  </el-row>
                  <pl-display-text label="メモ" :value="data.memo"/>
                  <el-form-item label="タグ">
                    <br clear="all"/>
                    <span style="display: block; line-height: normal;">
                      <el-tag v-for="(tag, i) in data.tags" :key="i" size="mini" style="margin-right:3px;">
                        {{tag}}
                      </el-tag>
                    </span>
                  </el-form-item>
                </div>
                <i class="el-icon-info" slot="reference"/>
              </el-popover>
            </el-col>

          </el-row>
        </div>
      </Draggable>
    </Container>
    <div class="footer" :class="'color-' + viewInfo.colorIndex">
      <el-pagination
        ref="pagination"
        style="position: relative; top: 6px; "
        layout="total,prev,next,jumper"
        :page-size="viewInfo.currentPageSize"
        :current-page="viewInfo.currentPage"
        :total="viewInfo.filteredTotal"
        @current-change="page => emitPaging(viewInfo.entryName, page, searchCondition)"
      />
    </div>
  </div>
</template>

<script>
  import {Container, Draggable} from 'vue-smooth-dnd'
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import SmartSearchInput from '@/components/common/SmartSearchInput/Index.vue'

  export default {
    name: 'EntryDataList',
    props: ['viewInfo', 'disabled', 'moveList', 'dataList', 'width'],
    components: {
      Container,
      Draggable,
      'pl-display-text': DisplayTextForm,
      'pl-smart-search-input': SmartSearchInput
    },
    data () {
      return {
        searchCondition: {}, // 検索条件
        searchConfigs: [
          {prop: 'id', name: 'ID', type: 'number'},
          {prop: 'name', name: 'データ名', type: 'text'},
          {prop: 'memo', name: 'メモ', type: 'text'},
          {prop: 'createdAt', name: '登録日時', type: 'date'},
          {prop: 'createdBy', name: '登録者', type: 'text'},
          {prop: 'tag', name: 'タグ', type: 'text', multiple: true}
        ],
        checkSpanSize: 2,
        idSpanSize: 0,
        nameSpanSize: 14,
        dataSpanSize: 0,
        tagSpanSize: 0,
        assignSpanSize: 5,
        infoSpanSize: 3,
        isAllChecked: false// 全選択ボタンを押されているかどうか
      }
    },
    created () {
      this.bind()
      this.$forceUpdate()
    },
    beforeUpdate () {
      for (let i = 0; i < this.$refs.pagination.$children.length; i++) {
        if (this.$refs.pagination.$children[i].$el.className === 'el-pagination__jump') {
          this.$refs.pagination.$children[i].$el.lastChild.textContent = '／' + this.getTotalPages(this.viewInfo) + 'ページ目へ'
        }
      }
    },
    watch: {
      async width () {
        this.bind()
        this.$forceUpdate()
      },
      async dataList () {
        this.$forceUpdate()
      }
    },
    methods: {
      bind () {
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
      handleDrop (collection, dropResult) {
        const {addedIndex, removedIndex, payload} = dropResult

        // 同じ列内の操作の場合は何もしない
        if (addedIndex !== null && removedIndex !== null) return

        // 編集不可の場合
        if (this.disabled) return

        if (addedIndex !== null) {
          let addedInfo = {data: payload, addedIndex, entryName: this.viewInfo.entryName}
          this.emitAdd(addedInfo)
        }

        if (removedIndex !== null) {
          let removedInfo = {data: payload, entryName: this.viewInfo.entryName}
          this.emitRemove(removedInfo)
        }
      },

      getChildPayload (index) {
        return this.viewInfo.dataList[index]
      },

      handleAllCheck (checked) {
        this.dataList.forEach(x => {
          x.checked = checked
        })
        this.$forceUpdate()
      },
      handleSelectedCommand (command) {
        let moveDataList = this.dataList.filter(x => x.checked)
        moveDataList.forEach(originalData => {
          originalData.checked = false
          if (command !== originalData.assign) { // 移動先(command)と現在(data.assign)が一致していたら何もしない
            // add の後に remove するが、 addによってoriginalDataのassignが変わってしまい
            // remove時に assignが取れない。 コピーを作成して assignが変わらずに伝わるようにする
            let data = Object.assign({}, originalData)
            let addedInfo = {data, addedIndex: 0, entryName: command} // 移動先に追加
            this.emitAdd(addedInfo)
            let removedInfo = {data, entryName: this.viewInfo.entryName} // このリストから削除
            this.emitRemove(removedInfo)
          } else {
            this.$forceUpdate() // そのままだとチェックボックスの変更が反映されないので、強制更新
          }
        })
        this.isAllChecked = false // 全選択ボタンは非選択状態にする
      },
      handleCheck (data) {
        this.$forceUpdate()
      },
      // addとremoveはドラッグ, moveはドロップダウンから呼ばれる
      emitAdd (info) {
        this.$emit('add', info)
      },
      emitRemove (info) {
        this.$emit('remove', info)
      },
      emitFilter () {
        this.$emit('filter', {entryName: this.viewInfo.entryName, filter: this.searchCondition})
      },
      emitPaging (entryName, page, searchCondition) {
        this.$emit('paging', {entryName, page, searchCondition})
      },
      getTotalPages () {
        if (this.viewInfo.filteredTotal !== undefined && this.viewInfo.filteredTotal !== 0 && this.viewInfo.filteredTotal > this.viewInfo.currentPageSize) {
          return Math.ceil(this.viewInfo.filteredTotal / this.viewInfo.currentPageSize)
        } else {
          return 1
        }
      }
    }
  }
</script>

<style lang="scss" scoped>
  $border-color: #c0c0c0;
  $assign-0: #1ad58e;
  $assign-1: #1ABFD5;
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
    transform: rotateZ(5deg)
  }

  .card-ghost-drop {
    transition: transform 0.18s ease-in-out;
    transform: rotateZ(0deg)
  }

  .popover /deep/ label {
    font-weight: bold !important
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

</style>
