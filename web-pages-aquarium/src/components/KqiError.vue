<template>
  <div class="parent">
    <p class="parent2">
      <el-card class="box-card frame">
        <el-row>
          <el-col :span="11" class="frame-image">
            <span style="font-size: 250px; vertical-align: middle;" />
            <img
              v-if="status === 404"
              class="img"
              src="@/assets/error_404.png"
              width="300px"
              alt=""
            />
            <img
              v-else-if="status === 503"
              class="img"
              src="@/assets/error_503.png"
              width="300px"
              alt=""
            />
            <img
              v-else
              src="@/assets/error.png"
              class="img"
              width="300px"
              alt=""
            />
          </el-col>
          <el-col :span="13" class="frame-text">
            <div class="error-message">
              <h2>不正な操作が行われました。</h2>
              <h5>
                <span v-if="url">
                  "{{ url }}" のページが見つかりませんでした。<br />
                </span>
                <span v-if="status"> コード：{{ status }}<br /> </span>
                <span v-if="message"> メッセージ：{{ message }}<br /> </span>
              </h5>
              <h4>
                解決方法<br />
                <br />
                ・再度操作を行ってください<br />
                <br />
                ・マニュアルを確認<br />
                <br />
                サーバにエラーが記録されました。<br />
                現在、鋭意努力しておりますので今しばらくお待ちください。
              </h4>
            </div>
          </el-col>
        </el-row>
      </el-card>
    </p>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
interface DataType {
  url: string | (string | null)[]
  status: string | (string | null)[]
  message: string | (string | null)[]
}

export default Vue.extend({
  data(): DataType {
    return {
      url: this.$route.query.url,
      status: this.$route.query.status,
      message: this.$route.query.message,
    }
  },
  watch: {
    $route() {
      this.url = this.$route.query.url
      this.status = this.$route.query.status
      this.message = this.$route.query.message
    },
  },
})
</script>

<style lang="scss" scoped>
.parent {
  position: relative;
  height: 600px;
}

.parent2 {
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  -webkit-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
  width: 800px;
  text-align: center;
}

.frame {
  border: 1px solid gray;
  border-color: rgb(235, 238, 245) rgb(235, 238, 245) rgb(235, 238, 245)
    rgb(26, 191, 213);
  border-style: solid;
  border-width: 1px 1px 1px 20px;
  border-image: none 100% / 1 / 0 stretch;
}

.frame-image {
  vertical-align: middle;
}

.frame-text {
  border-left: dashed rgb(26, 191, 213) 2px;
  padding-left: 20px;
}

.img {
  vertical-align: middle;
}

.error-message {
  text-align: left;
}
</style>
