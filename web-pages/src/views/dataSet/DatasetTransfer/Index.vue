<template>
  <!-- データセット管理画面において、データ一覧(all data)を各entry(training, testing...)に振り分けるコンポーネント -->
  <!-- this.entryListに結果を格納し、その内容をemitする -->
  <div>
    <!-- all data, training... の欄の表示/非表示を切り替えるチェックボックス -->
    <div v-if="!isFlat" :style="{ marginLeft: '20px', marginBottom: '20px' }">
      <el-checkbox-group
        v-model="selectedList"
        :min="1"
        @change="handleViewGroupChange"
      >
        <el-checkbox
          v-for="(entryName, index) in checkList"
          :key="index"
          :label="entryName"
          :class="'assign assign-' + index"
          border
        />
      </el-checkbox-group>
    </div>

    <!-- 各データ区分の表示欄 -->
    <div id="list-container">
      <!-- all data の表示欄 -->
      <span v-if="dataViewInfo.visible">
        <data-list
          ref="data"
          :data-list="dataViewInfo.dataList"
          :view-info="dataViewInfo"
          :move-list="moveList"
          :disabled="disabled"
          :width="dataViewInfo.width"
          @showData="handleShowData"
          @add="handleAdd"
          @remove="handleRemove"
          @paging="handleDataViewPaging"
          @filter="x => retrieveData(dataViewInfo.currentPage, x.filter)"
        />
      </span>

      <!-- 他のデータ区分(training, testingの表示欄 -->
      <span v-for="viewInfo in entryViewInfo" :key="viewInfo.entryName">
        <!-- dataList, widthは変更監視を有効にするために直接propに入れる -->
        <template v-if="viewInfo.visible">
          <data-list
            :data-list="viewInfo.dataList"
            :view-info="viewInfo"
            :move-list="moveList"
            :disabled="disabled"
            :width="viewInfo.width"
            @showData="handleShowData"
            @add="handleAdd"
            @remove="handleRemove"
            @paging="handlePaging"
            @filter="handleFilter"
          />
        </template>
      </span>
    </div>
  </div>
</template>

<script>
import DataList from './DataList'
import Util from '@/util/util'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('data')

export default {
  components: {
    DataList,
  },
  props: {
    // 各項目ごとに分類されているデータ配列のオブジェクト
    // {
    //   training: Array(3)
    //   testing: Array(1)
    //   ...
    // }
    // Arrayの中身はdata: {id: , name: , ...}
    value: {
      type: Object,
      default: () => {
        return {}
      },
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    isFlat: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      defaultViewInfo: {
        visible: true,
        currentPage: 1,
        currentPageSize: 100,
        width: 330,
        filter: null, // 検索条件
      },
      // 表示の性能問題が出たため、子に直接データをpropで渡さずに親側で表示分を取り出して渡す
      dataViewInfo: {}, // all dataのpaging情報やentry自体の表示情報。createdで初期化
      entryViewInfo: [], // 各entry(training, testing, ...)のpaging情報やentry自体の表示情報

      selectedList: [], // checkListの内選択されたもの
      checkList: [], // checkListの項目名
      moveList: {}, // Entry名とドロップダウンに表示する移動メッセージの連想配列
      entryList: {}, // datasetの各entryの持つ dataリストの連想配列。dataset APIから受け取ったもの
      filteredEntryList: {}, // フィルタを適用したentryList
    }
  },
  computed: {
    ...mapGetters(['data', 'total']),
  },

  watch: {
    async value() {
      this.entryList = this.value
      this.$forceUpdate()
    },
  },
  async created() {
    // all dataの初期化
    this.dataViewInfo = this.makeViewInfo({
      entryName: 'all　data', // 全角スペースによりエントリー名との衝突を回避
      colorIndex: 0,
      showAssign: true,
    })
    this.entryList = this.value

    this.checkList = [this.dataViewInfo.entryName].concat(
      Object.keys(this.entryList),
    )

    for (let entryName in this.entryList) {
      this.moveList[entryName] = `選択を ${entryName} に移動`
    }
    this.moveList[this.dataViewInfo.entryName] = '選択を削除'

    await this.initViewInfo()
    await this.retrieveData(1, '')

    // 表示更新
    for (let entryName in this.entryList) {
      this.handleFilter({ entryName, filter: null })
      this.handlePaging({ entryName, page: 1 })
    }
    if (this.isFlat) {
      this.selectedList = [this.dataViewInfo.entryName, 'selected']
    } else {
      // 要素なしEntryは非表示
      let nonEmptyEntries = Object.keys(this.entryList).filter(
        x => this.entryList[x].length > 0,
      )
      this.selectedList = [this.dataViewInfo.entryName].concat(nonEmptyEntries)
    }
    this.handleViewGroupChange()
  },
  methods: {
    ...mapActions(['fetchData']),

    // データ一覧を取得し、それぞれのデータをentryに割り当てる
    async retrieveData(page, filter) {
      let params = Object.assign({}, filter)
      params.page = page
      params.perPage = this.dataViewInfo.currentPageSize
      params.withTotal = true
      await this.fetchData(params)
      this.dataViewInfo.dataList = this.data
      this.dataViewInfo.filteredTotal = this.total
      this.dataViewInfo.currentPage = page
      this.refreshAssign()
    },

    // 全データの割り当て状況をアップデートする
    refreshAssign() {
      this.dataViewInfo.dataList.forEach(x => this.setAssign(x))
      this.$refs.data.$forceUpdate()
      this.$forceUpdate()
    },

    // あるデータの割り当て状況を変更する
    setAssign(data) {
      // 最初に初期化（見つからなかった場合に備える）
      data.assign = undefined
      data.colorIndex = 0

      for (let entryName in this.entryList) {
        let listedData = this.entryList[entryName].find(x => x.id === data.id)
        if (listedData) {
          data.assign = entryName
          data.colorIndex = this.checkList.indexOf(entryName)

          // All Dataが持っているdataと他のパネルが持っているdataは別インスタンス
          // なのでそれぞれ状況を更新する
          listedData.assign = data.assign
          listedData.colorIndex = data.colorIndex

          return
        }
      }
    },

    // viewInfo(training, testing, validation)の初期化
    initViewInfo() {
      for (let entryName in this.entryList) {
        let initViewInfo = this.makeViewInfo({
          entryName,
          colorIndex: this.checkList.indexOf(entryName),
          showAssign: false,
        })
        this.entryViewInfo.push(initViewInfo)
      }
    },

    // defaultViewInfoにoptionalPropsを追加し、ViewInfoを作成する
    makeViewInfo(optionalProps) {
      return Object.assign(optionalProps, this.defaultViewInfo)
    },

    // training, testing等に対応するviewInfoを取得する
    getViewInfo(entryName) {
      return this.entryViewInfo.find(x => x.entryName === entryName)
    },

    // all dataのページング処理
    handleDataViewPaging(x) {
      this.retrieveData(x.page, x.searchCondition)
    },

    // training, testingのページング処理
    handlePaging(pagingInfo) {
      let { entryName, page } = pagingInfo
      let viewInfo = this.getViewInfo(entryName)
      let entry = this.filteredEntryList[entryName]
      let maxPage = Math.ceil(entry.length / viewInfo.currentPageSize)
      let nextPage = Math.min(page, maxPage)
      nextPage = Math.max(nextPage, 1)

      let pageStartIndex = (nextPage - 1) * viewInfo.currentPageSize // pageは1オリジン。配列は0オリジン
      let pageEndIndex = Math.min(
        pageStartIndex + viewInfo.currentPageSize,
        entry.length,
      )
      let dataList = entry.slice(pageStartIndex, pageEndIndex)
      viewInfo.currentPage = nextPage
      viewInfo.dataList = dataList
      viewInfo.filteredTotal = entry.length
      this.$forceUpdate()
    },

    // entryにデータを追加する
    insertData(entryName, viewInfo, addedIndex, data) {
      // フィルタ上で一つ下隣になるDataからentryListに挿入するindexを割り出す
      // 挿入先indexにいる現在の要素が挿入後の下隣り。
      let neighborIndex =
        (viewInfo.currentPage - 1) * viewInfo.currentPageSize + addedIndex
      if (viewInfo.filteredTotal === 0) {
        // リストが空で隣が存在しない
        this.entryList[entryName].push(data)
      } else if (neighborIndex <= viewInfo.filteredTotal - 1) {
        let neighborData = this.filteredEntryList[entryName][neighborIndex]
        let neighborOriginalIndex = this.entryList[entryName].indexOf(
          neighborData,
        )
        this.entryList[entryName].splice(neighborOriginalIndex, 0, data)
      } else {
        // 末尾への追加のため、下隣りが存在しない。現在の末尾要素を上隣りに挿入する
        let neighborData = this.filteredEntryList[entryName][
          viewInfo.filteredTotal - 1
        ]
        let neighborOriginalIndex = this.entryList[entryName].indexOf(
          neighborData,
        )
        this.entryList[entryName].splice(neighborOriginalIndex + 1, 0, data)
      }
    },

    handleShowData(id) {
      this.$emit('showData', id)
    },

    // 'add'がemitされた際の処理
    handleAdd({ data, addedIndex, entryName }) {
      if (entryName !== this.dataViewInfo.entryName) {
        // そのまま移すとチェックボックス等が連動してしまうのでコピーを作成
        data = Object.assign({}, data)
        let viewInfo = this.getViewInfo(entryName)
        this.insertData(entryName, viewInfo, addedIndex, data)
        viewInfo.currentPage =
          addedIndex >= viewInfo.currentPageSize
            ? viewInfo.currentPage + 1
            : viewInfo.currentPage
        this.handleFilter({ filter: viewInfo.filter, entryName })
      }
      this.refreshAssign()
      this.emitInput()
    },

    // 'remove'がemitされた際の処理
    handleRemove({ data, entryName }) {
      // データ列の場合はデータのアサインされているエントリの列から消す
      let removeEntryName =
        entryName === this.dataViewInfo.entryName ? data.assign : entryName
      // データ列かつ assign なしデータの場合、削除する列はない
      if (removeEntryName) {
        let viewInfo = this.getViewInfo(removeEntryName)
        this.entryList[removeEntryName] = this.entryList[
          removeEntryName
        ].filter(x => x.id !== data.id)
        this.handleFilter({
          filter: viewInfo.filter,
          entryName: removeEntryName,
        })
        this.refreshAssign()
        this.emitInput()
      }
    },

    // 'filter'がemitされた際の処理
    handleFilter(info) {
      let { filter, entryName } = info
      let viewInfo = this.getViewInfo(entryName)
      viewInfo.filter = filter
      if (filter && Object.keys(filter).length > 0) {
        // filterがnullでない＆空Objectでない場合はフィルタ
        this.filteredEntryList[entryName] = this.entryList[entryName].filter(
          x =>
            Util.isMatchAsNumber(x.id, filter.id) &&
            Util.isMatchAsText(x.name, filter.name) &&
            Util.isMatchAsText(x.memo, filter.memo) &&
            Util.isMatchAsText(x.createdBy, filter.createdBy) &&
            Util.isMatchAsDate(x.createdAt, filter.createdAt) &&
            Util.isMatchAsTextArrayByFilters(x.tags, filter.tag),
        )
      } else {
        this.filteredEntryList[entryName] = this.entryList[entryName]
      }
      this.handlePaging({ entryName, page: viewInfo.currentPage })
    },

    // entryの選択が変更された際の処理
    handleViewGroupChange() {
      let w = this.getListWidth()
      this.entryViewInfo.forEach(viewInfo => {
        viewInfo.visible = this.selectedList.includes(viewInfo.entryName)
        viewInfo.width = w
      })
      this.dataViewInfo.visible = this.selectedList.includes(
        this.dataViewInfo.entryName,
      )
      this.dataViewInfo.width = w
    },

    // コンポーネントの横幅の計算
    getListWidth() {
      let cnt = this.selectedList.length
      let obj = document.getElementById('list-container')
      let min = 330
      try {
        let w = obj.getBoundingClientRect().width
        let ret = Number(w / cnt) - 20
        return Math.max(ret, min)
      } catch (e) {
        return min
      }
    },

    emitInput() {
      this.$emit('input', this.entryList)
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

.assign {
  border-left: 10px solid $assign-other !important;
}

.assign-0 {
  border-left: 10px solid $assign-0 !important;
}

.assign-1 {
  border-left: 10px solid $assign-1 !important;
}

.assign-2 {
  border-left: 10px solid $assign-2 !important;
}

.assign-3 {
  border-left: 10px solid $assign-3 !important;
}

.assign-4 {
  border-left: 10px solid $assign-4 !important;
}

.assign-5 {
  border-left: 10px solid $assign-5 !important;
}

.assign-6 {
  border-left: 10px solid $assign-6 !important;
}

.assign-7 {
  border-left: 10px solid $assign-7 !important;
}

.assign-8 {
  border-left: 10px solid $assign-8 !important;
}

.assign-9 {
  border-left: 10px solid $assign-9 !important;
}

.assign-10 {
  border-left: 10px solid $assign-10 !important;
}

#list-container {
  display: flex;
  flex-wrap: wrap;
}
</style>
