export default class Validator {
  static containerImageValidator = (rule, value, callback) => {
    if (value.registry === null || value.image === null || value.tag === null) {
      callback(new Error('必須項目です'))
    } else {
      callback()
    }
  }
}
