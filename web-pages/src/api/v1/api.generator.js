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
 *                    For developers only.
 ==========================================================*/
/**
 * ログインユーザのアカウント情報を取得する
 * request: ApiV1AccountGet
 * url: ApiV1AccountGetURL
 * method: ApiV1AccountGet_TYPE
 * raw_url: ApiV1AccountGet_RAW_URL
 */
export const ApiV1AccountGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account'
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
export const ApiV1AccountGet_RAW_URL = function() {
  return '/api/v1/account'
}
export const ApiV1AccountGet_TYPE = function() {
  return 'get'
}
export const ApiV1AccountGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account'
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
 * request: ApiV1AccountPut
 * url: ApiV1AccountPutURL
 * method: ApiV1AccountPut_TYPE
 * raw_url: ApiV1AccountPut_RAW_URL
 * @param defaultTenant - デフォルトテナント
 */
export const ApiV1AccountPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account'
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
export const ApiV1AccountPut_RAW_URL = function() {
  return '/api/v1/account'
}
export const ApiV1AccountPut_TYPE = function() {
  return 'put'
}
export const ApiV1AccountPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account'
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
 * request: ApiV1AccountPasswordPut
 * url: ApiV1AccountPasswordPutURL
 * method: ApiV1AccountPasswordPut_TYPE
 * raw_url: ApiV1AccountPasswordPut_RAW_URL
 * @param model - 
 */
export const ApiV1AccountPasswordPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/password'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AccountPasswordPut_RAW_URL = function() {
  return '/api/v1/account/password'
}
export const ApiV1AccountPasswordPut_TYPE = function() {
  return 'put'
}
export const ApiV1AccountPasswordPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/password'
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
 * request: ApiV1AccountLoginPost
 * url: ApiV1AccountLoginPostURL
 * method: ApiV1AccountLoginPost_TYPE
 * raw_url: ApiV1AccountLoginPost_RAW_URL
 * @param model - 
 */
export const ApiV1AccountLoginPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/login'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AccountLoginPost_RAW_URL = function() {
  return '/api/v1/account/login'
}
export const ApiV1AccountLoginPost_TYPE = function() {
  return 'post'
}
export const ApiV1AccountLoginPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/login'
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
 * request: ApiV1AccountTenantsByTenantIdTokenPost
 * url: ApiV1AccountTenantsByTenantIdTokenPostURL
 * method: ApiV1AccountTenantsByTenantIdTokenPost_TYPE
 * raw_url: ApiV1AccountTenantsByTenantIdTokenPost_RAW_URL
 * @param tenantId - テナントID
 * @param expiresIn - 有効期限(秒)。省略時はシステムの既定値。
 */
export const ApiV1AccountTenantsByTenantIdTokenPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/tenants/{tenantId}/token'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['tenantId'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: tenantId'))
  }
  if (parameters['expiresIn'] !== undefined) {
    queryParameters['expiresIn'] = parameters['expiresIn']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AccountTenantsByTenantIdTokenPost_RAW_URL = function() {
  return '/api/v1/account/tenants/{tenantId}/token'
}
export const ApiV1AccountTenantsByTenantIdTokenPost_TYPE = function() {
  return 'post'
}
export const ApiV1AccountTenantsByTenantIdTokenPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/tenants/{tenantId}/token'
  path = path.replace('{tenantId}', `${parameters['tenantId']}`)
  if (parameters['expiresIn'] !== undefined) {
    queryParameters['expiresIn'] = parameters['expiresIn']
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
 * アクセス可能なメニュー一覧をツリー形式で取得する。
 * request: ApiV1AccountMenusTreeGet
 * url: ApiV1AccountMenusTreeGetURL
 * method: ApiV1AccountMenusTreeGet_TYPE
 * raw_url: ApiV1AccountMenusTreeGet_RAW_URL
 * @param lang - 
 */
export const ApiV1AccountMenusTreeGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/menus/tree'
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
export const ApiV1AccountMenusTreeGet_RAW_URL = function() {
  return '/api/v1/account/menus/tree'
}
export const ApiV1AccountMenusTreeGet_TYPE = function() {
  return 'get'
}
export const ApiV1AccountMenusTreeGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/menus/tree'
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
 * アクセス可能なメニュー一覧をリスト形式で取得する。
 * request: ApiV1AccountMenusListGet
 * url: ApiV1AccountMenusListGetURL
 * method: ApiV1AccountMenusListGet_TYPE
 * raw_url: ApiV1AccountMenusListGet_RAW_URL
 * @param lang - 
 */
export const ApiV1AccountMenusListGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/menus/list'
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
export const ApiV1AccountMenusListGet_RAW_URL = function() {
  return '/api/v1/account/menus/list'
}
export const ApiV1AccountMenusListGet_TYPE = function() {
  return 'get'
}
export const ApiV1AccountMenusListGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/menus/list'
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
 * request: ApiV1AccountGitsGet
 * url: ApiV1AccountGitsGetURL
 * method: ApiV1AccountGitsGet_TYPE
 * raw_url: ApiV1AccountGitsGet_RAW_URL
 */
export const ApiV1AccountGitsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/gits'
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
export const ApiV1AccountGitsGet_RAW_URL = function() {
  return '/api/v1/account/gits'
}
export const ApiV1AccountGitsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AccountGitsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/gits'
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
 * request: ApiV1AccountGitsPut
 * url: ApiV1AccountGitsPutURL
 * method: ApiV1AccountGitsPut_TYPE
 * raw_url: ApiV1AccountGitsPut_RAW_URL
 * @param model - 更新内容
 */
export const ApiV1AccountGitsPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/gits'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AccountGitsPut_RAW_URL = function() {
  return '/api/v1/account/gits'
}
export const ApiV1AccountGitsPut_TYPE = function() {
  return 'put'
}
export const ApiV1AccountGitsPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/gits'
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
 * request: ApiV1AccountRegistriesGet
 * url: ApiV1AccountRegistriesGetURL
 * method: ApiV1AccountRegistriesGet_TYPE
 * raw_url: ApiV1AccountRegistriesGet_RAW_URL
 */
export const ApiV1AccountRegistriesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/registries'
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
export const ApiV1AccountRegistriesGet_RAW_URL = function() {
  return '/api/v1/account/registries'
}
export const ApiV1AccountRegistriesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AccountRegistriesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/registries'
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
 * request: ApiV1AccountRegistriesPut
 * url: ApiV1AccountRegistriesPutURL
 * method: ApiV1AccountRegistriesPut_TYPE
 * raw_url: ApiV1AccountRegistriesPut_RAW_URL
 * @param model - 更新内容
 */
export const ApiV1AccountRegistriesPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/account/registries'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AccountRegistriesPut_RAW_URL = function() {
  return '/api/v1/account/registries'
}
export const ApiV1AccountRegistriesPut_TYPE = function() {
  return 'put'
}
export const ApiV1AccountRegistriesPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/account/registries'
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
 * request: ApiV1TenantPartitionsGet
 * url: ApiV1TenantPartitionsGetURL
 * method: ApiV1TenantPartitionsGet_TYPE
 * raw_url: ApiV1TenantPartitionsGet_RAW_URL
 */
export const ApiV1TenantPartitionsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/partitions'
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
export const ApiV1TenantPartitionsGet_RAW_URL = function() {
  return '/api/v1/tenant/partitions'
}
export const ApiV1TenantPartitionsGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantPartitionsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/partitions'
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
 * request: ApiV1AdminPartitionsGet
 * url: ApiV1AdminPartitionsGetURL
 * method: ApiV1AdminPartitionsGet_TYPE
 * raw_url: ApiV1AdminPartitionsGet_RAW_URL
 */
export const ApiV1AdminPartitionsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/partitions'
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
export const ApiV1AdminPartitionsGet_RAW_URL = function() {
  return '/api/v1/admin/partitions'
}
export const ApiV1AdminPartitionsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminPartitionsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/partitions'
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
 * request: ApiV1AdminQuotasGet
 * url: ApiV1AdminQuotasGetURL
 * method: ApiV1AdminQuotasGet_TYPE
 * raw_url: ApiV1AdminQuotasGet_RAW_URL
 */
export const ApiV1AdminQuotasGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/quotas'
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
export const ApiV1AdminQuotasGet_RAW_URL = function() {
  return '/api/v1/admin/quotas'
}
export const ApiV1AdminQuotasGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminQuotasGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/quotas'
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
 * request: ApiV1AdminQuotasPost
 * url: ApiV1AdminQuotasPostURL
 * method: ApiV1AdminQuotasPost_TYPE
 * raw_url: ApiV1AdminQuotasPost_RAW_URL
 * @param models - 
 */
export const ApiV1AdminQuotasPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/quotas'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['models'] !== undefined) {
    body = parameters['models']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminQuotasPost_RAW_URL = function() {
  return '/api/v1/admin/quotas'
}
export const ApiV1AdminQuotasPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminQuotasPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/quotas'
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
 * request: ApiV1TenantQuotaGet
 * url: ApiV1TenantQuotaGetURL
 * method: ApiV1TenantQuotaGet_TYPE
 * raw_url: ApiV1TenantQuotaGet_RAW_URL
 */
export const ApiV1TenantQuotaGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/quota'
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
export const ApiV1TenantQuotaGet_RAW_URL = function() {
  return '/api/v1/tenant/quota'
}
export const ApiV1TenantQuotaGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantQuotaGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/quota'
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
 * request: ApiV1AdminTensorboardsDelete
 * url: ApiV1AdminTensorboardsDeleteURL
 * method: ApiV1AdminTensorboardsDelete_TYPE
 * raw_url: ApiV1AdminTensorboardsDelete_RAW_URL
 */
export const ApiV1AdminTensorboardsDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tensorboards'
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
export const ApiV1AdminTensorboardsDelete_RAW_URL = function() {
  return '/api/v1/admin/tensorboards'
}
export const ApiV1AdminTensorboardsDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminTensorboardsDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tensorboards'
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
 * request: ApiV1AdminEventsByIdGet
 * url: ApiV1AdminEventsByIdGetURL
 * method: ApiV1AdminEventsByIdGet_TYPE
 * raw_url: ApiV1AdminEventsByIdGet_RAW_URL
 * @param id - テナントID
 * @param name - コンテナ名
 */
export const ApiV1AdminEventsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/events/{id}'
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
export const ApiV1AdminEventsByIdGet_RAW_URL = function() {
  return '/api/v1/admin/events/{id}'
}
export const ApiV1AdminEventsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminEventsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/events/{id}'
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
* request: ApiV1DataGet
* url: ApiV1DataGetURL
* method: ApiV1DataGet_TYPE
* raw_url: ApiV1DataGet_RAW_URL
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
export const ApiV1DataGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data'
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
export const ApiV1DataGet_RAW_URL = function() {
  return '/api/v1/data'
}
export const ApiV1DataGet_TYPE = function() {
  return 'get'
}
export const ApiV1DataGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data'
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
 * request: ApiV1DataPost
 * url: ApiV1DataPostURL
 * method: ApiV1DataPost_TYPE
 * raw_url: ApiV1DataPost_RAW_URL
 * @param model - 
 */
export const ApiV1DataPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1DataPost_RAW_URL = function() {
  return '/api/v1/data'
}
export const ApiV1DataPost_TYPE = function() {
  return 'post'
}
export const ApiV1DataPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data'
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
 * request: ApiV1DataByIdGet
 * url: ApiV1DataByIdGetURL
 * method: ApiV1DataByIdGet_TYPE
 * raw_url: ApiV1DataByIdGet_RAW_URL
 * @param id - データID
 */
export const ApiV1DataByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}'
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
export const ApiV1DataByIdGet_RAW_URL = function() {
  return '/api/v1/data/{id}'
}
export const ApiV1DataByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1DataByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}'
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
* request: ApiV1DataByIdPut
* url: ApiV1DataByIdPutURL
* method: ApiV1DataByIdPut_TYPE
* raw_url: ApiV1DataByIdPut_RAW_URL
     * @param id - 変更対象のデータID
     * @param model - 変更内容
*/
export const ApiV1DataByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1DataByIdPut_RAW_URL = function() {
  return '/api/v1/data/{id}'
}
export const ApiV1DataByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1DataByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}'
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
 * request: ApiV1DataByIdDelete
 * url: ApiV1DataByIdDeleteURL
 * method: ApiV1DataByIdDelete_TYPE
 * raw_url: ApiV1DataByIdDelete_RAW_URL
 * @param id - データID
 */
