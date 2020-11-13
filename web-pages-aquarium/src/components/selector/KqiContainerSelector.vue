<!--name: コンテナのイメージ名＆バージョンの入力コンポーネント,-->
<!--description: イメージ名、タグ名を指定するドロップダウンをそれぞれ表示する,-->

<template>
  <el-form-item label="コンテナイメージ" prop="containerImage">
    <el-row></el-row>
    <el-row>
      <!-- レジストリの選択 -->
      <el-col :span="6" :offset="1">レジストリ</el-col>
      <el-col :span="12">
        <el-select
          size="small"
          filterable
          value-key="id"
          clearable
          remote
          :disabled="disabled"
          :value="value.registry"
          @change="changeRegistry"
        >
          <el-option
            v-for="item in registries"
            :key="item.id"
            :label="item.name"
            :value="item"
          />
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- イメージの選択 -->
      <el-col :span="6" :offset="1">イメージ</el-col>
      <el-col :span="12">
        <el-select
          :value="value.image"
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          :disabled="!value.registry || disabled"
          automatic-dropdown
          @change="changeImage"
        >
          <el-option
            v-for="(item, index) in images"
            :key="index"
            :label="item"
            :value="item"
          />
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- タグの選択 -->
      <el-col :span="6" :offset="1">タグ</el-col>
      <el-col :span="12">
        <el-select
          :value="value.tag"
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          :disabled="!value.image || disabled"
          @change="changeTag"
        >
          <el-option
            v-for="(item, index) in tags"
            :key="index"
            :label="item"
            :value="item"
          />
        </el-select>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
export default {
  props: {
    // レジストリ一覧
    registries: {
      type: Array,
      default: () => {
        return []
      },
    },
    // イメージ一覧
    images: {
      type: Array,
      default: () => {
        return []
      },
    },
    // タグ一覧
    tags: {
      type: Array,
      default: () => {
        return []
      },
    },
    // 選択されたレジストリ、イメージ、タグをvalueで保持
    value: {
      type: Object,
      default: () => {
        return {
          registry: null,
          image: null,
          tag: null,
        }
      },
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },

  methods: {
    // 選択しているレジストリが切り替わった時に呼ばれるイベントハンドラ。
    changeRegistry(registry) {
      let containerImage = this.value
      if (registry === '') {
        // clearボタンが押下された場合
        containerImage.registry = null
      } else {
        containerImage.registry = registry
      }
      this.$emit('input', containerImage)
      this.$emit('selectRegistry', registry === '' ? null : registry.id)
    },

    // 選択しているイメージが切り替わった時に呼ばれるイベントハンドラ。
    changeImage(image) {
      let containerImage = this.value

      if (image === '') {
        // clearボタンが押下された場合
        containerImage.image = null
      } else {
        containerImage.image = image
      }
      this.$emit('input', containerImage)
      this.$emit('selectImage', image === '' ? null : image)
    },

    // 選択しているタグが切り替わった時に呼ばれるイベントハンドラ。
    changeTag(tag) {
      let containerImage = this.value

      if (tag === '') {
        // clearボタンが押下された場合
        containerImage.tag = null
      } else {
        containerImage.tag = tag
      }
      this.$emit('input', containerImage)
    },
  },
}
</script>

<style lang="scss" scoped></style>
