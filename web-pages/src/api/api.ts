import * as gen from './api.generate'

import * as gen_ from './api.generator.js'
import axiosRoot from 'axios'
import * as ext from '@/util/axios-ext'

import axios from 'axios'
import { Configuration } from './configuration'
const API_URL = 'http://' + process.env.VUE_APP_API_HOST //process.env
const config = new Configuration({
  basePath: API_URL, //'http://localhost:44367',
})
const axiosInstance = axios.create({
  baseURL: API_URL,
  withCredentials: true,
})
ext.axiosLoggerInterceptors(axiosInstance)
ext.axiosAuthInterceptors(axiosInstance)
ext.axiosLoadingInterceptors(axiosInstance)
ext.axiosErrorHandlingInterceptors(axiosInstance, null)
//const userApi = new UserApi(config);
const dataApi = new gen.DataApi(config, '', axiosInstance)
console.log('api.ts')
console.log(dataApi)

// -----------------------------------------------------------------------
// axiosの拡張
// テストでモックさせるためaxiosをexport
let axios_ = axiosRoot.create({
  // VUE_APP_API_HOST: .envから取得
  baseURL: 'http://' + (process.env.VUE_APP_API_HOST || ''),
  headers: { 'Content-Type': 'application/json' },
})
ext.axiosLoggerInterceptors(axios_)
ext.axiosAuthInterceptors(axios_)
ext.axiosLoadingInterceptors(axios_)
ext.axiosErrorHandlingInterceptors(axios_)
gen_.setAxios(axios_)

// -----------------------------------------------------------------------
// HTTP Method の拡張
let simpleStringBody = function(func, paramName) {
  return async function(params) {
    if (paramName in params) {
      params[paramName] = '"' + params[paramName] + '"'
    }
    return await func(params)
  }
}