export const ApiV1DataByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}'
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
export const ApiV1DataByIdDelete_RAW_URL = function() {
  return '/api/v1/data/{id}'
}
export const ApiV1DataByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1DataByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}'
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
 * request: ApiV1DataByIdTagsByTagPut
 * url: ApiV1DataByIdTagsByTagPutURL
 * method: ApiV1DataByIdTagsByTagPut_TYPE
 * raw_url: ApiV1DataByIdTagsByTagPut_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const ApiV1DataByIdTagsByTagPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/tags/{tag}'
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
export const ApiV1DataByIdTagsByTagPut_RAW_URL = function() {
  return '/api/v1/data/{id}/tags/{tag}'
}
export const ApiV1DataByIdTagsByTagPut_TYPE = function() {
  return 'put'
}
export const ApiV1DataByIdTagsByTagPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/tags/{tag}'
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
 * request: ApiV1DataByIdTagsByTagDelete
 * url: ApiV1DataByIdTagsByTagDeleteURL
 * method: ApiV1DataByIdTagsByTagDelete_TYPE
 * raw_url: ApiV1DataByIdTagsByTagDelete_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const ApiV1DataByIdTagsByTagDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/tags/{tag}'
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
export const ApiV1DataByIdTagsByTagDelete_RAW_URL = function() {
  return '/api/v1/data/{id}/tags/{tag}'
}
export const ApiV1DataByIdTagsByTagDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1DataByIdTagsByTagDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/tags/{tag}'
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
 * request: ApiV1DataByIdFilesByNameGet
 * url: ApiV1DataByIdFilesByNameGetURL
 * method: ApiV1DataByIdFilesByNameGet_TYPE
 * raw_url: ApiV1DataByIdFilesByNameGet_RAW_URL
 * @param id - 対象データID
 * @param name - 対象ファイル名
 */
export const ApiV1DataByIdFilesByNameGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/files/{name}'
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
export const ApiV1DataByIdFilesByNameGet_RAW_URL = function() {
  return '/api/v1/data/{id}/files/{name}'
}
export const ApiV1DataByIdFilesByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV1DataByIdFilesByNameGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/files/{name}'
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
 * request: ApiV1DataByIdFilesGet
 * url: ApiV1DataByIdFilesGetURL
 * method: ApiV1DataByIdFilesGet_TYPE
 * raw_url: ApiV1DataByIdFilesGet_RAW_URL
 * @param id - 対象データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV1DataByIdFilesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/files'
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
export const ApiV1DataByIdFilesGet_RAW_URL = function() {
  return '/api/v1/data/{id}/files'
}
export const ApiV1DataByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV1DataByIdFilesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/files'
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
 * request: ApiV1DataByIdFilesPost
 * url: ApiV1DataByIdFilesPostURL
 * method: ApiV1DataByIdFilesPost_TYPE
 * raw_url: ApiV1DataByIdFilesPost_RAW_URL
 * @param id - 変更対象のデータID
 * @param model - 追加するファイル情報
 */
export const ApiV1DataByIdFilesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1DataByIdFilesPost_RAW_URL = function() {
  return '/api/v1/data/{id}/files'
}
export const ApiV1DataByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV1DataByIdFilesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/files'
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
 * request: ApiV1DataByIdFilesByFileIdDelete
 * url: ApiV1DataByIdFilesByFileIdDeleteURL
 * method: ApiV1DataByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV1DataByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象のデータID
 * @param fileId - 削除するファイルのID
 */
export const ApiV1DataByIdFilesByFileIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/{id}/files/{fileId}'
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
export const ApiV1DataByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v1/data/{id}/files/{fileId}'
}
export const ApiV1DataByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1DataByIdFilesByFileIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/{id}/files/{fileId}'
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
 * request: ApiV1DataDatatagsGet
 * url: ApiV1DataDatatagsGetURL
 * method: ApiV1DataDatatagsGet_TYPE
 * raw_url: ApiV1DataDatatagsGet_RAW_URL
 */
export const ApiV1DataDatatagsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/datatags'
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
export const ApiV1DataDatatagsGet_RAW_URL = function() {
  return '/api/v1/data/datatags'
}
export const ApiV1DataDatatagsGet_TYPE = function() {
  return 'get'
}
export const ApiV1DataDatatagsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/datatags'
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
 * request: ApiV1DataDatatagsDelete
 * url: ApiV1DataDatatagsDeleteURL
 * method: ApiV1DataDatatagsDelete_TYPE
 * raw_url: ApiV1DataDatatagsDelete_RAW_URL
 */
export const ApiV1DataDatatagsDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/data/datatags'
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
export const ApiV1DataDatatagsDelete_RAW_URL = function() {
  return '/api/v1/data/datatags'
}
export const ApiV1DataDatatagsDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1DataDatatagsDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/data/datatags'
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
* request: ApiV1DatasetsGet
* url: ApiV1DatasetsGetURL
* method: ApiV1DatasetsGet_TYPE
* raw_url: ApiV1DatasetsGet_RAW_URL
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
export const ApiV1DatasetsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets'
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
export const ApiV1DatasetsGet_RAW_URL = function() {
  return '/api/v1/datasets'
}
export const ApiV1DatasetsGet_TYPE = function() {
  return 'get'
}
export const ApiV1DatasetsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets'
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
 * request: ApiV1DatasetsPost
 * url: ApiV1DatasetsPostURL
 * method: ApiV1DatasetsPost_TYPE
 * raw_url: ApiV1DatasetsPost_RAW_URL
 * @param model - 新規作成内容
 */
export const ApiV1DatasetsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1DatasetsPost_RAW_URL = function() {
  return '/api/v1/datasets'
}
export const ApiV1DatasetsPost_TYPE = function() {
  return 'post'
}
export const ApiV1DatasetsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets'
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
 * request: ApiV1DatasetsByIdGet
 * url: ApiV1DatasetsByIdGetURL
 * method: ApiV1DatasetsByIdGet_TYPE
 * raw_url: ApiV1DatasetsByIdGet_RAW_URL
 * @param id - データセットID
 */
export const ApiV1DatasetsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}'
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
export const ApiV1DatasetsByIdGet_RAW_URL = function() {
  return '/api/v1/datasets/{id}'
}
export const ApiV1DatasetsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1DatasetsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}'
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
 * request: ApiV1DatasetsByIdPut
 * url: ApiV1DatasetsByIdPutURL
 * method: ApiV1DatasetsByIdPut_TYPE
 * raw_url: ApiV1DatasetsByIdPut_RAW_URL
 * @param id - データセットID
 * @param model - 変更内容
 */
export const ApiV1DatasetsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1DatasetsByIdPut_RAW_URL = function() {
  return '/api/v1/datasets/{id}'
}
export const ApiV1DatasetsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1DatasetsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}'
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
 * request: ApiV1DatasetsByIdDelete
 * url: ApiV1DatasetsByIdDeleteURL
 * method: ApiV1DatasetsByIdDelete_TYPE
 * raw_url: ApiV1DatasetsByIdDelete_RAW_URL
 * @param id - データセットID
 */
export const ApiV1DatasetsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}'
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
export const ApiV1DatasetsByIdDelete_RAW_URL = function() {
  return '/api/v1/datasets/{id}'
}
export const ApiV1DatasetsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1DatasetsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}'
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
 * request: ApiV1DatasetsByIdPatch
 * url: ApiV1DatasetsByIdPatchURL
 * method: ApiV1DatasetsByIdPatch_TYPE
 * raw_url: ApiV1DatasetsByIdPatch_RAW_URL
 * @param id - データセットID
 * @param model - 変更内容
 */
export const ApiV1DatasetsByIdPatch = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const ApiV1DatasetsByIdPatch_RAW_URL = function() {
  return '/api/v1/datasets/{id}'
}
export const ApiV1DatasetsByIdPatch_TYPE = function() {
  return 'patch'
}
export const ApiV1DatasetsByIdPatchURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}'
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
 * request: ApiV1DatasetsByIdFilesGet
 * url: ApiV1DatasetsByIdFilesGetURL
 * method: ApiV1DatasetsByIdFilesGet_TYPE
 * raw_url: ApiV1DatasetsByIdFilesGet_RAW_URL
 * @param id - データセットID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV1DatasetsByIdFilesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}/files'
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
export const ApiV1DatasetsByIdFilesGet_RAW_URL = function() {
  return '/api/v1/datasets/{id}/files'
}
export const ApiV1DatasetsByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV1DatasetsByIdFilesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}/files'
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
 * request: ApiV1DatasetsByIdPathpairsGet
 * url: ApiV1DatasetsByIdPathpairsGetURL
 * method: ApiV1DatasetsByIdPathpairsGet_TYPE
 * raw_url: ApiV1DatasetsByIdPathpairsGet_RAW_URL
 * @param id - データセットID
 */
export const ApiV1DatasetsByIdPathpairsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datasets/{id}/pathpairs'
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
export const ApiV1DatasetsByIdPathpairsGet_RAW_URL = function() {
  return '/api/v1/datasets/{id}/pathpairs'
}
export const ApiV1DatasetsByIdPathpairsGet_TYPE = function() {
  return 'get'
}
export const ApiV1DatasetsByIdPathpairsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datasets/{id}/pathpairs'
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
 * request: ApiV1DatatypesGet
 * url: ApiV1DatatypesGetURL
 * method: ApiV1DatatypesGet_TYPE
 * raw_url: ApiV1DatatypesGet_RAW_URL
 */
export const ApiV1DatatypesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/datatypes'
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
export const ApiV1DatatypesGet_RAW_URL = function() {
  return '/api/v1/datatypes'
}
export const ApiV1DatatypesGet_TYPE = function() {
  return 'get'
}
export const ApiV1DatatypesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/datatypes'
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
 * request: ApiV1AdminGitEndpointsGet
 * url: ApiV1AdminGitEndpointsGetURL
 * method: ApiV1AdminGitEndpointsGet_TYPE
 * raw_url: ApiV1AdminGitEndpointsGet_RAW_URL
 */
export const ApiV1AdminGitEndpointsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/endpoints'
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
export const ApiV1AdminGitEndpointsGet_RAW_URL = function() {
  return '/api/v1/admin/git/endpoints'
}
export const ApiV1AdminGitEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminGitEndpointsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/endpoints'
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
 * request: ApiV1AdminGitEndpointsPost
 * url: ApiV1AdminGitEndpointsPostURL
 * method: ApiV1AdminGitEndpointsPost_TYPE
 * raw_url: ApiV1AdminGitEndpointsPost_RAW_URL
 * @param model - 新規作成モデル
 */
export const ApiV1AdminGitEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminGitEndpointsPost_RAW_URL = function() {
  return '/api/v1/admin/git/endpoints'
}
export const ApiV1AdminGitEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminGitEndpointsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/endpoints'
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
 * request: ApiV1AdminGitTypesGet
 * url: ApiV1AdminGitTypesGetURL
 * method: ApiV1AdminGitTypesGet_TYPE
 * raw_url: ApiV1AdminGitTypesGet_RAW_URL
 */
export const ApiV1AdminGitTypesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/types'
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
export const ApiV1AdminGitTypesGet_RAW_URL = function() {
  return '/api/v1/admin/git/types'
}
export const ApiV1AdminGitTypesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminGitTypesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/types'
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
 * request: ApiV1AdminGitEndpointsByIdGet
 * url: ApiV1AdminGitEndpointsByIdGetURL
 * method: ApiV1AdminGitEndpointsByIdGet_TYPE
 * raw_url: ApiV1AdminGitEndpointsByIdGet_RAW_URL
 * @param id - GitエンドポイントID
 */
export const ApiV1AdminGitEndpointsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/endpoints/{id}'
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
export const ApiV1AdminGitEndpointsByIdGet_RAW_URL = function() {
  return '/api/v1/admin/git/endpoints/{id}'
}
export const ApiV1AdminGitEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminGitEndpointsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/endpoints/{id}'
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
 * request: ApiV1AdminGitEndpointsByIdPut
 * url: ApiV1AdminGitEndpointsByIdPutURL
 * method: ApiV1AdminGitEndpointsByIdPut_TYPE
 * raw_url: ApiV1AdminGitEndpointsByIdPut_RAW_URL
 * @param id - 編集対象GitID
 * @param model - 編集モデル
 */
export const ApiV1AdminGitEndpointsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminGitEndpointsByIdPut_RAW_URL = function() {
  return '/api/v1/admin/git/endpoints/{id}'
}
export const ApiV1AdminGitEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminGitEndpointsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/endpoints/{id}'
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
 * request: ApiV1AdminGitEndpointsByIdDelete
 * url: ApiV1AdminGitEndpointsByIdDeleteURL
 * method: ApiV1AdminGitEndpointsByIdDelete_TYPE
 * raw_url: ApiV1AdminGitEndpointsByIdDelete_RAW_URL
 * @param id - 削除対象GitID
 */
