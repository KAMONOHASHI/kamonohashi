<template>
  <div>
    <el-row>
      <el-col :span="24">
        <el-button
          type="text"
          style="font-size: 1.8rem;"
          icon="el-icon-location"
          @click="handleNav('/')"
        />
        <span
          v-for="(d, idx) in navigation"
          :key="idx"
          style="font-size: 1.8rem; line-height: 2.5rem;"
        >
          <span v-if="idx !== 0">/</span>
          <span v-if="d !== ``">
            <el-button
              type="text"
              style="font-size: 1.8rem;"
              @click="handleNav(navigation.slice(0, idx + 1).join('/') + '/')"
            >
              {{ d }}
            </el-button>
          </span>
        </span>
      </el-col>
    </el-row>

    <hr />
    <el-row style="height: 500px; overflow: auto;">
      <el-col :span="24">
        <el-table :data="fileList">
          <el-table-column label="" width="35px">
            <template slot-scope="scope">
              <i v-if="!scope.row.isDirectory" class="el-icon-document" />
            </template>
          </el-table-column>
          <el-table-column prop="name" label="ファイル名" width="auto">
            <template slot-scope="scope">
              <span v-if="scope.row.isDirectory">
                <el-button type="text" @click="handleNavAdd(scope.row.name)">
                  {{ scope.row.name }}
                </el-button>
              </span>
              <span v-else>
                {{ scope.row.name }}
                <a
                  :href="scope.row.url"
                  :download="scope.row.name"
                  style="margin-left: 5px;"
                >
                  <el-button icon="el-icon-download" size="mini" />
                </a>
                <!-- 画像プレビュー -->
                <!-- ここから呼び出す場合、checkFileSizeでサイズの確認が終わっている -->
                <!-- そのため、file-size=0として呼び出し、コンポーネントでファイルの -->
                <!-- サイズのチェックをさせない -->
                <kqi-display-image-button
                  v-if="isImage(scope.row.name)"
                  :download-url="scope.row.url"
                  :file-name="scope.row.name"
                  :show-image="checkFileSize(scope.row.size)"
                  :file-size="0"
                />
              </span>
            </template>
          </el-table-column>
          <el-table-column
            prop="size"
            label="サイズ"
            width="150px"
            align="right"
          />
          <el-table-column label="" width="20px" />
          <el-table-column prop="lastModified" label="更新日時" width="210px" />
        </el-table>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import KqiDisplayImageButton from '@/components/KqiDisplayImageButton'

export default {
  name: 'FileIndex',
  components: { KqiDisplayImageButton },
  props: {
    fileList: {
      type: Array,
      default: () => {
        return []
      },
    },
  },
  data() {
    return {
      dialogVisible: true,
      error: undefined,
      path: '/',
    }
  },
  computed: {
    navigation() {
      return this.path.split('/')
    },
  },
  methods: {
    async handleNav(path) {
      this.path = path
      this.$emit('updatePath', this.path)
    },
    async handleNavAdd(dir) {
      this.path += dir + '/'
      this.$emit('updatePath', this.path)
    },
    isImage(fileName) {
      return (
        fileName.endsWith('.png') ||
        fileName.endsWith('.jpg') ||
        fileName.endsWith('.jpeg') ||
        fileName.endsWith('.gif') ||
        fileName.endsWith('.bmp')
      )
    },
    checkFileSize(fileSizeString) {
      if (fileSizeString.endsWith('Byte') || fileSizeString.endsWith('KB')) {
        return true
      } else {
        // case fileSizeString ends with MB
        var fileSize = parseFloat(fileSizeString.split(' ')[0])
        return fileSize < 10
      }
    },
  },
}
</script>

<style lang="scss" scoped>
.dialog /deep/ .el-dialog {
  min-width: 800px;
}

.dialog /deep/ label {
  font-weight: bold !important;
}

.dialog /deep/ .el-dialog__title {
  font-size: 24px;
}

.right-button-group {
  text-align: right;
}

.footer {
  padding-top: 40px;
}
</style>
