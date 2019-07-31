<template>
  <div>
    <h2> ダッシュボード </h2>
    <div class="dashboard">
      <el-card class="box-card" shadow="Always">
        <div slot="header">
          <span>Workflow</span>
        </div>
        <div>
          <el-row>
            <el-col :span="6">
              <div class="menu-name">
                <img class="logo" src="/static/images/illust_folder2_bl.png" alt="" width="160" height="135">
                <br/>
                {{'データ準備' }}
              </div>
              <div class=menu-description style="padding:10px;font-size:14px;">{{'学習で使用するデータの準備を行います'}}</div>
              <router-link :to='"/data"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'データアップロード'}}
                  </div>
                </el-row>
              </router-link>
              <router-link :to='"/preprocessing"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'前処理実行'}}
                  </div>
                </el-row>
              </router-link>
              <router-link :to='"/dataset"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'データセット作成'}}
                  </div>
                </el-row>
              </router-link>
            </el-col>
            <el-col :span="6">
              <div class="menu-name">
                <img class="logo" src="/static/images/illust_book3.png" alt="" width="160" height="135">
                <br>
                {{'note book' }}
              </div>
              <div class=menu-description style="padding:10px;font-size:14px;">{{'jupyter上で試行錯誤ができます'}}</div>

              <router-link :to='"/notebook"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'Jupyter Lab'}}
                    <br>
                  </div>
                </el-row>
              </router-link>
            </el-col>
            <el-col :span="6">
              <div class="menu-name">
                <img class="logo" src="/static/images/illust_neuralBrain_bl.png" alt="" width="160" height="135">
                <br>
                {{'学習'}}
              </div>
              <div class=menu-description style="padding:10px;font-size:14px;">{{'モデルを訓練し調整します'}}</div>
              <router-link :to='"/training"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'学習実行'}}
                    <br>
                  </div>
                </el-row>
              </router-link>
            </el-col>
            <el-col :span="6">
              <div class="menu-name">
                <img class="logo" src="/static/images/illust_machineLearning_bl.png" alt="" width="160" height="135">
                <br>
                {{'推論' }}
              </div>
              <div class=menu-description style="padding:10px;font-size:14px;">{{'学習結果を用いて推論処理を行います'}}</div>
              <router-link :to='"/inference"'>
                <el-row>
                  <div class="menu-item" style="padding:10px;font-size:16px;">
                    {{'推論実行'}}
                    <br>
                  </div>
                </el-row>
              </router-link>
            </el-col>
          </el-row>
        </div>
      </el-card>
    </div>
    <div class="footer">
      <span class="footer-content">© 2016-2019 NS Solutions Corporation, All Rights Reserved.</span>
    </div>
  </div>
</template>
<script>
  import api from '@/api/v1/api'

  export default {
    name: 'Dashboard',
    title: 'Dashboard',
    data () {
      return {
        menuList: [],
        unwatchLogin: undefined
      }
    },

    props: {
      selectedMenu: String
    },

    async created () {
      this.unwatchLogin = this.$store.watch(this.$store.getters.getLoginTenant, this.watchLogin)
      await this.init()
    },

    async beforeDestroy () {
      this.unwatchLogin()
    },

    methods: {
      async init () {
        let response = await api.menuList.getMenuList()
        this.menuList = response.data
      },
      async watchLogin (tenant) {
        if (tenant) {
          let response = await api.menuList.getMenuList()
          this.menuList = response.data
        }
      }
    }
  }
</script>
<style lang="scss" scoped>
  .menu-icon {
    position: relative;
    top: 5px;
    left: -5px;
  }

  .menu-name {
    font-weight: bold;
    padding: 10px;
    font-size: 20px;
  }

  .menu-description {
    font-weight: lighter;
  }

  .dashboard {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-evenly;
    align-content: flex-start;
  }

  .card-container {
    float: left;
    margin: 20px 20px 10px 0;
  }

  .footer {
    display: flex;
    justify-content: center;
  }

  .footer-content {
    position: fixed;
    bottom: 0;
    padding: 10px;
  }

  .box-card {
    width: 100%;
    margin-bottom: 20px;
  }
  a {
    color: #303133;
    text-decoration: none;
  }

  .menu-item {
    border: solid 1px #EBEEF5;
    padding: 2% 2% 2% 3%;
    margin: 10px 0px;
    overflow: hidden;
    width: 80%;
    max-width: 240px;
    border-radius: 4px;
  }
  .menu-item:hover {
    border-left: solid 5px #1abfd5;
  }
</style>
