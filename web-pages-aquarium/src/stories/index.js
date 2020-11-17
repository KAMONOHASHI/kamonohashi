import { storiesOf } from '@storybook/vue'
import Vue from 'vue'
import Element from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

Vue.use(Element)

import KqiDownloadButton from './../components/KqiDownloadButton.vue'

storiesOf('Button', module).add('download', () => ({
  components: { KqiDownloadButton },
  template: `        <kqi-download-button
  :download-url="url"
  :file-name="fileName"
>test</kqi-download-button>`,
}))
