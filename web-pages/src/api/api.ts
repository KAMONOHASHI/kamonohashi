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
      postQuota: (params: {
        body: [
          {
            tenantId: number
            cpu: number
            memory: number
            gpu: number
          },
        ]
      }) =>
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
    post: (params: {
      body: [
        {
          tenantId: number
          cpu: number
          memory: number
          gpu: number
        },
      ]
    }) =>
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
        body: {
          name: string
          memo: string
          partition: string
          accessLevel: number
          assignedTenantIds: [number]
          tensorBoardEnabled: true
          notebookEnabled: true
        }
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
      put: (params: {
        id: number
        body: {
          name: string
          memo: string
          partition: string
          accessLevel: number
          assignedTenantIds: [number]
          tensorBoardEnabled: boolean
          notebookEnabled: boolean
        }
      }) =>
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
        gen.getApiV2AdminRolesById(params) as {
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
    get: (params: {
      id?: string
      name?: string
      memo?: string
      createdAt?: string
      perPage?: number
      page?: number
      withTotal?: boolean
    }) =>
      gen.getApiV2Datasets(params) as {
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
            isFlat: true
          },
        ]
      },
    post: (params: {
      body: {
        name: string
        memo: string
        isFlat: boolean
        entries: {
          additionalProp1: [
            {
              id: number
            },
          ]
          additionalProp2: [
            {
              id: number
            },
          ]
          additionalProp3: [
            {
              id: number
            },
          ]
        }
        flatEntries: [
          {
            id: number
          },
        ]
      }
    }) =>
      gen.postApiV2Datasets(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isFlat: boolean
        }
      },
    getById: (params: { id: number }) =>
      gen.getApiV2DatasetsById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isFlat: boolean
          entries: {
            additionalProp1: [
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
                tags: [string]
              },
            ]
            additionalProp2: [
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
                tags: [string]
              },
            ]
            additionalProp3: [
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
                tags: [string]
              },
            ]
          }
          flatEntries: [
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
              tags: [string]
            },
          ]
          isLocked: boolean
        }
      },
    put: (params: {
      id: number
      body: {
        name: string
        memo: string
        entries: {
          additionalProp1: [
            {
              id: number
            },
          ]
          additionalProp2: [
            {
              id: number
            },
          ]
          additionalProp3: [
            {
              id: number
            },
          ]
        }
        flatEntries: [
          {
            id: number
          },
        ]
      }
    }) =>
      gen.putApiV2DatasetsById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isFlat: boolean
        }
      },
    delete: (params: { id: number }) => gen.deleteApiV2DatasetsById(params),
    patch: (params: { id: number; body: { name: string; memo: string } }) =>
      gen.patchApiV2DatasetsById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isFlat: boolean
        }
      },
    getFiles: (params: { id: number; withUrl?: boolean }) =>
      gen.getApiV2DataByIdFiles(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          isFlat: boolean
          entries: [
            {
              type: string
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
          ]
          flatEntries: [
            {
              id: number
              fileId: number
              key: string
              url: string
              fileName: string
              fileSize: number
            },
          ]
        }
      },
    getDatatypes: () =>
      gen.getApiV2Datatypes() as {
        data: [
          {
            id: number
            name: string
          },
        ]
      },
  },

  git: {
    admin: {
      getEndpoints: () =>
        gen.getApiV2AdminGitEndpoints() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              serviceType: number
              repositoryUrl: string
              apiUrl: string
              serviceTypeName: string
            },
          ]
        },
      postEndpoint: (params: {
        body: {
          name: string
          serviceType: number
          apiUrl: string
          repositoryUrl: string
        }
      }) =>
        gen.postApiV2AdminGitEndpoints(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            repositoryUrl: string
            apiUrl: string
            serviceTypeName: string
          }
        },
      putEndpoint: (params: {
        id: number
        body: {
          name: string
          serviceType: number
          apiUrl: string
          repositoryUrl: string
        }
      }) =>
        gen.putApiV2AdminGitEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            repositoryUrl: string
            apiUrl: string
            serviceTypeName: string
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminGitEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            repositoryUrl: string
            apiUrl: string
            serviceTypeName: string
            isNotEditable: boolean
          }
        },
      deleteById: (params: { id: number }) =>
        gen.deleteApiV2AdminGitEndpointsById(params),
      getTypes: () =>
        gen.getApiV2AdminGitTypes() as {
          data: [
            {
              id: number
              name: string
            },
          ]
        },
    },
    tenant: {
      getEndpoints: () =>
        gen.getApiV2TenantGitEndpoints() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              serviceType: number
              repositoryUrl: string
              apiUrl: string
              serviceTypeName: string
            },
          ]
        },
    },
    getRepos: (params: { gitId: number }) =>
      gen.getApiV2GitByGitIdRepos(params) as {
        data: [
          {
            owner: string
            name: string
            fullName: string
          },
        ]
      },
    getBranches: (params: {
      gitId: number
      owner: string
      repositoryName: string
    }) =>
      gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameBranches(params) as {
        data: [
          {
            branchName: string
            commitId: string
          },
        ]
      },
    getCommits: (params: {
      gitId: number
      owner: string
      repositoryName: string
      branch?: string
      page?: string
    }) =>
      gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommits(params) as {
        data: [
          {
            commitId: string
            committerName: string
            commitAt: string
            comment: string
            display: string
          },
        ]
      },
    getCommit: (params: {
      gitId: number
      owner: string
      repositoryName: string
      commitId: string
    }) =>
      gen.getApiV2GitByGitIdReposByOwnerByRepositoryNameCommitsByCommitId(
        params,
      ) as {
        data: {
          commitId: string
          committerName: string
          commitAt: string
          comment: string
          display: string
        }
      },
    // GET /spa/git/repos/{segments}
  },

  preprocessings: {
    get: (params: {
      id?: string
      name?: string
      createdAt?: string
      memo?: string
      perPage?: number
      page?: number
      withTotal?: boolean
    }) =>
      gen.getApiV2Preprocessings(params) as {
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
            cpu: number
            memory: number
            gpu: number
          },
        ]
      },
    post: (params: {
      body: {
        name: string
        entryPoint: string
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        memo: string
        cpu: number
        memory: number
        gpu: number
      }
    }) =>
      gen.postApiV2Preprocessings(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          memo: string
          cpu: number
          memory: number
          gpu: number
        }
      },
    getById: (params: { id: number }) =>
      gen.getApiV2PreprocessingsById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          memo: string
          cpu: number
          memory: number
          gpu: number
          gitModel: {
            gitId: number
            repository: string
            owner: string
            branch: string
            commitId: string
            url: string
          }
          containerImage: {
            registryId: number
            image: string
            tag: string
            registryName: string
            url: string
          }
          entryPoint: string
          isLocked: boolean
        }
      },
    put: (params: {
      id: number
      body: {
        name: string
        entryPoint: string
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        memo: string
        cpu: number
        memory: number
        gpu: number
      }
    }) =>
      gen.putApiV2PreprocessingsById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          memo: string
          cpu: number
          memory: number
          gpu: number
        }
      },
    delete: (params: { id: number }) =>
      gen.deleteApiV2PreprocessingsById(params),
    patch: (params: {
      id: number
      body: {
        name: string
        memo: string
        cpu: number
        memory: number
        gpu: number
      }
    }) =>
      gen.patchApiV2PreprocessingsById(params) as {
        data: {
          name: string
          memo: string
          cpu: number
          memory: number
          gpu: number
        }
      },
    getFilesById: (params: { id: number; dataId: number; withUrl?: boolean }) =>
      gen.getApiV2PreprocessingsByIdHistoriesByDataIdFiles(params) as {
        data: {
          id: number
          fileId: number
          url: string
          fileName: string
          isLocked: boolean
        }
      },
    getHistory: (params: { id: number }) =>
      gen.getApiV2PreprocessingsByIdHistories(params) as {
        data: [
          {
            key: string
            status: string
            statusType: string
            createdAt: string
            dataId: number
            dataName: string
            preprocessId: number
            preprocessName: string
          },
        ]
      },
    getHistroyById: (params: { id: number; dataId: number }) =>
      gen.getApiV2PreprocessingsByIdHistoriesByDataId(params) as {
        data: {
          key: string
          status: string
          statusType: string
          createdAt: string
          dataId: number
          dataName: string
          preprocessId: number
          preprocessName: string
          outputDataIds: [number]
        }
      },
    deleteHistroyById: (params: { id: number; dataId: number }) =>
      gen.deleteApiV2PreprocessingsByIdHistoriesByDataId(params),
    getEventsById: (params: { id: number; dataId: number }) =>
      gen.getApiV2PreprocessingsByIdHistoriesByDataIdEvents(params) as {
        data: {
          key: string
          status: string
          statusType: string
          createdAt: string
          dataId: number
          dataName: string
          preprocessId: number
          preprocessName: string
        }
      },
    runById: (params: {
      id: number
      body: {
        dataId: number
        options: {
          additionalProp1: string
          additionalProp2: string
          additionalProp3: string
        }
        cpu: number
        memory: number
        gpu: number
        partition: string
      }
    }) =>
      gen.postApiV2PreprocessingsByIdRun(params) as {
        data: {
          key: string
          status: string
          statusType: string
          createdAt: string
          dataId: number
          dataName: string
          preprocessId: number
          preprocessName: string
        }
      },
  },

  resource: {
    admin: {
      getNodes: () =>
        gen.getApiV2AdminResourceNodes() as {
          data: [
            {
              name: string
              memo: string
              partition: string
              accessLevel: number
              tensorBoardEnabled: boolean
              allocatableCpu: number
              allocatableMemory: number
              allocatableGpu: number
              assignedCpu: number
              assignedMemory: number
              assignedGpu: number
              cpuInfo: string
              memoryInfo: string
              gpuInfo: string
              containerResourceList: [
                {
                  name: string
                  createdBy: string
                  containerType: number
                  conditionNote: string
                  cpu: number
                  memory: number
                  gpu: number
                  statusType: string
                  status: string
                  nodeName: string
                  tenantId: number
                  tenantName: string
                  displayName: string
                },
              ]
            },
          ]
        },
      getTenants: () =>
        gen.getApiV2AdminResourceTenants() as {
          data: [
            {
              id: number
              name: string
              displayName: string
              allocatableCpu: number
              allocatableMemory: number
              allocatableGpu: number
              assignedCpu: number
              assignedMemory: number
              assignedGpu: number
              cpuInfo: string
              memoryInfo: string
              gpuInfo: string
              containerResourceList: [
                {
                  name: string
                  createdBy: string
                  containerType: number
                  conditionNote: string
                  cpu: number
                  memory: number
                  gpu: number
                  statusType: string
                  status: string
                  nodeName: string
                  tenantId: number
                  tenantName: string
                  displayName: string
                },
              ]
            },
          ]
        },
      getContainers: () =>
        gen.getApiV2AdminResourceContainers() as {
          data: [
            {
              name: string
              createdBy: string
              containerType: number
              conditionNote: string
              cpu: number
              memory: number
              gpu: number
              statusType: string
              status: string
              nodeName: string
              tenantId: number
              tenantName: string
              displayName: string
            },
          ]
        },
      getContainerByName: (params: { tenantId: number; name: string }) =>
        gen.getApiV2AdminResourceContainersByTenantIdByName(params) as {
          data: {
            name: string
            createdBy: string
            containerType: number
            conditionNote: string
            cpu: number
            memory: number
            gpu: number
            statusType: string
            status: string
            nodeName: string
            tenantId: number
            tenantName: string
            displayName: string
          }
        },
      deleteContainerByName: (params: { tenantId: number; name: string }) =>
        gen.deleteApiV2AdminResourceContainersByTenantIdByName(params),
      getContainerLogByName: (params: { tenantId: number; name: string }) =>
        gen.getApiV2AdminResourceContainersByTenantIdByNameLog(params),
      getContainerEventsByName: (params: { tenantId: number; name: string }) =>
        gen.getApiV2AdminResourceContainersByTenantIdByNameEvents(params) as {
          data: {
            canRead: boolean
            canSeek: boolean
            canTimeout: boolean
            canWrite: boolean
            length: number
            position: number
            readTimeout: number
            writeTimeout: number
          }
        },
      getHistoriesContainersMetadata: () =>
        gen.getApiV2AdminResourceHistoriesContainersMetadata() as {
          data: {
            count: number
            startDate: string
            endDate: string
          }
        },
      getHistoriesContainersData: (params: {
        startData?: string
        endDate?: string
        withHeader?: boolean
      }) => gen.getApiV2AdminResourceHistoriesContainersData(params),
      deleteHistoriesContainers: (params: { body: { endDate: string } }) =>
        gen.patchApiV2AdminResourceHistoriesContainers(params),
      getHistoriesJobsMetadata: () =>
        gen.getApiV2AdminResourceHistoriesJobsMetadata() as {
          data: {
            count: number
            startDate: string
            endDate: string
          }
        },
      getHistoriesJobsData: (params: {
        startDate?: string
        endDate?: string
        withHeader?: boolean
      }) => gen.getApiV2AdminResourceHistoriesJobsData(params),
      deleteHistoriesJobs: (params: { body: { endDate: string } }) =>
        gen.patchApiV2AdminResourceHistoriesJobs(params),
    },
    tenant: {
      getNodes: () =>
        gen.getApiV2TenantResourceNodes() as {
          data: [
            {
              name: string
              memo: string
              partition: string
              accessLevel: number
              tensorBoardEnabled: boolean
              allocatableCpu: number
              allocatableMemory: number
              allocatableGpu: number
              assignedCpu: number
              assignedMemory: number
              assignedGpu: number
              cpuInfo: string
              memoryInfo: string
              gpuInfo: string
              containerResourceList: [
                {
                  name: string
                  createdBy: string
                  containerType: number
                  conditionNote: string
                  cpu: number
                  memory: number
                  gpu: number
                  statusType: string
                  status: string
                  nodeName: string
                  tenantId: number
                  tenantName: string
                  displayName: string
                },
              ]
            },
          ]
        },
      getContainers: () =>
        gen.getApiV2TenantResourceContainers() as {
          data: [
            {
              name: string
              createdBy: string
              containerType: number
              conditionNote: string
              cpu: number
              memory: number
              gpu: number
              statusType: string
              status: string
              nodeName: string
            },
          ]
        },
      getContainerByName: (params: { name: string }) =>
        gen.getApiV2TenantResourceContainersByName(params) as {
          data: {
            name: string
            createdBy: string
            containerType: number
            conditionNote: string
            cpu: number
            memory: number
            gpu: number
            statusType: string
            status: string
            nodeName: string
          }
        },
      deleteContainerByName: (params: { name: string }) =>
        gen.deleteApiV2TenantResourceContainersByName(params),
      getContainerLogByName: (params: { name: string }) =>
        gen.getApiV2TenantResourceContainersByNameLog(params),
    },
  },

  training: {
    getSimple: () =>
      gen.getApiV2TrainingSimple() as {
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
            status: string
            favorite: boolean
            fullName: string
          },
        ]
      },
    get: (params: {
      id?: string
      name?: string
      parentId?: string
      parentName?: string
      startedAt?: string
      startedBy?: string
      dataSet?: string
      memo?: string
      status?: string
      entryPoint?: string
      tag?: [string]
      perPage?: number
      page?: number
      withTotal?: boolean
    }) =>
      gen.getApiV2Training(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            tags: [string]
          },
        ]
      },
    getSearch: (params: {
      idLower?: number
      idUpper?: number
      name?: string
      nameOr?: boolean
      parentName?: string
      parentNameOr?: boolean
      startedAtLower?: string
      startedAtUpper?: string
      startedBy?: string
      startedByOr?: boolean
      dataSet?: string
      dataSetOr?: boolean
      memo?: string
      memoOr?: boolean
      status?: string
      statusOr?: boolean
      entryPoint?: string
      entryPointOr?: boolean
      tags?: string
      tagsOr?: boolean
      perPage?: number
      page?: number
      withTotl?: boolean
    }) =>
      gen.getApiV2TrainingSearch(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            tags: [string]
          },
        ]
      },
    post: (params: {
      body: {
        name: string
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        dataSetId: number
        parentIds: [number]
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        entryPoint: string
        options: {
          additionalProp1: string
          additionalProp2: string
          additionalProp3: string
        }
        cpu: number
        memory: number
        gpu: number
        partition: string
        ports: [number]
        memo: string
        tags: [string]
        zip: boolean
        localDataSet: boolean
      }
    }) =>
      gen.postApiV2TrainingRun(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getById: (params: { id: number }) =>
      gen.getApiV2TrainingById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
          dataSet: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            displayId: number
            name: string
            memo: string
            isFlat: boolean
          }
          entryPoint: string
          parentFullNameList: [string]
          tags: [string]
          key: string
          gitModel: {
            gitId: number
            repository: string
            owner: string
            branch: string
            commitId: string
            url: string
          }
          options: [
            {
              key: string
              value: string
            },
          ]
          containerImage: {
            registryId: number
            image: string
            tag: string
            registryName: string
            url: string
          }
          parents: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              status: string
              favorite: boolean
              fullName: string
              dataSet: {
                createdBy: string
                createdAt: string
                modifiedBy: string
                modifiedAt: string
                id: number
                displayId: number
                name: string
                memo: string
                isFlat: boolean
              }
              entryPoint: string
              parentFullNameList: [string]
              tags: [string]
            },
          ]
          completedAt: string
          startedAt: string
          node: string
          logSummary: string
          cpu: number
          memory: number
          gpu: number
          partition: string
          ports: [number]
          nodePorts: [
            {
              key: string
              value: string
            },
          ]
          statusType: string
          conditionNote: string
          waitingTime: string
          executionTime: string
          zip: boolean
          localDataSet: boolean
        }
      },
    deleteById: (params: { id: number }) => gen.deleteApiV2TrainingById(params),
    putById: (params: {
      id: number
      body: {
        name: string
        memo: string
        favorite: boolean
        tags: [string]
      }
    }) =>
      gen.putApiV2TrainingById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    // GET /spa/trains/{id}/log
    getFilesById: (params: { id: number; withUrl?: boolean }) =>
      gen.getApiV2TrainingByIdFiles(params) as {
        data: [
          {
            id: number
            fileId: number
            url: string
            fileName: string
            isLocked: boolean
          },
        ]
      },
    getFileSize: (params: { id: number; name: string }) =>
      gen.getApiV2TrainingByIdFilesByNameSize(params) as {
        data: {
          id: number
          fileId: number
          key: string
          url: string
          fileName: string
          fileSize: number
        }
      },
    postFilesById: (params: {
      id: number
      body: {
        fileName: string
        storedPath: string
      }
    }) =>
      gen.postApiV2TrainingByIdFiles(params) as {
        data: {
          id: number
          fileId: number
          url: string
          fileName: string
          isLocked: boolean
        }
      },
    getContainerFilesById: (params: {
      id: number
      path?: string
      withUrl?: boolean
    }) =>
      gen.getApiV2TrainingByIdContainerFiles(params) as {
        data: {
          dirs: [
            {
              dirPath: string
              dirName: string
            },
          ]
          files: [
            {
              key: string
              fileName: string
              lastModified: string
              size: number
              url: string
            },
          ]
          exceeded: boolean
        }
      },
    deleteByIdFilesByFileId: (params: { id: number; fileId: number }) =>
      gen.deleteApiV2TrainingByIdFilesByFileId(params),
    getTensorboardById: (params: { id: number }) =>
      gen.getApiV2TrainingByIdTensorboard(params) as {
        data: {
          name: string
          status: string
          statusType: string
          nodePort: string
          remainingTime: string
          mountedTrainingHistoryIds: [number]
        }
      },
    putTensorboardById: (params: {
      id: number
      body: {
        expiresIn: number
        selectedHistoryIds: [number]
      }
    }) =>
      gen.putApiV2TrainingByIdTensorboard(params) as {
        data: {
          name: string
          status: string
          statusType: string
          nodePort: string
          remainingTime: string
          mountedTrainingHistoryIds: [number]
        }
      },
    deleteTensorboardById: (params: { id: number }) =>
      gen.deleteApiV2TrainingByIdTensorboard(params),
    postHaltById: (params: { id: number }) =>
      gen.postApiV2TrainingByIdHalt(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    postUserCancelById: (params: { id: number }) =>
      gen.postApiV2TrainingByIdUserCancel(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getEventsById: (params: { id: number }) =>
      gen.getApiV2TrainingByIdEvents(params) as {
        data: {
          tenantId: number
          tenantName: string
          containerName: string
          message: string
          details: string
          isError: boolean
          firstTimestamp: string
          lastTimestamp: string
        }
      },
    getMount: (params: { status: [string] }) =>
      gen.getApiV2TrainingMount(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            tags: [string]
          },
        ]
      },
    getTags: () => gen.getApiV2TrainingTags() as { data: [string] },
    postTags: (params: {
      body: {
        id: [number]
        tags: [string]
      }
    }) =>
      gen.postApiV2TrainingTags(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            tags: [string]
          },
        ]
      },
    deleteTags: (params: {
      body: {
        id: [number]
        tags: [string]
      }
    }) =>
      gen.deleteApiV2TrainingTags(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            tags: [string]
          },
        ]
      },
    getSearchHistory: () =>
      gen.getApiV2TrainingSearchHistory() as {
        data: [
          {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            name: string
            id: number
            searchDetail: {
              idLower: number
              idUpper: number
              name: string
              nameOr: boolean
              parentName: string
              parentNameOr: boolean
              startedAtLower: string
              startedAtUpper: string
              startedBy: string
              startedByOr: boolean
              dataSet: string
              dataSetOr: boolean
              memo: string
              memoOr: boolean
              status: string
              statusOr: boolean
              entryPoint: string
              entryPointOr: boolean
              tags: string
              tagsOr: boolean
            }
          },
        ]
      },
    postSearchHistory: (params: {
      body: {
        name: string
        searchDetailInputModel: {
          idLower: number
          idUpper: number
          name: string
          nameOr: boolean
          parentName: string
          parentNameOr: boolean
          startedAtLower: string
          startedAtUpper: string
          startedBy: string
          startedByOr: boolean
          dataSet: string
          dataSetOr: boolean
          memo: string
          memoOr: boolean
          status: string
          statusOr: boolean
          entryPoint: string
          entryPointOr: boolean
          tags: string
          tagsOr: boolean
        }
      }
    }) =>
      gen.postApiV2TrainingSearchHistory(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          name: string
          id: number
          searchDetail: {
            idLower: number
            idUpper: number
            name: string
            nameOr: boolean
            parentName: string
            parentNameOr: boolean
            startedAtLower: string
            startedAtUpper: string
            startedBy: string
            startedByOr: boolean
            dataSet: string
            dataSetOr: boolean
            memo: string
            memoOr: boolean
            status: string
            statusOr: boolean
            entryPoint: string
            entryPointOr: boolean
            tags: string
            tagsOr: boolean
          }
        }
      },
    deleteSearchHistoryById: (params: { id: number }) =>
      gen.deleteApiV2TrainingSearchHistoryById(params),
    getSearchFill: () =>
      gen.getApiV2TrainingSearchFill() as {
        data: {
          createdBy: [string]
          status: [string]
          tags: [string]
          datasets: [string]
        }
      },
  },

  notebook: {
    getSimple: () =>
      gen.getApiV2NotebookSimple() as {
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
            status: string
            favorite: boolean
            fullName: string
          },
        ]
      },
    get: (params: {
      id?: string
      name?: string
      createdAt?: string
      createdBy?: string
      memo?: string
      status?: string
      perPage?: number
      page?: number
      withTotal?: boolean
    }) =>
      gen.getApiV2Notebook(params) as {
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
            status: string
            favorite: boolean
            fullName: string
          },
        ]
      },
    post: (params: {
      body: {
        name: string
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        dataSetId: number
        jupyterLabVersion: string
        parentIds: [number]
        inferenceIds: [number]
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        options: {
          additionalProp1: string
          additionalProp2: string
          additionalProp3: string
        }
        cpu: number
        memory: number
        gpu: number
        partition: string
        memo: string
        expiresIn: number
        localDataSet: boolean
        entryPoint: string
      }
    }) =>
      gen.postApiV2NotebookRun(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getById: (params: { id: number }) =>
      gen.getApiV2NotebookById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
          key: string
          dataSet: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            displayId: number
            name: string
            memo: string
            isFlat: boolean
          }
          parents: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              status: string
              favorite: boolean
              fullName: string
              dataSet: {
                createdBy: string
                createdAt: string
                modifiedBy: string
                modifiedAt: string
                id: number
                displayId: number
                name: string
                memo: string
                isFlat: boolean
              }
              entryPoint: string
              parentFullNameList: [string]
              tags: [string]
            },
          ]
          inferences: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              status: string
              favorite: boolean
              fullName: string
              dataSet: {
                createdBy: string
                createdAt: string
                modifiedBy: string
                modifiedAt: string
                id: number
                displayId: number
                name: string
                memo: string
                isFlat: boolean
              }
              entryPoint: string
              parentFullNameList: [string]
              parentInferenceFullNameList: [string]
              outputValue: string
            },
          ]
          gitModel: {
            gitId: number
            repository: string
            owner: string
            branch: string
            commitId: string
            url: string
          }
          options: [
            {
              key: string
              value: string
            },
          ]
          containerImage: {
            registryId: number
            image: string
            tag: string
            registryName: string
            url: string
          }
          completedAt: string
          startedAt: string
          node: string
          cpu: number
          memory: number
          gpu: number
          partition: string
          jupyterLabVersion: string
          statusType: string
          notebookNodePort: string
          notebookToken: string
          conditionNote: string
          waitingTime: string
          executionTime: string
          expiresIn: number
          localDataSet: boolean
          entryPoint: string
        }
      },
    deleteById: (params: { id: number }) => gen.deleteApiV2NotebookById(params),
    putById: (params: {
      id: number
      body: {
        name: string
        memo: string
        favorite: boolean
      }
    }) =>
      gen.putApiV2NotebookById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getContainerFilesById: (params: {
      id: number
      path?: string
      withUrl?: boolean
    }) =>
      gen.getApiV2NotebookByIdContainerFiles(params) as {
        data: {
          dirs: [
            {
              dirPath: string
              dirName: string
            },
          ]
          files: [
            {
              key: string
              fileName: string
              lastModified: string
              size: number
              url: string
            },
          ]
          exceeded: boolean
        }
      },
    postHaltById: (params: { id: number }) =>
      gen.postApiV2NotebookByIdHalt(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getEventsById: (params: { id: number }) =>
      gen.getApiV2NotebookByIdEvents(params) as {
        data: {
          tenantId: number
          tenantName: string
          containerName: string
          message: string
          details: string
          isError: boolean
          firstTimestamp: string
          lastTimestamp: string
        }
      },
    getEndpointById: (params: { id: number }) =>
      gen.getApiV2NotebookByIdEndpoint(params) as {
        data: {
          nodePort: string
          token: string
        }
      },
    getFilesById: (params: { id: number; path?: string; withUrl?: boolean }) =>
      gen.getApiV2NotebookByIdContainerFiles(params) as {
        data: {
          dirs: [
            {
              dirPath: string
              dirName: string
            },
          ]
          files: [
            {
              key: string
              fileName: string
              lastModified: string
              size: number
              url: string
            },
          ]
          exceeded: boolean
        }
      },
    postRerun: (params: {
      id: number
      body: {
        dataSetId: number
        parentIds: [number]
        inferenceIds: [number]
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        jupyterLabVersion: string
        cpu: number
        memory: number
        gpu: number
        expiresIn: number
        localDataSet: boolean
        entryPoint: string
      }
    }) =>
      gen.postApiV2NotebookByIdRerun(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getAvailableInfiniteTime: () =>
      gen.getApiV2NotebookAvailableInfiniteTime() as { data: boolean },
  },

  inference: {
    getSimple: () =>
      gen.getApiV2InferencesSimple() as {
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
            status: string
            favorite: boolean
            fullName: string
          },
        ]
      },
    get: (params: {
      id?: string
      name?: string
      startedAt?: string
      startedBy?: string
      dataSet?: string
      memo?: string
      status?: string
      entryPoint?: string
      parentId?: string
      parentInferenceId?: string
      parentName?: string
      parentInferenceName?: string
      perPage?: number
      page?: number
      withTotal?: boolean
    }) =>
      gen.getApiV2Inferences(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            parentInferenceFullNameList: [string]
            outputValue: string
          },
        ]
      },
    getMount: (params: { status: [string] }) =>
      gen.getApiV2InferencesMount(params) as {
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
            status: string
            favorite: boolean
            fullName: string
            dataSet: {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              isFlat: boolean
            }
            entryPoint: string
            parentFullNameList: [string]
            parentInferenceFullNameList: [string]
            outputValue: string
          },
        ]
      },
    post: (params: {
      body: {
        name: string
        containerImage: {
          registryId: number
          image: string
          tag: string
        }
        dataSetId: number
        parentIds: [number]
        inferenceIds: [number]
        gitModel: {
          gitId: number
          repository: string
          owner: string
          branch: string
          commitId: string
        }
        entryPoint: string
        options: {
          additionalProp1: string
          additionalProp2: string
          additionalProp3: string
        }
        cpu: number
        memory: number
        gpu: number
        partition: string
        ports: [number]
        memo: string
        tags: [string]
        zip: boolean
        localDataSet: boolean
      }
    }) =>
      gen.postApiV2InferencesRun(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getById: (params: { id: number }) =>
      gen.getApiV2InferencesById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
          dataSet: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            displayId: number
            name: string
            memo: string
            isFlat: boolean
          }
          entryPoint: string
          parentFullNameList: [string]
          parentInferenceFullNameList: [string]
          outputValue: string
          key: string
          gitModel: {
            gitId: number
            repository: string
            owner: string
            branch: string
            commitId: string
            url: string
          }
          options: [
            {
              key: string
              value: string
            },
          ]
          containerImage: {
            registryId: number
            image: string
            tag: string
            registryName: string
            url: string
          }
          parents: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              status: string
              favorite: boolean
              fullName: string
              dataSet: {
                createdBy: string
                createdAt: string
                modifiedBy: string
                modifiedAt: string
                id: number
                displayId: number
                name: string
                memo: string
                isFlat: boolean
              }
              entryPoint: string
              parentFullNameList: [string]
              tags: [string]
            },
          ]
          parentInferences: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              displayId: number
              name: string
              memo: string
              status: string
              favorite: boolean
              fullName: string
              dataSet: {
                createdBy: string
                createdAt: string
                modifiedBy: string
                modifiedAt: string
                id: number
                displayId: number
                name: string
                memo: string
                isFlat: boolean
              }
              entryPoint: string
              parentFullNameList: [string]
              parentInferenceFullNameList: [string]
              outputValue: string
            },
          ]
          completedAt: string
          startedAt: string
          node: string
          logSummary: string
          cpu: number
          memory: number
          gpu: number
          partition: string
          statusType: string
          conditionNote: string
          zip: boolean
          localDataSet: boolean
        }
      },
    deleteById: (params: { id: number }) =>
      gen.deleteApiV2InferencesById(params),
    putById: (params: {
      id: number
      body: {
        name: string
        memo: string
        favorite: boolean
        tags: [string]
      }
    }) =>
      gen.putApiV2InferencesById(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    // GET /spa/trains/{id}/log
    getFilesById: (params: { id: number; withUrl?: boolean }) =>
      gen.getApiV2InferencesByIdFiles(params) as {
        data: [
          {
            id: number
            fileId: number
            url: string
            fileName: string
            isLocked: boolean
          },
        ]
      },
    getFileSize: (params: { id: number; name: string }) =>
      gen.getApiV2InferencesByIdFilesByNameSize(params) as {
        data: {
          id: number
          fileId: number
          key: string
          url: string
          fileName: string
          fileSize: number
        }
      },
    postFilesById: (params: {
      id: number
      body: {
        fileName: string
        storedPath: string
      }
    }) =>
      gen.postApiV2InferencesByIdFiles(params) as {
        data: {
          id: number
          fileId: number
          url: string
          fileName: string
          isLocked: boolean
        }
      },
    getContainerFilesById: (params: {
      id: number
      path?: string
      withUrl?: boolean
    }) =>
      gen.getApiV2InferencesByIdContainerFiles(params) as {
        data: {
          dirs: [
            {
              dirPath: string
              dirName: string
            },
          ]
          files: [
            {
              key: string
              fileName: string
              lastModified: string
              size: number
              url: string
            },
          ]
          exceeded: boolean
        }
      },
    deleteByIdFilesByFileId: (params: { id: number; fileId: number }) =>
      gen.deleteApiV2InferencesByIdFilesByFileId(params),
    postHaltById: (params: { id: number }) =>
      gen.postApiV2InferencesByIdHalt(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    postUserCancelById: (params: { id: number }) =>
      gen.postApiV2InferencesByIdUserCancel(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          displayId: number
          name: string
          memo: string
          status: string
          favorite: boolean
          fullName: string
        }
      },
    getEventsById: (params: { id: number }) =>
      gen.getApiV2InferencesByIdEvents(params) as {
        data: {
          tenantId: number
          tenantName: string
          containerName: string
          message: string
          details: string
          isError: boolean
          firstTimestamp: string
          lastTimestamp: string
        }
      },
  },

  storage: {
    admin: {
      get: () =>
        gen.getApiV2AdminStorageEndpoints() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              serverUrl: string
              nfsServer: string
              nfsRoot: string
            },
          ]
        },
      post: (params: {
        body: {
          name: string
          serverUrl: string
          accessKey: string
          secretKey: string
          nfsServer: string
          nfsRoot: string
        }
      }) =>
        gen.postApiV2AdminStorageEndpoints(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serverUrl: string
            nfsServer: string
            nfsRoot: string
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminStorageEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serverUrl: string
            nfsServer: string
            nfsRoot: string
            accessKey: string
            secretKey: string
          }
        },
      put: (params: {
        id: number
        body: {
          name: string
          serverUrl: string
          accessKey: string
          secretKey: string
          nfsServer: string
          nfsRoot: string
        }
      }) =>
        gen.putApiV2AdminStorageEndpointsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serverUrl: string
            nfsServer: string
            nfsRoot: string
          }
        },
      delete: (params: { id: number }) =>
        gen.deleteApiV2AdminStorageEndpointsById(params),
    },
    getUploadParameter: (params: {
      fileName: string
      partSum: number
      type: string
    }) =>
      gen.getApiV2UploadParameter(params) as {
        data: {
          uris: [string]
          partsSum: number
          uploadId: string
          key: string
          fileName: string
          storedPath: string
        }
      },
    postUploadComplete: (params: {
      body: {
        partETags: [string]
        uploadId: string
        key: string
      }
    }) =>
      gen.postApiV2UploadComplete(params) as {
        data: {
          partETags: [string]
          uploadId: string
          key: string
        }
      },
    getDownloadUrl: (params: {
      type?: string
      storedPath?: string
      fileName?: string
      secure: boolean
    }) => gen.getApiV2DownloadUrl(params) as { data: { url: string } },
  },

  tenant: {
    admin: {
      get: () =>
        gen.getApiV2AdminTenants() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              displayName: string
              storagePath: string
            },
          ]
        },
      post: (params: {
        body: {
          displayName: string
          defaultGitId: number
          gitIds: [number]
          defaultRegistryId: number
          registryIds: [number]
          storageId: number
          availableInfiniteTimeNotebook: boolean
          userGroupIds: [number]
          tenantName: string
        }
      }) =>
        gen.postApiV2AdminTenants(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            storagePath: string
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminTenantsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            storagePath: string
            defaultGitId: number
            gitIds: [number]
            defaultRegistryId: number
            registryIds: [number]
            storageId: number
            availableInfiniteTimeNotebook: boolean
            userGroupIds: [number]
          }
        },
      put: (params: {
        id: number
        body: {
          displayName: string
          defaultGitId: number
          gitIds: [number]
          defaultRegistryId: number
          registryIds: [number]
          storageId: number
          availableInfiniteTimeNotebook: boolean
          userGroupIds: [number]
        }
      }) =>
        gen.putApiV2AdminTenantsById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            displayName: string
            storagePath: string
          }
        },
      delete: (params: { id: number }) =>
        gen.deleteApiV2AdminTenantsById(params) as {
          data: {
            containerWarnMsg: string
          }
        },
    },
    get: () =>
      gen.getApiV2Tenant() as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          displayName: string
          storagePath: string
          defaultGitId: number
          gitIds: [number]
          defaultRegistryId: number
          registryIds: [number]
          storageId: number
          availableInfiniteTimeNotebook: boolean
          userGroupIds: [number]
        }
      },
    put: (params: {
      body: {
        displayName: string
        defaultGitId: number
        gitIds: [number]
        defaultRegistryId: number
        registryIds: [number]
        storageId: number
        availableInfiniteTimeNotebook: boolean
        userGroupIds: [number]
      }
    }) =>
      gen.putApiV2Tenant(params) as {
        data: {
          createdBy: string
          createdAt: string
          modifiedBy: string
          modifiedAt: string
          id: number
          name: string
          displayName: string
          storagePath: string
        }
      },
  },

  user: {
    admin: {
      get: () =>
        gen.getApiV2AdminUsers() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              serviceType: number
              systemRoles: [
                {
                  id: number
                  name: string
                  displayName: string
                  isCustomed: boolean
                  sortOrder: number
                  isOrigin: boolean
                  userGroupTanantMapIdLists: [number]
                },
              ]
              tenants: [
                {
                  id: number
                  name: string
                  default: boolean
                  displayName: string
                  roles: [
                    {
                      id: number
                      name: string
                      displayName: string
                      isCustomed: boolean
                      sortOrder: number
                      isOrigin: boolean
                      userGroupTanantMapIdLists: [number]
                    },
                  ]
                  isOrigin: boolean
                },
              ]
            },
          ]
        },
      post: (params: {
        body: {
          name: string
          password: string
          systemRoles: [number]
          tenants: [
            {
              id: number
              default: boolean
              roles: [number]
            },
          ]
        }
      }) =>
        gen.postApiV2AdminUsers(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            systemRoles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
            tenants: [
              {
                id: number
                name: string
                default: boolean
                displayName: string
                roles: [
                  {
                    id: number
                    name: string
                    displayName: string
                    isCustomed: boolean
                    sortOrder: number
                    isOrigin: boolean
                    userGroupTanantMapIdLists: [number]
                  },
                ]
                isOrigin: boolean
              },
            ]
          }
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminUsersById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            systemRoles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
            tenants: [
              {
                id: number
                name: string
                default: boolean
                displayName: string
                roles: [
                  {
                    id: number
                    name: string
                    displayName: string
                    isCustomed: boolean
                    sortOrder: number
                    isOrigin: boolean
                    userGroupTanantMapIdLists: [number]
                  },
                ]
                isOrigin: boolean
              },
            ]
          }
        },
      delete: (params: { id: number }) => gen.deleteApiV2AdminUsersById(params),
      put: (params: {
        id: number
        body: {
          systemRoles: [number]
          tenants: [
            {
              id: number
              default: boolean
              roles: [number]
            },
          ]
        }
      }) =>
        gen.putApiV2AdminUsersById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            systemRoles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
            tenants: [
              {
                id: number
                name: string
                default: boolean
                displayName: string
                roles: [
                  {
                    id: number
                    name: string
                    displayName: string
                    isCustomed: boolean
                    sortOrder: number
                    isOrigin: boolean
                    userGroupTanantMapIdLists: [number]
                  },
                ]
                isOrigin: boolean
              },
            ]
          }
        },
      putPassword: (params: { id: number; body: string }) =>
        simpleStringBody(gen.putApiV2AdminUsersByIdPassword, 'body')(params),
      postSyncLdap: (params: {
        body: {
          userName: string
          password: string
        }
      }) => gen.postApiV2AdminUsersSyncLdap(params),
    },

    tenant: {
      get: () =>
        gen.getApiV2TenantUsers() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              serviceType: number
              roles: [
                {
                  id: number
                  name: string
                  displayName: string
                  isCustomed: boolean
                  sortOrder: number
                  isOrigin: boolean
                  userGroupTanantMapIdLists: [number]
                },
              ]
            },
          ]
        },
      getById: (params: { id: number }) =>
        gen.getApiV2TenantUsersById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
          }
        },
      delete: (params: { id: number }) =>
        gen.deleteApiV2TenantUsersById(params),
      putRoles: (params: { id: number; body: [number] }) =>
        gen.putApiV2TenantUsersByIdRoles(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            serviceType: number
            systemRoles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
            tenants: [
              {
                id: number
                name: string
                default: boolean
                displayName: string
                roles: [
                  {
                    id: number
                    name: string
                    displayName: string
                    isCustomed: boolean
                    sortOrder: number
                    isOrigin: boolean
                    userGroupTanantMapIdLists: [number]
                  },
                ]
                isOrigin: boolean
              },
            ]
          }
        },
    },
  },

  userGroup: {
    admin: {
      get: () =>
        gen.getApiV2AdminUsergroup() as {
          data: [
            {
              createdBy: string
              createdAt: string
              modifiedBy: string
              modifiedAt: string
              id: number
              name: string
              memo: string
              isGroup: boolean
              dn: string
            },
          ]
        },
      getById: (params: { id: number }) =>
        gen.getApiV2AdminUsergroupById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            memo: string
            isGroup: boolean
            dn: string
            isDirect: boolean
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
          }
        },
      post: (params: {
        body: {
          name: string
          memo: string
          isGroup: boolean
          dn: string
          isDirect: boolean
          roleIds: [number]
        }
      }) =>
        gen.postApiV2AdminUsergroup(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            memo: string
            isGroup: boolean
            dn: string
            isDirect: boolean
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
          }
        },
      put: (params: {
        id: number
        body: {
          name: string
          memo: string
          isGroup: boolean
          dn: string
          isDirect: boolean
          roleIds: [number]
        }
      }) =>
        gen.putApiV2AdminUsergroupById(params) as {
          data: {
            createdBy: string
            createdAt: string
            modifiedBy: string
            modifiedAt: string
            id: number
            name: string
            memo: string
            isGroup: boolean
            dn: string
            isDirect: boolean
            roles: [
              {
                id: number
                name: string
                displayName: string
                isCustomed: boolean
                sortOrder: number
                isOrigin: boolean
                userGroupTanantMapIdLists: [number]
              },
            ]
          }
        },
      delete: (params: { id: number }) =>
        gen.deleteApiV2AdminUsergroupById(params),
    },
  },

  version: {
    get: () =>
      gen.getApiV2Version() as {
        data: {
          version: string
          messages: [string]
        }
      },
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
