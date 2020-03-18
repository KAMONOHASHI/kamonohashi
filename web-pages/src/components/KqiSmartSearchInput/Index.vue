<template>
  <span class="box" :class="mini ? 'box-mini' : 'box-small'">
    <span v-if="dynamicTags.length">
      <el-tag
        v-for="(tag, index) in dynamicTags"
        :key="index"
        closable
        :disable-transitions="false"
        :size="mini ? 'mini' : 'default'"
        @close="handleClose(tag)"
      >
        <el-button
          type="text"
          style="padding: 0px; margin: 0px; font-size: 12px;"
          @click="handleEdit(tag)"
        >
          <span class="tag-prefix">{{ tag.prefix }}</span>
          <span v-show="!tag.input" class="tag-value">{{ tag.display }}</span>
          <span v-show="!tag.input" class="tag-suffix">{{ tag.suffix }}</span>
        </el-button>

        <component
          :is="tag.component"
          v-show="tag.input"
          :tag="tag"
          @done="handleInputDone(tag, $event)"
          @cancel="handleInputCancel(tag)"
        />
      </el-tag>
    </span>

    <el-dropdown
      trigger="click"
      placement="bottom-start"
      :size="mini ? 'mini' : 'default'"
      @command="handlePlus"
    >
      <el-input class="space" :size="mini ? 'mini' : 'small'" readonly />
      <el-dropdown-menu slot="dropdown">
        <el-dropdown-item
          v-for="c in configs"
          :key="c.prop"
          :command="c"
          :disabled="c.disabled"
        >
          {{ c.name }}
        </el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
    <el-button
      class="search"
      :size="mini ? 'mini' : 'small'"
      icon="el-icon-search"
      @click="handleSearch"
    />
  </span>
</template>

<script>
import SmartSearchInputText from './InputText'
import SmartSearchInputNumber from './InputNumber'
import SmartSearchInputDate from './InputDate'
import SmartSearchInputSelect from './InputSelect'

export default {
  props: {
    value: {
      type: Object,
      default: () => ({}),
    },
    configs: {
      type: Array,
      default: () => [
        {
          prop: 'id',
          name: 'ID',
          type: 'number',
          disabled: false,
          option: { default: '1' },
        },
        {
          prop: 'name',
          name: '名前',
          type: 'text',
          disabled: false,
          option: { default: 'B' },
        },
        {
          prop: 'createdAt',
          name: '登録日時',
          type: 'date',
          disabled: false,
          option: { default: 'D' },
        },
        {
          prop: 'status',
          name: 'ステータス',
          type: 'select',
          disabled: false,
          option: { default: 'E' },
        },
      ],
    },
    mini: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      dynamicTags: [],
    }
  },
  created() {
    // 各画面で使用できる検索条件の設定
    this.configs.forEach(c => {
      c.disabled = false
    })
  },
  methods: {
    // 使用している検索条件の解除
    handleClose(tag) {
      this.handleInputCancel(tag)
    },

    // 検索条件の追加
    handlePlus(config) {
      let component = null
      if (config.type === 'text') component = SmartSearchInputText
      if (config.type === 'number') component = SmartSearchInputNumber
      if (config.type === 'date') component = SmartSearchInputDate
      if (config.type === 'select') component = SmartSearchInputSelect

      this.dynamicTags.push({
        component: component,
        config: config,
        prefix: config.name,
        value: '',
        suffix: '',
        input: true,
      })
    },

    // 検索に使用する条件の選択
    handleEdit(tag) {
      let component = null
      if (tag.config.type === 'text') component = SmartSearchInputText
      if (tag.config.type === 'number') component = SmartSearchInputNumber
      if (tag.config.type === 'date') component = SmartSearchInputDate
      if (tag.config.type === 'select') component = SmartSearchInputSelect

      tag.component = null
      this.$nextTick(() => {
        tag.component = component
        tag.input = true
      })
    },

    // 検索実行
    handleSearch() {
      this.emitSearch()
    },

    // 下位コンポーネントから'done'を受け取った場合、追加した検索条件を使用して再検索
    handleInputDone(tag, event) {
      tag.input = false
      tag.value = event.value
      tag.display = event.display
      tag.suffix = event.suffix
      if (tag.config.multiple) {
        // 複数選択可能か
        let currentValue = this.value[tag.config.prop] // 複数選択可能な項目は配列で管理
        if (!currentValue || currentValue.length === 0) {
          currentValue = [event.value]
        } else {
          currentValue = []
          for (let i = 0; i < this.dynamicTags.length; i++) {
            if (this.dynamicTags[i].config.prop === 'tag') {
              currentValue.push(this.dynamicTags[i].value)
            }
          }
        }
        this.value[tag.config.prop] = currentValue
      } else {
        this.value[tag.config.prop] = event.value
        tag.config.disabled = true
      }
      this.emitValue(this.value)
      this.emitSearch()
    },

    // 対象の検索条件を解除して再検索
    handleInputCancel(tag) {
      this.dynamicTags.splice(this.dynamicTags.indexOf(tag), 1)

      if (tag.config.multiple) {
        // 複数選択可能か
        let currentValue = this.value[tag.config.prop]
        currentValue.splice(currentValue.indexOf(tag.value), 1)
        if (currentValue.length > 0) {
          this.value[tag.config.prop] = currentValue
        } else {
          this.value[tag.config.prop] = null
        }
      } else {
        delete this.value[tag.config.prop]
        tag.config.disabled = false
      }
      this.emitValue(this.value)
      this.emitSearch()
    },

    emitSearch() {
      this.$emit('search')
    },
    emitValue(v) {
      this.$emit('input', v)
    },
  },
}
</script>

<style lang="scss" scoped>
.box {
  text-align: left;
  border: 1px solid #dddddd;
  border-radius: 5px;
  background-color: white;
}

.box-mini {
  padding: 5px 3px 7px 3px;
}

.box-small {
  padding: 9px 5px 12px 5px;
}

.space {
  width: 100px;

  /deep/ .el-input__inner {
    border: 0px !important;
  }
}

.search {
  border: 0px !important;
}

.tag-prefix,
.tag-suffix {
  color: gray;
  opacity: 0.7;
}
</style>

<style scoped>
/* 公式の設定 */
.el-tag + .el-tag {
  margin-left: 10px;
}

.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}

.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>
