<template>
  <div>
    <h2>Confusion Matrix</h2>
    <div>
      この表では、モデルで各ラベルが正しく分類された頻度（青色）と、そのラベルに対して最も多く混同されたラベル（灰色）を示します。
    </div>
    <el-table
      :data="matrixdata"
      style="width: 100%;margin-top:30px;margin-bottom:30px"
    >
      <el-table-column prop="" label="" width="150">
        <el-table-column prop="true" label="Trueラベル" width="150">
        </el-table-column>
      </el-table-column>
      <el-table-column label="予測ラベル">
        <el-table-column
          v-for="(name, index) in labelName"
          :key="index"
          :prop="name"
          :label="name"
          width="120"
        >
        </el-table-column>
      </el-table-column>
    </el-table>

    <aqualium-tensorboard-handler
      :id="String(id)"
      :visible="tesorboardVisible"
    />
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import AqualiumTensorboardHandler from './AqualiumTensorboardHandler.vue'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: '実験結果',
  components: { AqualiumTensorboardHandler },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      importfile: null,
      labelName: ['sunflowers', 'dandelion', 'tulips', 'roses', 'daisy'],
      tesorboardVisible: true,
      matrixdata: [
        {
          true: 'sunflowers',
          sunflowers: 94,
          dandelion: 1,
          tulips: 1,
          roses: 1,
          daisy: 1,
        },
        {
          true: 'dandelion',
          sunflowers: 0,
          dandelion: 100,
          tulips: 0,
          roses: 0,
          daisy: 0,
        },
        {
          true: 'tulips',
          sunflowers: 0,
          dandelion: 0,
          tulips: 95,
          roses: 5,
          daisy: 0,
        },
        {
          true: 'roses',
          sunflowers: 0,
          dandelion: 0,
          tulips: 6,
          roses: 92,
          daisy: 2,
        },
        {
          true: 'daisy',
          sunflowers: 0,
          dandelion: 3,
          tulips: 0,
          roses: 2,
          daisy: 95,
        },
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
.importfile-detail {
  padding-top: 50px;
}
.importfile-detail > h3 {
  padding-bottom: 10px;
}
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
