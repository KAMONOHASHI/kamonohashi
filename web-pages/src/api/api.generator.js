/* eslint-disable */
import net from 'axios'
import qs from 'qs'
let domain = ''
let axios = net.create()
export const getAxios = () => {
  return axios
}
export const setAxios = ($axios) => {
  axios = $axios
}
export const getDomain = () => {
  return domain
}
export const setDomain = ($domain) => {
  domain = $domain
}
export const request = (method, url, body, queryParameters, form, config) => {
  method = method.toLowerCase()
  let keys = Object.keys(queryParameters)
  let queryUrl = url
  if (keys.length > 0) {
    queryUrl = url + '?' + qs.stringify(queryParameters)
  }
  // let queryUrl = url+(keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
  if (body) {
    return axios[method](queryUrl, body, config)
  } else if (method === 'get') {
    let p = config || {}
    p.params = form
    return axios[method](queryUrl, p)
  } else {
    return axios[method](queryUrl, qs.stringify(form), config)
  }
}
/*==========================================================
 *                    A platform for deep learning
 ==========================================================*/
/**
 * ログインユーザのアカウント情報を取得する
 * request: getApiV2Account
 * url: getApiV2AccountURL
 * method: getApiV2Account_TYPE
 * raw_url: getApiV2Account_RAW_URL
 */
export const getApiV2Account = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Account_RAW_URL = function() {
  return '/api/v2/account'
}
export const getApiV2Account_TYPE = function() {
  return 'get'
}
export const getApiV2AccountURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ログインユーザのアカウント情報を変更する
 * request: putApiV2Account
 * url: putApiV2AccountURL
 * method: putApiV2Account_TYPE
 * raw_url: putApiV2Account_RAW_URL
 * @param defaultTenant - デフォルトテナント
 */
export const putApiV2Account = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['defaultTenant'] !== undefined) {
    queryParameters['DefaultTenant'] = parameters['defaultTenant']
  }
  if (parameters['defaultTenant'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: defaultTenant'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2Account_RAW_URL = function() {
  return '/api/v2/account'
}
export const putApiV2Account_TYPE = function() {
  return 'put'
}
export const putApiV2AccountURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account'
  if (parameters['defaultTenant'] !== undefined) {
    queryParameters['DefaultTenant'] = parameters['defaultTenant']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ログインユーザのパスワードを変更する
 * request: putApiV2AccountPassword
 * url: putApiV2AccountPasswordURL
 * method: putApiV2AccountPassword_TYPE
 * raw_url: putApiV2AccountPassword_RAW_URL
 * @param body - 
 */
export const putApiV2AccountPassword = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/password'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AccountPassword_RAW_URL = function() {
  return '/api/v2/account/password'
}
export const putApiV2AccountPassword_TYPE = function() {
  return 'put'
}
export const putApiV2AccountPasswordURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/password'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ログインする
 * request: postApiV2AccountLogin
 * url: postApiV2AccountLoginURL
 * method: postApiV2AccountLogin_TYPE
 * raw_url: postApiV2AccountLogin_RAW_URL
 * @param body - 
 */
export const postApiV2AccountLogin = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/login'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AccountLogin_RAW_URL = function() {
  return '/api/v2/account/login'
}
export const postApiV2AccountLogin_TYPE = function() {
  return 'post'
}
export const postApiV2AccountLoginURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/login'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 現在の認証情報を使用し、新規にアクセストークンを取得する
 * request: postApiV2AccountTenantsByTenantIdToken
 * url: postApiV2AccountTenantsByTenantIdTokenURL
 * method: postApiV2AccountTenantsByTenantIdToken_TYPE
 * raw_url: postApiV2AccountTenantsByTenantIdToken_RAW_URL
 * @param tenantId - テナントID
 * @param body - テナント切替用入力モデル
 */
export const postApiV2AccountTenantsByTenantIdToken = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/tenants/{tenantId}/token'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AccountTenantsByTenantIdToken_RAW_URL = function() {
  return '/api/v2/account/tenants/{tenantId}/token'
}
export const postApiV2AccountTenantsByTenantIdToken_TYPE = function() {
  return 'post'
}
export const postApiV2AccountTenantsByTenantIdTokenURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/tenants/{tenantId}/token'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクセス可能なKQIのメニュー一覧をツリー形式で取得する。
 * request: getApiV2AccountMenusTree
 * url: getApiV2AccountMenusTreeURL
 * method: getApiV2AccountMenusTree_TYPE
 * raw_url: getApiV2AccountMenusTree_RAW_URL
 * @param lang - 
 */
export const getApiV2AccountMenusTree = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/menus/tree'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountMenusTree_RAW_URL = function() {
  return '/api/v2/account/menus/tree'
}
export const getApiV2AccountMenusTree_TYPE = function() {
  return 'get'
}
export const getApiV2AccountMenusTreeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/menus/tree'
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクセス可能なKQIのメニュー一覧をリスト形式で取得する。
 * request: getApiV2AccountMenusList
 * url: getApiV2AccountMenusListURL
 * method: getApiV2AccountMenusList_TYPE
 * raw_url: getApiV2AccountMenusList_RAW_URL
 * @param lang - 
 */
export const getApiV2AccountMenusList = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/menus/list'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountMenusList_RAW_URL = function() {
  return '/api/v2/account/menus/list'
}
export const getApiV2AccountMenusList_TYPE = function() {
  return 'get'
}
export const getApiV2AccountMenusListURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/menus/list'
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクセス可能なAquariumのメニュー一覧をツリー形式で取得する。
 * request: getApiV2AccountAquariumMenusTree
 * url: getApiV2AccountAquariumMenusTreeURL
 * method: getApiV2AccountAquariumMenusTree_TYPE
 * raw_url: getApiV2AccountAquariumMenusTree_RAW_URL
 * @param lang - 
 */
export const getApiV2AccountAquariumMenusTree = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/aquarium/menus/tree'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountAquariumMenusTree_RAW_URL = function() {
  return '/api/v2/account/aquarium/menus/tree'
}
export const getApiV2AccountAquariumMenusTree_TYPE = function() {
  return 'get'
}
export const getApiV2AccountAquariumMenusTreeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/aquarium/menus/tree'
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクセス可能なAquariumのメニュー一覧をリスト形式で取得する。
 * request: getApiV2AccountAquariumMenusList
 * url: getApiV2AccountAquariumMenusListURL
 * method: getApiV2AccountAquariumMenusList_TYPE
 * raw_url: getApiV2AccountAquariumMenusList_RAW_URL
 * @param lang - 
 */
export const getApiV2AccountAquariumMenusList = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/aquarium/menus/list'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountAquariumMenusList_RAW_URL = function() {
  return '/api/v2/account/aquarium/menus/list'
}
export const getApiV2AccountAquariumMenusList_TYPE = function() {
  return 'get'
}
export const getApiV2AccountAquariumMenusListURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/aquarium/menus/list'
  if (parameters['lang'] !== undefined) {
    queryParameters['lang'] = parameters['lang']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントにおけるGit情報を取得する
 * request: getApiV2AccountGits
 * url: getApiV2AccountGitsURL
 * method: getApiV2AccountGits_TYPE
 * raw_url: getApiV2AccountGits_RAW_URL
 */
export const getApiV2AccountGits = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/gits'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountGits_RAW_URL = function() {
  return '/api/v2/account/gits'
}
export const getApiV2AccountGits_TYPE = function() {
  return 'get'
}
export const getApiV2AccountGitsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/gits'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントにおけるGitのトークン情報を更新する。
 * request: putApiV2AccountGits
 * url: putApiV2AccountGitsURL
 * method: putApiV2AccountGits_TYPE
 * raw_url: putApiV2AccountGits_RAW_URL
 * @param body - 更新内容
 */
export const putApiV2AccountGits = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/gits'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AccountGits_RAW_URL = function() {
  return '/api/v2/account/gits'
}
export const putApiV2AccountGits_TYPE = function() {
  return 'put'
}
export const putApiV2AccountGitsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/gits'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントにおけるレジストリ情報を取得する
 * request: getApiV2AccountRegistries
 * url: getApiV2AccountRegistriesURL
 * method: getApiV2AccountRegistries_TYPE
 * raw_url: getApiV2AccountRegistries_RAW_URL
 */
export const getApiV2AccountRegistries = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/registries'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountRegistries_RAW_URL = function() {
  return '/api/v2/account/registries'
}
export const getApiV2AccountRegistries_TYPE = function() {
  return 'get'
}
export const getApiV2AccountRegistriesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/registries'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントにおけるレジストリのトークン情報を更新する。
 * request: putApiV2AccountRegistries
 * url: putApiV2AccountRegistriesURL
 * method: putApiV2AccountRegistries_TYPE
 * raw_url: putApiV2AccountRegistries_RAW_URL
 * @param body - 更新内容
 */
export const putApiV2AccountRegistries = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/registries'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AccountRegistries_RAW_URL = function() {
  return '/api/v2/account/registries'
}
export const putApiV2AccountRegistries_TYPE = function() {
  return 'put'
}
export const putApiV2AccountRegistriesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/registries'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * WebHook情報を取得する
 * request: getApiV2AccountWebhookSlack
 * url: getApiV2AccountWebhookSlackURL
 * method: getApiV2AccountWebhookSlack_TYPE
 * raw_url: getApiV2AccountWebhookSlack_RAW_URL
 */
export const getApiV2AccountWebhookSlack = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/webhook/slack'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AccountWebhookSlack_RAW_URL = function() {
  return '/api/v2/account/webhook/slack'
}
export const getApiV2AccountWebhookSlack_TYPE = function() {
  return 'get'
}
export const getApiV2AccountWebhookSlackURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/webhook/slack'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * WebHook情報を更新する
 * request: putApiV2AccountWebhookSlack
 * url: putApiV2AccountWebhookSlackURL
 * method: putApiV2AccountWebhookSlack_TYPE
 * raw_url: putApiV2AccountWebhookSlack_RAW_URL
 * @param body - Webhook情報モデル
 */
export const putApiV2AccountWebhookSlack = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/webhook/slack'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AccountWebhookSlack_RAW_URL = function() {
  return '/api/v2/account/webhook/slack'
}
export const putApiV2AccountWebhookSlack_TYPE = function() {
  return 'put'
}
export const putApiV2AccountWebhookSlackURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/webhook/slack'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テスト通知を送信する
 * request: postApiV2AccountWebhookSlackTest
 * url: postApiV2AccountWebhookSlackTestURL
 * method: postApiV2AccountWebhookSlackTest_TYPE
 * raw_url: postApiV2AccountWebhookSlackTest_RAW_URL
 * @param body - Webhook情報モデル
 */
export const postApiV2AccountWebhookSlackTest = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/webhook/slack/test'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AccountWebhookSlackTest_RAW_URL = function() {
  return '/api/v2/account/webhook/slack/test'
}
export const postApiV2AccountWebhookSlackTest_TYPE = function() {
  return 'post'
}
export const postApiV2AccountWebhookSlackTestURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/webhook/slack/test'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットを作成する
 * request: postApiV2AquariumDatasets
 * url: postApiV2AquariumDatasetsURL
 * method: postApiV2AquariumDatasets_TYPE
 * raw_url: postApiV2AquariumDatasets_RAW_URL
 * @param body - アクアリウムデータセット
 */
export const postApiV2AquariumDatasets = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AquariumDatasets_RAW_URL = function() {
  return '/api/v2/aquarium/datasets'
}
export const postApiV2AquariumDatasets_TYPE = function() {
  return 'post'
}
export const postApiV2AquariumDatasetsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、アクアリウムデータセット一覧を取得する
* request: getApiV2AquariumDatasets
* url: getApiV2AquariumDatasetsURL
* method: getApiV2AquariumDatasets_TYPE
* raw_url: getApiV2AquariumDatasets_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param createdAt - 作成時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param createdBy - 作成者
     * @param modifiedAt - 更新時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param modifiedBy - 更新者
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2AquariumDatasets = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['modifiedAt'] !== undefined) {
    queryParameters['ModifiedAt'] = parameters['modifiedAt']
  }
  if (parameters['modifiedBy'] !== undefined) {
    queryParameters['ModifiedBy'] = parameters['modifiedBy']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AquariumDatasets_RAW_URL = function() {
  return '/api/v2/aquarium/datasets'
}
export const getApiV2AquariumDatasets_TYPE = function() {
  return 'get'
}
export const getApiV2AquariumDatasetsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['modifiedAt'] !== undefined) {
    queryParameters['ModifiedAt'] = parameters['modifiedAt']
  }
  if (parameters['modifiedBy'] !== undefined) {
    queryParameters['ModifiedBy'] = parameters['modifiedBy']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットバージョンを作成する
 * request: postApiV2AquariumDatasetsByIdVersions
 * url: postApiV2AquariumDatasetsByIdVersionsURL
 * method: postApiV2AquariumDatasetsByIdVersions_TYPE
 * raw_url: postApiV2AquariumDatasetsByIdVersions_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param body - アクアリウムデータセットバージョン
 */
export const postApiV2AquariumDatasetsByIdVersions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets/{id}/versions'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AquariumDatasetsByIdVersions_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions'
}
export const postApiV2AquariumDatasetsByIdVersions_TYPE = function() {
  return 'post'
}
export const postApiV2AquariumDatasetsByIdVersionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets/{id}/versions'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットバージョン一覧を取得する
 * request: getApiV2AquariumDatasetsByIdVersions
 * url: getApiV2AquariumDatasetsByIdVersionsURL
 * method: getApiV2AquariumDatasetsByIdVersions_TYPE
 * raw_url: getApiV2AquariumDatasetsByIdVersions_RAW_URL
 * @param id - アクアリウムデータセットID
 */
export const getApiV2AquariumDatasetsByIdVersions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets/{id}/versions'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AquariumDatasetsByIdVersions_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions'
}
export const getApiV2AquariumDatasetsByIdVersions_TYPE = function() {
  return 'get'
}
export const getApiV2AquariumDatasetsByIdVersionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets/{id}/versions'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットバージョンを取得する
 * request: getApiV2AquariumDatasetsByIdVersionsByVersionId
 * url: getApiV2AquariumDatasetsByIdVersionsByVersionIdURL
 * method: getApiV2AquariumDatasetsByIdVersionsByVersionId_TYPE
 * raw_url: getApiV2AquariumDatasetsByIdVersionsByVersionId_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param versionId - アクアリウムデータセットバージョンID
 */
export const getApiV2AquariumDatasetsByIdVersionsByVersionId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters['versionId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: versionId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AquariumDatasetsByIdVersionsByVersionId_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
}
export const getApiV2AquariumDatasetsByIdVersionsByVersionId_TYPE = function() {
  return 'get'
}
export const getApiV2AquariumDatasetsByIdVersionsByVersionIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットバージョンを削除する
 * request: deleteApiV2AquariumDatasetsByIdVersionsByVersionId
 * url: deleteApiV2AquariumDatasetsByIdVersionsByVersionIdURL
 * method: deleteApiV2AquariumDatasetsByIdVersionsByVersionId_TYPE
 * raw_url: deleteApiV2AquariumDatasetsByIdVersionsByVersionId_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param versionId - アクアリウムデータセットバージョンID
 */
export const deleteApiV2AquariumDatasetsByIdVersionsByVersionId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters['versionId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: versionId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AquariumDatasetsByIdVersionsByVersionId_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
}
export const deleteApiV2AquariumDatasetsByIdVersionsByVersionId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AquariumDatasetsByIdVersionsByVersionIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウムデータセットを削除する
 * request: deleteApiV2AquariumDatasetsById
 * url: deleteApiV2AquariumDatasetsByIdURL
 * method: deleteApiV2AquariumDatasetsById_TYPE
 * raw_url: deleteApiV2AquariumDatasetsById_RAW_URL
 * @param id - アクアリウムデータセットID
 */
export const deleteApiV2AquariumDatasetsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AquariumDatasetsById_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}'
}
export const deleteApiV2AquariumDatasetsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AquariumDatasetsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/aquarium/datasets/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントに有効なパーティションの一覧を取得する。
 * request: getApiV2TenantPartitions
 * url: getApiV2TenantPartitionsURL
 * method: getApiV2TenantPartitions_TYPE
 * raw_url: getApiV2TenantPartitions_RAW_URL
 */
export const getApiV2TenantPartitions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/partitions'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantPartitions_RAW_URL = function() {
  return '/api/v2/tenant/partitions'
}
export const getApiV2TenantPartitions_TYPE = function() {
  return 'get'
}
export const getApiV2TenantPartitionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/partitions'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * パーティションの一覧を取得する。
 * request: getApiV2AdminPartitions
 * url: getApiV2AdminPartitionsURL
 * method: getApiV2AdminPartitions_TYPE
 * raw_url: getApiV2AdminPartitions_RAW_URL
 */
export const getApiV2AdminPartitions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/partitions'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminPartitions_RAW_URL = function() {
  return '/api/v2/admin/partitions'
}
export const getApiV2AdminPartitions_TYPE = function() {
  return 'get'
}
export const getApiV2AdminPartitionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/partitions'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * クォータ設定を取得する。
 * request: getApiV2AdminQuotas
 * url: getApiV2AdminQuotasURL
 * method: getApiV2AdminQuotas_TYPE
 * raw_url: getApiV2AdminQuotas_RAW_URL
 */
export const getApiV2AdminQuotas = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/quotas'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminQuotas_RAW_URL = function() {
  return '/api/v2/admin/quotas'
}
export const getApiV2AdminQuotas_TYPE = function() {
  return 'get'
}
export const getApiV2AdminQuotasURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/quotas'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 0が指定された場合、上限なしを示す。また、指定のなかったテナントは更新しない。
 * request: postApiV2AdminQuotas
 * url: postApiV2AdminQuotasURL
 * method: postApiV2AdminQuotas_TYPE
 * raw_url: postApiV2AdminQuotas_RAW_URL
 * @param body - 
 */
export const postApiV2AdminQuotas = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/quotas'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminQuotas_RAW_URL = function() {
  return '/api/v2/admin/quotas'
}
export const postApiV2AdminQuotas_TYPE = function() {
  return 'post'
}
export const postApiV2AdminQuotasURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/quotas'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中テナントのクォータ情報を取得
 * request: getApiV2TenantQuota
 * url: getApiV2TenantQuotaURL
 * method: getApiV2TenantQuota_TYPE
 * raw_url: getApiV2TenantQuota_RAW_URL
 */
