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
// 使いやすいようにAPI領域で再定義
// （swagger-vue で自動生成生成：https://github.com/chenweiqun/swagger-vue）
let api = {
  cluster: {
    getPartitions: gen.ApiV2TenantPartitionsGet,
    getQuota: gen.ApiV2TenantQuotaGet,
    getTenantNodes: gen.ApiV2TenantNodesGet,

    admin: {
      getQuotas: gen.ApiV2AdminQuotasGet,
      postQuota: gen.ApiV2AdminQuotasPost,
      deleteTensorboards: gen.ApiV2AdminTensorboardsDelete,
    },
  },

  menuList: {
    getMenuList: gen.ApiV2AccountAquariumMenusListGet,
  },

  templates: {
    admin: {
      getById: gen.ApiV2AdminTemplatesByIdGet,
      get: gen.ApiV2TemplatesGet,
      post: gen.ApiV2AdminTemplatesPost,
      put: gen.ApiV2AdminTemplatesByIdPut,
      delete: gen.ApiV2AdminTemplatesByIdDelete,
      deleteVersion: gen.ApiV2AdminTemplatesByIdVersionsByVersionIdDelete,
      getByIdVersions: gen.ApiV2AdminTemplatesByIdVersionsGet,
      postByIdVersions: gen.ApiV2AdminTemplatesByIdVersionsPost,
      getByIdVersionsByVersionId:
        gen.ApiV2AdminTemplatesByIdVersionsByVersionIdGet,
    },
    getTenantTemplate: gen.ApiV2TenantTemplatesGet,
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
    getTreeMenus: gen.ApiV2AccountAquariumMenusTreeGet,
    getListMenus: gen.ApiV2AccountAquariumMenusListGet,
    getRegistries: gen.ApiV2AccountRegistriesGet,
    putRegistries: gen.ApiV2AccountRegistriesPut,
    getGits: gen.ApiV2AccountGitsGet,
    putGits: gen.ApiV2AccountGitsPut,
    getWebhookSlack: gen.ApiV2AccountWebhookSlackGet,
    putWebhookSlack: gen.ApiV2AccountWebhookSlackPut,
    postWebhookSlackTest: gen.ApiV2AccountWebhookSlackTestPost,
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
    getFileSize: gen.ApiV2DataByIdFilesByNameSizeGet,
  },

  aquariumDatasets: {
    get: gen.ApiV2AquariumDatasetsGet,
    post: gen.ApiV2AquariumDatasetsPost,
    getByIdVersions: gen.ApiV2AquariumDatasetsByIdVersionsGet,
    postByIdVersions: gen.ApiV2AquariumDatasetsByIdVersionsPost,
    getByIdVersionsByVersionId:
      gen.ApiV2AquariumDatasetsByIdVersionsByVersionIdGet,
    delete: gen.ApiV2AquariumDatasetsByIdDelete,
    deleteVersion: gen.ApiV2AquariumDatasetsByIdVersionsByVersionIdDelete,
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

  training: {
    getSimple: gen.ApiV2TrainingSimpleGet,
    get: gen.ApiV2TrainingGet,
    post: gen.ApiV2TrainingRunPost,
    getById: gen.ApiV2TrainingByIdGet,
    deleteById: gen.ApiV2TrainingByIdDelete,
    putById: gen.ApiV2TrainingByIdPut,
    // GET /spa/trains/{id}/log
    getFilesById: gen.ApiV2TrainingByIdFilesGet,
    getFileSize: gen.ApiV2TrainingByIdFilesByNameSizeGet,
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

  experiment: {
    get: gen.ApiV2ExperimentGet,
    post: gen.ApiV2ExperimentRunPost,
    getById: gen.ApiV2ExperimentByIdGet,
    postPreprocessingCompleteById:
      gen.ApiV2ExperimentByIdPreprocessingCompletePost,
    deleteById: gen.ApiV2ExperimentByIdDelete,
    getEvaluationsById: gen.ApiV2ExperimentByIdEvaluationsGet,
    postEvaluationsById: gen.ApiV2ExperimentByIdEvaluationsPost,
    deleteByIdEvaluationsByEvaluationId:
      gen.ApiV2ExperimentByIdEvaluationsByEvaluationIdDelete,
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
