using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nssol.Platypus.ApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.DataAccess;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.DataAccess.Repositories.TenantRepositories;
using Nssol.Platypus.Filters;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic;
using Nssol.Platypus.Logic.HostedService;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Services;
using Nssol.Platypus.Services.Interfaces;
using Nssol.Platypus.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;

namespace Nssol.Platypus
{
    /// <summary>
    /// スタートアップクラス
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// コンフィギュレーション
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// 共通DBの接続文字列
        /// TODO: Entity Framework CoreにSeed機能が実装されたら削除する。
        /// </summary>
        public static string DefaultConnectionString { get; set; }

        /// <summary>
        /// <see cref="Configure(IApplicationBuilder, IHostingEnvironment, ILoggerFactory, IOptions{WebSecurityOptions}, ICommonDiLogic, IApiVersionDescriptionProvider)"/> 内処理用のロガー
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// パイプラインのデバッグがあまりに辛いので、デバッグ用のログを出せるようにした。
        /// </summary>
        private bool isDebug;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration">設定情報</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 使用するサービスをコンテナに追加します。
        /// </summary>
        /// <param name="services">サービスコンテナ</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions();
            services.Configure<ActiveDirectoryOptions>(Configuration.GetSection("ActiveDirectoryOptions"));
            services.Configure<ContainerManageOptions>(Configuration.GetSection("ContainerManageOptions"));
            services.Configure<WebSecurityOptions>(Configuration.GetSection("WebSecurityOptions"));
            services.Configure<ObjectStorageOptions>(Configuration.GetSection("ObjectStorageOptions"));
            services.Configure<DeleteTensorBoardContainerTimerOptions>(Configuration.GetSection("DeleteTensorBoardContainerTimerOptions"));
            services.Configure<DeleteNotebookContainerTimerOptions>(Configuration.GetSection("DeleteNotebookContainerTimerOptions"));
            services.Configure<BackupPostgresTimerOptions>(Configuration.GetSection("BackupPostgresTimerOptions"));
            services.Configure<DBInitRetryOptions>(Configuration.GetSection("DBInitRetryOptions"));
            services.Configure<GetKQIReleaseVersionTimerOptions>(Configuration.GetSection("GetKQIReleaseVersionTimerOptions"));

            services.Configure<SyncClusterFromDBOptions>(Configuration.GetSection("SyncClusterFromDBOptions"));
            services.Configure<DeployOptions>(Configuration.GetSection("DeployOptions"));

            DefaultConnectionString = Configuration.GetConnectionString("DefaultConnection");

            // Entity Frameworkの追加
            services.AddDbContext<CommonDbContext>(ServiceLifetime.Scoped);

            // LogicのDI設定
            services.AddTransient<IClusterManagementLogic, ClusterManagementLogic>();
            services.AddTransient<ILoginLogic, LoginLogic>();
            services.AddTransient<IDataLogic, DataLogic>();
            services.AddTransient<ITrainingLogic, TrainingLogic>();
            services.AddTransient<IInferenceLogic, InferenceLogic>();
            services.AddTransient<IPreprocessLogic, PreprocessLogic>();
            services.AddTransient<INotebookLogic, NotebookLogic>();
            services.AddTransient<IExperimentLogic, ExperimentLogic>();
            services.AddTransient<IStorageLogic, StorageLogic>();
            services.AddTransient<ITagLogic, TagLogic>();
            services.AddTransient<IGitLogic, GitLogic>();
            services.AddTransient<IRegistryLogic, RegistryLogic>();
            services.AddTransient<IMenuLogic, MenuLogic>();
            services.AddTransient<IVersionLogic, VersionLogic>();

            // ServiceのDI設定
            services.AddTransient<IClusterManagementService, KubernetesService>();
            services.AddTransient<IObjectStorageService, ObjectStorageS3Service>();
            services.AddTransient<IVersionService, VersionService>();
            // 切替のため型指定でDI設定
            services.AddTransient<GitHubService>();
            services.AddTransient<GitLabService>();
            services.AddTransient<GitLabComService>();
            services.AddTransient<GitBucketService>();
            services.AddTransient<DockerHubRegistryService>();
            services.AddTransient<GitLabRegistryService>();
            services.AddTransient<PrivateDockerRegistryService>();
            services.AddTransient<NvidiaGPUCloudRegistryService>();