// -----------------------------------------------------------------------
// 使いやすいようにAPI領域で再定義
// （swagger-vue で自動生成生成：https://github.com/chenweiqun/swagger-vue）
let api = {
  cluster: {
    getPartitions: gen_.getApiV2TenantPartitions,
    getQuota: gen_.getApiV2TenantQuota,
    getTenantNodes: gen_.getApiV2TenantNodes,

    admin: {
      getQuotas: gen_.getApiV2AdminQuotas,
      postQuota: gen_.postApiV2AdminQuotas,
      deleteTensorboards: gen_.deleteApiV2AdminTensorboards,
    },
  },

  menu: {
    admin: {
      get: gen_.getApiV2AdminMenus,
      put: gen_.putApiV2AdminMenusById,
      getTypes: gen_.getApiV2AdminMenuTypes,
    },

    tenant: {
      get: gen_.getApiV2TenantMenus,
      put: gen_.putApiV2TenantMenusById,
      getTypes: gen_.getApiV2TenantMenuTypes,
    },
  },

  menuList: {
    getMenuList: gen_.getApiV2AccountMenusList,
  },

  quotas: {
    get: gen_.getApiV2AdminQuotas,
    post: gen_.postApiV2AdminQuotas,
  },

  nodes: {
    admin: {
      get: gen_.getApiV2AdminNodes,
      post: gen_.postApiV2AdminNodes,
      getById: gen_.getApiV2AdminNodesById,
      put: gen_.putApiV2AdminNodesById,
      delete: gen_.deleteApiV2AdminNodesById,
      postSyncFromDb: gen_.postApiV2AdminNodesSyncClusterFromDb,
      getAccessLevel: gen_.getApiV2AdminNodeAccessLevels,
    },
  },
  registry: {
    admin: {
      get: gen_.getApiV2AdminRegistryEndpoints,
      getById: gen_.getApiV2AdminRegistryEndpointsById,
      getType: gen_.getApiV2AdminRegistryTypes,
      post: gen_.postApiV2AdminRegistryEndpoints,
      putById: gen_.putApiV2AdminRegistryEndpointsById,
      deleteById: gen_.deleteApiV2AdminRegistryEndpointsById,
    },
    tenant: {
      getEndpoints: gen_.getApiV2TenantRegistryEndpoints,
    },
    getImages: gen_.getApiV2RegistriesByRegistryIdImages,
    getTags: gen_.getApiV2RegistriesByRegistryIdImagesByImageTags,
  },

  account: {
    get: gen_.getApiV2Account,
    put: gen_.putApiV2Account,
    putPassword: gen_.putApiV2AccountPassword,
    postLogin: gen_.postApiV2AccountLogin,
    postTokenTenants: gen_.postApiV2AccountTenantsByTenantIdToken,
    getTreeMenus: gen_.getApiV2AccountMenusTree,
    getListMenus: gen_.getApiV2AccountMenusList,
    getRegistries: gen_.getApiV2AccountRegistries,
    putRegistries: gen_.putApiV2AccountRegistries,
    getGits: gen_.getApiV2AccountGits,
    putGits: gen_.putApiV2AccountGits,
    getWebhookSlack: gen_.getApiV2AccountWebhookSlack,
    putWebhookSlack: gen_.putApiV2AccountWebhookSlack,
    postWebhookSlackTest: gen_.postApiV2AccountWebhookSlackTest,
  },

  role: {
    admin: {
      get: gen_.getApiV2AdminRoles,
      getTenantCommonRoles: gen_.getApiV2AdminTenantCommonRoles,
      post: gen_.postApiV2AdminRoles,
      getById: gen_.getApiV2AdminRolesById,
      put: gen_.putApiV2AdminRolesById,
      delete: gen_.deleteApiV2AdminRolesById,
    },
    tenant: {
      get: gen_.getApiV2TenantRoles,
      post: gen_.postApiV2TenantRoles,
      getById: gen_.getApiV2TenantRolesById,
      put: gen_.putApiV2TenantRolesById,
      delete: gen_.deleteApiV2TenantRolesById,
    },
  },

  data: {
    get: dataApi.apiV2DataGet,
    post: ({ body }: any) =>
      dataApi.apiV2DataPost({
        nssolPlatypusApiModelsDataApiModelsCreateInputModel: body,
      }),
    getById: dataApi.apiV2DataIdGet,
    putById: dataApi.apiV2DataIdPut,
    deleteById: dataApi.apiV2DataIdDelete,
    getFilesByKey: dataApi.apiV2DataIdFilesNameGet,
    getFilesById: dataApi.apiV2DataIdFilesGet,
    putFilesById: ({ id, body }: any) =>
      dataApi.apiV2DataIdFilesPost({
        id: id,
        nssolPlatypusApiModelsDataApiModelsAddFilesInputModel: body,
      }),
    deleteFilesById: dataApi.apiV2DataIdFilesFileIdDelete,
    getDataTags: dataApi.apiV2DataDatatagsGet,
    getFileSize: dataApi.apiV2DataIdFilesNameSizeGet,
  },

  datasets: {
    get: gen_.getApiV2Datasets,
    post: gen_.postApiV2Datasets,
    getById: gen_.getApiV2DatasetsById,
    put: gen_.putApiV2DatasetsById,
    delete: gen_.deleteApiV2DatasetsById,
    patch: gen_.patchApiV2DatasetsById,
    getFiles: gen_.getApiV2DataByIdFiles,
    getDatatypes: gen_.getApiV2Datatypes,
  },

  git: {
    admin: {
      getEndpoints: gen_.getApiV2AdminGitEndpoints,
      postEndpoint: gen_.postApiV2AdminGitEndpoints,
      putEndpoint: gen_.putApiV2AdminGitEndpointsById,
      getById: gen_.getApiV2AdminGitEndpointsById,
      deleteById: gen_.deleteApiV2AdminGitEndpointsById,
      getTypes: gen_.getApiV2AdminGitTypes,
    },
    tenant: {
      getEndpoints: gen_.getApiV2TenantGitEndpoints,
    },
    getRepos: gen_.getApiV2GitByGitIdRepos,
    getBranches: gen_.getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches,
    getCommits: gen_.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits,
    getCommit:
      gen_.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId,
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: gen_.getApiV2Preprocessings,
    post: gen_.postApiV2Preprocessings,
    getById: gen_.getApiV2PreprocessingsById,
    put: gen_.putApiV2PreprocessingsById,
    delete: gen_.deleteApiV2PreprocessingsById,
    patch: gen_.patchApiV2PreprocessingsById,
    getFilesById: gen_.getApiV2PreprocessingsByIdHistoriesByDataIdFiles,
    getHistory: gen_.getApiV2PreprocessingsByIdHistories,
    getHistroyById: gen_.getApiV2PreprocessingsByIdHistoriesByDataId,
    deleteHistroyById: gen_.deleteApiV2PreprocessingsByIdHistoriesByDataId,
    getEventsById: gen_.getApiV2PreprocessingsByIdHistoriesByDataIdEvents,
    runById: gen_.postApiV2PreprocessingsByIdRun,
  },

  resource: {
    admin: {
      getNodes: gen_.getApiV2AdminResourceNodes,
      getTenants: gen_.getApiV2AdminResourceTenants,
      getContainers: gen_.getApiV2AdminResourceContainers,
      getContainerByName: gen_.getApiV2AdminResourceContainersByTenantIdByName,
      deleteContainerByName:
        gen_.deleteApiV2AdminResourceContainersByTenantIdByName,
      getContainerLogByName:
        gen_.getApiV2AdminResourceContainersByTenantIdByNameLog,
      getContainerEventsByName:
        gen_.getApiV2AdminResourceContainersByTenantIdByNameEvents,
      getHistoriesContainersMetadata:
        gen_.getApiV2AdminResourceHistoriesContainersMetadata,
      getHistoriesContainersData:
        gen_.getApiV2AdminResourceHistoriesContainersData,
      deleteHistoriesContainers:
        gen_.patchApiV2AdminResourceHistoriesContainers,
      getHistoriesJobsMetadata: gen_.getApiV2AdminResourceHistoriesJobsMetadata,
      getHistoriesJobsData: gen_.getApiV2AdminResourceHistoriesJobsData,
      deleteHistoriesJobs: gen_.patchApiV2AdminResourceHistoriesJobs,
    },
    tenant: {
      getNodes: gen_.getApiV2TenantResourceNodes,
      getContainers: gen_.getApiV2TenantResourceContainers,
      getContainerByName: gen_.getApiV2TenantResourceContainersByName,
      deleteContainerByName: gen_.deleteApiV2TenantResourceContainersByName,
      getContainerLogByName: gen_.getApiV2TenantResourceContainersByNameLog,
    },
  },

  training: {
    getSimple: gen_.getApiV2TrainingSimple,
    get: gen_.getApiV2Training,
    getSearch: gen_.getApiV2TrainingSearch,
    post: gen_.postApiV2TrainingRun,
    getById: gen_.getApiV2TrainingById,
    deleteById: gen_.deleteApiV2TrainingById,
    putById: gen_.putApiV2TrainingById,
    // GET /spa/trains/{id}/log
    getFilesById: gen_.getApiV2TrainingByIdFiles,
    getFileSize: gen_.getApiV2TrainingByIdFilesByNameSize,
    postFilesById: gen_.postApiV2TrainingByIdFiles,
    getContainerFilesById: gen_.getApiV2TrainingByIdContainerFiles,
    deleteByIdFilesByFileId: gen_.deleteApiV2TrainingByIdFilesByFileId,
    getTensorboardById: gen_.getApiV2TrainingByIdTensorboard,
    putTensorboardById: gen_.putApiV2TrainingByIdTensorboard,
    deleteTensorboardById: gen_.deleteApiV2TrainingByIdTensorboard,
    postHaltById: gen_.postApiV2TrainingByIdHalt,
    postUserCancelById: gen_.postApiV2TrainingByIdUserCancel,
    getEventsById: gen_.getApiV2TrainingByIdEvents,
    getMount: gen_.getApiV2TrainingMount,
    getTags: gen_.getApiV2TrainingTags,
    postTags: gen_.postApiV2TrainingTags,
    deleteTags: gen_.deleteApiV2TrainingTags,
    getSearchHistory: gen_.getApiV2TrainingSearchHistory,
    postSearchHistory: gen_.postApiV2TrainingSearchHistory,
    deleteSearchHistoryById: gen_.deleteApiV2TrainingSearchHistoryById,
    getSearchFill: gen_.getApiV2TrainingSearchFill,
  },

  notebook: {
    getSimple: gen_.getApiV2NotebookSimple,
    get: gen_.getApiV2Notebook,
    post: gen_.postApiV2NotebookRun,
    getById: gen_.getApiV2NotebookById,
    deleteById: gen_.deleteApiV2NotebookById,
    putById: gen_.putApiV2NotebookById,
    getContainerFilesById: gen_.getApiV2NotebookByIdContainerFiles,
    postHaltById: gen_.postApiV2NotebookByIdHalt,
    getEventsById: gen_.getApiV2NotebookByIdEvents,
    getEndpointById: gen_.getApiV2NotebookByIdEndpoint,
    getFilesById: gen_.getApiV2NotebookByIdContainerFiles,
    postRerun: gen_.postApiV2NotebookByIdRerun,
    getAvailableInfiniteTime: gen_.getApiV2NotebookAvailableInfiniteTime,
  },

  inference: {
    getSimple: gen_.getApiV2InferencesSimple,
    get: gen_.getApiV2Inferences,
    getMount: gen_.getApiV2InferencesMount,
    post: gen_.postApiV2InferencesRun,
    getById: gen_.getApiV2InferencesById,
    deleteById: gen_.deleteApiV2InferencesById,
    putById: gen_.putApiV2InferencesById,
    // GET /spa/trains/{id}/log
    getFilesById: gen_.getApiV2InferencesByIdFiles,
    getFileSize: gen_.getApiV2InferencesByIdFilesByNameSize,
    postFilesById: gen_.postApiV2InferencesByIdFiles,
    getContainerFilesById: gen_.getApiV2InferencesByIdContainerFiles,
    deleteByIdFilesByFileId: gen_.deleteApiV2InferencesByIdFilesByFileId,
    postHaltById: gen_.postApiV2InferencesByIdHalt,
    postUserCancelById: gen_.postApiV2InferencesByIdUserCancel,
    getEventsById: gen_.getApiV2InferencesByIdEvents,
  },

  storage: {
    admin: {
      get: gen_.getApiV2AdminStorageEndpoints,
      post: gen_.postApiV2AdminStorageEndpoints,
      getById: gen_.getApiV2AdminStorageEndpointsById,
      put: gen_.putApiV2AdminStorageEndpointsById,
      delete: gen_.deleteApiV2AdminStorageEndpointsById,
    },
    getUploadParameter: gen_.getApiV2UploadParameter,
    postUploadComplete: gen_.postApiV2UploadComplete,
    getDownloadUrl: gen_.getApiV2DownloadUrl,
  },

  tenant: {
    admin: {
      get: gen_.getApiV2AdminTenants,
      post: gen_.postApiV2AdminTenants,
      getById: gen_.getApiV2AdminTenantsById,
      put: gen_.putApiV2AdminTenantsById,
      delete: gen_.deleteApiV2AdminTenantsById,
    },
    get: gen_.getApiV2Tenant,
    put: gen_.putApiV2Tenant,
  },

  user: {
    admin: {
      get: gen_.getApiV2AdminUsers,
      post: gen_.postApiV2AdminUsers,
      getById: gen_.getApiV2AdminUsersById,
      delete: gen_.deleteApiV2AdminUsersById,
      put: gen_.putApiV2AdminUsersById,
      putPassword: simpleStringBody(
        gen_.putApiV2AdminUsersByIdPassword,
        'body',
      ),
      postSyncLdap: gen_.postApiV2AdminUsersSyncLdap,
    },

    tenant: {
      get: gen_.getApiV2TenantUsers,
      getById: gen_.getApiV2TenantUsersById,
      delete: gen_.deleteApiV2TenantUsersById,
      putRoles: gen_.putApiV2TenantUsersByIdRoles,
    },
  },

  userGroup: {
    admin: {
      get: gen_.getApiV2AdminUsergroup,
      getById: gen_.getApiV2AdminUsergroupById,
      post: gen_.postApiV2AdminUsergroup,
      put: gen_.putApiV2AdminUsergroupById,
      delete: gen_.deleteApiV2AdminUsergroupById,
    },
  },

  version: {
    get: gen_.getApiV2Version,
  },

  // dataを取り出すメソッド
  f: {
    data(response) {
      return [response.data]
    },
    dataTotal(response) {
      return [response.data, response.headers['x-total-count']]
    },
  },
}

export { api as default }
