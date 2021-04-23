<template>
  <div>
    <h2>モデルテンプレート</h2>
    <h3>AI作成のためのテンプレートを登録できます</h3>
    <div class="">
      <!-- TODO 検索ボタンデザイン -->
      <el-row :gutter="20" style="margin-top:20px">
        <el-col class="search" :span="12">
          <!-- <kqi-smart-search-input
            v-model="searchCondition"
            style="width:300px"
            :configs="searchConfigs"
            @search="search"
          /> -->
        </el-col>
        <!-- TODO新規登録ボタンデザイン -->
        <el-col class="right-top-button" :span="18">
          <el-button
            icon="el-icon-edit-outline"
            type="primary"
            plain
            @click="openCreateDialog()"
          >
            モデルテンプレート登録
          </el-button>
        </el-col>
      </el-row>

      <!-- TODO
        登録したテンプレートをカード形式で表示 -->
      <el-row>
        <el-col>
          <div class="dashboard">
            <div
              v-for="(template, index) in templateList"
              :key="index"
              class="card-container"
            >
              <el-card
                class="template"
                style="border: solid 1px #ebeef5; width: 360px; height: 300px;"
              >
                <router-link :to="`/aquarium/model-template/${template.id}`">
                  <div class="template-name">
                    {{ template.name }}
                  </div>
                </router-link>

                <div
                  class="template-description"
                  style="padding: 10px; font-size: 14px;"
                >
                  <span v-for="(s, i) in template.memoList" :key="i">
                    <span
                      v-if="s.type == 'url'"
                      style=" z-index :20;font-size:15px"
                    >
                      <a
                        :href="s.value"
                        target="_blank"
                        rel="noopener noreferrer"
                        >{{ s.value }}</a
                      ></span
                    >
                    <span v-else-if="s.type == 'string'">{{ s.value }}</span>
                  </span>
                </div>
                <!-- タグを想定 -->
                <div
                  class="template-description"
                  style="padding: 20px; font-size: 18px;text-align:center;"
                >
                  <!-- <div>
                      <el-tag class="tag"> {{ template.tag }}</el-tag>
                    </div> -->
                </div>
              </el-card>
            </div>
          </div>
        </el-col>
      </el-row>
      <router-view @cancel="closeDialog" @done="done" />
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
// import KqiSmartSearchInput from '@/components/KqiSmartSearchInput/Index'
// TODO template API に変更
const { mapGetters, mapActions } = createNamespacedHelpers('template')

export default {
  title: 'モデルテンプレート',
  components: {
    // KqiSmartSearchInput,
  },
  data() {
    return {
      templateList: null,
      pageStatus: {
        currentPage: 1,
        currentPageSize: 100,
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
    // TODO template API に変更
    ...mapGetters(['templates']),
  },
  async created() {
    // TODO template API に変更
    await this.retrieveData()
  },

  methods: {
    ...mapActions(['fetchModelTemplates']),

    async retrieveData() {
      this.templateList = []
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchModelTemplates(params)
      for (let i in this.templates) {
        let memo_org = this.templates[i].memo
        let memoList = this.urlSplitter(memo_org, this)
        this.templates[i].memoList = memoList
        this.templateList.push(this.templates[i])
      }
    },
    urlSplitter(memo, that) {
      if (
        memo != null &&
        memo.length > 0 &&
        (memo.indexOf('https://') > -1 || memo.indexOf('http://') > -1)
      ) {
        let h_num = 0
        let st = null
        let num = memo.indexOf('http://')
        let nums = memo.indexOf('https://')
        if (nums == -1 || num < nums) {
          st = num
          h_num = 7
        } else if (num == -1 || num > nums) {
          st = nums
          h_num = 8
        } else {
          return
        }
        let end = 0
        for (let j = st + h_num; j < memo.length; j++) {
          if (
            !memo[j].match(/^[A-Za-z0-9]*$/) &&
            memo[j] != '-' &&
            memo[j] != '_' &&
            memo[j] != '/' &&
            memo[j] != '.' &&
            memo[j] != '#'
          ) {
            end = j
            break
          }
        }
        if (end == 0) {
          end = memo.length
        }

        let before = memo.substring(0, st)
        let url = memo.substring(st, end)
        let after = memo.substring(end, memo.length)

        let memoList = [
          { type: 'string', value: before },
          { type: 'url', value: url },
        ]

        memoList = memoList.concat(that.urlSplitter(after, that))

        return memoList
      } else {
        return [{ type: 'string', value: memo }]
      }
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
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
    // openEditDialog(selectedTemplate) {
    //   this.$router.push('/aquarium/model-template/edit/' + selectedTemplate.id)
    // },
    // TODO テンプレートIDを引数にとってページ遷移
    openEditDialog() {
      this.$router.push('/aquarium/model-template/edit/')
    },
    openCreateDialog() {
      this.$router.push('/aquarium/model-template/create')
    },
    closeDialog() {
      this.$router.push('/aquarium/model-template')
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
.right-top-button {
  text-align: right;
}
</style>
