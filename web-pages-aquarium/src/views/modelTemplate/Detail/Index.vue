<template>
  <div>
    <h2>
      テンプレート詳細＞ {{ detail.name }}
      <el-tag type="info" style="border-radius:15px">Version 1</el-tag>
    </h2>
    <el-tabs v-model="activeName">
      <el-tab-pane label="基本設定" name="baseSetting">
        <base-setting v-if="baseForm" v-model="baseForm" />
      </el-tab-pane>
      <el-tab-pane label="前処理" name="preprocessing">
        <preprocessing v-if="preprocForm" v-model="preprocForm" />
      </el-tab-pane>
      <el-tab-pane label="学習と推論" name="train">
        <training v-if="trainingForm" v-model="trainingForm" />
      </el-tab-pane>
    </el-tabs>
    <el-button type="primary" plain @click="submit()">
      更新
    </el-button>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import BaseSetting from './BaseSetting'

import ContainerSettings from './ContainerSettings'
const { mapGetters, mapActions } = createNamespacedHelpers('template')

export default {
  title: 'モデルテンプレート',
  components: {
    BaseSetting,
    Preprocessing: ContainerSettings,
    Training: ContainerSettings,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'type', name: '種類', type: 'text' },
        { prop: 'totalImageNumber', name: 'イメージの総数', type: 'text' },
        {
          prop: 'labeledImageNumber',
          name: 'ラベル付きのイメージ数',
          type: 'text',
        },
        { prop: 'lastModified', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
      ],
      tableData: [],
      activeName: 'baseSetting',
      preprocForm: null,
      trainingForm: null,
      baseForm: null,
    }
  },
  computed: {
    ...mapGetters(['detail', 'total']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDetail', 'delete', 'post']),
    async retrieveData() {
      await this.fetchDetail(this.id)

      this.baseForm = {
        name: this.detail.name,
        memo: this.detail.memo,
        accessLevel: this.detail.accessLevel,
        assignedTenants: this.detail.assignedTenants,
      }

      this.preprocForm = {
        containerImage: { ...this.detail.preprocessContainerImage },
        gitModel: { ...this.detail.preprocessGitModel },
        entryPoint: this.detail.preprocessEntryPoint,
        resource: {
          cpu: this.detail.preprocessCpu,
          memory: this.detail.preprocessMemory,
          gpu: this.detail.preprocessGpu,
        },
      }
      this.trainingForm = {
        containerImage: { ...this.detail.trainingContainerImage },
        gitModel: { ...this.detail.trainingGitModel },
        entryPoint: this.detail.trainingEntryPoint,
        resource: {
          cpu: this.detail.trainingCpu,
          memory: this.detail.trainingMemory,
          gpu: this.detail.trainingGpu,
        },
      }
    },
    async submit() {
      const params = {
        name: this.baseForm.name,
        memo: this.baseForm.memo,
        accessLevel: this.baseForm.accessLevel,
        version: 0, // 未実装のためダミー
        groupId: 0, // 未実装のためダミー
        assignedTenantId: 0, // 未実装のためダミー
        preprocessEntryPoint: this.preprocForm.entryPoint,
        preprocessContainerImage: { ...this.preprocForm.containerImage },
        preprocessGitModel: {
          gitId: this.preprocForm.gitModel.git.id,
          repository: this.preprocForm.gitModel.repository.split('/')[1],
          owner: this.preprocForm.gitModel.repository.split('/')[0],
          branch: this.preprocForm.gitModel.branch,
          commitId: this.preprocForm.gitModel.commit.commitId,
        },
        preprocessCpu: this.preprocForm.resource.cpu,
        preprocessMemory: this.preprocForm.resource.memory,
        preprocessGpu: this.preprocForm.resource.gpu,
        trainingEntryPoint: this.trainingForm.entryPoint,
        trainingContainerImage: { ...this.trainingForm.containerImage },
        trainingGitModel: {
          gitId: this.trainingForm.gitModel.git.id,
          repository: this.trainingForm.gitModel.repository.split('/')[1],
          owner: this.trainingForm.gitModel.repository.split('/')[0],
          branch: this.trainingForm.gitModel.branch,
          commitId: this.trainingForm.gitModel.commit.commitId,
        },
        trainingCpu: this.trainingForm.resource.cpu,
        trainingMemory: this.trainingForm.resource.memory,
        trainingGpu: this.trainingForm.resource.gpu,
      }
      params
      //await this['post'](params)
      this.$alert('編集は製品版で使用可能予定です', 'お知らせ', {
        confirmButtonText: 'OK',
      })
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
</style>