export const getApiV2TenantQuota = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/quota'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantQuota_RAW_URL = function() {
  return '/api/v2/tenant/quota'
}
export const getApiV2TenantQuota_TYPE = function() {
  return 'get'
}
export const getApiV2TenantQuotaURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/quota'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントが利用可能なノード一覧（リソース値を含む）を取得する。
 * request: getApiV2TenantNodes
 * url: getApiV2TenantNodesURL
 * method: getApiV2TenantNodes_TYPE
 * raw_url: getApiV2TenantNodes_RAW_URL
 */
export const getApiV2TenantNodes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantNodes_RAW_URL = function() {
  return '/api/v2/tenant/nodes'
}
export const getApiV2TenantNodes_TYPE = function() {
  return 'get'
}
export const getApiV2TenantNodesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/nodes'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * REST APIとして定時バッチから実行される想定。
 * request: deleteApiV2AdminTensorboards
 * url: deleteApiV2AdminTensorboardsURL
 * method: deleteApiV2AdminTensorboards_TYPE
 * raw_url: deleteApiV2AdminTensorboards_RAW_URL
 */
export const deleteApiV2AdminTensorboards = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tensorboards'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminTensorboards_RAW_URL = function() {
  return '/api/v2/admin/tensorboards'
}
export const deleteApiV2AdminTensorboards_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminTensorboardsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tensorboards'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * イベントを取得する
 * request: getApiV2AdminEventsById
 * url: getApiV2AdminEventsByIdURL
 * method: getApiV2AdminEventsById_TYPE
 * raw_url: getApiV2AdminEventsById_RAW_URL
 * @param id - テナントID
 * @param name - コンテナ名
 */
export const getApiV2AdminEventsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/events/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['name'] !== undefined) {
    queryParameters['name'] = parameters['name']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminEventsById_RAW_URL = function() {
  return '/api/v2/admin/events/{id}'
}
export const getApiV2AdminEventsById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminEventsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/events/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['name'] !== undefined) {
    queryParameters['name'] = parameters['name']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、データ一覧を取得。
タグ情報が含まれる。
* request: getApiV2Data
* url: getApiV2DataURL
* method: getApiV2Data_TYPE
* raw_url: getApiV2Data_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param memo - メモ
     * @param createdAt - 作成時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param createdBy - 作成者
     * @param tag - タグ。
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Data = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['tag'] !== undefined) {
    queryParameters['tag'] = parameters['tag']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Data_RAW_URL = function() {
  return '/api/v2/data'
}
export const getApiV2Data_TYPE = function() {
  return 'get'
}
export const getApiV2DataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['tag'] !== undefined) {
    queryParameters['tag'] = parameters['tag']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * データの新規作成
 * request: postApiV2Data
 * url: postApiV2DataURL
 * method: postApiV2Data_TYPE
 * raw_url: postApiV2Data_RAW_URL
 * @param body - 
 */
export const postApiV2Data = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2Data_RAW_URL = function() {
  return '/api/v2/data'
}
export const postApiV2Data_TYPE = function() {
  return 'post'
}
export const postApiV2DataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIDのデータ詳細情報を取得する。
 * request: getApiV2DataById
 * url: getApiV2DataByIdURL
 * method: getApiV2DataById_TYPE
 * raw_url: getApiV2DataById_RAW_URL
 * @param id - データID
 */
export const getApiV2DataById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DataById_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const getApiV2DataById_TYPE = function() {
  return 'get'
}
export const getApiV2DataByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* データ編集メソッド。
ファイルの追加は別のメソッドで行う。
* request: putApiV2DataById
* url: putApiV2DataByIdURL
* method: putApiV2DataById_TYPE
* raw_url: putApiV2DataById_RAW_URL
     * @param id - 変更対象のデータID
     * @param body - 変更内容
*/
export const putApiV2DataById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2DataById_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const putApiV2DataById_TYPE = function() {
  return 'put'
}
export const putApiV2DataByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したデータを削除する
 * request: deleteApiV2DataById
 * url: deleteApiV2DataByIdURL
 * method: deleteApiV2DataById_TYPE
 * raw_url: deleteApiV2DataById_RAW_URL
 * @param id - データID
 */
export const deleteApiV2DataById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2DataById_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const deleteApiV2DataById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2DataByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したデータに対してタグを追加する。
 * request: putApiV2DataByIdTagsByTag
 * url: putApiV2DataByIdTagsByTagURL
 * method: putApiV2DataByIdTagsByTag_TYPE
 * raw_url: putApiV2DataByIdTagsByTag_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const putApiV2DataByIdTagsByTag = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/tags/{tag}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{tag}', `${parameters['tag']}`)
  if (parameters['tag'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tag'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2DataByIdTagsByTag_RAW_URL = function() {
  return '/api/v2/data/{id}/tags/{tag}'
}
export const putApiV2DataByIdTagsByTag_TYPE = function() {
  return 'put'
}
export const putApiV2DataByIdTagsByTagURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/tags/{tag}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{tag}', `${parameters['tag']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したデータからタグを削除する。
 * request: deleteApiV2DataByIdTagsByTag
 * url: deleteApiV2DataByIdTagsByTagURL
 * method: deleteApiV2DataByIdTagsByTag_TYPE
 * raw_url: deleteApiV2DataByIdTagsByTag_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const deleteApiV2DataByIdTagsByTag = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/tags/{tag}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{tag}', `${parameters['tag']}`)
  if (parameters['tag'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tag'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2DataByIdTagsByTag_RAW_URL = function() {
  return '/api/v2/data/{id}/tags/{tag}'
}
export const deleteApiV2DataByIdTagsByTag_TYPE = function() {
  return 'delete'
}
export const deleteApiV2DataByIdTagsByTagURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/tags/{tag}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{tag}', `${parameters['tag']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ファイルのダウンロードURLを取得する
 * request: getApiV2DataByIdFilesByName
 * url: getApiV2DataByIdFilesByNameURL
 * method: getApiV2DataByIdFilesByName_TYPE
 * raw_url: getApiV2DataByIdFilesByName_RAW_URL
 * @param id - 対象データID
 * @param name - 対象ファイル名
 */
export const getApiV2DataByIdFilesByName = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/files/{name}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DataByIdFilesByName_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{name}'
}
export const getApiV2DataByIdFilesByName_TYPE = function() {
  return 'get'
}
export const getApiV2DataByIdFilesByNameURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/files/{name}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ファイルのサイズ(Byte)を取得する
 * request: getApiV2DataByIdFilesByNameSize
 * url: getApiV2DataByIdFilesByNameSizeURL
 * method: getApiV2DataByIdFilesByNameSize_TYPE
 * raw_url: getApiV2DataByIdFilesByNameSize_RAW_URL
 * @param id - 対象データID
 * @param name - 対象ファイル名
 */
export const getApiV2DataByIdFilesByNameSize = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/files/{name}/size'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DataByIdFilesByNameSize_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{name}/size'
}
export const getApiV2DataByIdFilesByNameSize_TYPE = function() {
  return 'get'
}
export const getApiV2DataByIdFilesByNameSizeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/files/{name}/size'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したデータのファイル情報を全て取得する
 * request: getApiV2DataByIdFiles
 * url: getApiV2DataByIdFilesURL
 * method: getApiV2DataByIdFiles_TYPE
 * raw_url: getApiV2DataByIdFiles_RAW_URL
 * @param id - 対象データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const getApiV2DataByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DataByIdFiles_RAW_URL = function() {
  return '/api/v2/data/{id}/files'
}
export const getApiV2DataByIdFiles_TYPE = function() {
  return 'get'
}
export const getApiV2DataByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ファイルを追加する。
 * request: postApiV2DataByIdFiles
 * url: postApiV2DataByIdFilesURL
 * method: postApiV2DataByIdFiles_TYPE
 * raw_url: postApiV2DataByIdFiles_RAW_URL
 * @param id - 変更対象のデータID
 * @param body - 追加するファイル情報
 */
export const postApiV2DataByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2DataByIdFiles_RAW_URL = function() {
  return '/api/v2/data/{id}/files'
}
export const postApiV2DataByIdFiles_TYPE = function() {
  return 'post'
}
export const postApiV2DataByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのファイルを削除する
 * request: deleteApiV2DataByIdFilesByFileId
 * url: deleteApiV2DataByIdFilesByFileIdURL
 * method: deleteApiV2DataByIdFilesByFileId_TYPE
 * raw_url: deleteApiV2DataByIdFilesByFileId_RAW_URL
 * @param id - 対象のデータID
 * @param fileId - 削除するファイルのID
 */
export const deleteApiV2DataByIdFilesByFileId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/{id}/files/{fileId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters['fileId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: fileId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2DataByIdFilesByFileId_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{fileId}'
}
export const deleteApiV2DataByIdFilesByFileId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2DataByIdFilesByFileIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/{id}/files/{fileId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントに登録されているデータ管理で使用できるタグを表示する
 * request: getApiV2DataDatatags
 * url: getApiV2DataDatatagsURL
 * method: getApiV2DataDatatags_TYPE
 * raw_url: getApiV2DataDatatags_RAW_URL
 */
export const getApiV2DataDatatags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/datatags'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DataDatatags_RAW_URL = function() {
  return '/api/v2/data/datatags'
}
export const getApiV2DataDatatags_TYPE = function() {
  return 'get'
}
export const getApiV2DataDatatagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/datatags'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 
 * request: deleteApiV2DataDatatags
 * url: deleteApiV2DataDatatagsURL
 * method: deleteApiV2DataDatatags_TYPE
 * raw_url: deleteApiV2DataDatatags_RAW_URL
 */
export const deleteApiV2DataDatatags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data/datatags'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2DataDatatags_RAW_URL = function() {
  return '/api/v2/data/datatags'
}
export const deleteApiV2DataDatatags_TYPE = function() {
  return 'delete'
}
export const deleteApiV2DataDatatagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/data/datatags'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、データセット一覧を取得
* request: getApiV2Datasets
* url: getApiV2DatasetsURL
* method: getApiV2Datasets_TYPE
* raw_url: getApiV2Datasets_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param memo - メモ
     * @param createdAt - 作成時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Datasets = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Datasets_RAW_URL = function() {
  return '/api/v2/datasets'
}
export const getApiV2Datasets_TYPE = function() {
  return 'get'
}
export const getApiV2DatasetsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * データセットを新規作成する
 * request: postApiV2Datasets
 * url: postApiV2DatasetsURL
 * method: postApiV2Datasets_TYPE
 * raw_url: postApiV2Datasets_RAW_URL
 * @param body - 新規作成内容
 */
export const postApiV2Datasets = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2Datasets_RAW_URL = function() {
  return '/api/v2/datasets'
}
export const postApiV2Datasets_TYPE = function() {
  return 'post'
}
export const postApiV2DatasetsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIDのデータセット詳細情報を取得する。
 * request: getApiV2DatasetsById
 * url: getApiV2DatasetsByIdURL
 * method: getApiV2DatasetsById_TYPE
 * raw_url: getApiV2DatasetsById_RAW_URL
 * @param id - データセットID
 */
export const getApiV2DatasetsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DatasetsById_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const getApiV2DatasetsById_TYPE = function() {
  return 'get'
}
export const getApiV2DatasetsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * データセットの付属情報（メモなど、任意のタイミングで変更できるもの）を変更する。
 * request: patchApiV2DatasetsById
 * url: patchApiV2DatasetsByIdURL
 * method: patchApiV2DatasetsById_TYPE
 * raw_url: patchApiV2DatasetsById_RAW_URL
 * @param id - データセットID
 * @param body - 変更内容
 */
export const patchApiV2DatasetsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const patchApiV2DatasetsById_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const patchApiV2DatasetsById_TYPE = function() {
  return 'patch'
}
export const patchApiV2DatasetsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * データセットのエントリ内容（学習で使用後は編集不可）を変更する
 * request: putApiV2DatasetsById
 * url: putApiV2DatasetsByIdURL
 * method: putApiV2DatasetsById_TYPE
 * raw_url: putApiV2DatasetsById_RAW_URL
 * @param id - データセットID
 * @param body - 変更内容
 */
export const putApiV2DatasetsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2DatasetsById_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const putApiV2DatasetsById_TYPE = function() {
  return 'put'
}
export const putApiV2DatasetsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * データセットを削除する
 * request: deleteApiV2DatasetsById
 * url: deleteApiV2DatasetsByIdURL
 * method: deleteApiV2DatasetsById_TYPE
 * raw_url: deleteApiV2DatasetsById_RAW_URL
 * @param id - データセットID
 */
export const deleteApiV2DatasetsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2DatasetsById_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const deleteApiV2DatasetsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2DatasetsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIDのデータセットに含まれるデータファイル情報を取得する。
 * request: getApiV2DatasetsByIdFiles
 * url: getApiV2DatasetsByIdFilesURL
 * method: getApiV2DatasetsByIdFiles_TYPE
 * raw_url: getApiV2DatasetsByIdFiles_RAW_URL
 * @param id - データセットID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const getApiV2DatasetsByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DatasetsByIdFiles_RAW_URL = function() {
  return '/api/v2/datasets/{id}/files'
}
export const getApiV2DatasetsByIdFiles_TYPE = function() {
  return 'get'
}
export const getApiV2DatasetsByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIDのデータセットに含まれるデータとNFS上のデータ名のペア情報を取得する。
 * request: getApiV2DatasetsByIdPathpairs
 * url: getApiV2DatasetsByIdPathpairsURL
 * method: getApiV2DatasetsByIdPathpairs_TYPE
 * raw_url: getApiV2DatasetsByIdPathpairs_RAW_URL
 * @param id - データセットID
 */
export const getApiV2DatasetsByIdPathpairs = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets/{id}/pathpairs'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DatasetsByIdPathpairs_RAW_URL = function() {
  return '/api/v2/datasets/{id}/pathpairs'
}
export const getApiV2DatasetsByIdPathpairs_TYPE = function() {
  return 'get'
}
export const getApiV2DatasetsByIdPathpairsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datasets/{id}/pathpairs'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全データ種別を取得する
 * request: getApiV2Datatypes
 * url: getApiV2DatatypesURL
 * method: getApiV2Datatypes_TYPE
 * raw_url: getApiV2Datatypes_RAW_URL
 */
export const getApiV2Datatypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datatypes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Datatypes_RAW_URL = function() {
  return '/api/v2/datatypes'
}
export const getApiV2Datatypes_TYPE = function() {
  return 'get'
}
export const getApiV2DatatypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/datatypes'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、実験一覧を取得する
* request: getApiV2Experiment
* url: getApiV2ExperimentURL
* method: getApiV2Experiment_TYPE
* raw_url: getApiV2Experiment_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param startedAt - 実行時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Experiment = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Experiment_RAW_URL = function() {
  return '/api/v2/experiment'
}
export const getApiV2Experiment_TYPE = function() {
  return 'get'
}
export const getApiV2ExperimentURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 実験を取得する
 * request: getApiV2ExperimentById
 * url: getApiV2ExperimentByIdURL
 * method: getApiV2ExperimentById_TYPE
 * raw_url: getApiV2ExperimentById_RAW_URL
 * @param id - 実験ID
 */
export const getApiV2ExperimentById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2ExperimentById_RAW_URL = function() {
  return '/api/v2/experiment/{id}'
}
export const getApiV2ExperimentById_TYPE = function() {
  return 'get'
}
export const getApiV2ExperimentByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 実験を削除する
 * request: deleteApiV2ExperimentById
 * url: deleteApiV2ExperimentByIdURL
 * method: deleteApiV2ExperimentById_TYPE
 * raw_url: deleteApiV2ExperimentById_RAW_URL
 * @param id - 実験ID
 */
export const deleteApiV2ExperimentById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2ExperimentById_RAW_URL = function() {
  return '/api/v2/experiment/{id}'
}
export const deleteApiV2ExperimentById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2ExperimentByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 実験を開始する
 * request: postApiV2ExperimentRun
 * url: postApiV2ExperimentRunURL
 * method: postApiV2ExperimentRun_TYPE
 * raw_url: postApiV2ExperimentRun_RAW_URL
 * @param body - 
 */
export const postApiV2ExperimentRun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2ExperimentRun_RAW_URL = function() {
  return '/api/v2/experiment/run'
}
export const postApiV2ExperimentRun_TYPE = function() {
  return 'post'
}
export const postApiV2ExperimentRunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/run'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 前処理を正常終了させ、後続の学習を開始する
 * request: postApiV2ExperimentByIdPreprocessingComplete
 * url: postApiV2ExperimentByIdPreprocessingCompleteURL
 * method: postApiV2ExperimentByIdPreprocessingComplete_TYPE
 * raw_url: postApiV2ExperimentByIdPreprocessingComplete_RAW_URL
 * @param id - 学習履歴ID
 */
export const postApiV2ExperimentByIdPreprocessingComplete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}/preprocessing/complete'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2ExperimentByIdPreprocessingComplete_RAW_URL = function() {
  return '/api/v2/experiment/{id}/preprocessing/complete'
}
export const postApiV2ExperimentByIdPreprocessingComplete_TYPE = function() {
  return 'post'
}
export const postApiV2ExperimentByIdPreprocessingCompleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}/preprocessing/complete'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウム推論を作成する
 * request: postApiV2ExperimentByIdEvaluations
 * url: postApiV2ExperimentByIdEvaluationsURL
 * method: postApiV2ExperimentByIdEvaluations_TYPE
 * raw_url: postApiV2ExperimentByIdEvaluations_RAW_URL
 * @param id - 実験ID
 * @param body - アクアリウム推論
 */
export const postApiV2ExperimentByIdEvaluations = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}/evaluations'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2ExperimentByIdEvaluations_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations'
}
export const postApiV2ExperimentByIdEvaluations_TYPE = function() {
  return 'post'
}
export const postApiV2ExperimentByIdEvaluationsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}/evaluations'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウム推論一覧を取得する
 * request: getApiV2ExperimentByIdEvaluations
 * url: getApiV2ExperimentByIdEvaluationsURL
 * method: getApiV2ExperimentByIdEvaluations_TYPE
 * raw_url: getApiV2ExperimentByIdEvaluations_RAW_URL
 * @param id - 実験ID
 */