export const ApiV1AdminGitEndpointsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/git/endpoints/{id}'
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
export const ApiV1AdminGitEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/git/endpoints/{id}'
}
export const ApiV1AdminGitEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminGitEndpointsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/git/endpoints/{id}'
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
 * request: ApiV1TenantGitEndpointsGet
 * url: ApiV1TenantGitEndpointsGetURL
 * method: ApiV1TenantGitEndpointsGet_TYPE
 * raw_url: ApiV1TenantGitEndpointsGet_RAW_URL
 */
export const ApiV1TenantGitEndpointsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/git/endpoints'
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
export const ApiV1TenantGitEndpointsGet_RAW_URL = function() {
  return '/api/v1/tenant/git/endpoints'
}
export const ApiV1TenantGitEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantGitEndpointsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/git/endpoints'
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
 * request: ApiV1GitByGitIdReposGet
 * url: ApiV1GitByGitIdReposGetURL
 * method: ApiV1GitByGitIdReposGet_TYPE
 * raw_url: ApiV1GitByGitIdReposGet_RAW_URL
 * @param gitId - 
 */
export const ApiV1GitByGitIdReposGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/git/{gitId}/repos'
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
export const ApiV1GitByGitIdReposGet_RAW_URL = function() {
  return '/api/v1/git/{gitId}/repos'
}
export const ApiV1GitByGitIdReposGet_TYPE = function() {
  return 'get'
}
export const ApiV1GitByGitIdReposGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/git/{gitId}/repos'
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
 * request: ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet
 * url: ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGetURL
 * method: ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet_TYPE
 * raw_url: ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 */
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/branches'
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
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet_RAW_URL = function() {
  return '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/branches'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet_TYPE = function() {
  return 'get'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/branches'
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
 * request: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet
 * url: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGetURL
 * method: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet_TYPE
 * raw_url: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param branch - ブランチ名
 */
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits'
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
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet_RAW_URL = function() {
  return '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet_TYPE = function() {
  return 'get'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{owner}', `${parameters['owner']}`)
  path = path.replace('{repositoryName}', `${parameters['repositoryName']}`)
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
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
 * request: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet
 * url: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGetURL
 * method: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_TYPE
 * raw_url: ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param commitId - コミットID
 */
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
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
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_RAW_URL = function() {
  return '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
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
* request: ApiV1GitByGitIdReposBySegmentsGet
* url: ApiV1GitByGitIdReposBySegmentsGetURL
* method: ApiV1GitByGitIdReposBySegmentsGet_TYPE
* raw_url: ApiV1GitByGitIdReposBySegmentsGet_RAW_URL
     * @param gitId - 
     * @param segments - 
     * @param branch - 
*/
export const ApiV1GitByGitIdReposBySegmentsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/git/{gitId}/repos/{segments}'
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
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const ApiV1GitByGitIdReposBySegmentsGet_RAW_URL = function() {
  return '/api/v1/git/{gitId}/repos/{segments}'
}
export const ApiV1GitByGitIdReposBySegmentsGet_TYPE = function() {
  return 'get'
}
export const ApiV1GitByGitIdReposBySegmentsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/git/{gitId}/repos/{segments}'
  path = path.replace('{gitId}', `${parameters['gitId']}`)
  path = path.replace('{segments}', `${parameters['segments']}`)
  if (parameters['branch'] !== undefined) {
    queryParameters['branch'] = parameters['branch']
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
 * request: ApiV1InferencesSimpleGet
 * url: ApiV1InferencesSimpleGetURL
 * method: ApiV1InferencesSimpleGet_TYPE
 * raw_url: ApiV1InferencesSimpleGet_RAW_URL
 */
export const ApiV1InferencesSimpleGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/simple'
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
export const ApiV1InferencesSimpleGet_RAW_URL = function() {
  return '/api/v1/inferences/simple'
}
export const ApiV1InferencesSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesSimpleGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/simple'
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
* request: ApiV1InferencesGet
* url: ApiV1InferencesGetURL
* method: ApiV1InferencesGet_TYPE
* raw_url: ApiV1InferencesGet_RAW_URL
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
     * @param parentName - マウントした学習名
     * @param perPage - 表示件数。指定がない場合は上限(1000件)。
     * @param page - ページ番号。デフォルトは1。
     * @param withTotal - 合計件数をレスポンスヘッダ(X-Total-Count)に含めるか。デフォルトはfalse。
*/
export const ApiV1InferencesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences'
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
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
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
export const ApiV1InferencesGet_RAW_URL = function() {
  return '/api/v1/inferences'
}
export const ApiV1InferencesGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences'
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
  if (parameters['parentName'] !== undefined) {
    queryParameters['ParentName'] = parameters['parentName']
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
 * 指定されたIDの推論履歴の詳細情報を取得
 * request: ApiV1InferencesByIdGet
 * url: ApiV1InferencesByIdGetURL
 * method: ApiV1InferencesByIdGet_TYPE
 * raw_url: ApiV1InferencesByIdGet_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV1InferencesByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}'
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
export const ApiV1InferencesByIdGet_RAW_URL = function() {
  return '/api/v1/inferences/{id}'
}
export const ApiV1InferencesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}'
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
 * request: ApiV1InferencesByIdPut
 * url: ApiV1InferencesByIdPutURL
 * method: ApiV1InferencesByIdPut_TYPE
 * raw_url: ApiV1InferencesByIdPut_RAW_URL
 * @param id - 変更対象の推論履歴ID
 * @param model - 変更内容
 */
export const ApiV1InferencesByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1InferencesByIdPut_RAW_URL = function() {
  return '/api/v1/inferences/{id}'
}
export const ApiV1InferencesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1InferencesByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}'
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
 * request: ApiV1InferencesByIdDelete
 * url: ApiV1InferencesByIdDeleteURL
 * method: ApiV1InferencesByIdDelete_TYPE
 * raw_url: ApiV1InferencesByIdDelete_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV1InferencesByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}'
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
export const ApiV1InferencesByIdDelete_RAW_URL = function() {
  return '/api/v1/inferences/{id}'
}
export const ApiV1InferencesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1InferencesByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}'
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
 * request: ApiV1InferencesByIdEventsGet
 * url: ApiV1InferencesByIdEventsGetURL
 * method: ApiV1InferencesByIdEventsGet_TYPE
 * raw_url: ApiV1InferencesByIdEventsGet_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV1InferencesByIdEventsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/events'
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
export const ApiV1InferencesByIdEventsGet_RAW_URL = function() {
  return '/api/v1/inferences/{id}/events'
}
export const ApiV1InferencesByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesByIdEventsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/events'
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
 * request: ApiV1InferencesRunPost
 * url: ApiV1InferencesRunPostURL
 * method: ApiV1InferencesRunPost_TYPE
 * raw_url: ApiV1InferencesRunPost_RAW_URL
 * @param model - 新規推論実行内容
 */
export const ApiV1InferencesRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1InferencesRunPost_RAW_URL = function() {
  return '/api/v1/inferences/run'
}
export const ApiV1InferencesRunPost_TYPE = function() {
  return 'post'
}
export const ApiV1InferencesRunPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/run'
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
 * request: ApiV1InferencesByIdFilesGet
 * url: ApiV1InferencesByIdFilesGetURL
 * method: ApiV1InferencesByIdFilesGet_TYPE
 * raw_url: ApiV1InferencesByIdFilesGet_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV1InferencesByIdFilesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/files'
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
export const ApiV1InferencesByIdFilesGet_RAW_URL = function() {
  return '/api/v1/inferences/{id}/files'
}
export const ApiV1InferencesByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesByIdFilesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/files'
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
 * 推論履歴添付ファイルを登録
 * request: ApiV1InferencesByIdFilesPost
 * url: ApiV1InferencesByIdFilesPostURL
 * method: ApiV1InferencesByIdFilesPost_TYPE
 * raw_url: ApiV1InferencesByIdFilesPost_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param model - 追加するファイル情報
 */
export const ApiV1InferencesByIdFilesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1InferencesByIdFilesPost_RAW_URL = function() {
  return '/api/v1/inferences/{id}/files'
}
export const ApiV1InferencesByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV1InferencesByIdFilesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/files'
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
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: ApiV1InferencesByIdContainer_filesGet
* url: ApiV1InferencesByIdContainer_filesGetURL
* method: ApiV1InferencesByIdContainer_filesGet_TYPE
* raw_url: ApiV1InferencesByIdContainer_filesGet_RAW_URL
     * @param id - 対象の推論履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV1InferencesByIdContainer_filesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/container-files'
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
export const ApiV1InferencesByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v1/inferences/{id}/container-files'
}
export const ApiV1InferencesByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV1InferencesByIdContainer_filesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/container-files'
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
 * request: ApiV1InferencesByIdFilesByFileIdDelete
 * url: ApiV1InferencesByIdFilesByFileIdDeleteURL
 * method: ApiV1InferencesByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV1InferencesByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param fileId - 削除するファイルのID
 */
export const ApiV1InferencesByIdFilesByFileIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/files/{fileId}'
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
export const ApiV1InferencesByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v1/inferences/{id}/files/{fileId}'
}
export const ApiV1InferencesByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1InferencesByIdFilesByFileIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/files/{fileId}'
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
 * 推論を途中で強制終了
 * request: ApiV1InferencesByIdHaltPost
 * url: ApiV1InferencesByIdHaltPostURL
 * method: ApiV1InferencesByIdHaltPost_TYPE
 * raw_url: ApiV1InferencesByIdHaltPost_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV1InferencesByIdHaltPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/halt'
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
export const ApiV1InferencesByIdHaltPost_RAW_URL = function() {
  return '/api/v1/inferences/{id}/halt'
}
export const ApiV1InferencesByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV1InferencesByIdHaltPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/halt'
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
* request: ApiV1InferencesByIdUser_cancelPost
* url: ApiV1InferencesByIdUser_cancelPostURL
* method: ApiV1InferencesByIdUser_cancelPost_TYPE
* raw_url: ApiV1InferencesByIdUser_cancelPost_RAW_URL
     * @param id - 推論履歴ID
*/
export const ApiV1InferencesByIdUser_cancelPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/user-cancel'
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
export const ApiV1InferencesByIdUser_cancelPost_RAW_URL = function() {
  return '/api/v1/inferences/{id}/user-cancel'
}
export const ApiV1InferencesByIdUser_cancelPost_TYPE = function() {
  return 'post'
}
export const ApiV1InferencesByIdUser_cancelPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/user-cancel'
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
 * request: ApiV1InferencesByIdCompletePost
 * url: ApiV1InferencesByIdCompletePostURL
 * method: ApiV1InferencesByIdCompletePost_TYPE
 * raw_url: ApiV1InferencesByIdCompletePost_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV1InferencesByIdCompletePost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/inferences/{id}/complete'
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
export const ApiV1InferencesByIdCompletePost_RAW_URL = function() {
  return '/api/v1/inferences/{id}/complete'
}
export const ApiV1InferencesByIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV1InferencesByIdCompletePostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/inferences/{id}/complete'
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
 * request: ApiV1TenantMenusGet
 * url: ApiV1TenantMenusGetURL
 * method: ApiV1TenantMenusGet_TYPE
 * raw_url: ApiV1TenantMenusGet_RAW_URL
 */
export const ApiV1TenantMenusGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/menus'
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
export const ApiV1TenantMenusGet_RAW_URL = function() {
  return '/api/v1/tenant/menus'
}
export const ApiV1TenantMenusGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantMenusGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/menus'
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
 * request: ApiV1TenantMenusByIdPut
 * url: ApiV1TenantMenusByIdPutURL
 * method: ApiV1TenantMenusByIdPut_TYPE
 * raw_url: ApiV1TenantMenusByIdPut_RAW_URL
 * @param id - 
 * @param roleIds - 
 */
