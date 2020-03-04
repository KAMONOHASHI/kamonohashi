<template>
  <!-- アクセストークン取得 -->
  <div id="second_tab01" class="cp_tabpanel">
    <el-row class="row-element">
      <el-col :span="8">
        期限切れまでの日数
      </el-col>
      <el-col :span="16">
        <el-slider
          :value="value"
          class="el-input"
          :min="1"
          :max="3650"
          show-input
          @input="updateDay"
        />
      </el-col>
      <br />
      <el-col :offset="8" :span="16">
        値は 1 ～ 3650 の数字を入力して下さい。
      </el-col>
    </el-row>
    <el-row class="row-element">
      <div v-if="!token">
        <el-col :offset="20">
          <el-button type="primary" @click="$emit('getAccessToken')">
            トークン発行
          </el-button>
        </el-col>
      </div>
      <div v-else>
        トークン
        <el-input v-model="token" type="textarea" autosize readonly />
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
  margin: 30px;
}
</style>
