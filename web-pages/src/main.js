// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import router from './router'
import store from './store'
import componentExt from './util/component-ext'
import VueI18n from 'vue-i18n'
import lineClamp from 'vue-line-clamp'
import VueClipboard from 'vue-clipboard2'
import message from './message/index'
import Icon from 'vue-awesome/components/Icon'
import './icon'

Vue.config.productionTip = false

Vue.use(VueI18n)
const i18n = new VueI18n({ locale: 'ja', messages: message })

Vue.use(ElementUI, { i18n: (key, value) => i18n.t(key, value) })
Vue.use(lineClamp, {})
Vue.use(VueClipboard)

Vue.mixin(componentExt)

Vue.component('icon', Icon)

new Vue({
  store,
  el: '#app',
  router,
  i18n,
  components: { App },
  template: '<App/>',
})
