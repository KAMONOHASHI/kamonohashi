// Utilityメソッドをまとめた静的クラス メソッド個別にImportするのは面倒であるためクラス化
export default class Util {
  // 指定した秒数Sleepする
  static wait(second?: number) {
    return new Promise(resolve => {
      setTimeout(resolve, second)
    })
  }

  static getByteString(byte: number) {
    let suffix = ''
    let val: number | string = byte
    if (val > 1024 * 1024) {
      val = val / (1024 * 1024)
      suffix = ' MB'
    } else if (val > 1024) {
      val = val / 1024
      suffix = ' KB'
    } else {
      suffix = ' Byte'
    }
    val = val.toFixed(1) // 小数点１位まで残す
    val = val.toString().replace(/(\d)(?=(\d{3})+(\.\d+))/g, '$1,') // カンマ区切り
    return val + suffix
  }

  /**
   * 指定した数値がフィルタ条件に合致していたらtrueを返す
   * @param {*} target  比較対象の数値
   * @param {*} filter フィルタ条件
   */
  static isMatchAsNumber(target: number, filter: string) {
    if (filter) {
      let symbol = filter.substring(0, 1)
      let strValue = filter.slice(1)
      if (Util.startWith(strValue, '=')) {
        symbol += '='
        strValue = strValue.slice(1)
      }
      let value = Number(strValue)
      switch (symbol) {
        case '=':
          return target === value
        case '>=':
          return target >= value
        case '<=':
          return target <= value
        case '>':
          return target > value
        case '<':
          return target < value
      }
    }
    return true
  }

  /**
   * 指定した日時がフィルタ条件に合致していたらtrueを返す
   * @param {*} target  比較対象の日時の文字列
   * @param {*} filter フィルタ条件
   */
  static isMatchAsDate(target: string, filter: string) {
    if (filter) {
      let symbol = filter.substring(0, 1)
      let strValue = filter.slice(1)
      if (Util.startWith(strValue, '=')) {
        symbol += '='
        strValue = strValue.slice(1)
      }
      let value = new Date(strValue)
      let targetDate = new Date(target)
      switch (symbol) {
        case '=':
          return targetDate.toDateString() === value.toDateString()
        case '>=':
          return targetDate > value
        case '<':
          return targetDate < value
      }
    }
    return true
  }

  /**
   * 指定した文字列がフィルタ条件に合致していたらtrueを返す
   * @param {*} target  比較対象の文字列
   * @param {*} filter フィルタ条件
   */
  static isMatchAsText(target: string, filter: string) {
    if (filter) {
      if (Util.startWith(filter, '!')) {
        return !target || target.includes(filter.slice(1)) === false
      } else {
        return target && target.includes(filter)
      }
    }
    return true
  }

  /**
   * 指定した文字列配列がフィルタ条件群にすべて合致していたらtrueを返す
   * @param {*} target  比較対象の文字列配列
   * @param {*} filter フィルタ条件配列
   */
  static isMatchAsTextArrayByFilters(
    target: Array<string>,
    filters: Array<string>,
  ) {
    if (filters && filters.length > 0) {
      return filters.every((f: string) => Util.isMatchAsTextArray(target, f))
    }
    return true
  }

  /**
   * 指定した文字列配列がフィルタ条件に合致していたらtrueを返す
   * @param {*} target  比較対象の文字列配列
   * @param {*} filter フィルタ条件
   */
  static isMatchAsTextArray(target: Array<string>, filter: string) {
    if (filter) {
      if (Util.startWith(filter, '!')) {
        if (!target || target.length === 0) {
          return true
        }
        let value = filter.slice(1)
        return target.some(element => element.includes(value)) === false
      } else {
        if (!target || target.length === 0) {
          return false
        }
        return target.some(element => element.includes(filter))
      }
    }
    return true
  }

  static startWith(text: string, prefix: string) {
    return text.lastIndexOf(prefix, 0) === 0
  }

  static getCookie(key: string) {
    var result = null
    var cookieName = key + '='
    var allcookies = document.cookie

    var position = allcookies.indexOf(cookieName)
    if (position !== -1) {
      var startIndex = position + cookieName.length
      var endIndex = allcookies.indexOf(';', startIndex)
      if (endIndex === -1) {
        endIndex = allcookies.length
      }

      result = decodeURIComponent(allcookies.substring(startIndex, endIndex))
    }
    return result
  }

  static setCookie(key: string, val: string | boolean | null | undefined) {
    document.cookie = `${key}=${val}`
  }

  static deleteCookie(key: string) {
    document.cookie = `${key}=; max-age=0`
  }
}
