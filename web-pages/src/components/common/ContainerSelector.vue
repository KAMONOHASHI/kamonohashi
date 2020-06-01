<!--name: コンテナのイメージ名＆バージョンの入力コンポーネント,-->
<!--description: イメージ名、タグ名を指定するドロップダウンをそれぞれ表示する,-->
<!--props: {  value: デフォルト値にするコンテナ情報,}-->
<!--events: {input(ContainerModel): 入力されたコンテナ情報。各入力値が変更されるたびに発火される。,}-->

<template>
  <div class="el-input">
    <el-row>
      <!-- レジストリの選択 -->
      <el-col :span="6" :offset="1">レジストリ</el-col>
      <el-col :span="12">
        <el-select
          size="small"
          filterable
          clearable
          remote
          @change="changeRegistry"
          :loading="listLoading"
          v-model="selectedRegistryId"
          :disabled="disabled">
          <el-option
            v-for="item in registries"
            :key="item.id"
            :label="item.name"
            :value="item.id">
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- イメージの選択 -->
      <el-col :span="6" :offset="1">イメージ</el-col>
      <el-col :span="12">
        <el-select
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          @change="changeImage"
          @focus="getImages"
          :loading="listLoading"
          v-model="selectedImage"
          :disabled="!model.registryId || disabled">
          <el-option
            v-for="(item, index) in images"
            :key="index"
            :label="item"
            :value="item">
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- タグの選択 -->
      <el-col :span="6" :offset="1">タグ</el-col>
      <el-col :span="12">
        <el-select
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          :disabled="!model.image || disabled"
          @change="changeTag"
          @focus="getTags"
          :loading="listLoading"
          v-model="selectedTag">
          <el-option
            v-for="(item, index) in tags"
            :key="index"
            :label="item"
            :value="item">
          </el-option>
        </el-select>
      </el-col>
    </el-row>
  </div>
</template>

<script>
  import api from '@/api/v1/api'

  export default {
    props: ['value', 'disabled'],

    data () {
      return {
        // ContainerModel。emit用
        model: this.value === undefined || this.value === null
          ? {}
          : this.value,

        listLoading: false,
        registries: [], // レジストリ一覧 object配列
        images: [], // イメージ名一覧 string配列
        tags: [], // バージョン(タグ)名一覧 string配列

        selectedRegistryId: null, // 現在選択中のレジストリ
        selectedImage: null, // 現在選択中のイメージ名
        selectedTag: null // 現在選択中のバージョン
      }
    },

    async mounted () {
      if (this.value) {
        this.selectedRegistryId = this.value.registryId
        this.selectedImage = this.value.image
        this.selectedTag = this.value.tag
      }
      await this.getRegistries()
    },

    watch: {
      async value () {
        await this.getData()
      }
    },

    methods: {
      async getData () {
        this.model = this.value
        if (this.value) {
          if (this.selectedRegistryId !== this.value.registryId) {
            this.selectedRegistryId = this.value.registryId
            await this.getImages() // イメージ一覧の再取得
          }
          if (this.selectedImage !== this.value.image) {
            this.selectedImage = this.value.image

            this.getTags() // タグ一覧の再取得
          }
          if (this.selectedTag !== this.value.tag) {
            this.selectedTag = this.value.tag
          }
        }
      },

      async getRegistries () {
        if (this.registries && this.registries.length > 0) {
          // 一度取得していたら再取得はしない
          return
        }

        try {
          this.listLoading = true
          let [result] = api.f.data(await api.account.getRegistries())
          this.registries = result.registries
          if (this.selectedRegistryId === null) {
            this.selectedRegistryId = result.defaultRegistryId
            this.model.registryId = result.defaultRegistryId
          }
          this.getImages() // 新規・コピーどちらの場合も初期値は決まっているので、イメージを取ってくる
        } finally {
          this.listLoading = false
        }
      },

      async getImages () {
        if (this.images && this.images.length > 0) {
          // 一度取得していたら再取得はしない
          return
        }
        this.listLoading = true
        try {
          let [list] = api.f.data(await api.registry.getImages({registryId: this.selectedRegistryId}))
          this.images = list
        } catch (e) {
          this.$notify.error('Couldn\'t get')
        } finally {
          this.listLoading = false
        }
      },

      async getTags () {
        if (this.tags && this.tags.length > 0) {
          // 一度取得していたら再取得はしない
          return
        }

        this.listLoading = true
        try {
          let params = {
            registryId: this.selectedRegistryId,
            image: this.selectedImage
          }
          let [list] = api.f.data(await api.registry.getTags(params))
          this.tags = list
        } catch (e) {
          this.$notify.error('Couldn\'t get')
          console.log('Failed to get tags: ' + e)
        } finally {
          this.listLoading = false
        }
      },

      // @Emit()
      input (value) {
        this.$emit('input', value)
      },

      // イメージ以下の選択状態をリセットする
      resetImage () {
        this.images = []
        this.selectedImage = null
        this.model.image = null

        this.resetTag()
      },

      // タグの選択状態をリセットする
      resetTag () {
        this.tags = []
        this.selectedTag = null
        this.model.tag = null
      },

      // 選択しているレジストリが切り替わった時に呼ばれるイベントハンドラ。
      changeRegistry (registryId) {
        // レジストリが変わったので、イメージ以下の選択状態をリセットする
        this.resetImage()

        this.model.registryId = registryId
        if (registryId) { // バツボタンを押してレジストリを削除した場合はFalseになる
          this.input(this.model)
          this.getImages() // イメージ一覧の再取得
        }
      },

      // 選択しているイメージが切り替わった時に呼ばれるイベントハンドラ。
      changeImage (image) {
        // イメージが変わったので、バージョン（タグ）の選択状態をリセットする
        this.resetTag()

        this.model.image = image
        if (image) {
          this.input(this.model)
          this.getTags() // タグ一覧の再取得
        }
      },

      // 選択しているタグが切り替わった時に呼ばれるイベントハンドラ。
      changeTag (tag) {
        this.model.tag = tag
        this.input(this.model)
      }
    }
  }
</script>

<style lang="scss" scoped>
</style>
