import * as gen from './api.generator.js'
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
    getPartitions: () => gen.getApiV2TenantPartitions() as { data: [string] },
    getQuota: () =>
      gen.getApiV2TenantQuota() as {
        data: {
          tenantId: number
          cpu: number
          memory: number
          gpu: number
          tenantName: string
        }
      },
    getTenantNodes: () =>
      gen.getApiV2TenantNodes() as {
        data: [
          {
            name: string
            memo: string
            partition: string
            accessLevel: number
            allocatableCpu: number
            allocatableMemory: number
            allocatableGpu: number
          },
        ]
      },

    admin: {
      getQuotas: () =>
        gen.getApiV2AdminQuotas() as {
          data: [
            {
              tenantId: number
              cpu: number
              memory: number
              gpu: number
              tenantName: string
            },
          ]
        },
      postQuota: (
        params: [
          {
            tenantId: number
            cpu: number
            memory: number
            gpu: number
          },
        ],
      ) =>
        gen.postApiV2AdminQuotas(params) as {
          data: [
            {
              tenantId: number
              cpu: number
              memory: number
              gpu: number
              tenantName: string
            },
          ]
        },
      deleteTensorboards: gen.deleteApiV2AdminTensorboards as number,
    },
  },

  menu: {
    admin: {
      get: () =>
        gen.getApiV2AdminMenus() as {
          data: [
            {
              id: 0
              name: string
              description: string
              menuType: 0
              roles: [
                {
                  id: 0
                  name: string
                  isSystemRole: true
                },
              ]
            },
          ]
        },
      put: (params: { id: number }) => gen.putApiV2AdminMenusById(params),
      getTypes: () =>
        gen.getApiV2AdminMenuTypes() as {
          data: [
            {
              id: number
              name: string
            },
          ]
        },
    },

    tenant: {
      get: gen.getApiV2TenantMenus,
      put: gen.putApiV2TenantMenusById,
      getTypes: gen.getApiV2TenantMenuTypes,
    },
  },

  menuList: {
    getMenuList: () =>
      gen.getApiV2AccountMenusList() as {
        data: [
          {
            name: 'string'
            description: 'string'
            url: 'string'
            category: 'string'
          },
        ]
      },
  },

  quotas: {
    get: () =>
      gen.getApiV2AdminQuotas() as {
        data: [
          {
            tenantId: number
            cpu: number
            memory: number
            gpu: number
            tenantName: string
          },
        ]
      },
    post: (
      params: [
        {
          tenantId: number
          cpu: number
          memory: number
          gpu: number
        },
      ],
    ) =>
      gen.postApiV2AdminQuotas(params) as {
        data: [
          {
            tenantId: number
            cpu: number
            memory: number
            gpu: number
            tenantName: string
          },
        ]
      },
  },

  nodes: {
    admin: {
      get: (params: {
        name?: string
        perPage?: number
        page?: number
        withTotal?: boolean
      }) =>
        gen.getApiV2AdminNodes(params) as {
          headers: { 'x-total-count': string }
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              memo: string
              partition: string
              accessLevel: number
              accessLevelStr: string
              tensorBoardEnabled: true
              notebookEnabled: true
            },
          ]
        },
      post: (params: {
        name: string
        memo: string
        partition: string
        accessLevel: number
        assignedTenantIds: [number]
        tensorBoardEnabled: true
        notebookEnabled: true
      }) =>
        gen.postApiV2AdminNodes(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            memo: string
            partition: string
            accessLevel: number
            accessLevelStr: string
            tensorBoardEnabled: true
            notebookEnabled: true
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminNodesById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            memo: string
            partition: string
            accessLevel: number
            accessLevelStr: string
            tensorBoardEnabled: true
            notebookEnabled: true
            assignedTenants: [
              {
                id: number
                name: string
                displayName: string
              },
            ]
          }
        },
      put: (params: { id: number }) =>
        gen.putApiV2AdminNodesById(params) as {
          data: {
            name: string
            memo: string
            partition: string
            accessLevel: number
            assignedTenantIds: [number]
            tensorBoardEnabled: true
            notebookEnabled: true
          }
        },
      delete: (params: { id: number }) => gen.deleteApiV2AdminNodesById(params),
      postSyncFromDb: () =>
        gen.postApiV2AdminNodesSyncClusterFromDb() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              memo: string
              partition: string
              accessLevel: number
              accessLevelStr: string
              tensorBoardEnabled: true
              notebookEnabled: true
            },
          ]
        },
      getAccessLevel: () =>
        gen.getApiV2AdminNodeAccessLevels() as {
          data: [
            {
              id: number
              name: 'string'
            },
          ]
        },
    },
  },
  registry: {
    admin: {
      get: () =>
        gen.getApiV2AdminRegistryEndpoints() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              registryPath: string
              projectName: string
              serviceType: number
            },
          ]
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminRegistryEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            registryPath: string
            projectName: string
            serviceType: number
            host: string
            portNo: number
            apiUrl: string
            registryUrl: string
            isNotEditable: true
          }
        },
      getType: () =>
        gen.getApiV2AdminRegistryTypes() as {
          data: [
            {
              id: number
              name: string
            },
          ]
        },
      post: (params: {
        body: {
          name: string
          host: string
          portNo: number
          serviceType: number
          projectName: string
          apiUrl: string
          registryUrl: string
        }
      }) =>
        gen.postApiV2AdminRegistryEndpoints(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            registryPath: string
            projectName: string
            serviceType: number
          }
        },
      putById: (params: {
        id: number
        body: {
          name: string
          host: string
          portNo: number
          serviceType: number
          projectName: string
          apiUrl: string
          registryUrl: string
        }
      }) =>
        gen.putApiV2AdminRegistryEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            registryPath: string
            projectName: string
            serviceType: number
          }
        },
      deleteById: (params: { id: number }) =>
        gen.deleteApiV2AdminRegistryEndpointsById(params),
    },
    tenant: {
      getEndpoints: () =>
        gen.getApiV2TenantRegistryEndpoints() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              registryPath: string
              projectName: string
              serviceType: number
            },
          ]
        },
    },
    getImages: (params: { registryId: number }) =>
      gen.getApiV2RegistriesByRegistryIdImages(params) as { data: [string] },
    getTags: (params: { registryId: number; image: string }) =>
      gen.getApiV2RegistriesByRegistryIdImagesByImageTags(params) as {
        data: [string]
      },
  },

  account: {
    get: () =>
      gen.getApiV2Account() as {
        data: {
          userId: number
          userName: string
          passwordChangeEnabled: true
          selectedTenant: {
            id: number
            name: string
            default: true
            displayName: string
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: true
                sortOrder: number
                isOrigin: true
                userGroupTanantMapIdLists: [number]
              },
            ]
            isOrigin: true
          }
          defaultTenant: {
            id: number
            name: string
            default: true
            displayName: string
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: true
                sortOrder: number
                isOrigin: true
                userGroupTanantMapIdLists: [number]
              },
            ]
            isOrigin: true
          }
          tenants: [
            {
              id: number
              name: string
              default: true
              displayName: string
              roles: [
                {
                  id: number
                  name: string
                  displayName: string
                  isCustomed: true
                  sortOrder: number
                  isOrigin: true
                  userGroupTanantMapIdLists: [number]
                },
              ]
              isOrigin: true
            },
          ]
        }
      },
    put: (params: { DefaultTenant: string }) =>
      gen.putApiV2Account(params) as {
        data: {
          userId: number
          userName: string
          passwordChangeEnabled: true
          selectedTenant: {
            id: number
            name: string
            default: true
            displayName: string
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: true
                sortOrder: number
                isOrigin: true
                userGroupTanantMapIdLists: [number]
              },
            ]
            isOrigin: true
          }
          defaultTenant: {
            id: number
            name: string
            default: true
            displayName: string
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: true
                sortOrder: number
                isOrigin: true
                userGroupTanantMapIdLists: [number]
              },
            ]
            isOrigin: true
          }
          tenants: [
            {
              id: number
              name: string
              default: true
              displayName: string
              roles: [
                {
                  id: number
                  name: string
                  displayName: string
                  isCustomed: true
                  sortOrder: number
                  isOrigin: true
                  userGroupTanantMapIdLists: [number]
                },
              ]
              isOrigin: true
            },
          ]
        }
      },
    putPassword: (params: { currentPassword: string; newPassword: string }) =>
      gen.putApiV2AccountPassword(params),
    postLogin: (params: {
      userName: string
      password: string
      tenantId: number
      expiresIn: number
    }) =>
      gen.postApiV2AccountLogin(params) as {
        data: {
          token: string
          userName: string
          tenantId: number
          tenantName: string
          expiresIn: number
        }
      },
    postTokenTenants: (params: {
      tenantId: number
      body: { expiresIn: null }
    }) =>
      gen.postApiV2AccountTenantsByTenantIdToken(params) as {
        data: {
          token: string
          userName: string
          tenantId: number
          tenantName: string
          expiresIn: number
        }
      },
    getTreeMenus: () =>
      gen.getApiV2AccountMenusTree() as {
        data: [
          {
            label: string
            category: string
            url: string
            children: [
              {
                label: string
                category: string
                url: string
              },
            ]
          },
        ]
      },
    getListMenus: () =>
      gen.getApiV2AccountMenusList() as {
        data: [
          {
            name: string
            description: string
            url: string
            category: string
          },
        ]
      },
    getRegistries: () =>
      gen.getApiV2AccountRegistries() as {
        data: {
          defaultRegistryId: number
          registries: [
            {
              id: number
              userName: string
              password: string
              name: string
              projectName: string
              serviceType: number
            },
          ]
        }
      },
    putRegistries: (params: {
      id: number
      userName: string
      password: string
    }) =>
      gen.putApiV2AccountRegistries(params) as {
        data: {
          id: number
          userName: string
          password: string
          name: string
          projectName: string
          serviceType: number
        }
      },
    getGits: () =>
      gen.getApiV2AccountGits() as {
        data: {
          defaultGitId: number
          gits: [
            {
              id: number
              token: string
              name: string
              serviceType: number
            },
          ]
        }
      },
    putGits: (params: {
      body: {
        id: number
        token: string
      }
    }) =>
      gen.putApiV2AccountGits(params) as {
        data: {
          id: number
          token: string
          name: string
          serviceType: number
        }
      },
    getWebhookSlack: () =>
      gen.getApiV2AccountWebhookSlack() as {
        data: {
          slackUrl: string
          mention: string
        }
      },
    putWebhookSlack: (params: {
      body: {
        slackUrl: string
        mention: string
      }
    }) => gen.putApiV2AccountWebhookSlack(params),
    postWebhookSlackTest: (params: {
      body: {
        slackUrl: string
        mention: string
      }
    }) => gen.postApiV2AccountWebhookSlackTest(params),
  },

  role: {
    admin: {
      get: () =>
        gen.getApiV2AdminRoles() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              displayName: string
              isSystemRole: true
              tenantId: number
              sortOrder: number
            },
          ]
        },
      getTenantCommonRoles: () =>
        gen.getApiV2AdminTenantCommonRoles as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              displayName: string
              isSystemRole: true
              tenantId: number
              sortOrder: number
            },
          ]
        },
      post: (params: {
        body: {
          name: string
          displayName: string
          sortOrder: number
          isSystemRole: true
          tenantId: number
        }
      }) =>
        gen.postApiV2AdminRoles(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            isSystemRole: true
            tenantId: number
            sortOrder: number
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminRolesById() as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            isSystemRole: true
            tenantId: number
            sortOrder: number
            tenantName: string
            isNotEditable: true
          }
        },
      put: (params: {
        id: number
        body: {
          name: string
          displayName: string
          sortOrder: number
          isSystemRole: true
          tenantId: number
        }
      }) =>
        gen.putApiV2AdminRolesById(params) as {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          displayName: string
          isSystemRole: true
          tenantId: number
          sortOrder: number
        },
      delete: (params: { id: number }) => gen.deleteApiV2AdminRolesById(params),
    },
    tenant: {
      get: () =>
        gen.getApiV2TenantRoles() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              displayName: string
              isSystemRole: true
              tenantId: number
              sortOrder: number
            },
          ]
        },
      post: (params: {
        body: {
          name: string
          displayName: string
          sortOrder: number
        }
      }) =>
        gen.postApiV2TenantRoles(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            isSystemRole: true
            tenantId: number
            sortOrder: number
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2TenantRolesById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            isSystemRole: true
            tenantId: number
            sortOrder: number
            tenantName: string
            isNotEditable: true
          }
        },
      put: (params: {
        id: number
        body: {
          name: string
          displayName: string
          sortOrder: number
        }
      }) =>
        gen.putApiV2TenantRolesById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            isSystemRole: true
            tenantId: number
            sortOrder: number
          }
        },
      delete: (params: { id: number }) =>
        gen.deleteApiV2TenantRolesById(params),
    },
  },

  data: {
    get: (params: {
      perPage: number
      page: number
      withTotal: boolean
      id?: string
      name?: string
      memo?: string
      createdAt?: string
      createdBy?: string
      tag?: string[]
    }) =>
      gen.getApiV2Data(params) as {
        headers: { 'x-total-count': string }
        data: [
          {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            displayId: number
            name: string
            memo: string
            isRaw: boolean
            parentDataName: string
            parentDataId: number
            tags: ['string']
          },
        ]
      },
    post: (params: {
      body: { name: string; memo?: string; tags?: [string] }
    }) => gen.postApiV2Data(params),
    getById: (params: { id: number }) =>
      gen.getApiV2DataById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isRaw: true
          parentDataName: string
          parentDataId: number
          tags: [string]
          fileNames: [string]
          parent: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            displayId: number
            name: string
            memo: string
            isRaw: true
            parentDataName: string
            parentDataId: number
            tags: [string]
          }
          children: [
            {
              id: number
              inputDataId: number
              inputDataName: string
              preprocessId: number
              preprocessName: string
            },
          ]
        }
      },
    putById: (params: { name: string; memo?: string; tags?: [string] }) =>
      gen.putApiV2DataById(params) as {
        createdBy: string
        createdAt: string
        modifiedBy: string
        modifiedAt: string
        id: number
        displayId: number
        name: string
        memo: string
        isRaw: true
        parentDataName: string
        parentDataId: number
        tags: [string]
      },
    deleteById: (params: { id: number }) => gen.deleteApiV2DataById(params),
    getFilesByKey: (params: { id: number; name: string }) =>
      gen.getApiV2DataByIdFilesByName(params) as {
        data: {
          id: number
          fileId: number
          key: string
          url: string
          fileName: string
          fileSize: number
        }
      },
    getFilesById: (params: { id: number; withUrl?: boolean }) =>
      gen.getApiV2DataByIdFiles(params) as {
        data: [
          {
            id: number
            fileId: number
            key: string
            url: string
            fileName: string
            fileSize: number
          },
        ]
      },
    putFilesById: (params: {
      id: number
      body: {
        files: [
          {
            fileName: string
            storedPath: string
          },
        ]
      }
    }) =>
      gen.postApiV2DataByIdFiles(params) as {
        id: number
        files: [
          {
            id: number
            fileId: number
            key: string
            url: string
            fileName: string
            fileSize: number
          },
        ]
      },
    deleteFilesById: (params: { id: number; fileId: string }) =>
      gen.deleteApiV2DataByIdFilesByFileId(params),
    getDataTags: () => gen.getApiV2DataDatatags() as { data: [string] },
    getFileSize: (params: { id: number; name: string }) =>
      gen.getApiV2DataByIdFilesByNameSize(params) as {
        data: {
          id: number
          fileId: number
          key: string
          url: string
          fileName: string
          fileSize: number
        }
      },
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
    getSearch: gen.getApiV2TrainingSearch,
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
    postTags: gen.postApiV2TrainingTags,
    deleteTags: gen.deleteApiV2TrainingTags,
    getSearchHistory: gen.getApiV2TrainingSearchHistory,
    postSearchHistory: gen.postApiV2TrainingSearchHistory,
    deleteSearchHistoryById: gen.deleteApiV2TrainingSearchHistoryById,
    getSearchFill: gen.getApiV2TrainingSearchFill,
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
      postSyncLdap: gen.postApiV2AdminUsersSyncLdap,
    },

    tenant: {
      get: gen.getApiV2TenantUsers,
      getById: gen.getApiV2TenantUsersById,
      delete: gen.deleteApiV2TenantUsersById,
      putRoles: gen.putApiV2TenantUsersByIdRoles,
    },
  },

  userGroup: {
    admin: {
      get: gen.getApiV2AdminUsergroup,
      getById: gen.getApiV2AdminUsergroupById,
      post: gen.postApiV2AdminUsergroup,
      put: gen.putApiV2AdminUsergroupById,
      delete: gen.deleteApiV2AdminUsergroupById,
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