export const ApiV1TenantMenusByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/menus/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['roleIds'] !== undefined) {
    body = parameters['roleIds']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TenantMenusByIdPut_RAW_URL = function() {
  return '/api/v1/tenant/menus/{id}'
}
export const ApiV1TenantMenusByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1TenantMenusByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/menus/{id}'
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
 * request: ApiV1TenantMenu_typesGet
 * url: ApiV1TenantMenu_typesGetURL
 * method: ApiV1TenantMenu_typesGet_TYPE
 * raw_url: ApiV1TenantMenu_typesGet_RAW_URL
 */
export const ApiV1TenantMenu_typesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/menu-types'
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
export const ApiV1TenantMenu_typesGet_RAW_URL = function() {
  return '/api/v1/tenant/menu-types'
}
export const ApiV1TenantMenu_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantMenu_typesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/menu-types'
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
 * request: ApiV1AdminMenusGet
 * url: ApiV1AdminMenusGetURL
 * method: ApiV1AdminMenusGet_TYPE
 * raw_url: ApiV1AdminMenusGet_RAW_URL
 */
export const ApiV1AdminMenusGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/menus'
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
export const ApiV1AdminMenusGet_RAW_URL = function() {
  return '/api/v1/admin/menus'
}
export const ApiV1AdminMenusGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminMenusGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/menus'
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
 * request: ApiV1AdminMenusByIdPut
 * url: ApiV1AdminMenusByIdPutURL
 * method: ApiV1AdminMenusByIdPut_TYPE
 * raw_url: ApiV1AdminMenusByIdPut_RAW_URL
 * @param id - 
 * @param roleIds - 
 */
export const ApiV1AdminMenusByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/menus/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['roleIds'] !== undefined) {
    body = parameters['roleIds']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminMenusByIdPut_RAW_URL = function() {
  return '/api/v1/admin/menus/{id}'
}
export const ApiV1AdminMenusByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminMenusByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/menus/{id}'
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
 * request: ApiV1AdminMenu_typesGet
 * url: ApiV1AdminMenu_typesGetURL
 * method: ApiV1AdminMenu_typesGet_TYPE
 * raw_url: ApiV1AdminMenu_typesGet_RAW_URL
 */
export const ApiV1AdminMenu_typesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/menu-types'
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
export const ApiV1AdminMenu_typesGet_RAW_URL = function() {
  return '/api/v1/admin/menu-types'
}
export const ApiV1AdminMenu_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminMenu_typesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/menu-types'
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
 * request: ApiV1AdminNodesGet
 * url: ApiV1AdminNodesGetURL
 * method: ApiV1AdminNodesGet_TYPE
 * raw_url: ApiV1AdminNodesGet_RAW_URL
 * @param name - 
 * @param perPage - 
 * @param page - 
 * @param withTotal - 
 */
export const ApiV1AdminNodesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes'
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
export const ApiV1AdminNodesGet_RAW_URL = function() {
  return '/api/v1/admin/nodes'
}
export const ApiV1AdminNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminNodesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes'
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
 * request: ApiV1AdminNodesPost
 * url: ApiV1AdminNodesPostURL
 * method: ApiV1AdminNodesPost_TYPE
 * raw_url: ApiV1AdminNodesPost_RAW_URL
 * @param model - 
 */
export const ApiV1AdminNodesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminNodesPost_RAW_URL = function() {
  return '/api/v1/admin/nodes'
}
export const ApiV1AdminNodesPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminNodesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes'
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
 * request: ApiV1AdminNode_access_levelsGet
 * url: ApiV1AdminNode_access_levelsGetURL
 * method: ApiV1AdminNode_access_levelsGet_TYPE
 * raw_url: ApiV1AdminNode_access_levelsGet_RAW_URL
 */
export const ApiV1AdminNode_access_levelsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/node-access-levels'
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
export const ApiV1AdminNode_access_levelsGet_RAW_URL = function() {
  return '/api/v1/admin/node-access-levels'
}
export const ApiV1AdminNode_access_levelsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminNode_access_levelsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/node-access-levels'
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
 * request: ApiV1AdminNodesByIdGet
 * url: ApiV1AdminNodesByIdGetURL
 * method: ApiV1AdminNodesByIdGet_TYPE
 * raw_url: ApiV1AdminNodesByIdGet_RAW_URL
 * @param id - ノードID
 */
export const ApiV1AdminNodesByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes/{id}'
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
export const ApiV1AdminNodesByIdGet_RAW_URL = function() {
  return '/api/v1/admin/nodes/{id}'
}
export const ApiV1AdminNodesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminNodesByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes/{id}'
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
 * request: ApiV1AdminNodesByIdPut
 * url: ApiV1AdminNodesByIdPutURL
 * method: ApiV1AdminNodesByIdPut_TYPE
 * raw_url: ApiV1AdminNodesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV1AdminNodesByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminNodesByIdPut_RAW_URL = function() {
  return '/api/v1/admin/nodes/{id}'
}
export const ApiV1AdminNodesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminNodesByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes/{id}'
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
 * request: ApiV1AdminNodesByIdDelete
 * url: ApiV1AdminNodesByIdDeleteURL
 * method: ApiV1AdminNodesByIdDelete_TYPE
 * raw_url: ApiV1AdminNodesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV1AdminNodesByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes/{id}'
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
export const ApiV1AdminNodesByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/nodes/{id}'
}
export const ApiV1AdminNodesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminNodesByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes/{id}'
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
 * request: ApiV1AdminNodesSync_cluster_from_dbPost
 * url: ApiV1AdminNodesSync_cluster_from_dbPostURL
 * method: ApiV1AdminNodesSync_cluster_from_dbPost_TYPE
 * raw_url: ApiV1AdminNodesSync_cluster_from_dbPost_RAW_URL
 */
export const ApiV1AdminNodesSync_cluster_from_dbPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/nodes/sync-cluster-from-db'
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
export const ApiV1AdminNodesSync_cluster_from_dbPost_RAW_URL = function() {
  return '/api/v1/admin/nodes/sync-cluster-from-db'
}
export const ApiV1AdminNodesSync_cluster_from_dbPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminNodesSync_cluster_from_dbPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/nodes/sync-cluster-from-db'
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
 * request: ApiV1NotebookSimpleGet
 * url: ApiV1NotebookSimpleGetURL
 * method: ApiV1NotebookSimpleGet_TYPE
 * raw_url: ApiV1NotebookSimpleGet_RAW_URL
 */
export const ApiV1NotebookSimpleGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/simple'
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
export const ApiV1NotebookSimpleGet_RAW_URL = function() {
  return '/api/v1/notebook/simple'
}
export const ApiV1NotebookSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookSimpleGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/simple'
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
* request: ApiV1NotebookGet
* url: ApiV1NotebookGetURL
* method: ApiV1NotebookGet_TYPE
* raw_url: ApiV1NotebookGet_RAW_URL
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
export const ApiV1NotebookGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook'
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
export const ApiV1NotebookGet_RAW_URL = function() {
  return '/api/v1/notebook'
}
export const ApiV1NotebookGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook'
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
 * request: ApiV1NotebookByIdGet
 * url: ApiV1NotebookByIdGetURL
 * method: ApiV1NotebookByIdGet_TYPE
 * raw_url: ApiV1NotebookByIdGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV1NotebookByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}'
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
export const ApiV1NotebookByIdGet_RAW_URL = function() {
  return '/api/v1/notebook/{id}'
}
export const ApiV1NotebookByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}'
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
 * request: ApiV1NotebookByIdPut
 * url: ApiV1NotebookByIdPutURL
 * method: ApiV1NotebookByIdPut_TYPE
 * raw_url: ApiV1NotebookByIdPut_RAW_URL
 * @param id - 変更対象のノートブック履歴ID
 * @param model - 変更内容
 */
export const ApiV1NotebookByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1NotebookByIdPut_RAW_URL = function() {
  return '/api/v1/notebook/{id}'
}
export const ApiV1NotebookByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1NotebookByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}'
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
 * request: ApiV1NotebookByIdDelete
 * url: ApiV1NotebookByIdDeleteURL
 * method: ApiV1NotebookByIdDelete_TYPE
 * raw_url: ApiV1NotebookByIdDelete_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV1NotebookByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}'
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
export const ApiV1NotebookByIdDelete_RAW_URL = function() {
  return '/api/v1/notebook/{id}'
}
export const ApiV1NotebookByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1NotebookByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}'
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
 * request: ApiV1NotebookByIdEventsGet
 * url: ApiV1NotebookByIdEventsGetURL
 * method: ApiV1NotebookByIdEventsGet_TYPE
 * raw_url: ApiV1NotebookByIdEventsGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV1NotebookByIdEventsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}/events'
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
export const ApiV1NotebookByIdEventsGet_RAW_URL = function() {
  return '/api/v1/notebook/{id}/events'
}
export const ApiV1NotebookByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookByIdEventsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}/events'
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
 * request: ApiV1NotebookByIdEndpointGet
 * url: ApiV1NotebookByIdEndpointGetURL
 * method: ApiV1NotebookByIdEndpointGet_TYPE
 * raw_url: ApiV1NotebookByIdEndpointGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV1NotebookByIdEndpointGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}/endpoint'
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
export const ApiV1NotebookByIdEndpointGet_RAW_URL = function() {
  return '/api/v1/notebook/{id}/endpoint'
}
export const ApiV1NotebookByIdEndpointGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookByIdEndpointGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}/endpoint'
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
 * request: ApiV1NotebookRunPost
 * url: ApiV1NotebookRunPostURL
 * method: ApiV1NotebookRunPost_TYPE
 * raw_url: ApiV1NotebookRunPost_RAW_URL
 * @param model - 新規実行内容
 */
export const ApiV1NotebookRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1NotebookRunPost_RAW_URL = function() {
  return '/api/v1/notebook/run'
}
export const ApiV1NotebookRunPost_TYPE = function() {
  return 'post'
}
export const ApiV1NotebookRunPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/run'
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
* request: ApiV1NotebookByIdContainer_filesGet
* url: ApiV1NotebookByIdContainer_filesGetURL
* method: ApiV1NotebookByIdContainer_filesGet_TYPE
* raw_url: ApiV1NotebookByIdContainer_filesGet_RAW_URL
     * @param id - 対象のノートブック履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV1NotebookByIdContainer_filesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}/container-files'
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
export const ApiV1NotebookByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v1/notebook/{id}/container-files'
}
export const ApiV1NotebookByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookByIdContainer_filesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}/container-files'
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
 * request: ApiV1NotebookByIdHaltPost
 * url: ApiV1NotebookByIdHaltPostURL
 * method: ApiV1NotebookByIdHaltPost_TYPE
 * raw_url: ApiV1NotebookByIdHaltPost_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV1NotebookByIdHaltPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}/halt'
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
export const ApiV1NotebookByIdHaltPost_RAW_URL = function() {
  return '/api/v1/notebook/{id}/halt'
}
export const ApiV1NotebookByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV1NotebookByIdHaltPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}/halt'
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
 * request: ApiV1NotebookByIdRerunPost
 * url: ApiV1NotebookByIdRerunPostURL
 * method: ApiV1NotebookByIdRerunPost_TYPE
 * raw_url: ApiV1NotebookByIdRerunPost_RAW_URL
 * @param id - ノートブック履歴ID
 * @param model - 再起動内容
 */
export const ApiV1NotebookByIdRerunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/{id}/rerun'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1NotebookByIdRerunPost_RAW_URL = function() {
  return '/api/v1/notebook/{id}/rerun'
}
export const ApiV1NotebookByIdRerunPost_TYPE = function() {
  return 'post'
}
export const ApiV1NotebookByIdRerunPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/{id}/rerun'
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
 * request: ApiV1NotebookAvailable_infinite_timeGet
 * url: ApiV1NotebookAvailable_infinite_timeGetURL
 * method: ApiV1NotebookAvailable_infinite_timeGet_TYPE
 * raw_url: ApiV1NotebookAvailable_infinite_timeGet_RAW_URL
 */
export const ApiV1NotebookAvailable_infinite_timeGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/notebook/available-infinite-time'
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
export const ApiV1NotebookAvailable_infinite_timeGet_RAW_URL = function() {
  return '/api/v1/notebook/available-infinite-time'
}
export const ApiV1NotebookAvailable_infinite_timeGet_TYPE = function() {
  return 'get'
}
export const ApiV1NotebookAvailable_infinite_timeGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/notebook/available-infinite-time'
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
* request: ApiV1PreprocessingsGet
* url: ApiV1PreprocessingsGetURL
* method: ApiV1PreprocessingsGet_TYPE
* raw_url: ApiV1PreprocessingsGet_RAW_URL
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
export const ApiV1PreprocessingsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings'
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
export const ApiV1PreprocessingsGet_RAW_URL = function() {
  return '/api/v1/preprocessings'
}
export const ApiV1PreprocessingsGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings'
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
 * request: ApiV1PreprocessingsPost
 * url: ApiV1PreprocessingsPostURL
 * method: ApiV1PreprocessingsPost_TYPE
 * raw_url: ApiV1PreprocessingsPost_RAW_URL
 * @param model - 新規作成内容
 */
export const ApiV1PreprocessingsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1PreprocessingsPost_RAW_URL = function() {
  return '/api/v1/preprocessings'
}
export const ApiV1PreprocessingsPost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings'
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
 * request: ApiV1PreprocessingsByIdGet
 * url: ApiV1PreprocessingsByIdGetURL
 * method: ApiV1PreprocessingsByIdGet_TYPE
 * raw_url: ApiV1PreprocessingsByIdGet_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV1PreprocessingsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}'
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
export const ApiV1PreprocessingsByIdGet_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}'
}
export const ApiV1PreprocessingsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}'
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
 * request: ApiV1PreprocessingsByIdPut
 * url: ApiV1PreprocessingsByIdPutURL
 * method: ApiV1PreprocessingsByIdPut_TYPE
 * raw_url: ApiV1PreprocessingsByIdPut_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param model - 変更内容
 */