export const getApiV2ExperimentByIdEvaluations = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}/evaluations'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2ExperimentByIdEvaluations_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations'
}
export const getApiV2ExperimentByIdEvaluations_TYPE = function() {
  return 'get'
}
export const getApiV2ExperimentByIdEvaluationsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}/evaluations'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * アクアリウム推論を削除する
 * request: deleteApiV2ExperimentByIdEvaluationsByEvaluationId
 * url: deleteApiV2ExperimentByIdEvaluationsByEvaluationIdURL
 * method: deleteApiV2ExperimentByIdEvaluationsByEvaluationId_TYPE
 * raw_url: deleteApiV2ExperimentByIdEvaluationsByEvaluationId_RAW_URL
 * @param id - 実験ID
 * @param evaluationId - アクアリウム推論ID
 */
export const deleteApiV2ExperimentByIdEvaluationsByEvaluationId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/{id}/evaluations/{evaluationId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{evaluationId}', `${parameters['evaluationId']}`)
  if (parameters['evaluationId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: evaluationId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2ExperimentByIdEvaluationsByEvaluationId_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations/{evaluationId}'
}
export const deleteApiV2ExperimentByIdEvaluationsByEvaluationId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2ExperimentByIdEvaluationsByEvaluationIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/experiment/{id}/evaluations/{evaluationId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{evaluationId}', `${parameters['evaluationId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 登録済みのGitエンドポイント一覧を取得
 * request: getApiV2AdminGitEndpoints
 * url: getApiV2AdminGitEndpointsURL
 * method: getApiV2AdminGitEndpoints_TYPE
 * raw_url: getApiV2AdminGitEndpoints_RAW_URL
 */
export const getApiV2AdminGitEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminGitEndpoints_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints'
}
export const getApiV2AdminGitEndpoints_TYPE = function() {
  return 'get'
}
export const getApiV2AdminGitEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にGitエンドポイントを登録する
 * request: postApiV2AdminGitEndpoints
 * url: postApiV2AdminGitEndpointsURL
 * method: postApiV2AdminGitEndpoints_TYPE
 * raw_url: postApiV2AdminGitEndpoints_RAW_URL
 * @param body - 新規作成モデル
 */
export const postApiV2AdminGitEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminGitEndpoints_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints'
}
export const postApiV2AdminGitEndpoints_TYPE = function() {
  return 'post'
}
export const postApiV2AdminGitEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Git種別一覧を取得
 * request: getApiV2AdminGitTypes
 * url: getApiV2AdminGitTypesURL
 * method: getApiV2AdminGitTypes_TYPE
 * raw_url: getApiV2AdminGitTypes_RAW_URL
 */
export const getApiV2AdminGitTypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/types'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminGitTypes_RAW_URL = function() {
  return '/api/v2/admin/git/types'
}
export const getApiV2AdminGitTypes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminGitTypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/types'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのGitエンドポイント情報を取得。
 * request: getApiV2AdminGitEndpointsById
 * url: getApiV2AdminGitEndpointsByIdURL
 * method: getApiV2AdminGitEndpointsById_TYPE
 * raw_url: getApiV2AdminGitEndpointsById_RAW_URL
 * @param id - GitエンドポイントID
 */
export const getApiV2AdminGitEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminGitEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const getApiV2AdminGitEndpointsById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminGitEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Gitエンドポイント情報の編集
 * request: putApiV2AdminGitEndpointsById
 * url: putApiV2AdminGitEndpointsByIdURL
 * method: putApiV2AdminGitEndpointsById_TYPE
 * raw_url: putApiV2AdminGitEndpointsById_RAW_URL
 * @param id - 編集対象GitID
 * @param body - 編集モデル
 */
export const putApiV2AdminGitEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminGitEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const putApiV2AdminGitEndpointsById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminGitEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Gitエンドポイント情報の削除
 * request: deleteApiV2AdminGitEndpointsById
 * url: deleteApiV2AdminGitEndpointsByIdURL
 * method: deleteApiV2AdminGitEndpointsById_TYPE
 * raw_url: deleteApiV2AdminGitEndpointsById_RAW_URL
 * @param id - 削除対象GitID
 */
export const deleteApiV2AdminGitEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminGitEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const deleteApiV2AdminGitEndpointsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminGitEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/git/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント管理者が選択可能な登録済みのGitエンドポイント一覧を取得
 * request: getApiV2TenantGitEndpoints
 * url: getApiV2TenantGitEndpointsURL
 * method: getApiV2TenantGitEndpoints_TYPE
 * raw_url: getApiV2TenantGitEndpoints_RAW_URL
 */
export const getApiV2TenantGitEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/git/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantGitEndpoints_RAW_URL = function() {
  return '/api/v2/tenant/git/endpoints'
}
export const getApiV2TenantGitEndpoints_TYPE = function() {
  return 'get'
}
export const getApiV2TenantGitEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/git/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全てのリポジトリを取得する
 * request: getApiV2GitByGitIdRepos
 * url: getApiV2GitByGitIdReposURL
 * method: getApiV2GitByGitIdRepos_TYPE
 * raw_url: getApiV2GitByGitIdRepos_RAW_URL
 * @param gitId - 
 */
