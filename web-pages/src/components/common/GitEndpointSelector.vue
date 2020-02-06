<template>
  <div>
    <el-form-item label="Git" prop="gitIds">
      <el-select class="selectGit" :value="value" @change="handleChange" multiple placeholder="Select"
                 :clearable="true">
        <el-option
          v-for="item in list"
          :key="item.id"
          :label="item.name"
          :value="item.id">
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.repositoryUrl }}</span>
        </el-option>
      </el-select>
    </el-form-item>

    <el-form-item label="デフォルト" prop="defaultGitId">
      <el-select class="selectGit" :value="selectedDefaultId" @change="handleChangeDefaultId" placeholder="Select"
                 :clearable="true">
        <el-option
          v-for="item in selectedList"
          :key="item.id"
          :label="item.name"
          :value="item.id">
          <span style="float: left">{{ item.name }}</span>
          <span style="float: right; color: #8492a6; font-size: 13px">{{ item.repositryUrl }}</span>
        </el-option>
      </el-select>
    </el-form-item>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    name: 'GitEndpointSelector',
    props: ['value', 'defaultId', 'tenantId'],
    data () {
      return {
        list: []
      }
    },
    async created () {
      await this.init()
    },
    watch: {
      async tenantId () {
        await this.init()
      }
    },
    computed: {
      selectedList: function () { // 現在フォーム内で選択中のGit
        let selectedList = [] // 初期化
        if (this.value !== undefined) {
          this.list.forEach((git) => {
            if (this.value.some(r => r === git.id)) {
              // 選択中だったらリストに追加
              selectedList.push(git)
            }
          })
        }
        return selectedList
      },
      selectedDefaultId: function () { // 現在フォーム内で選択中のデフォルトGit
        if (this.value !== undefined && this.defaultId !== undefined) {
          if (this.value.some(r => r === this.defaultId) === false) {
            // デフォルトに指定中のものが選択から外されたら、デフォルトも外す
            this.handleChangeDefaultId(null) // 親にも反映
            return null
          }
        }
        return this.defaultId
      }
    },
    methods: {
      async init () {
        if (this.tenantId) {
          this.list = (await api.git.tenant.getEndpoints()).data
        } else {
          this.list = (await api.git.admin.getEndpoints()).data
        }
      },
      async handleChange (v) {
        this.$emit('input', v)
      },
      async handleChangeDefaultId (v) {
        this.$emit('changeDefaultId', v)
      }
    }
  }
</script>

<style scoped>
  .selectGit {
    width: 100%;
  }
</style>
