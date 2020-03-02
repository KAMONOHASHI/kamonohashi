<template>
  <span>
    <el-select
      class="selectTenant"
      :value="value.roleIds"
      multiple
      placeholder="Select"
      @change="handleChange"
    >
      <template v-for="item in availableRoles">
        <el-option
          v-if="item.isSystemRole === showSystem"
          :key="item.id"
          :label="item.displayName"
          :value="item.id"
        >
        </el-option>
      </template>
    </el-select>
  </span>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
const { mapGetters } = createNamespacedHelpers('role')

export default {
  props: {
    value: {
      type: Object,
      default: () => {
        return {
          roleIds: [],
          system: false,
          tenant: false,
        }
      },
    },
  },
  data() {
    return {
      selectedRoleIds: [],
      showSystem: false,
    }
  },
  computed: {
    ...mapGetters(['roles', 'tenantRoles']),
    availableRoles: function() {
      if (this.value.tenant) {
        return this.selectedRoleIds
      } else {
        this.showRoles()
        return this.roles
      }
    },
  },
  methods: {
    async showRoles() {
      if (!this.value.tenant) {
        this.showSystem = true
      }
    },
    async handleChange(v) {
      let updateValue = this.value
      updateValue.roleIds = v
      if (v === '') {
        this.$emit('input', null)
      } else {
        this.$emit('input', updateValue)
      }
    },
  },
}
</script>

<style scoped>
.selectTenant {
  width: 100%;
}
</style>
