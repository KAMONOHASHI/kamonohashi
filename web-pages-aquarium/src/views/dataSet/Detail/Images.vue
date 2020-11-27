<template>
  <div>
    <el-row class="tac">
      <el-col :span="6">
        <el-menu
          class="el-menu-vertical-demo"
          @open="handleOpen"
          @close="handleClose"
        >
          <el-menu-item index="1">
            <span>全ての画像</span>
          </el-menu-item>
          <el-menu-item index="2">
            <span>ラベル付き</span>
          </el-menu-item>
          <el-menu-item index="3">
            <span>ラベルなし</span>
          </el-menu-item>
        </el-menu>
        <el-menu
          class="el-menu-vertical-demo"
          @open="handleOpen"
          @close="handleClose"
        >
          <div class="line"></div>
          <el-menu-item index="4">
            <i class="el-icon-setting"></i>
            <span>ラベルをフィルタ</span>
            <i class="el-icon-more"></i>
          </el-menu-item>
          <div class="line"></div>
          <el-menu-item index="5">
            <span>daisy</span>
          </el-menu-item>
          <el-menu-item index="5">
            <span>dandelion</span>
          </el-menu-item>
          <el-menu-item index="5">
            <span>roses</span>
          </el-menu-item>
          <el-menu-item index="5">
            <span>sunflowers</span>
          </el-menu-item>
          <el-menu-item index="5">
            <span>tulips</span>
          </el-menu-item>
          <el-menu-item index="3">
            <span>新規ラベルを追加</span>
          </el-menu-item>
        </el-menu>
      </el-col>
      <el-col :span="18">
        <el-menu
          :default-active="activeIndex"
          class="el-menu-demo"
          mode="horizontal"
          @select="handleSelect"
        >
          <el-menu-item index="1">イメージのフィルタリング</el-menu-item>

          <el-menu-item index="3" disabled
            ><i class="el-icon-s-grid"></i
          ></el-menu-item>
        </el-menu>
        <div class="line"></div>
        <el-checkbox
          label="すべて選択"
          name="type"
          style="margin:10px"
        ></el-checkbox>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: 'データセット',
  components: {},
  data() {
    return {}
  },
  computed: {
    ...mapGetters(['dataSets', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDataSets']),

    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done(type) {
      if (type === 'delete') {
        // 削除時、表示していたページにデータが無くなっている可能性がある。
        // 総数 % ページサイズ === 1の時、残り1の状態で削除したため、currentPageが1で無ければ1つ前のページに戻す
        if (this.total % this.pageStatus.currentPageSize === 1) {
          if (this.pageStatus.currentPage !== 1) {
            this.pageStatus.currentPage -= 1
          }
        }
      }
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    openCreateDialog() {
      this.$router.push('/dataset/create')
    },
    openEditDialog(selectedRow) {
      this.$router.push('/dataset/edit/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/dataset/create/' + id)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