export const getApiV2GitByGitIdRepos = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/git/{gitId}/repos'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters['gitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: gitId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2GitByGitIdRepos_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos'
}
export const getApiV2GitByGitIdRepos_TYPE = function() {
  return 'get'
}
export const getApiV2GitByGitIdReposURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ブランチ一覧を取得する
 * request: getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches
 * url: getApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesURL
 * method: getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches_TYPE
 * raw_url: getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 */
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/branches'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters['gitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: gitId'))
  }
  path = path.replace('{owner}', `${parameters['owner']}`)
  if (parameters['owner'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: owner'))
  }
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters['repositoryName'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: repositoryName'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/branches'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches_TYPE = function() {
  return 'get'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/branches'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{owner}', `${parameters['owner']}`)
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * コミット一覧を取得する
 * request: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits
 * url: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsURL
 * method: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits_TYPE
 * raw_url: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param branch - ブランチ名
 * @param page - ページ番号
 */
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters['gitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: gitId'))
  }
  path = path.replace('{owner}', `${parameters['owner']}`)
  if (parameters['owner'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: owner'))
  }
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters['repositoryName'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: repositoryName'))
  }
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits_TYPE = function() {
  return 'get'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{owner}', `${parameters['owner']}`)
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したコミットIDのコミット詳細を取得する
 * request: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId
 * url: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdURL
 * method: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId_TYPE
 * raw_url: getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param commitId - コミットID
 */
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters['gitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: gitId'))
  }
  path = path.replace('{owner}', `${parameters['owner']}`)
  if (parameters['owner'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: owner'))
  }
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters['repositoryName'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: repositoryName'))
  }
  path = path.replace('{commitId}', `${parameters['commitId']}`)
  if (parameters['commitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: commitId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId_TYPE = function() {
  return 'get'
}
export const getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{owner}', `${parameters['owner']}`)
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  path = path.replace('{commitId}', `${parameters['commitId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 階層化されたURLを吸収するためのダミーAPI。
製品版のSwaggerからは削除する。
* request: getApiV2GitByGitIdReposBySegments
* url: getApiV2GitByGitIdReposBySegmentsURL
* method: getApiV2GitByGitIdReposBySegments_TYPE
* raw_url: getApiV2GitByGitIdReposBySegments_RAW_URL
     * @param gitId - 
     * @param segments - 
     * @param branch - 
     * @param page - 
*/
export const getApiV2GitByGitIdReposBySegments = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/git/{gitId}/repos/{segments}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  if (parameters['gitId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: gitId'))
  }
  path = path.replace('{segments}', `${parameters['segments']}`)
  if (parameters['segments'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: segments'))
  }
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2GitByGitIdReposBySegments_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{segments}'
}
export const getApiV2GitByGitIdReposBySegments_TYPE = function() {
  return 'get'
}
export const getApiV2GitByGitIdReposBySegmentsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{segments}'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{segments}', `${parameters['segments']}`)
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全推論履歴のIDと名前を取得
 * request: getApiV2InferencesSimple
 * url: getApiV2InferencesSimpleURL
 * method: getApiV2InferencesSimple_TYPE
 * raw_url: getApiV2InferencesSimple_RAW_URL
 */
export const getApiV2InferencesSimple = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/simple'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesSimple_RAW_URL = function() {
  return '/api/v2/inferences/simple'
}
export const getApiV2InferencesSimple_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesSimpleURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/simple'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、全推論履歴を取得
* request: getApiV2Inferences
* url: getApiV2InferencesURL
* method: getApiV2Inferences_TYPE
* raw_url: getApiV2Inferences_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param startedAt - 実行時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param startedBy - 実行者
     * @param dataSet - データセット名
     * @param memo - メモ
     * @param status - ステータス
     * @param entryPoint - 実行コマンド
     * @param parentId - マウントした学習ID
     * @param parentInferenceId - マウントした推論ID
     * @param parentName - マウントした学習名
     * @param parentInferenceName - マウントした推論名
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Inferences = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['parentId'] !== undefined) {
    queryParameters['ParentId'] = parameters['parentId']
  }
  if (parameters['parentInferenceId'] !== undefined) {
    queryParameters['ParentInferenceId'] = parameters['parentInferenceId']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['parentInferenceName'] !== undefined) {
    queryParameters['ParentInferenceName'] = parameters['parentInferenceName']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Inferences_RAW_URL = function() {
  return '/api/v2/inferences'
}
export const getApiV2Inferences_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['parentId'] !== undefined) {
    queryParameters['ParentId'] = parameters['parentId']
  }
  if (parameters['parentInferenceId'] !== undefined) {
    queryParameters['ParentInferenceId'] = parameters['parentInferenceId']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['parentInferenceName'] !== undefined) {
    queryParameters['ParentInferenceName'] = parameters['parentInferenceName']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * マウントする推論履歴を取得
 * request: getApiV2InferencesMount
 * url: getApiV2InferencesMountURL
 * method: getApiV2InferencesMount_TYPE
 * raw_url: getApiV2InferencesMount_RAW_URL
 * @param status - ステータス
 */
export const getApiV2InferencesMount = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/mount'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesMount_RAW_URL = function() {
  return '/api/v2/inferences/mount'
}
export const getApiV2InferencesMount_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesMountURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/mount'
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDの推論履歴の詳細情報を取得
 * request: getApiV2InferencesById
 * url: getApiV2InferencesByIdURL
 * method: getApiV2InferencesById_TYPE
 * raw_url: getApiV2InferencesById_RAW_URL
 * @param id - 推論履歴ID
 */
export const getApiV2InferencesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesById_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const getApiV2InferencesById_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴の編集
 * request: putApiV2InferencesById
 * url: putApiV2InferencesByIdURL
 * method: putApiV2InferencesById_TYPE
 * raw_url: putApiV2InferencesById_RAW_URL
 * @param id - 変更対象の推論履歴ID
 * @param body - 変更内容
 */
export const putApiV2InferencesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2InferencesById_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const putApiV2InferencesById_TYPE = function() {
  return 'put'
}
export const putApiV2InferencesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴を削除
 * request: deleteApiV2InferencesById
 * url: deleteApiV2InferencesByIdURL
 * method: deleteApiV2InferencesById_TYPE
 * raw_url: deleteApiV2InferencesById_RAW_URL
 * @param id - 推論履歴ID
 */
export const deleteApiV2InferencesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2InferencesById_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const deleteApiV2InferencesById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2InferencesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定された推論履歴のエラーイベントを取得
 * request: getApiV2InferencesByIdEvents
 * url: getApiV2InferencesByIdEventsURL
 * method: getApiV2InferencesByIdEvents_TYPE
 * raw_url: getApiV2InferencesByIdEvents_RAW_URL
 * @param id - 推論履歴ID
 */
export const getApiV2InferencesByIdEvents = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/events'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesByIdEvents_RAW_URL = function() {
  return '/api/v2/inferences/{id}/events'
}
export const getApiV2InferencesByIdEvents_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesByIdEventsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/events'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規に推論を開始
 * request: postApiV2InferencesRun
 * url: postApiV2InferencesRunURL
 * method: postApiV2InferencesRun_TYPE
 * raw_url: postApiV2InferencesRun_RAW_URL
 * @param body - 新規推論実行内容
 */
export const postApiV2InferencesRun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2InferencesRun_RAW_URL = function() {
  return '/api/v2/inferences/run'
}
export const postApiV2InferencesRun_TYPE = function() {
  return 'post'
}
export const postApiV2InferencesRunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/run'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴添付ファイルを登録
 * request: postApiV2InferencesByIdFiles
 * url: postApiV2InferencesByIdFilesURL
 * method: postApiV2InferencesByIdFiles_TYPE
 * raw_url: postApiV2InferencesByIdFiles_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param body - 追加するファイル情報
 */
export const postApiV2InferencesByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2InferencesByIdFiles_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files'
}
export const postApiV2InferencesByIdFiles_TYPE = function() {
  return 'post'
}
export const postApiV2InferencesByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴添付ファイルの一覧を取得
 * request: getApiV2InferencesByIdFiles
 * url: getApiV2InferencesByIdFilesURL
 * method: getApiV2InferencesByIdFiles_TYPE
 * raw_url: getApiV2InferencesByIdFiles_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const getApiV2InferencesByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesByIdFiles_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files'
}
export const getApiV2InferencesByIdFiles_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: getApiV2InferencesByIdContainerFiles
* url: getApiV2InferencesByIdContainerFilesURL
* method: getApiV2InferencesByIdContainerFiles_TYPE
* raw_url: getApiV2InferencesByIdContainerFiles_RAW_URL
     * @param id - 対象の推論履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const getApiV2InferencesByIdContainerFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/container-files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesByIdContainerFiles_RAW_URL = function() {
  return '/api/v2/inferences/{id}/container-files'
}
export const getApiV2InferencesByIdContainerFiles_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesByIdContainerFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/container-files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴添付ファイルを削除
 * request: deleteApiV2InferencesByIdFilesByFileId
 * url: deleteApiV2InferencesByIdFilesByFileIdURL
 * method: deleteApiV2InferencesByIdFilesByFileId_TYPE
 * raw_url: deleteApiV2InferencesByIdFilesByFileId_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param fileId - 削除するファイルのID
 */
export const deleteApiV2InferencesByIdFilesByFileId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/files/{fileId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters['fileId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: fileId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2InferencesByIdFilesByFileId_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files/{fileId}'
}
export const deleteApiV2InferencesByIdFilesByFileId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2InferencesByIdFilesByFileIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/files/{fileId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論履歴添付ファイルのサイズ(Byte)を取得する
 * request: getApiV2InferencesByIdFilesByNameSize
 * url: getApiV2InferencesByIdFilesByNameSizeURL
 * method: getApiV2InferencesByIdFilesByNameSize_TYPE
 * raw_url: getApiV2InferencesByIdFilesByNameSize_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param name - 対象ファイル名
 */
export const getApiV2InferencesByIdFilesByNameSize = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/files/{name}/size'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2InferencesByIdFilesByNameSize_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files/{name}/size'
}
export const getApiV2InferencesByIdFilesByNameSize_TYPE = function() {
  return 'get'
}
export const getApiV2InferencesByIdFilesByNameSizeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/files/{name}/size'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論を途中で強制終了
 * request: postApiV2InferencesByIdHalt
 * url: postApiV2InferencesByIdHaltURL
 * method: postApiV2InferencesByIdHalt_TYPE
 * raw_url: postApiV2InferencesByIdHalt_RAW_URL
 * @param id - 推論履歴ID
 */
export const postApiV2InferencesByIdHalt = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/halt'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2InferencesByIdHalt_RAW_URL = function() {
  return '/api/v2/inferences/{id}/halt'
}
export const postApiV2InferencesByIdHalt_TYPE = function() {
  return 'post'
}
export const postApiV2InferencesByIdHaltURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/halt'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 推論を途中で強制終了させる。
ユーザ自身がジョブを停止させた場合。
* request: postApiV2InferencesByIdUserCancel
* url: postApiV2InferencesByIdUserCancelURL
* method: postApiV2InferencesByIdUserCancel_TYPE
* raw_url: postApiV2InferencesByIdUserCancel_RAW_URL
     * @param id - 推論履歴ID
*/
export const postApiV2InferencesByIdUserCancel = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/user-cancel'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2InferencesByIdUserCancel_RAW_URL = function() {
  return '/api/v2/inferences/{id}/user-cancel'
}
export const postApiV2InferencesByIdUserCancel_TYPE = function() {
  return 'post'
}
export const postApiV2InferencesByIdUserCancelURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/user-cancel'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 推論を正常終了
 * request: postApiV2InferencesByIdComplete
 * url: postApiV2InferencesByIdCompleteURL
 * method: postApiV2InferencesByIdComplete_TYPE
 * raw_url: postApiV2InferencesByIdComplete_RAW_URL
 * @param id - 推論履歴ID
 */
export const postApiV2InferencesByIdComplete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/{id}/complete'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2InferencesByIdComplete_RAW_URL = function() {
  return '/api/v2/inferences/{id}/complete'
}
export const postApiV2InferencesByIdComplete_TYPE = function() {
  return 'post'
}
export const postApiV2InferencesByIdCompleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/inferences/{id}/complete'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント向けに、メニューとロールのマッピング情報一覧を取得
 * request: getApiV2TenantMenus
 * url: getApiV2TenantMenusURL
 * method: getApiV2TenantMenus_TYPE
 * raw_url: getApiV2TenantMenus_RAW_URL
 */
export const getApiV2TenantMenus = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/menus'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantMenus_RAW_URL = function() {
  return '/api/v2/tenant/menus'
}
export const getApiV2TenantMenus_TYPE = function() {
  return 'get'
}
export const getApiV2TenantMenusURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/menus'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント向けの、メニューとロールのマッピング情報を更新
 * request: putApiV2TenantMenusById
 * url: putApiV2TenantMenusByIdURL
 * method: putApiV2TenantMenusById_TYPE
 * raw_url: putApiV2TenantMenusById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2TenantMenusById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/menus/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2TenantMenusById_RAW_URL = function() {
  return '/api/v2/tenant/menus/{id}'
}
export const putApiV2TenantMenusById_TYPE = function() {
  return 'put'
}
export const putApiV2TenantMenusByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/menus/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント向けのメニュー種別一覧を取得
 * request: getApiV2TenantMenuTypes
 * url: getApiV2TenantMenuTypesURL
 * method: getApiV2TenantMenuTypes_TYPE
 * raw_url: getApiV2TenantMenuTypes_RAW_URL
 */
export const getApiV2TenantMenuTypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/menu-types'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantMenuTypes_RAW_URL = function() {
  return '/api/v2/tenant/menu-types'
}
export const getApiV2TenantMenuTypes_TYPE = function() {
  return 'get'
}
export const getApiV2TenantMenuTypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/menu-types'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 管理者向けに、メニューとロールのマッピング情報一覧を取得
 * request: getApiV2AdminMenus
 * url: getApiV2AdminMenusURL
 * method: getApiV2AdminMenus_TYPE
 * raw_url: getApiV2AdminMenus_RAW_URL
 */
export const getApiV2AdminMenus = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/menus'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminMenus_RAW_URL = function() {
  return '/api/v2/admin/menus'
}
export const getApiV2AdminMenus_TYPE = function() {
  return 'get'
}
export const getApiV2AdminMenusURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/menus'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 管理者向けの、メニューとロールのマッピング情報を更新
 * request: putApiV2AdminMenusById
 * url: putApiV2AdminMenusByIdURL
 * method: putApiV2AdminMenusById_TYPE
 * raw_url: putApiV2AdminMenusById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminMenusById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/menus/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminMenusById_RAW_URL = function() {
  return '/api/v2/admin/menus/{id}'
}
export const putApiV2AdminMenusById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminMenusByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/menus/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 管理者向けメニュー種別一覧を取得
 * request: getApiV2AdminMenuTypes
 * url: getApiV2AdminMenuTypesURL
 * method: getApiV2AdminMenuTypes_TYPE
 * raw_url: getApiV2AdminMenuTypes_RAW_URL
 */
export const getApiV2AdminMenuTypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/menu-types'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminMenuTypes_RAW_URL = function() {
  return '/api/v2/admin/menu-types'
}
export const getApiV2AdminMenuTypes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminMenuTypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/menu-types'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全ノード一覧を取得
 * request: getApiV2AdminNodes
 * url: getApiV2AdminNodesURL
 * method: getApiV2AdminNodes_TYPE
 * raw_url: getApiV2AdminNodes_RAW_URL
 * @param name - 
 * @param perPage - 
 * @param page - 
 * @param withTotal - 
 */
export const getApiV2AdminNodes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['name'] !== undefined) {
    queryParameters['name'] = parameters['name']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminNodes_RAW_URL = function() {
  return '/api/v2/admin/nodes'
}
export const getApiV2AdminNodes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminNodesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes'
  if (parameters['name'] !== undefined) {
    queryParameters['name'] = parameters['name']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にノードを登録する
 * request: postApiV2AdminNodes
 * url: postApiV2AdminNodesURL
 * method: postApiV2AdminNodes_TYPE
 * raw_url: postApiV2AdminNodes_RAW_URL
 * @param body - 
 */
export const postApiV2AdminNodes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminNodes_RAW_URL = function() {
  return '/api/v2/admin/nodes'
}
export const postApiV2AdminNodes_TYPE = function() {
  return 'post'
}
export const postApiV2AdminNodesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノードアクセスレベルの一覧を取得する
 * request: getApiV2AdminNodeAccessLevels
 * url: getApiV2AdminNodeAccessLevelsURL
 * method: getApiV2AdminNodeAccessLevels_TYPE
 * raw_url: getApiV2AdminNodeAccessLevels_RAW_URL
 */
export const getApiV2AdminNodeAccessLevels = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/node-access-levels'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminNodeAccessLevels_RAW_URL = function() {
  return '/api/v2/admin/node-access-levels'
}
export const getApiV2AdminNodeAccessLevels_TYPE = function() {
  return 'get'
}
export const getApiV2AdminNodeAccessLevelsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/node-access-levels'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのノード情報を取得。
 * request: getApiV2AdminNodesById
 * url: getApiV2AdminNodesByIdURL
 * method: getApiV2AdminNodesById_TYPE
 * raw_url: getApiV2AdminNodesById_RAW_URL
 * @param id - ノードID
 */
export const getApiV2AdminNodesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminNodesById_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const getApiV2AdminNodesById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminNodesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノード情報の編集
 * request: putApiV2AdminNodesById
 * url: putApiV2AdminNodesByIdURL
 * method: putApiV2AdminNodesById_TYPE
 * raw_url: putApiV2AdminNodesById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminNodesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminNodesById_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const putApiV2AdminNodesById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminNodesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノードを削除する。
 * request: deleteApiV2AdminNodesById
 * url: deleteApiV2AdminNodesByIdURL
 * method: deleteApiV2AdminNodesById_TYPE
 * raw_url: deleteApiV2AdminNodesById_RAW_URL
 * @param id - 
 */
export const deleteApiV2AdminNodesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminNodesById_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const deleteApiV2AdminNodesById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminNodesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノード情報をDBからClusterへ同期させる
 * request: postApiV2AdminNodesSyncClusterFromDb
 * url: postApiV2AdminNodesSyncClusterFromDbURL
 * method: postApiV2AdminNodesSyncClusterFromDb_TYPE
 * raw_url: postApiV2AdminNodesSyncClusterFromDb_RAW_URL
 */
export const postApiV2AdminNodesSyncClusterFromDb = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes/sync-cluster-from-db'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminNodesSyncClusterFromDb_RAW_URL = function() {
  return '/api/v2/admin/nodes/sync-cluster-from-db'
}
export const postApiV2AdminNodesSyncClusterFromDb_TYPE = function() {
  return 'post'
}
export const postApiV2AdminNodesSyncClusterFromDbURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/nodes/sync-cluster-from-db'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全ノートブック履歴のIDと名前を取得
 * request: getApiV2NotebookSimple
 * url: getApiV2NotebookSimpleURL
 * method: getApiV2NotebookSimple_TYPE
 * raw_url: getApiV2NotebookSimple_RAW_URL
 */
export const getApiV2NotebookSimple = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/simple'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookSimple_RAW_URL = function() {
  return '/api/v2/notebook/simple'
}
export const getApiV2NotebookSimple_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookSimpleURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/simple'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、全ノートブック履歴を取得
* request: getApiV2Notebook
* url: getApiV2NotebookURL
* method: getApiV2Notebook_TYPE
* raw_url: getApiV2Notebook_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param createdAt - 作成日時
     * @param createdBy - 作成者
     * @param memo - メモ
     * @param status - ステータス
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Notebook = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Notebook_RAW_URL = function() {
  return '/api/v2/notebook'
}
export const getApiV2Notebook_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['createdBy'] !== undefined) {
    queryParameters['CreatedBy'] = parameters['createdBy']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのノートブック履歴の詳細情報を取得。
 * request: getApiV2NotebookById
 * url: getApiV2NotebookByIdURL
 * method: getApiV2NotebookById_TYPE
 * raw_url: getApiV2NotebookById_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const getApiV2NotebookById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookById_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const getApiV2NotebookById_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノートブック履歴の編集
 * request: putApiV2NotebookById
 * url: putApiV2NotebookByIdURL
 * method: putApiV2NotebookById_TYPE
 * raw_url: putApiV2NotebookById_RAW_URL
 * @param id - 変更対象のノートブック履歴ID
 * @param body - 変更内容
 */
export const putApiV2NotebookById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2NotebookById_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const putApiV2NotebookById_TYPE = function() {
  return 'put'
}
export const putApiV2NotebookByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノートブック履歴を削除する。
 * request: deleteApiV2NotebookById
 * url: deleteApiV2NotebookByIdURL
 * method: deleteApiV2NotebookById_TYPE
 * raw_url: deleteApiV2NotebookById_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const deleteApiV2NotebookById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2NotebookById_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const deleteApiV2NotebookById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2NotebookByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたノートブック履歴のエラーイベントを取得します。
 * request: getApiV2NotebookByIdEvents
 * url: getApiV2NotebookByIdEventsURL
 * method: getApiV2NotebookByIdEvents_TYPE
 * raw_url: getApiV2NotebookByIdEvents_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const getApiV2NotebookByIdEvents = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}/events'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookByIdEvents_RAW_URL = function() {
  return '/api/v2/notebook/{id}/events'
}
export const getApiV2NotebookByIdEvents_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookByIdEventsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}/events'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたノートブック履歴のエンドポイントを取得します。
 * request: getApiV2NotebookByIdEndpoint
 * url: getApiV2NotebookByIdEndpointURL
 * method: getApiV2NotebookByIdEndpoint_TYPE
 * raw_url: getApiV2NotebookByIdEndpoint_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const getApiV2NotebookByIdEndpoint = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}/endpoint'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookByIdEndpoint_RAW_URL = function() {
  return '/api/v2/notebook/{id}/endpoint'
}
export const getApiV2NotebookByIdEndpoint_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookByIdEndpointURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}/endpoint'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にノートブックコンテナを開始する
 * request: postApiV2NotebookRun
 * url: postApiV2NotebookRunURL
 * method: postApiV2NotebookRun_TYPE
 * raw_url: postApiV2NotebookRun_RAW_URL
 * @param body - 新規実行内容
 */
export const postApiV2NotebookRun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2NotebookRun_RAW_URL = function() {
  return '/api/v2/notebook/run'
}
export const postApiV2NotebookRun_TYPE = function() {
  return 'post'
}
export const postApiV2NotebookRunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/run'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: getApiV2NotebookByIdContainerFiles
* url: getApiV2NotebookByIdContainerFilesURL
* method: getApiV2NotebookByIdContainerFiles_TYPE
* raw_url: getApiV2NotebookByIdContainerFiles_RAW_URL
     * @param id - 対象のノートブック履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const getApiV2NotebookByIdContainerFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}/container-files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookByIdContainerFiles_RAW_URL = function() {
  return '/api/v2/notebook/{id}/container-files'
}
export const getApiV2NotebookByIdContainerFiles_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookByIdContainerFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}/container-files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノートブックコンテナを途中で強制終了させる。
 * request: postApiV2NotebookByIdHalt
 * url: postApiV2NotebookByIdHaltURL
 * method: postApiV2NotebookByIdHalt_TYPE
 * raw_url: postApiV2NotebookByIdHalt_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const postApiV2NotebookByIdHalt = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}/halt'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2NotebookByIdHalt_RAW_URL = function() {
  return '/api/v2/notebook/{id}/halt'
}
export const postApiV2NotebookByIdHalt_TYPE = function() {
  return 'post'
}
export const postApiV2NotebookByIdHaltURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}/halt'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたノートブック履歴のコンテナを再起動する
 * request: postApiV2NotebookByIdRerun
 * url: postApiV2NotebookByIdRerunURL
 * method: postApiV2NotebookByIdRerun_TYPE
 * raw_url: postApiV2NotebookByIdRerun_RAW_URL
 * @param id - ノートブック履歴ID
 * @param body - 再起動内容
 */
export const postApiV2NotebookByIdRerun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/{id}/rerun'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2NotebookByIdRerun_RAW_URL = function() {
  return '/api/v2/notebook/{id}/rerun'
}
export const postApiV2NotebookByIdRerun_TYPE = function() {
  return 'post'
}
export const postApiV2NotebookByIdRerunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/{id}/rerun'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントのノートブック無期限利用可否フラグを取得する
 * request: getApiV2NotebookAvailableInfiniteTime
 * url: getApiV2NotebookAvailableInfiniteTimeURL
 * method: getApiV2NotebookAvailableInfiniteTime_TYPE
 * raw_url: getApiV2NotebookAvailableInfiniteTime_RAW_URL
 */
export const getApiV2NotebookAvailableInfiniteTime = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/available-infinite-time'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2NotebookAvailableInfiniteTime_RAW_URL = function() {
  return '/api/v2/notebook/available-infinite-time'
}
export const getApiV2NotebookAvailableInfiniteTime_TYPE = function() {
  return 'get'
}
export const getApiV2NotebookAvailableInfiniteTimeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/notebook/available-infinite-time'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、全前処理を取得
* request: getApiV2Preprocessings
* url: getApiV2PreprocessingsURL
* method: getApiV2Preprocessings_TYPE
* raw_url: getApiV2Preprocessings_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param createdAt - 実行時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param memo - メモ
     * @param perPage - 表示件数。指定がない場合は全件。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Preprocessings = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Preprocessings_RAW_URL = function() {
  return '/api/v2/preprocessings'
}
export const getApiV2Preprocessings_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['createdAt'] !== undefined) {
    queryParameters['CreatedAt'] = parameters['createdAt']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規に前処理を登録する
 * request: postApiV2Preprocessings
 * url: postApiV2PreprocessingsURL
 * method: postApiV2Preprocessings_TYPE
 * raw_url: postApiV2Preprocessings_RAW_URL
 * @param body - 新規作成内容
 */
export const postApiV2Preprocessings = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2Preprocessings_RAW_URL = function() {
  return '/api/v2/preprocessings'
}
export const postApiV2Preprocessings_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDの前処理の詳細情報を取得。
 * request: getApiV2PreprocessingsById
 * url: getApiV2PreprocessingsByIdURL
 * method: getApiV2PreprocessingsById_TYPE
 * raw_url: getApiV2PreprocessingsById_RAW_URL
 * @param id - 前処理ID
 */
export const getApiV2PreprocessingsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2PreprocessingsById_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const getApiV2PreprocessingsById_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 前処理が実行済みの場合でも編集可能な項目のみ扱う
 * request: patchApiV2PreprocessingsById
 * url: patchApiV2PreprocessingsByIdURL
 * method: patchApiV2PreprocessingsById_TYPE
 * raw_url: patchApiV2PreprocessingsById_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param body - 変更内容
 */
export const patchApiV2PreprocessingsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const patchApiV2PreprocessingsById_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const patchApiV2PreprocessingsById_TYPE = function() {
  return 'patch'
}
export const patchApiV2PreprocessingsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全ての項目が対象だが、一度でも前処理が実行されていた場合、編集不可
 * request: putApiV2PreprocessingsById
 * url: putApiV2PreprocessingsByIdURL
 * method: putApiV2PreprocessingsById_TYPE
 * raw_url: putApiV2PreprocessingsById_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param body - 変更内容
 */
export const putApiV2PreprocessingsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2PreprocessingsById_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const putApiV2PreprocessingsById_TYPE = function() {
  return 'put'
}
export const putApiV2PreprocessingsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 一度でも前処理が実行されていた場合、削除不可
 * request: deleteApiV2PreprocessingsById
 * url: deleteApiV2PreprocessingsByIdURL
 * method: deleteApiV2PreprocessingsById_TYPE
 * raw_url: deleteApiV2PreprocessingsById_RAW_URL
 * @param id - 前処理ID
 */
export const deleteApiV2PreprocessingsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2PreprocessingsById_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const deleteApiV2PreprocessingsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2PreprocessingsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定した前処理の履歴情報を取得する。
 * request: getApiV2PreprocessingsByIdHistories
 * url: getApiV2PreprocessingsByIdHistoriesURL
 * method: getApiV2PreprocessingsByIdHistories_TYPE
 * raw_url: getApiV2PreprocessingsByIdHistories_RAW_URL
 * @param id - 前処理ID
 */
export const getApiV2PreprocessingsByIdHistories = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2PreprocessingsByIdHistories_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories'
}
export const getApiV2PreprocessingsByIdHistories_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsByIdHistoriesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたデータに対する前処理の履歴を取得。
 * request: getApiV2PreprocessingsByIdHistoriesByDataId
 * url: getApiV2PreprocessingsByIdHistoriesByDataIdURL
 * method: getApiV2PreprocessingsByIdHistoriesByDataId_TYPE
 * raw_url: getApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL
 * @param id - 前処理ID
 * @param dataId - データID
 */
export const getApiV2PreprocessingsByIdHistoriesByDataId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const getApiV2PreprocessingsByIdHistoriesByDataId_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 前処理実行用のコンテナなどは起動しない。ローカル環境など、KAMONOHASHI外で作成した前処理結果をアップロードする際に用いる。
作成された前処理履歴は実行中のステータスとなり、前処理結果の追加が可能な状態になる。
* request: postApiV2PreprocessingsByIdHistoriesByDataId
* url: postApiV2PreprocessingsByIdHistoriesByDataIdURL
* method: postApiV2PreprocessingsByIdHistoriesByDataId_TYPE
* raw_url: postApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const postApiV2PreprocessingsByIdHistoriesByDataId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const postApiV2PreprocessingsByIdHistoriesByDataId_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 前処理履歴を削除する。生成された前処理済みデータもまとめて削除する。
 * request: deleteApiV2PreprocessingsByIdHistoriesByDataId
 * url: deleteApiV2PreprocessingsByIdHistoriesByDataIdURL
 * method: deleteApiV2PreprocessingsByIdHistoriesByDataId_TYPE
 * raw_url: deleteApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const deleteApiV2PreprocessingsByIdHistoriesByDataId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2PreprocessingsByIdHistoriesByDataId_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const deleteApiV2PreprocessingsByIdHistoriesByDataId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2PreprocessingsByIdHistoriesByDataIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 前処理履歴のイベントを取得する
 * request: getApiV2PreprocessingsByIdHistoriesByDataIdEvents
 * url: getApiV2PreprocessingsByIdHistoriesByDataIdEventsURL
 * method: getApiV2PreprocessingsByIdHistoriesByDataIdEvents_TYPE
 * raw_url: getApiV2PreprocessingsByIdHistoriesByDataIdEvents_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const getApiV2PreprocessingsByIdHistoriesByDataIdEvents = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/events'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdEvents_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/events'
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdEvents_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdEventsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/events'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 前処理実行用のコンテナを起動する。
作成された前処理履歴は実行中のステータスとなり、前処理結果の追加が可能な状態になる。
* request: postApiV2PreprocessingsByIdRun
* url: postApiV2PreprocessingsByIdRunURL
* method: postApiV2PreprocessingsByIdRun_TYPE
* raw_url: postApiV2PreprocessingsByIdRun_RAW_URL
     * @param id - 前処理ID
     * @param body - 実行設定
*/
export const postApiV2PreprocessingsByIdRun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/run'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2PreprocessingsByIdRun_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/run'
}
export const postApiV2PreprocessingsByIdRun_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsByIdRunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/run'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 前処理履歴に出力データを追加する。
追加する対象の前処理履歴は実行中のステータスのみ許可される。
* request: postApiV2PreprocessingsByIdHistoriesByDataIdData
* url: postApiV2PreprocessingsByIdHistoriesByDataIdDataURL
* method: postApiV2PreprocessingsByIdHistoriesByDataIdData_TYPE
* raw_url: postApiV2PreprocessingsByIdHistoriesByDataIdData_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
     * @param body - データ情報
*/
export const postApiV2PreprocessingsByIdHistoriesByDataIdData = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/data'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdData_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/data'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdData_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdDataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/data'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 前処理履歴の登録を異常終了させる。
前処理履歴はエラーのステータスとなり、前処理結果の追加が不可能な状態になる。
* request: postApiV2PreprocessingsByIdHistoriesByDataIdHalt
* url: postApiV2PreprocessingsByIdHistoriesByDataIdHaltURL
* method: postApiV2PreprocessingsByIdHistoriesByDataIdHalt_TYPE
* raw_url: postApiV2PreprocessingsByIdHistoriesByDataIdHalt_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const postApiV2PreprocessingsByIdHistoriesByDataIdHalt = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/halt'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdHalt_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/halt'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdHalt_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdHaltURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/halt'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 前処理履歴の登録を完了する。
前処理履歴は完了のステータスとなり、前処理結果の追加が不可能な状態になる。
* request: postApiV2PreprocessingsByIdHistoriesByDataIdComplete
* url: postApiV2PreprocessingsByIdHistoriesByDataIdCompleteURL
* method: postApiV2PreprocessingsByIdHistoriesByDataIdComplete_TYPE
* raw_url: postApiV2PreprocessingsByIdHistoriesByDataIdComplete_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const postApiV2PreprocessingsByIdHistoriesByDataIdComplete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/complete'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdComplete_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/complete'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdComplete_TYPE = function() {
  return 'post'
}
export const postApiV2PreprocessingsByIdHistoriesByDataIdCompleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/complete'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 前処理履歴添付ファイルの一覧を取得する。
 * request: getApiV2PreprocessingsByIdHistoriesByDataIdFiles
 * url: getApiV2PreprocessingsByIdHistoriesByDataIdFilesURL
 * method: getApiV2PreprocessingsByIdHistoriesByDataIdFiles_TYPE
 * raw_url: getApiV2PreprocessingsByIdHistoriesByDataIdFiles_RAW_URL
 * @param id - 対象の前処理履歴ID
 * @param dataId - 入力データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const getApiV2PreprocessingsByIdHistoriesByDataIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['dataId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: dataId'))
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdFiles_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/files'
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdFiles_TYPE = function() {
  return 'get'
}
export const getApiV2PreprocessingsByIdHistoriesByDataIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/preprocessings/{id}/histories/{dataId}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{dataId}', `${parameters['dataId']}`)
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 登録済みのDockerレジストリ エンドポイント一覧を取得
 * request: getApiV2AdminRegistryEndpoints
 * url: getApiV2AdminRegistryEndpointsURL
 * method: getApiV2AdminRegistryEndpoints_TYPE
 * raw_url: getApiV2AdminRegistryEndpoints_RAW_URL
 */
export const getApiV2AdminRegistryEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminRegistryEndpoints_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints'
}
export const getApiV2AdminRegistryEndpoints_TYPE = function() {
  return 'get'
}
export const getApiV2AdminRegistryEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にDockerレジストリ エンドポイントを登録する
 * request: postApiV2AdminRegistryEndpoints
 * url: postApiV2AdminRegistryEndpointsURL
 * method: postApiV2AdminRegistryEndpoints_TYPE
 * raw_url: postApiV2AdminRegistryEndpoints_RAW_URL
 * @param body - 新規作成モデル
 */
export const postApiV2AdminRegistryEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminRegistryEndpoints_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints'
}
export const postApiV2AdminRegistryEndpoints_TYPE = function() {
  return 'post'
}
export const postApiV2AdminRegistryEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Dockerレジストリ種別一覧を取得
 * request: getApiV2AdminRegistryTypes
 * url: getApiV2AdminRegistryTypesURL
 * method: getApiV2AdminRegistryTypes_TYPE
 * raw_url: getApiV2AdminRegistryTypes_RAW_URL
 */
export const getApiV2AdminRegistryTypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/types'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminRegistryTypes_RAW_URL = function() {
  return '/api/v2/admin/registry/types'
}
export const getApiV2AdminRegistryTypes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminRegistryTypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/types'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのDockerレジストリ エンドポイント情報を取得
 * request: getApiV2AdminRegistryEndpointsById
 * url: getApiV2AdminRegistryEndpointsByIdURL
 * method: getApiV2AdminRegistryEndpointsById_TYPE
 * raw_url: getApiV2AdminRegistryEndpointsById_RAW_URL
 * @param id - レジストリID
 */
export const getApiV2AdminRegistryEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminRegistryEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const getApiV2AdminRegistryEndpointsById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminRegistryEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Dockerレジストリ エンドポイント情報の編集
 * request: putApiV2AdminRegistryEndpointsById
 * url: putApiV2AdminRegistryEndpointsByIdURL
 * method: putApiV2AdminRegistryEndpointsById_TYPE
 * raw_url: putApiV2AdminRegistryEndpointsById_RAW_URL
 * @param id - 編集対象レジストリID
 * @param body - 編集モデル
 */
export const putApiV2AdminRegistryEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminRegistryEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const putApiV2AdminRegistryEndpointsById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminRegistryEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Dockerレジストリ エンドポイント情報の削除
 * request: deleteApiV2AdminRegistryEndpointsById
 * url: deleteApiV2AdminRegistryEndpointsByIdURL
 * method: deleteApiV2AdminRegistryEndpointsById_TYPE
 * raw_url: deleteApiV2AdminRegistryEndpointsById_RAW_URL
 * @param id - 削除対象レジストリID
 */
export const deleteApiV2AdminRegistryEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminRegistryEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const deleteApiV2AdminRegistryEndpointsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminRegistryEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/registry/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント管理者が選択可能な登録済みのDockerレジストリ エンドポイント一覧を取得
 * request: getApiV2TenantRegistryEndpoints
 * url: getApiV2TenantRegistryEndpointsURL
 * method: getApiV2TenantRegistryEndpoints_TYPE
 * raw_url: getApiV2TenantRegistryEndpoints_RAW_URL
 */
export const getApiV2TenantRegistryEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/registry/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantRegistryEndpoints_RAW_URL = function() {
  return '/api/v2/tenant/registry/endpoints'
}
export const getApiV2TenantRegistryEndpoints_TYPE = function() {
  return 'get'
}
export const getApiV2TenantRegistryEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/registry/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * レジストリに存在する全イメージの取得
 * request: getApiV2RegistriesByRegistryIdImages
 * url: getApiV2RegistriesByRegistryIdImagesURL
 * method: getApiV2RegistriesByRegistryIdImages_TYPE
 * raw_url: getApiV2RegistriesByRegistryIdImages_RAW_URL
 * @param registryId - レジストリID
 */
export const getApiV2RegistriesByRegistryIdImages = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/registries/{registryId}/images'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  if (parameters['registryId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: registryId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2RegistriesByRegistryIdImages_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images'
}
export const getApiV2RegistriesByRegistryIdImages_TYPE = function() {
  return 'get'
}
export const getApiV2RegistriesByRegistryIdImagesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/registries/{registryId}/images'
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * イメージ名に対応するタグ一覧を取得
 * request: getApiV2RegistriesByRegistryIdImagesByImageTags
 * url: getApiV2RegistriesByRegistryIdImagesByImageTagsURL
 * method: getApiV2RegistriesByRegistryIdImagesByImageTags_TYPE
 * raw_url: getApiV2RegistriesByRegistryIdImagesByImageTags_RAW_URL
 * @param registryId - レジストリID
 * @param image - イメージ名
 */
export const getApiV2RegistriesByRegistryIdImagesByImageTags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/registries/{registryId}/images/{image}/tags'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  if (parameters['registryId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: registryId'))
  }
  path = path.replace('{image}', `${parameters['image']}`)
  if (parameters['image'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: image'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2RegistriesByRegistryIdImagesByImageTags_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images/{image}/tags'
}
export const getApiV2RegistriesByRegistryIdImagesByImageTags_TYPE = function() {
  return 'get'
}
export const getApiV2RegistriesByRegistryIdImagesByImageTagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/registries/{registryId}/images/{image}/tags'
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  path = path.replace('{image}', `${parameters['image']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 階層化されたURLを吸収するためのダミーAPI。
製品版のSwaggerからは削除する。
* request: getApiV2RegistriesByRegistryIdImagesBySegments
* url: getApiV2RegistriesByRegistryIdImagesBySegmentsURL
* method: getApiV2RegistriesByRegistryIdImagesBySegments_TYPE
* raw_url: getApiV2RegistriesByRegistryIdImagesBySegments_RAW_URL
     * @param registryId - 
     * @param segments - 
*/
export const getApiV2RegistriesByRegistryIdImagesBySegments = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/registries/{registryId}/images/{segments}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  if (parameters['registryId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: registryId'))
  }
  path = path.replace('{segments}', `${parameters['segments']}`)
  if (parameters['segments'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: segments'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2RegistriesByRegistryIdImagesBySegments_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images/{segments}'
}
export const getApiV2RegistriesByRegistryIdImagesBySegments_TYPE = function() {
  return 'get'
}
export const getApiV2RegistriesByRegistryIdImagesBySegmentsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/registries/{registryId}/images/{segments}'
  path = path.replace('{registryId}', `${parameters['registryId']}`)
  path = path.replace('{segments}', `${parameters['segments']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ノード単位のリソースデータを取得する
 * request: getApiV2AdminResourceNodes
 * url: getApiV2AdminResourceNodesURL
 * method: getApiV2AdminResourceNodes_TYPE
 * raw_url: getApiV2AdminResourceNodes_RAW_URL
 */
export const getApiV2AdminResourceNodes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceNodes_RAW_URL = function() {
  return '/api/v2/admin/resource/nodes'
}
export const getApiV2AdminResourceNodes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceNodesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/nodes'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント単位のリソースデータを取得する
 * request: getApiV2AdminResourceTenants
 * url: getApiV2AdminResourceTenantsURL
 * method: getApiV2AdminResourceTenants_TYPE
 * raw_url: getApiV2AdminResourceTenants_RAW_URL
 */
export const getApiV2AdminResourceTenants = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/tenants'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceTenants_RAW_URL = function() {
  return '/api/v2/admin/resource/tenants'
}
export const getApiV2AdminResourceTenants_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceTenantsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/tenants'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 起動中のコンテナ一覧を取得する
 * request: getApiV2AdminResourceContainers
 * url: getApiV2AdminResourceContainersURL
 * method: getApiV2AdminResourceContainers_TYPE
 * raw_url: getApiV2AdminResourceContainers_RAW_URL
 */
export const getApiV2AdminResourceContainers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/containers'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceContainers_RAW_URL = function() {
  return '/api/v2/admin/resource/containers'
}
export const getApiV2AdminResourceContainers_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceContainersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/containers'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * コンテナ種別一覧を取得
 * request: getApiV2AdminResourceContainerTypes
 * url: getApiV2AdminResourceContainerTypesURL
 * method: getApiV2AdminResourceContainerTypes_TYPE
 * raw_url: getApiV2AdminResourceContainerTypes_RAW_URL
 */
export const getApiV2AdminResourceContainerTypes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/container-types'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceContainerTypes_RAW_URL = function() {
  return '/api/v2/admin/resource/container-types'
}
export const getApiV2AdminResourceContainerTypes_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceContainerTypesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/container-types'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナのリソースデータを取得する
 * request: getApiV2AdminResourceContainersByTenantIdByName
 * url: getApiV2AdminResourceContainersByTenantIdByNameURL
 * method: getApiV2AdminResourceContainersByTenantIdByName_TYPE
 * raw_url: getApiV2AdminResourceContainersByTenantIdByName_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const getApiV2AdminResourceContainersByTenantIdByName = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceContainersByTenantIdByName_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}'
}
export const getApiV2AdminResourceContainersByTenantIdByName_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceContainersByTenantIdByNameURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナを削除する
 * request: deleteApiV2AdminResourceContainersByTenantIdByName
 * url: deleteApiV2AdminResourceContainersByTenantIdByNameURL
 * method: deleteApiV2AdminResourceContainersByTenantIdByName_TYPE
 * raw_url: deleteApiV2AdminResourceContainersByTenantIdByName_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const deleteApiV2AdminResourceContainersByTenantIdByName = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminResourceContainersByTenantIdByName_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}'
}
export const deleteApiV2AdminResourceContainersByTenantIdByName_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminResourceContainersByTenantIdByNameURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナのログを取得する
 * request: getApiV2AdminResourceContainersByTenantIdByNameLog
 * url: getApiV2AdminResourceContainersByTenantIdByNameLogURL
 * method: getApiV2AdminResourceContainersByTenantIdByNameLog_TYPE
 * raw_url: getApiV2AdminResourceContainersByTenantIdByNameLog_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const getApiV2AdminResourceContainersByTenantIdByNameLog = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}/log'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceContainersByTenantIdByNameLog_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}/log'
}
export const getApiV2AdminResourceContainersByTenantIdByNameLog_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceContainersByTenantIdByNameLogURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}/log'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナのイベントを取得する
 * request: getApiV2AdminResourceContainersByTenantIdByNameEvents
 * url: getApiV2AdminResourceContainersByTenantIdByNameEventsURL
 * method: getApiV2AdminResourceContainersByTenantIdByNameEvents_TYPE
 * raw_url: getApiV2AdminResourceContainersByTenantIdByNameEvents_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const getApiV2AdminResourceContainersByTenantIdByNameEvents = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}/events'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceContainersByTenantIdByNameEvents_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}/events'
}
export const getApiV2AdminResourceContainersByTenantIdByNameEvents_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceContainersByTenantIdByNameEventsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/containers/{tenantId}/{name}/events'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 特定テナント向けに起動中のコンテナ一覧を取得する
 * request: getApiV2TenantResourceContainers
 * url: getApiV2TenantResourceContainersURL
 * method: getApiV2TenantResourceContainers_TYPE
 * raw_url: getApiV2TenantResourceContainers_RAW_URL
 */
export const getApiV2TenantResourceContainers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/resource/containers'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantResourceContainers_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers'
}
export const getApiV2TenantResourceContainers_TYPE = function() {
  return 'get'
}
export const getApiV2TenantResourceContainersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/resource/containers'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 特定テナントに紐づくノード単位のリソースデータを取得する
 * request: getApiV2TenantResourceNodes
 * url: getApiV2TenantResourceNodesURL
 * method: getApiV2TenantResourceNodes_TYPE
 * raw_url: getApiV2TenantResourceNodes_RAW_URL
 */
export const getApiV2TenantResourceNodes = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/resource/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantResourceNodes_RAW_URL = function() {
  return '/api/v2/tenant/resource/nodes'
}
export const getApiV2TenantResourceNodes_TYPE = function() {
  return 'get'
}
export const getApiV2TenantResourceNodesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/resource/nodes'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナのリソースデータを取得する
 * request: getApiV2TenantResourceContainersByName
 * url: getApiV2TenantResourceContainersByNameURL
 * method: getApiV2TenantResourceContainersByName_TYPE
 * raw_url: getApiV2TenantResourceContainersByName_RAW_URL
 * @param name - コンテナ名
 */
export const getApiV2TenantResourceContainersByName = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/resource/containers/{name}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantResourceContainersByName_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}'
}
export const getApiV2TenantResourceContainersByName_TYPE = function() {
  return 'get'
}
export const getApiV2TenantResourceContainersByNameURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/resource/containers/{name}'
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナを削除する
 * request: deleteApiV2TenantResourceContainersByName
 * url: deleteApiV2TenantResourceContainersByNameURL
 * method: deleteApiV2TenantResourceContainersByName_TYPE
 * raw_url: deleteApiV2TenantResourceContainersByName_RAW_URL
 * @param name - 
 */
export const deleteApiV2TenantResourceContainersByName = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/resource/containers/{name}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TenantResourceContainersByName_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}'
}
export const deleteApiV2TenantResourceContainersByName_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TenantResourceContainersByNameURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/resource/containers/{name}'
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定コンテナのログを取得する
 * request: getApiV2TenantResourceContainersByNameLog
 * url: getApiV2TenantResourceContainersByNameLogURL
 * method: getApiV2TenantResourceContainersByNameLog_TYPE
 * raw_url: getApiV2TenantResourceContainersByNameLog_RAW_URL
 * @param name - コンテナ名
 */
export const getApiV2TenantResourceContainersByNameLog = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/resource/containers/{name}/log'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantResourceContainersByNameLog_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}/log'
}
export const getApiV2TenantResourceContainersByNameLog_TYPE = function() {
  return 'get'
}
export const getApiV2TenantResourceContainersByNameLogURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/resource/containers/{name}/log'
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * コンテナリソース履歴のメタデータを取得する
 * request: getApiV2AdminResourceHistoriesContainersMetadata
 * url: getApiV2AdminResourceHistoriesContainersMetadataURL
 * method: getApiV2AdminResourceHistoriesContainersMetadata_TYPE
 * raw_url: getApiV2AdminResourceHistoriesContainersMetadata_RAW_URL
 */
export const getApiV2AdminResourceHistoriesContainersMetadata = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/containers/metadata'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceHistoriesContainersMetadata_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/containers/metadata'
}
export const getApiV2AdminResourceHistoriesContainersMetadata_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceHistoriesContainersMetadataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/containers/metadata'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * コンテナリソース履歴のデータを取得する
 * request: getApiV2AdminResourceHistoriesContainersData
 * url: getApiV2AdminResourceHistoriesContainersDataURL
 * method: getApiV2AdminResourceHistoriesContainersData_TYPE
 * raw_url: getApiV2AdminResourceHistoriesContainersData_RAW_URL
 * @param startDate - 開始日
 * @param endDate - 終了日
 * @param withHeader - ヘッダ情報を付与するか
 */
export const getApiV2AdminResourceHistoriesContainersData = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/containers/data'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['startDate'] !== undefined) {
    queryParameters['startDate'] = parameters['startDate']
  }
  if (parameters['endDate'] !== undefined) {
    queryParameters['endDate'] = parameters['endDate']
  }
  if (parameters['withHeader'] !== undefined) {
    queryParameters['withHeader'] = parameters['withHeader']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceHistoriesContainersData_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/containers/data'
}
export const getApiV2AdminResourceHistoriesContainersData_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceHistoriesContainersDataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/containers/data'
  if (parameters['startDate'] !== undefined) {
    queryParameters['startDate'] = parameters['startDate']
  }
  if (parameters['endDate'] !== undefined) {
    queryParameters['endDate'] = parameters['endDate']
  }
  if (parameters['withHeader'] !== undefined) {
    queryParameters['withHeader'] = parameters['withHeader']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * コンテナリソース履歴を削除する
 * request: patchApiV2AdminResourceHistoriesContainers
 * url: patchApiV2AdminResourceHistoriesContainersURL
 * method: patchApiV2AdminResourceHistoriesContainers_TYPE
 * raw_url: patchApiV2AdminResourceHistoriesContainers_RAW_URL
 * @param body - 削除対象の入力モデル
 */
export const patchApiV2AdminResourceHistoriesContainers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/containers'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const patchApiV2AdminResourceHistoriesContainers_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/containers'
}
export const patchApiV2AdminResourceHistoriesContainers_TYPE = function() {
  return 'patch'
}
export const patchApiV2AdminResourceHistoriesContainersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/containers'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ジョブ実行履歴のメタデータを取得する
 * request: getApiV2AdminResourceHistoriesJobsMetadata
 * url: getApiV2AdminResourceHistoriesJobsMetadataURL
 * method: getApiV2AdminResourceHistoriesJobsMetadata_TYPE
 * raw_url: getApiV2AdminResourceHistoriesJobsMetadata_RAW_URL
 */
export const getApiV2AdminResourceHistoriesJobsMetadata = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/jobs/metadata'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceHistoriesJobsMetadata_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/jobs/metadata'
}
export const getApiV2AdminResourceHistoriesJobsMetadata_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceHistoriesJobsMetadataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/jobs/metadata'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ジョブ実行履歴のデータを取得する
 * request: getApiV2AdminResourceHistoriesJobsData
 * url: getApiV2AdminResourceHistoriesJobsDataURL
 * method: getApiV2AdminResourceHistoriesJobsData_TYPE
 * raw_url: getApiV2AdminResourceHistoriesJobsData_RAW_URL
 * @param startDate - 開始日
 * @param endDate - 終了日
 * @param withHeader - ヘッダ情報を付与するか
 */
export const getApiV2AdminResourceHistoriesJobsData = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/jobs/data'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['startDate'] !== undefined) {
    queryParameters['startDate'] = parameters['startDate']
  }
  if (parameters['endDate'] !== undefined) {
    queryParameters['endDate'] = parameters['endDate']
  }
  if (parameters['withHeader'] !== undefined) {
    queryParameters['withHeader'] = parameters['withHeader']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminResourceHistoriesJobsData_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/jobs/data'
}
export const getApiV2AdminResourceHistoriesJobsData_TYPE = function() {
  return 'get'
}
export const getApiV2AdminResourceHistoriesJobsDataURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/jobs/data'
  if (parameters['startDate'] !== undefined) {
    queryParameters['startDate'] = parameters['startDate']
  }
  if (parameters['endDate'] !== undefined) {
    queryParameters['endDate'] = parameters['endDate']
  }
  if (parameters['withHeader'] !== undefined) {
    queryParameters['withHeader'] = parameters['withHeader']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ジョブ実行履歴を削除する
 * request: patchApiV2AdminResourceHistoriesJobs
 * url: patchApiV2AdminResourceHistoriesJobsURL
 * method: patchApiV2AdminResourceHistoriesJobs_TYPE
 * raw_url: patchApiV2AdminResourceHistoriesJobs_RAW_URL
 * @param body - 削除対象の入力モデル
 */
export const patchApiV2AdminResourceHistoriesJobs = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/resource/histories/jobs'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const patchApiV2AdminResourceHistoriesJobs_RAW_URL = function() {
  return '/api/v2/admin/resource/histories/jobs'
}
export const patchApiV2AdminResourceHistoriesJobs_TYPE = function() {
  return 'patch'
}
export const patchApiV2AdminResourceHistoriesJobsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/resource/histories/jobs'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全ロール一覧を取得
 * request: getApiV2AdminRoles
 * url: getApiV2AdminRolesURL
 * method: getApiV2AdminRoles_TYPE
 * raw_url: getApiV2AdminRoles_RAW_URL
 */
export const getApiV2AdminRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminRoles_RAW_URL = function() {
  return '/api/v2/admin/roles'
}
export const getApiV2AdminRoles_TYPE = function() {
  return 'get'
}
export const getApiV2AdminRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/roles'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にロールを登録する
 * request: postApiV2AdminRoles
 * url: postApiV2AdminRolesURL
 * method: postApiV2AdminRoles_TYPE
 * raw_url: postApiV2AdminRoles_RAW_URL
 * @param body - 
 */
export const postApiV2AdminRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminRoles_RAW_URL = function() {
  return '/api/v2/admin/roles'
}
export const postApiV2AdminRoles_TYPE = function() {
  return 'post'
}
export const postApiV2AdminRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/roles'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* ユーザグループ管理画面から参照する。
テナント共通ロールが対象。（テナント用カスタムロールは対象外）
* request: getApiV2AdminTenantCommonRoles
* url: getApiV2AdminTenantCommonRolesURL
* method: getApiV2AdminTenantCommonRoles_TYPE
* raw_url: getApiV2AdminTenantCommonRoles_RAW_URL
*/
export const getApiV2AdminTenantCommonRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenant-common-roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTenantCommonRoles_RAW_URL = function() {
  return '/api/v2/admin/tenant-common-roles'
}
export const getApiV2AdminTenantCommonRoles_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTenantCommonRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenant-common-roles'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのロール情報を取得。
 * request: getApiV2AdminRolesById
 * url: getApiV2AdminRolesByIdURL
 * method: getApiV2AdminRolesById_TYPE
 * raw_url: getApiV2AdminRolesById_RAW_URL
 * @param id - ロールID
 */
export const getApiV2AdminRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminRolesById_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const getApiV2AdminRolesById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ロール情報の編集
 * request: putApiV2AdminRolesById
 * url: putApiV2AdminRolesByIdURL
 * method: putApiV2AdminRolesById_TYPE
 * raw_url: putApiV2AdminRolesById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminRolesById_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const putApiV2AdminRolesById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ロールを削除する。
 * request: deleteApiV2AdminRolesById
 * url: deleteApiV2AdminRolesByIdURL
 * method: deleteApiV2AdminRolesById_TYPE
 * raw_url: deleteApiV2AdminRolesById_RAW_URL
 * @param id - 
 */
export const deleteApiV2AdminRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminRolesById_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const deleteApiV2AdminRolesById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * システムロール以外の共通ロールと、テナント用カスタムロールが対象。
 * request: getApiV2TenantRoles
 * url: getApiV2TenantRolesURL
 * method: getApiV2TenantRoles_TYPE
 * raw_url: getApiV2TenantRoles_RAW_URL
 */
export const getApiV2TenantRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantRoles_RAW_URL = function() {
  return '/api/v2/tenant/roles'
}
export const getApiV2TenantRoles_TYPE = function() {
  return 'get'
}
export const getApiV2TenantRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/roles'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にロールを登録する
 * request: postApiV2TenantRoles
 * url: postApiV2TenantRolesURL
 * method: postApiV2TenantRoles_TYPE
 * raw_url: postApiV2TenantRoles_RAW_URL
 * @param body - 
 */
export const postApiV2TenantRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TenantRoles_RAW_URL = function() {
  return '/api/v2/tenant/roles'
}
export const postApiV2TenantRoles_TYPE = function() {
  return 'post'
}
export const postApiV2TenantRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/roles'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのテナント用カスタムロール情報を取得。
 * request: getApiV2TenantRolesById
 * url: getApiV2TenantRolesByIdURL
 * method: getApiV2TenantRolesById_TYPE
 * raw_url: getApiV2TenantRolesById_RAW_URL
 * @param id - ロールID
 */
export const getApiV2TenantRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantRolesById_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const getApiV2TenantRolesById_TYPE = function() {
  return 'get'
}
export const getApiV2TenantRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント用カスタムロール情報の編集
 * request: putApiV2TenantRolesById
 * url: putApiV2TenantRolesByIdURL
 * method: putApiV2TenantRolesById_TYPE
 * raw_url: putApiV2TenantRolesById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2TenantRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2TenantRolesById_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const putApiV2TenantRolesById_TYPE = function() {
  return 'put'
}
export const putApiV2TenantRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント用カスタムロールを削除する。
 * request: deleteApiV2TenantRolesById
 * url: deleteApiV2TenantRolesByIdURL
 * method: deleteApiV2TenantRolesById_TYPE
 * raw_url: deleteApiV2TenantRolesById_RAW_URL
 * @param id - 
 */
export const deleteApiV2TenantRolesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TenantRolesById_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const deleteApiV2TenantRolesById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TenantRolesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/roles/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 登録済みのStorageエンドポイント一覧を取得
 * request: getApiV2AdminStorageEndpoints
 * url: getApiV2AdminStorageEndpointsURL
 * method: getApiV2AdminStorageEndpoints_TYPE
 * raw_url: getApiV2AdminStorageEndpoints_RAW_URL
 */
export const getApiV2AdminStorageEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminStorageEndpoints_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints'
}
export const getApiV2AdminStorageEndpoints_TYPE = function() {
  return 'get'
}
export const getApiV2AdminStorageEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/storage/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にStorageエンドポイントを登録する
 * request: postApiV2AdminStorageEndpoints
 * url: postApiV2AdminStorageEndpointsURL
 * method: postApiV2AdminStorageEndpoints_TYPE
 * raw_url: postApiV2AdminStorageEndpoints_RAW_URL
 * @param body - 
 */
export const postApiV2AdminStorageEndpoints = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminStorageEndpoints_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints'
}
export const postApiV2AdminStorageEndpoints_TYPE = function() {
  return 'post'
}
export const postApiV2AdminStorageEndpointsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/storage/endpoints'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのStorageエンドポイント情報を取得。
 * request: getApiV2AdminStorageEndpointsById
 * url: getApiV2AdminStorageEndpointsByIdURL
 * method: getApiV2AdminStorageEndpointsById_TYPE
 * raw_url: getApiV2AdminStorageEndpointsById_RAW_URL
 * @param id - StorageエンドポイントID
 */
export const getApiV2AdminStorageEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminStorageEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const getApiV2AdminStorageEndpointsById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminStorageEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/storage/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Storageエンドポイント情報の編集
 * request: putApiV2AdminStorageEndpointsById
 * url: putApiV2AdminStorageEndpointsByIdURL
 * method: putApiV2AdminStorageEndpointsById_TYPE
 * raw_url: putApiV2AdminStorageEndpointsById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminStorageEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminStorageEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const putApiV2AdminStorageEndpointsById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminStorageEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/storage/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * Storageエンドポイント情報の削除
 * request: deleteApiV2AdminStorageEndpointsById
 * url: deleteApiV2AdminStorageEndpointsByIdURL
 * method: deleteApiV2AdminStorageEndpointsById_TYPE
 * raw_url: deleteApiV2AdminStorageEndpointsById_RAW_URL
 * @param id - 
 */
export const deleteApiV2AdminStorageEndpointsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminStorageEndpointsById_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const deleteApiV2AdminStorageEndpointsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminStorageEndpointsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/storage/endpoints/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 分割アップロードを行うためのパラメータを取得する
 * request: getApiV2UploadParameter
 * url: getApiV2UploadParameterURL
 * method: getApiV2UploadParameter_TYPE
 * raw_url: getApiV2UploadParameter_RAW_URL
 * @param fileName - ファイル名
 * @param partSum - 分割数
 * @param type - リソース名
 */
export const getApiV2UploadParameter = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/upload/parameter'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['fileName'] !== undefined) {
    queryParameters['fileName'] = parameters['fileName']
  }
  if (parameters['fileName'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: fileName'))
  }
  if (parameters['partSum'] !== undefined) {
    queryParameters['partSum'] = parameters['partSum']
  }
  if (parameters['partSum'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: partSum'))
  }
  if (parameters['type'] !== undefined) {
    queryParameters['type'] = parameters['type']
  }
  if (parameters['type'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: type'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2UploadParameter_RAW_URL = function() {
  return '/api/v2/upload/parameter'
}
export const getApiV2UploadParameter_TYPE = function() {
  return 'get'
}
export const getApiV2UploadParameterURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/upload/parameter'
  if (parameters['fileName'] !== undefined) {
    queryParameters['fileName'] = parameters['fileName']
  }
  if (parameters['partSum'] !== undefined) {
    queryParameters['partSum'] = parameters['partSum']
  }
  if (parameters['type'] !== undefined) {
    queryParameters['type'] = parameters['type']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 分割アップロードの処理を完了する
 * request: postApiV2UploadComplete
 * url: postApiV2UploadCompleteURL
 * method: postApiV2UploadComplete_TYPE
 * raw_url: postApiV2UploadComplete_RAW_URL
 * @param body - 
 */
export const postApiV2UploadComplete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/upload/complete'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2UploadComplete_RAW_URL = function() {
  return '/api/v2/upload/complete'
}
export const postApiV2UploadComplete_TYPE = function() {
  return 'post'
}
export const postApiV2UploadCompleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/upload/complete'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ダウンロード用の一時署名URLを取得する
 * request: getApiV2DownloadUrl
 * url: getApiV2DownloadUrlURL
 * method: getApiV2DownloadUrl_TYPE
 * raw_url: getApiV2DownloadUrl_RAW_URL
 * @param type - リソース種別
 * @param storedPath - 保存先パス
 * @param fileName - ダウンロード時に書き換えるファイル名
 * @param secure - HTTPS化するか
 */
export const getApiV2DownloadUrl = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/download/url'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['type'] !== undefined) {
    queryParameters['type'] = parameters['type']
  }
  if (parameters['storedPath'] !== undefined) {
    queryParameters['storedPath'] = parameters['storedPath']
  }
  if (parameters['fileName'] !== undefined) {
    queryParameters['fileName'] = parameters['fileName']
  }
  if (parameters['secure'] !== undefined) {
    queryParameters['secure'] = parameters['secure']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2DownloadUrl_RAW_URL = function() {
  return '/api/v2/download/url'
}
export const getApiV2DownloadUrl_TYPE = function() {
  return 'get'
}
export const getApiV2DownloadUrlURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/download/url'
  if (parameters['type'] !== undefined) {
    queryParameters['type'] = parameters['type']
  }
  if (parameters['storedPath'] !== undefined) {
    queryParameters['storedPath'] = parameters['storedPath']
  }
  if (parameters['fileName'] !== undefined) {
    queryParameters['fileName'] = parameters['fileName']
  }
  if (parameters['secure'] !== undefined) {
    queryParameters['secure'] = parameters['secure']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントに有効なテンプレート一覧を取得する
 * request: getApiV2TenantTemplates
 * url: getApiV2TenantTemplatesURL
 * method: getApiV2TenantTemplates_TYPE
 * raw_url: getApiV2TenantTemplates_RAW_URL
 * @param withTotal - 
 */
export const getApiV2TenantTemplates = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/templates'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantTemplates_RAW_URL = function() {
  return '/api/v2/tenant/templates'
}
export const getApiV2TenantTemplates_TYPE = function() {
  return 'get'
}
export const getApiV2TenantTemplatesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/templates'
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントで作成されたテンプレート一覧を取得する
 * request: getApiV2Templates
 * url: getApiV2TemplatesURL
 * method: getApiV2Templates_TYPE
 * raw_url: getApiV2Templates_RAW_URL
 * @param withTotal - 
 */
export const getApiV2Templates = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/templates'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Templates_RAW_URL = function() {
  return '/api/v2/templates'
}
export const getApiV2Templates_TYPE = function() {
  return 'get'
}
export const getApiV2TemplatesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/templates'
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートを作成する
 * request: postApiV2AdminTemplates
 * url: postApiV2AdminTemplatesURL
 * method: postApiV2AdminTemplates_TYPE
 * raw_url: postApiV2AdminTemplates_RAW_URL
 * @param body - 
 */
export const postApiV2AdminTemplates = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminTemplates_RAW_URL = function() {
  return '/api/v2/admin/templates'
}
export const postApiV2AdminTemplates_TYPE = function() {
  return 'post'
}
export const postApiV2AdminTemplatesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートを取得する
 * request: getApiV2AdminTemplatesById
 * url: getApiV2AdminTemplatesByIdURL
 * method: getApiV2AdminTemplatesById_TYPE
 * raw_url: getApiV2AdminTemplatesById_RAW_URL
 * @param id - テンプレートID
 */
export const getApiV2AdminTemplatesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTemplatesById_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const getApiV2AdminTemplatesById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTemplatesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートを編集する
 * request: putApiV2AdminTemplatesById
 * url: putApiV2AdminTemplatesByIdURL
 * method: putApiV2AdminTemplatesById_TYPE
 * raw_url: putApiV2AdminTemplatesById_RAW_URL
 * @param id - テンプレートID
 * @param body - 編集内容
 */
export const putApiV2AdminTemplatesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminTemplatesById_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const putApiV2AdminTemplatesById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminTemplatesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートを削除する
 * request: deleteApiV2AdminTemplatesById
 * url: deleteApiV2AdminTemplatesByIdURL
 * method: deleteApiV2AdminTemplatesById_TYPE
 * raw_url: deleteApiV2AdminTemplatesById_RAW_URL
 * @param id - テンプレートID
 */
export const deleteApiV2AdminTemplatesById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminTemplatesById_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const deleteApiV2AdminTemplatesById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminTemplatesByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートバージョンを作成する
 * request: postApiV2AdminTemplatesByIdVersions
 * url: postApiV2AdminTemplatesByIdVersionsURL
 * method: postApiV2AdminTemplatesByIdVersions_TYPE
 * raw_url: postApiV2AdminTemplatesByIdVersions_RAW_URL
 * @param id - テンプレートID
 * @param body - 作成内容
 */
export const postApiV2AdminTemplatesByIdVersions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}/versions'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminTemplatesByIdVersions_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions'
}
export const postApiV2AdminTemplatesByIdVersions_TYPE = function() {
  return 'post'
}
export const postApiV2AdminTemplatesByIdVersionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}/versions'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートバージョン一覧を取得する
 * request: getApiV2AdminTemplatesByIdVersions
 * url: getApiV2AdminTemplatesByIdVersionsURL
 * method: getApiV2AdminTemplatesByIdVersions_TYPE
 * raw_url: getApiV2AdminTemplatesByIdVersions_RAW_URL
 * @param id - テンプレートID
 */
export const getApiV2AdminTemplatesByIdVersions = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}/versions'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTemplatesByIdVersions_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions'
}
export const getApiV2AdminTemplatesByIdVersions_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTemplatesByIdVersionsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}/versions'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートバージョンを取得する
 * request: getApiV2AdminTemplatesByIdVersionsByVersionId
 * url: getApiV2AdminTemplatesByIdVersionsByVersionIdURL
 * method: getApiV2AdminTemplatesByIdVersionsByVersionId_TYPE
 * raw_url: getApiV2AdminTemplatesByIdVersionsByVersionId_RAW_URL
 * @param id - テンプレートID
 * @param versionId - テンプレートバージョンID
 */
export const getApiV2AdminTemplatesByIdVersionsByVersionId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}/versions/{versionId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters['versionId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: versionId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTemplatesByIdVersionsByVersionId_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions/{versionId}'
}
export const getApiV2AdminTemplatesByIdVersionsByVersionId_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTemplatesByIdVersionsByVersionIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}/versions/{versionId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テンプレートバージョンを削除する
 * request: deleteApiV2AdminTemplatesByIdVersionsByVersionId
 * url: deleteApiV2AdminTemplatesByIdVersionsByVersionIdURL
 * method: deleteApiV2AdminTemplatesByIdVersionsByVersionId_TYPE
 * raw_url: deleteApiV2AdminTemplatesByIdVersionsByVersionId_RAW_URL
 * @param id - テンプレートID
 * @param versionId - テンプレートバージョンID
 */
export const deleteApiV2AdminTemplatesByIdVersionsByVersionId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates/{id}/versions/{versionId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters['versionId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: versionId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminTemplatesByIdVersionsByVersionId_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions/{versionId}'
}
export const deleteApiV2AdminTemplatesByIdVersionsByVersionId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminTemplatesByIdVersionsByVersionIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/templates/{id}/versions/{versionId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{versionId}', `${parameters['versionId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント一覧を取得
 * request: getApiV2AdminTenants
 * url: getApiV2AdminTenantsURL
 * method: getApiV2AdminTenants_TYPE
 * raw_url: getApiV2AdminTenants_RAW_URL
 */
export const getApiV2AdminTenants = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTenants_RAW_URL = function() {
  return '/api/v2/admin/tenants'
}
export const getApiV2AdminTenants_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTenantsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenants'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にテナントを登録する
 * request: postApiV2AdminTenants
 * url: postApiV2AdminTenantsURL
 * method: postApiV2AdminTenants_TYPE
 * raw_url: postApiV2AdminTenants_RAW_URL
 * @param body - 登録内容
 */
export const postApiV2AdminTenants = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminTenants_RAW_URL = function() {
  return '/api/v2/admin/tenants'
}
export const postApiV2AdminTenants_TYPE = function() {
  return 'post'
}
export const postApiV2AdminTenantsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenants'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのテナント情報を取得。
 * request: getApiV2AdminTenantsById
 * url: getApiV2AdminTenantsByIdURL
 * method: getApiV2AdminTenantsById_TYPE
 * raw_url: getApiV2AdminTenantsById_RAW_URL
 * @param id - テナントID
 */
export const getApiV2AdminTenantsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminTenantsById_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const getApiV2AdminTenantsById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminTenantsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenants/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナントを削除する。(他のユーザが未ログイン状態の時間帯で実施するのが望ましい)
 * request: deleteApiV2AdminTenantsById
 * url: deleteApiV2AdminTenantsByIdURL
 * method: deleteApiV2AdminTenantsById_TYPE
 * raw_url: deleteApiV2AdminTenantsById_RAW_URL
 * @param id - テナントID
 */
export const deleteApiV2AdminTenantsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminTenantsById_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const deleteApiV2AdminTenantsById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminTenantsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenants/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント情報の編集
 * request: putApiV2AdminTenantsById
 * url: putApiV2AdminTenantsByIdURL
 * method: putApiV2AdminTenantsById_TYPE
 * raw_url: putApiV2AdminTenantsById_RAW_URL
 * @param id - テナントID
 * @param body - 変更内容
 */
export const putApiV2AdminTenantsById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminTenantsById_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const putApiV2AdminTenantsById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminTenantsByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/tenants/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントの情報を取得。
 * request: getApiV2Tenant
 * url: getApiV2TenantURL
 * method: getApiV2Tenant_TYPE
 * raw_url: getApiV2Tenant_RAW_URL
 */
export const getApiV2Tenant = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Tenant_RAW_URL = function() {
  return '/api/v2/tenant'
}
export const getApiV2Tenant_TYPE = function() {
  return 'get'
}
export const getApiV2TenantURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 接続中のテナントの情報の編集
 * request: putApiV2Tenant
 * url: putApiV2TenantURL
 * method: putApiV2Tenant_TYPE
 * raw_url: putApiV2Tenant_RAW_URL
 * @param body - 変更内容
 */
export const putApiV2Tenant = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2Tenant_RAW_URL = function() {
  return '/api/v2/tenant'
}
export const putApiV2Tenant_TYPE = function() {
  return 'put'
}
export const putApiV2TenantURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 全学習履歴のIDと名前を取得
 * request: getApiV2TrainingSimple
 * url: getApiV2TrainingSimpleURL
 * method: getApiV2TrainingSimple_TYPE
 * raw_url: getApiV2TrainingSimple_RAW_URL
 */
export const getApiV2TrainingSimple = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/simple'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingSimple_RAW_URL = function() {
  return '/api/v2/training/simple'
}
export const getApiV2TrainingSimple_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingSimpleURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/simple'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、全学習履歴を取得
* request: getApiV2Training
* url: getApiV2TrainingURL
* method: getApiV2Training_TYPE
* raw_url: getApiV2Training_RAW_URL
     * @param id - IDの検索条件。
比較文字列＋数値の形式。
     * @param name - 名前
     * @param parentId - 親学習ID
     * @param parentName - 親学習名
     * @param startedAt - 実行時刻の検索条件。
比較文字列＋時刻の形式。
e.g.（比較文字列は半角でOK）
"2018/01/01" → 2018/01/01 00:00:00 以降 ～ 2018/01/02 00:00:00 より前
"＞2018/01/01" → 2018/01/01 00:00:00 以降
"＜2018/01/01" → 2018/01/01 00:00:00 以前
     * @param startedBy - 実行者
     * @param dataSet - データセット名
     * @param memo - メモ
     * @param status - ステータス
     * @param entryPoint - 実行コマンド
     * @param tag - タグ
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2Training = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['parentId'] !== undefined) {
    queryParameters['ParentId'] = parameters['parentId']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['tag'] !== undefined) {
    queryParameters['tag'] = parameters['tag']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Training_RAW_URL = function() {
  return '/api/v2/training'
}
export const getApiV2Training_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training'
  if (parameters['id'] !== undefined) {
    queryParameters['Id'] = parameters['id']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['parentId'] !== undefined) {
    queryParameters['ParentId'] = parameters['parentId']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['startedAt'] !== undefined) {
    queryParameters['StartedAt'] = parameters['startedAt']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['tag'] !== undefined) {
    queryParameters['tag'] = parameters['tag']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 指定された条件でページングされた状態で、全学習履歴を取得。詳細検索で使用。
* request: getApiV2TrainingSearch
* url: getApiV2TrainingSearchURL
* method: getApiV2TrainingSearch_TYPE
* raw_url: getApiV2TrainingSearch_RAW_URL
     * @param idLower - IDの検索条件。
この数値以上のIDが検索される。
     * @param idUpper - IDの検索条件。
この数値以下のIDが検索される。
     * @param name - 名前
複数のワードが含まれる場合は","区切り
     * @param nameOr - 名前がor検索かand検索か
     * @param parentName - 親学習名
複数のワードが含まれる場合は","区切り
     * @param parentNameOr - 親学習名がor検索かand検索か
     * @param startedAtLower - 実行時刻の検索の期間の開始の条件。日付の形式。
"2018/01/01" → 2018/01/01 00:00:00 以降が検索される。
     * @param startedAtUpper - 実行時刻の検索の期間の終了の条件。日付の形式。
"2018/01/01" → 2018/01/01 23:59:59 以前が検索される。
     * @param startedBy - 実行者
複数のワードが含まれる場合は","区切り
     * @param startedByOr - 実行者の検索がor検索かand検索か
     * @param dataSet - データセット名
複数のワードが含まれる場合は","区切り
     * @param dataSetOr - データセット名がor検索かand検索か
     * @param memo - メモ
複数のワードが含まれる場合は","区切り
     * @param memoOr - メモがor検索かand検索か
     * @param status - ステータス
複数のワードが含まれる場合は","区切り
     * @param statusOr - ステータスがor検索かand検索か
     * @param entryPoint - 実行コマンド
複数のワードが含まれる場合は","区切り
     * @param entryPointOr - 実行コマンドがor検索かand検索か
     * @param tags - タグ
複数のワードが含まれる場合は","区切り
     * @param tagsOr - タグがor検索かand検索か
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const getApiV2TrainingSearch = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/search'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['idLower'] !== undefined) {
    queryParameters['IdLower'] = parameters['idLower']
  }
  if (parameters['idUpper'] !== undefined) {
    queryParameters['IdUpper'] = parameters['idUpper']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['nameOr'] !== undefined) {
    queryParameters['NameOr'] = parameters['nameOr']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['parentNameOr'] !== undefined) {
    queryParameters['ParentNameOr'] = parameters['parentNameOr']
  }
  if (parameters['startedAtLower'] !== undefined) {
    queryParameters['StartedAtLower'] = parameters['startedAtLower']
  }
  if (parameters['startedAtUpper'] !== undefined) {
    queryParameters['StartedAtUpper'] = parameters['startedAtUpper']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['startedByOr'] !== undefined) {
    queryParameters['StartedByOr'] = parameters['startedByOr']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['dataSetOr'] !== undefined) {
    queryParameters['DataSetOr'] = parameters['dataSetOr']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['memoOr'] !== undefined) {
    queryParameters['MemoOr'] = parameters['memoOr']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['statusOr'] !== undefined) {
    queryParameters['StatusOr'] = parameters['statusOr']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['entryPointOr'] !== undefined) {
    queryParameters['EntryPointOr'] = parameters['entryPointOr']
  }
  if (parameters['tags'] !== undefined) {
    queryParameters['Tags'] = parameters['tags']
  }
  if (parameters['tagsOr'] !== undefined) {
    queryParameters['TagsOr'] = parameters['tagsOr']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingSearch_RAW_URL = function() {
  return '/api/v2/training/search'
}
export const getApiV2TrainingSearch_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingSearchURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/search'
  if (parameters['idLower'] !== undefined) {
    queryParameters['IdLower'] = parameters['idLower']
  }
  if (parameters['idUpper'] !== undefined) {
    queryParameters['IdUpper'] = parameters['idUpper']
  }
  if (parameters['name'] !== undefined) {
    queryParameters['Name'] = parameters['name']
  }
  if (parameters['nameOr'] !== undefined) {
    queryParameters['NameOr'] = parameters['nameOr']
  }
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
  }
  if (parameters['parentNameOr'] !== undefined) {
    queryParameters['ParentNameOr'] = parameters['parentNameOr']
  }
  if (parameters['startedAtLower'] !== undefined) {
    queryParameters['StartedAtLower'] = parameters['startedAtLower']
  }
  if (parameters['startedAtUpper'] !== undefined) {
    queryParameters['StartedAtUpper'] = parameters['startedAtUpper']
  }
  if (parameters['startedBy'] !== undefined) {
    queryParameters['StartedBy'] = parameters['startedBy']
  }
  if (parameters['startedByOr'] !== undefined) {
    queryParameters['StartedByOr'] = parameters['startedByOr']
  }
  if (parameters['dataSet'] !== undefined) {
    queryParameters['DataSet'] = parameters['dataSet']
  }
  if (parameters['dataSetOr'] !== undefined) {
    queryParameters['DataSetOr'] = parameters['dataSetOr']
  }
  if (parameters['memo'] !== undefined) {
    queryParameters['Memo'] = parameters['memo']
  }
  if (parameters['memoOr'] !== undefined) {
    queryParameters['MemoOr'] = parameters['memoOr']
  }
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters['statusOr'] !== undefined) {
    queryParameters['StatusOr'] = parameters['statusOr']
  }
  if (parameters['entryPoint'] !== undefined) {
    queryParameters['EntryPoint'] = parameters['entryPoint']
  }
  if (parameters['entryPointOr'] !== undefined) {
    queryParameters['EntryPointOr'] = parameters['entryPointOr']
  }
  if (parameters['tags'] !== undefined) {
    queryParameters['Tags'] = parameters['tags']
  }
  if (parameters['tagsOr'] !== undefined) {
    queryParameters['TagsOr'] = parameters['tagsOr']
  }
  if (parameters['perPage'] !== undefined) {
    queryParameters['perPage'] = parameters['perPage']
  }
  if (parameters['page'] !== undefined) {
    queryParameters['page'] = parameters['page']
  }
  if (parameters['withTotal'] !== undefined) {
    queryParameters['withTotal'] = parameters['withTotal']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * マウントする学習履歴を取得
 * request: getApiV2TrainingMount
 * url: getApiV2TrainingMountURL
 * method: getApiV2TrainingMount_TYPE
 * raw_url: getApiV2TrainingMount_RAW_URL
 * @param status - ステータス
 */
export const getApiV2TrainingMount = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/mount'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingMount_RAW_URL = function() {
  return '/api/v2/training/mount'
}
export const getApiV2TrainingMount_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingMountURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/mount'
  if (parameters['status'] !== undefined) {
    queryParameters['Status'] = parameters['status']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDの学習履歴の詳細情報を取得。
 * request: getApiV2TrainingById
 * url: getApiV2TrainingByIdURL
 * method: getApiV2TrainingById_TYPE
 * raw_url: getApiV2TrainingById_RAW_URL
 * @param id - 学習履歴ID
 */
export const getApiV2TrainingById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingById_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const getApiV2TrainingById_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴の編集
 * request: putApiV2TrainingById
 * url: putApiV2TrainingByIdURL
 * method: putApiV2TrainingById_TYPE
 * raw_url: putApiV2TrainingById_RAW_URL
 * @param id - 変更対象の学習履歴ID
 * @param body - 変更内容
 */
export const putApiV2TrainingById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2TrainingById_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const putApiV2TrainingById_TYPE = function() {
  return 'put'
}
export const putApiV2TrainingByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴を削除する。
 * request: deleteApiV2TrainingById
 * url: deleteApiV2TrainingByIdURL
 * method: deleteApiV2TrainingById_TYPE
 * raw_url: deleteApiV2TrainingById_RAW_URL
 * @param id - 学習履歴ID
 */
export const deleteApiV2TrainingById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TrainingById_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const deleteApiV2TrainingById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TrainingByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定された学習履歴のエラーイベントを取得します。
 * request: getApiV2TrainingByIdEvents
 * url: getApiV2TrainingByIdEventsURL
 * method: getApiV2TrainingByIdEvents_TYPE
 * raw_url: getApiV2TrainingByIdEvents_RAW_URL
 * @param id - 学習履歴ID
 */
export const getApiV2TrainingByIdEvents = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/events'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingByIdEvents_RAW_URL = function() {
  return '/api/v2/training/{id}/events'
}
export const getApiV2TrainingByIdEvents_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdEventsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/events'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規に学習を開始する
 * request: postApiV2TrainingRun
 * url: postApiV2TrainingRunURL
 * method: postApiV2TrainingRun_TYPE
 * raw_url: postApiV2TrainingRun_RAW_URL
 * @param body - 新規学習実行内容
 */
export const postApiV2TrainingRun = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingRun_RAW_URL = function() {
  return '/api/v2/training/run'
}
export const postApiV2TrainingRun_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingRunURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/run'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴添付ファイルを登録する。
 * request: postApiV2TrainingByIdFiles
 * url: postApiV2TrainingByIdFilesURL
 * method: postApiV2TrainingByIdFiles_TYPE
 * raw_url: postApiV2TrainingByIdFiles_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param body - 追加するファイル情報
 */
export const postApiV2TrainingByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingByIdFiles_RAW_URL = function() {
  return '/api/v2/training/{id}/files'
}
export const postApiV2TrainingByIdFiles_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴添付ファイルの一覧を取得する。
 * request: getApiV2TrainingByIdFiles
 * url: getApiV2TrainingByIdFilesURL
 * method: getApiV2TrainingByIdFiles_TYPE
 * raw_url: getApiV2TrainingByIdFiles_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const getApiV2TrainingByIdFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingByIdFiles_RAW_URL = function() {
  return '/api/v2/training/{id}/files'
}
export const getApiV2TrainingByIdFiles_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: getApiV2TrainingByIdContainerFiles
* url: getApiV2TrainingByIdContainerFilesURL
* method: getApiV2TrainingByIdContainerFiles_TYPE
* raw_url: getApiV2TrainingByIdContainerFiles_RAW_URL
     * @param id - 対象の学習履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const getApiV2TrainingByIdContainerFiles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/container-files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingByIdContainerFiles_RAW_URL = function() {
  return '/api/v2/training/{id}/container-files'
}
export const getApiV2TrainingByIdContainerFiles_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdContainerFilesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/container-files'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['path'] !== undefined) {
    queryParameters['path'] = parameters['path']
  }
  if (parameters['withUrl'] !== undefined) {
    queryParameters['withUrl'] = parameters['withUrl']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴添付ファイルを削除する
 * request: deleteApiV2TrainingByIdFilesByFileId
 * url: deleteApiV2TrainingByIdFilesByFileIdURL
 * method: deleteApiV2TrainingByIdFilesByFileId_TYPE
 * raw_url: deleteApiV2TrainingByIdFilesByFileId_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param fileId - 削除するファイルのID
 */
export const deleteApiV2TrainingByIdFilesByFileId = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/files/{fileId}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters['fileId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: fileId'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TrainingByIdFilesByFileId_RAW_URL = function() {
  return '/api/v2/training/{id}/files/{fileId}'
}
export const deleteApiV2TrainingByIdFilesByFileId_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TrainingByIdFilesByFileIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/files/{fileId}'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{fileId}', `${parameters['fileId']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習履歴添付ファイルのサイズ(Byte)を取得する
 * request: getApiV2TrainingByIdFilesByNameSize
 * url: getApiV2TrainingByIdFilesByNameSizeURL
 * method: getApiV2TrainingByIdFilesByNameSize_TYPE
 * raw_url: getApiV2TrainingByIdFilesByNameSize_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param name - 対象ファイル名
 */
export const getApiV2TrainingByIdFilesByNameSize = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/files/{name}/size'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters['name'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: name'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingByIdFilesByNameSize_RAW_URL = function() {
  return '/api/v2/training/{id}/files/{name}/size'
}
export const getApiV2TrainingByIdFilesByNameSize_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdFilesByNameSizeURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/files/{name}/size'
  path = path.replace('{id}', `${parameters['id']}`)
  path = path.replace('{name}', `${parameters['name']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したTensorBoardコンテナ情報を取得する
 * request: getApiV2TrainingByIdTensorboard
 * url: getApiV2TrainingByIdTensorboardURL
 * method: getApiV2TrainingByIdTensorboard_TYPE
 * raw_url: getApiV2TrainingByIdTensorboard_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const getApiV2TrainingByIdTensorboard = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/tensorboard'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingByIdTensorboard_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const getApiV2TrainingByIdTensorboard_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingByIdTensorboardURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/tensorboard'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定した学習のTensor Boardを立てる
 * request: putApiV2TrainingByIdTensorboard
 * url: putApiV2TrainingByIdTensorboardURL
 * method: putApiV2TrainingByIdTensorboard_TYPE
 * raw_url: putApiV2TrainingByIdTensorboard_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param body - 起動モデル
 */
export const putApiV2TrainingByIdTensorboard = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/tensorboard'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2TrainingByIdTensorboard_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const putApiV2TrainingByIdTensorboard_TYPE = function() {
  return 'put'
}
export const putApiV2TrainingByIdTensorboardURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/tensorboard'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定した学習のTensor Boardを削除する
 * request: deleteApiV2TrainingByIdTensorboard
 * url: deleteApiV2TrainingByIdTensorboardURL
 * method: deleteApiV2TrainingByIdTensorboard_TYPE
 * raw_url: deleteApiV2TrainingByIdTensorboard_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const deleteApiV2TrainingByIdTensorboard = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/tensorboard'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TrainingByIdTensorboard_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const deleteApiV2TrainingByIdTensorboard_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TrainingByIdTensorboardURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/tensorboard'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習を途中で強制終了させる。
 * request: postApiV2TrainingByIdHalt
 * url: postApiV2TrainingByIdHaltURL
 * method: postApiV2TrainingByIdHalt_TYPE
 * raw_url: postApiV2TrainingByIdHalt_RAW_URL
 * @param id - 学習履歴ID
 */
export const postApiV2TrainingByIdHalt = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/halt'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingByIdHalt_RAW_URL = function() {
  return '/api/v2/training/{id}/halt'
}
export const postApiV2TrainingByIdHalt_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingByIdHaltURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/halt'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
* 学習を途中で強制終了させる。
ユーザ自身がジョブを停止させた場合。
* request: postApiV2TrainingByIdUserCancel
* url: postApiV2TrainingByIdUserCancelURL
* method: postApiV2TrainingByIdUserCancel_TYPE
* raw_url: postApiV2TrainingByIdUserCancel_RAW_URL
     * @param id - 学習履歴ID
*/
export const postApiV2TrainingByIdUserCancel = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/user-cancel'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingByIdUserCancel_RAW_URL = function() {
  return '/api/v2/training/{id}/user-cancel'
}
export const postApiV2TrainingByIdUserCancel_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingByIdUserCancelURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/user-cancel'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 学習を正常終了させる。
 * request: postApiV2TrainingByIdComplete
 * url: postApiV2TrainingByIdCompleteURL
 * method: postApiV2TrainingByIdComplete_TYPE
 * raw_url: postApiV2TrainingByIdComplete_RAW_URL
 * @param id - 学習履歴ID
 */
export const postApiV2TrainingByIdComplete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/{id}/complete'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingByIdComplete_RAW_URL = function() {
  return '/api/v2/training/{id}/complete'
}
export const postApiV2TrainingByIdComplete_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingByIdCompleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/{id}/complete'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 選択中のテナントに登録されている学習管理で使用するタグを表示する
 * request: getApiV2TrainingTags
 * url: getApiV2TrainingTagsURL
 * method: getApiV2TrainingTags_TYPE
 * raw_url: getApiV2TrainingTags_RAW_URL
 */
export const getApiV2TrainingTags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/tags'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingTags_RAW_URL = function() {
  return '/api/v2/training/tags'
}
export const getApiV2TrainingTags_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingTagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/tags'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIdに対応したTagをつける。
 * request: postApiV2TrainingTags
 * url: postApiV2TrainingTagsURL
 * method: postApiV2TrainingTags_TYPE
 * raw_url: postApiV2TrainingTags_RAW_URL
 * @param body - 付与タグの入力モデル
 */
export const postApiV2TrainingTags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/tags'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingTags_RAW_URL = function() {
  return '/api/v2/training/tags'
}
export const postApiV2TrainingTags_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingTagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/tags'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したIdに対応したTagを消去する。
 * request: deleteApiV2TrainingTags
 * url: deleteApiV2TrainingTagsURL
 * method: deleteApiV2TrainingTags_TYPE
 * raw_url: deleteApiV2TrainingTags_RAW_URL
 * @param body - 削除タグの入力モデル
 */
export const deleteApiV2TrainingTags = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/tags'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TrainingTags_RAW_URL = function() {
  return '/api/v2/training/tags'
}
export const deleteApiV2TrainingTags_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TrainingTagsURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/tags'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 検索履歴の一覧を取得する
 * request: getApiV2TrainingSearchHistory
 * url: getApiV2TrainingSearchHistoryURL
 * method: getApiV2TrainingSearchHistory_TYPE
 * raw_url: getApiV2TrainingSearchHistory_RAW_URL
 */
