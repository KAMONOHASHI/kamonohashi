import * as gen from './api.generator'
import axiosRoot from 'axios'
import * as ext from '@/util/axios-ext'

// -----------------------------------------------------------------------
// axiosの拡張
// テストでモックさせるためaxiosをexport
let axios = axiosRoot.create({
  // VUE_APP_API_HOST: .envから取得
  baseURL: 'http://' + (process.env.VUE_APP_API_HOST || ''),
  headers: { 'Content-Type': 'application/json' },
})
ext.axiosLoggerInterceptors(axios)
ext.axiosAuthInterceptors(axios)
ext.axiosLoadingInterceptors(axios)
ext.axiosErrorHandlingInterceptors(axios)
gen.setAxios(axios)

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
    getPartitions: gen.getApiV2TenantPartitions,
    getQuota: gen.getApiV2TenantQuota,
    getTenantNodes: gen.getApiV2TenantNodes,

    admin: {
      getQuotas: gen.getApiV2AdminQuotas,
      postQuota: gen.postApiV2AdminQuotas,
      deleteTensorboards: gen.deleteApiV2AdminTensorboards,
    },
  },

  menu: {
    admin: {
      get: gen.getApiV2AdminMenus,
      put: gen.putApiV2AdminMenusById,
      getTypes: gen.getApiV2AdminMenuTypes,
    },

    tenant: {
      get: gen.getApiV2TenantMenus,
      put: gen.putApiV2TenantMenusById,
      getTypes: gen.getApiV2TenantMenuTypes,
    },
  },

  menuList: {
    getMenuList: gen.getApiV2AccountMenusList,
  },

  quotas: {
    get: gen.getApiV2AdminQuotas,
    post: gen.postApiV2AdminQuotas,
  },

  nodes: {
    admin: {
      get: gen.getApiV2AdminNodes,
      post: gen.postApiV2AdminNodes,
      getById: gen.getApiV2AdminNodesById,
      put: gen.putApiV2AdminNodesById,
      delete: gen.deleteApiV2AdminNodesById,
      postSyncFromDb: gen.postApiV2AdminNodesSyncClusterFromDb,
      getAccessLevel: gen.getApiV2AdminNodeAccessLevels,
    },
  },
  registry: {
    admin: {
      get: gen.getApiV2AdminRegistryEndpoints,
      getById: gen.getApiV2AdminRegistryEndpointsById,
      getType: gen.getApiV2AdminRegistryTypes,
      post: gen.postApiV2AdminRegistryEndpoints,
      putById: gen.putApiV2AdminRegistryEndpointsById,
      deleteById: gen.deleteApiV2AdminRegistryEndpointsById,
    },
    tenant: {
      getEndpoints: gen.getApiV2TenantRegistryEndpoints,
    },
    getImages: gen.getApiV2RegistriesByRegistryIdImages,
    getTags: gen.getApiV2RegistriesByRegistryIdImagesByImageTags,
  },

  account: {
    get: gen.getApiV2Account,
    put: gen.putApiV2Account,
    putPassword: gen.putApiV2AccountPassword,
    postLogin: gen.postApiV2AccountLogin,
    postTokenTenants: gen.postApiV2AccountTenantsByTenantIdToken,
    getTreeMenus: gen.getApiV2AccountMenusTree,
    getListMenus: gen.getApiV2AccountMenusList,
    getRegistries: gen.getApiV2AccountRegistries,
    putRegistries: gen.putApiV2AccountRegistries,
    getGits: gen.getApiV2AccountGits,
    putGits: gen.putApiV2AccountGits,
    getWebhookSlack: gen.getApiV2AccountWebhookSlack,
    putWebhookSlack: gen.putApiV2AccountWebhookSlack,
    postWebhookSlackTest: gen.postApiV2AccountWebhookSlackTest,
  },

  role: {
    admin: {
      get: gen.getApiV2AdminRoles,
      post: gen.postApiV2AdminRoles,
      getById: gen.getApiV2AdminRolesById,
      put: gen.putApiV2AdminRolesById,
      delete: gen.deleteApiV2AdminRolesById,
    },
    tenant: {
      get: gen.getApiV2TenantRoles,
      post: gen.postApiV2TenantRoles,
      getById: gen.getApiV2TenantRolesById,
      put: gen.putApiV2TenantRolesById,
      delete: gen.deleteApiV2TenantRolesById,
    },
  },

  data: {
    get: gen.getApiV2Data,
    post: gen.postApiV2Data,
    getById: gen.getApiV2DataById,
    putById: gen.putApiV2DataById,
    deleteById: gen.deleteApiV2DataById,
    getFilesByKey: gen.getApiV2DataByIdFilesByName,
    getFilesById: gen.getApiV2DataByIdFiles,
    putFilesById: gen.postApiV2DataByIdFiles,
    deleteFilesById: gen.deleteApiV2DataByIdFilesByFileId,
    getDataTags: gen.getApiV2DataDatatags,
    getFileSize: gen.getApiV2DataByIdFilesByNameSize,
  },

  datasets: {
    get: gen.getApiV2Datasets,
    post: gen.postApiV2Datasets,
    getById: gen.getApiV2DatasetsById,
    put: gen.putApiV2DatasetsById,
    delete: gen.deleteApiV2DatasetsById,
    patch: gen.patchApiV2DatasetsById,
    getFiles: gen.getApiV2DataByIdFiles,
    getDatatypes: gen.getApiV2Datatypes,
  },

  git: {
    admin: {
      getEndpoints: gen.getApiV2AdminGitEndpoints,
      postEndpoint: gen.postApiV2AdminGitEndpoints,
      putEndpoint: gen.putApiV2AdminGitEndpointsById,
      getById: gen.getApiV2AdminGitEndpointsById,
      deleteById: gen.deleteApiV2AdminGitEndpointsById,
      getTypes: gen.getApiV2AdminGitTypes,
    },
    tenant: {
      getEndpoints: gen.getApiV2TenantGitEndpoints,
    },
    getRepos: gen.getApiV2GitByGitIdRepos,
    getBranches: gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches,
    getCommits: gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits,
    getCommit:
      gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId,
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: gen.getApiV2Preprocessings,
    post: gen.postApiV2Preprocessings,
    getById: gen.getApiV2PreprocessingsById,
    put: gen.putApiV2PreprocessingsById,
    delete: gen.deleteApiV2PreprocessingsById,
    patch: gen.patchApiV2PreprocessingsById,
    getFilesById: gen.getApiV2PreprocessingsByIdHistoriesByDataIdFiles,
    getHistory: gen.getApiV2PreprocessingsByIdHistories,
    getHistroyById: gen.getApiV2PreprocessingsByIdHistoriesByDataId,
    deleteHistroyById: gen.deleteApiV2PreprocessingsByIdHistoriesByDataId,
    getEventsById: gen.getApiV2PreprocessingsByIdHistoriesByDataIdEvents,
    runById: gen.postApiV2PreprocessingsByIdRun,
  },

  resource: {
    admin: {
      getNodes: gen.getApiV2AdminResourceNodes,
      getTenants: gen.getApiV2AdminResourceTenants,
      getContainers: gen.getApiV2AdminResourceContainers,
      getContainerByName: gen.getApiV2AdminResourceContainersByTenantIdByName,
      deleteContainerByName:
        gen.deleteApiV2AdminResourceContainersByTenantIdByName,
      getContainerLogByName:
        gen.getApiV2AdminResourceContainersByTenantIdByNameLog,
      getContainerEventsByName:
        gen.getApiV2AdminResourceContainersByTenantIdByNameEvents,
      getHistoriesContainersMetadata:
        gen.getApiV2AdminResourceHistoriesContainersMetadata,
      getHistoriesContainersData:
        gen.getApiV2AdminResourceHistoriesContainersData,
      deleteHistoriesContainers: gen.patchApiV2AdminResourceHistoriesContainers,
      getHistoriesJobsMetadata: gen.getApiV2AdminResourceHistoriesJobsMetadata,
      getHistoriesJobsData: gen.getApiV2AdminResourceHistoriesJobsData,
      deleteHistoriesJobs: gen.patchApiV2AdminResourceHistoriesJobs,
    },
    tenant: {
      getNodes: gen.getApiV2TenantResourceNodes,
      getContainers: gen.getApiV2TenantResourceContainers,
      getContainerByName: gen.getApiV2TenantResourceContainersByName,
      deleteContainerByName: gen.deleteApiV2TenantResourceContainersByName,
      getContainerLogByName: gen.getApiV2TenantResourceContainersByNameLog,
    },
  },

  training: {
    getSimple: gen.getApiV2TrainingSimple,
    get: gen.getApiV2Training,
    post: gen.postApiV2TrainingRun,
    getById: gen.getApiV2TrainingById,
    deleteById: gen.deleteApiV2TrainingById,
    putById: gen.putApiV2TrainingById,
    // GET /spa/trains/{id}/log
    getFilesById: gen.getApiV2TrainingByIdFiles,
    getFileSize: gen.getApiV2TrainingByIdFilesByNameSize,
    postFilesById: gen.postApiV2TrainingByIdFiles,
    getContainerFilesById: gen.getApiV2TrainingByIdContainerFiles,
    deleteByIdFilesByFileId: gen.deleteApiV2TrainingByIdFilesByFileId,
    getTensorboardById: gen.getApiV2TrainingByIdTensorboard,
    putTensorboardById: gen.putApiV2TrainingByIdTensorboard,
    deleteTensorboardById: gen.deleteApiV2TrainingByIdTensorboard,
    postHaltById: gen.postApiV2TrainingByIdHalt,
    postUserCancelById: gen.postApiV2TrainingByIdUserCancel,
    getEventsById: gen.getApiV2TrainingByIdEvents,
    getMount: gen.getApiV2TrainingMount,
    getTags: gen.getApiV2TrainingTags,
  },

  notebook: {
    getSimple: gen.getApiV2NotebookSimple,
    get: gen.getApiV2Notebook,
    post: gen.postApiV2NotebookRun,
    getById: gen.getApiV2NotebookById,
    deleteById: gen.deleteApiV2NotebookById,
    putById: gen.putApiV2NotebookById,
    getContainerFilesById: gen.getApiV2NotebookByIdContainerFiles,
    postHaltById: gen.postApiV2NotebookByIdHalt,
    getEventsById: gen.getApiV2NotebookByIdEvents,
    getEndpointById: gen.getApiV2NotebookByIdEndpoint,
    getFilesById: gen.getApiV2NotebookByIdContainerFiles,
    postRerun: gen.postApiV2NotebookByIdRerun,
    getAvailableInfiniteTime: gen.getApiV2NotebookAvailableInfiniteTime,
  },

  inference: {
    getSimple: gen.getApiV2InferencesSimple,
    get: gen.getApiV2Inferences,
    getMount: gen.getApiV2InferencesMount,
    post: gen.postApiV2InferencesRun,
    getById: gen.getApiV2InferencesById,
    deleteById: gen.deleteApiV2InferencesById,
    putById: gen.putApiV2InferencesById,
    // GET /spa/trains/{id}/log
    getFilesById: gen.getApiV2InferencesByIdFiles,
    getFileSize: gen.getApiV2InferencesByIdFilesByNameSize,
    postFilesById: gen.postApiV2InferencesByIdFiles,
    getContainerFilesById: gen.getApiV2InferencesByIdContainerFiles,
    deleteByIdFilesByFileId: gen.deleteApiV2InferencesByIdFilesByFileId,
    postHaltById: gen.postApiV2InferencesByIdHalt,
    postUserCancelById: gen.postApiV2InferencesByIdUserCancel,
    getEventsById: gen.getApiV2InferencesByIdEvents,
  },

  storage: {
    admin: {
      get: gen.getApiV2AdminStorageEndpoints,
      post: gen.postApiV2AdminStorageEndpoints,
      getById: gen.getApiV2AdminStorageEndpointsById,
      put: gen.putApiV2AdminStorageEndpointsById,
      delete: gen.deleteApiV2AdminStorageEndpointsById,
    },
    getUploadParameter: gen.getApiV2UploadParameter,
    postUploadComplete: gen.postApiV2UploadComplete,
    getDownloadUrl: gen.getApiV2DownloadUrl,
  },

  tenant: {
    admin: {
      get: gen.getApiV2AdminTenants,
      post: gen.postApiV2AdminTenants,
      getById: gen.getApiV2AdminTenantsById,
      put: gen.putApiV2AdminTenantsById,
      delete: gen.deleteApiV2AdminTenantsById,
    },
    get: gen.getApiV2Tenant,
    put: gen.putApiV2Tenant,
  },

  user: {
    admin: {
      get: gen.getApiV2AdminUsers,
      post: gen.postApiV2AdminUsers,
      getById: gen.getApiV2AdminUsersById,
      delete: gen.deleteApiV2AdminUsersById,
      put: gen.putApiV2AdminUsersById,
      putPassword: simpleStringBody(gen.putApiV2AdminUsersByIdPassword, 'body'),
    },

    tenant: {
      get: gen.getApiV2TenantUsers,
      getById: gen.getApiV2TenantUsersById,
      delete: gen.deleteApiV2TenantUsersById,
      putRoles: gen.putApiV2TenantUsersByIdRoles,
    },
  },
  version: {
    get: gen.getApiV2Version,
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