export const ApiV1PreprocessingsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1PreprocessingsByIdPut_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}'
}
export const ApiV1PreprocessingsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1PreprocessingsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}'
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
 * request: ApiV1PreprocessingsByIdDelete
 * url: ApiV1PreprocessingsByIdDeleteURL
 * method: ApiV1PreprocessingsByIdDelete_TYPE
 * raw_url: ApiV1PreprocessingsByIdDelete_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV1PreprocessingsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}'
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
export const ApiV1PreprocessingsByIdDelete_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}'
}
export const ApiV1PreprocessingsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1PreprocessingsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}'
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
 * request: ApiV1PreprocessingsByIdPatch
 * url: ApiV1PreprocessingsByIdPatchURL
 * method: ApiV1PreprocessingsByIdPatch_TYPE
 * raw_url: ApiV1PreprocessingsByIdPatch_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param model - 変更内容
 */
export const ApiV1PreprocessingsByIdPatch = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('patch', domain + path, body, queryParameters, form, config)
}
export const ApiV1PreprocessingsByIdPatch_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}'
}
export const ApiV1PreprocessingsByIdPatch_TYPE = function() {
  return 'patch'
}
export const ApiV1PreprocessingsByIdPatchURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}'
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
 * request: ApiV1PreprocessingsByIdHistoriesGet
 * url: ApiV1PreprocessingsByIdHistoriesGetURL
 * method: ApiV1PreprocessingsByIdHistoriesGet_TYPE
 * raw_url: ApiV1PreprocessingsByIdHistoriesGet_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV1PreprocessingsByIdHistoriesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories'
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
export const ApiV1PreprocessingsByIdHistoriesGet_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories'
}
export const ApiV1PreprocessingsByIdHistoriesGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsByIdHistoriesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories'
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
 * request: ApiV1PreprocessingsByIdHistoriesByDataIdGet
 * url: ApiV1PreprocessingsByIdHistoriesByDataIdGetURL
 * method: ApiV1PreprocessingsByIdHistoriesByDataIdGet_TYPE
 * raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdGet_RAW_URL
 * @param id - 前処理ID
 * @param dataId - データID
 */
export const ApiV1PreprocessingsByIdHistoriesByDataIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdGet_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
* request: ApiV1PreprocessingsByIdHistoriesByDataIdPost
* url: ApiV1PreprocessingsByIdHistoriesByDataIdPostURL
* method: ApiV1PreprocessingsByIdHistoriesByDataIdPost_TYPE
* raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV1PreprocessingsByIdHistoriesByDataIdPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdPost_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdPost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
 * request: ApiV1PreprocessingsByIdHistoriesByDataIdDelete
 * url: ApiV1PreprocessingsByIdHistoriesByDataIdDeleteURL
 * method: ApiV1PreprocessingsByIdHistoriesByDataIdDelete_TYPE
 * raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdDelete_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const ApiV1PreprocessingsByIdHistoriesByDataIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdDelete_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}'
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
 * request: ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet
 * url: ApiV1PreprocessingsByIdHistoriesByDataIdEventsGetURL
 * method: ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet_TYPE
 * raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/events'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}/events'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdEventsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/events'
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
* request: ApiV1PreprocessingsByIdRunPost
* url: ApiV1PreprocessingsByIdRunPostURL
* method: ApiV1PreprocessingsByIdRunPost_TYPE
* raw_url: ApiV1PreprocessingsByIdRunPost_RAW_URL
     * @param id - 前処理ID
     * @param model - 実行設定
*/
export const ApiV1PreprocessingsByIdRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/run'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1PreprocessingsByIdRunPost_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/run'
}
export const ApiV1PreprocessingsByIdRunPost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsByIdRunPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/run'
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
* request: ApiV1PreprocessingsByIdHistoriesByDataIdDataPost
* url: ApiV1PreprocessingsByIdHistoriesByDataIdDataPostURL
* method: ApiV1PreprocessingsByIdHistoriesByDataIdDataPost_TYPE
* raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdDataPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
     * @param model - データ情報
*/
export const ApiV1PreprocessingsByIdHistoriesByDataIdDataPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/data'
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
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdDataPost_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}/data'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdDataPost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdDataPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/data'
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
* request: ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost
* url: ApiV1PreprocessingsByIdHistoriesByDataIdHaltPostURL
* method: ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost_TYPE
* raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/halt'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}/halt'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdHaltPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/halt'
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
* request: ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost
* url: ApiV1PreprocessingsByIdHistoriesByDataIdCompletePostURL
* method: ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost_TYPE
* raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/complete'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}/complete'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdCompletePostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/complete'
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
 * request: ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet
 * url: ApiV1PreprocessingsByIdHistoriesByDataIdFilesGetURL
 * method: ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet_TYPE
 * raw_url: ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet_RAW_URL
 * @param id - 対象の前処理履歴ID
 * @param dataId - 入力データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/files'
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
export const ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet_RAW_URL = function() {
  return '/api/v1/preprocessings/{id}/histories/{dataId}/files'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV1PreprocessingsByIdHistoriesByDataIdFilesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/preprocessings/{id}/histories/{dataId}/files'
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
 * request: ApiV1AdminRegistryEndpointsGet
 * url: ApiV1AdminRegistryEndpointsGetURL
 * method: ApiV1AdminRegistryEndpointsGet_TYPE
 * raw_url: ApiV1AdminRegistryEndpointsGet_RAW_URL
 */
export const ApiV1AdminRegistryEndpointsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/endpoints'
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
export const ApiV1AdminRegistryEndpointsGet_RAW_URL = function() {
  return '/api/v1/admin/registry/endpoints'
}
export const ApiV1AdminRegistryEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminRegistryEndpointsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/endpoints'
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
 * request: ApiV1AdminRegistryEndpointsPost
 * url: ApiV1AdminRegistryEndpointsPostURL
 * method: ApiV1AdminRegistryEndpointsPost_TYPE
 * raw_url: ApiV1AdminRegistryEndpointsPost_RAW_URL
 * @param model - 新規作成モデル
 */
export const ApiV1AdminRegistryEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminRegistryEndpointsPost_RAW_URL = function() {
  return '/api/v1/admin/registry/endpoints'
}
export const ApiV1AdminRegistryEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminRegistryEndpointsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/endpoints'
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
 * request: ApiV1AdminRegistryTypesGet
 * url: ApiV1AdminRegistryTypesGetURL
 * method: ApiV1AdminRegistryTypesGet_TYPE
 * raw_url: ApiV1AdminRegistryTypesGet_RAW_URL
 */
export const ApiV1AdminRegistryTypesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/types'
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
export const ApiV1AdminRegistryTypesGet_RAW_URL = function() {
  return '/api/v1/admin/registry/types'
}
export const ApiV1AdminRegistryTypesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminRegistryTypesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/types'
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
 * request: ApiV1AdminRegistryEndpointsByIdGet
 * url: ApiV1AdminRegistryEndpointsByIdGetURL
 * method: ApiV1AdminRegistryEndpointsByIdGet_TYPE
 * raw_url: ApiV1AdminRegistryEndpointsByIdGet_RAW_URL
 * @param id - レジストリID
 */
export const ApiV1AdminRegistryEndpointsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/endpoints/{id}'
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
export const ApiV1AdminRegistryEndpointsByIdGet_RAW_URL = function() {
  return '/api/v1/admin/registry/endpoints/{id}'
}
export const ApiV1AdminRegistryEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminRegistryEndpointsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/endpoints/{id}'
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
 * request: ApiV1AdminRegistryEndpointsByIdPut
 * url: ApiV1AdminRegistryEndpointsByIdPutURL
 * method: ApiV1AdminRegistryEndpointsByIdPut_TYPE
 * raw_url: ApiV1AdminRegistryEndpointsByIdPut_RAW_URL
 * @param id - 編集対象レジストリID
 * @param model - 編集モデル
 */
export const ApiV1AdminRegistryEndpointsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminRegistryEndpointsByIdPut_RAW_URL = function() {
  return '/api/v1/admin/registry/endpoints/{id}'
}
export const ApiV1AdminRegistryEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminRegistryEndpointsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/endpoints/{id}'
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
 * request: ApiV1AdminRegistryEndpointsByIdDelete
 * url: ApiV1AdminRegistryEndpointsByIdDeleteURL
 * method: ApiV1AdminRegistryEndpointsByIdDelete_TYPE
 * raw_url: ApiV1AdminRegistryEndpointsByIdDelete_RAW_URL
 * @param id - 削除対象レジストリID
 */
export const ApiV1AdminRegistryEndpointsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/registry/endpoints/{id}'
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
export const ApiV1AdminRegistryEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/registry/endpoints/{id}'
}
export const ApiV1AdminRegistryEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminRegistryEndpointsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/registry/endpoints/{id}'
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
 * request: ApiV1TenantRegistryEndpointsGet
 * url: ApiV1TenantRegistryEndpointsGetURL
 * method: ApiV1TenantRegistryEndpointsGet_TYPE
 * raw_url: ApiV1TenantRegistryEndpointsGet_RAW_URL
 */
export const ApiV1TenantRegistryEndpointsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/registry/endpoints'
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
export const ApiV1TenantRegistryEndpointsGet_RAW_URL = function() {
  return '/api/v1/tenant/registry/endpoints'
}
export const ApiV1TenantRegistryEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantRegistryEndpointsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/registry/endpoints'
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
 * request: ApiV1RegistriesByRegistryIdImagesGet
 * url: ApiV1RegistriesByRegistryIdImagesGetURL
 * method: ApiV1RegistriesByRegistryIdImagesGet_TYPE
 * raw_url: ApiV1RegistriesByRegistryIdImagesGet_RAW_URL
 * @param registryId - レジストリID
 */
export const ApiV1RegistriesByRegistryIdImagesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/registries/{registryId}/images'
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
export const ApiV1RegistriesByRegistryIdImagesGet_RAW_URL = function() {
  return '/api/v1/registries/{registryId}/images'
}
export const ApiV1RegistriesByRegistryIdImagesGet_TYPE = function() {
  return 'get'
}
export const ApiV1RegistriesByRegistryIdImagesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/registries/{registryId}/images'
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
 * request: ApiV1RegistriesByRegistryIdImagesByImageTagsGet
 * url: ApiV1RegistriesByRegistryIdImagesByImageTagsGetURL
 * method: ApiV1RegistriesByRegistryIdImagesByImageTagsGet_TYPE
 * raw_url: ApiV1RegistriesByRegistryIdImagesByImageTagsGet_RAW_URL
 * @param registryId - レジストリID
 * @param image - イメージ名
 */
export const ApiV1RegistriesByRegistryIdImagesByImageTagsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/registries/{registryId}/images/{image}/tags'
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
export const ApiV1RegistriesByRegistryIdImagesByImageTagsGet_RAW_URL = function() {
  return '/api/v1/registries/{registryId}/images/{image}/tags'
}
export const ApiV1RegistriesByRegistryIdImagesByImageTagsGet_TYPE = function() {
  return 'get'
}
export const ApiV1RegistriesByRegistryIdImagesByImageTagsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/registries/{registryId}/images/{image}/tags'
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
* request: ApiV1RegistriesByRegistryIdImagesBySegmentsGet
* url: ApiV1RegistriesByRegistryIdImagesBySegmentsGetURL
* method: ApiV1RegistriesByRegistryIdImagesBySegmentsGet_TYPE
* raw_url: ApiV1RegistriesByRegistryIdImagesBySegmentsGet_RAW_URL
     * @param registryId - 
     * @param segments - 
*/
export const ApiV1RegistriesByRegistryIdImagesBySegmentsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/registries/{registryId}/images/{segments}'
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
export const ApiV1RegistriesByRegistryIdImagesBySegmentsGet_RAW_URL = function() {
  return '/api/v1/registries/{registryId}/images/{segments}'
}
export const ApiV1RegistriesByRegistryIdImagesBySegmentsGet_TYPE = function() {
  return 'get'
}
export const ApiV1RegistriesByRegistryIdImagesBySegmentsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/registries/{registryId}/images/{segments}'
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
 * request: ApiV1AdminResourceNodesGet
 * url: ApiV1AdminResourceNodesGetURL
 * method: ApiV1AdminResourceNodesGet_TYPE
 * raw_url: ApiV1AdminResourceNodesGet_RAW_URL
 */
