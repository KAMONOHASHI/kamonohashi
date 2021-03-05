import Icon from 'vue-awesome/components/Icon'
import 'vue-awesome/icons/fish'
import 'vue-awesome/icons/images'
import 'vue-awesome/icons/flask'
import 'vue-awesome/icons/home'

Icon.register({
  'aq-dashboard': Icon.icons['home'],
  'aq-template': Icon.icons['fish'],
  'aq-dataset': Icon.icons['images'],
  'aq-experiment': Icon.icons['flask'],
})
