<template>
  <!-- アクセストークン取得 -->
  <div>
    <el-row class="row-element" style="padding-top: 50px;">
      <el-col :span="6" class="content-color">
        期限切れまでの日数
      </el-col>
      <el-col :span="17">
        <el-slider
          :value="value"
          class="el-input"
          :min="1"
          :max="3650"
          show-input
          @input="updateDay"
        />
      </el-col>
      <el-col :offset="6" :span="12">
        値は 1 ～ 3650 の数字を入力して下さい。
      </el-col>
    </el-row>
    <el-row class="row-element">
      <div v-if="!token">
        <el-col class="button-group">
          <el-button type="primary" @click="$emit('getAccessToken')">
            トークン発行
          </el-button>
        </el-col>
      </div>
      <div v-else>
        <el-col :span="6" class="content-color">
          トークン
        </el-col>
        <el-col :span="17">
          <el-input v-model="token" type="textarea" autosize readonly />
        </el-col>
      </div>
    </el-row>
  </div>
</template>

<script>
export default {
  props: {
    // 期限切れまでの日数
    value: {
      type: Number,
      default: 30,
    },
    token: {
      type: String,
      default: null,
    },
  },
  methods: {
    updateDay(day) {
      this.$emit('input', day)
    },
  },
}
</script>

<style scoped>
.row-element {
  font-size: 14px;
  line-height: 40px;
  margin-top: 30px;
  font-weight: bold !important;
}

.button-group {
  text-align: right;
  padding-top: 100px;
  padding-right: 30px;
}

.content-color {
  color: #606266;
}
</style>
