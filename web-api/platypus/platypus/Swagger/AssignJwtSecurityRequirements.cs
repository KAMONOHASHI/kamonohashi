using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Nssol.Platypus.Swagger
{
    public class AssignJwtSecurityRequirements : IOperationFilter
    {
        /// <summary>
        /// Swagger UI用のフィルタ。
        /// Swagger上でAPIを実行する際のJWTトークン認証対応を実現する。
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            //AllowAnonymousが付いている場合は、アクセスコードを要求しない
            var allowAnonymousAccess = context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AllowAnonymousAttribute));

            if (allowAnonymousAccess == false)
            {
                var oAuthRequirements = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "api_key",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                };

                operation.Security.Add(oAuthRequirements);
            }
        }
    }
}
