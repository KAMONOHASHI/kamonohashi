<template>
  <div>
    <el-row class="row-element" style="margin-top: 30px;font-size: 18px;">
      <el-col :span="6">Slack</el-col>
    </el-row>
    <el-row class="row-element" style="margin-top: 20px;margin-left:20px">
      <el-col :span="3">通知先URL</el-col>
      <el-col :span="19"
        ><el-input :value="value.slackUrl" type="text" @input="slackUrlChange"
      /></el-col>
    </el-row>
    <el-row class="row-element" style="margin-top: 30px;margin-left:20px">
      <el-col :span="3">メンション</el-col>
      <el-col :span="19"
        ><el-input
          :value="value.mentionId"
          type="text"
          placeholder="mention"
          @input="mentionIdChange"
      /></el-col>
    </el-row>
    <el-row>
      <el-col class="button-group" :span="21">
        <el-button type="primary" @click="$emit('sendNotification')">
          テスト通知
        </el-button>
      </el-col>
      <el-col class="button-group" :span="3">
        <el-button type="primary" @click="$emit('updateWebhook')">
          更新
        </el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      default: () => ({
        slackUrl: '',
        mentionId: '',
      }),
    },
  },
  methods: {
    slackUrlChange(slackUrl) {
      let form = Object.assign({}, this.value)
      form.slackUrl = slackUrl
      this.$emit('input', form)
    },
    mentionIdChange(mentionId) {
      let form = Object.assign({}, this.value)
      form.mentionId = mentionId
      this.$emit('input', form)
    },
  },
}
</script>

<style>
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
</style>
