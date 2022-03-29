<template>
  <div>
    <el-dialog
      :visible.sync="searchDialogVisible"
      title="詳細検索"
      :before-close="close"
    >
      <el-form ref="form" :model="searchForm" label-width="120px">
        <el-form-item label="ID">
          <el-col :span="11">
            <el-input v-model="searchForm.idLower" />
          </el-col>
          <el-col class="line" :span="2" style="text-align:center">-</el-col>
          <el-col :span="11">
            <el-input v-model="searchForm.idUpper" />
          </el-col>
        </el-form-item>
        <el-form-item label="学習名">
          <el-col :span="20">
            <multi-input v-model="searchForm.name" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.nameOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="親学習名">
          <el-col :span="20">
            <multi-input v-model="searchForm.parentName" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.parentNameOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="開始日時">
          <el-col :span="11">
            <el-date-picker
              v-model="searchForm.startedAtLower"
              type="date"
              placeholder="Pick a day"
              format="yyyy/MM/dd"
              value-format="yyyy/MM/dd"
              style="width:100%"
            />
          </el-col>
          <el-col class="line" :span="2" style="text-align:center">-</el-col>
          <el-col :span="11">
            <el-date-picker
              v-model="searchForm.startedAtUpper"
              type="date"
              placeholder="Pick a day"
              format="yyyy/MM/dd"
              value-format="yyyy/MM/dd"
              style="width:100%"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="実行者">
          <el-col :span="20">
            <multi-input v-model="searchForm.startedBy" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.startedByOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="データセット名">
          <el-col :span="20">
            <multi-input v-model="searchForm.dataSet" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.dataSetOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="メモ">
          <el-col :span="20">
            <multi-input v-model="searchForm.memo" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.memoOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="ステータス">
          <el-col :span="20">
            <multi-input v-model="searchForm.status" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.statusOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="実行コマンド">
          <el-col :span="20">
            <multi-input v-model="searchForm.entryPoint" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.entryPointOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
        <el-form-item label="タグ">
          <el-col :span="20">
            <multi-input v-model="searchForm.tags" />
          </el-col>
          <el-col :span="4">
            <el-switch
              v-model="searchForm.tagsOr"
              active-text="or"
              inactive-text="and"
            />
          </el-col>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="saveSearchFormDialogVisible = true">
          検索条件を保存
        </el-button>
        <el-button type="primary" @click="search">検索</el-button>
      </span>
    </el-dialog>
    <el-dialog
      :visible.sync="saveSearchFormDialogVisible"
      title="検索条件の保存"
    >
      <el-form>
        <el-form-item label="登録名">
          <el-col :span="18">
            <kqi-display-error :error="error" />
            <el-input v-model="searchConditionName" />
          </el-col>
          <el-col :span="4">
            <el-button type="primary" @click="saveSearchCondition">
              登録
            </el-button>
          </el-col>
        </el-form-item>
      </el-form>
    </el-dialog>
  </div>
</template>

<script>
import MultiInput from './MultiInput'
import KqiDisplayError from '@/components/KqiDisplayError'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('training')

export default {
  title: '詳細検索',
  components: {
    MultiInput,
    KqiDisplayError,
  },
  props: {
    searchDialogVisible: {
      type: Boolean,
      default: () => {
        return false
      },
    },
    searchForm: {
      type: Object,
      default: () => {
        return {
          idLower: '',
          idUpper: '',
          name: [],
          nameOr: true,
          parentName: [],
          parentNameOr: true,
          startedAtLower: '',
          startedAtUpper: '',
          startedBy: [],
          startedByOr: true,
          dataSet: [],
          dataSetOr: true,
          memo: [],
          memoOr: true,
          status: [],
          statusOr: true,
          entryPoint: [],
          entryPointOr: true,
          tags: [],
          tagsOr: true,
        }
      },
    },
  },
  data() {
    return {
      saveSearchFormDialogVisible: false,
      searchConditionName: null,
      error: null,
    }
  },
  computed: {
    ...mapGetters(['searchHistories']),
  },
  created() {},
  methods: {
    ...mapActions([
      'postSearchHistory',
      'deleteSearchHistory',
      'fetchSearchFill',
    ]),
    close() {
      this.$emit('close')
    },

    search() {
      this.$emit('search')
    },

    changeSearchFormListToString() {
      let form = {
        idLower: parseInt(this.searchForm.idLower),
        idUpper: parseInt(this.searchForm.idUpper),
        name: this.changeListToString(this.searchForm.name),
        nameOr: this.searchForm.nameOr,
        parentName: this.changeListToString(this.searchForm.parentName),
        parentNameOr: this.searchForm.parentNameOr,
        startedAtLower: this.searchForm.startedAtLower,
        startedAtUpper: this.searchForm.startedAtUpper,
        startedBy: this.changeListToString(this.searchForm.startedBy),
        startedByOr: this.searchForm.startedByOr,
        dataSet: this.changeListToString(this.searchForm.dataSet),
        dataSetOr: this.searchForm.dataSetOr,
        memo: this.changeListToString(this.searchForm.memo),
        memoOr: this.searchForm.memoOr,
        status: this.changeListToString(this.searchForm.status),
        statusOr: this.searchForm.statusOr,
        entryPoint: this.changeListToString(this.searchForm.entryPoint),
        entryPointOr: this.searchForm.entryPointOr,
        tags: this.changeListToString(this.searchForm.tags),
        tagsOr: this.searchForm.tagsOr,
      }
      return form
    },

    changeListToString(list) {
      if (list == null || list.length == 0) {
        return ''
      }
      let str = ''
      for (let i in list) {
        if (i == 0) {
          str = str + list[i]
        } else {
          str = str + ',' + list[i]
        }
      }
      return str
    },
    async saveSearchCondition() {
      if (
        this.searchConditionName == null ||
        this.searchConditionName.length < 4
      ) {
        this.error = new Error('4文字以上で入力してください')
        return
      }
      let params = {}
      params.name = this.searchConditionName
      params.searchDetailInputModel = this.changeSearchFormListToString()
      try {
        await this.postSearchHistory(params)
        this.saveSearchFormDialogVisible = false
        this.$emit('save')
      } catch (e) {
        this.error = e
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.favorite {
  color: rgb(230, 162, 60);
}

.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.entry-point-column {
  display: table-cell;
}

.entry-point-column div.cell {
  /*! autoprefixer: off */
  overflow: hidden;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  display: -webkit-box;
}

.el-tag--mini {
  max-width: 100%;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.parent {
  margin-right: 5px;
}
</style>
