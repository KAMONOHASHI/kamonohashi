<template>
  <div>
    <h2>実験結果</h2>
    <div class="confusion-matrix">
      <!-- TODO outputValueがnullでなかった場合にconfusionmatrix.csvをファイルから取得して描画 -->
      <!-- <div v-if="outputValue"> -->
      <h3>Confusion Matrix</h3>
      <div>
        製品版で実装予定です。
        コンテナ出力ファイルにconfusion_matrix.csvが出力されていた場合にmatrixを描画します。
        <!-- この表では、モデルで各ラベルが正しく分類された頻度（青色）と、そのラベルに対して最も多く混同されたラベル（灰色）を示します。 -->
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
    </div>
    <!-- <div v-else>
        <h3>Confusion Matrix</h3>
        <div>
          コンテナ出力ファイルにconfusion_matrix.csvが出力されていた場合にmatrixを描画します。
        </div>
      </div> -->
    <!-- </div> -->


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
      labelName: [
        'labelname1',
        'labelname2',
        'labelname3',
        'labelname4',
        'labelname5',
      ],
      tesorboardVisible: true,
      outputValue: null,
      matrixdata: [
        {
          true: 'labelname1',
          labelname1: 94,
          labelname2: 1,
          labelname3: 1,
          labelname4: 1,
          labelname5: 1,
        },
        {
          true: 'labelname2',
          labelname1: 0,
          labelname2: 100,
          labelname3: 0,
          labelname4: 0,
          labelname5: 0,
        },
        {
          true: 'labelname3',
          labelname1: 0,
          labelname2: 0,
          labelname3: 95,
          labelname4: 5,
          labelname5: 0,
        },
        {
          true: 'labelname4',
          labelname1: 0,
          labelname2: 0,
          labelname3: 6,
          labelname4: 92,
          labelname5: 2,
        },
        {
          true: 'labelname5',
          labelname1: 0,
          labelname2: 3,
          labelname3: 0,
          labelname4: 2,
          labelname5: 95,
        },
      ],
    }
  },
  computed: {
    ...mapGetters(['detail']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDetail']),

    async retrieveData() {},
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
.confusion-matrix {
  margin: 40px 0;
}
h3 {
  font-size: 20px;
  margin: 10px 0;
}
.el-table .nonactive-row {
  background: red;
}
</style>
