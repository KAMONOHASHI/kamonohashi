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
    getPartitions: gen.ApiV2TenantPartitionsGet,
    getQuota: gen.ApiV2TenantQuotaGet,

    admin: {
      getQuotas: gen.ApiV2AdminQuotasGet,
      postQuota: gen.ApiV2AdminQuotasPost,
      deleteTensorboards: gen.ApiV2AdminTensorboardsDelete,
    },
  },

  menu: {
    admin: {
      get: gen.ApiV2AdminMenusGet,
      put: gen.ApiV2AdminMenusByIdPut,
      getTypes: gen.ApiV2AdminMenu_typesGet,
    },

    tenant: {
      get: gen.ApiV2TenantMenusGet,
      put: gen.ApiV2TenantMenusByIdPut,
      getTypes: gen.ApiV2TenantMenu_typesGet,
    },
  },

  menuList: {
    getMenuList: gen.ApiV2AccountMenusListGet,
  },

  quotas: {
    get: gen.ApiV2AdminQuotasGet,
    post: gen.ApiV2AdminQuotasPost,
  },

  nodes: {
    admin: {
      get: gen.ApiV2AdminNodesGet,
      post: gen.ApiV2AdminNodesPost,
      getById: gen.ApiV2AdminNodesByIdGet,
      put: gen.ApiV2AdminNodesByIdPut,
      delete: gen.ApiV2AdminNodesByIdDelete,
      postSyncFromDb: gen.ApiV2AdminNodesSync_cluster_from_dbPost,
      getAccessLevel: gen.ApiV2AdminNode_access_levelsGet,
    },
  },
  registry: {
    admin: {
      get: gen.ApiV2AdminRegistryEndpointsGet,
      getById: gen.ApiV2AdminRegistryEndpointsByIdGet,
      getType: gen.ApiV2AdminRegistryTypesGet,
      post: gen.ApiV2AdminRegistryEndpointsPost,
      putById: gen.ApiV2AdminRegistryEndpointsByIdPut,
      deleteById: gen.ApiV2AdminRegistryEndpointsByIdDelete,
    },
    tenant: {
      getEndpoints: gen.ApiV2TenantRegistryEndpointsGet,
    },
    getImages: gen.ApiV2RegistriesByRegistryIdImagesGet,
    getTags: gen.ApiV2RegistriesByRegistryIdImagesByImageTagsGet,
  },

  account: {
    get: gen.ApiV2AccountGet,
    put: gen.ApiV2AccountPut,
    putPassword: gen.ApiV2AccountPasswordPut,
    postLogin: gen.ApiV2AccountLoginPost,
    postTokenTenants: gen.ApiV2AccountTenantsByTenantIdTokenPost,
    getTreeMenus: gen.ApiV2AccountMenusTreeGet,
    getListMenus: gen.ApiV2AccountMenusListGet,
    getRegistries: gen.ApiV2AccountRegistriesGet,
    putRegistries: gen.ApiV2AccountRegistriesPut,
    getGits: gen.ApiV2AccountGitsGet,
    putGits: gen.ApiV2AccountGitsPut,
  },

  role: {
    admin: {
      get: gen.ApiV2AdminRolesGet,
      post: gen.ApiV2AdminRolesPost,
      getById: gen.ApiV2AdminRolesByIdGet,
      put: gen.ApiV2AdminRolesByIdPut,
      delete: gen.ApiV2AdminRolesByIdDelete,
    },
    tenant: {
      get: gen.ApiV2TenantRolesGet,
      post: gen.ApiV2TenantRolesPost,
      getById: gen.ApiV2TenantRolesByIdGet,
      put: gen.ApiV2TenantRolesByIdPut,
      delete: gen.ApiV2TenantRolesByIdDelete,
    },
  },

  data: {
    get: gen.ApiV2DataGet,
    post: gen.ApiV2DataPost,
    getById: gen.ApiV2DataByIdGet,
    putById: gen.ApiV2DataByIdPut,
    deleteById: gen.ApiV2DataByIdDelete,
    getFilesByKey: gen.ApiV2DataByIdFilesByNameGet,
    getFilesById: gen.ApiV2DataByIdFilesGet,
    putFilesById: gen.ApiV2DataByIdFilesPost,
    deleteFilesById: gen.ApiV2DataByIdFilesByFileIdDelete,
    getDataTags: gen.ApiV2DataDatatagsGet,
  },

  datasets: {
    get: gen.ApiV2DatasetsGet,
    post: gen.ApiV2DatasetsPost,
    getById: gen.ApiV2DatasetsByIdGet,
    put: gen.ApiV2DatasetsByIdPut,
    delete: gen.ApiV2DatasetsByIdDelete,
    patch: gen.ApiV2DatasetsByIdPatch,
    getFiles: gen.ApiV2DataByIdFilesGet,
    getDatatypes: gen.ApiV2DatatypesGet,
  },

  git: {
    admin: {
      getEndpoints: gen.ApiV2AdminGitEndpointsGet,
      postEndpoint: gen.ApiV2AdminGitEndpointsPost,
      putEndpoint: gen.ApiV2AdminGitEndpointsByIdPut,
      getById: gen.ApiV2AdminGitEndpointsByIdGet,
      deleteById: gen.ApiV2AdminGitEndpointsByIdDelete,
      getTypes: gen.ApiV2AdminGitTypesGet,
    },
    tenant: {
      getEndpoints: gen.ApiV2TenantGitEndpointsGet,
    },
    getRepos: gen.ApiV2GitByGitIdReposGet,
    getBranches: gen.ApiV2GitByGitIdReposByOwnerByRepositoryNameBranchesGet,
    getCommits: gen.ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsGet,
    getCommit:
      gen.ApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitIdGet,
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: gen.ApiV2PreprocessingsGet,
    post: gen.ApiV2PreprocessingsPost,
    getById: gen.ApiV2PreprocessingsByIdGet,
    put: gen.ApiV2PreprocessingsByIdPut,
    delete: gen.ApiV2PreprocessingsByIdDelete,
    patch: gen.ApiV2PreprocessingsByIdPatch,
    getFilesById: gen.ApiV2PreprocessingsByIdHistoriesByDataIdFilesGet,
    getHistory: gen.ApiV2PreprocessingsByIdHistoriesGet,
    getHistroyById: gen.ApiV2PreprocessingsByIdHistoriesByDataIdGet,
    deleteHistroyById: gen.ApiV2PreprocessingsByIdHistoriesByDataIdDelete,
    getEventsById: gen.ApiV2PreprocessingsByIdHistoriesByDataIdEventsGet,
    runById: gen.ApiV2PreprocessingsByIdRunPost,
  },

  resource: {
    admin: {
      getNodes: gen.ApiV2AdminResourceNodesGet,
      getTenants: gen.ApiV2AdminResourceTenantsGet,
      getContainers: gen.ApiV2AdminResourceContainersGet,
      getContainerByName: gen.ApiV2AdminResourceContainersByTenantIdByNameGet,
      deleteContainerByName:
        gen.ApiV2AdminResourceContainersByTenantIdByNameDelete,
      getContainerLogByName:
        gen.ApiV2AdminResourceContainersByTenantIdByNameLogGet,
      getContainerEventsByName:
        gen.ApiV2AdminResourceContainersByTenantIdByNameEventsGet,
    },
    tenant: {
      getNodes: gen.ApiV2TenantResourceNodesGet,
      getContainers: gen.ApiV2TenantResourceContainersGet,
      getContainerByName: gen.ApiV2TenantResourceContainersByNameGet,
      deleteContainerByName: gen.ApiV2TenantResourceContainersByNameDelete,
      getContainerLogByName: gen.ApiV2TenantResourceContainersByNameLogGet,
    },
  },

  training: {
    getSimple: gen.ApiV2TrainingSimpleGet,
    get: gen.ApiV2TrainingGet,
    post: gen.ApiV2TrainingRunPost,
    getById: gen.ApiV2TrainingByIdGet,
    deleteById: gen.ApiV2TrainingByIdDelete,
    putById: gen.ApiV2TrainingByIdPut,
    // GET /spa/trains/{id}/log
    getFilesById: gen.ApiV2TrainingByIdFilesGet,
    postFilesById: gen.ApiV2TrainingByIdFilesPost,
    getContainerFilesById: gen.ApiV2TrainingByIdContainer_filesGet,
    deleteByIdFilesByFileId: gen.ApiV2TrainingByIdFilesByFileIdDelete,
    getTensorboardById: gen.ApiV2TrainingByIdTensorboardGet,
    putTensorboardById: gen.ApiV2TrainingByIdTensorboardPut,
    deleteTensorboardById: gen.ApiV2TrainingByIdTensorboardDelete,
    postHaltById: gen.ApiV2TrainingByIdHaltPost,
    postUserCancelById: gen.ApiV2TrainingByIdUser_cancelPost,
    getEventsById: gen.ApiV2TrainingByIdEventsGet,
    getMount: gen.ApiV2TrainingMountGet,
    getTags: gen.ApiV2TrainingTagsGet,
  },

  notebook: {
    getSimple: gen.ApiV2NotebookSimpleGet,
    get: gen.ApiV2NotebookGet,
    post: gen.ApiV2NotebookRunPost,
    getById: gen.ApiV2NotebookByIdGet,
    deleteById: gen.ApiV2NotebookByIdDelete,
    putById: gen.ApiV2NotebookByIdPut,
    getContainerFilesById: gen.ApiV2NotebookByIdContainer_filesGet,
    postHaltById: gen.ApiV2NotebookByIdHaltPost,
    getEventsById: gen.ApiV2NotebookByIdEventsGet,
    getEndpointById: gen.ApiV2NotebookByIdEndpointGet,
    getFilesById: gen.ApiV2NotebookByIdContainer_filesGet,
    postRerun: gen.ApiV2NotebookByIdRerunPost,
    getAvailableInfiniteTime: gen.ApiV2NotebookAvailable_infinite_timeGet,
  },

  inference: {
    getSimple: gen.ApiV2InferencesSimpleGet,
    get: gen.ApiV2InferencesGet,
    getMount: gen.ApiV2InferencesMountGet,
    post: gen.ApiV2InferencesRunPost,
    getById: gen.ApiV2InferencesByIdGet,
    deleteById: gen.ApiV2InferencesByIdDelete,
    putById: gen.ApiV2InferencesByIdPut,
    // GET /spa/trains/{id}/log
    getFilesById: gen.ApiV2InferencesByIdFilesGet,
    postFilesById: gen.ApiV2InferencesByIdFilesPost,
    getContainerFilesById: gen.ApiV2InferencesByIdContainer_filesGet,
    deleteByIdFilesByFileId: gen.ApiV2InferencesByIdFilesByFileIdDelete,
    postHaltById: gen.ApiV2InferencesByIdHaltPost,
    postUserCancelById: gen.ApiV2InferencesByIdUser_cancelPost,
    getEventsById: gen.ApiV2InferencesByIdEventsGet,
  },

  storage: {
    admin: {
      get: gen.ApiV2AdminStorageEndpointsGet,
      post: gen.ApiV2AdminStorageEndpointsPost,
      getById: gen.ApiV2AdminStorageEndpointsByIdGet,
      put: gen.ApiV2AdminStorageEndpointsByIdPut,
      delete: gen.ApiV2AdminStorageEndpointsByIdDelete,
    },
    getUploadParameter: gen.ApiV2UploadParameterGet,
    postUploadComplete: gen.ApiV2UploadCompletePost,
    getDownloadUrl: gen.ApiV2DownloadUrlGet,
  },

  tenant: {
    admin: {
      get: gen.ApiV2AdminTenantsGet,
      post: gen.ApiV2AdminTenantsPost,
      getById: gen.ApiV2AdminTenantsByIdGet,
      put: gen.ApiV2AdminTenantsByIdPut,
      delete: gen.ApiV2AdminTenantsByIdDelete,
    },
    get: gen.ApiV2TenantGet,
    put: gen.ApiV2TenantPut,
  },

  user: {
    admin: {
      get: gen.ApiV2AdminUsersGet,
      post: gen.ApiV2AdminUsersPost,
      getById: gen.ApiV2AdminUsersByIdGet,
      delete: gen.ApiV2AdminUsersByIdDelete,
      put: gen.ApiV2AdminUsersByIdPut,
      putPassword: simpleStringBody(
        gen.ApiV2AdminUsersByIdPasswordPut,
        'password',
      ),
    },

    tenant: {
      get: gen.ApiV2TenantUsersGet,
      getById: gen.ApiV2TenantUsersByIdGet,
      delete: gen.ApiV2TenantUsersByIdDelete,
      putRoles: gen.ApiV2TenantUsersByIdRolesPut,
    },
  },
  version: {
    get: gen.ApiV2VersionGet,
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