export const getApiV2TrainingSearchHistory = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/search-history'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingSearchHistory_RAW_URL = function() {
  return '/api/v2/training/search-history'
}
export const getApiV2TrainingSearchHistory_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingSearchHistoryURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/search-history'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 検索履歴を保存する
 * request: postApiV2TrainingSearchHistory
 * url: postApiV2TrainingSearchHistoryURL
 * method: postApiV2TrainingSearchHistory_TYPE
 * raw_url: postApiV2TrainingSearchHistory_RAW_URL
 * @param body - 保存対象の検索履歴入力モデル
 */
export const postApiV2TrainingSearchHistory = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/search-history'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2TrainingSearchHistory_RAW_URL = function() {
  return '/api/v2/training/search-history'
}
export const postApiV2TrainingSearchHistory_TYPE = function() {
  return 'post'
}
export const postApiV2TrainingSearchHistoryURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/search-history'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 検索履歴を削除する
 * request: deleteApiV2TrainingSearchHistoryById
 * url: deleteApiV2TrainingSearchHistoryByIdURL
 * method: deleteApiV2TrainingSearchHistoryById_TYPE
 * raw_url: deleteApiV2TrainingSearchHistoryById_RAW_URL
 * @param id - 削除対象の検索履歴ID
 */
