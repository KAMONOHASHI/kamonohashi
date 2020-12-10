<template>
  <div>
    <el-row class="tac">
      <el-col :span="24" style="padding:15px">
        データセットバージョン
        <el-select v-model="versionValue" placeholder="Select">
          <el-option
            v-for="item in versionOptions"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <el-col :span="4">
        <h3 style="padding:15px">ファイル一覧</h3>
        <el-menu
          class="el-menu-vertical-demo"
          @open="handleOpen"
          @close="handleClose"
        >
          <el-submenu
            v-for="(list, index) in fileList"
            :key="index"
            :index="index"
          >
            <template slot="title">
              <i class="el-icon-arrow-down"></i>
              <span>{{ list.listName }}<i class="el-icon-delete"></i></span>
            </template>
            <el-menu-item-group
              v-for="(item, subIndex) in list.list"
              :key="subIndex"
            >
              <el-menu-item :index="index - subIndex">{{ item }}</el-menu-item>
            </el-menu-item-group>
          </el-submenu>
        </el-menu>
      </el-col>
      <el-col :span="18">
        <el-row>
          <el-col>
            <h3 style="padding:15px">プレビュー</h3>
          </el-col>
        </el-row>
        <div class="line"></div>
        <div>
          <el-card
            v-for="(name, index) in selectImageList"
            :key="name"
            :body-style="{ padding: '0px' }"
            style="width:300px;height:300px ;float: left;"
            :offset="index > 0 ? 2 : 0"
          >
            <img src="" class="image" style="width:300px;height:270px " />
            <div style="padding: 14px;">
              <span>{{ name }}</span>
            </div>
          </el-card>
        </div>
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
    return {
      selectImageList: [
        'rose(1).png',
        'dasy(1).png',
        'rose(2).png',
        'dasy(3).png',
      ],
      versionOptions: [
        //TODO：APIからとってくる
        {
          value: 'V1',
          label: 'V1',
        },
        {
          value: 'V2',
          label: 'V2',
        },
        {
          value: 'V3',
          label: 'V3',
        },
      ],
      versionValue: 'V3',
      fileList: [
        //TODO：APIからとってくる
        {
          listName: 'test20201201',
          list: ['rose(1).png', 'dasy(1).png', 'label.csv'],
        },
        {
          listName: 'test20201101',
          list: ['rose(1).png', 'dasy(1).png', 'label.csv'],
        },
        { listName: 'test20201001', list: [] },
        { listName: 'test20200901', list: [] },
      ],
    }
  },
  computed: {
    ...mapGetters(['dataSets', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDataSets']),

    async currentChange() {
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition

      await this.fetchDataSets(params)
    },
    closeDialog() {
      this.$router.push('/dataset')
    },
    async done() {
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

/******** */
.time {
  font-size: 13px;
  color: #999;
}

.bottom {
  margin-top: 13px;
  line-height: 12px;
}

.button {
  padding: 0;
  float: right;
}

.image {
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: '';
}

.clearfix:after {
  clear: both;
}
</style>
