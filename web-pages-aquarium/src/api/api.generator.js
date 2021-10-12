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
 * request: ApiV2AccountGet
 * url: ApiV2AccountGetURL
 * method: ApiV2AccountGet_TYPE
 * raw_url: ApiV2AccountGet_RAW_URL
 */
export const ApiV2AccountGet = function(parameters = {}) {
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
export const ApiV2AccountGet_RAW_URL = function() {
  return '/api/v2/account'
}
export const ApiV2AccountGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountPut
 * url: ApiV2AccountPutURL
 * method: ApiV2AccountPut_TYPE
 * raw_url: ApiV2AccountPut_RAW_URL
 * @param defaultTenant - デフォルトテナント
 */
export const ApiV2AccountPut = function(parameters = {}) {
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
export const ApiV2AccountPut_RAW_URL = function() {
  return '/api/v2/account'
}
export const ApiV2AccountPut_TYPE = function() {
  return 'put'
}
export const ApiV2AccountPutURL = function(parameters = {}) {
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
 * request: ApiV2AccountPasswordPut
 * url: ApiV2AccountPasswordPutURL
 * method: ApiV2AccountPasswordPut_TYPE
 * raw_url: ApiV2AccountPasswordPut_RAW_URL
 * @param model - 
 */
export const ApiV2AccountPasswordPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/password'
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
export const ApiV2AccountPasswordPut_RAW_URL = function() {
  return '/api/v2/account/password'
}
export const ApiV2AccountPasswordPut_TYPE = function() {
  return 'put'
}
export const ApiV2AccountPasswordPutURL = function(parameters = {}) {
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
 * request: ApiV2AccountLoginPost
 * url: ApiV2AccountLoginPostURL
 * method: ApiV2AccountLoginPost_TYPE
 * raw_url: ApiV2AccountLoginPost_RAW_URL
 * @param model - 
 */
export const ApiV2AccountLoginPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/login'
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
export const ApiV2AccountLoginPost_RAW_URL = function() {
  return '/api/v2/account/login'
}
export const ApiV2AccountLoginPost_TYPE = function() {
  return 'post'
}
export const ApiV2AccountLoginPostURL = function(parameters = {}) {
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
 * request: ApiV2AccountTenantsByTenantIdTokenPost
 * url: ApiV2AccountTenantsByTenantIdTokenPostURL
 * method: ApiV2AccountTenantsByTenantIdTokenPost_TYPE
 * raw_url: ApiV2AccountTenantsByTenantIdTokenPost_RAW_URL
 * @param tenantId - テナントID
 * @param expiresIn - 有効期限(秒)。省略時はシステムの既定値。
 */
export const ApiV2AccountTenantsByTenantIdTokenPost = function(parameters = {}) {
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
export const ApiV2AccountTenantsByTenantIdTokenPost_RAW_URL = function() {
  return '/api/v2/account/tenants/{tenantId}/token'
}
export const ApiV2AccountTenantsByTenantIdTokenPost_TYPE = function() {
  return 'post'
}
export const ApiV2AccountTenantsByTenantIdTokenPostURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/account/tenants/{tenantId}/token'
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
 * アクセス可能なKQIのメニュー一覧をツリー形式で取得する。
 * request: ApiV2AccountMenusTreeGet
 * url: ApiV2AccountMenusTreeGetURL
 * method: ApiV2AccountMenusTreeGet_TYPE
 * raw_url: ApiV2AccountMenusTreeGet_RAW_URL
 * @param lang - 
 */
export const ApiV2AccountMenusTreeGet = function(parameters = {}) {
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
export const ApiV2AccountMenusTreeGet_RAW_URL = function() {
  return '/api/v2/account/menus/tree'
}
export const ApiV2AccountMenusTreeGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountMenusTreeGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountMenusListGet
 * url: ApiV2AccountMenusListGetURL
 * method: ApiV2AccountMenusListGet_TYPE
 * raw_url: ApiV2AccountMenusListGet_RAW_URL
 * @param lang - 
 */
export const ApiV2AccountMenusListGet = function(parameters = {}) {
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
export const ApiV2AccountMenusListGet_RAW_URL = function() {
  return '/api/v2/account/menus/list'
}
export const ApiV2AccountMenusListGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountMenusListGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountAquariumMenusTreeGet
 * url: ApiV2AccountAquariumMenusTreeGetURL
 * method: ApiV2AccountAquariumMenusTreeGet_TYPE
 * raw_url: ApiV2AccountAquariumMenusTreeGet_RAW_URL
 * @param lang - 
 */
export const ApiV2AccountAquariumMenusTreeGet = function(parameters = {}) {
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
export const ApiV2AccountAquariumMenusTreeGet_RAW_URL = function() {
  return '/api/v2/account/aquarium/menus/tree'
}
export const ApiV2AccountAquariumMenusTreeGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountAquariumMenusTreeGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountAquariumMenusListGet
 * url: ApiV2AccountAquariumMenusListGetURL
 * method: ApiV2AccountAquariumMenusListGet_TYPE
 * raw_url: ApiV2AccountAquariumMenusListGet_RAW_URL
 * @param lang - 
 */
export const ApiV2AccountAquariumMenusListGet = function(parameters = {}) {
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
export const ApiV2AccountAquariumMenusListGet_RAW_URL = function() {
  return '/api/v2/account/aquarium/menus/list'
}
export const ApiV2AccountAquariumMenusListGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountAquariumMenusListGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountGitsGet
 * url: ApiV2AccountGitsGetURL
 * method: ApiV2AccountGitsGet_TYPE
 * raw_url: ApiV2AccountGitsGet_RAW_URL
 */
export const ApiV2AccountGitsGet = function(parameters = {}) {
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
export const ApiV2AccountGitsGet_RAW_URL = function() {
  return '/api/v2/account/gits'
}
export const ApiV2AccountGitsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountGitsGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountGitsPut
 * url: ApiV2AccountGitsPutURL
 * method: ApiV2AccountGitsPut_TYPE
 * raw_url: ApiV2AccountGitsPut_RAW_URL
 * @param model - 更新内容
 */
export const ApiV2AccountGitsPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/gits'
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
export const ApiV2AccountGitsPut_RAW_URL = function() {
  return '/api/v2/account/gits'
}
export const ApiV2AccountGitsPut_TYPE = function() {
  return 'put'
}
export const ApiV2AccountGitsPutURL = function(parameters = {}) {
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
 * request: ApiV2AccountRegistriesGet
 * url: ApiV2AccountRegistriesGetURL
 * method: ApiV2AccountRegistriesGet_TYPE
 * raw_url: ApiV2AccountRegistriesGet_RAW_URL
 */
export const ApiV2AccountRegistriesGet = function(parameters = {}) {
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
export const ApiV2AccountRegistriesGet_RAW_URL = function() {
  return '/api/v2/account/registries'
}
export const ApiV2AccountRegistriesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AccountRegistriesGetURL = function(parameters = {}) {
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
 * request: ApiV2AccountRegistriesPut
 * url: ApiV2AccountRegistriesPutURL
 * method: ApiV2AccountRegistriesPut_TYPE
 * raw_url: ApiV2AccountRegistriesPut_RAW_URL
 * @param model - 更新内容
 */
export const ApiV2AccountRegistriesPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/account/registries'
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
export const ApiV2AccountRegistriesPut_RAW_URL = function() {
  return '/api/v2/account/registries'
}
export const ApiV2AccountRegistriesPut_TYPE = function() {
  return 'put'
}
export const ApiV2AccountRegistriesPutURL = function(parameters = {}) {
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
* 指定された条件でページングされた状態で、アクアリウムデータセット一覧を取得する
* request: ApiV2AquariumDatasetsGet
* url: ApiV2AquariumDatasetsGetURL
* method: ApiV2AquariumDatasetsGet_TYPE
* raw_url: ApiV2AquariumDatasetsGet_RAW_URL
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
export const ApiV2AquariumDatasetsGet = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsGet_RAW_URL = function() {
  return '/api/v2/aquarium/datasets'
}
export const ApiV2AquariumDatasetsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AquariumDatasetsGetURL = function(parameters = {}) {
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
 * アクアリウムデータセットを作成する
 * request: ApiV2AquariumDatasetsPost
 * url: ApiV2AquariumDatasetsPostURL
 * method: ApiV2AquariumDatasetsPost_TYPE
 * raw_url: ApiV2AquariumDatasetsPost_RAW_URL
 * @param model - アクアリウムデータセット
 */
export const ApiV2AquariumDatasetsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/aquarium/datasets'
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
export const ApiV2AquariumDatasetsPost_RAW_URL = function() {
  return '/api/v2/aquarium/datasets'
}
export const ApiV2AquariumDatasetsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AquariumDatasetsPostURL = function(parameters = {}) {
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
 * アクアリウムデータセットバージョン一覧を取得する
 * request: ApiV2AquariumDatasetsByIdVersionsGet
 * url: ApiV2AquariumDatasetsByIdVersionsGetURL
 * method: ApiV2AquariumDatasetsByIdVersionsGet_TYPE
 * raw_url: ApiV2AquariumDatasetsByIdVersionsGet_RAW_URL
 * @param id - アクアリウムデータセットID
 */
export const ApiV2AquariumDatasetsByIdVersionsGet = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsByIdVersionsGet_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions'
}
export const ApiV2AquariumDatasetsByIdVersionsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AquariumDatasetsByIdVersionsGetURL = function(parameters = {}) {
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
 * アクアリウムデータセットバージョンを作成する
 * request: ApiV2AquariumDatasetsByIdVersionsPost
 * url: ApiV2AquariumDatasetsByIdVersionsPostURL
 * method: ApiV2AquariumDatasetsByIdVersionsPost_TYPE
 * raw_url: ApiV2AquariumDatasetsByIdVersionsPost_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param model - アクアリウムデータセットバージョン
 */
export const ApiV2AquariumDatasetsByIdVersionsPost = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsByIdVersionsPost_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions'
}
export const ApiV2AquariumDatasetsByIdVersionsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AquariumDatasetsByIdVersionsPostURL = function(parameters = {}) {
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
 * request: ApiV2AquariumDatasetsByIdVersionsByVersionIdGet
 * url: ApiV2AquariumDatasetsByIdVersionsByVersionIdGetURL
 * method: ApiV2AquariumDatasetsByIdVersionsByVersionIdGet_TYPE
 * raw_url: ApiV2AquariumDatasetsByIdVersionsByVersionIdGet_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param versionId - アクアリウムデータセットバージョンID
 */
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdGet = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdGet_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
}
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete
 * url: ApiV2AquariumDatasetsByIdVersionsByVersionIdDeleteURL
 * method: ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete_TYPE
 * raw_url: ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete_RAW_URL
 * @param id - アクアリウムデータセットID
 * @param versionId - アクアリウムデータセットバージョンID
 */
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}/versions/{versionId}'
}
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AquariumDatasetsByIdVersionsByVersionIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AquariumDatasetsByIdDelete
 * url: ApiV2AquariumDatasetsByIdDeleteURL
 * method: ApiV2AquariumDatasetsByIdDelete_TYPE
 * raw_url: ApiV2AquariumDatasetsByIdDelete_RAW_URL
 * @param id - アクアリウムデータセットID
 */
export const ApiV2AquariumDatasetsByIdDelete = function(parameters = {}) {
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
export const ApiV2AquariumDatasetsByIdDelete_RAW_URL = function() {
  return '/api/v2/aquarium/datasets/{id}'
}
export const ApiV2AquariumDatasetsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AquariumDatasetsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantPartitionsGet
 * url: ApiV2TenantPartitionsGetURL
 * method: ApiV2TenantPartitionsGet_TYPE
 * raw_url: ApiV2TenantPartitionsGet_RAW_URL
 */
export const ApiV2TenantPartitionsGet = function(parameters = {}) {
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
export const ApiV2TenantPartitionsGet_RAW_URL = function() {
  return '/api/v2/tenant/partitions'
}
export const ApiV2TenantPartitionsGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantPartitionsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminPartitionsGet
 * url: ApiV2AdminPartitionsGetURL
 * method: ApiV2AdminPartitionsGet_TYPE
 * raw_url: ApiV2AdminPartitionsGet_RAW_URL
 */
export const ApiV2AdminPartitionsGet = function(parameters = {}) {
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
export const ApiV2AdminPartitionsGet_RAW_URL = function() {
  return '/api/v2/admin/partitions'
}
export const ApiV2AdminPartitionsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminPartitionsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminQuotasGet
 * url: ApiV2AdminQuotasGetURL
 * method: ApiV2AdminQuotasGet_TYPE
 * raw_url: ApiV2AdminQuotasGet_RAW_URL
 */
export const ApiV2AdminQuotasGet = function(parameters = {}) {
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
export const ApiV2AdminQuotasGet_RAW_URL = function() {
  return '/api/v2/admin/quotas'
}
export const ApiV2AdminQuotasGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminQuotasGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminQuotasPost
 * url: ApiV2AdminQuotasPostURL
 * method: ApiV2AdminQuotasPost_TYPE
 * raw_url: ApiV2AdminQuotasPost_RAW_URL
 * @param models - 
 */
export const ApiV2AdminQuotasPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/quotas'
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
export const ApiV2AdminQuotasPost_RAW_URL = function() {
  return '/api/v2/admin/quotas'
}
export const ApiV2AdminQuotasPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminQuotasPostURL = function(parameters = {}) {
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
 * request: ApiV2TenantQuotaGet
 * url: ApiV2TenantQuotaGetURL
 * method: ApiV2TenantQuotaGet_TYPE
 * raw_url: ApiV2TenantQuotaGet_RAW_URL
 */
export const ApiV2TenantQuotaGet = function(parameters = {}) {
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
export const ApiV2TenantQuotaGet_RAW_URL = function() {
  return '/api/v2/tenant/quota'
}
export const ApiV2TenantQuotaGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantQuotaGetURL = function(parameters = {}) {
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
 * REST APIとして定時バッチから実行される想定。
 * request: ApiV2AdminTensorboardsDelete
 * url: ApiV2AdminTensorboardsDeleteURL
 * method: ApiV2AdminTensorboardsDelete_TYPE
 * raw_url: ApiV2AdminTensorboardsDelete_RAW_URL
 */
export const ApiV2AdminTensorboardsDelete = function(parameters = {}) {
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
export const ApiV2AdminTensorboardsDelete_RAW_URL = function() {
  return '/api/v2/admin/tensorboards'
}
export const ApiV2AdminTensorboardsDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminTensorboardsDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminEventsByIdGet
 * url: ApiV2AdminEventsByIdGetURL
 * method: ApiV2AdminEventsByIdGet_TYPE
 * raw_url: ApiV2AdminEventsByIdGet_RAW_URL
 * @param id - テナントID
 * @param name - コンテナ名
 */
export const ApiV2AdminEventsByIdGet = function(parameters = {}) {
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
export const ApiV2AdminEventsByIdGet_RAW_URL = function() {
  return '/api/v2/admin/events/{id}'
}
export const ApiV2AdminEventsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminEventsByIdGetURL = function(parameters = {}) {
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
* request: ApiV2DataGet
* url: ApiV2DataGetURL
* method: ApiV2DataGet_TYPE
* raw_url: ApiV2DataGet_RAW_URL
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
export const ApiV2DataGet = function(parameters = {}) {
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
export const ApiV2DataGet_RAW_URL = function() {
  return '/api/v2/data'
}
export const ApiV2DataGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataGetURL = function(parameters = {}) {
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
 * request: ApiV2DataPost
 * url: ApiV2DataPostURL
 * method: ApiV2DataPost_TYPE
 * raw_url: ApiV2DataPost_RAW_URL
 * @param model - 
 */
export const ApiV2DataPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/data'
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
export const ApiV2DataPost_RAW_URL = function() {
  return '/api/v2/data'
}
export const ApiV2DataPost_TYPE = function() {
  return 'post'
}
export const ApiV2DataPostURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdGet
 * url: ApiV2DataByIdGetURL
 * method: ApiV2DataByIdGet_TYPE
 * raw_url: ApiV2DataByIdGet_RAW_URL
 * @param id - データID
 */
export const ApiV2DataByIdGet = function(parameters = {}) {
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
export const ApiV2DataByIdGet_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const ApiV2DataByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataByIdGetURL = function(parameters = {}) {
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
* request: ApiV2DataByIdPut
* url: ApiV2DataByIdPutURL
* method: ApiV2DataByIdPut_TYPE
* raw_url: ApiV2DataByIdPut_RAW_URL
     * @param id - 変更対象のデータID
     * @param model - 変更内容
*/
export const ApiV2DataByIdPut = function(parameters = {}) {
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
export const ApiV2DataByIdPut_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const ApiV2DataByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2DataByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdDelete
 * url: ApiV2DataByIdDeleteURL
 * method: ApiV2DataByIdDelete_TYPE
 * raw_url: ApiV2DataByIdDelete_RAW_URL
 * @param id - データID
 */
export const ApiV2DataByIdDelete = function(parameters = {}) {
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
export const ApiV2DataByIdDelete_RAW_URL = function() {
  return '/api/v2/data/{id}'
}
export const ApiV2DataByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2DataByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdTagsByTagPut
 * url: ApiV2DataByIdTagsByTagPutURL
 * method: ApiV2DataByIdTagsByTagPut_TYPE
 * raw_url: ApiV2DataByIdTagsByTagPut_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const ApiV2DataByIdTagsByTagPut = function(parameters = {}) {
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
export const ApiV2DataByIdTagsByTagPut_RAW_URL = function() {
  return '/api/v2/data/{id}/tags/{tag}'
}
export const ApiV2DataByIdTagsByTagPut_TYPE = function() {
  return 'put'
}
export const ApiV2DataByIdTagsByTagPutURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdTagsByTagDelete
 * url: ApiV2DataByIdTagsByTagDeleteURL
 * method: ApiV2DataByIdTagsByTagDelete_TYPE
 * raw_url: ApiV2DataByIdTagsByTagDelete_RAW_URL
 * @param id - 
 * @param tag - 
 */
export const ApiV2DataByIdTagsByTagDelete = function(parameters = {}) {
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
export const ApiV2DataByIdTagsByTagDelete_RAW_URL = function() {
  return '/api/v2/data/{id}/tags/{tag}'
}
export const ApiV2DataByIdTagsByTagDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2DataByIdTagsByTagDeleteURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdFilesByNameGet
 * url: ApiV2DataByIdFilesByNameGetURL
 * method: ApiV2DataByIdFilesByNameGet_TYPE
 * raw_url: ApiV2DataByIdFilesByNameGet_RAW_URL
 * @param id - 対象データID
 * @param name - 対象ファイル名
 */
export const ApiV2DataByIdFilesByNameGet = function(parameters = {}) {
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
export const ApiV2DataByIdFilesByNameGet_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{name}'
}
export const ApiV2DataByIdFilesByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataByIdFilesByNameGetURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdFilesByNameSizeGet
 * url: ApiV2DataByIdFilesByNameSizeGetURL
 * method: ApiV2DataByIdFilesByNameSizeGet_TYPE
 * raw_url: ApiV2DataByIdFilesByNameSizeGet_RAW_URL
 * @param id - 対象データID
 * @param name - 対象ファイル名
 */
export const ApiV2DataByIdFilesByNameSizeGet = function(parameters = {}) {
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
export const ApiV2DataByIdFilesByNameSizeGet_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{name}/size'
}
export const ApiV2DataByIdFilesByNameSizeGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataByIdFilesByNameSizeGetURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdFilesGet
 * url: ApiV2DataByIdFilesGetURL
 * method: ApiV2DataByIdFilesGet_TYPE
 * raw_url: ApiV2DataByIdFilesGet_RAW_URL
 * @param id - 対象データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV2DataByIdFilesGet = function(parameters = {}) {
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
export const ApiV2DataByIdFilesGet_RAW_URL = function() {
  return '/api/v2/data/{id}/files'
}
export const ApiV2DataByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataByIdFilesGetURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdFilesPost
 * url: ApiV2DataByIdFilesPostURL
 * method: ApiV2DataByIdFilesPost_TYPE
 * raw_url: ApiV2DataByIdFilesPost_RAW_URL
 * @param id - 変更対象のデータID
 * @param model - 追加するファイル情報
 */
export const ApiV2DataByIdFilesPost = function(parameters = {}) {
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
export const ApiV2DataByIdFilesPost_RAW_URL = function() {
  return '/api/v2/data/{id}/files'
}
export const ApiV2DataByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV2DataByIdFilesPostURL = function(parameters = {}) {
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
 * request: ApiV2DataByIdFilesByFileIdDelete
 * url: ApiV2DataByIdFilesByFileIdDeleteURL
 * method: ApiV2DataByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV2DataByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象のデータID
 * @param fileId - 削除するファイルのID
 */
export const ApiV2DataByIdFilesByFileIdDelete = function(parameters = {}) {
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
export const ApiV2DataByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v2/data/{id}/files/{fileId}'
}
export const ApiV2DataByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2DataByIdFilesByFileIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2DataDatatagsGet
 * url: ApiV2DataDatatagsGetURL
 * method: ApiV2DataDatatagsGet_TYPE
 * raw_url: ApiV2DataDatatagsGet_RAW_URL
 */
export const ApiV2DataDatatagsGet = function(parameters = {}) {
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
export const ApiV2DataDatatagsGet_RAW_URL = function() {
  return '/api/v2/data/datatags'
}
export const ApiV2DataDatatagsGet_TYPE = function() {
  return 'get'
}
export const ApiV2DataDatatagsGetURL = function(parameters = {}) {
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
 * request: ApiV2DataDatatagsDelete
 * url: ApiV2DataDatatagsDeleteURL
 * method: ApiV2DataDatatagsDelete_TYPE
 * raw_url: ApiV2DataDatatagsDelete_RAW_URL
 */
export const ApiV2DataDatatagsDelete = function(parameters = {}) {
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
export const ApiV2DataDatatagsDelete_RAW_URL = function() {
  return '/api/v2/data/datatags'
}
export const ApiV2DataDatatagsDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2DataDatatagsDeleteURL = function(parameters = {}) {
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
* request: ApiV2DatasetsGet
* url: ApiV2DatasetsGetURL
* method: ApiV2DatasetsGet_TYPE
* raw_url: ApiV2DatasetsGet_RAW_URL
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
export const ApiV2DatasetsGet = function(parameters = {}) {
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
export const ApiV2DatasetsGet_RAW_URL = function() {
  return '/api/v2/datasets'
}
export const ApiV2DatasetsGet_TYPE = function() {
  return 'get'
}
export const ApiV2DatasetsGetURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsPost
 * url: ApiV2DatasetsPostURL
 * method: ApiV2DatasetsPost_TYPE
 * raw_url: ApiV2DatasetsPost_RAW_URL
 * @param model - 新規作成内容
 */
export const ApiV2DatasetsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/datasets'
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
export const ApiV2DatasetsPost_RAW_URL = function() {
  return '/api/v2/datasets'
}
export const ApiV2DatasetsPost_TYPE = function() {
  return 'post'
}
export const ApiV2DatasetsPostURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdGet
 * url: ApiV2DatasetsByIdGetURL
 * method: ApiV2DatasetsByIdGet_TYPE
 * raw_url: ApiV2DatasetsByIdGet_RAW_URL
 * @param id - データセットID
 */
export const ApiV2DatasetsByIdGet = function(parameters = {}) {
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
export const ApiV2DatasetsByIdGet_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const ApiV2DatasetsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2DatasetsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdPut
 * url: ApiV2DatasetsByIdPutURL
 * method: ApiV2DatasetsByIdPut_TYPE
 * raw_url: ApiV2DatasetsByIdPut_RAW_URL
 * @param id - データセットID
 * @param model - 変更内容
 */
export const ApiV2DatasetsByIdPut = function(parameters = {}) {
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
export const ApiV2DatasetsByIdPut_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const ApiV2DatasetsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2DatasetsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdDelete
 * url: ApiV2DatasetsByIdDeleteURL
 * method: ApiV2DatasetsByIdDelete_TYPE
 * raw_url: ApiV2DatasetsByIdDelete_RAW_URL
 * @param id - データセットID
 */
export const ApiV2DatasetsByIdDelete = function(parameters = {}) {
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
export const ApiV2DatasetsByIdDelete_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const ApiV2DatasetsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2DatasetsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdPatch
 * url: ApiV2DatasetsByIdPatchURL
 * method: ApiV2DatasetsByIdPatch_TYPE
 * raw_url: ApiV2DatasetsByIdPatch_RAW_URL
 * @param id - データセットID
 * @param model - 変更内容
 */
export const ApiV2DatasetsByIdPatch = function(parameters = {}) {
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
export const ApiV2DatasetsByIdPatch_RAW_URL = function() {
  return '/api/v2/datasets/{id}'
}
export const ApiV2DatasetsByIdPatch_TYPE = function() {
  return 'patch'
}
export const ApiV2DatasetsByIdPatchURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdFilesGet
 * url: ApiV2DatasetsByIdFilesGetURL
 * method: ApiV2DatasetsByIdFilesGet_TYPE
 * raw_url: ApiV2DatasetsByIdFilesGet_RAW_URL
 * @param id - データセットID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV2DatasetsByIdFilesGet = function(parameters = {}) {
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
export const ApiV2DatasetsByIdFilesGet_RAW_URL = function() {
  return '/api/v2/datasets/{id}/files'
}
export const ApiV2DatasetsByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV2DatasetsByIdFilesGetURL = function(parameters = {}) {
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
 * request: ApiV2DatasetsByIdPathpairsGet
 * url: ApiV2DatasetsByIdPathpairsGetURL
 * method: ApiV2DatasetsByIdPathpairsGet_TYPE
 * raw_url: ApiV2DatasetsByIdPathpairsGet_RAW_URL
 * @param id - データセットID
 */
export const ApiV2DatasetsByIdPathpairsGet = function(parameters = {}) {
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
export const ApiV2DatasetsByIdPathpairsGet_RAW_URL = function() {
  return '/api/v2/datasets/{id}/pathpairs'
}
export const ApiV2DatasetsByIdPathpairsGet_TYPE = function() {
  return 'get'
}
export const ApiV2DatasetsByIdPathpairsGetURL = function(parameters = {}) {
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
 * request: ApiV2DatatypesGet
 * url: ApiV2DatatypesGetURL
 * method: ApiV2DatatypesGet_TYPE
 * raw_url: ApiV2DatatypesGet_RAW_URL
 */
export const ApiV2DatatypesGet = function(parameters = {}) {
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
export const ApiV2DatatypesGet_RAW_URL = function() {
  return '/api/v2/datatypes'
}
export const ApiV2DatatypesGet_TYPE = function() {
  return 'get'
}
export const ApiV2DatatypesGetURL = function(parameters = {}) {
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
* request: ApiV2ExperimentGet
* url: ApiV2ExperimentGetURL
* method: ApiV2ExperimentGet_TYPE
* raw_url: ApiV2ExperimentGet_RAW_URL
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
     * @param withTotal - 
*/
export const ApiV2ExperimentGet = function(parameters = {}) {
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
export const ApiV2ExperimentGet_RAW_URL = function() {
  return '/api/v2/experiment'
}
export const ApiV2ExperimentGet_TYPE = function() {
  return 'get'
}
export const ApiV2ExperimentGetURL = function(parameters = {}) {
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
 * request: ApiV2ExperimentByIdGet
 * url: ApiV2ExperimentByIdGetURL
 * method: ApiV2ExperimentByIdGet_TYPE
 * raw_url: ApiV2ExperimentByIdGet_RAW_URL
 * @param id - 実験ID
 */
export const ApiV2ExperimentByIdGet = function(parameters = {}) {
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
export const ApiV2ExperimentByIdGet_RAW_URL = function() {
  return '/api/v2/experiment/{id}'
}
export const ApiV2ExperimentByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2ExperimentByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2ExperimentByIdDelete
 * url: ApiV2ExperimentByIdDeleteURL
 * method: ApiV2ExperimentByIdDelete_TYPE
 * raw_url: ApiV2ExperimentByIdDelete_RAW_URL
 * @param id - 実験ID
 */
export const ApiV2ExperimentByIdDelete = function(parameters = {}) {
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
export const ApiV2ExperimentByIdDelete_RAW_URL = function() {
  return '/api/v2/experiment/{id}'
}
export const ApiV2ExperimentByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2ExperimentByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2ExperimentRunPost
 * url: ApiV2ExperimentRunPostURL
 * method: ApiV2ExperimentRunPost_TYPE
 * raw_url: ApiV2ExperimentRunPost_RAW_URL
 * @param model - 
 */
export const ApiV2ExperimentRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/experiment/run'
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
export const ApiV2ExperimentRunPost_RAW_URL = function() {
  return '/api/v2/experiment/run'
}
export const ApiV2ExperimentRunPost_TYPE = function() {
  return 'post'
}
export const ApiV2ExperimentRunPostURL = function(parameters = {}) {
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
 * request: ApiV2ExperimentByIdPreprocessingCompletePost
 * url: ApiV2ExperimentByIdPreprocessingCompletePostURL
 * method: ApiV2ExperimentByIdPreprocessingCompletePost_TYPE
 * raw_url: ApiV2ExperimentByIdPreprocessingCompletePost_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2ExperimentByIdPreprocessingCompletePost = function(parameters = {}) {
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
export const ApiV2ExperimentByIdPreprocessingCompletePost_RAW_URL = function() {
  return '/api/v2/experiment/{id}/preprocessing/complete'
}
export const ApiV2ExperimentByIdPreprocessingCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV2ExperimentByIdPreprocessingCompletePostURL = function(parameters = {}) {
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
 * アクアリウム推論一覧を取得する
 * request: ApiV2ExperimentByIdEvaluationsGet
 * url: ApiV2ExperimentByIdEvaluationsGetURL
 * method: ApiV2ExperimentByIdEvaluationsGet_TYPE
 * raw_url: ApiV2ExperimentByIdEvaluationsGet_RAW_URL
 * @param id - 実験ID
 */
export const ApiV2ExperimentByIdEvaluationsGet = function(parameters = {}) {
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
export const ApiV2ExperimentByIdEvaluationsGet_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations'
}
export const ApiV2ExperimentByIdEvaluationsGet_TYPE = function() {
  return 'get'
}
export const ApiV2ExperimentByIdEvaluationsGetURL = function(parameters = {}) {
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
 * アクアリウム推論を作成する
 * request: ApiV2ExperimentByIdEvaluationsPost
 * url: ApiV2ExperimentByIdEvaluationsPostURL
 * method: ApiV2ExperimentByIdEvaluationsPost_TYPE
 * raw_url: ApiV2ExperimentByIdEvaluationsPost_RAW_URL
 * @param id - 実験ID
 * @param model - アクアリウム推論
 */
export const ApiV2ExperimentByIdEvaluationsPost = function(parameters = {}) {
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
export const ApiV2ExperimentByIdEvaluationsPost_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations'
}
export const ApiV2ExperimentByIdEvaluationsPost_TYPE = function() {
  return 'post'
}
export const ApiV2ExperimentByIdEvaluationsPostURL = function(parameters = {}) {
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
 * request: ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete
 * url: ApiV2ExperimentByIdEvaluationsByEvaluationIdDeleteURL
 * method: ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete_TYPE
 * raw_url: ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete_RAW_URL
 * @param id - 実験ID
 * @param evaluationId - アクアリウム推論ID
 */
export const ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete = function(parameters = {}) {
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
export const ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete_RAW_URL = function() {
  return '/api/v2/experiment/{id}/evaluations/{evaluationId}'
}
export const ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2ExperimentByIdEvaluationsByEvaluationIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitEndpointsGet
 * url: ApiV2AdminGitEndpointsGetURL
 * method: ApiV2AdminGitEndpointsGet_TYPE
 * raw_url: ApiV2AdminGitEndpointsGet_RAW_URL
 */
export const ApiV2AdminGitEndpointsGet = function(parameters = {}) {
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
export const ApiV2AdminGitEndpointsGet_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints'
}
export const ApiV2AdminGitEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminGitEndpointsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitEndpointsPost
 * url: ApiV2AdminGitEndpointsPostURL
 * method: ApiV2AdminGitEndpointsPost_TYPE
 * raw_url: ApiV2AdminGitEndpointsPost_RAW_URL
 * @param model - 新規作成モデル
 */
export const ApiV2AdminGitEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/git/endpoints'
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
export const ApiV2AdminGitEndpointsPost_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints'
}
export const ApiV2AdminGitEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminGitEndpointsPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitTypesGet
 * url: ApiV2AdminGitTypesGetURL
 * method: ApiV2AdminGitTypesGet_TYPE
 * raw_url: ApiV2AdminGitTypesGet_RAW_URL
 */
export const ApiV2AdminGitTypesGet = function(parameters = {}) {
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
export const ApiV2AdminGitTypesGet_RAW_URL = function() {
  return '/api/v2/admin/git/types'
}
export const ApiV2AdminGitTypesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminGitTypesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitEndpointsByIdGet
 * url: ApiV2AdminGitEndpointsByIdGetURL
 * method: ApiV2AdminGitEndpointsByIdGet_TYPE
 * raw_url: ApiV2AdminGitEndpointsByIdGet_RAW_URL
 * @param id - GitエンドポイントID
 */
export const ApiV2AdminGitEndpointsByIdGet = function(parameters = {}) {
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
export const ApiV2AdminGitEndpointsByIdGet_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const ApiV2AdminGitEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminGitEndpointsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitEndpointsByIdPut
 * url: ApiV2AdminGitEndpointsByIdPutURL
 * method: ApiV2AdminGitEndpointsByIdPut_TYPE
 * raw_url: ApiV2AdminGitEndpointsByIdPut_RAW_URL
 * @param id - 編集対象GitID
 * @param model - 編集モデル
 */
export const ApiV2AdminGitEndpointsByIdPut = function(parameters = {}) {
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
export const ApiV2AdminGitEndpointsByIdPut_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const ApiV2AdminGitEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminGitEndpointsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminGitEndpointsByIdDelete
 * url: ApiV2AdminGitEndpointsByIdDeleteURL
 * method: ApiV2AdminGitEndpointsByIdDelete_TYPE
 * raw_url: ApiV2AdminGitEndpointsByIdDelete_RAW_URL
 * @param id - 削除対象GitID
 */
export const ApiV2AdminGitEndpointsByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminGitEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/git/endpoints/{id}'
}
export const ApiV2AdminGitEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminGitEndpointsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantGitEndpointsGet
 * url: ApiV2TenantGitEndpointsGetURL
 * method: ApiV2TenantGitEndpointsGet_TYPE
 * raw_url: ApiV2TenantGitEndpointsGet_RAW_URL
 */
export const ApiV2TenantGitEndpointsGet = function(parameters = {}) {
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
export const ApiV2TenantGitEndpointsGet_RAW_URL = function() {
  return '/api/v2/tenant/git/endpoints'
}
export const ApiV2TenantGitEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantGitEndpointsGetURL = function(parameters = {}) {
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
 * request: ApiV2GitByGitIdReposGet
 * url: ApiV2GitByGitIdReposGetURL
 * method: ApiV2GitByGitIdReposGet_TYPE
 * raw_url: ApiV2GitByGitIdReposGet_RAW_URL
 * @param gitId - 
 */
export const ApiV2GitByGitIdReposGet = function(parameters = {}) {
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
export const ApiV2GitByGitIdReposGet_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos'
}
export const ApiV2GitByGitIdReposGet_TYPE = function() {
  return 'get'
}
export const ApiV2GitByGitIdReposGetURL = function(parameters = {}) {
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
 * request: ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet
 * url: ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGetURL
 * method: ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet_TYPE
 * raw_url: ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 */
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet = function(parameters = {}) {
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
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/branches'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet_TYPE = function() {
  return 'get'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGetURL = function(parameters = {}) {
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
 * request: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet
 * url: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGetURL
 * method: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet_TYPE
 * raw_url: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param branch - ブランチ名
 */
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet = function(parameters = {}) {
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
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet_TYPE = function() {
  return 'get'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits'
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
 * request: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet
 * url: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGetURL
 * method: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_TYPE
 * raw_url: ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_RAW_URL
 * @param gitId - GitId
 * @param owner - オーナー名
 * @param repositoryName - リポジトリ名
 * @param commitId - コミットID
 */
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet = function(parameters = {}) {
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
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{owner}/{repositoryName}/commits/{commitId}'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGetURL = function(parameters = {}) {
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
* request: ApiV2GitByGitIdReposBySegmentsGet
* url: ApiV2GitByGitIdReposBySegmentsGetURL
* method: ApiV2GitByGitIdReposBySegmentsGet_TYPE
* raw_url: ApiV2GitByGitIdReposBySegmentsGet_RAW_URL
     * @param gitId - 
     * @param segments - 
     * @param branch - 
*/
export const ApiV2GitByGitIdReposBySegmentsGet = function(parameters = {}) {
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
  if (parameters.$queryParameters) {
    Object.keys(parameters.$queryParameters).forEach(function(parameterName) {
      queryParameters[parameterName] = parameters.$queryParameters[parameterName]
    });
  }
  return request('get', domain + path, body, queryParameters, form, config)
}
export const ApiV2GitByGitIdReposBySegmentsGet_RAW_URL = function() {
  return '/api/v2/git/{gitId}/repos/{segments}'
}
export const ApiV2GitByGitIdReposBySegmentsGet_TYPE = function() {
  return 'get'
}
export const ApiV2GitByGitIdReposBySegmentsGetURL = function(parameters = {}) {
  let queryParameters = {}
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  let path = '/api/v2/git/{gitId}/repos/{segments}'
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
 * request: ApiV2InferencesSimpleGet
 * url: ApiV2InferencesSimpleGetURL
 * method: ApiV2InferencesSimpleGet_TYPE
 * raw_url: ApiV2InferencesSimpleGet_RAW_URL
 */
export const ApiV2InferencesSimpleGet = function(parameters = {}) {
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
export const ApiV2InferencesSimpleGet_RAW_URL = function() {
  return '/api/v2/inferences/simple'
}
export const ApiV2InferencesSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesSimpleGetURL = function(parameters = {}) {
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
* request: ApiV2InferencesGet
* url: ApiV2InferencesGetURL
* method: ApiV2InferencesGet_TYPE
* raw_url: ApiV2InferencesGet_RAW_URL
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
export const ApiV2InferencesGet = function(parameters = {}) {
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
export const ApiV2InferencesGet_RAW_URL = function() {
  return '/api/v2/inferences'
}
export const ApiV2InferencesGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesMountGet
 * url: ApiV2InferencesMountGetURL
 * method: ApiV2InferencesMountGet_TYPE
 * raw_url: ApiV2InferencesMountGet_RAW_URL
 * @param status - ステータス
 */
export const ApiV2InferencesMountGet = function(parameters = {}) {
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
export const ApiV2InferencesMountGet_RAW_URL = function() {
  return '/api/v2/inferences/mount'
}
export const ApiV2InferencesMountGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesMountGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdGet
 * url: ApiV2InferencesByIdGetURL
 * method: ApiV2InferencesByIdGet_TYPE
 * raw_url: ApiV2InferencesByIdGet_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV2InferencesByIdGet = function(parameters = {}) {
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
export const ApiV2InferencesByIdGet_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const ApiV2InferencesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdPut
 * url: ApiV2InferencesByIdPutURL
 * method: ApiV2InferencesByIdPut_TYPE
 * raw_url: ApiV2InferencesByIdPut_RAW_URL
 * @param id - 変更対象の推論履歴ID
 * @param model - 変更内容
 */
export const ApiV2InferencesByIdPut = function(parameters = {}) {
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
export const ApiV2InferencesByIdPut_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const ApiV2InferencesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2InferencesByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdDelete
 * url: ApiV2InferencesByIdDeleteURL
 * method: ApiV2InferencesByIdDelete_TYPE
 * raw_url: ApiV2InferencesByIdDelete_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV2InferencesByIdDelete = function(parameters = {}) {
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
export const ApiV2InferencesByIdDelete_RAW_URL = function() {
  return '/api/v2/inferences/{id}'
}
export const ApiV2InferencesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2InferencesByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdEventsGet
 * url: ApiV2InferencesByIdEventsGetURL
 * method: ApiV2InferencesByIdEventsGet_TYPE
 * raw_url: ApiV2InferencesByIdEventsGet_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV2InferencesByIdEventsGet = function(parameters = {}) {
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
export const ApiV2InferencesByIdEventsGet_RAW_URL = function() {
  return '/api/v2/inferences/{id}/events'
}
export const ApiV2InferencesByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesByIdEventsGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesRunPost
 * url: ApiV2InferencesRunPostURL
 * method: ApiV2InferencesRunPost_TYPE
 * raw_url: ApiV2InferencesRunPost_RAW_URL
 * @param model - 新規推論実行内容
 */
export const ApiV2InferencesRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/inferences/run'
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
export const ApiV2InferencesRunPost_RAW_URL = function() {
  return '/api/v2/inferences/run'
}
export const ApiV2InferencesRunPost_TYPE = function() {
  return 'post'
}
export const ApiV2InferencesRunPostURL = function(parameters = {}) {
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
 * 推論履歴添付ファイルの一覧を取得
 * request: ApiV2InferencesByIdFilesGet
 * url: ApiV2InferencesByIdFilesGetURL
 * method: ApiV2InferencesByIdFilesGet_TYPE
 * raw_url: ApiV2InferencesByIdFilesGet_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV2InferencesByIdFilesGet = function(parameters = {}) {
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
export const ApiV2InferencesByIdFilesGet_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files'
}
export const ApiV2InferencesByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesByIdFilesGetURL = function(parameters = {}) {
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
 * 推論履歴添付ファイルを登録
 * request: ApiV2InferencesByIdFilesPost
 * url: ApiV2InferencesByIdFilesPostURL
 * method: ApiV2InferencesByIdFilesPost_TYPE
 * raw_url: ApiV2InferencesByIdFilesPost_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param model - 追加するファイル情報
 */
export const ApiV2InferencesByIdFilesPost = function(parameters = {}) {
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
export const ApiV2InferencesByIdFilesPost_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files'
}
export const ApiV2InferencesByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV2InferencesByIdFilesPostURL = function(parameters = {}) {
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
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: ApiV2InferencesByIdContainer_filesGet
* url: ApiV2InferencesByIdContainer_filesGetURL
* method: ApiV2InferencesByIdContainer_filesGet_TYPE
* raw_url: ApiV2InferencesByIdContainer_filesGet_RAW_URL
     * @param id - 対象の推論履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV2InferencesByIdContainer_filesGet = function(parameters = {}) {
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
export const ApiV2InferencesByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v2/inferences/{id}/container-files'
}
export const ApiV2InferencesByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesByIdContainer_filesGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdFilesByFileIdDelete
 * url: ApiV2InferencesByIdFilesByFileIdDeleteURL
 * method: ApiV2InferencesByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV2InferencesByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param fileId - 削除するファイルのID
 */
export const ApiV2InferencesByIdFilesByFileIdDelete = function(parameters = {}) {
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
export const ApiV2InferencesByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files/{fileId}'
}
export const ApiV2InferencesByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2InferencesByIdFilesByFileIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdFilesByNameSizeGet
 * url: ApiV2InferencesByIdFilesByNameSizeGetURL
 * method: ApiV2InferencesByIdFilesByNameSizeGet_TYPE
 * raw_url: ApiV2InferencesByIdFilesByNameSizeGet_RAW_URL
 * @param id - 対象の推論履歴ID
 * @param name - 対象ファイル名
 */
export const ApiV2InferencesByIdFilesByNameSizeGet = function(parameters = {}) {
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
export const ApiV2InferencesByIdFilesByNameSizeGet_RAW_URL = function() {
  return '/api/v2/inferences/{id}/files/{name}/size'
}
export const ApiV2InferencesByIdFilesByNameSizeGet_TYPE = function() {
  return 'get'
}
export const ApiV2InferencesByIdFilesByNameSizeGetURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdHaltPost
 * url: ApiV2InferencesByIdHaltPostURL
 * method: ApiV2InferencesByIdHaltPost_TYPE
 * raw_url: ApiV2InferencesByIdHaltPost_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV2InferencesByIdHaltPost = function(parameters = {}) {
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
export const ApiV2InferencesByIdHaltPost_RAW_URL = function() {
  return '/api/v2/inferences/{id}/halt'
}
export const ApiV2InferencesByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV2InferencesByIdHaltPostURL = function(parameters = {}) {
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
* request: ApiV2InferencesByIdUser_cancelPost
* url: ApiV2InferencesByIdUser_cancelPostURL
* method: ApiV2InferencesByIdUser_cancelPost_TYPE
* raw_url: ApiV2InferencesByIdUser_cancelPost_RAW_URL
     * @param id - 推論履歴ID
*/
export const ApiV2InferencesByIdUser_cancelPost = function(parameters = {}) {
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
export const ApiV2InferencesByIdUser_cancelPost_RAW_URL = function() {
  return '/api/v2/inferences/{id}/user-cancel'
}
export const ApiV2InferencesByIdUser_cancelPost_TYPE = function() {
  return 'post'
}
export const ApiV2InferencesByIdUser_cancelPostURL = function(parameters = {}) {
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
 * request: ApiV2InferencesByIdCompletePost
 * url: ApiV2InferencesByIdCompletePostURL
 * method: ApiV2InferencesByIdCompletePost_TYPE
 * raw_url: ApiV2InferencesByIdCompletePost_RAW_URL
 * @param id - 推論履歴ID
 */
export const ApiV2InferencesByIdCompletePost = function(parameters = {}) {
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
export const ApiV2InferencesByIdCompletePost_RAW_URL = function() {
  return '/api/v2/inferences/{id}/complete'
}
export const ApiV2InferencesByIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV2InferencesByIdCompletePostURL = function(parameters = {}) {
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
 * request: ApiV2TenantMenusGet
 * url: ApiV2TenantMenusGetURL
 * method: ApiV2TenantMenusGet_TYPE
 * raw_url: ApiV2TenantMenusGet_RAW_URL
 */
export const ApiV2TenantMenusGet = function(parameters = {}) {
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
export const ApiV2TenantMenusGet_RAW_URL = function() {
  return '/api/v2/tenant/menus'
}
export const ApiV2TenantMenusGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantMenusGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantMenusByIdPut
 * url: ApiV2TenantMenusByIdPutURL
 * method: ApiV2TenantMenusByIdPut_TYPE
 * raw_url: ApiV2TenantMenusByIdPut_RAW_URL
 * @param id - 
 * @param roleIds - 
 */
export const ApiV2TenantMenusByIdPut = function(parameters = {}) {
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
export const ApiV2TenantMenusByIdPut_RAW_URL = function() {
  return '/api/v2/tenant/menus/{id}'
}
export const ApiV2TenantMenusByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2TenantMenusByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2TenantMenu_typesGet
 * url: ApiV2TenantMenu_typesGetURL
 * method: ApiV2TenantMenu_typesGet_TYPE
 * raw_url: ApiV2TenantMenu_typesGet_RAW_URL
 */
export const ApiV2TenantMenu_typesGet = function(parameters = {}) {
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
export const ApiV2TenantMenu_typesGet_RAW_URL = function() {
  return '/api/v2/tenant/menu-types'
}
export const ApiV2TenantMenu_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantMenu_typesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminMenusGet
 * url: ApiV2AdminMenusGetURL
 * method: ApiV2AdminMenusGet_TYPE
 * raw_url: ApiV2AdminMenusGet_RAW_URL
 */
export const ApiV2AdminMenusGet = function(parameters = {}) {
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
export const ApiV2AdminMenusGet_RAW_URL = function() {
  return '/api/v2/admin/menus'
}
export const ApiV2AdminMenusGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminMenusGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminMenusByIdPut
 * url: ApiV2AdminMenusByIdPutURL
 * method: ApiV2AdminMenusByIdPut_TYPE
 * raw_url: ApiV2AdminMenusByIdPut_RAW_URL
 * @param id - 
 * @param roleIds - 
 */
export const ApiV2AdminMenusByIdPut = function(parameters = {}) {
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
export const ApiV2AdminMenusByIdPut_RAW_URL = function() {
  return '/api/v2/admin/menus/{id}'
}
export const ApiV2AdminMenusByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminMenusByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminMenu_typesGet
 * url: ApiV2AdminMenu_typesGetURL
 * method: ApiV2AdminMenu_typesGet_TYPE
 * raw_url: ApiV2AdminMenu_typesGet_RAW_URL
 */
export const ApiV2AdminMenu_typesGet = function(parameters = {}) {
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
export const ApiV2AdminMenu_typesGet_RAW_URL = function() {
  return '/api/v2/admin/menu-types'
}
export const ApiV2AdminMenu_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminMenu_typesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesGet
 * url: ApiV2AdminNodesGetURL
 * method: ApiV2AdminNodesGet_TYPE
 * raw_url: ApiV2AdminNodesGet_RAW_URL
 * @param name - 
 * @param perPage - 
 * @param page - 
 * @param withTotal - 
 */
export const ApiV2AdminNodesGet = function(parameters = {}) {
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
export const ApiV2AdminNodesGet_RAW_URL = function() {
  return '/api/v2/admin/nodes'
}
export const ApiV2AdminNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminNodesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesPost
 * url: ApiV2AdminNodesPostURL
 * method: ApiV2AdminNodesPost_TYPE
 * raw_url: ApiV2AdminNodesPost_RAW_URL
 * @param model - 
 */
export const ApiV2AdminNodesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/nodes'
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
export const ApiV2AdminNodesPost_RAW_URL = function() {
  return '/api/v2/admin/nodes'
}
export const ApiV2AdminNodesPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminNodesPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminNode_access_levelsGet
 * url: ApiV2AdminNode_access_levelsGetURL
 * method: ApiV2AdminNode_access_levelsGet_TYPE
 * raw_url: ApiV2AdminNode_access_levelsGet_RAW_URL
 */
export const ApiV2AdminNode_access_levelsGet = function(parameters = {}) {
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
export const ApiV2AdminNode_access_levelsGet_RAW_URL = function() {
  return '/api/v2/admin/node-access-levels'
}
export const ApiV2AdminNode_access_levelsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminNode_access_levelsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesByIdGet
 * url: ApiV2AdminNodesByIdGetURL
 * method: ApiV2AdminNodesByIdGet_TYPE
 * raw_url: ApiV2AdminNodesByIdGet_RAW_URL
 * @param id - ノードID
 */
export const ApiV2AdminNodesByIdGet = function(parameters = {}) {
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
export const ApiV2AdminNodesByIdGet_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const ApiV2AdminNodesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminNodesByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesByIdPut
 * url: ApiV2AdminNodesByIdPutURL
 * method: ApiV2AdminNodesByIdPut_TYPE
 * raw_url: ApiV2AdminNodesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV2AdminNodesByIdPut = function(parameters = {}) {
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
export const ApiV2AdminNodesByIdPut_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const ApiV2AdminNodesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminNodesByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesByIdDelete
 * url: ApiV2AdminNodesByIdDeleteURL
 * method: ApiV2AdminNodesByIdDelete_TYPE
 * raw_url: ApiV2AdminNodesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV2AdminNodesByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminNodesByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/nodes/{id}'
}
export const ApiV2AdminNodesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminNodesByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminNodesSync_cluster_from_dbPost
 * url: ApiV2AdminNodesSync_cluster_from_dbPostURL
 * method: ApiV2AdminNodesSync_cluster_from_dbPost_TYPE
 * raw_url: ApiV2AdminNodesSync_cluster_from_dbPost_RAW_URL
 */
export const ApiV2AdminNodesSync_cluster_from_dbPost = function(parameters = {}) {
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
export const ApiV2AdminNodesSync_cluster_from_dbPost_RAW_URL = function() {
  return '/api/v2/admin/nodes/sync-cluster-from-db'
}
export const ApiV2AdminNodesSync_cluster_from_dbPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminNodesSync_cluster_from_dbPostURL = function(parameters = {}) {
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
 * request: ApiV2NotebookSimpleGet
 * url: ApiV2NotebookSimpleGetURL
 * method: ApiV2NotebookSimpleGet_TYPE
 * raw_url: ApiV2NotebookSimpleGet_RAW_URL
 */
export const ApiV2NotebookSimpleGet = function(parameters = {}) {
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
export const ApiV2NotebookSimpleGet_RAW_URL = function() {
  return '/api/v2/notebook/simple'
}
export const ApiV2NotebookSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookSimpleGetURL = function(parameters = {}) {
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
* request: ApiV2NotebookGet
* url: ApiV2NotebookGetURL
* method: ApiV2NotebookGet_TYPE
* raw_url: ApiV2NotebookGet_RAW_URL
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
export const ApiV2NotebookGet = function(parameters = {}) {
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
export const ApiV2NotebookGet_RAW_URL = function() {
  return '/api/v2/notebook'
}
export const ApiV2NotebookGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookGetURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdGet
 * url: ApiV2NotebookByIdGetURL
 * method: ApiV2NotebookByIdGet_TYPE
 * raw_url: ApiV2NotebookByIdGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV2NotebookByIdGet = function(parameters = {}) {
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
export const ApiV2NotebookByIdGet_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const ApiV2NotebookByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdPut
 * url: ApiV2NotebookByIdPutURL
 * method: ApiV2NotebookByIdPut_TYPE
 * raw_url: ApiV2NotebookByIdPut_RAW_URL
 * @param id - 変更対象のノートブック履歴ID
 * @param model - 変更内容
 */
export const ApiV2NotebookByIdPut = function(parameters = {}) {
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
export const ApiV2NotebookByIdPut_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const ApiV2NotebookByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2NotebookByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdDelete
 * url: ApiV2NotebookByIdDeleteURL
 * method: ApiV2NotebookByIdDelete_TYPE
 * raw_url: ApiV2NotebookByIdDelete_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV2NotebookByIdDelete = function(parameters = {}) {
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
export const ApiV2NotebookByIdDelete_RAW_URL = function() {
  return '/api/v2/notebook/{id}'
}
export const ApiV2NotebookByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2NotebookByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdEventsGet
 * url: ApiV2NotebookByIdEventsGetURL
 * method: ApiV2NotebookByIdEventsGet_TYPE
 * raw_url: ApiV2NotebookByIdEventsGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV2NotebookByIdEventsGet = function(parameters = {}) {
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
export const ApiV2NotebookByIdEventsGet_RAW_URL = function() {
  return '/api/v2/notebook/{id}/events'
}
export const ApiV2NotebookByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookByIdEventsGetURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdEndpointGet
 * url: ApiV2NotebookByIdEndpointGetURL
 * method: ApiV2NotebookByIdEndpointGet_TYPE
 * raw_url: ApiV2NotebookByIdEndpointGet_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV2NotebookByIdEndpointGet = function(parameters = {}) {
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
export const ApiV2NotebookByIdEndpointGet_RAW_URL = function() {
  return '/api/v2/notebook/{id}/endpoint'
}
export const ApiV2NotebookByIdEndpointGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookByIdEndpointGetURL = function(parameters = {}) {
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
 * request: ApiV2NotebookRunPost
 * url: ApiV2NotebookRunPostURL
 * method: ApiV2NotebookRunPost_TYPE
 * raw_url: ApiV2NotebookRunPost_RAW_URL
 * @param model - 新規実行内容
 */
export const ApiV2NotebookRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/notebook/run'
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
export const ApiV2NotebookRunPost_RAW_URL = function() {
  return '/api/v2/notebook/run'
}
export const ApiV2NotebookRunPost_TYPE = function() {
  return 'post'
}
export const ApiV2NotebookRunPostURL = function(parameters = {}) {
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
* request: ApiV2NotebookByIdContainer_filesGet
* url: ApiV2NotebookByIdContainer_filesGetURL
* method: ApiV2NotebookByIdContainer_filesGet_TYPE
* raw_url: ApiV2NotebookByIdContainer_filesGet_RAW_URL
     * @param id - 対象のノートブック履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV2NotebookByIdContainer_filesGet = function(parameters = {}) {
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
export const ApiV2NotebookByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v2/notebook/{id}/container-files'
}
export const ApiV2NotebookByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookByIdContainer_filesGetURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdHaltPost
 * url: ApiV2NotebookByIdHaltPostURL
 * method: ApiV2NotebookByIdHaltPost_TYPE
 * raw_url: ApiV2NotebookByIdHaltPost_RAW_URL
 * @param id - ノートブック履歴ID
 */
export const ApiV2NotebookByIdHaltPost = function(parameters = {}) {
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
export const ApiV2NotebookByIdHaltPost_RAW_URL = function() {
  return '/api/v2/notebook/{id}/halt'
}
export const ApiV2NotebookByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV2NotebookByIdHaltPostURL = function(parameters = {}) {
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
 * request: ApiV2NotebookByIdRerunPost
 * url: ApiV2NotebookByIdRerunPostURL
 * method: ApiV2NotebookByIdRerunPost_TYPE
 * raw_url: ApiV2NotebookByIdRerunPost_RAW_URL
 * @param id - ノートブック履歴ID
 * @param model - 再起動内容
 */
export const ApiV2NotebookByIdRerunPost = function(parameters = {}) {
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
export const ApiV2NotebookByIdRerunPost_RAW_URL = function() {
  return '/api/v2/notebook/{id}/rerun'
}
export const ApiV2NotebookByIdRerunPost_TYPE = function() {
  return 'post'
}
export const ApiV2NotebookByIdRerunPostURL = function(parameters = {}) {
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
 * request: ApiV2NotebookAvailable_infinite_timeGet
 * url: ApiV2NotebookAvailable_infinite_timeGetURL
 * method: ApiV2NotebookAvailable_infinite_timeGet_TYPE
 * raw_url: ApiV2NotebookAvailable_infinite_timeGet_RAW_URL
 */
export const ApiV2NotebookAvailable_infinite_timeGet = function(parameters = {}) {
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
export const ApiV2NotebookAvailable_infinite_timeGet_RAW_URL = function() {
  return '/api/v2/notebook/available-infinite-time'
}
export const ApiV2NotebookAvailable_infinite_timeGet_TYPE = function() {
  return 'get'
}
export const ApiV2NotebookAvailable_infinite_timeGetURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsGet
* url: ApiV2PreprocessingsGetURL
* method: ApiV2PreprocessingsGet_TYPE
* raw_url: ApiV2PreprocessingsGet_RAW_URL
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
export const ApiV2PreprocessingsGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsGet_RAW_URL = function() {
  return '/api/v2/preprocessings'
}
export const ApiV2PreprocessingsGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsGetURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsPost
 * url: ApiV2PreprocessingsPostURL
 * method: ApiV2PreprocessingsPost_TYPE
 * raw_url: ApiV2PreprocessingsPost_RAW_URL
 * @param model - 新規作成内容
 */
export const ApiV2PreprocessingsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/preprocessings'
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
export const ApiV2PreprocessingsPost_RAW_URL = function() {
  return '/api/v2/preprocessings'
}
export const ApiV2PreprocessingsPost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsPostURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdGet
 * url: ApiV2PreprocessingsByIdGetURL
 * method: ApiV2PreprocessingsByIdGet_TYPE
 * raw_url: ApiV2PreprocessingsByIdGet_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV2PreprocessingsByIdGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdGet_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const ApiV2PreprocessingsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdPut
 * url: ApiV2PreprocessingsByIdPutURL
 * method: ApiV2PreprocessingsByIdPut_TYPE
 * raw_url: ApiV2PreprocessingsByIdPut_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param model - 変更内容
 */
export const ApiV2PreprocessingsByIdPut = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdPut_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const ApiV2PreprocessingsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2PreprocessingsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdDelete
 * url: ApiV2PreprocessingsByIdDeleteURL
 * method: ApiV2PreprocessingsByIdDelete_TYPE
 * raw_url: ApiV2PreprocessingsByIdDelete_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV2PreprocessingsByIdDelete = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdDelete_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const ApiV2PreprocessingsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2PreprocessingsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdPatch
 * url: ApiV2PreprocessingsByIdPatchURL
 * method: ApiV2PreprocessingsByIdPatch_TYPE
 * raw_url: ApiV2PreprocessingsByIdPatch_RAW_URL
 * @param id - 変更対象の前処理ID
 * @param model - 変更内容
 */
export const ApiV2PreprocessingsByIdPatch = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdPatch_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}'
}
export const ApiV2PreprocessingsByIdPatch_TYPE = function() {
  return 'patch'
}
export const ApiV2PreprocessingsByIdPatchURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdHistoriesGet
 * url: ApiV2PreprocessingsByIdHistoriesGetURL
 * method: ApiV2PreprocessingsByIdHistoriesGet_TYPE
 * raw_url: ApiV2PreprocessingsByIdHistoriesGet_RAW_URL
 * @param id - 前処理ID
 */
export const ApiV2PreprocessingsByIdHistoriesGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesGet_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories'
}
export const ApiV2PreprocessingsByIdHistoriesGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsByIdHistoriesGetURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdHistoriesByDataIdGet
 * url: ApiV2PreprocessingsByIdHistoriesByDataIdGetURL
 * method: ApiV2PreprocessingsByIdHistoriesByDataIdGet_TYPE
 * raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdGet_RAW_URL
 * @param id - 前処理ID
 * @param dataId - データID
 */
export const ApiV2PreprocessingsByIdHistoriesByDataIdGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdGet_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdGetURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsByIdHistoriesByDataIdPost
* url: ApiV2PreprocessingsByIdHistoriesByDataIdPostURL
* method: ApiV2PreprocessingsByIdHistoriesByDataIdPost_TYPE
* raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV2PreprocessingsByIdHistoriesByDataIdPost = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdPost_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdPost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdPostURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdHistoriesByDataIdDelete
 * url: ApiV2PreprocessingsByIdHistoriesByDataIdDeleteURL
 * method: ApiV2PreprocessingsByIdHistoriesByDataIdDelete_TYPE
 * raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdDelete_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const ApiV2PreprocessingsByIdHistoriesByDataIdDelete = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdDelete_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet
 * url: ApiV2PreprocessingsByIdHistoriesByDataIdEventsGetURL
 * method: ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet_TYPE
 * raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet_RAW_URL
 * @param id - 前処理ID
 * @param dataId - 入力データID
 */
export const ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/events'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdEventsGetURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsByIdRunPost
* url: ApiV2PreprocessingsByIdRunPostURL
* method: ApiV2PreprocessingsByIdRunPost_TYPE
* raw_url: ApiV2PreprocessingsByIdRunPost_RAW_URL
     * @param id - 前処理ID
     * @param model - 実行設定
*/
export const ApiV2PreprocessingsByIdRunPost = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdRunPost_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/run'
}
export const ApiV2PreprocessingsByIdRunPost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsByIdRunPostURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsByIdHistoriesByDataIdDataPost
* url: ApiV2PreprocessingsByIdHistoriesByDataIdDataPostURL
* method: ApiV2PreprocessingsByIdHistoriesByDataIdDataPost_TYPE
* raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdDataPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
     * @param model - データ情報
*/
export const ApiV2PreprocessingsByIdHistoriesByDataIdDataPost = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdDataPost_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/data'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdDataPost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdDataPostURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost
* url: ApiV2PreprocessingsByIdHistoriesByDataIdHaltPostURL
* method: ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost_TYPE
* raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/halt'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdHaltPostURL = function(parameters = {}) {
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
* request: ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost
* url: ApiV2PreprocessingsByIdHistoriesByDataIdCompletePostURL
* method: ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost_TYPE
* raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost_RAW_URL
     * @param id - 前処理ID
     * @param dataId - 入力データID
*/
export const ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/complete'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdCompletePostURL = function(parameters = {}) {
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
 * request: ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet
 * url: ApiV2PreprocessingsByIdHistoriesByDataIdFilesGetURL
 * method: ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet_TYPE
 * raw_url: ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet_RAW_URL
 * @param id - 対象の前処理履歴ID
 * @param dataId - 入力データID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet = function(parameters = {}) {
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
export const ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet_RAW_URL = function() {
  return '/api/v2/preprocessings/{id}/histories/{dataId}/files'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV2PreprocessingsByIdHistoriesByDataIdFilesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryEndpointsGet
 * url: ApiV2AdminRegistryEndpointsGetURL
 * method: ApiV2AdminRegistryEndpointsGet_TYPE
 * raw_url: ApiV2AdminRegistryEndpointsGet_RAW_URL
 */
export const ApiV2AdminRegistryEndpointsGet = function(parameters = {}) {
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
export const ApiV2AdminRegistryEndpointsGet_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints'
}
export const ApiV2AdminRegistryEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminRegistryEndpointsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryEndpointsPost
 * url: ApiV2AdminRegistryEndpointsPostURL
 * method: ApiV2AdminRegistryEndpointsPost_TYPE
 * raw_url: ApiV2AdminRegistryEndpointsPost_RAW_URL
 * @param model - 新規作成モデル
 */
export const ApiV2AdminRegistryEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/registry/endpoints'
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
export const ApiV2AdminRegistryEndpointsPost_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints'
}
export const ApiV2AdminRegistryEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminRegistryEndpointsPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryTypesGet
 * url: ApiV2AdminRegistryTypesGetURL
 * method: ApiV2AdminRegistryTypesGet_TYPE
 * raw_url: ApiV2AdminRegistryTypesGet_RAW_URL
 */
export const ApiV2AdminRegistryTypesGet = function(parameters = {}) {
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
export const ApiV2AdminRegistryTypesGet_RAW_URL = function() {
  return '/api/v2/admin/registry/types'
}
export const ApiV2AdminRegistryTypesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminRegistryTypesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryEndpointsByIdGet
 * url: ApiV2AdminRegistryEndpointsByIdGetURL
 * method: ApiV2AdminRegistryEndpointsByIdGet_TYPE
 * raw_url: ApiV2AdminRegistryEndpointsByIdGet_RAW_URL
 * @param id - レジストリID
 */
export const ApiV2AdminRegistryEndpointsByIdGet = function(parameters = {}) {
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
export const ApiV2AdminRegistryEndpointsByIdGet_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const ApiV2AdminRegistryEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminRegistryEndpointsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryEndpointsByIdPut
 * url: ApiV2AdminRegistryEndpointsByIdPutURL
 * method: ApiV2AdminRegistryEndpointsByIdPut_TYPE
 * raw_url: ApiV2AdminRegistryEndpointsByIdPut_RAW_URL
 * @param id - 編集対象レジストリID
 * @param model - 編集モデル
 */
export const ApiV2AdminRegistryEndpointsByIdPut = function(parameters = {}) {
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
export const ApiV2AdminRegistryEndpointsByIdPut_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const ApiV2AdminRegistryEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminRegistryEndpointsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminRegistryEndpointsByIdDelete
 * url: ApiV2AdminRegistryEndpointsByIdDeleteURL
 * method: ApiV2AdminRegistryEndpointsByIdDelete_TYPE
 * raw_url: ApiV2AdminRegistryEndpointsByIdDelete_RAW_URL
 * @param id - 削除対象レジストリID
 */
export const ApiV2AdminRegistryEndpointsByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminRegistryEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/registry/endpoints/{id}'
}
export const ApiV2AdminRegistryEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminRegistryEndpointsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantRegistryEndpointsGet
 * url: ApiV2TenantRegistryEndpointsGetURL
 * method: ApiV2TenantRegistryEndpointsGet_TYPE
 * raw_url: ApiV2TenantRegistryEndpointsGet_RAW_URL
 */
export const ApiV2TenantRegistryEndpointsGet = function(parameters = {}) {
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
export const ApiV2TenantRegistryEndpointsGet_RAW_URL = function() {
  return '/api/v2/tenant/registry/endpoints'
}
export const ApiV2TenantRegistryEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantRegistryEndpointsGetURL = function(parameters = {}) {
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
 * request: ApiV2RegistriesByRegistryIdImagesGet
 * url: ApiV2RegistriesByRegistryIdImagesGetURL
 * method: ApiV2RegistriesByRegistryIdImagesGet_TYPE
 * raw_url: ApiV2RegistriesByRegistryIdImagesGet_RAW_URL
 * @param registryId - レジストリID
 */
export const ApiV2RegistriesByRegistryIdImagesGet = function(parameters = {}) {
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
export const ApiV2RegistriesByRegistryIdImagesGet_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images'
}
export const ApiV2RegistriesByRegistryIdImagesGet_TYPE = function() {
  return 'get'
}
export const ApiV2RegistriesByRegistryIdImagesGetURL = function(parameters = {}) {
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
 * request: ApiV2RegistriesByRegistryIdImagesByImageTagsGet
 * url: ApiV2RegistriesByRegistryIdImagesByImageTagsGetURL
 * method: ApiV2RegistriesByRegistryIdImagesByImageTagsGet_TYPE
 * raw_url: ApiV2RegistriesByRegistryIdImagesByImageTagsGet_RAW_URL
 * @param registryId - レジストリID
 * @param image - イメージ名
 */
export const ApiV2RegistriesByRegistryIdImagesByImageTagsGet = function(parameters = {}) {
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
export const ApiV2RegistriesByRegistryIdImagesByImageTagsGet_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images/{image}/tags'
}
export const ApiV2RegistriesByRegistryIdImagesByImageTagsGet_TYPE = function() {
  return 'get'
}
export const ApiV2RegistriesByRegistryIdImagesByImageTagsGetURL = function(parameters = {}) {
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
* request: ApiV2RegistriesByRegistryIdImagesBySegmentsGet
* url: ApiV2RegistriesByRegistryIdImagesBySegmentsGetURL
* method: ApiV2RegistriesByRegistryIdImagesBySegmentsGet_TYPE
* raw_url: ApiV2RegistriesByRegistryIdImagesBySegmentsGet_RAW_URL
     * @param registryId - 
     * @param segments - 
*/
export const ApiV2RegistriesByRegistryIdImagesBySegmentsGet = function(parameters = {}) {
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
export const ApiV2RegistriesByRegistryIdImagesBySegmentsGet_RAW_URL = function() {
  return '/api/v2/registries/{registryId}/images/{segments}'
}
export const ApiV2RegistriesByRegistryIdImagesBySegmentsGet_TYPE = function() {
  return 'get'
}
export const ApiV2RegistriesByRegistryIdImagesBySegmentsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceNodesGet
 * url: ApiV2AdminResourceNodesGetURL
 * method: ApiV2AdminResourceNodesGet_TYPE
 * raw_url: ApiV2AdminResourceNodesGet_RAW_URL
 */
export const ApiV2AdminResourceNodesGet = function(parameters = {}) {
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
export const ApiV2AdminResourceNodesGet_RAW_URL = function() {
  return '/api/v2/admin/resource/nodes'
}
export const ApiV2AdminResourceNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceNodesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceTenantsGet
 * url: ApiV2AdminResourceTenantsGetURL
 * method: ApiV2AdminResourceTenantsGet_TYPE
 * raw_url: ApiV2AdminResourceTenantsGet_RAW_URL
 */
export const ApiV2AdminResourceTenantsGet = function(parameters = {}) {
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
export const ApiV2AdminResourceTenantsGet_RAW_URL = function() {
  return '/api/v2/admin/resource/tenants'
}
export const ApiV2AdminResourceTenantsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceTenantsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainersGet
 * url: ApiV2AdminResourceContainersGetURL
 * method: ApiV2AdminResourceContainersGet_TYPE
 * raw_url: ApiV2AdminResourceContainersGet_RAW_URL
 */
export const ApiV2AdminResourceContainersGet = function(parameters = {}) {
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
export const ApiV2AdminResourceContainersGet_RAW_URL = function() {
  return '/api/v2/admin/resource/containers'
}
export const ApiV2AdminResourceContainersGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceContainersGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainer_typesGet
 * url: ApiV2AdminResourceContainer_typesGetURL
 * method: ApiV2AdminResourceContainer_typesGet_TYPE
 * raw_url: ApiV2AdminResourceContainer_typesGet_RAW_URL
 */
export const ApiV2AdminResourceContainer_typesGet = function(parameters = {}) {
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
export const ApiV2AdminResourceContainer_typesGet_RAW_URL = function() {
  return '/api/v2/admin/resource/container-types'
}
export const ApiV2AdminResourceContainer_typesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceContainer_typesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainersByTenantIdByNameGet
 * url: ApiV2AdminResourceContainersByTenantIdByNameGetURL
 * method: ApiV2AdminResourceContainersByTenantIdByNameGet_TYPE
 * raw_url: ApiV2AdminResourceContainersByTenantIdByNameGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV2AdminResourceContainersByTenantIdByNameGet = function(parameters = {}) {
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
export const ApiV2AdminResourceContainersByTenantIdByNameGet_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}'
}
export const ApiV2AdminResourceContainersByTenantIdByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceContainersByTenantIdByNameGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainersByTenantIdByNameDelete
 * url: ApiV2AdminResourceContainersByTenantIdByNameDeleteURL
 * method: ApiV2AdminResourceContainersByTenantIdByNameDelete_TYPE
 * raw_url: ApiV2AdminResourceContainersByTenantIdByNameDelete_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV2AdminResourceContainersByTenantIdByNameDelete = function(parameters = {}) {
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
export const ApiV2AdminResourceContainersByTenantIdByNameDelete_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}'
}
export const ApiV2AdminResourceContainersByTenantIdByNameDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminResourceContainersByTenantIdByNameDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainersByTenantIdByNameLogGet
 * url: ApiV2AdminResourceContainersByTenantIdByNameLogGetURL
 * method: ApiV2AdminResourceContainersByTenantIdByNameLogGet_TYPE
 * raw_url: ApiV2AdminResourceContainersByTenantIdByNameLogGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV2AdminResourceContainersByTenantIdByNameLogGet = function(parameters = {}) {
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
export const ApiV2AdminResourceContainersByTenantIdByNameLogGet_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}/log'
}
export const ApiV2AdminResourceContainersByTenantIdByNameLogGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceContainersByTenantIdByNameLogGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminResourceContainersByTenantIdByNameEventsGet
 * url: ApiV2AdminResourceContainersByTenantIdByNameEventsGetURL
 * method: ApiV2AdminResourceContainersByTenantIdByNameEventsGet_TYPE
 * raw_url: ApiV2AdminResourceContainersByTenantIdByNameEventsGet_RAW_URL
 * @param tenantId - テナントID
 * @param name - コンテナ名
 */
export const ApiV2AdminResourceContainersByTenantIdByNameEventsGet = function(parameters = {}) {
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
export const ApiV2AdminResourceContainersByTenantIdByNameEventsGet_RAW_URL = function() {
  return '/api/v2/admin/resource/containers/{tenantId}/{name}/events'
}
export const ApiV2AdminResourceContainersByTenantIdByNameEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminResourceContainersByTenantIdByNameEventsGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantResourceContainersGet
 * url: ApiV2TenantResourceContainersGetURL
 * method: ApiV2TenantResourceContainersGet_TYPE
 * raw_url: ApiV2TenantResourceContainersGet_RAW_URL
 */
export const ApiV2TenantResourceContainersGet = function(parameters = {}) {
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
export const ApiV2TenantResourceContainersGet_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers'
}
export const ApiV2TenantResourceContainersGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantResourceContainersGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantResourceNodesGet
 * url: ApiV2TenantResourceNodesGetURL
 * method: ApiV2TenantResourceNodesGet_TYPE
 * raw_url: ApiV2TenantResourceNodesGet_RAW_URL
 */
export const ApiV2TenantResourceNodesGet = function(parameters = {}) {
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
export const ApiV2TenantResourceNodesGet_RAW_URL = function() {
  return '/api/v2/tenant/resource/nodes'
}
export const ApiV2TenantResourceNodesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantResourceNodesGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantResourceContainersByNameGet
 * url: ApiV2TenantResourceContainersByNameGetURL
 * method: ApiV2TenantResourceContainersByNameGet_TYPE
 * raw_url: ApiV2TenantResourceContainersByNameGet_RAW_URL
 * @param name - コンテナ名
 */
export const ApiV2TenantResourceContainersByNameGet = function(parameters = {}) {
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
export const ApiV2TenantResourceContainersByNameGet_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}'
}
export const ApiV2TenantResourceContainersByNameGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantResourceContainersByNameGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantResourceContainersByNameDelete
 * url: ApiV2TenantResourceContainersByNameDeleteURL
 * method: ApiV2TenantResourceContainersByNameDelete_TYPE
 * raw_url: ApiV2TenantResourceContainersByNameDelete_RAW_URL
 * @param name - 
 */
export const ApiV2TenantResourceContainersByNameDelete = function(parameters = {}) {
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
export const ApiV2TenantResourceContainersByNameDelete_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}'
}
export const ApiV2TenantResourceContainersByNameDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TenantResourceContainersByNameDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantResourceContainersByNameLogGet
 * url: ApiV2TenantResourceContainersByNameLogGetURL
 * method: ApiV2TenantResourceContainersByNameLogGet_TYPE
 * raw_url: ApiV2TenantResourceContainersByNameLogGet_RAW_URL
 * @param name - コンテナ名
 */
export const ApiV2TenantResourceContainersByNameLogGet = function(parameters = {}) {
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
export const ApiV2TenantResourceContainersByNameLogGet_RAW_URL = function() {
  return '/api/v2/tenant/resource/containers/{name}/log'
}
export const ApiV2TenantResourceContainersByNameLogGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantResourceContainersByNameLogGetURL = function(parameters = {}) {
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
 * 全ロール一覧を取得
 * request: ApiV2AdminRolesGet
 * url: ApiV2AdminRolesGetURL
 * method: ApiV2AdminRolesGet_TYPE
 * raw_url: ApiV2AdminRolesGet_RAW_URL
 */
export const ApiV2AdminRolesGet = function(parameters = {}) {
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
export const ApiV2AdminRolesGet_RAW_URL = function() {
  return '/api/v2/admin/roles'
}
export const ApiV2AdminRolesGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminRolesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRolesPost
 * url: ApiV2AdminRolesPostURL
 * method: ApiV2AdminRolesPost_TYPE
 * raw_url: ApiV2AdminRolesPost_RAW_URL
 * @param model - 
 */
export const ApiV2AdminRolesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/roles'
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
export const ApiV2AdminRolesPost_RAW_URL = function() {
  return '/api/v2/admin/roles'
}
export const ApiV2AdminRolesPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminRolesPostURL = function(parameters = {}) {
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
 * 指定されたIDのロール情報を取得。
 * request: ApiV2AdminRolesByIdGet
 * url: ApiV2AdminRolesByIdGetURL
 * method: ApiV2AdminRolesByIdGet_TYPE
 * raw_url: ApiV2AdminRolesByIdGet_RAW_URL
 * @param id - ロールID
 */
export const ApiV2AdminRolesByIdGet = function(parameters = {}) {
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
export const ApiV2AdminRolesByIdGet_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const ApiV2AdminRolesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminRolesByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminRolesByIdPut
 * url: ApiV2AdminRolesByIdPutURL
 * method: ApiV2AdminRolesByIdPut_TYPE
 * raw_url: ApiV2AdminRolesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV2AdminRolesByIdPut = function(parameters = {}) {
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
export const ApiV2AdminRolesByIdPut_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const ApiV2AdminRolesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminRolesByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminRolesByIdDelete
 * url: ApiV2AdminRolesByIdDeleteURL
 * method: ApiV2AdminRolesByIdDelete_TYPE
 * raw_url: ApiV2AdminRolesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV2AdminRolesByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminRolesByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/roles/{id}'
}
export const ApiV2AdminRolesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminRolesByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantRolesGet
 * url: ApiV2TenantRolesGetURL
 * method: ApiV2TenantRolesGet_TYPE
 * raw_url: ApiV2TenantRolesGet_RAW_URL
 */
export const ApiV2TenantRolesGet = function(parameters = {}) {
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
export const ApiV2TenantRolesGet_RAW_URL = function() {
  return '/api/v2/tenant/roles'
}
export const ApiV2TenantRolesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantRolesGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantRolesPost
 * url: ApiV2TenantRolesPostURL
 * method: ApiV2TenantRolesPost_TYPE
 * raw_url: ApiV2TenantRolesPost_RAW_URL
 * @param model - 
 */
export const ApiV2TenantRolesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant/roles'
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
export const ApiV2TenantRolesPost_RAW_URL = function() {
  return '/api/v2/tenant/roles'
}
export const ApiV2TenantRolesPost_TYPE = function() {
  return 'post'
}
export const ApiV2TenantRolesPostURL = function(parameters = {}) {
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
 * request: ApiV2TenantRolesByIdGet
 * url: ApiV2TenantRolesByIdGetURL
 * method: ApiV2TenantRolesByIdGet_TYPE
 * raw_url: ApiV2TenantRolesByIdGet_RAW_URL
 * @param id - ロールID
 */
export const ApiV2TenantRolesByIdGet = function(parameters = {}) {
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
export const ApiV2TenantRolesByIdGet_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const ApiV2TenantRolesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantRolesByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantRolesByIdPut
 * url: ApiV2TenantRolesByIdPutURL
 * method: ApiV2TenantRolesByIdPut_TYPE
 * raw_url: ApiV2TenantRolesByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV2TenantRolesByIdPut = function(parameters = {}) {
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
export const ApiV2TenantRolesByIdPut_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const ApiV2TenantRolesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2TenantRolesByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2TenantRolesByIdDelete
 * url: ApiV2TenantRolesByIdDeleteURL
 * method: ApiV2TenantRolesByIdDelete_TYPE
 * raw_url: ApiV2TenantRolesByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV2TenantRolesByIdDelete = function(parameters = {}) {
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
export const ApiV2TenantRolesByIdDelete_RAW_URL = function() {
  return '/api/v2/tenant/roles/{id}'
}
export const ApiV2TenantRolesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TenantRolesByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminStorageEndpointsGet
 * url: ApiV2AdminStorageEndpointsGetURL
 * method: ApiV2AdminStorageEndpointsGet_TYPE
 * raw_url: ApiV2AdminStorageEndpointsGet_RAW_URL
 */
export const ApiV2AdminStorageEndpointsGet = function(parameters = {}) {
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
export const ApiV2AdminStorageEndpointsGet_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints'
}
export const ApiV2AdminStorageEndpointsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminStorageEndpointsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminStorageEndpointsPost
 * url: ApiV2AdminStorageEndpointsPostURL
 * method: ApiV2AdminStorageEndpointsPost_TYPE
 * raw_url: ApiV2AdminStorageEndpointsPost_RAW_URL
 * @param model - 
 */
export const ApiV2AdminStorageEndpointsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/storage/endpoints'
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
export const ApiV2AdminStorageEndpointsPost_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints'
}
export const ApiV2AdminStorageEndpointsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminStorageEndpointsPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminStorageEndpointsByIdGet
 * url: ApiV2AdminStorageEndpointsByIdGetURL
 * method: ApiV2AdminStorageEndpointsByIdGet_TYPE
 * raw_url: ApiV2AdminStorageEndpointsByIdGet_RAW_URL
 * @param id - StorageエンドポイントID
 */
export const ApiV2AdminStorageEndpointsByIdGet = function(parameters = {}) {
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
export const ApiV2AdminStorageEndpointsByIdGet_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const ApiV2AdminStorageEndpointsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminStorageEndpointsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminStorageEndpointsByIdPut
 * url: ApiV2AdminStorageEndpointsByIdPutURL
 * method: ApiV2AdminStorageEndpointsByIdPut_TYPE
 * raw_url: ApiV2AdminStorageEndpointsByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV2AdminStorageEndpointsByIdPut = function(parameters = {}) {
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
export const ApiV2AdminStorageEndpointsByIdPut_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const ApiV2AdminStorageEndpointsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminStorageEndpointsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminStorageEndpointsByIdDelete
 * url: ApiV2AdminStorageEndpointsByIdDeleteURL
 * method: ApiV2AdminStorageEndpointsByIdDelete_TYPE
 * raw_url: ApiV2AdminStorageEndpointsByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV2AdminStorageEndpointsByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminStorageEndpointsByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/storage/endpoints/{id}'
}
export const ApiV2AdminStorageEndpointsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminStorageEndpointsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2UploadParameterGet
 * url: ApiV2UploadParameterGetURL
 * method: ApiV2UploadParameterGet_TYPE
 * raw_url: ApiV2UploadParameterGet_RAW_URL
 * @param fileName - ファイル名
 * @param partSum - 分割数
 * @param type - リソース名
 */
export const ApiV2UploadParameterGet = function(parameters = {}) {
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
export const ApiV2UploadParameterGet_RAW_URL = function() {
  return '/api/v2/upload/parameter'
}
export const ApiV2UploadParameterGet_TYPE = function() {
  return 'get'
}
export const ApiV2UploadParameterGetURL = function(parameters = {}) {
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
 * request: ApiV2UploadCompletePost
 * url: ApiV2UploadCompletePostURL
 * method: ApiV2UploadCompletePost_TYPE
 * raw_url: ApiV2UploadCompletePost_RAW_URL
 * @param model - 
 */
export const ApiV2UploadCompletePost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/upload/complete'
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
export const ApiV2UploadCompletePost_RAW_URL = function() {
  return '/api/v2/upload/complete'
}
export const ApiV2UploadCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV2UploadCompletePostURL = function(parameters = {}) {
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
 * request: ApiV2DownloadUrlGet
 * url: ApiV2DownloadUrlGetURL
 * method: ApiV2DownloadUrlGet_TYPE
 * raw_url: ApiV2DownloadUrlGet_RAW_URL
 * @param type - リソース種別
 * @param storedPath - 保存先パス
 * @param fileName - ダウンロード時に書き換えるファイル名
 * @param secure - HTTPS化するか
 */
export const ApiV2DownloadUrlGet = function(parameters = {}) {
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
export const ApiV2DownloadUrlGet_RAW_URL = function() {
  return '/api/v2/download/url'
}
export const ApiV2DownloadUrlGet_TYPE = function() {
  return 'get'
}
export const ApiV2DownloadUrlGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantTemplatesGet
 * url: ApiV2TenantTemplatesGetURL
 * method: ApiV2TenantTemplatesGet_TYPE
 * raw_url: ApiV2TenantTemplatesGet_RAW_URL
 * @param withTotal - 
 */
export const ApiV2TenantTemplatesGet = function(parameters = {}) {
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
export const ApiV2TenantTemplatesGet_RAW_URL = function() {
  return '/api/v2/tenant/templates'
}
export const ApiV2TenantTemplatesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantTemplatesGetURL = function(parameters = {}) {
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
 * request: ApiV2TemplatesGet
 * url: ApiV2TemplatesGetURL
 * method: ApiV2TemplatesGet_TYPE
 * raw_url: ApiV2TemplatesGet_RAW_URL
 * @param withTotal - 
 */
export const ApiV2TemplatesGet = function(parameters = {}) {
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
export const ApiV2TemplatesGet_RAW_URL = function() {
  return '/api/v2/templates'
}
export const ApiV2TemplatesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TemplatesGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesPost
 * url: ApiV2AdminTemplatesPostURL
 * method: ApiV2AdminTemplatesPost_TYPE
 * raw_url: ApiV2AdminTemplatesPost_RAW_URL
 * @param model - 
 */
export const ApiV2AdminTemplatesPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/templates'
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
export const ApiV2AdminTemplatesPost_RAW_URL = function() {
  return '/api/v2/admin/templates'
}
export const ApiV2AdminTemplatesPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminTemplatesPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesByIdGet
 * url: ApiV2AdminTemplatesByIdGetURL
 * method: ApiV2AdminTemplatesByIdGet_TYPE
 * raw_url: ApiV2AdminTemplatesByIdGet_RAW_URL
 * @param id - テンプレートID
 */
export const ApiV2AdminTemplatesByIdGet = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdGet_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const ApiV2AdminTemplatesByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminTemplatesByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesByIdPut
 * url: ApiV2AdminTemplatesByIdPutURL
 * method: ApiV2AdminTemplatesByIdPut_TYPE
 * raw_url: ApiV2AdminTemplatesByIdPut_RAW_URL
 * @param id - テンプレートID
 * @param model - 
 */
export const ApiV2AdminTemplatesByIdPut = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdPut_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const ApiV2AdminTemplatesByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminTemplatesByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesByIdDelete
 * url: ApiV2AdminTemplatesByIdDeleteURL
 * method: ApiV2AdminTemplatesByIdDelete_TYPE
 * raw_url: ApiV2AdminTemplatesByIdDelete_RAW_URL
 * @param id - テンプレートID
 */
export const ApiV2AdminTemplatesByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}'
}
export const ApiV2AdminTemplatesByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminTemplatesByIdDeleteURL = function(parameters = {}) {
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
 * テンプレートバージョン一覧を取得する
 * request: ApiV2AdminTemplatesByIdVersionsGet
 * url: ApiV2AdminTemplatesByIdVersionsGetURL
 * method: ApiV2AdminTemplatesByIdVersionsGet_TYPE
 * raw_url: ApiV2AdminTemplatesByIdVersionsGet_RAW_URL
 * @param id - テンプレートID
 */
export const ApiV2AdminTemplatesByIdVersionsGet = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdVersionsGet_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions'
}
export const ApiV2AdminTemplatesByIdVersionsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminTemplatesByIdVersionsGetURL = function(parameters = {}) {
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
 * テンプレートバージョンを作成する
 * request: ApiV2AdminTemplatesByIdVersionsPost
 * url: ApiV2AdminTemplatesByIdVersionsPostURL
 * method: ApiV2AdminTemplatesByIdVersionsPost_TYPE
 * raw_url: ApiV2AdminTemplatesByIdVersionsPost_RAW_URL
 * @param id - テンプレートID
 * @param model - 
 */
export const ApiV2AdminTemplatesByIdVersionsPost = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdVersionsPost_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions'
}
export const ApiV2AdminTemplatesByIdVersionsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminTemplatesByIdVersionsPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesByIdVersionsByVersionIdGet
 * url: ApiV2AdminTemplatesByIdVersionsByVersionIdGetURL
 * method: ApiV2AdminTemplatesByIdVersionsByVersionIdGet_TYPE
 * raw_url: ApiV2AdminTemplatesByIdVersionsByVersionIdGet_RAW_URL
 * @param id - テンプレートID
 * @param versionId - テンプレートバージョンID
 */
export const ApiV2AdminTemplatesByIdVersionsByVersionIdGet = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdVersionsByVersionIdGet_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions/{versionId}'
}
export const ApiV2AdminTemplatesByIdVersionsByVersionIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminTemplatesByIdVersionsByVersionIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminTemplatesByIdVersionsByVersionIdDelete
 * url: ApiV2AdminTemplatesByIdVersionsByVersionIdDeleteURL
 * method: ApiV2AdminTemplatesByIdVersionsByVersionIdDelete_TYPE
 * raw_url: ApiV2AdminTemplatesByIdVersionsByVersionIdDelete_RAW_URL
 * @param id - テンプレートID
 * @param versionId - テンプレートバージョンID
 */
export const ApiV2AdminTemplatesByIdVersionsByVersionIdDelete = function(parameters = {}) {
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
export const ApiV2AdminTemplatesByIdVersionsByVersionIdDelete_RAW_URL = function() {
  return '/api/v2/admin/templates/{id}/versions/{versionId}'
}
export const ApiV2AdminTemplatesByIdVersionsByVersionIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminTemplatesByIdVersionsByVersionIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminTenantsGet
 * url: ApiV2AdminTenantsGetURL
 * method: ApiV2AdminTenantsGet_TYPE
 * raw_url: ApiV2AdminTenantsGet_RAW_URL
 */
export const ApiV2AdminTenantsGet = function(parameters = {}) {
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
export const ApiV2AdminTenantsGet_RAW_URL = function() {
  return '/api/v2/admin/tenants'
}
export const ApiV2AdminTenantsGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminTenantsGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminTenantsPost
 * url: ApiV2AdminTenantsPostURL
 * method: ApiV2AdminTenantsPost_TYPE
 * raw_url: ApiV2AdminTenantsPost_RAW_URL
 * @param model - 登録内容
 */
export const ApiV2AdminTenantsPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/tenants'
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
export const ApiV2AdminTenantsPost_RAW_URL = function() {
  return '/api/v2/admin/tenants'
}
export const ApiV2AdminTenantsPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminTenantsPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminTenantsByIdGet
 * url: ApiV2AdminTenantsByIdGetURL
 * method: ApiV2AdminTenantsByIdGet_TYPE
 * raw_url: ApiV2AdminTenantsByIdGet_RAW_URL
 * @param id - テナントID
 */
export const ApiV2AdminTenantsByIdGet = function(parameters = {}) {
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
export const ApiV2AdminTenantsByIdGet_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const ApiV2AdminTenantsByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminTenantsByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminTenantsByIdPut
 * url: ApiV2AdminTenantsByIdPutURL
 * method: ApiV2AdminTenantsByIdPut_TYPE
 * raw_url: ApiV2AdminTenantsByIdPut_RAW_URL
 * @param id - テナントID
 * @param model - 変更内容
 */
export const ApiV2AdminTenantsByIdPut = function(parameters = {}) {
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
export const ApiV2AdminTenantsByIdPut_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const ApiV2AdminTenantsByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminTenantsByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminTenantsByIdDelete
 * url: ApiV2AdminTenantsByIdDeleteURL
 * method: ApiV2AdminTenantsByIdDelete_TYPE
 * raw_url: ApiV2AdminTenantsByIdDelete_RAW_URL
 * @param id - テナントID
 */
export const ApiV2AdminTenantsByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminTenantsByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/tenants/{id}'
}
export const ApiV2AdminTenantsByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminTenantsByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantGet
 * url: ApiV2TenantGetURL
 * method: ApiV2TenantGet_TYPE
 * raw_url: ApiV2TenantGet_RAW_URL
 */
export const ApiV2TenantGet = function(parameters = {}) {
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
export const ApiV2TenantGet_RAW_URL = function() {
  return '/api/v2/tenant'
}
export const ApiV2TenantGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantPut
 * url: ApiV2TenantPutURL
 * method: ApiV2TenantPut_TYPE
 * raw_url: ApiV2TenantPut_RAW_URL
 * @param model - 変更内容
 */
export const ApiV2TenantPut = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/tenant'
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
export const ApiV2TenantPut_RAW_URL = function() {
  return '/api/v2/tenant'
}
export const ApiV2TenantPut_TYPE = function() {
  return 'put'
}
export const ApiV2TenantPutURL = function(parameters = {}) {
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
 * request: ApiV2TrainingSimpleGet
 * url: ApiV2TrainingSimpleGetURL
 * method: ApiV2TrainingSimpleGet_TYPE
 * raw_url: ApiV2TrainingSimpleGet_RAW_URL
 */
export const ApiV2TrainingSimpleGet = function(parameters = {}) {
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
export const ApiV2TrainingSimpleGet_RAW_URL = function() {
  return '/api/v2/training/simple'
}
export const ApiV2TrainingSimpleGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingSimpleGetURL = function(parameters = {}) {
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
* request: ApiV2TrainingGet
* url: ApiV2TrainingGetURL
* method: ApiV2TrainingGet_TYPE
* raw_url: ApiV2TrainingGet_RAW_URL
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
export const ApiV2TrainingGet = function(parameters = {}) {
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
export const ApiV2TrainingGet_RAW_URL = function() {
  return '/api/v2/training'
}
export const ApiV2TrainingGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingGetURL = function(parameters = {}) {
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
 * マウントする学習履歴を取得
 * request: ApiV2TrainingMountGet
 * url: ApiV2TrainingMountGetURL
 * method: ApiV2TrainingMountGet_TYPE
 * raw_url: ApiV2TrainingMountGet_RAW_URL
 * @param status - ステータス
 */
export const ApiV2TrainingMountGet = function(parameters = {}) {
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
export const ApiV2TrainingMountGet_RAW_URL = function() {
  return '/api/v2/training/mount'
}
export const ApiV2TrainingMountGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingMountGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdGet
 * url: ApiV2TrainingByIdGetURL
 * method: ApiV2TrainingByIdGet_TYPE
 * raw_url: ApiV2TrainingByIdGet_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2TrainingByIdGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdGet_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const ApiV2TrainingByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdPut
 * url: ApiV2TrainingByIdPutURL
 * method: ApiV2TrainingByIdPut_TYPE
 * raw_url: ApiV2TrainingByIdPut_RAW_URL
 * @param id - 変更対象の学習履歴ID
 * @param model - 変更内容
 */
export const ApiV2TrainingByIdPut = function(parameters = {}) {
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
export const ApiV2TrainingByIdPut_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const ApiV2TrainingByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2TrainingByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdDelete
 * url: ApiV2TrainingByIdDeleteURL
 * method: ApiV2TrainingByIdDelete_TYPE
 * raw_url: ApiV2TrainingByIdDelete_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2TrainingByIdDelete = function(parameters = {}) {
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
export const ApiV2TrainingByIdDelete_RAW_URL = function() {
  return '/api/v2/training/{id}'
}
export const ApiV2TrainingByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TrainingByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdEventsGet
 * url: ApiV2TrainingByIdEventsGetURL
 * method: ApiV2TrainingByIdEventsGet_TYPE
 * raw_url: ApiV2TrainingByIdEventsGet_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2TrainingByIdEventsGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdEventsGet_RAW_URL = function() {
  return '/api/v2/training/{id}/events'
}
export const ApiV2TrainingByIdEventsGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdEventsGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingRunPost
 * url: ApiV2TrainingRunPostURL
 * method: ApiV2TrainingRunPost_TYPE
 * raw_url: ApiV2TrainingRunPost_RAW_URL
 * @param model - 新規学習実行内容
 */
export const ApiV2TrainingRunPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/training/run'
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
export const ApiV2TrainingRunPost_RAW_URL = function() {
  return '/api/v2/training/run'
}
export const ApiV2TrainingRunPost_TYPE = function() {
  return 'post'
}
export const ApiV2TrainingRunPostURL = function(parameters = {}) {
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
 * 学習履歴添付ファイルの一覧を取得する。
 * request: ApiV2TrainingByIdFilesGet
 * url: ApiV2TrainingByIdFilesGetURL
 * method: ApiV2TrainingByIdFilesGet_TYPE
 * raw_url: ApiV2TrainingByIdFilesGet_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param withUrl - 結果にダウンロード用のURLを含めるか
 */
export const ApiV2TrainingByIdFilesGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdFilesGet_RAW_URL = function() {
  return '/api/v2/training/{id}/files'
}
export const ApiV2TrainingByIdFilesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdFilesGetURL = function(parameters = {}) {
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
 * 学習履歴添付ファイルを登録する。
 * request: ApiV2TrainingByIdFilesPost
 * url: ApiV2TrainingByIdFilesPostURL
 * method: ApiV2TrainingByIdFilesPost_TYPE
 * raw_url: ApiV2TrainingByIdFilesPost_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param model - 追加するファイル情報
 */
export const ApiV2TrainingByIdFilesPost = function(parameters = {}) {
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
export const ApiV2TrainingByIdFilesPost_RAW_URL = function() {
  return '/api/v2/training/{id}/files'
}
export const ApiV2TrainingByIdFilesPost_TYPE = function() {
  return 'post'
}
export const ApiV2TrainingByIdFilesPostURL = function(parameters = {}) {
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
* コンテナの/output/配下から指定ディレクトリパスの直下を検索する
検索対象ディレクトリが見つからない場合もファイル・ディレクトリが空の結果を返す
* request: ApiV2TrainingByIdContainer_filesGet
* url: ApiV2TrainingByIdContainer_filesGetURL
* method: ApiV2TrainingByIdContainer_filesGet_TYPE
* raw_url: ApiV2TrainingByIdContainer_filesGet_RAW_URL
     * @param id - 対象の学習履歴ID
     * @param path - 検索対象ディレクトリ。使用可能文字は「-_1-9a-zA-Z/」
     * @param withUrl - 結果にダウンロード用のURLを含めるか
*/
export const ApiV2TrainingByIdContainer_filesGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdContainer_filesGet_RAW_URL = function() {
  return '/api/v2/training/{id}/container-files'
}
export const ApiV2TrainingByIdContainer_filesGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdContainer_filesGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdFilesByFileIdDelete
 * url: ApiV2TrainingByIdFilesByFileIdDeleteURL
 * method: ApiV2TrainingByIdFilesByFileIdDelete_TYPE
 * raw_url: ApiV2TrainingByIdFilesByFileIdDelete_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param fileId - 削除するファイルのID
 */
export const ApiV2TrainingByIdFilesByFileIdDelete = function(parameters = {}) {
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
export const ApiV2TrainingByIdFilesByFileIdDelete_RAW_URL = function() {
  return '/api/v2/training/{id}/files/{fileId}'
}
export const ApiV2TrainingByIdFilesByFileIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TrainingByIdFilesByFileIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdFilesByNameSizeGet
 * url: ApiV2TrainingByIdFilesByNameSizeGetURL
 * method: ApiV2TrainingByIdFilesByNameSizeGet_TYPE
 * raw_url: ApiV2TrainingByIdFilesByNameSizeGet_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param name - 対象ファイル名
 */
export const ApiV2TrainingByIdFilesByNameSizeGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdFilesByNameSizeGet_RAW_URL = function() {
  return '/api/v2/training/{id}/files/{name}/size'
}
export const ApiV2TrainingByIdFilesByNameSizeGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdFilesByNameSizeGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdTensorboardGet
 * url: ApiV2TrainingByIdTensorboardGetURL
 * method: ApiV2TrainingByIdTensorboardGet_TYPE
 * raw_url: ApiV2TrainingByIdTensorboardGet_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const ApiV2TrainingByIdTensorboardGet = function(parameters = {}) {
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
export const ApiV2TrainingByIdTensorboardGet_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const ApiV2TrainingByIdTensorboardGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingByIdTensorboardGetURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdTensorboardPut
 * url: ApiV2TrainingByIdTensorboardPutURL
 * method: ApiV2TrainingByIdTensorboardPut_TYPE
 * raw_url: ApiV2TrainingByIdTensorboardPut_RAW_URL
 * @param id - 対象の学習履歴ID
 * @param model - 起動モデル
 */
export const ApiV2TrainingByIdTensorboardPut = function(parameters = {}) {
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
export const ApiV2TrainingByIdTensorboardPut_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const ApiV2TrainingByIdTensorboardPut_TYPE = function() {
  return 'put'
}
export const ApiV2TrainingByIdTensorboardPutURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdTensorboardDelete
 * url: ApiV2TrainingByIdTensorboardDeleteURL
 * method: ApiV2TrainingByIdTensorboardDelete_TYPE
 * raw_url: ApiV2TrainingByIdTensorboardDelete_RAW_URL
 * @param id - 対象の学習履歴ID
 */
export const ApiV2TrainingByIdTensorboardDelete = function(parameters = {}) {
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
export const ApiV2TrainingByIdTensorboardDelete_RAW_URL = function() {
  return '/api/v2/training/{id}/tensorboard'
}
export const ApiV2TrainingByIdTensorboardDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TrainingByIdTensorboardDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdHaltPost
 * url: ApiV2TrainingByIdHaltPostURL
 * method: ApiV2TrainingByIdHaltPost_TYPE
 * raw_url: ApiV2TrainingByIdHaltPost_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2TrainingByIdHaltPost = function(parameters = {}) {
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
export const ApiV2TrainingByIdHaltPost_RAW_URL = function() {
  return '/api/v2/training/{id}/halt'
}
export const ApiV2TrainingByIdHaltPost_TYPE = function() {
  return 'post'
}
export const ApiV2TrainingByIdHaltPostURL = function(parameters = {}) {
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
* request: ApiV2TrainingByIdUser_cancelPost
* url: ApiV2TrainingByIdUser_cancelPostURL
* method: ApiV2TrainingByIdUser_cancelPost_TYPE
* raw_url: ApiV2TrainingByIdUser_cancelPost_RAW_URL
     * @param id - 学習履歴ID
*/
export const ApiV2TrainingByIdUser_cancelPost = function(parameters = {}) {
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
export const ApiV2TrainingByIdUser_cancelPost_RAW_URL = function() {
  return '/api/v2/training/{id}/user-cancel'
}
export const ApiV2TrainingByIdUser_cancelPost_TYPE = function() {
  return 'post'
}
export const ApiV2TrainingByIdUser_cancelPostURL = function(parameters = {}) {
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
 * request: ApiV2TrainingByIdCompletePost
 * url: ApiV2TrainingByIdCompletePostURL
 * method: ApiV2TrainingByIdCompletePost_TYPE
 * raw_url: ApiV2TrainingByIdCompletePost_RAW_URL
 * @param id - 学習履歴ID
 */
export const ApiV2TrainingByIdCompletePost = function(parameters = {}) {
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
export const ApiV2TrainingByIdCompletePost_RAW_URL = function() {
  return '/api/v2/training/{id}/complete'
}
export const ApiV2TrainingByIdCompletePost_TYPE = function() {
  return 'post'
}
export const ApiV2TrainingByIdCompletePostURL = function(parameters = {}) {
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
 * request: ApiV2TrainingTagsGet
 * url: ApiV2TrainingTagsGetURL
 * method: ApiV2TrainingTagsGet_TYPE
 * raw_url: ApiV2TrainingTagsGet_RAW_URL
 */
export const ApiV2TrainingTagsGet = function(parameters = {}) {
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
export const ApiV2TrainingTagsGet_RAW_URL = function() {
  return '/api/v2/training/tags'
}
export const ApiV2TrainingTagsGet_TYPE = function() {
  return 'get'
}
export const ApiV2TrainingTagsGetURL = function(parameters = {}) {
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
 * 管理者向けにユーザの一覧を取得する。
 * request: ApiV2AdminUsersGet
 * url: ApiV2AdminUsersGetURL
 * method: ApiV2AdminUsersGet_TYPE
 * raw_url: ApiV2AdminUsersGet_RAW_URL
 */
export const ApiV2AdminUsersGet = function(parameters = {}) {
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
export const ApiV2AdminUsersGet_RAW_URL = function() {
  return '/api/v2/admin/users'
}
export const ApiV2AdminUsersGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminUsersGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminUsersPost
 * url: ApiV2AdminUsersPostURL
 * method: ApiV2AdminUsersPost_TYPE
 * raw_url: ApiV2AdminUsersPost_RAW_URL
 * @param model - 
 */
export const ApiV2AdminUsersPost = function(parameters = {}) {
  const domain = parameters.$domain ? parameters.$domain : getDomain()
  const config = parameters.$config
  let path = '/api/v2/admin/users'
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
export const ApiV2AdminUsersPost_RAW_URL = function() {
  return '/api/v2/admin/users'
}
export const ApiV2AdminUsersPost_TYPE = function() {
  return 'post'
}
export const ApiV2AdminUsersPostURL = function(parameters = {}) {
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
 * request: ApiV2AdminUsersByIdGet
 * url: ApiV2AdminUsersByIdGetURL
 * method: ApiV2AdminUsersByIdGet_TYPE
 * raw_url: ApiV2AdminUsersByIdGet_RAW_URL
 * @param id - 
 */
export const ApiV2AdminUsersByIdGet = function(parameters = {}) {
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
export const ApiV2AdminUsersByIdGet_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const ApiV2AdminUsersByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2AdminUsersByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2AdminUsersByIdPut
 * url: ApiV2AdminUsersByIdPutURL
 * method: ApiV2AdminUsersByIdPut_TYPE
 * raw_url: ApiV2AdminUsersByIdPut_RAW_URL
 * @param id - 
 * @param model - 
 */
export const ApiV2AdminUsersByIdPut = function(parameters = {}) {
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
export const ApiV2AdminUsersByIdPut_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const ApiV2AdminUsersByIdPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminUsersByIdPutURL = function(parameters = {}) {
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
 * request: ApiV2AdminUsersByIdDelete
 * url: ApiV2AdminUsersByIdDeleteURL
 * method: ApiV2AdminUsersByIdDelete_TYPE
 * raw_url: ApiV2AdminUsersByIdDelete_RAW_URL
 * @param id - 
 */
export const ApiV2AdminUsersByIdDelete = function(parameters = {}) {
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
export const ApiV2AdminUsersByIdDelete_RAW_URL = function() {
  return '/api/v2/admin/users/{id}'
}
export const ApiV2AdminUsersByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2AdminUsersByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2AdminUsersByIdPasswordPut
 * url: ApiV2AdminUsersByIdPasswordPutURL
 * method: ApiV2AdminUsersByIdPasswordPut_TYPE
 * raw_url: ApiV2AdminUsersByIdPasswordPut_RAW_URL
 * @param id - 
 * @param password - 
 */
export const ApiV2AdminUsersByIdPasswordPut = function(parameters = {}) {
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
export const ApiV2AdminUsersByIdPasswordPut_RAW_URL = function() {
  return '/api/v2/admin/users/{id}/password'
}
export const ApiV2AdminUsersByIdPasswordPut_TYPE = function() {
  return 'put'
}
export const ApiV2AdminUsersByIdPasswordPutURL = function(parameters = {}) {
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
 * テナント向けに、所属しているユーザの一覧を取得する。
 * request: ApiV2TenantUsersGet
 * url: ApiV2TenantUsersGetURL
 * method: ApiV2TenantUsersGet_TYPE
 * raw_url: ApiV2TenantUsersGet_RAW_URL
 */
export const ApiV2TenantUsersGet = function(parameters = {}) {
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
export const ApiV2TenantUsersGet_RAW_URL = function() {
  return '/api/v2/tenant/users'
}
export const ApiV2TenantUsersGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantUsersGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantUsersByIdGet
 * url: ApiV2TenantUsersByIdGetURL
 * method: ApiV2TenantUsersByIdGet_TYPE
 * raw_url: ApiV2TenantUsersByIdGet_RAW_URL
 * @param id - 
 */
export const ApiV2TenantUsersByIdGet = function(parameters = {}) {
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
export const ApiV2TenantUsersByIdGet_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}'
}
export const ApiV2TenantUsersByIdGet_TYPE = function() {
  return 'get'
}
export const ApiV2TenantUsersByIdGetURL = function(parameters = {}) {
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
 * request: ApiV2TenantUsersByIdDelete
 * url: ApiV2TenantUsersByIdDeleteURL
 * method: ApiV2TenantUsersByIdDelete_TYPE
 * raw_url: ApiV2TenantUsersByIdDelete_RAW_URL
 * @param id - ユーザID
 */
export const ApiV2TenantUsersByIdDelete = function(parameters = {}) {
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
export const ApiV2TenantUsersByIdDelete_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}'
}
export const ApiV2TenantUsersByIdDelete_TYPE = function() {
  return 'delete'
}
export const ApiV2TenantUsersByIdDeleteURL = function(parameters = {}) {
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
 * request: ApiV2TenantUsersByIdRolesPut
 * url: ApiV2TenantUsersByIdRolesPutURL
 * method: ApiV2TenantUsersByIdRolesPut_TYPE
 * raw_url: ApiV2TenantUsersByIdRolesPut_RAW_URL
 * @param id - ユーザID
 * @param roleIds - 登録するロールID
 */
export const ApiV2TenantUsersByIdRolesPut = function(parameters = {}) {
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
export const ApiV2TenantUsersByIdRolesPut_RAW_URL = function() {
  return '/api/v2/tenant/users/{id}/roles'
}
export const ApiV2TenantUsersByIdRolesPut_TYPE = function() {
  return 'put'
}
export const ApiV2TenantUsersByIdRolesPutURL = function(parameters = {}) {
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
 * バージョン情報を取得
 * request: ApiV2VersionGet
 * url: ApiV2VersionGetURL
 * method: ApiV2VersionGet_TYPE
 * raw_url: ApiV2VersionGet_RAW_URL
 */
export const ApiV2VersionGet = function(parameters = {}) {
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
export const ApiV2VersionGet_RAW_URL = function() {
  return '/api/v2/version'
}
export const ApiV2VersionGet_TYPE = function() {
  return 'get'
}
export const ApiV2VersionGetURL = function(parameters = {}) {
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
