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
    getPartitions: gen.getApiV2TenantPartitions,
    getQuota: gen.getApiV2TenantQuota,
    getTenantNodes: gen.getApiV2TenantNodes,

    admin: {
      getQuotas: gen.getApiV2AdminQuotas,
      postQuota: gen.postApiV2AdminQuotas,
      deleteTensorboards: gen.deleteApiV2AdminTensorboards,
    },
  },

  menuList: {
    getMenuList: gen.getApiV2AccountAquariumMenusList,
  },

  templates: {
    admin: {
      getById: gen.getApiV2AdminTemplatesById,
      get: gen.getApiV2Templates,
      post: gen.postApiV2AdminTemplates,
      put: gen.putApiV2AdminTemplatesById,
      delete: gen.deleteApiV2AdminTemplatesById,
      deleteVersion: gen.deleteApiV2AdminTemplatesByIdVersionsByVersionId,
      getByIdVersions: gen.getApiV2AdminTemplatesByIdVersions,
      postByIdVersions: gen.postApiV2AdminTemplatesByIdVersions,
      getByIdVersionsByVersionId:
        gen.getApiV2AdminTemplatesByIdVersionsByVersionId,
    },
    getTenantTemplate: gen.getApiV2TenantTemplates,
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
    getTreeMenus: gen.getApiV2AccountAquariumMenusTree,
    getListMenus: gen.getApiV2AccountAquariumMenusList,
    getRegistries: gen.getApiV2AccountRegistries,
    putRegistries: gen.putApiV2AccountRegistries,
    getGits: gen.getApiV2AccountGits,
    putGits: gen.putApiV2AccountGits,
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

  aquariumDatasets: {
    get: gen.getApiV2AquariumDatasets,
    post: gen.postApiV2AquariumDatasets,
    getByIdVersions: gen.getApiV2AquariumDatasetsByIdVersions,
    postByIdVersions: gen.postApiV2AquariumDatasetsByIdVersions,
    getByIdVersionsByVersionId:
      gen.getApiV2AquariumDatasetsByIdVersionsByVersionId,
    delete: gen.deleteApiV2AquariumDatasetsById,
    deleteVersion: gen.deleteApiV2AquariumDatasetsByIdVersionsByVersionId,
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

  experiment: {
    get: gen.getApiV2Experiment,
    post: gen.postApiV2ExperimentRun,
    getById: gen.getApiV2ExperimentById,
    postPreprocessingCompleteById:
      gen.postApiV2ExperimentByIdPreprocessingComplete,
    deleteById: gen.deleteApiV2ExperimentById,
    getEvaluationsById: gen.getApiV2ExperimentByIdEvaluations,
    postEvaluationsById: gen.postApiV2ExperimentByIdEvaluations,
    deleteByIdEvaluationsByEvaluationId:
      gen.deleteApiV2ExperimentByIdEvaluationsByEvaluationId,
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
