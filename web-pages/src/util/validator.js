export default class Validator {
  static containerImageValidator = (rule, value, callback) => {
    if (value.registry === null || value.image === null || value.tag === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }
  static gitModelValidator = (rule, value, callback) => {
    if (
      value.git === null ||
      value.repository === null ||
      value.branch === null
    ) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }
}
