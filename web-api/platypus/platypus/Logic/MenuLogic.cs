using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// メニュー関係の操作をまとめたロジック
    /// </summary>
    public class MenuLogic : PlatypusLogicBase, IMenuLogic
    {
        private IRoleRepository roleRepository;
        private ITenantRepository tenantRepository;
        private readonly IMenuRepository menuRepository;

        static MenuLogic()
        {
            MenuList = new List<MenuItemInfo>()
            {
                LoginMenu,
                DashBoardMenu,
                AccountMenu,

                DataMenu,
                DataSetMenu,
                PreprocessMenu,
                TrainingMenu,
                InferenceMenu,

                //TenantSettingMenu,
                //TenantRoleMenu,
                TenantUserMenu,
                //TenantMenuAccessMenu,
                TenantResourceMenu,

                TenantMenu,
                GitMenu,
                RegistryMenu,
                StorageMenu,
                RoleMenu,
                QuotaMenu,
                UserMenu,
                NodeMenu,
                MenuAccessMenu,
                ResourceMenu
            };

            MenuMap = new Dictionary<MenuCode, MenuItemInfo>();
            foreach(var menu in MenuList)
            {
                MenuMap.Add(menu.Code, menu);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MenuLogic(
            IRoleRepository roleRepository,
            ITenantRepository tenantRepository,
            IMenuRepository menuRepository,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.roleRepository = roleRepository;
            this.tenantRepository = tenantRepository;
            this.menuRepository = menuRepository;
        }

        /// <summary>
        /// ログインユーザが表示可能なホーム画面用のメニューリストを取得する
        /// </summary>
        /// <returns>トップメニュー用のメニューリスト</returns>
        public async Task<IEnumerable<MenuItemInfo>> GetTopMenuListAsync()
        {
            if (CurrentUserInfo.SelectedTenantRoles.Count() == 0)
            {
                return new List<MenuItemInfo>();
            }

            //本来はLinqで取りたいが、Where句でawaitが使えないので仕方なく手で詰めなおす
            var result = new List<MenuItemInfo>();
            foreach(var menu in MenuList)
            {
                if(menu.ShowTopMenu && await IsAccessibleMenuAsync(menu))
                {
                    result.Add(menu);
                }
            }
            return result;
        }

        /// <summary>
        /// ログインユーザが表示可能なサイドメニューリストを取得する
        /// </summary>
        /// <returns>サイドメニュー用のメニューリスト</returns>
        public async Task<IEnumerable<MenuItemInfo>> GetSideMenuListAsync()
        {
            if (CurrentUserInfo.SelectedTenantRoles.Count() == 0)
            {
                return new List<MenuItemInfo>();
            }

            return await GetAccessibleMenuTreeAsync(MenuTree);
        }

        /// <summary>
        /// アクセス可能なメニューのみをツリー形式で取得するための再帰メソッド
        /// </summary>
        private async Task<IEnumerable<MenuItemInfo>> GetAccessibleMenuTreeAsync(IEnumerable<MenuItemInfo> sourceList)
        {
            var targetList = new List<MenuItemInfo>();

            foreach (var menu in sourceList)
            {
                if(menu.Children == null)
                {
                    //子供がいない場合（＝メニュー項目）は、サイドメニュー表示が有効で、アクセス許可があるメニューを追加
                    if (menu.ShowSideMenu && await IsAccessibleMenuAsync(menu))
                    {
                        targetList.Add(menu);
                    }
                }
                else
                {
                    //子供がいる場合（＝グループ項目）は、子供を再帰的に調べて、表示できる子供が一つでもいれば追加
                    var children = await GetAccessibleMenuTreeAsync(menu.Children);
                    if(children.Count() > 0)
                    {
                        var group = new MenuItemInfo()
                        {
                            Name = menu.Name,
                            Children = children,
                            Category = menu.Category
                        };
                        targetList.Add(group);
                    }
                }
            }

            return targetList;
        }

        /// <summary>
        /// 指定されたメニューコードのいづれかにアクセス可能か判定する
        /// </summary>
        /// <param name="menuCodes">メニューコードのリスト</param>
        /// <returns>アクセス可能な場合、true</returns>
        public async Task<bool> IsAccessibleMenuAsync(MenuCode[] menuCodes)
        {
            // 何も指定されてない場合、共通とみなしてアクセス可と判断
            if (menuCodes == null || menuCodes.Length == 0)
            {
                return true;
            }

            //メニューコードそれぞれについて、権限を確認。一つでもOKなら許可。
            foreach (MenuCode menuCode in menuCodes)
            {
                if (MenuMap.ContainsKey(menuCode))
                {
                    var menu = MenuMap[menuCode];
                    if (await IsAccessibleMenuAsync(menu))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 指定されたメニューにアクセス可能か判定する
        /// </summary>
        /// <param name="menu">メニュー</param>
        /// <returns>アクセス可能な場合、true</returns>
        public async Task<bool> IsAccessibleMenuAsync(MenuItemInfo menu)
        {
            if (menu.MenuType == MenuType.Public || menu.MenuType == MenuType.Internal)
            {
                //公開済みメニューなら無条件で許可
                return true;
            }
            
            //現在のロールを取得
            IEnumerable<Role> roles = menu.MenuType == MenuType.System ?
                CurrentUserInfo.SystemRoles : //システムメニューの場合はシステムロールを確認
                CurrentUserInfo.SelectedTenantRoles;
            
            foreach (var role in roles)
            {
                if (await roleRepository.AuthorizeAsync(role.Id, menu.Code))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 各メニューごとにアクセス許可されている管理者用ロールを取得する
        /// </summary>
        public Dictionary<MenuItemInfo, IEnumerable<Role>> GetRoleIdsForAdminDictionary()
        {
            var result = new Dictionary<MenuItemInfo, IEnumerable<Role>>();

            foreach (var menu in MenuList)
            {
                IEnumerable<Role> roles;
                if (menu.MenuType != MenuType.Tenant && menu.MenuType != MenuType.System)
                {
                    roles = new List<Role>();
                }
                else
                {
                    roles = menuRepository.GetAttachedRoles(menu.Code);
                }
                result.Add(menu, roles);
            }
            return result;
        }

        /// <summary>
        /// 各メニューごとにアクセス許可されているテナント用ロールを取得する
        /// </summary>
        public Dictionary<MenuItemInfo, IEnumerable<Role>> GetRoleIdsForTenantDictionary(long tenantId)
        {
            var result = new Dictionary<MenuItemInfo, IEnumerable<Role>>();

            foreach (var menu in MenuList)
            {
                if (menu.MenuType != MenuType.Tenant)
                {
                    continue;
                }
                var roles = menuRepository.GetAttachedRoles(menu.Code, tenantId);
                result.Add(menu, roles);
            }
            return result;
        }

        /// <summary>
        /// 指定したメニューコードに一致するメニュー情報を取得する
        /// </summary>
        public MenuItemInfo GetMenu(MenuCode menuCode)
        {
            return MenuList.Find(m => m.Code == menuCode);
        }

        #region メニュー項目
        
        /// <summary>
        /// メニュー一覧。
        /// 全メニューを並べる。
        /// </summary>
        public static List<MenuItemInfo> MenuList { get; private set; }

        /// <summary>
        /// <see cref="MenuCode"/>から<see cref="MenuItemInfo"/>を引くためのマップ。
        /// <see cref="MenuList"/>を元にstaticコンストラクタで初期化される。
        /// </summary>
        public static Dictionary<MenuCode, MenuItemInfo> MenuMap;

        #region メニュー単体
        internal static MenuItemInfo LoginMenu = new MenuItemInfo()
        {
            Name = "ログイン",
            Description = "ログイン",
            Code = MenuCode.Login,
            Url = "/login",
            ShowTopMenu = false,
            ShowSideMenu = false,
            MenuType = MenuType.Public
        };
        internal static MenuItemInfo AccountMenu = new MenuItemInfo()
        {
            Name = "アカウント管理",
            Description = "ログインユーザアカウントの管理",
            Code = MenuCode.Account,
            Url = "/setting",
            ShowTopMenu = false,
            ShowSideMenu = false,
            MenuType = MenuType.Internal
        };
        internal static MenuItemInfo DashBoardMenu = new MenuItemInfo()
        {
            Name = "ダッシュボード",
            Description = "ダッシュボード",
            Code = MenuCode.DashBoard,
            Url = "/",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Internal
        };
        internal static MenuItemInfo DataMenu = new MenuItemInfo()
        {
            Name = "データ管理",
            NameEn = "Data",
            Description = "各種データや教師データ(アノテーションファイル)の管理",
            DescriptionEn = "Upload and Download file",
            Category = "pl-data",
            Code = MenuCode.Data,
            Url = "/data",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo DataSetMenu = new MenuItemInfo()
        {
            Name = "データセット管理",
            NameEn = "DataSet",
            Description = "登録したデータを train / test / validation にわけデータセットとして利用できるようにする",
            DescriptionEn = "Grouping Uploaded Data for Training and Test",
            Category = "pl-dataset",
            Code = MenuCode.DataSet,
            Url = "/dataset",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo PreprocessMenu = new MenuItemInfo()
        {
            Name = "前処理管理",
            NameEn = "Preprocess",
            Description = "ファイルの解凍や画像ファイルのクロッピングなど、各種データ加工処理を管理",
            DescriptionEn = "Managing Preprocess Status and Histories",
            Category = "pl-preprocessing",
            Code = MenuCode.Preprocess,
            Url = "/preprocessing",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo NotebookMenu = new MenuItemInfo()
        {
            Name = "ノートブック管理",
            NameEn = "Notebook",
            Description = "ノートブックジョブに関する各種管理",
            DescriptionEn = "Managing Notebook Status and Histories",
            Category = "pl-notebook",
            Code = MenuCode.Notebook,
            Url = "/notebook",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo TrainingMenu = new MenuItemInfo()
        {
            Name = "学習管理",
            NameEn = "Training",
            Description = "機械学習などのジョブを投入するなどの各種管理",
            DescriptionEn = "Managing Training Status and Histories",
            Category = "pl-training",
            Code = MenuCode.Training,
            Url = "/training",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo InferenceMenu = new MenuItemInfo()
        {
            Name = "推論管理",
            NameEn = "Inference",
            Description = "推論ジョブを投入するための各種管理",
            DescriptionEn = "Managing Inference Status and Histories",
            Category = "pl-inference",
            Code = MenuCode.Inference,
            Url = "/inference",
            ShowTopMenu = true,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };

        internal static MenuItemInfo TenantSettingMenu = new MenuItemInfo()
        {
            Name = "接続テナント設定",
            Description = "接続しているテナントの設定変更",
            Code = MenuCode.TenantSetting,
            Url = "/manage/tenant",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo TenantRoleMenu = new MenuItemInfo()
        {
            Name = "テナントロール管理",
            Description = "テナント用カスタムロール管理",
            Code = MenuCode.TenantRole,
            Url = "/manage/role",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        }; 
        internal static MenuItemInfo TenantUserMenu = new MenuItemInfo()
        {
            Name = "テナントユーザ管理",
            Description = "所属しているユーザに対する各種設定",
            Code = MenuCode.TenantUser,
            Url = "/manage/user",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo TenantResourceMenu = new MenuItemInfo()
        {
            Name = "テナントリソース管理",
            Description = "テナント単位でのリソース利用状況管理",
            Code = MenuCode.TenantResource,
            Url = "/manage/resource",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };
        internal static MenuItemInfo TenantMenuAccessMenu = new MenuItemInfo()
        {
            Name = "テナントメニュー管理",
            Description = "メニューに対するカスタムロール毎のアクセス権管理",
            Code = MenuCode.TenantMenu,
            Url = "/manage/menu",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.Tenant
        };


        internal static MenuItemInfo TenantMenu = new MenuItemInfo()
        {
            Name = "テナント管理",
            Description = "新規テナントの追加や既存テナントの設定変更",
            Code = MenuCode.Tenant,
            Url = "/tenant",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo GitMenu = new MenuItemInfo()
        {
            Name = "Git管理",
            Description = "テナントに関連付ける Git リポジトリの管理",
            Code = MenuCode.Git,
            Url = "/git",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo RegistryMenu = new MenuItemInfo()
        {
            Name = "レジストリ管理",
            Description = "テナントに関連付ける Docker レジストリの管理",
            Code = MenuCode.Registry,
            Url = "/registry",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo StorageMenu = new MenuItemInfo()
        {
            Name = "ストレージ管理",
            Description = "テナントに関連付ける NFS ストレージの管理",
            Code = MenuCode.Storage,
            Url = "/storage",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo RoleMenu = new MenuItemInfo()
        {
            Name = "ロール管理",
            Description = "テナント横断で使用するシステムロールおよびテナントロールの改廃",
            Code = MenuCode.Role,
            Url = "/role",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo QuotaMenu = new MenuItemInfo()
        {
            Name = "クォータ管理",
            Description = "テナントが利用できる最大リソース量を制限",
            Code = MenuCode.Quota,
            Url = "/quota",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo UserMenu = new MenuItemInfo()
        {
            Name = "ユーザ管理",
            Description = "テナントへのユーザ追加・削除や各種ユーザのロール変更",
            Code = MenuCode.User,
            Url = "/user",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo NodeMenu = new MenuItemInfo()
        {
            Name = "ノード管理",
            Description = "クラスタノードの設定管理",
            Code = MenuCode.Node,
            Url = "/node",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo MenuAccessMenu = new MenuItemInfo()
        {
            Name = "メニューアクセス管理",
            Description = "メニューに対するロール毎のアクセス権管理",
            Code = MenuCode.Menu,
            Url = "/menu",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };
        internal static MenuItemInfo ResourceMenu = new MenuItemInfo()
        {
            Name = "リソース管理",
            Description = "クラスタノードの利用状況を参照",
            Code = MenuCode.Resource,
            Url = "/cluster-resource",
            ShowTopMenu = false,
            ShowSideMenu = true,
            MenuType = MenuType.System
        };

        #endregion

        #region メニューツリー

        private static IEnumerable<MenuItemInfo> MenuTree = new List<MenuItemInfo>()
        {
            DataMenu,
            DataSetMenu,
            PreprocessMenu,
            TrainingMenu,
            InferenceMenu,

            new MenuItemInfo()
            {
                Name = "テナント設定",
                Category = "pl-tenant-setting",
                Children = new List<MenuItemInfo>()
                {
                    //TenantSettingMenu,
                    //TenantRoleMenu,
                    TenantUserMenu,
                    //TenantMenuAccessMenu,
                    TenantResourceMenu
                }
            },
            new MenuItemInfo()
            {
                Name = "システム設定",
                Category = "pl-system-setting",
                Children = new List<MenuItemInfo>()
                {
                    TenantMenu,
                    GitMenu,
                    RegistryMenu,
                    StorageMenu,
                    RoleMenu,
                    QuotaMenu,
                    NodeMenu,
                    UserMenu,
                    MenuAccessMenu,
                    ResourceMenu
                }
            }
        };

        #endregion
        #endregion
    }
}