export const ApiV1AdminResourceNodesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/nodes'
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
export const ApiV1AdminResourceNodesGet_RAW_URL = function() {
  return '/api/v1/admin/resource/nodes'
}
export const ApiV1AdminResourceNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceNodesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/nodes'
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
 * request: ApiV1AdminResourceTenantsGet
 * url: ApiV1AdminResourceTenantsGetURL
 * method: ApiV1AdminResourceTenantsGet_TYPE
 * raw_url: ApiV1AdminResourceTenantsGet_RAW_URL
 */
export const ApiV1AdminResourceTenantsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/tenants'
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
export const ApiV1AdminResourceTenantsGet_RAW_URL = function() {
  return '/api/v1/admin/resource/tenants'
}
export const ApiV1AdminResourceTenantsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceTenantsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/tenants'
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
 * request: ApiV1AdminResourceContainersGet
 * url: ApiV1AdminResourceContainersGetURL
 * method: ApiV1AdminResourceContainersGet_TYPE
 * raw_url: ApiV1AdminResourceContainersGet_RAW_URL
 */
export const ApiV1AdminResourceContainersGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/containers'
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
export const ApiV1AdminResourceContainersGet_RAW_URL = function() {
  return '/api/v1/admin/resource/containers'
}
export const ApiV1AdminResourceContainersGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceContainersGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/containers'
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
 * request: ApiV1AdminResourceContainer_typesGet
 * url: ApiV1AdminResourceContainer_typesGetURL
 * method: ApiV1AdminResourceContainer_typesGet_TYPE
 * raw_url: ApiV1AdminResourceContainer_typesGet_RAW_URL
 */
export const ApiV1AdminResourceContainer_typesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/container-types'
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
export const ApiV1AdminResourceContainer_typesGet_RAW_URL = function() {
  return '/api/v1/admin/resource/container-types'
}
export const ApiV1AdminResourceContainer_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceContainer_typesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/container-types'
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
 * request: ApiV1AdminResourceContainersByTenantIdByNameGet
 * url: ApiV1AdminResourceContainersByTenantIdByNameGetURL
 * method: ApiV1AdminResourceContainersByTenantIdByNameGet_TYPE
 * raw_url: ApiV1AdminResourceContainersByTenantIdByNameGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV1AdminResourceContainersByTenantIdByNameGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}'
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
export const ApiV1AdminResourceContainersByTenantIdByNameGet_RAW_URL = function() {
  return '/api/v1/admin/resource/containers/{tenantId}/{name}'
}
export const ApiV1AdminResourceContainersByTenantIdByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceContainersByTenantIdByNameGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}'
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
 * request: ApiV1AdminResourceContainersByTenantIdByNameDelete
 * url: ApiV1AdminResourceContainersByTenantIdByNameDeleteURL
 * method: ApiV1AdminResourceContainersByTenantIdByNameDelete_TYPE
 * raw_url: ApiV1AdminResourceContainersByTenantIdByNameDelete_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV1AdminResourceContainersByTenantIdByNameDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}'
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
export const ApiV1AdminResourceContainersByTenantIdByNameDelete_RAW_URL = function() {
  return '/api/v1/admin/resource/containers/{tenantId}/{name}'
}
export const ApiV1AdminResourceContainersByTenantIdByNameDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminResourceContainersByTenantIdByNameDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}'
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
 * request: ApiV1AdminResourceContainersByTenantIdByNameLogGet
 * url: ApiV1AdminResourceContainersByTenantIdByNameLogGetURL
 * method: ApiV1AdminResourceContainersByTenantIdByNameLogGet_TYPE
 * raw_url: ApiV1AdminResourceContainersByTenantIdByNameLogGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV1AdminResourceContainersByTenantIdByNameLogGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}/log'
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
export const ApiV1AdminResourceContainersByTenantIdByNameLogGet_RAW_URL = function() {
  return '/api/v1/admin/resource/containers/{tenantId}/{name}/log'
}
export const ApiV1AdminResourceContainersByTenantIdByNameLogGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceContainersByTenantIdByNameLogGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}/log'
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
 * request: ApiV1AdminResourceContainersByTenantIdByNameEventsGet
 * url: ApiV1AdminResourceContainersByTenantIdByNameEventsGetURL
 * method: ApiV1AdminResourceContainersByTenantIdByNameEventsGet_TYPE
 * raw_url: ApiV1AdminResourceContainersByTenantIdByNameEventsGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV1AdminResourceContainersByTenantIdByNameEventsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}/events'
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
export const ApiV1AdminResourceContainersByTenantIdByNameEventsGet_RAW_URL = function() {
  return '/api/v1/admin/resource/containers/{tenantId}/{name}/events'
}
export const ApiV1AdminResourceContainersByTenantIdByNameEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminResourceContainersByTenantIdByNameEventsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/resource/containers/{tenantId}/{name}/events'
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
 * request: ApiV1TenantResourceContainersGet
 * url: ApiV1TenantResourceContainersGetURL
 * method: ApiV1TenantResourceContainersGet_TYPE
 * raw_url: ApiV1TenantResourceContainersGet_RAW_URL
 */
export const ApiV1TenantResourceContainersGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/resource/containers'
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
export const ApiV1TenantResourceContainersGet_RAW_URL = function() {
  return '/api/v1/tenant/resource/containers'
}
export const ApiV1TenantResourceContainersGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantResourceContainersGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/resource/containers'
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
 * request: ApiV1TenantResourceNodesGet
 * url: ApiV1TenantResourceNodesGetURL
 * method: ApiV1TenantResourceNodesGet_TYPE
 * raw_url: ApiV1TenantResourceNodesGet_RAW_URL
 */
export const ApiV1TenantResourceNodesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/resource/nodes'
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
export const ApiV1TenantResourceNodesGet_RAW_URL = function() {
  return '/api/v1/tenant/resource/nodes'
}
export const ApiV1TenantResourceNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantResourceNodesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/resource/nodes'
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
 * request: ApiV1TenantResourceContainersByNameGet
 * url: ApiV1TenantResourceContainersByNameGetURL
 * method: ApiV1TenantResourceContainersByNameGet_TYPE
 * raw_url: ApiV1TenantResourceContainersByNameGet_RAW_URL
 * @param name - コンテナ名
 */
export const ApiV1TenantResourceContainersByNameGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/resource/containers/{name}'
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
export const ApiV1TenantResourceContainersByNameGet_RAW_URL = function() {
  return '/api/v1/tenant/resource/containers/{name}'
}
export const ApiV1TenantResourceContainersByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantResourceContainersByNameGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/resource/containers/{name}'
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
 * request: ApiV1TenantResourceContainersByNameDelete
 * url: ApiV1TenantResourceContainersByNameDeleteURL
 * method: ApiV1TenantResourceContainersByNameDelete_TYPE
 * raw_url: ApiV1TenantResourceContainersByNameDelete_RAW_URL
 * @param name - 
 */
export const ApiV1TenantResourceContainersByNameDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/resource/containers/{name}'
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
export const ApiV1TenantResourceContainersByNameDelete_RAW_URL = function() {
  return '/api/v1/tenant/resource/containers/{name}'
}
export const ApiV1TenantResourceContainersByNameDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TenantResourceContainersByNameDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/resource/containers/{name}'
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
 * request: ApiV1TenantResourceContainersByNameLogGet
 * url: ApiV1TenantResourceContainersByNameLogGetURL
 * method: ApiV1TenantResourceContainersByNameLogGet_TYPE
 * raw_url: ApiV1TenantResourceContainersByNameLogGet_RAW_URL
 * @param name - コンテナ名
 */
export const ApiV1TenantResourceContainersByNameLogGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/resource/containers/{name}/log'
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
export const ApiV1TenantResourceContainersByNameLogGet_RAW_URL = function() {
  return '/api/v1/tenant/resource/containers/{name}/log'
}
export const ApiV1TenantResourceContainersByNameLogGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantResourceContainersByNameLogGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/resource/containers/{name}/log'
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
 * 全ロール一覧を取得
 * request: ApiV1AdminRolesGet
 * url: ApiV1AdminRolesGetURL
 * method: ApiV1AdminRolesGet_TYPE
 * raw_url: ApiV1AdminRolesGet_RAW_URL
 */
export const ApiV1AdminRolesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/roles'
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
export const ApiV1AdminRolesGet_RAW_URL = function() {
  return '/api/v1/admin/roles'
}
export const ApiV1AdminRolesGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminRolesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/roles'
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
 * request: ApiV1AdminRolesPost
 * url: ApiV1AdminRolesPostURL
 * method: ApiV1AdminRolesPost_TYPE
 * raw_url: ApiV1AdminRolesPost_RAW_URL
 * @param model - 
 */
export const ApiV1AdminRolesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminRolesPost_RAW_URL = function() {
  return '/api/v1/admin/roles'
}
export const ApiV1AdminRolesPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminRolesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/roles'
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
 * request: ApiV1AdminRolesByIdGet
 * url: ApiV1AdminRolesByIdGetURL
 * method: ApiV1AdminRolesByIdGet_TYPE
 * raw_url: ApiV1AdminRolesByIdGet_RAW_URL
 * @param id - ロールID
 */
export const ApiV1AdminRolesByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/roles/{id}'
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
export const ApiV1AdminRolesByIdGet_RAW_URL = function() {
  return '/api/v1/admin/roles/{id}'
}
export const ApiV1AdminRolesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminRolesByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/roles/{id}'
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
 * request: ApiV1AdminRolesByIdPut
 * url: ApiV1AdminRolesByIdPutURL
 * method: ApiV1AdminRolesByIdPut_TYPE
 * raw_url: ApiV1AdminRolesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV1AdminRolesByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminRolesByIdPut_RAW_URL = function() {
  return '/api/v1/admin/roles/{id}'
}
export const ApiV1AdminRolesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminRolesByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/roles/{id}'
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
 * request: ApiV1AdminRolesByIdDelete
 * url: ApiV1AdminRolesByIdDeleteURL
 * method: ApiV1AdminRolesByIdDelete_TYPE
 * raw_url: ApiV1AdminRolesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV1AdminRolesByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/roles/{id}'
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
export const ApiV1AdminRolesByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/roles/{id}'
}
export const ApiV1AdminRolesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminRolesByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/roles/{id}'
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
 * request: ApiV1TenantRolesGet
 * url: ApiV1TenantRolesGetURL
 * method: ApiV1TenantRolesGet_TYPE
 * raw_url: ApiV1TenantRolesGet_RAW_URL
 */
export const ApiV1TenantRolesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/roles'
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
export const ApiV1TenantRolesGet_RAW_URL = function() {
  return '/api/v1/tenant/roles'
}
export const ApiV1TenantRolesGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantRolesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/roles'
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
 * request: ApiV1TenantRolesPost
 * url: ApiV1TenantRolesPostURL
 * method: ApiV1TenantRolesPost_TYPE
 * raw_url: ApiV1TenantRolesPost_RAW_URL
 * @param model - 
 */
export const ApiV1TenantRolesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/roles'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1TenantRolesPost_RAW_URL = function() {
  return '/api/v1/tenant/roles'
}
export const ApiV1TenantRolesPost_TYPE = function() {
  return 'post'
}
export const ApiV1TenantRolesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/roles'
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
 * request: ApiV1TenantRolesByIdGet
 * url: ApiV1TenantRolesByIdGetURL
 * method: ApiV1TenantRolesByIdGet_TYPE
 * raw_url: ApiV1TenantRolesByIdGet_RAW_URL
 * @param id - ロールID
 */
export const ApiV1TenantRolesByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/roles/{id}'
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
export const ApiV1TenantRolesByIdGet_RAW_URL = function() {
  return '/api/v1/tenant/roles/{id}'
}
export const ApiV1TenantRolesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantRolesByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/roles/{id}'
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
 * request: ApiV1TenantRolesByIdPut
 * url: ApiV1TenantRolesByIdPutURL
 * method: ApiV1TenantRolesByIdPut_TYPE
 * raw_url: ApiV1TenantRolesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV1TenantRolesByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/roles/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TenantRolesByIdPut_RAW_URL = function() {
  return '/api/v1/tenant/roles/{id}'
}
export const ApiV1TenantRolesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1TenantRolesByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/roles/{id}'
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
 * request: ApiV1TenantRolesByIdDelete
 * url: ApiV1TenantRolesByIdDeleteURL
 * method: ApiV1TenantRolesByIdDelete_TYPE
 * raw_url: ApiV1TenantRolesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV1TenantRolesByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/roles/{id}'
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
export const ApiV1TenantRolesByIdDelete_RAW_URL = function() {
  return '/api/v1/tenant/roles/{id}'
}
export const ApiV1TenantRolesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TenantRolesByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/roles/{id}'
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
 * request: ApiV1AdminStorageEndpointsGet
 * url: ApiV1AdminStorageEndpointsGetURL
 * method: ApiV1AdminStorageEndpointsGet_TYPE
 * raw_url: ApiV1AdminStorageEndpointsGet_RAW_URL
 */
