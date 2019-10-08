import * as gen from './api.generator'
import axiosRoot from 'axios'
import * as ext from '@/util/axios-ext'

// -----------------------------------------------------------------------
// axiosの拡張
// テストでモックさせるためaxiosをexport
let axios = axiosRoot.create({
  // API_HOST: webpackのdefine pluginから渡ってくる。 config/*.env.jsに定義がある。
  baseURL: 'http://' + (process.env.API_HOST || ''),
  headers: {'Content-Type': 'application/json'}
})
ext.axiosLoggerInterceptors(axios)
ext.axiosAuthInterceptors(axios)
ext.axiosLoadingInterceptors(axios)
ext.axiosErrorHandlingInterceptors(axios)
gen.setAxios(axios)

// -----------------------------------------------------------------------
// HTTP Method の拡張
/* eslint-disable */
let simpleStringBody = function (func, paramName) {
  return async function (params) {
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
    getPartitions: gen.ApiV1TenantPartitionsGet,

    admin: {
      getQuotas: gen.ApiV1AdminQuotasGet,
      postQuota: gen.ApiV1AdminQuotasPost,
      deleteTensorboards: gen.ApiV1AdminTensorboardsDelete
    }
  },

  menu: {
    admin: {
      get: gen.ApiV1AdminMenusGet,
      put: gen.ApiV1AdminMenusByIdPut,
      getTypes: gen.ApiV1AdminMenu_typesGet
    },

    tenant: {
      get: gen.ApiV1TenantMenusGet,
      put: gen.ApiV1TenantMenusByIdPut,
      getTypes: gen.ApiV1TenantMenu_typesGet
    }
  },

  menuList: {
    getMenuList: gen.ApiV1AccountMenusListGet
  },

  quotas: {
    get: gen.ApiV1AdminQuotasGet,
    post: gen.ApiV1AdminQuotasPost
  },

  nodes: {
    admin: {
      get: gen.ApiV1AdminNodesGet,
      post: gen.ApiV1AdminNodesPost,
      getById: gen.ApiV1AdminNodesByIdGet,
      put: gen.ApiV1AdminNodesByIdPut,
      delete: gen.ApiV1AdminNodesByIdDelete,
      postSyncFromDb: gen.ApiV1AdminNodesSync_cluster_from_dbPost,
      postSyncFromCluster: gen.ApiV1AdminNodesSync_db_from_clusterPost,
      getAccessLevel: gen.ApiV1AdminNode_access_levelsGet
    }
  },
  registry: {
    admin: {
      get: gen.ApiV1AdminRegistryEndpointsGet,
      getById: gen.ApiV1AdminRegistryEndpointsByIdGet,
      getType: gen.ApiV1AdminRegistryTypesGet,
      post: gen.ApiV1AdminRegistryEndpointsPost,
      putById: gen.ApiV1AdminRegistryEndpointsByIdPut,
      deleteById: gen.ApiV1AdminRegistryEndpointsByIdDelete
    },
    getImages: gen.ApiV1RegistriesByRegistryIdImagesGet,
    getTags: gen.ApiV1RegistriesByRegistryIdImagesByImageTagsGet
  },

  account: {
    get: gen.ApiV1AccountGet,
    put: gen.ApiV1AccountPut,
    putPassword: gen.ApiV1AccountPasswordPut,
    postLogin: gen.ApiV1AccountLoginPost,
    postTokenTenants: gen.ApiV1AccountTenantsByTenantIdTokenPost,
    getTreeMenus: gen.ApiV1AccountMenusTreeGet,
    getListMenus: gen.ApiV1AccountMenusListGet,
    getRegistries: gen.ApiV1AccountRegistriesGet,
    putRegistries: gen.ApiV1AccountRegistriesPut,
    getGits: gen.ApiV1AccountGitsGet,
    putGits: gen.ApiV1AccountGitsPut
  },

  role: {
    admin: {
      get: gen.ApiV1AdminRolesGet,
      post: gen.ApiV1AdminRolesPost,
      getById: gen.ApiV1AdminRolesByIdGet,
      put: gen.ApiV1AdminRolesByIdPut,
      delete: gen.ApiV1AdminRolesByIdDelete
    },
    tenant: {
      get: gen.ApiV1TenantRolesGet,
      post: gen.ApiV1TenantRolesPost,
      getById: gen.ApiV1TenantRolesByIdGet,
      put: gen.ApiV1TenantRolesByIdPut,
      delete: gen.ApiV1TenantRolesByIdDelete
    }
  },

  data: {
    get: gen.ApiV1DataGet,
    post: gen.ApiV1DataPost,
    getById: gen.ApiV1DataByIdGet,
    putById: gen.ApiV1DataByIdPut,
    deleteById: gen.ApiV1DataByIdDelete,
    getFilesByKey: gen.ApiV1DataByIdFilesByNameGet,
    getFilesById: gen.ApiV1DataByIdFilesGet,
    putFilesById: gen.ApiV1DataByIdFilesPost,
    getDataTags: gen.ApiV1DataDatatagsGet
  },

  datasets: {
    get: gen.ApiV1DatasetsGet,
    post: gen.ApiV1DatasetsPost,
    getById: gen.ApiV1DatasetsByIdGet,
    put: gen.ApiV1DatasetsByIdPut,
    delete: gen.ApiV1DatasetsByIdDelete,
    patch: gen.ApiV1DatasetsByIdPatch,
    getFiles: gen.ApiV1DataByIdFilesGet,
    getDatatypes: gen.ApiV1DatatypesGet
  },

  git: {
    admin: {
      getEndpoints: gen.ApiV1AdminGitEndpointsGet,
      postEndpoint: gen.ApiV1AdminGitEndpointsPost,
      putEndpoint: gen.ApiV1AdminGitEndpointsByIdPut,
      getById: gen.ApiV1AdminGitEndpointsByIdGet,
      deleteById: gen.ApiV1AdminGitEndpointsByIdDelete,
      getTypes: gen.ApiV1AdminGitTypesGet
    },
    getRepos: gen.ApiV1GitByGitIdReposGet,
    getBranches: gen.ApiV1GitByGitIdReposByOwnerByRepositoryNameBranchesGet,
    getCommits: gen.ApiV1GitByGitIdReposByOwnerByRepositoryNameCommitsGet
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: gen.ApiV1PreprocessingsGet,
    post: gen.ApiV1PreprocessingsPost,
    getById: gen.ApiV1PreprocessingsByIdGet,
    put: gen.ApiV1PreprocessingsByIdPut,
    delete: gen.ApiV1PreprocessingsByIdDelete,
    patch: gen.ApiV1PreprocessingsByIdPatch,
    getFilesById: gen.ApiV1PreprocessingsByIdHistoriesByDataIdFilesGet,
    getHistory: gen.ApiV1PreprocessingsByIdHistoriesGet,
    getHistroyById: gen.ApiV1PreprocessingsByIdHistoriesByDataIdGet,
    deleteHistroyById: gen.ApiV1PreprocessingsByIdHistoriesByDataIdDelete,
    getEventsById: gen.ApiV1PreprocessingsByIdHistoriesByDataIdEventsGet,
    runById: gen.ApiV1PreprocessingsByIdRunPost
  },

  resource: {
    admin: {
      getNodes: gen.ApiV1AdminResourceNodesGet,
      getTenants: gen.ApiV1AdminResourceTenantsGet,
      getContainers: gen.ApiV1AdminResourceContainersGet,
      getContainerByName: gen.ApiV1AdminResourceContainersByTenantIdByNameGet,
      deleteContainerByName: gen.ApiV1AdminResourceContainersByTenantIdByNameDelete,
      getContainerLogByName: gen.ApiV1AdminResourceContainersByTenantIdByNameLogGet,
      getContainerEventsByName: gen.ApiV1AdminResourceContainersByTenantIdByNameEventsGet
    },
    tenant: {
      getContainers: gen.ApiV1TenantResourceContainersGet,
      getContainerByName: gen.ApiV1TenantResourceContainersByNameGet,
      deleteContainerByName: gen.ApiV1TenantResourceContainersByNameDelete,
      getContainerLogByName: gen.ApiV1TenantResourceContainersByNameLogGet
    }
  },

  training: {
    getSimple: gen.ApiV1TrainingSimpleGet,
    get: gen.ApiV1TrainingGet,
    post: gen.ApiV1TrainingRunPost,
    getById: gen.ApiV1TrainingByIdGet,
    deleteById: gen.ApiV1TrainingByIdDelete,
    putById: gen.ApiV1TrainingByIdPut,
    // GET /spa/trains/{id}/log
    getFilesById: gen.ApiV1TrainingByIdFilesGet,
    postFilesById: gen.ApiV1TrainingByIdFilesPost,
    getContainerFilesById: gen.ApiV1TrainingByIdContainer_filesGet,
    deleteByIdFilesByFileId: gen.ApiV1TrainingByIdFilesByFileIdDelete,
    getTensorboardById: gen.ApiV1TrainingByIdTensorboardGet,
    putTensorboardById: gen.ApiV1TrainingByIdTensorboardPut,
    deleteTensorboardById: gen.ApiV1TrainingByIdTensorboardDelete,
    postHaltById: gen.ApiV1TrainingByIdHaltPost,
    postUserCancelById: gen.ApiV1TrainingByIdUser_cancelPost,
    getEventsById: gen.ApiV1TrainingByIdEventsGet,
    getMount: gen.ApiV1TrainingMountGet
  },

  notebook: {
    getSimple: gen.ApiV1NotebookSimpleGet,
    get: gen.ApiV1NotebookGet,
    post: gen.ApiV1NotebookRunPost,
    getById: gen.ApiV1NotebookByIdGet,
    deleteById: gen.ApiV1NotebookByIdDelete,
    putById: gen.ApiV1NotebookByIdPut,
    getContainerFilesById: gen.ApiV1NotebookByIdContainer_filesGet,
    postHaltById: gen.ApiV1NotebookByIdHaltPost,
    getEventsById: gen.ApiV1NotebookByIdEventsGet,
    getEndpointById: gen.ApiV1NotebookByIdEndpointGet,
    getFilesById: gen.ApiV1NotebookByIdContainer_filesGet,
    postRerun: gen.ApiV1NotebookByIdRerunPost
  },

  inference: {
    getSimple: gen.ApiV1InferencesSimpleGet,
    get: gen.ApiV1InferencesGet,
    post: gen.ApiV1InferencesRunPost,
    getById: gen.ApiV1InferencesByIdGet,
    deleteById: gen.ApiV1InferencesByIdDelete,
    putById: gen.ApiV1InferencesByIdPut,
    // GET /spa/trains/{id}/log
    getFilesById: gen.ApiV1InferencesByIdFilesGet,
    postFilesById: gen.ApiV1InferencesByIdFilesPost,
    getContainerFilesById: gen.ApiV1InferencesByIdContainer_filesGet,
    deleteByIdFilesByFileId: gen.ApiV1InferencesByIdFilesByFileIdDelete,
    postHaltById: gen.ApiV1InferencesByIdHaltPost,
    postUserCancelById: gen.ApiV1InferencesByIdUser_cancelPost,
    getEventsById: gen.ApiV1InferencesByIdEventsGet
  },

  storage: {
    admin: {
      get: gen.ApiV1AdminStorageEndpointsGet,
      post: gen.ApiV1AdminStorageEndpointsPost,
      getById: gen.ApiV1AdminStorageEndpointsByIdGet,
      put: gen.ApiV1AdminStorageEndpointsByIdPut,
      delete: gen.ApiV1AdminStorageEndpointsByIdDelete
    },
    getUploadParameter: gen.ApiV1UploadParameterGet,
    postUploadComplete: gen.ApiV1UploadCompletePost,
    getDownloadUrl: gen.ApiV1DownloadUrlGet
  },

  tenant: {
    admin: {
      get: gen.ApiV1AdminTenantsGet,
      post: gen.ApiV1AdminTenantsPost,
      getById: gen.ApiV1AdminTenantsByIdGet,
      put: gen.ApiV1AdminTenantsByIdPut,
      getMembers: gen.ApiV1AdminTenantsByIdMembersGet,
      delete: gen.ApiV1AdminTenantsByIdDelete
    }
  },

  user: {
    admin: {
      get: gen.ApiV1AdminUsersGet,
      post: gen.ApiV1AdminUsersPost,
      getById: gen.ApiV1AdminUsersByIdGet,
      delete: gen.ApiV1AdminUsersByIdDelete,
      put: gen.ApiV1AdminUsersByIdPut,
      putPassword: simpleStringBody(gen.ApiV1AdminUsersByIdPasswordPut, 'password')
    },

    tenant: {
      get: gen.ApiV1TenantUsersGet,
      getById: gen.ApiV1TenantUsersByIdGet,
      delete: gen.ApiV1TenantUsersByIdDelete,
      putRoles: gen.ApiV1TenantUsersByIdRolesPut
    }
  },

  // dataを取り出すメソッド
  f: {
    data (response) {
      return [response.data]
    },
    dataTotal (response) {
      return [response.data, response.headers['x-total-count']]
    }
  }
}

export {api as default}