export const deleteApiV2TrainingSearchHistoryById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/search-history/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TrainingSearchHistoryById_RAW_URL = function() {
  return '/api/v2/training/search-history/{id}'
}
export const deleteApiV2TrainingSearchHistoryById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TrainingSearchHistoryByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/search-history/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 検索時の補完に使用する項目を出力する
 * request: getApiV2TrainingSearchFill
 * url: getApiV2TrainingSearchFillURL
 * method: getApiV2TrainingSearchFill_TYPE
 * raw_url: getApiV2TrainingSearchFill_RAW_URL
 */
export const getApiV2TrainingSearchFill = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/search/fill'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TrainingSearchFill_RAW_URL = function() {
  return '/api/v2/training/search/fill'
}
export const getApiV2TrainingSearchFill_TYPE = function() {
  return 'get'
}
export const getApiV2TrainingSearchFillURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/training/search/fill'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 管理者向けにユーザの一覧を取得する。
 * request: getApiV2AdminUsers
 * url: getApiV2AdminUsersURL
 * method: getApiV2AdminUsers_TYPE
 * raw_url: getApiV2AdminUsers_RAW_URL
 */
export const getApiV2AdminUsers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminUsers_RAW_URL = function() {
  return '/api/v2/admin/users'
}
export const getApiV2AdminUsers_TYPE = function() {
  return 'get'
}
export const getApiV2AdminUsersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ユーザをローカルアカウントとして新規追加する
 * request: postApiV2AdminUsers
 * url: postApiV2AdminUsersURL
 * method: postApiV2AdminUsers_TYPE
 * raw_url: postApiV2AdminUsers_RAW_URL
 * @param body - 
 */
