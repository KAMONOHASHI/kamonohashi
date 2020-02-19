<!--name: コンテナのイメージ名＆バージョンの入力コンポーネント,-->
<!--description: イメージ名、タグ名を指定するドロップダウンをそれぞれ表示する,-->

<template>
  <el-form-item label="コンテナイメージ">
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
          :value="registry"
          @change="changeRegistry"
        >
          <el-option
            v-for="item in registries"
            :key="item.id"
            :label="item.name"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- イメージの選択 -->
      <el-col :span="6" :offset="1">イメージ</el-col>
      <el-col :span="12">
        <el-select
          :value="image"
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          :disabled="!registry || disabled"
          automatic-dropdown
          @change="changeImage"
        >
          <el-option
            v-for="(item, index) in images"
            :key="index"
            :label="item"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>

    <el-row>
      <!-- タグの選択 -->
      <el-col :span="6" :offset="1">タグ</el-col>
      <el-col :span="12">
        <el-select
          :value="tag"
          size="small"
          filterable
          clearable
          default-first-option
          allow-create
          :disabled="!image || disabled"
          @change="changeTag"
        >
          <el-option
            v-for="(item, index) in tags"
            :key="index"
            :label="item"
            :value="item"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>
  </el-form-item>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('registrySelector')

export default {
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
  },

  computed: {
    ...mapGetters(['registries', 'registry', 'images', 'image', 'tags', 'tag']),
  },

  methods: {
    // 選択しているレジストリが切り替わった時に呼ばれるイベントハンドラ。
    changeRegistry(registry) {
      if (registry === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'registry', value: null })
      } else {
        this.$emit('input', { type: 'registry', value: registry })
      }
    },

    // 選択しているイメージが切り替わった時に呼ばれるイベントハンドラ。
    changeImage(image) {
      if (image === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'image', value: null })
      } else {
        this.$emit('input', { type: 'image', value: image })
      }
    },

    // 選択しているタグが切り替わった時に呼ばれるイベントハンドラ。
    changeTag(tag) {
      if (tag === '') {
        // clearボタンが押下された場合
        this.$emit('input', { type: 'tag', value: null })
      } else {
        this.$emit('input', { type: 'tag', value: tag })
      }
    },
  },
}
</script>

<style lang="scss" scoped></style>
