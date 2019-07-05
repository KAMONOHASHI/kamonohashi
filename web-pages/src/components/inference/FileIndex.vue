<template>
  <div>
    <el-dialog class="dialog"
               title="ファイル一覧"
               :visible.sync="dialogVisible"
               :before-close="emitCancel"
               :close-on-click-modal="false">

      <pl-display-error :error="error"/>
      <el-row>
        <el-col :span="6">
          <pl-display-text-form label="推論名" :value="job.name"/>
        </el-col>
        <el-col :span="6">
          <pl-display-text-form label="開始日時" :value="job.createdAt"/>
        </el-col>
        <el-col :span="6">
          <pl-display-text-form label="完了日時" :value="job.completedAt"/>
        </el-col>
        <el-col :span="6">
          <pl-display-text-form label="ステータス" :value="job.statusType"/>
        </el-col>
      </el-row>

      <el-card>
        <el-row>
          <el-col :span="24">
            <el-button type="text" style="font-size: 1.8rem;" icon="el-icon-location"
                       @click="handleNav('/')"/>
            <span v-for="(d, idx) in navigation" :key="idx" style="font-size: 1.8rem; line-height: 2.5rem;">
              <span v-if="idx !== 0">/</span>
              <span v-if="d !== ``">
                <el-button type="text" style="font-size: 1.8rem;"
                           @click="handleNav(navigation.slice(0, idx + 1).join('/') + '/')">{{d}}</el-button>
              </span>
            </span>
          </el-col>
        </el-row>

        <hr/>
        <el-row style="height: 500px; overflow: auto">
          <el-col :span="24">
            <el-table :data="fileList">
              <el-table-column label="" width="35px">
                <template slot-scope="scope">
                  <i class="el-icon-document" v-if="!scope.row.isDirectory"/>
                </template>
              </el-table-column>
              <el-table-column prop="name" label="ファイル名" width="auto">
                <template slot-scope="scope">
                  <span v-if="scope.row.isDirectory">
                    <el-button type="text" @click="handleNavAdd(scope.row.name)">{{scope.row.name}}</el-button>
                  </span>
                  <span v-else>
                    {{scope.row.name}}
                    <a :href="scope.row.url" :download="scope.row.name" style="margin-left: 5px;">
                      <el-button icon="el-icon-download" size="mini"></el-button>
                    </a>
                  </span>
                </template>
              </el-table-column>
              <el-table-column prop="size" label="サイズ" width="150px" align="right"/>
              <el-table-column label="" width="20px"/>
              <el-table-column prop="lastModified" label="更新日時" width="210px"/>
            </el-table>
          </el-col>
        </el-row>
      </el-card>

      <el-row :gutter="20" class="footer">
        <el-col class="right-button-group" :span="24">
          <el-button @click="emitCancel">キャンセル</el-button>
          <el-button @click="emitReturn">戻る</el-button>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script>
  import DisplayTextForm from '@/components/common/DisplayTextForm.vue'
  import DisplayError from '@/components/common/DisplayError'
  import api from '@/api/v1/api'
  import Util from '@/util/util'

  export default {
    name: 'FileIndex',
    components: {
      'pl-display-text-form': DisplayTextForm,
      'pl-display-error': DisplayError
    },
    props: {
      id: String
    },
    data () {
      return {
        dialogVisible: true,
        error: undefined,
        job: {},
        path: '/',
        fileList: []
      }
    },
    computed: {
      navigation () {
        return this.path.split('/')
      }
    },
    async created () {
      await this.getDetail()
      await this.getFileList()
    },
    methods: {
      async getDetail () {
        try {
          this.job = (await api.inference.getById({id: this.id})).data
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async getFileList () {
        try {
          let params = {
            id: this.id,
            path: this.path,
            withUrl: true
          }
          let data = (await api.inference.getContainerFilesById(params)).data
          let newList = []
          data.dirs.forEach(d => newList.push({
            isDirectory: true,
            name: d.dirName
          }))
          data.files.forEach(f => newList.push({
            isDirectory: false,
            name: f.fileName,
            url: f.url,
            size: Util.getByteString(f.size),
            lastModified: f.lastModified
          }))
          this.fileList = newList
          this.error = null
        } catch (e) {
          this.error = e
        }
      },
      async handleNav (path) {
        this.path = path
        await this.getFileList()
      },
      async handleNavAdd (dir) {
        this.path += dir + '/'
        await this.handleNav(this.path)
      },
      emitCancel () {
        this.$emit('cancel')
      },
      emitReturn () {
        this.$emit('return')
      }
    }
  }

</script>
<style lang="scss" scoped>
  .dialog /deep/ .el-dialog {
    min-width: 800px;
  }

  .dialog /deep/ label {
    font-weight: bold !important
  }

  .dialog /deep/ .el-dialog__title {
    font-size: 24px
  }

  .right-button-group {
    text-align: right;
  }

  .footer {
    padding-top: 40px;
  }
</style>
