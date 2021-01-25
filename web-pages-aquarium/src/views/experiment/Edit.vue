<template>
  <div>
    <h2>新しくモデルをトレーニングする</h2>
    <h3>テンプレートを選択しAIを作成します</h3>
    <div class="">
      <!-- TODO 検索ボタンデザイン -->

      <el-row :gutter="20" style="margin-top:20px">
        <el-col class="search" :span="12">
          <kqi-smart-search-input
            v-model="searchCondition"
            style="width:300px"
            :configs="searchConfigs"
            @search="search"
          />
        </el-col>
      </el-row>
      <!-- TODO
        登録したテンプレートをカード形式で表示 -->
      <div class="dashboard">
        <div
          v-for="(template, index) in templates"
          :key="index"
          class="card-container"
        >
          <router-link :to="`/aquarium/experiment/createStep/${template.id}`">
            <el-card
              class="template"
              style="border: solid 1px #ebeef5; width: 360px; height: 300px;"
            >
              <div class="template-name">
                {{ template.name }}
              </div>

              <div
                class="template-description"
                style="padding: 10px; font-size: 14px;"
              >
                {{ template.memo }}
              </div>
              <!-- タグを想定 -->
              <div
                class="template-description"
                style="padding: 20px; font-size: 18px;text-align:center;"
              >
                <div style="padding:20px">
                  <el-button type="primary">Pytorch</el-button>
                </div>
                <!-- <div>
                      <el-tag class="tag"> {{ template.tag }}</el-tag>
                    </div> -->
              </div>
            </el-card>
          </router-link>
        </div>
      </div>
      <router-view />
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
// TODO template API に変更
const { mapGetters, mapActions } = createNamespacedHelpers('template')
export default {
  title: '新規学習実行',
  components: {
    KqiSmartSearchInput,
  },
  data() {
    return {
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      unwatchLogin: undefined,
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'テンプレート名', type: 'text' },
        { prop: 'tag', name: 'タグ', type: 'text', multiple: true },
      ],
    }
  },
  computed: {
    ...mapGetters(['templates']),
  },
  async created() {
    await this.retrieveData()
  },

  methods: {
    ...mapActions(['fetchTenantModelTemplates']),

    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchTenantModelTemplates(params)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
a {
  text-decoration: none;
}

.template {
  &:hover {
    transform: scale(1.05);
  }
}

.template-name {
  font-weight: bold;
  padding: 30px 10px;
  font-size: 24px;
  border-bottom: 1px solid #ebeef5;
  text-align: center;
}

.template-description {
  font-weight: lighter;
}

.model-template {
  padding-top: 80px;
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
.tag {
  border-radius: 15px;
}
</style>