export const ApiV1AdminStorageEndpointsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/storage/endpoints'
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
export const ApiV1AdminStorageEndpointsGet_RAW_URL = function() {
  return '/api/v1/admin/storage/endpoints'
}
export const ApiV1AdminStorageEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminStorageEndpointsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/storage/endpoints'
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
 * request: ApiV1AdminStorageEndpointsPost
 * url: ApiV1AdminStorageEndpointsPostURL
 * method: ApiV1AdminStorageEndpointsPost_TYPE
 * raw_url: ApiV1AdminStorageEndpointsPost_RAW_URL
 * @param model - 
 */
export const ApiV1AdminStorageEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/storage/endpoints'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminStorageEndpointsPost_RAW_URL = function() {
  return '/api/v1/admin/storage/endpoints'
}
export const ApiV1AdminStorageEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminStorageEndpointsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/storage/endpoints'
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
 * request: ApiV1AdminStorageEndpointsByIdGet
 * url: ApiV1AdminStorageEndpointsByIdGetURL
 * method: ApiV1AdminStorageEndpointsByIdGet_TYPE
 * raw_url: ApiV1AdminStorageEndpointsByIdGet_RAW_URL
 * @param id - StorageエンドポイントID
 */
export const ApiV1AdminStorageEndpointsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/storage/endpoints/{id}'
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
export const ApiV1AdminStorageEndpointsByIdGet_RAW_URL = function() {
  return '/api/v1/admin/storage/endpoints/{id}'
}
export const ApiV1AdminStorageEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminStorageEndpointsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/storage/endpoints/{id}'
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
 * request: ApiV1AdminStorageEndpointsByIdPut
 * url: ApiV1AdminStorageEndpointsByIdPutURL
 * method: ApiV1AdminStorageEndpointsByIdPut_TYPE
 * raw_url: ApiV1AdminStorageEndpointsByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV1AdminStorageEndpointsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/storage/endpoints/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminStorageEndpointsByIdPut_RAW_URL = function() {
  return '/api/v1/admin/storage/endpoints/{id}'
}
export const ApiV1AdminStorageEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminStorageEndpointsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/storage/endpoints/{id}'
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
 * request: ApiV1AdminStorageEndpointsByIdDelete
 * url: ApiV1AdminStorageEndpointsByIdDeleteURL
 * method: ApiV1AdminStorageEndpointsByIdDelete_TYPE
 * raw_url: ApiV1AdminStorageEndpointsByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV1AdminStorageEndpointsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/storage/endpoints/{id}'
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
export const ApiV1AdminStorageEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/storage/endpoints/{id}'
}
export const ApiV1AdminStorageEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminStorageEndpointsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/storage/endpoints/{id}'
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
 * request: ApiV1UploadParameterGet
 * url: ApiV1UploadParameterGetURL
 * method: ApiV1UploadParameterGet_TYPE
 * raw_url: ApiV1UploadParameterGet_RAW_URL
 * @param fileName - ファイル名
 * @param partSum - 分割数
 * @param type - リソース名
 */
export const ApiV1UploadParameterGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/upload/parameter'
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
export const ApiV1UploadParameterGet_RAW_URL = function() {
  return '/api/v1/upload/parameter'
}
export const ApiV1UploadParameterGet_TYPE = function() {
  return 'get'
}
export const ApiV1UploadParameterGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/upload/parameter'
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
 * request: ApiV1UploadCompletePost
 * url: ApiV1UploadCompletePostURL
 * method: ApiV1UploadCompletePost_TYPE
 * raw_url: ApiV1UploadCompletePost_RAW_URL
 * @param model - 
 */
export const ApiV1UploadCompletePost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/upload/complete'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1UploadCompletePost_RAW_URL = function() {
  return '/api/v1/upload/complete'
}
export const ApiV1UploadCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV1UploadCompletePostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/upload/complete'
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
 * request: ApiV1DownloadUrlGet
 * url: ApiV1DownloadUrlGetURL
 * method: ApiV1DownloadUrlGet_TYPE
 * raw_url: ApiV1DownloadUrlGet_RAW_URL
 * @param type - リソース種別
 * @param storedPath - 保存先パス
 * @param fileName - ダウンロード時に書き換えるファイル名
 * @param secure - HTTPS化するか
 */
export const ApiV1DownloadUrlGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/download/url'
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
export const ApiV1DownloadUrlGet_RAW_URL = function() {
  return '/api/v1/download/url'
}
export const ApiV1DownloadUrlGet_TYPE = function() {
  return 'get'
}
export const ApiV1DownloadUrlGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/download/url'
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
 * テナント一覧を取得
 * request: ApiV1AdminTenantsGet
 * url: ApiV1AdminTenantsGetURL
 * method: ApiV1AdminTenantsGet_TYPE
 * raw_url: ApiV1AdminTenantsGet_RAW_URL
 */
export const ApiV1AdminTenantsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tenants'
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
export const ApiV1AdminTenantsGet_RAW_URL = function() {
  return '/api/v1/admin/tenants'
}
export const ApiV1AdminTenantsGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminTenantsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tenants'
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
 * request: ApiV1AdminTenantsPost
 * url: ApiV1AdminTenantsPostURL
 * method: ApiV1AdminTenantsPost_TYPE
 * raw_url: ApiV1AdminTenantsPost_RAW_URL
 * @param model - 登録内容
 */
export const ApiV1AdminTenantsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tenants'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminTenantsPost_RAW_URL = function() {
  return '/api/v1/admin/tenants'
}
export const ApiV1AdminTenantsPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminTenantsPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tenants'
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
 * request: ApiV1AdminTenantsByIdGet
 * url: ApiV1AdminTenantsByIdGetURL
 * method: ApiV1AdminTenantsByIdGet_TYPE
 * raw_url: ApiV1AdminTenantsByIdGet_RAW_URL
 * @param id - テナントID
 */
export const ApiV1AdminTenantsByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tenants/{id}'
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
export const ApiV1AdminTenantsByIdGet_RAW_URL = function() {
  return '/api/v1/admin/tenants/{id}'
}
export const ApiV1AdminTenantsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminTenantsByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tenants/{id}'
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
 * request: ApiV1AdminTenantsByIdPut
 * url: ApiV1AdminTenantsByIdPutURL
 * method: ApiV1AdminTenantsByIdPut_TYPE
 * raw_url: ApiV1AdminTenantsByIdPut_RAW_URL
 * @param id - テナントID
 * @param model - 変更内容
 */
export const ApiV1AdminTenantsByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tenants/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminTenantsByIdPut_RAW_URL = function() {
  return '/api/v1/admin/tenants/{id}'
}
export const ApiV1AdminTenantsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminTenantsByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tenants/{id}'
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
 * request: ApiV1AdminTenantsByIdDelete
 * url: ApiV1AdminTenantsByIdDeleteURL
 * method: ApiV1AdminTenantsByIdDelete_TYPE
 * raw_url: ApiV1AdminTenantsByIdDelete_RAW_URL
 * @param id - テナントID
 */
export const ApiV1AdminTenantsByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/tenants/{id}'
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
export const ApiV1AdminTenantsByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/tenants/{id}'
}
export const ApiV1AdminTenantsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminTenantsByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/tenants/{id}'
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
 * request: ApiV1TenantGet
 * url: ApiV1TenantGetURL
 * method: ApiV1TenantGet_TYPE
 * raw_url: ApiV1TenantGet_RAW_URL
 */
export const ApiV1TenantGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant'
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
export const ApiV1TenantGet_RAW_URL = function() {
  return '/api/v1/tenant'
}
export const ApiV1TenantGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant'
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
 * request: ApiV1TenantPut
 * url: ApiV1TenantPutURL
 * method: ApiV1TenantPut_TYPE
 * raw_url: ApiV1TenantPut_RAW_URL
 * @param model - 変更内容
 */
export const ApiV1TenantPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TenantPut_RAW_URL = function() {
  return '/api/v1/tenant'
}
export const ApiV1TenantPut_TYPE = function() {
  return 'put'
}
export const ApiV1TenantPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant'
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
 * request: ApiV1TrainingSimpleGet
 * url: ApiV1TrainingSimpleGetURL
 * method: ApiV1TrainingSimpleGet_TYPE
 * raw_url: ApiV1TrainingSimpleGet_RAW_URL
 */
export const ApiV1TrainingSimpleGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/simple'
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
export const ApiV1TrainingSimpleGet_RAW_URL = function() {
  return '/api/v1/training/simple'
}
export const ApiV1TrainingSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingSimpleGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/simple'
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
* request: ApiV1TrainingGet
* url: ApiV1TrainingGetURL
* method: ApiV1TrainingGet_TYPE
* raw_url: ApiV1TrainingGet_RAW_URL
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
export const ApiV1TrainingGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training'
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
export const ApiV1TrainingGet_RAW_URL = function() {
  return '/api/v1/training'
}
export const ApiV1TrainingGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training'
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
 * マウントする学習履歴を取得
 * request: ApiV1TrainingMountGet
 * url: ApiV1TrainingMountGetURL
 * method: ApiV1TrainingMountGet_TYPE
 * raw_url: ApiV1TrainingMountGet_RAW_URL
 * @param status - ステータス
 */
export const ApiV1TrainingMountGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/mount'
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
export const ApiV1TrainingMountGet_RAW_URL = function() {
  return '/api/v1/training/mount'
}
export const ApiV1TrainingMountGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingMountGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/mount'
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
 * request: ApiV1TrainingByIdGet
 * url: ApiV1TrainingByIdGetURL
 * method: ApiV1TrainingByIdGet_TYPE
 * raw_url: ApiV1TrainingByIdGet_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV1TrainingByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}'
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
export const ApiV1TrainingByIdGet_RAW_URL = function() {
  return '/api/v1/training/{id}'
}
export const ApiV1TrainingByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}'
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
 * request: ApiV1TrainingByIdPut
 * url: ApiV1TrainingByIdPutURL
 * method: ApiV1TrainingByIdPut_TYPE
 * raw_url: ApiV1TrainingByIdPut_RAW_URL
 * @param id - 変更対象の学習履歴ID
 * @param model - 変更内容
 */
export const ApiV1TrainingByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TrainingByIdPut_RAW_URL = function() {
  return '/api/v1/training/{id}'
}
export const ApiV1TrainingByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1TrainingByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}'
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
 * request: ApiV1TrainingByIdDelete
 * url: ApiV1TrainingByIdDeleteURL
 * method: ApiV1TrainingByIdDelete_TYPE
 * raw_url: ApiV1TrainingByIdDelete_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV1TrainingByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}'
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
export const ApiV1TrainingByIdDelete_RAW_URL = function() {
  return '/api/v1/training/{id}'
}
export const ApiV1TrainingByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TrainingByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}'
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
 * request: ApiV1TrainingByIdEventsGet
 * url: ApiV1TrainingByIdEventsGetURL
 * method: ApiV1TrainingByIdEventsGet_TYPE
 * raw_url: ApiV1TrainingByIdEventsGet_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV1TrainingByIdEventsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/events'
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
export const ApiV1TrainingByIdEventsGet_RAW_URL = function() {
  return '/api/v1/training/{id}/events'
}
export const ApiV1TrainingByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingByIdEventsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/events'
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
 * request: ApiV1TrainingRunPost
 * url: ApiV1TrainingRunPostURL
 * method: ApiV1TrainingRunPost_TYPE
 * raw_url: ApiV1TrainingRunPost_RAW_URL
 * @param model - 新規学習実行内容
 */
export const ApiV1TrainingRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/run'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1TrainingRunPost_RAW_URL = function() {
  return '/api/v1/training/run'
}
export const ApiV1TrainingRunPost_TYPE = function() {
  return 'post'
}
export const ApiV1TrainingRunPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/run'
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
 * request: ApiV1TrainingByIdFilesGet
 * url: ApiV1TrainingByIdFilesGetURL
 * method: ApiV1TrainingByIdFilesGet_TYPE
 * raw_url: ApiV1TrainingByIdFilesGet_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV1TrainingByIdFilesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/files'
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
export const ApiV1TrainingByIdFilesGet_RAW_URL = function() {
  return '/api/v1/training/{id}/files'
}
export const ApiV1TrainingByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingByIdFilesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/files'
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
 * 学習履歴添付ファイルを登録する。
 * request: ApiV1TrainingByIdFilesPost
 * url: ApiV1TrainingByIdFilesPostURL
 * method: ApiV1TrainingByIdFilesPost_TYPE
 * raw_url: ApiV1TrainingByIdFilesPost_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param model - 追加するファイル情報
 */
