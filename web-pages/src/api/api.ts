import * as ext from '@/util/axios-ext'
import * as gen from './api.generate'

import axios from 'axios'
const API_URL = 'http://' + process.env.VUE_APP_API_HOST //process.env

const axiosInstance = axios.create({
  baseURL: API_URL,
  headers: { 'Content-Type': 'application/json' },
})

ext.axiosAuthInterceptors(axiosInstance)
ext.axiosLoadingInterceptors(axiosInstance)
ext.axiosErrorHandlingInterceptors(axiosInstance, null)
ext.axiosLoggerInterceptors(axiosInstance)

const accountApi = new gen.AccountApi(undefined, '', axiosInstance)
const clusterApi = new gen.ClusterApi(undefined, '', axiosInstance)
const dataApi = new gen.DataApi(undefined, '', axiosInstance)
const dataSetApi = new gen.DataSetApi(undefined, '', axiosInstance)
const gitApi = new gen.GitApi(undefined, '', axiosInstance)
const inferenceApi = new gen.InferenceApi(undefined, '', axiosInstance)
const menuApi = new gen.MenuApi(undefined, '', axiosInstance)
const nodeApi = new gen.NodeApi(undefined, '', axiosInstance)
const notebookApi = new gen.NotebookApi(undefined, '', axiosInstance)
const preprocessingApi = new gen.PreprocessingApi(undefined, '', axiosInstance)
const registryApi = new gen.RegistryApi(undefined, '', axiosInstance)
const resourceApi = new gen.ResourceApi(undefined, '', axiosInstance)
const roleApi = new gen.RoleApi(undefined, '', axiosInstance)
const storageApi = new gen.StorageApi(undefined, '', axiosInstance)
const tenantApi = new gen.TenantApi(undefined, '', axiosInstance)
const trainingApi = new gen.TrainingApi(undefined, '', axiosInstance)
const userApi = new gen.UserApi(undefined, '', axiosInstance)
const userGroupApi = new gen.UserGroupApi(undefined, '', axiosInstance)
const versionApi = new gen.VersionApi(undefined, '', axiosInstance)