export const postApiV2AdminUsers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminUsers_RAW_URL = function() {
  return '/api/v2/admin/users'
}
export const postApiV2AdminUsers_TYPE = function() {
  return 'post'
}
export const postApiV2AdminUsersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 管理者向けに指定したユーザの情報を取得する。
 * request: getApiV2AdminUsersById
 * url: getApiV2AdminUsersByIdURL
 * method: getApiV2AdminUsersById_TYPE
 * raw_url: getApiV2AdminUsersById_RAW_URL
 * @param id - 
 */
export const getApiV2AdminUsersById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminUsersById_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const getApiV2AdminUsersById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminUsersByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したユーザを削除する
 * request: deleteApiV2AdminUsersById
 * url: deleteApiV2AdminUsersByIdURL
 * method: deleteApiV2AdminUsersById_TYPE
 * raw_url: deleteApiV2AdminUsersById_RAW_URL
 * @param id - 
 */
export const deleteApiV2AdminUsersById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminUsersById_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const deleteApiV2AdminUsersById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminUsersByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したユーザを編集する
 * request: putApiV2AdminUsersById
 * url: putApiV2AdminUsersByIdURL
 * method: putApiV2AdminUsersById_TYPE
 * raw_url: putApiV2AdminUsersById_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminUsersById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminUsersById_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const putApiV2AdminUsersById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminUsersByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したユーザのパスワードを変更する
 * request: putApiV2AdminUsersByIdPassword
 * url: putApiV2AdminUsersByIdPasswordURL
 * method: putApiV2AdminUsersByIdPassword_TYPE
 * raw_url: putApiV2AdminUsersByIdPassword_RAW_URL
 * @param id - 
 * @param body - 
 */