            // RepositoryのDI設定
            services.AddTransient<IDataRepository, DataRepository>();
            services.AddTransient<IDataTypeRepository, DataTypeRepository>();
            services.AddTransient<IDataSetRepository, DataSetRepository>();
            services.AddTransient<IPreprocessHistoryRepository, PreprocessHistoryRepository>();
            services.AddTransient<IPreprocessRepository, PreprocessRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<IRegistryRepository, RegistryRepository>();
            services.AddTransient<IGitRepository, GitRepository>();
            services.AddTransient<ITensorBoardContainerRepository, TensorBoardContainerRepository>();
            services.AddTransient<ITrainingHistoryRepository, TrainingHistoryRepository>();
            services.AddTransient<IInferenceHistoryRepository, InferenceHistoryRepository>();
            services.AddTransient<INotebookHistoryRepository, NotebookHistoryRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<INodeRepository, NodeRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<INodeTenantMapRepository, NodeTenantMapRepository>();
            services.AddTransient<ITemplateRepository, TemplateRepository>();
            services.AddTransient<IExperimentHistoryRepository, ExperimentHistoryRepository>();
            services.AddTransient<IAqiariumDataSetRepository, AquariumDataSetRepository>();

            // その他のDI設定
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(JsonExceptionHandlerAttribute));
            services.AddTransient(typeof(Seed));

            services.AddScoped<IMultiTenancyLogic, MultiTenancyLogic>();
            services.AddScoped<ICommonDiLogic, CommonDiLogic>();

            //appsettingss.jsonから設定値を取得できるようにする
            services.AddSingleton<IConfiguration>(Configuration);

            // HostedService(Timer類)のDI設定
            services.AddSingleton<DeleteTensorBoardContainerTimer>();
            services.AddSingleton<DeleteNotebookContainerTimer>();
            services.AddSingleton<BackupPostgresTimer>();
            services.AddSingleton<SyncClusterFromDBTimer>();
            services.AddSingleton<GetKQIReleaseVersionTimer>();

            //ASP.NET Core MVCの追加
            WebSecurityOptions wsops = new WebSecurityOptions();
            Configuration.GetSection("WebSecurityOptions").Bind(wsops);

            //DBの内容を一定時間保持するためのメモリキャッシュを利用する
            services.AddMemoryCache();

            #region services.AddAuthentication

            var sp = services.BuildServiceProvider();
            var settingRepository = sp.GetService<ISettingRepository>();
            var key = settingRepository.GetApiJwtSigningKey();

            services.AddAuthentication(
                //既定の認証スキーマ。
                JwtBearerDefaults.AuthenticationScheme
            )
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        // 署名キー検証
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        // iss（issuer）クレーム
                        ValidateIssuer = true,
                        ValidIssuer = wsops.ApiJwtIssuer,
                        // aud（audience）クレーム
                        ValidateAudience = true,
                        ValidAudience = wsops.ApiJwtAudience,
                        // トークンの有効期限の検証
                        ValidateLifetime = true,
                        // クライアントとサーバーの間の時刻の設定で許容される最大の時刻のずれ
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var token = context.SecurityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                            if (token != null && isDebug)
                            {
                                //アクセスログ出力
                                LogUtil.WritePLog(logger.LogInformation, context.HttpContext, $"Receive authorized api request with token {token.Id} as {token.Subject}");
                            }
                            return Task.FromResult(0);
                        },
                        OnChallenge = context =>
                        {
                            // 失敗した際のメッセージをレスポンスに格納する
                            if (context.AuthenticateFailure != null)
                            {
                                context.Response.OnStarting(async state =>
                                {
                                    // アクセスコードが不正な文字列で復元できない場合や、アクセスコードの期限が切れているときにこちらに入る

                                    //期限切れはInfo扱いで出力する
                                    LogUtil.WritePLog(logger.LogInformation, context.HttpContext, $"The Access code is expired or invalid：{context.AuthenticateFailure.Message}");

                                    await new CustomJsonResult(StatusCodes.Status401Unauthorized,
                                        new JsonErrorResponse()
                                        {
                                            Type = this.GetType().FullName,
                                            Title = "The Access code is expired or invalid.",
                                            Instance = context.Request?.Path.Value
                                        }
                                        ).SerializeJsonAsync(((JwtBearerChallengeContext)state).Response);
                                    return;
                                }, context);
                            }
                            else
                            {
                                context.Response.OnStarting(async state =>
                                {
                                    // アクセスコードがヘッダに設定されていない場合はこちらに入る

                                    //WebAPIは常にplatypus cliから実行されるので、ヘッダが設定されていない場合はありえない。
                                    //なのでここに到達したら不正アクセスと見なして、警告を出力する
                                    LogUtil.WritePLog(logger.LogWarning, context.HttpContext, $"The access code is required. status = {state}");

                                    await new CustomJsonResult(StatusCodes.Status401Unauthorized,
                                        new JsonErrorResponse()
                                        {
                                            Type = this.GetType().FullName,
                                            Title = "The access code is required.",
                                            Instance = context.Request?.Path.Value
                                        }
                                        ).SerializeJsonAsync(((JwtBearerChallengeContext)state).Response);
                                    return;
                                }, context);
                            }
                            return Task.FromResult(0);
                        },
                    };
                });
            #endregion

            services.AddCors();
            services.AddMvc(cfg =>
            {
                // 集約エラー用フィルター
                cfg.Filters.Add(typeof(GlobalExceptionHandlerAttribute));
            });

            // API Versioning
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options => 
                options.ApiVersionReader = new UrlSegmentApiVersionReader());

            if (wsops.EnableSwagger)
            {
                // SwaggerにXMLコメントの内容を反映させるために、サーバ上での XML Document のパスを渡す
                var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                var xmlPath = location.Replace("dll", "xml");
                //// SwaggerGen を追加
                services.AddSwaggerGen(options =>
                {
                    // APIの署名を記載
                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerDoc(description.GroupName, new Info
                            {
                                Title = "KAMONOHASHI API",
                                Version = description.GroupName,
                                Description = "A platform for deep learning",
                                Contact = new Contact()
                                {
                                    Email = "kamonohashi-support@jp.nssol.nipponsteel.com",
                                    Name = "KAMONOHASHI Support"
                                },
                                TermsOfService = ApplicationConst.Copyright,
                            });
                        }
                    }

                    // デフォルトだと同じクラス名の入出力モデルを使えないので、識別に名前空間名も含める
                    // https://stackoverflow.com/questions/46071513/swagger-error-conflicting-schemaids-duplicate-schemaids-detected-for-types-a-a
                    options.CustomSchemaIds(x => x.FullName);

                    // XML Document Comment を読込む
                    options.IncludeXmlComments(xmlPath);
                    //トークン認証用のUIを追加する
                    options.AddSecurityDefinition("api_key", new ApiKeyScheme()
                    {
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey", //この指定が必須。https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/124
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                    });

                    options.OperationFilter<AssignJwtSecurityRequirements>();
                });
            }

            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                //アプリケーションのアップロードサイズ上限を4GBに設定
                options.MultipartBodyLengthLimit = 4294967295;
            });

            // HostedService(Timer類)の登録
            services.AddHostedService<DeleteTensorBoardContainerTimer>();
            services.AddHostedService<DeleteNotebookContainerTimer>();
            services.AddHostedService<BackupPostgresTimer>();
            services.AddHostedService<SyncClusterFromDBTimer>();
            services.AddHostedService<GetKQIReleaseVersionTimer>();
        }

        /// <summary>
        /// HTTPリクエストパイプラインを構成します。
        /// app.UseXxx()でパイプラインにミドルウェアを登録して、app.Run()でミドルウェアのチェーンを終端させます。
        /// </summary>
        /// <param name="app">アプリケーション</param>
        /// <param name="env">ホスト環境</param>
        /// <param name="loggerFactory">ロガーファクトリ</param>
        /// <param name="securityOptions">appsettings.jsonから読み込んだセキュリティ設定情報</param>
        /// <param name="commonDiLogic">DI用</param>
        /// <param name="provider">API Versioning</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<WebSecurityOptions> securityOptions, ICommonDiLogic commonDiLogic, IApiVersionDescriptionProvider provider)
        {
            WebSecurityOptions options = securityOptions.Value;
            isDebug = options.EnableRequestPiplineDebugLog;

            //ログ設定（ここで一回やれば、各クラスでの設定は不要）
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddProvider(new Log4NetProvider());

            logger = loggerFactory.CreateLogger<Startup>();
            LogUtil.WriteSystemLog(logger.LogDebug, "Start to configure Platypus WebUI application");

            //Startup中に例外が起きるとログが残らないため、原因追跡のためのtry/catchでエラー出力を用意する
            try
            {
                AddMiddlewareForLogging(app, "Start pipeline", "end pipeline");

                //例外処理のミドルウェアを追加する
                //ASP.NET Core MVC内で発生した例外はFilterで吸収し、ここではその前段で発生した例外を処理する

                //例外の発生点からこのミドルウェアまでの間のパイプラインは、特にcatch処理がなければスキップされるので注意
                app.Use(async (context, next) =>
                {
                    try
                    {
                        await next();
                    }
                    catch (Exception e)
                    {
                        string errorMessage = "Unhandled Exception Occured: " + e.Message;
                        LogUtil.WriteErrorPLog(logger, context, errorMessage, e);

                        await new CustomJsonResult(StatusCodes.Status500InternalServerError,
                            new JsonErrorResponse()
                            {
                                Type = this.GetType().FullName,
                                Title = errorMessage,
                                Instance = context.Request?.Path.Value,
                            })
                            .SerializeJsonAsync(context.Response);
                    }
                });

                AddMiddlewareForLogging(app, "Execute Request", "Return Response");

                app.Use(async (httpContext, next) =>
                {
                    //クリックジャッキング対策
                    httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
                    //後続のミドルウェアにつなぐ

                    await next();
                });

                //静的ファイルがあればそれを出力する
                //エラーは発生しないハズなのでログ出力は行わない
                app.UseStaticFiles();

                AddMiddlewareForLogging(app, "Move to dinamic resource pipe", "Back from dinamic resource pipe");

                if (options.EnableSwagger)
                {
                    // UseSwagger と UseSwaggerUi を追加
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                        }
                    });
                }

                AddMiddlewareForLogging(app, "Move to authentication", "Back from authentication");

                app.UseAuthentication();

                //ASP.NET Core MVCからのレスポンスは常にデバッグログを残す
                AddMiddlewareForLogging(app, "Execute Request in ASP.NET Core MVC", null);
                AddMiddlewareForLogging(app, null, "Return Response from ASP.NET Core MVC", true);

                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Account}/{action=Index}/{id?}");
                });

                // /wsにアクセスされた際、kubernetes podのshell実行用のwebsocket通信を確立する
                app.UseWhen(context => IsWebSocket(context), appBuilder =>
                {
                    AddMiddlewareForLogging(appBuilder, "Execute WebSocket Request", null, true);

                    appBuilder.UseWebSockets();
                    appBuilder.Use(async (context, next) =>
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            var clusterManagementLogic = commonDiLogic.DynamicDi<Logic.Interfaces.IClusterManagementLogic>();
                            await clusterManagementLogic.ConnectKubernetesWebSocketAsync(context);
                        }
                    }
                    );
                });

                // 終端
                app.Run(async context =>
                {
                    //静的ファイル(拡張子付きのURL)はspa-fallbackに捕まらずバイパスされるので、ここでも404処理を入れる
                    LogUtil.WritePLog(logger.LogWarning, context, $"Failed to access valid resources or applications.");

                    await new CustomJsonResult(StatusCodes.Status404NotFound,
                        new JsonErrorResponse()
                        {
                            Type = this.GetType().FullName,
                            Title = "404 NOT FOUND..",
                            Instance = context.Request?.Path.Value
                        })
                        .SerializeJsonAsync(context.Response);
                    return;
                });
            }
            catch (Exception e)
            {
                //ログ出力するためのcatch処理。そのままリスロー。
                LogUtil.WriteSystemErrorLog(logger, e.Message, e);
                throw;
            }
            LogUtil.WriteSystemLog(logger.LogDebug, "End to configure Platypus WebUI application");
        }

        /// <summary>
        /// リクエストパスがWebsocketに向けたもの(/ws)か否かを判定
        /// </summary>
        /// <param name="context">コンテキスト</param>
        private bool IsWebSocket(HttpContext context)
        {
            bool result = context.Request.Path.StartsWithSegments(new PathString("/ws"));
            return result;
        }


        /// <summary>
        /// ログ出力用のミドルウェアを追加する。
        /// </summary>
        /// <param name="app">ミドルウェアを追加するApplicationBuilder</param>
        /// <param name="inMessage">次のミドルウェアに進む前に出力するログメッセージ</param>
        /// <param name="outMessage">次のミドルウェアから戻ってきた後に出力するログメッセージ</param>
        /// <param name="force">デバッグフラグがついていてもログ出力を強制するか</param>
        private void AddMiddlewareForLogging(IApplicationBuilder app, string inMessage, string outMessage, bool force = false)
        {
            if (isDebug == false && force == false)
            {
                return;
            }
            app.Use(async (context, next) =>
            {
                //force=trueになっているものについては、レスポンスタイムを追記する
                System.Diagnostics.Stopwatch stopwatch = null;
                if (force)
                {
                    stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                }

                if (string.IsNullOrEmpty(inMessage) == false)
                {
                    LogUtil.WritePLog(logger.LogDebug, context, $"{context.Response.StatusCode}:{inMessage}");
                }

                //後続のミドルウェアにつなぐ
                await next();

                if (string.IsNullOrEmpty(outMessage) == false)
                {
                    string message = $"{context.Response.StatusCode}:{outMessage}";
                    if (force)
                    {
                        //実行時間の計測
                        stopwatch.Stop();
                        message += $"({stopwatch.ElapsedMilliseconds}ms)";
                    }
                    LogUtil.WritePLog(logger.LogDebug, context, message);
                }
            });
        }
    }
}
