import enlang from 'element-ui/lib/locale/lang/en'
import jalang from 'element-ui/lib/locale/lang/ja'

// 各画面の文言定義は各画面で行う。
// ここでは element ui の言語翻訳がおかしいものの修正を行う。

let ja = {
  ...jalang,
}
ja.el.pagination.total = '合計 {total} 件' // 総計 → 合計に変更

let en = {
  ...enlang,
}

let message = {
  ja,
  en,
}

export { message as default }
