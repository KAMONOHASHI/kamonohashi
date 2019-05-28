using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nssol.Platypus.ApiModels.VersionApiModels;
using Nssol.Platypus.Controllers.Util;
using Nssol.Platypus.Logic.Interfaces;
using System.Collections.Generic;
using System.Net;

namespace Nssol.Platypus.Controllers.spa
{
    [Route("api/v1/version")]

    public class VersionController : PlatypusApiControllerBase
    {
        private readonly IVersionLogic versionLogic;

        public VersionController(IVersionLogic versionLogic, IHttpContextAccessor accessor) : base(accessor)
        {
            this.versionLogic = versionLogic;
        }

        /// <summary>
        /// バージョン情報を取得
        /// </summary>
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VersionModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {           
            return JsonOK(new VersionModel(versionLogic.GetVersion()));
        }
    }
}
