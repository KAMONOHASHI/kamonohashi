import { configure } from '@storybook/vue'

import Vue from 'vue'

function loadStories() {
  require('./../src/stories')
}

configure(loadStories, module)