export const ApiV1TrainingByIdFilesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/files'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1TrainingByIdFilesPost_RAW_URL = function() {
  return '/api/v1/training/{id}/files'
}
export const ApiV1TrainingByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV1TrainingByIdFilesPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/files'
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
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: ApiV1TrainingByIdContainer_filesGet
* url: ApiV1TrainingByIdContainer_filesGetURL
* method: ApiV1TrainingByIdContainer_filesGet_TYPE
* raw_url: ApiV1TrainingByIdContainer_filesGet_RAW_URL
     * @param id - 対象の学習履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV1TrainingByIdContainer_filesGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/container-files'
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
export const ApiV1TrainingByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v1/training/{id}/container-files'
}
export const ApiV1TrainingByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingByIdContainer_filesGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/container-files'
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
 * request: ApiV1TrainingByIdFilesByFileIdDelete
 * url: ApiV1TrainingByIdFilesByFileIdDeleteURL
 * method: ApiV1TrainingByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV1TrainingByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param fileId - 削除するファイルのID
 */
export const ApiV1TrainingByIdFilesByFileIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/files/{fileId}'
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
export const ApiV1TrainingByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v1/training/{id}/files/{fileId}'
}
export const ApiV1TrainingByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TrainingByIdFilesByFileIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/files/{fileId}'
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
 * 指定したTensorBoardコンテナ情報を取得する
 * request: ApiV1TrainingByIdTensorboardGet
 * url: ApiV1TrainingByIdTensorboardGetURL
 * method: ApiV1TrainingByIdTensorboardGet_TYPE
 * raw_url: ApiV1TrainingByIdTensorboardGet_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const ApiV1TrainingByIdTensorboardGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/tensorboard'
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
export const ApiV1TrainingByIdTensorboardGet_RAW_URL = function() {
  return '/api/v1/training/{id}/tensorboard'
}
export const ApiV1TrainingByIdTensorboardGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingByIdTensorboardGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/tensorboard'
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
 * request: ApiV1TrainingByIdTensorboardPut
 * url: ApiV1TrainingByIdTensorboardPutURL
 * method: ApiV1TrainingByIdTensorboardPut_TYPE
 * raw_url: ApiV1TrainingByIdTensorboardPut_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param model - 起動モデル
 */
export const ApiV1TrainingByIdTensorboardPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/tensorboard'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TrainingByIdTensorboardPut_RAW_URL = function() {
  return '/api/v1/training/{id}/tensorboard'
}
export const ApiV1TrainingByIdTensorboardPut_TYPE = function() {
  return 'put'
}
export const ApiV1TrainingByIdTensorboardPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/tensorboard'
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
 * request: ApiV1TrainingByIdTensorboardDelete
 * url: ApiV1TrainingByIdTensorboardDeleteURL
 * method: ApiV1TrainingByIdTensorboardDelete_TYPE
 * raw_url: ApiV1TrainingByIdTensorboardDelete_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const ApiV1TrainingByIdTensorboardDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/tensorboard'
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
export const ApiV1TrainingByIdTensorboardDelete_RAW_URL = function() {
  return '/api/v1/training/{id}/tensorboard'
}
export const ApiV1TrainingByIdTensorboardDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TrainingByIdTensorboardDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/tensorboard'
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
 * request: ApiV1TrainingByIdHaltPost
 * url: ApiV1TrainingByIdHaltPostURL
 * method: ApiV1TrainingByIdHaltPost_TYPE
 * raw_url: ApiV1TrainingByIdHaltPost_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV1TrainingByIdHaltPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/halt'
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
export const ApiV1TrainingByIdHaltPost_RAW_URL = function() {
  return '/api/v1/training/{id}/halt'
}
export const ApiV1TrainingByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV1TrainingByIdHaltPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/halt'
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
* request: ApiV1TrainingByIdUser_cancelPost
* url: ApiV1TrainingByIdUser_cancelPostURL
* method: ApiV1TrainingByIdUser_cancelPost_TYPE
* raw_url: ApiV1TrainingByIdUser_cancelPost_RAW_URL
     * @param id - 学習履歴ID
*/
export const ApiV1TrainingByIdUser_cancelPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/user-cancel'
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
export const ApiV1TrainingByIdUser_cancelPost_RAW_URL = function() {
  return '/api/v1/training/{id}/user-cancel'
}
export const ApiV1TrainingByIdUser_cancelPost_TYPE = function() {
  return 'post'
}
export const ApiV1TrainingByIdUser_cancelPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/user-cancel'
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
 * request: ApiV1TrainingByIdCompletePost
 * url: ApiV1TrainingByIdCompletePostURL
 * method: ApiV1TrainingByIdCompletePost_TYPE
 * raw_url: ApiV1TrainingByIdCompletePost_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV1TrainingByIdCompletePost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/{id}/complete'
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
export const ApiV1TrainingByIdCompletePost_RAW_URL = function() {
  return '/api/v1/training/{id}/complete'
}
export const ApiV1TrainingByIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV1TrainingByIdCompletePostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/{id}/complete'
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
 * request: ApiV1TrainingTagsGet
 * url: ApiV1TrainingTagsGetURL
 * method: ApiV1TrainingTagsGet_TYPE
 * raw_url: ApiV1TrainingTagsGet_RAW_URL
 */
export const ApiV1TrainingTagsGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/training/tags'
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
export const ApiV1TrainingTagsGet_RAW_URL = function() {
  return '/api/v1/training/tags'
}
export const ApiV1TrainingTagsGet_TYPE = function() {
  return 'get'
}
export const ApiV1TrainingTagsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/training/tags'
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
 * request: ApiV1AdminUsersGet
 * url: ApiV1AdminUsersGetURL
 * method: ApiV1AdminUsersGet_TYPE
 * raw_url: ApiV1AdminUsersGet_RAW_URL
 */
export const ApiV1AdminUsersGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users'
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
export const ApiV1AdminUsersGet_RAW_URL = function() {
  return '/api/v1/admin/users'
}
export const ApiV1AdminUsersGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminUsersGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users'
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
 * request: ApiV1AdminUsersPost
 * url: ApiV1AdminUsersPostURL
 * method: ApiV1AdminUsersPost_TYPE
 * raw_url: ApiV1AdminUsersPost_RAW_URL
 * @param model - 
 */
export const ApiV1AdminUsersPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users'
  let body
  let queryParameters = {}
  let form = {}
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('post', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminUsersPost_RAW_URL = function() {
  return '/api/v1/admin/users'
}
export const ApiV1AdminUsersPost_TYPE = function() {
  return 'post'
}
export const ApiV1AdminUsersPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users'
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
 * request: ApiV1AdminUsersByIdGet
 * url: ApiV1AdminUsersByIdGetURL
 * method: ApiV1AdminUsersByIdGet_TYPE
 * raw_url: ApiV1AdminUsersByIdGet_RAW_URL
 * @param id - 
 */
export const ApiV1AdminUsersByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users/{id}'
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
export const ApiV1AdminUsersByIdGet_RAW_URL = function() {
  return '/api/v1/admin/users/{id}'
}
export const ApiV1AdminUsersByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1AdminUsersByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users/{id}'
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
 * request: ApiV1AdminUsersByIdPut
 * url: ApiV1AdminUsersByIdPutURL
 * method: ApiV1AdminUsersByIdPut_TYPE
 * raw_url: ApiV1AdminUsersByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV1AdminUsersByIdPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users/{id}'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['model'] !== undefined) {
    body = parameters['model']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminUsersByIdPut_RAW_URL = function() {
  return '/api/v1/admin/users/{id}'
}
export const ApiV1AdminUsersByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminUsersByIdPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users/{id}'
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
 * request: ApiV1AdminUsersByIdDelete
 * url: ApiV1AdminUsersByIdDeleteURL
 * method: ApiV1AdminUsersByIdDelete_TYPE
 * raw_url: ApiV1AdminUsersByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV1AdminUsersByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users/{id}'
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
export const ApiV1AdminUsersByIdDelete_RAW_URL = function() {
  return '/api/v1/admin/users/{id}'
}
export const ApiV1AdminUsersByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1AdminUsersByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users/{id}'
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
 * request: ApiV1AdminUsersByIdPasswordPut
 * url: ApiV1AdminUsersByIdPasswordPutURL
 * method: ApiV1AdminUsersByIdPasswordPut_TYPE
 * raw_url: ApiV1AdminUsersByIdPasswordPut_RAW_URL
 * @param id - 
 * @param password - 
 */
export const ApiV1AdminUsersByIdPasswordPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/admin/users/{id}/password'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['password'] !== undefined) {
    body = parameters['password']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1AdminUsersByIdPasswordPut_RAW_URL = function() {
  return '/api/v1/admin/users/{id}/password'
}
export const ApiV1AdminUsersByIdPasswordPut_TYPE = function() {
  return 'put'
}
export const ApiV1AdminUsersByIdPasswordPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/admin/users/{id}/password'
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
 * テナント向けに、所属しているユーザの一覧を取得する。
 * request: ApiV1TenantUsersGet
 * url: ApiV1TenantUsersGetURL
 * method: ApiV1TenantUsersGet_TYPE
 * raw_url: ApiV1TenantUsersGet_RAW_URL
 */
export const ApiV1TenantUsersGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/users'
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
export const ApiV1TenantUsersGet_RAW_URL = function() {
  return '/api/v1/tenant/users'
}
export const ApiV1TenantUsersGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantUsersGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/users'
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
 * request: ApiV1TenantUsersByIdGet
 * url: ApiV1TenantUsersByIdGetURL
 * method: ApiV1TenantUsersByIdGet_TYPE
 * raw_url: ApiV1TenantUsersByIdGet_RAW_URL
 * @param id - 
 */
export const ApiV1TenantUsersByIdGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/users/{id}'
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
export const ApiV1TenantUsersByIdGet_RAW_URL = function() {
  return '/api/v1/tenant/users/{id}'
}
export const ApiV1TenantUsersByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV1TenantUsersByIdGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/users/{id}'
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
 * request: ApiV1TenantUsersByIdDelete
 * url: ApiV1TenantUsersByIdDeleteURL
 * method: ApiV1TenantUsersByIdDelete_TYPE
 * raw_url: ApiV1TenantUsersByIdDelete_RAW_URL
 * @param id - ユーザID
 */
export const ApiV1TenantUsersByIdDelete = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/users/{id}'
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
export const ApiV1TenantUsersByIdDelete_RAW_URL = function() {
  return '/api/v1/tenant/users/{id}'
}
export const ApiV1TenantUsersByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV1TenantUsersByIdDeleteURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/users/{id}'
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
 * request: ApiV1TenantUsersByIdRolesPut
 * url: ApiV1TenantUsersByIdRolesPutURL
 * method: ApiV1TenantUsersByIdRolesPut_TYPE
 * raw_url: ApiV1TenantUsersByIdRolesPut_RAW_URL
 * @param id - ユーザID
 * @param roleIds - 登録するロールID
 */
export const ApiV1TenantUsersByIdRolesPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/tenant/users/{id}/roles'
  let body
  let queryParameters = {}
  let form = {}
  path = path.replace('{id}', `${parameters['id']}`)
  if (parameters['id'] === undefined) {
    return Promise.reject(new Error('Missing required  parameter: id'))
  }
  if (parameters['roleIds'] !== undefined) {
    body = parameters['roleIds']
  }
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('put', domain + path, body, queryParameters, form, config)
}
export const ApiV1TenantUsersByIdRolesPut_RAW_URL = function() {
  return '/api/v1/tenant/users/{id}/roles'
}
export const ApiV1TenantUsersByIdRolesPut_TYPE = function() {
  return 'put'
}
export const ApiV1TenantUsersByIdRolesPutURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/tenant/users/{id}/roles'
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
 * request: ApiV1VersionGet
 * url: ApiV1VersionGetURL
 * method: ApiV1VersionGet_TYPE
 * raw_url: ApiV1VersionGet_RAW_URL
 */
export const ApiV1VersionGet = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v1/version'
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
export const ApiV1VersionGet_RAW_URL = function() {
  return '/api/v1/version'
}
export const ApiV1VersionGet_TYPE = function() {
  return 'get'
}
export const ApiV1VersionGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v1/version'
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    })
  }
  let keys = Object.keys(queryParameters)
  return domain + path + (keys.length > 0 ? '?' + (keys.map(key => key + '=' + encodeURIComponent(queryParameters[key])).join('&')) : '')
}
