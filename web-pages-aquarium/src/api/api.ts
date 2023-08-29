import * as gen from './api.generate'
//import axiosRoot from 'axios'
import * as ext from '@/util/axios-ext'

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
const templateApi = new gen.TemplateApi(undefined, '', axiosInstance)
const registryApi = new gen.RegistryApi(undefined, '', axiosInstance)
const dataApi = new gen.DataApi(undefined, '', axiosInstance)
const aquariumDataSetApi = new gen.AquariumDataSetApi(
  undefined,
  '',
  axiosInstance,
)
const dataSetApi = new gen.DataSetApi(undefined, '', axiosInstance)
const gitApi = new gen.GitApi(undefined, '', axiosInstance)
const trainingApi = new gen.TrainingApi(undefined, '', axiosInstance)
const experimentApi = new gen.ExperimentApi(undefined, '', axiosInstance)
const storageApi = new gen.StorageApi(undefined, '', axiosInstance)
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

  menuList: {
    getMenuList: (
      params?: gen.AccountApiApiV2AccountAquariumMenusListGetRequest,
    ) => accountApi.apiV2AccountAquariumMenusListGet(params),
  },

  templates: {
    admin: {
      getById: (params: { id: number }) =>
        templateApi.apiV2AdminTemplatesIdGet(params),
      get: (params: gen.TemplateApiApiV2TemplatesGetRequest) =>
        templateApi.apiV2TemplatesGet(params),
      post: (params: {
        body: gen.NssolPlatypusApiModelsTemplateApiModelsCreateInputModel
      }) =>
        templateApi.apiV2AdminTemplatesPost({
          nssolPlatypusApiModelsTemplateApiModelsCreateInputModel: params.body,
        }),
      put: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsTemplateApiModelsEditInputModel
      }) =>
        templateApi.apiV2AdminTemplatesIdPut({
          id: params.id,
          nssolPlatypusApiModelsTemplateApiModelsEditInputModel: params.body,
        }),
      delete: (params: { id: number }) =>
        templateApi.apiV2AdminTemplatesIdDelete(params),
      deleteVersion: (params: { id: number; versionId: number }) =>
        templateApi.apiV2AdminTemplatesIdVersionsVersionIdDelete({
          id: params.id,
          versionId: params.versionId,
        }),
      getByIdVersions: (params: { id: number }) =>
        templateApi.apiV2AdminTemplatesIdVersionsGet(params),
      postByIdVersions: (params: {
        id: number
        body: gen.NssolPlatypusApiModelsTemplateApiModelsVersionCreateInputModel
      }) =>
        templateApi.apiV2AdminTemplatesIdVersionsPost({
          id: params.id,
          nssolPlatypusApiModelsTemplateApiModelsVersionCreateInputModel:
            params.body,
        }),
      getByIdVersionsByVersionId: (params: { id: number; versionId: number }) =>
        templateApi.apiV2AdminTemplatesIdVersionsVersionIdGet({
          id: params.id,
          versionId: params.versionId,
        }),
    },
    getTenantTemplate: (
      params: gen.TemplateApiApiV2TenantTemplatesGetRequest,
    ) => templateApi.apiV2TenantTemplatesGet(params),
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
      tenantId: number
      body: gen.NssolPlatypusApiModelsAccountApiModelsSwitchTenantInputModel
    }) =>
      accountApi.apiV2AccountTenantsTenantIdTokenPost({
        tenantId: params.tenantId,
        nssolPlatypusApiModelsAccountApiModelsSwitchTenantInputModel:
          params.body,
      }),
    getTreeMenus: () => accountApi.apiV2AccountAquariumMenusTreeGet(), //TODO 使用されていない？
    getListMenus: () => accountApi.apiV2AccountAquariumMenusListGet(), //TODO 使用されていない？
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

  aquariumDatasets: {
    get: (params: gen.AquariumDataSetApiApiV2AquariumDatasetsGetRequest) =>
      aquariumDataSetApi.apiV2AquariumDatasetsGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsAquariumDataSetApiModelsCreateInputModel
    }) =>
      aquariumDataSetApi.apiV2AquariumDatasetsPost({
        nssolPlatypusApiModelsAquariumDataSetApiModelsCreateInputModel:
          params.body,
      }),
    getByIdVersions: (
      params: gen.AquariumDataSetApiApiV2AquariumDatasetsIdVersionsGetRequest,
    ) => aquariumDataSetApi.apiV2AquariumDatasetsIdVersionsGet(params),
    postByIdVersions: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsAquariumDataSetApiModelsVersionCreateInputModel
    }) =>
      aquariumDataSetApi.apiV2AquariumDatasetsIdVersionsPost({
        id: params.id,
        nssolPlatypusApiModelsAquariumDataSetApiModelsVersionCreateInputModel:
          params.body,
      }),
    getByIdVersionsByVersionId: (
      params: gen.AquariumDataSetApiApiV2AquariumDatasetsIdVersionsVersionIdGetRequest,
    ) => aquariumDataSetApi.apiV2AquariumDatasetsIdVersionsVersionIdGet(params),
    delete: (
      params: gen.AquariumDataSetApiApiV2AquariumDatasetsIdDeleteRequest,
    ) => aquariumDataSetApi.apiV2AquariumDatasetsIdDelete(params),
    deleteVersion: (
      params: gen.AquariumDataSetApiApiV2AquariumDatasetsIdVersionsVersionIdDeleteRequest,
    ) =>
      aquariumDataSetApi.apiV2AquariumDatasetsIdVersionsVersionIdDelete(params),
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
    getById: (params: gen.DataSetApiApiV2DatasetsIdGetRequest) =>
      dataSetApi.apiV2DatasetsIdGet(params),
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
    //gitはほとんど使用していない？
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
    //↓これ以降は使用している
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

  training: {
    getSimple: (params: gen.TrainingApiApiV2TrainingGetRequest) =>
      trainingApi.apiV2TrainingGet(params),
    get: (params: gen.TrainingApiApiV2TrainingSearchGetRequest) =>
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
  },

  experiment: {
    get: (params: gen.ExperimentApiApiV2ExperimentGetRequest) =>
      experimentApi.apiV2ExperimentGet(params),
    post: (params: {
      body: gen.NssolPlatypusApiModelsExperimentApiModelsCreateInputModel
    }) =>
      experimentApi.apiV2ExperimentRunPost({
        nssolPlatypusApiModelsExperimentApiModelsCreateInputModel: params.body,
      }),
    getById: (params: gen.ExperimentApiApiV2ExperimentIdGetRequest) =>
      experimentApi.apiV2ExperimentIdGet(params),
    postPreprocessingCompleteById: (
      params: gen.ExperimentApiApiV2ExperimentIdPreprocessingCompletePostRequest,
    ) => experimentApi.apiV2ExperimentIdPreprocessingCompletePost(params),
    deleteById: (params: gen.ExperimentApiApiV2ExperimentIdDeleteRequest) =>
      experimentApi.apiV2ExperimentIdDelete(params),
    getEvaluationsById: (
      params: gen.ExperimentApiApiV2ExperimentIdEvaluationsGetRequest,
    ) => experimentApi.apiV2ExperimentIdEvaluationsGet(params),
    postEvaluationsById: (params: {
      id: number
      body: gen.NssolPlatypusApiModelsExperimentApiModelsEvaluationCreateInputModel
    }) =>
      experimentApi.apiV2ExperimentIdEvaluationsPost({
        id: params.id,
        nssolPlatypusApiModelsExperimentApiModelsEvaluationCreateInputModel:
          params.body,
      }),
    deleteByIdEvaluationsByEvaluationId: (
      params: gen.ExperimentApiApiV2ExperimentIdEvaluationsEvaluationIdDeleteRequest,
    ) => experimentApi.apiV2ExperimentIdEvaluationsEvaluationIdDelete(params),
  },

  storage: {
    //使用していない？
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
