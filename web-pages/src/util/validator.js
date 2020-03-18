export default class Validator {
  // KqiContainerImageSelectorで利用
  static containerImageValidator = (rule, value, callback) => {
    if (value.registry === null || value.image === null || value.tag === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiGitModelSelectorで利用
  static gitModelValidator = (rule, value, callback) => {
    if (value.git === null || value.repository === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiGitEndpointSelectorで利用
  static gitEndpointValidator = (rule, value, callback) => {
    if (value.selectedIds.length === 0 || value.defaultId === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiRegistryEndpointSelectorで利用
  static regystryEndpointValidator = (rule, value, callback) => {
    if (value.selectedIds.length === 0 || value.defaultId === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }
}
