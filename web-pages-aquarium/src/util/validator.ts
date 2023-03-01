export default class Validator {
  // KqiContainerImageSelectorで利用

  static containerImageValidator = (
    rule: any,
    value: {
      registry: {
        id: number
        name: string
      } | null
      image: null | string
      tag: null | string
    },
    callback: Function,
  ) => {
    if (value.registry === null || value.image === null || value.tag === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiGitModelSelectorで利用

  static gitModelValidator = (
    rule: any,
    value: {
      git: {
        id: number
        name: string
      } | null
      repository: { name: string; owner: string } | string | null
    },
    callback: Function,
  ) => {
    if (value.git === null || value.repository === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiGitEndpointSelectorで利用

  static gitEndpointValidator = (
    rule: any,
    value: {
      selectedIds: Array<number>
      defaultId: null | number
    },
    callback: Function,
  ) => {
    if (value.selectedIds.length === 0 || value.defaultId === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }

  // KqiRegistryEndpointSelectorで利用

  static regystryEndpointValidator = (
    rule: any,
    value: {
      selectedIds: Array<number>
      defaultId: null | number
    },
    callback: Function,
  ) => {
    if (value.selectedIds.length === 0 || value.defaultId === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }
}