export const putApiV2AdminUsersByIdPassword = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users/{id}/password'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminUsersByIdPassword_RAW_URL = function() {
  return '/api/v2/admin/users/{id}/password'
}
export const putApiV2AdminUsersByIdPassword_TYPE = function() {
  return 'put'
}
export const putApiV2AdminUsersByIdPasswordURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users/{id}/password'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * LDAPサーバに問い合わせを行い、各ユーザの権限を更新する
 * request: postApiV2AdminUsersSyncLdap
 * url: postApiV2AdminUsersSyncLdapURL
 * method: postApiV2AdminUsersSyncLdap_TYPE
 * raw_url: postApiV2AdminUsersSyncLdap_RAW_URL
 * @param body - LDAP認証情報入力モデル
 */
export const postApiV2AdminUsersSyncLdap = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users/sync-ldap'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminUsersSyncLdap_RAW_URL = function() {
  return '/api/v2/admin/users/sync-ldap'
}
export const postApiV2AdminUsersSyncLdap_TYPE = function() {
  return 'post'
}
export const postApiV2AdminUsersSyncLdapURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/users/sync-ldap'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント向けに、所属しているユーザの一覧を取得する。
 * request: getApiV2TenantUsers
 * url: getApiV2TenantUsersURL
 * method: getApiV2TenantUsers_TYPE
 * raw_url: getApiV2TenantUsers_RAW_URL
 */
export const getApiV2TenantUsers = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/users'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantUsers_RAW_URL = function() {
  return '/api/v2/tenant/users'
}
export const getApiV2TenantUsers_TYPE = function() {
  return 'get'
}
export const getApiV2TenantUsersURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/users'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * テナント向けに指定したユーザの情報を取得する。
 * request: getApiV2TenantUsersById
 * url: getApiV2TenantUsersByIdURL
 * method: getApiV2TenantUsersById_TYPE
 * raw_url: getApiV2TenantUsersById_RAW_URL
 * @param id - 
 */
export const getApiV2TenantUsersById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2TenantUsersById_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}'
}
export const getApiV2TenantUsersById_TYPE = function() {
  return 'get'
}
export const getApiV2TenantUsersByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/users/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したユーザを接続中のテナントから削除する
 * request: deleteApiV2TenantUsersById
 * url: deleteApiV2TenantUsersByIdURL
 * method: deleteApiV2TenantUsersById_TYPE
 * raw_url: deleteApiV2TenantUsersById_RAW_URL
 * @param id - ユーザID
 */
export const deleteApiV2TenantUsersById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2TenantUsersById_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}'
}
export const deleteApiV2TenantUsersById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2TenantUsersByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/users/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定したユーザについての接続中のテナントに対するロール情報を編集する
 * request: putApiV2TenantUsersByIdRoles
 * url: putApiV2TenantUsersByIdRolesURL
 * method: putApiV2TenantUsersByIdRoles_TYPE
 * raw_url: putApiV2TenantUsersByIdRoles_RAW_URL
 * @param id - ユーザID
 * @param body - 登録するロールID
 */
export const putApiV2TenantUsersByIdRoles = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/users/{id}/roles'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2TenantUsersByIdRoles_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}/roles'
}
export const putApiV2TenantUsersByIdRoles_TYPE = function() {
  return 'put'
}
export const putApiV2TenantUsersByIdRolesURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/tenant/users/{id}/roles'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ユーザグループ一覧を取得する
 * request: getApiV2AdminUsergroup
 * url: getApiV2AdminUsergroupURL
 * method: getApiV2AdminUsergroup_TYPE
 * raw_url: getApiV2AdminUsergroup_RAW_URL
 */
export const getApiV2AdminUsergroup = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/usergroup'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminUsergroup_RAW_URL = function() {
  return '/api/v2/admin/usergroup'
}
export const getApiV2AdminUsergroup_TYPE = function() {
  return 'get'
}
export const getApiV2AdminUsergroupURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/usergroup'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 新規にユーザグループを登録する
 * request: postApiV2AdminUsergroup
 * url: postApiV2AdminUsergroupURL
 * method: postApiV2AdminUsergroup_TYPE
 * raw_url: postApiV2AdminUsergroup_RAW_URL
 * @param body - ユーザグループ入力モデル
 */
export const postApiV2AdminUsergroup = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/usergroup'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const postApiV2AdminUsergroup_RAW_URL = function() {
  return '/api/v2/admin/usergroup'
}
export const postApiV2AdminUsergroup_TYPE = function() {
  return 'post'
}
export const postApiV2AdminUsergroupURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/usergroup'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 指定されたIDのユーザグループ情報を取得する
 * request: getApiV2AdminUsergroupById
 * url: getApiV2AdminUsergroupByIdURL
 * method: getApiV2AdminUsergroupById_TYPE
 * raw_url: getApiV2AdminUsergroupById_RAW_URL
 * @param id - ユーザグループID
 */
export const getApiV2AdminUsergroupById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/usergroup/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2AdminUsergroupById_RAW_URL = function() {
  return '/api/v2/admin/usergroup/{id}'
}
export const getApiV2AdminUsergroupById_TYPE = function() {
  return 'get'
}
export const getApiV2AdminUsergroupByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/usergroup/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * 既存のユーザグループを編集する
 * request: putApiV2AdminUsergroupById
 * url: putApiV2AdminUsergroupByIdURL
 * method: putApiV2AdminUsergroupById_TYPE
 * raw_url: putApiV2AdminUsergroupById_RAW_URL
 * @param id - ユーザグループID
 * @param body - ユーザグループ入力モデル
 */
export const putApiV2AdminUsergroupById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/usergroup/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['body'] !== undefined) {
    body = parameters['body']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const putApiV2AdminUsergroupById_RAW_URL = function() {
  return '/api/v2/admin/usergroup/{id}'
}
export const putApiV2AdminUsergroupById_TYPE = function() {
  return 'put'
}
export const putApiV2AdminUsergroupByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/usergroup/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * ユーザグループを削除する
 * request: deleteApiV2AdminUsergroupById
 * url: deleteApiV2AdminUsergroupByIdURL
 * method: deleteApiV2AdminUsergroupById_TYPE
 * raw_url: deleteApiV2AdminUsergroupById_RAW_URL
 * @param id - ユーザグループID
 */
export const deleteApiV2AdminUsergroupById = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/usergroup/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('delete', domain + path, body, queryParameters, form, config)
}
export const deleteApiV2AdminUsergroupById_RAW_URL = function() {
  return '/api/v2/admin/usergroup/{id}'
}
export const deleteApiV2AdminUsergroupById_TYPE = function() {
  return 'delete'
}
export const deleteApiV2AdminUsergroupByIdURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/admin/usergroup/{id}'
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
/**
 * バージョン情報を取得
 * request: getApiV2Version
 * url: getApiV2VersionURL
 * method: getApiV2Version_TYPE
 * raw_url: getApiV2Version_RAW_URL
 */
export const getApiV2Version = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/version'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const getApiV2Version_RAW_URL = function() {
  return '/api/v2/version'
}
export const getApiV2Version_TYPE = function() {
  return 'get'
}
export const getApiV2VersionURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/version'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
