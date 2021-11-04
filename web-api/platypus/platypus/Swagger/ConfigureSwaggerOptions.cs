using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nssol.Platypus.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;
 
        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
                // APIの署名を記載
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    
                // デフォルトだと同じクラス名の入出力モデルを使えないので、識別に名前空間名も含める
                // https://stackoverflow.com/questions/46071513/swagger-error-conflicting-schemaids-duplicate-schemaids-detected-for-types-a-a
                options.CustomSchemaIds(x => x.FullName);
            }

            //トークン認証用のUIを追加する
            options.AddSecurityDefinition("api_key", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
            });
            options.OperationFilter<AssignJwtSecurityRequirements>();            
        }
 
        /// <summary>
        /// APIバージョン情報定義
        /// </summary>
        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "KAMONOHASHI API",
                Version = description.GroupName,
                Description = "A platform for deep learning",
                Contact = new OpenApiContact()
                {
                    Email = "kamonohashi-support@jp.nssol.nipponsteel.com",
                    Name = "KAMONOHASHI Support"
                },
                // サービス利用規約
                //TermsOfService = new Uri("http://example.com/terms/")

            };
            return info;
        }
    }
}