// -----------------------------------------------------------------------
// 使いやすいようにAPI領域で再定義
// （swagger-vue で自動生成生成：https://github.com/chenweiqun/swagger-vue）
let api = {
  cluster: {
    getPartitions: () => clusterApi.apiV2TenantPartitionsGet(),
    getQuota: () => clusterApi.apiV2TenantQuotaGet(),
    getTenantNodes: () => clusterApi.apiV2TenantNodesGet(),

    admin: {
      getQuotas: () => clusterApi.apiV2AdminQuotasGet(),
      postQuota: (params: {
        body: Array<gen.NssolPlatypusApiModelsClusterApiModelsQuotaInputModel>
      }) =>
        clusterApi.apiV2AdminQuotasPost({
          nssolPlatypusApiModelsClusterApiModelsQuotaInputModel: params.body,
        }),
      deleteTensorboards: () => clusterApi.apiV2AdminTensorboardsDelete(),
    },
  },

  menu: {
    admin: {
      get: () => menuApi.apiV2AdminMenusGet(),
      put: (params: {
        id: gen.NssolPlatypusInfrastructureMenuCode
        body: Array<number>
      }) =>
        menuApi.apiV2AdminMenusIdPut({
          id: params.id,
          requestBody: params.body,
        }),
      getTypes: () => menuApi.apiV2AdminMenuTypesGet(),
    },

    tenant: {
      get: () => menuApi.apiV2TenantMenusGet(),
      put: (params: {
        id: gen.NssolPlatypusInfrastructureMenuCode
        body: Array<number>
      }) =>
        menuApi.apiV2TenantMenusIdPut({
          id: params.id,
          requestBody: params.body,
        }),
      getTypes: () => menuApi.apiV2TenantMenuTypesGet(),
    },
  },

  menuList: {
    getMenuList: (params?: gen.AccountApiApiV2AccountMenusListGetRequest) =>
      accountApi.apiV2AccountMenusListGet(params),
  },

  quotas: {
    get: () => clusterApi.apiV2AdminQuotasGet(),
    post: (params: {
      body: Array<gen.NssolPlatypusApiModelsClusterApiModelsQuotaInputModel>
    }) =>
      clusterApi.apiV2AdminQuotasPost({
        nssolPlatypusApiModelsClusterApiModelsQuotaInputModel: params.body,
      }),
  },

  nodes: {
    admin: {
      get: (params: gen.NodeApiApiV2AdminNodesGetRequest) =>
        nodeApi.apiV2AdminNodesGet(params),
      post: (params: {
        body: gen.NssolPlatypusApiModelsNodeApiModelsCreateInputModel
      }) =>
        nodeApi.apiV2AdminNodesPost({
          nssolPlatypusApiModelsNodeApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.NodeApiApiV2AdminNodesIdGetRequest) =>
        nodeApi.apiV2AdminNodesIdGet(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsNodeApiModelsCreateInputModel
      }) =>
        nodeApi.apiV2AdminNodesIdPut({
          id: params.id,
          nssolPlatypusApiModelsNodeApiModelsCreateInputModel: params.body,
        }),
      delete: (params: { id: number }) =>
        nodeApi.apiV2AdminNodesIdDelete({ id: params.id }),
      postSyncFromDb: () => nodeApi.apiV2AdminNodesSyncClusterFromDbPost(),
      getAccessLevel: () => nodeApi.apiV2AdminNodeAccessLevelsGet(),
    },
  },
  registry: {
    admin: {
      get: () => registryApi.apiV2AdminRegistryEndpointsGet(),
      getById: (
        params: gen.RegistryApiApiV2AdminRegistryEndpointsIdGetRequest,
      ) => registryApi.apiV2AdminRegistryEndpointsIdGet(params),
      getType: () => registryApi.apiV2AdminRegistryTypesGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsRegistryApiModelsCreateInputModel
      }) =>
        registryApi.apiV2AdminRegistryEndpointsPost({
          nssolPlatypusApiModelsRegistryApiModelsCreateInputModel: params.body,
        }),
      putById: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsRegistryApiModelsCreateInputModel
      }) =>
        registryApi.apiV2AdminRegistryEndpointsIdPut({
          id: params.id,
          nssolPlatypusApiModelsRegistryApiModelsCreateInputModel: params.body,
        }),
      deleteById: (
        params: gen.RegistryApiApiV2AdminRegistryEndpointsIdDeleteRequest,
      ) => registryApi.apiV2AdminRegistryEndpointsIdDelete(params),
    },
    tenant: {
      getEndpoints: () => registryApi.apiV2TenantRegistryEndpointsGet(),
    },
    getImages: (
      params: gen.RegistryApiApiV2RegistriesRegistryIdImagesGetRequest,
    ) => registryApi.apiV2RegistriesRegistryIdImagesGet(params),
    getTags: (
      params: gen.RegistryApiApiV2RegistriesRegistryIdImagesImageTagsGetRequest,
    ) => registryApi.apiV2RegistriesRegistryIdImagesImageTagsGet(params),
  },

  account: {
    get: () => accountApi.apiV2AccountGet(),
    put: (params: gen.AccountApiApiV2AccountPutRequest) =>
      accountApi.apiV2AccountPut(params),
    putPassword: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsPasswordInputModel
    }) =>
      accountApi.apiV2AccountPasswordPut({
        nssolPlatypusApiModelsAccountApiModelsPasswordInputModel: params.body,
      }),
    postLogin: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsLoginInputModel
    }) =>
      accountApi.apiV2AccountLoginPost({
        nssolPlatypusApiModelsAccountApiModelsLoginInputModel: params.body,
      }),
    postTokenTenants: (params: {
      body: { expiresIn: number | null }
      tenantId: number
    }) =>
      accountApi.apiV2AccountTenantsTenantIdTokenPost({
        tenantId: params.tenantId,
        nssolPlatypusApiModelsAccountApiModelsSwitchTenantInputModel:
          params.body,
      }),
    getTreeMenus: () => accountApi.apiV2AccountMenusTreeGet(),
    getListMenus: () => accountApi.apiV2AccountMenusListGet(),
    getRegistries: () => accountApi.apiV2AccountRegistriesGet(),
    putRegistries: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsRegistryCredentialInputModel
    }) =>
      accountApi.apiV2AccountRegistriesPut({
        nssolPlatypusApiModelsAccountApiModelsRegistryCredentialInputModel:
          params.body,
      }),
    getGits: () => accountApi.apiV2AccountGitsGet(),
    putGits: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsGitCredentialInputModel
    }) =>
      accountApi.apiV2AccountGitsPut({
        nssolPlatypusApiModelsAccountApiModelsGitCredentialInputModel:
          params.body,
      }),
    getWebhookSlack: () => accountApi.apiV2AccountWebhookSlackGet(),
    putWebhookSlack: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsWebhookModel
    }) =>
      accountApi.apiV2AccountWebhookSlackPut({
        nssolPlatypusApiModelsAccountApiModelsWebhookModel: params.body,
      }),
    postWebhookSlackTest: (params: {
      body: gen.NssolPlatypusApiModelsAccountApiModelsWebhookModel
    }) =>
      accountApi.apiV2AccountWebhookSlackTestPost({
        nssolPlatypusApiModelsAccountApiModelsWebhookModel: params.body,
      }),
  },

  role: {
    admin: {
      get: () => roleApi.apiV2AdminRolesGet(),
      getTenantCommonRoles: () => roleApi.apiV2AdminTenantCommonRolesGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsRoleApiModelsCreateInputModel
      }) =>
        roleApi.apiV2AdminRolesPost({
          nssolPlatypusApiModelsRoleApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.RoleApiApiV2AdminRolesIdGetRequest) =>
        roleApi.apiV2AdminRolesIdGet(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsRoleApiModelsEditInputModel
      }) =>
        roleApi.apiV2AdminRolesIdPut({
          id: params.id,
          nssolPlatypusApiModelsRoleApiModelsEditInputModel: params.body,
        }),
      delete: (params: gen.RoleApiApiV2AdminRolesIdDeleteRequest) =>
        roleApi.apiV2AdminRolesIdDelete(params),
    },
    tenant: {
      get: () => roleApi.apiV2TenantRolesGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsRoleApiModelsCreateForTenantInputModel
      }) =>
        roleApi.apiV2TenantRolesPost({
          nssolPlatypusApiModelsRoleApiModelsCreateForTenantInputModel:
            params.body,
        }),
      getById: (params: gen.RoleApiApiV2TenantRolesIdGetRequest) =>
        roleApi.apiV2TenantRolesIdGet(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsRoleApiModelsEditForTenantInputModel
      }) =>
        roleApi.apiV2TenantRolesIdPut({
          id: params.id,
          nssolPlatypusApiModelsRoleApiModelsEditForTenantInputModel:
            params.body,
        }),
      delete: (params: gen.RoleApiApiV2TenantRolesIdDeleteRequest) =>
        roleApi.apiV2TenantRolesIdDelete(params),
    },
  },

  data: {
    get: (params: gen.DataApiApiV2DataGetRequest) =>
      dataApi.apiV2DataGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsDataApiModelsCreateInputModel
    }) =>
      dataApi.apiV2DataPost({
        nssolPlatypusApiModelsDataApiModelsCreateInputModel: params.body,
      }),
    getById: (params: gen.DataApiApiV2DataIdGetRequest) =>
      dataApi.apiV2DataIdGet(params),
    putById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsDataApiModelsEditInputModel
    }) =>
      dataApi.apiV2DataIdPut({
        id: params.id,
        nssolPlatypusApiModelsDataApiModelsEditInputModel: params.body,
      }),
    deleteById: (params: gen.DataApiApiV2DataIdDeleteRequest) =>
      dataApi.apiV2DataIdDelete(params),
    getFilesByKey: (params: gen.DataApiApiV2DataIdFilesNameGetRequest) =>
      dataApi.apiV2DataIdFilesNameGet(params),
    getFilesById: (params: gen.DataApiApiV2DataIdFilesGetRequest) =>
      dataApi.apiV2DataIdFilesGet(params),
    putFilesById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsDataApiModelsAddFilesInputModel
    }) =>
      dataApi.apiV2DataIdFilesPost({
        id: params.id,
        nssolPlatypusApiModelsDataApiModelsAddFilesInputModel: params.body,
      }),
    deleteFilesById: (params: gen.DataApiApiV2DataIdFilesFileIdDeleteRequest) =>
      dataApi.apiV2DataIdFilesFileIdDelete(params),
    getDataTags: () => dataApi.apiV2DataDatatagsGet(),
    getFileSize: (params: gen.DataApiApiV2DataIdFilesNameSizeGetRequest) =>
      dataApi.apiV2DataIdFilesNameSizeGet(params),
  },

  datasets: {
    get: (params: gen.DataSetApiApiV2DatasetsGetRequest) =>
      dataSetApi.apiV2DatasetsGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsDataSetApiModelsCreateInputModel
    }) =>
      dataSetApi.apiV2DatasetsPost({
        nssolPlatypusApiModelsDataSetApiModelsCreateInputModel: params.body,
      }),
    getById: (params: { id: number }) => dataSetApi.apiV2DatasetsIdGet(params),
    put: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsDataSetApiModelsEditEntriesInputModel
    }) =>
      dataSetApi.apiV2DatasetsIdPut({
        id: params.id,
        nssolPlatypusApiModelsDataSetApiModelsEditEntriesInputModel:
          params.body,
      }),
    delete: (params: gen.DataSetApiApiV2DatasetsIdDeleteRequest) =>
      dataSetApi.apiV2DatasetsIdDelete(params),
    patch: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsDataSetApiModelsEditInputModel
    }) =>
      dataSetApi.apiV2DatasetsIdPatch({
        id: params.id,
        nssolPlatypusApiModelsDataSetApiModelsEditInputModel: params.body,
      }),
    getFiles: (params: gen.DataSetApiApiV2DatasetsIdFilesGetRequest) =>
      dataSetApi.apiV2DatasetsIdFilesGet(params),
    getDatatypes: () => dataSetApi.apiV2DatatypesGet(),
  },

  git: {
    admin: {
      getEndpoints: () => gitApi.apiV2AdminGitEndpointsGet(),
      postEndpoint: (params: {
        body: gen.NssolPlatypusApiModelsGitApiModelsCreateInputModel
      }) =>
        gitApi.apiV2AdminGitEndpointsPost({
          nssolPlatypusApiModelsGitApiModelsCreateInputModel: params.body,
        }),
      putEndpoint: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsGitApiModelsCreateInputModel
      }) =>
        gitApi.apiV2AdminGitEndpointsIdPut({
          id: params.id,
          nssolPlatypusApiModelsGitApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.GitApiApiV2AdminGitEndpointsIdGetRequest) =>
        gitApi.apiV2AdminGitEndpointsIdGet(params),
      deleteById: (params: gen.GitApiApiV2AdminGitEndpointsIdDeleteRequest) =>
        gitApi.apiV2AdminGitEndpointsIdDelete(params),
      getTypes: () => gitApi.apiV2AdminGitTypesGet(),
    },
    tenant: {
      getEndpoints: () => gitApi.apiV2TenantGitEndpointsGet(),
    },
    getRepos: (params: gen.GitApiApiV2GitGitIdReposGetRequest) =>
      gitApi.apiV2GitGitIdReposGet(params),
    getBranches: (
      params: gen.GitApiApiV2GitGitIdReposOwnerRepositoryNameBranchesGetRequest,
    ) => gitApi.apiV2GitGitIdReposOwnerRepositoryNameBranchesGet(params),
    getCommits: (
      params: gen.GitApiApiV2GitGitIdReposOwnerRepositoryNameCommitsGetRequest,
    ) => gitApi.apiV2GitGitIdReposOwnerRepositoryNameCommitsGet(params),
    getCommit: (
      params: gen.GitApiApiV2GitGitIdReposOwnerRepositoryNameCommitsCommitIdGetRequest,
    ) => gitApi.apiV2GitGitIdReposOwnerRepositoryNameCommitsCommitIdGet(params),
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: (params: gen.PreprocessingApiApiV2PreprocessingsGetRequest) =>
      preprocessingApi.apiV2PreprocessingsGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel
    }) =>
      preprocessingApi.apiV2PreprocessingsPost({
        nssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel:
          params.body,
      }),
    getById: (params: gen.PreprocessingApiApiV2PreprocessingsIdGetRequest) =>
      preprocessingApi.apiV2PreprocessingsIdGet(params),
    put: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel
    }) =>
      preprocessingApi.apiV2PreprocessingsIdPut({
        id: params.id,
        nssolPlatypusApiModelsPreprocessingApiModelsCreateInputModel:
          params.body,
      }),
    delete: (params: gen.PreprocessingApiApiV2PreprocessingsIdDeleteRequest) =>
      preprocessingApi.apiV2PreprocessingsIdDelete(params),
    patch: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsPreprocessingApiModelsEditInputModel
    }) =>
      preprocessingApi.apiV2PreprocessingsIdPatch({
        id: params.id,
        nssolPlatypusApiModelsPreprocessingApiModelsEditInputModel: params.body,
      }),
    getFilesById: (
      params: gen.PreprocessingApiApiV2PreprocessingsIdHistoriesDataIdFilesGetRequest,
    ) => preprocessingApi.apiV2PreprocessingsIdHistoriesDataIdFilesGet(params),
    getHistory: (
      params: gen.PreprocessingApiApiV2PreprocessingsIdHistoriesGetRequest,
    ) => preprocessingApi.apiV2PreprocessingsIdHistoriesGet(params),
    getHistroyById: (
      params: gen.PreprocessingApiApiV2PreprocessingsIdHistoriesDataIdGetRequest,
    ) => preprocessingApi.apiV2PreprocessingsIdHistoriesDataIdGet(params),
    deleteHistroyById: (
      params: gen.PreprocessingApiApiV2PreprocessingsIdHistoriesDataIdDeleteRequest,
    ) => preprocessingApi.apiV2PreprocessingsIdHistoriesDataIdDelete(params),
    getEventsById: (
      params: gen.PreprocessingApiApiV2PreprocessingsIdHistoriesDataIdEventsGetRequest,
    ) => preprocessingApi.apiV2PreprocessingsIdHistoriesDataIdEventsGet(params),
    runById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsPreprocessingApiModelsRunPreprocessHistoryInputModel
    }) =>
      preprocessingApi.apiV2PreprocessingsIdRunPost({
        id: params.id,
        nssolPlatypusApiModelsPreprocessingApiModelsRunPreprocessHistoryInputModel:
          params.body,
      }),
  },

  resource: {
    admin: {
      getNodes: () => resourceApi.apiV2AdminResourceNodesGet(),
      getTenants: () => resourceApi.apiV2AdminResourceTenantsGet(),
      getContainers: () => resourceApi.apiV2AdminResourceContainersGet(),
      getContainerByName: (
        params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameGetRequest,
      ) => resourceApi.apiV2AdminResourceContainersTenantIdNameGet(params),
      deleteContainerByName: (
        params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameDeleteRequest,
      ) => resourceApi.apiV2AdminResourceContainersTenantIdNameDelete(params),
      getContainerLogByName: (
        params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameLogGetRequest,
      ) => resourceApi.apiV2AdminResourceContainersTenantIdNameLogGet(params),
      getContainerEventsByName: (
        params: gen.ResourceApiApiV2AdminResourceContainersTenantIdNameEventsGetRequest,
      ) =>
        resourceApi.apiV2AdminResourceContainersTenantIdNameEventsGet(params),
      getHistoriesContainersMetadata: () =>
        resourceApi.apiV2AdminResourceHistoriesContainersMetadataGet(),
      getHistoriesContainersData: (
        params: gen.ResourceApiApiV2AdminResourceHistoriesContainersDataGetRequest,
      ) => resourceApi.apiV2AdminResourceHistoriesContainersDataGet(params),
      deleteHistoriesContainers: (params: {
        body: gen.NssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel
      }) =>
        resourceApi.apiV2AdminResourceHistoriesContainersPatch({
          nssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel:
            params.body,
        }),
      getHistoriesJobsMetadata: () =>
        resourceApi.apiV2AdminResourceHistoriesJobsMetadataGet(),
      getHistoriesJobsData: (
        params: gen.ResourceApiApiV2AdminResourceHistoriesJobsDataGetRequest,
      ) => resourceApi.apiV2AdminResourceHistoriesJobsDataGet(params),
      deleteHistoriesJobs: (params: {
        body: gen.NssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel
      }) =>
        resourceApi.apiV2AdminResourceHistoriesJobsPatch({
          nssolPlatypusApiModelsResourceApiModelsHistoryDeleteInputModel:
            params.body,
        }),
    },
    tenant: {
      getNodes: () => resourceApi.apiV2TenantResourceNodesGet(),
      getContainers: () => resourceApi.apiV2TenantResourceContainersGet(),
      getContainerByName: (
        params: gen.ResourceApiApiV2TenantResourceContainersNameGetRequest,
      ) => resourceApi.apiV2TenantResourceContainersNameGet(params),
      deleteContainerByName: (
        params: gen.ResourceApiApiV2TenantResourceContainersNameDeleteRequest,
      ) => resourceApi.apiV2TenantResourceContainersNameDelete(params),
      getContainerLogByName: (
        params: gen.ResourceApiApiV2TenantResourceContainersNameLogGetRequest,
      ) => resourceApi.apiV2TenantResourceContainersNameLogGet(params),
    },
  },

  training: {
    getSimple: () => trainingApi.apiV2TrainingSimpleGet(),
    get: (params: gen.TrainingApiApiV2TrainingGetRequest) =>
      trainingApi.apiV2TrainingGet(params),
    getSearch: (params: gen.TrainingApiApiV2TrainingSearchGetRequest) =>
      trainingApi.apiV2TrainingSearchGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsTrainingApiModelsCreateInputModel
    }) =>
      trainingApi.apiV2TrainingRunPost({
        nssolPlatypusApiModelsTrainingApiModelsCreateInputModel: params.body,
      }),
    getById: (params: gen.TrainingApiApiV2TrainingIdGetRequest) =>
      trainingApi.apiV2TrainingIdGet(params),
    deleteById: (params: gen.TrainingApiApiV2TrainingIdDeleteRequest) =>
      trainingApi.apiV2TrainingIdDelete(params),
    putById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsTrainingApiModelsEditInputModel
    }) =>
      trainingApi.apiV2TrainingIdPut({
        id: params.id,
        nssolPlatypusApiModelsTrainingApiModelsEditInputModel: params.body,
      }),
    // GET /spa/trains/{id}/log
    getFilesById: (params: gen.TrainingApiApiV2TrainingIdFilesGetRequest) =>
      trainingApi.apiV2TrainingIdFilesGet(params),
    getFileSize: (
      params: gen.TrainingApiApiV2TrainingIdFilesNameSizeGetRequest,
    ) => trainingApi.apiV2TrainingIdFilesNameSizeGet(params),
    postFilesById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsComponentsAddFileInputModel
    }) =>
      trainingApi.apiV2TrainingIdFilesPost({
        id: params.id,
        nssolPlatypusApiModelsComponentsAddFileInputModel: params.body,
      }),
    getContainerFilesById: (
      params: gen.TrainingApiApiV2TrainingIdContainerFilesGetRequest,
    ) => trainingApi.apiV2TrainingIdContainerFilesGet(params),
    deleteByIdFilesByFileId: (
      params: gen.TrainingApiApiV2TrainingIdFilesFileIdDeleteRequest,
    ) => trainingApi.apiV2TrainingIdFilesFileIdDelete(params),
    getTensorboardById: (
      params: gen.TrainingApiApiV2TrainingIdTensorboardGetRequest,
    ) => trainingApi.apiV2TrainingIdTensorboardGet(params),
    putTensorboardById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsTrainingApiModelsTensorBoardInputModel
    }) =>
      trainingApi.apiV2TrainingIdTensorboardPut({
        id: params.id,
        nssolPlatypusApiModelsTrainingApiModelsTensorBoardInputModel:
          params.body,
      }),
    deleteTensorboardById: (
      params: gen.TrainingApiApiV2TrainingIdTensorboardDeleteRequest,
    ) => trainingApi.apiV2TrainingIdTensorboardDelete(params),
    postHaltById: (params: gen.TrainingApiApiV2TrainingIdHaltPostRequest) =>
      trainingApi.apiV2TrainingIdHaltPost(params),
    postUserCancelById: (
      params: gen.TrainingApiApiV2TrainingIdUserCancelPostRequest,
    ) => trainingApi.apiV2TrainingIdUserCancelPost(params),
    getEventsById: (params: gen.TrainingApiApiV2TrainingIdEventsGetRequest) =>
      trainingApi.apiV2TrainingIdEventsGet(params),
    getMount: (params: gen.TrainingApiApiV2TrainingMountGetRequest) =>
      trainingApi.apiV2TrainingMountGet(params),
    getTags: () => trainingApi.apiV2TrainingTagsGet(),
    postTags: (params: {
      body: gen.NssolPlatypusApiModelsTrainingApiModelsTagsInputModel
    }) =>
      trainingApi.apiV2TrainingTagsPost({
        nssolPlatypusApiModelsTrainingApiModelsTagsInputModel: params.body,
      }),
    deleteTags: (params: {
      body: gen.NssolPlatypusApiModelsTrainingApiModelsTagsInputModel
    }) =>
      trainingApi.apiV2TrainingTagsDelete({
        nssolPlatypusApiModelsTrainingApiModelsTagsInputModel: params.body,
      }),
    getSearchHistory: () => trainingApi.apiV2TrainingSearchHistoryGet(),
    postSearchHistory: (params: {
      body: gen.NssolPlatypusApiModelsTrainingApiModelsSearchHistoryInputModel
    }) =>
      trainingApi.apiV2TrainingSearchHistoryPost({
        nssolPlatypusApiModelsTrainingApiModelsSearchHistoryInputModel:
          params.body,
      }),
    deleteSearchHistoryById: (
      params: gen.TrainingApiApiV2TrainingSearchHistoryIdDeleteRequest,
    ) => trainingApi.apiV2TrainingSearchHistoryIdDelete(params),
    getSearchFill: () => trainingApi.apiV2TrainingSearchFillGet(),
  },

  notebook: {
    getSimple: () => notebookApi.apiV2NotebookSimpleGet(),
    get: (params: gen.NotebookApiApiV2NotebookGetRequest) =>
      notebookApi.apiV2NotebookGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsNotebookApiModelsCreateInputModel
    }) =>
      notebookApi.apiV2NotebookRunPost({
        nssolPlatypusApiModelsNotebookApiModelsCreateInputModel: params.body,
      }),
    getById: (params: gen.NotebookApiApiV2NotebookIdGetRequest) =>
      notebookApi.apiV2NotebookIdGet(params),
    deleteById: (params: gen.NotebookApiApiV2NotebookIdDeleteRequest) =>
      notebookApi.apiV2NotebookIdDelete(params),
    putById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsNotebookApiModelsEditInputModel
    }) =>
      notebookApi.apiV2NotebookIdPut({
        id: params.id,
        nssolPlatypusApiModelsNotebookApiModelsEditInputModel: params.body,
      }),
    getContainerFilesById: (
      params: gen.NotebookApiApiV2NotebookIdContainerFilesGetRequest,
    ) => notebookApi.apiV2NotebookIdContainerFilesGet(params),
    postHaltById: (params: gen.NotebookApiApiV2NotebookIdHaltPostRequest) =>
      notebookApi.apiV2NotebookIdHaltPost(params),
    getEventsById: (params: gen.NotebookApiApiV2NotebookIdEventsGetRequest) =>
      notebookApi.apiV2NotebookIdEventsGet(params),
    getEndpointById: (
      params: gen.NotebookApiApiV2NotebookIdEndpointGetRequest,
    ) => notebookApi.apiV2NotebookIdEndpointGet(params),
    getFilesById: (
      params: gen.NotebookApiApiV2NotebookIdContainerFilesGetRequest,
    ) => notebookApi.apiV2NotebookIdContainerFilesGet(params),
    postRerun: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsNotebookApiModelsRerunInputModel
    }) =>
      notebookApi.apiV2NotebookIdRerunPost({
        id: params.id,
        nssolPlatypusApiModelsNotebookApiModelsRerunInputModel: params.body,
      }),
    getAvailableInfiniteTime: () =>
      notebookApi.apiV2NotebookAvailableInfiniteTimeGet(),
  },

  inference: {
    getSimple: () => inferenceApi.apiV2InferencesSimpleGet(),
    get: (params: gen.InferenceApiApiV2InferencesGetRequest) =>
      inferenceApi.apiV2InferencesGet(params),
    getMount: (params: gen.InferenceApiApiV2InferencesMountGetRequest) =>
      inferenceApi.apiV2InferencesMountGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsInferenceApiModelsCreateInputModel
    }) =>
      inferenceApi.apiV2InferencesRunPost({
        nssolPlatypusApiModelsInferenceApiModelsCreateInputModel: params.body,
      }),
    getById: (params: gen.InferenceApiApiV2InferencesIdGetRequest) =>
      inferenceApi.apiV2InferencesIdGet(params),
    deleteById: (params: gen.InferenceApiApiV2InferencesIdDeleteRequest) =>
      inferenceApi.apiV2InferencesIdDelete(params),
    putById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsTrainingApiModelsEditInputModel
    }) =>
      inferenceApi.apiV2InferencesIdPut({
        id: params.id,
        nssolPlatypusApiModelsTrainingApiModelsEditInputModel: params.body,
      }),
    // GET /spa/trains/{id}/log
    getFilesById: (params: gen.InferenceApiApiV2InferencesIdFilesGetRequest) =>
      inferenceApi.apiV2InferencesIdFilesGet(params),
    getFileSize: (
      params: gen.InferenceApiApiV2InferencesIdFilesNameSizeGetRequest,
    ) => inferenceApi.apiV2InferencesIdFilesNameSizeGet(params),
    postFilesById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsComponentsAddFileInputModel
    }) =>
      inferenceApi.apiV2InferencesIdFilesPost({
        id: params.id,
        nssolPlatypusApiModelsComponentsAddFileInputModel: params.body,
      }),
    getContainerFilesById: (
      params: gen.InferenceApiApiV2InferencesIdContainerFilesGetRequest,
    ) => inferenceApi.apiV2InferencesIdContainerFilesGet(params),
    deleteByIdFilesByFileId: (
      params: gen.InferenceApiApiV2InferencesIdFilesFileIdDeleteRequest,
    ) => inferenceApi.apiV2InferencesIdFilesFileIdDelete(params),
    postHaltById: (params: gen.InferenceApiApiV2InferencesIdHaltPostRequest) =>
      inferenceApi.apiV2InferencesIdHaltPost(params),
    postUserCancelById: (
      params: gen.InferenceApiApiV2InferencesIdUserCancelPostRequest,
    ) => inferenceApi.apiV2InferencesIdUserCancelPost(params),
    getEventsById: (
      params: gen.InferenceApiApiV2InferencesIdEventsGetRequest,
    ) => inferenceApi.apiV2InferencesIdEventsGet(params),
  },

  storage: {
    admin: {
      get: () => storageApi.apiV2AdminStorageEndpointsGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsStorageApiModelsCreateInputModel
      }) =>
        storageApi.apiV2AdminStorageEndpointsPost({
          nssolPlatypusApiModelsStorageApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.StorageApiApiV2AdminStorageEndpointsIdGetRequest) =>
        storageApi.apiV2AdminStorageEndpointsIdGet(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsStorageApiModelsCreateInputModel
      }) =>
        storageApi.apiV2AdminStorageEndpointsIdPut({
          id: params.id,
          nssolPlatypusApiModelsStorageApiModelsCreateInputModel: params.body,
        }),
      delete: (
        params: gen.StorageApiApiV2AdminStorageEndpointsIdDeleteRequest,
      ) => storageApi.apiV2AdminStorageEndpointsIdDelete(params),
    },
    getUploadParameter: (
      params: gen.StorageApiApiV2UploadParameterGetRequest,
    ) => storageApi.apiV2UploadParameterGet(params),
    postUploadComplete: (params: {
      body: gen.NssolPlatypusLogicModelsStorageLogicModelsCompleteMultiplePartUploadInputModel
    }) =>
      storageApi.apiV2UploadCompletePost({
        nssolPlatypusLogicModelsStorageLogicModelsCompleteMultiplePartUploadInputModel:
          params.body,
      }),
    getDownloadUrl: (params: gen.StorageApiApiV2DownloadUrlGetRequest) =>
      storageApi.apiV2DownloadUrlGet(params),
  },

  tenant: {
    admin: {
      get: () => tenantApi.apiV2AdminTenantsGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsTenantApiModelsCreateInputModel
      }) =>
        tenantApi.apiV2AdminTenantsPost({
          nssolPlatypusApiModelsTenantApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.TenantApiApiV2AdminTenantsIdGetRequest) =>
        tenantApi.apiV2AdminTenantsIdGet(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsTenantApiModelsEditInputModel
      }) =>
        tenantApi.apiV2AdminTenantsIdPut({
          id: params.id,
          nssolPlatypusApiModelsTenantApiModelsEditInputModel: params.body,
        }),
      delete: (params: gen.TenantApiApiV2AdminTenantsIdDeleteRequest) =>
        tenantApi.apiV2AdminTenantsIdDelete(params),
    },
    get: () => tenantApi.apiV2TenantGet(),
    put: (params: {
      body: gen.NssolPlatypusApiModelsTenantApiModelsEditInputModel
    }) =>
      tenantApi.apiV2TenantPut({
        nssolPlatypusApiModelsTenantApiModelsEditInputModel: params.body,
      }),
  },

  user: {
    admin: {
      get: () => userApi.apiV2AdminUsersGet(),
      post: (params: {
        body: gen.NssolPlatypusApiModelsUserApiModelsCreateInputModel
      }) =>
        userApi.apiV2AdminUsersPost({
          nssolPlatypusApiModelsUserApiModelsCreateInputModel: params.body,
        }),
      getById: (params: gen.UserApiApiV2AdminUsersIdGetRequest) =>
        userApi.apiV2AdminUsersIdGet(params),
      delete: (params: gen.UserApiApiV2AdminUsersIdDeleteRequest) =>
        userApi.apiV2AdminUsersIdDelete(params),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsUserApiModelsEditInputModel
      }) =>
        userApi.apiV2AdminUsersIdPut({
          id: params.id,
          nssolPlatypusApiModelsUserApiModelsEditInputModel: params.body,
        }),
      putPassword: (params: gen.UserApiApiV2AdminUsersIdPasswordPutRequest) =>
        userApi.apiV2AdminUsersIdPasswordPut({
          id: params.id,
          body: JSON.stringify(params.body),
        }),

      postSyncLdap: (params: {
        body: gen.NssolPlatypusApiModelsUserApiModelsLdapAuthenticationInputModel
      }) =>
        userApi.apiV2AdminUsersSyncLdapPost({
          nssolPlatypusApiModelsUserApiModelsLdapAuthenticationInputModel:
            params.body,
        }),
    },

    tenant: {
      get: () => userApi.apiV2TenantUsersGet(),
      getById: (params: gen.UserApiApiV2TenantUsersIdGetRequest) =>
        userApi.apiV2TenantUsersIdGet(params),
      delete: (params: gen.UserApiApiV2TenantUsersIdDeleteRequest) =>
        userApi.apiV2TenantUsersIdDelete(params),
      putRoles: (params: { id: number; body: Array<number> }) =>
        userApi.apiV2TenantUsersIdRolesPut({
          id: params.id,
          requestBody: params.body,
        }),
    },
  },

  userGroup: {
    admin: {
      get: () => userGroupApi.apiV2AdminUsergroupGet(),
      getById: (params: gen.UserGroupApiApiV2AdminUsergroupIdGetRequest) =>
        userGroupApi.apiV2AdminUsergroupIdGet(params),
      post: (params: {
        body: gen.NssolPlatypusApiModelsUserGroupApiModelsCreateInputModel
      }) =>
        userGroupApi.apiV2AdminUsergroupPost({
          nssolPlatypusApiModelsUserGroupApiModelsCreateInputModel: params.body,
        }),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsUserGroupApiModelsCreateInputModel
      }) =>
        userGroupApi.apiV2AdminUsergroupIdPut({
          id: params.id,
          nssolPlatypusApiModelsUserGroupApiModelsCreateInputModel: params.body,
        }),
      delete: (params: gen.UserGroupApiApiV2AdminUsergroupIdDeleteRequest) =>
        userGroupApi.apiV2AdminUsergroupIdDelete(params),
    },
  },

  version: {
    get: () => versionApi.apiV2VersionGet(),
  },

  // dataを取り出すメソッド
  f: {
    data(response: any) {
      return [response.data]
    },
    dataTotal(response: any) {
      return [response.data, response.headers['x-total-count']]
    },
  },
}

export { api as default }
